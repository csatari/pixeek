from datetime import datetime

from gluon.contrib.simplejson import dumps, load


def scoreboard():
    """Assemble a scoreboard from the top 10 accomplishments of players.

    The required URL format is /scoreboard/mode/difficulty, where
    * mode in {normal, time}, and
    * difficulty in {easy, normal, hard}.

    Returns JSON of the form
    {
    "scoreboard" : [ {
      "player"    : <string> ,
      "score"     : <number> ,
      "timestamp" : <number> } ]
    }

    """
    mode, difficulty = request.args
    query = (db.scores.mode == mode) & (db.scores.difficulty == difficulty)
    rows = db(query).select(orderby=~db.scores.score, limitby=(0, 10))
    results = []
    offset = datetime(2015, 1, 1)
    for row in rows:
        results.append(dict(
            player=row.name,
            score=row.score,
            timestamp=int((row.time_of_acmnt - offset).total_seconds())))
    response.headers['Content-Type'] = 'application/json'
    return dumps(dict(scoreboard=results))


def register_score():
    """Register a player's accomplishment.

    The required URL format is /register-score/mode/difficulty, where
    * mode in {normal, time}, and
    * difficulty in {easy, normal, hard}.

    Expects request body in JSON of the form
    {
    "player" : <string> ,
    "score"  : <number>
    }

    Returns with HTTP(200) if no errors happened.

    """
    mode, difficulty = request.args
    accomplishment = load(request.body)
    db.scores.insert(
        mode=mode,
        difficulty=difficulty,
        name=accomplishment['player'],
        score=accomplishment['score'],
        time_of_acmnt=datetime.now())
    raise HTTP(200, 'Accomplishment saved.')