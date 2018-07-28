import os
import json

def query_config(name: str):
    configpath = 'localconfig.json'
    config = {}
    if os.path.exists(configpath):
        with open(configpath, 'r') as read_f:
            config = json.load(read_f)

    value = config.get(name)
    print('{} is: {}'.format(name, value))
    return value

