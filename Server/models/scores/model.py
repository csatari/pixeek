db.define_table('scores',
    Field('mode', type='string', length=6, required=True),
    Field('difficulty', type='string', length=6, required=True),
    Field('name', type='string', length=100, required=True),
    Field('score', type='integer', required=True),
    Field('time_of_acmnt', type='datetime'))