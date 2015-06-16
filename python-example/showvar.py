import os
import sys
import time

time.sleep(float(sys.argv[1]))
sysvar = os.environ.get('SYSVAR')
print sysvar
