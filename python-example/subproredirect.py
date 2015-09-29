#!/usr/bin/env python

import os
import sys
import time
import subprocess

with open('temp.txt', 'w') as out:
    p = subprocess.Popen(['ls', '-al', '/'], stdout=out, stderr=subprocess.PIPE)
    (output, errmsg) = p.communicate()

print "!!out"
print output

print "!!errmsg"
print errmsg

