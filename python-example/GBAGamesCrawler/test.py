import csv
import os

exist_game_ids = []
with open(r"D:\Downloads\all.txt") as f:
    content = f.readlines()
    content = [x.strip() for x in content] 
    for line in content:
        exist_game_ids.append(line[:4])

for game_id in range(1, 2820):
    game_id_str = format(game_id, '04d')
    if game_id_str not in exist_game_ids:
        print(game_id_str + ' -')
