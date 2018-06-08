import csv
import os

with open("FTGGames.csv") as f:
    reader = csv.reader(f)
    #next(reader) # skip header
    data = [r for r in reader]

for r in data:
    pic_id = int(r[0])

    found = False
    pic_file_a = str(pic_id) + 'a.png'
    try:
        os.rename(pic_file_a, 'target/' + pic_file_a)
        found = True
    except:
        continue

    pic_file_b = str(pic_id) + 'b.png'
    try:
        os.rename(pic_file_b, 'target/' + pic_file_b)
        found = True
    except:
        continue

    if not found:
        print(pic_id)
