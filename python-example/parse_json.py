#!/usr/bin/env python

try:
    import json
except ImportError:
    import simplejson as json

import tarfile
import tempfile
import os
import sys
import yaml

def read_json(path):
    with open(path) as f:
        return json.load(f)

if __name__ == '__main__': 
    rootdir = '.'
    for root, subFolders, files in os.walk(rootdir): 
        if 'b' in files:
            with open(os.path.join(root, 'b'), 'r') as fin: 
                for lines in fin:
                    print lines

