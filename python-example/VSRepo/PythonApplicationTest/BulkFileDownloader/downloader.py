import urllib.request
from pathlib import Path

print('Beginning file download with urllib2...')

# ckpplayer
for i in range(1, 255):
    url_pattern = ''
    filename = '' + str(i).zfill(3) + '.ts'
    url = url_pattern + filename
    urllib.request.urlretrieve(url, filename)
