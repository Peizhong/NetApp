import functools
import time

from .config import query_config

def timer(func):
    @functools.wraps(func)
    def clocked(*args, **kwargs):
        t0 = time.time()
        res = func(*args, **kwargs)
        arglist = []
        if args:
            arglist.append(', '.join(repr(s) for s in args))
        if kwargs:
            paris = ['%s=%r' % (k, w) for k, w in sorted(kwargs.items())]
            arglist.append(','.join(paris))
        elapsed = time.time()-t0
        print('%s(%s): %r' % (func.__name__, arglist, elapsed))
        return res
    return clocked


def new_uuid():
    return uuid.uuid4().hex