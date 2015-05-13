"""This script imports all images from a local pixeek git repo instance.

1. It copies .jpg images from repo/Common/Content to webapp/private/images.
2. Creates the images table and insert a row for each image.
3. Creates the scores table.

Usage: python2.7 import_images.py <repo_path>

"""

import re
import os
import sys
from shutil import copy
from os import makedirs, listdir
from os.path import join, abspath, dirname, pardir, splitext

sys.path.append(join(abspath(dirname(__file__)), pardir, pardir, pardir))
from gluon import DAL, Field

# Check if repository path was provided.
if len(sys.argv) != 2:
    print 'Provide local pixeek repo instance path as command line arg.'
    sys.exit(-1)
repo_path = sys.argv[1]

# Copy .jpg images from the repo to deployed webapp.
repo_img_dir = join(repo_path, 'Common', 'Content')
wa_img_dir = abspath(join(dirname(__file__), pardir, 'private', 'images'))
try:
    makedirs(wa_img_dir)
except os.error:
    pass   # directory already created
for img in listdir(repo_img_dir):
    if splitext(img)[1] == '.jpg':
        copy(join(repo_img_dir, img), wa_img_dir)

# Connect to database
db_dir = abspath(join(dirname(__file__), pardir, 'databases'))
try:
    makedirs(db_dir)
except os.error:
    pass
db = DAL('sqlite://../databases/storage.db', folder=db_dir)

# Create images table.
db.define_table('images',
    Field('represent', type='string', length=100, required=True),
    Field('file_name', type='string', length=100, required=True))

# Insert image file names and meanings.
img_pattern = re.compile(r'(\D+?)\d*\.')
for img in listdir(wa_img_dir):
    match = img_pattern.match(img)
    if match is not None:
        db.images.insert(
            represent=match.group(1).capitalize(),
            file_name=img)

# Create scores table.
db.define_table('scores',
    Field('mode', type='string', length=6, required=True),
    Field('difficulty', type='string', length=6, required=True),
    Field('name', type='string', length=100, required=True),
    Field('score', type='integer', required=True),
    Field('time_of_acmnt', type='datetime'))

db.commit()