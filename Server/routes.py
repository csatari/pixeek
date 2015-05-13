routes_in = [
    (r'/pixeek/scoreboard/$m/$d', r'/pixeek/scores/scoreboard/$m/$d'),
    (r'/pixeek/register-score/$m/$d', r'/pixeek/scores/register_score/$m/$d'),
    (r'/pixeek/new-tile/$d', r'/pixeek/single/new_tile/$d'),
    (r'/pixeek/new-board/$d/$n', r'/pixeek/single/new_board/$d/$n')]

routes_out = [(y, x) for x, y in routes_in]