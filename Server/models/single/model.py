db.define_table('images',
    Field('represent', type='string', length=100, required=True),
    Field('file_name', type='string', length=100, required=True))