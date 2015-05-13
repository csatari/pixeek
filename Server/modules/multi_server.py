"""multi_server.py implements the multi-player services of pixeek.

The service is accessible on port 8001, and accepts TCP/IPv4 connections.

"""

import sys
import socket
import asyncore
import asynchat
import logging
import itertools
import collections
from random import seed, choice, sample, randint
from os.path import join, abspath, dirname, pardir

sys.path.append(join(abspath(dirname(__file__)), pardir, pardir, pardir))
from gluon.contrib.simplejson import loads, dumps
from gluon.contrib.simplejson.decoder import JSONDecodeError

import requests
pixeek_rest_uri = 'http://localhost:8000/pixeek/'


def dumps_wo_images(dictionary):
    """JSON serialize dictionary without base64 encoded JPEG images.

    Image blobs are substituted with the string "<image>".  Please note that
    this method modifies dictionary in place.

    """
    to_clean = [dictionary]
    while len(to_clean) > 0:
        to_process = to_clean.pop(0)
        for key in to_process.viewkeys():
            if key == 'image':
                to_process[key] = '<image>'
            elif isinstance(to_process[key], dict):
                to_clean.append(to_process[key])
            elif isinstance(to_process[key], list):
                for list_item in to_process[key]:
                    if isinstance(list_item, dict):
                        to_clean.append(list_item)
    return dumps(dictionary)


class ClientChannel(asynchat.async_chat):
    """ClientChannel is a communication channel with one player.

    It implements both multi-player modes as states, and works as a finite
    state machine, where state transitions are made by socket communication.

    """

    class RegisterState:
        """Read the game attributes and send the new game when becomes paired."""

        @staticmethod
        def process(channel, message):
            """Register client for pairing with messaged attributes."""
            attr = (message['mode'], message['difficulty'], message['layout'])
            channel.name = message['name']
            channel.game_mode = attr[0]
            channel.difficulty = attr[1]
            channel.layout = attr[2]
            dispatcher.register_client(channel.client_address, attr)

    class TimerActState:
        """Wait for the client to solve a task, then generate a new task."""

        @staticmethod
        def process(channel, message):
            """Generate new task, update both players, handle time-out."""
            if message['purpose'] == 'solved_task':
                sri = message['row_index']
                sci = message['col_index']
                new_tile = requests.get(
                    pixeek_rest_uri + 'new-tile/' + channel.difficulty).json()
                sw = channel.board[(sri, sci)]
                if channel.to_find[sw] == 0:
                    channel.logger.error(
                        'Received unassigned word "{}".'.format(sw))
                    channel.logger.debug(
                        'Expecting solutions: {}'.format(channel.to_find))
                    return
                channel.to_find[sw] -= 1
                channel.board[(sri, sci)] = new_tile['word']
                new_task = choice(list(
                    (collections.Counter(channel.board.itervalues()) -
                     channel.to_find).elements()))
                channel.to_find[new_task] += 1

                notification = dict(
                    purpose='update',
                    new_tile=dict(row_index=sri,
                                  col_index=sci,
                                  image=new_tile['image'],
                                  word=new_tile['word']),
                    new_task=new_task)
                channel.push(dumps(notification))
                channel.state = ClientChannel.WaitState
                channel.opponent.push(dumps(notification))
                channel.opponent.state = ClientChannel.TimerActState
                channel.logger.info('Solved a task "{}".'.format(sw))
                channel.logger.debug('New task and image are: {}'.format(
                    dumps_wo_images(notification)))

            else:  # message['purpose'] == 'time_out'
                channel.close_when_done()
                channel.opponent.push(dumps(dict(purpose='you_won')))
                channel.opponent.close_when_done()
                del channel.opponent
                channel.logger.info('The game has ended, opponent won.')

    class WaitState:
        """Do nothing while the client waits for her opponent's turn."""

        @staticmethod
        def process(channel, message):
            """Shouldn't get a message at all."""
            channel.logger.error('Got message while in WaitState.')

    class FightState:
        """Send updates on player's progress to her opponent, handle win."""

        @staticmethod
        def process(channel, message):
            sri = message['row_index']
            sci = message['col_index']
            sw = channel.board[(sri, sci)]
            if channel.to_find[sw] == 0:
                channel.logger.error(
                    'Received unassigned word "{}".'.format(sw))
                channel.logger.debug(
                    'Expecting solutions: {}'.format(channel.to_find))
                return
            channel.to_find[sw] -= 1
            if sum(channel.to_find.itervalues()) > 0:
                channel.opponent.push(dumps(dict(
                    purpose='update',
                    opponent_score=message['score'])))
                channel.logger.info('Reached score {}.'.
                    format(message['score']))
            else:
                channel.state = ClientChannel.WaitState
                channel.opponent.push(dumps(dict(
                    purpose='opponent_finished',
                    opponent_score=message['score'])))
                channel.logger.info('Reached the finish line with score {}.'.
                    format(message['score']))
                if channel.opponent.state is ClientChannel.WaitState:
                    channel.close_when_done()
                    channel.opponent.close_when_done()
                    del channel.opponent
                    channel.logger.info('Cached up with opponent, game over.')

    def __init__(self, sock, client_address):
        """Embed a socket and initialize channel in registration state."""
        asynchat.async_chat.__init__(self, sock=sock)
        self.set_terminator(None)
        self.client_address = client_address
        self.state = ClientChannel.RegisterState
        self._msg_buf = ''

        ip, port = self.client_address
        self.logger = logging.getLogger('.'.join(
            ('pixeek', 'multi', ip.replace('.', '-') + ':' + str(port))))

    def collect_incoming_data(self, data):
        """Buffer incoming data and call state handler if message ended."""
        self.logger.debug('Received bytes: {}'.format(data))
        self._msg_buf += data
        index = self._msg_buf.find('{')
        open_braces = 1
        while index > -1:
            new_index = self._msg_buf.find('}', index + 1)
            open_braces += self._msg_buf.count('{', index + 1, new_index)
            open_braces -= index > -1 and 1 or 0
            index = new_index
            if open_braces == 0:
                try:
                    message = loads(self._msg_buf[: index + 1])
                except JSONDecodeError:
                    self.logger.error('Malformed JSON message: {}'.format(
                        self._msg_buf[: index + 1]))
                else:
                    self.logger.info('Decoded message: {}'.format(message))
                    self.state.process(self, message)
                self._msg_buf = self._msg_buf[index + 1:]
                index = self._msg_buf.find('{')
                open_braces = 1


class MultiServer(asyncore.dispatcher):
    """Accept connections on main port and dispatch to ClientChannels.

    MultiServer queues clients and pairs them based on their game attribute
    preferences.

    """

    _n_to_find = {'easy': 5, 'normal': 10, 'hard': 20}
    _noise_to_find = {'easy': (-1, 1), 'normal': (-3, 3), 'hard': (-5, 5)}
    _edge_len_rectangle = {'easy': 5, 'normal': 9, 'hard': 16}
    _edge_len_diamond = {'easy': 7, 'normal': 9, 'hard': 15}

    def __init__(self, interface, port):
        """Initialize dispatcher."""
        asyncore.dispatcher.__init__(self)

        # Initialize logger
        self.logger = logging.getLogger(
            '.'.join(('pixeek', 'multi', 'dispatcher', str(port))))

        # Bind listening socket
        self.create_socket(socket.AF_INET, socket.SOCK_STREAM)
        self.set_reuse_addr()
        self.bind((interface, port))
        self.listen(5)
        self.logger.info('Successfully bound main socket.')

        # Tables temporarily containing client communication channels
        self._tmp_client_store = dict()
        self._pairing_table = dict()

    def handle_accept(self):
        """Enqueue connecting client and create a communication channel."""
        pair = self.accept()
        if pair is not None:
            sock, address = pair
            self.logger.info(
                'Accepting connection from {}:{}'.format(*address))
            self._tmp_client_store[address] = ClientChannel(sock, address)

    def register_client(self, address, attributes):
        """Register client for pairing with given attributes."""
        client = self._tmp_client_store[address]
        if attributes in self._pairing_table:
            other_client = self._pairing_table[attributes]
            del self._pairing_table[attributes]
            self.logger.info('Paired {} with {}.'.format(
                str(address), str(other_client.client_address)))
            self._start_new_game((client, other_client))
        else:
            self._pairing_table[attributes] = client
            self.logger.info('Registered {} for {}-{}-{}.'.format(
                str(address), *attributes))
        del self._tmp_client_store[address]

    def _start_new_game(self, clients):
        """Start a new multi-player game with paired opponents."""
        # Assemble a new game
        game_mode = clients[0].game_mode
        difficulty = clients[0].difficulty
        layout = self._get_layout(clients[0].layout, difficulty)
        n_active_fields = reduce(lambda acc, tv: acc + (tv and 1 or 0),
                                 layout['active_fields'], 0)
        board = requests.get(pixeek_rest_uri + 'new-board/' + difficulty +
                             '/' + str(n_active_fields)).json()['board']
        index = 0
        for i in xrange(layout['height']):
            for j in xrange(layout['width']):
                if layout['active_fields'][i * layout['width'] + j]:
                    board[index]['row_index'] = i
                    board[index]['col_index'] = j
                    index += 1
        to_find_count = self._n_to_find[difficulty] + \
                        randint(*self._noise_to_find[difficulty])
        to_find = [tile['word'] for tile in sample(board, to_find_count)]
        if game_mode == 'timer':
            beginner_index = randint(0, 1)

        # Notify participants
        game = dict(layout=layout,
                    board=board,
                    to_find=to_find)
        for index in xrange(2):
            game['opponent'] = clients[1 - index].name
            if game_mode == 'timer':
                game['your_turn'] = index == beginner_index
            client = clients[index]
            client.board = {
                (tile['row_index'], tile['col_index']): tile['word']
                for tile in board}
            client.to_find = collections.Counter(word for word in to_find)
            client.opponent = clients[1 - index]
            client.push(dumps(game))
            client.state = \
                game_mode == 'timer' and \
                    (index == beginner_index and
                        ClientChannel.TimerActState or
                        ClientChannel.WaitState) or \
                    ClientChannel.FightState
        self.logger.debug('New game is {}'.format(dumps_wo_images(game)))

    def _get_layout(self, layout_name, difficulty):
        """Generate layout based on name and difficulty."""
        if layout_name == 'rectangle':
            n = self._edge_len_rectangle[difficulty]
            return dict(height=n, width=n, active_fields=[True] * (n * n))
        elif layout_name == 'diamond':
            n = self._edge_len_diamond[difficulty]
            c = (n - 1) / 2
            return dict(height=n, width=n, active_fields=[
                abs(c - i) + abs(c - j) <= c
                for (i, j) in itertools.product(xrange(n), repeat=2)])
        else:  # layout_name == 'fish'
            return dict(height=7, width=16,
                        active_fields=[k == 1 for k in (
                0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
                0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
                0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0)])


if __name__ == '__main__':
    logging.basicConfig(
        format='[%(asctime)s %(levelname)s] %(name)s -> %(message)s',
        datefmt='%m/%d %T',
        level=logging.INFO,
        stream=sys.stderr)
    seed(None)
    dispatcher = MultiServer('', 8001)
    try:
        asyncore.loop(0.1)
    except KeyboardInterrupt:
        del dispatcher