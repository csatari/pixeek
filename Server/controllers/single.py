from base64 import b64encode
from cStringIO import StringIO
from os.path import join, dirname, pardir
from random import seed, randrange, choice

from PIL import Image
from PIL.ImageFilter import GaussianBlur
from PIL.ImageOps import grayscale, invert, posterize

from gluon.contrib.simplejson import dumps


def _random_assignments(sample_size, difficulty):
    num_images = db(db.images).count()
    image_dir = join(dirname(__file__), pardir, 'private', 'images')
    for _ in xrange(sample_size):
        image_rec = db.images[randrange(num_images) + 1]
        image = Image.open(join(image_dir, image_rec.file_name))
        new_image = _transform(image, difficulty)
        jpeg_buffer = StringIO()
        new_image.save(jpeg_buffer, 'JPEG')
        yield dict(word=image_rec.represent,
                   image=b64encode(jpeg_buffer.getvalue()))


def _transform(image, difficulty):
    transformations = ['transpose']
    if difficulty in ('normal', 'hard'):
        transformations.append('blur')
    if difficulty == 'hard':
        transformations.append('color')
    transformation = choice(transformations)
    if transformation == 'transpose':
        direction = choice((
            Image.FLIP_LEFT_RIGHT, Image.FLIP_TOP_BOTTOM, Image.TRANSPOSE,
            Image.ROTATE_90, Image.ROTATE_180, Image.ROTATE_270))
        return image.transpose(direction)
    elif transformation == 'blur':
        return image.filter(GaussianBlur(8))
    else:  # transformation == color
        color_trf_method = choice((
            grayscale, invert, lambda img: posterize(img, 4)))
        return color_trf_method(image)


def new_tile():
    difficulty = request.args[0]
    response.headers['Content-Type'] = 'application/json'
    seed(None)
    return dumps(iter(_random_assignments(1, difficulty)).next())


def new_board():
    difficulty = request.args[0]
    num_tiles = int(request.args[1])
    response.headers['Content-Type'] = 'application/json'
    seed(None)
    return dumps(dict(board=list(_random_assignments(num_tiles, difficulty))))
