import json
from pprint import pprint

with open('nitro_data.json') as data_file:
    data = json.load(data_file)

# structure
# Level1: b means task? i means label?
# Level2: ip
# Level3:
#  c: title
#  d: priority
#  e: ?
#  h: belongs to which task list
#  j: ?
#  k: ?
#  q: content
#  y: ?

# list: logbook, next, today,

# todo.txt @ means list, which is today or next

pprint(data)
