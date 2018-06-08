import os
import urllib.request

# f = urllib.request.urlopen("http://stackoverflow.com")
# content = f.read()
# print(content)

def download_pic(pic_name):
    print(pic_name)

    url = 'http://www.tgbus.com/gba/gameinfo/gba/item/screenshot/' + pic_name
    try:
        urllib.request.urlretrieve(url, pic_name)
    except IOError as e:
        print('IO', e) # Content too short or not found

path = r"GBAGames\pic"
pic_list = os.listdir(path)

for pic_id in range(1, 2231):
    pic_name_a = str(pic_id) + 'a.png'
    if pic_name_a not in pic_list:
        download_pic(pic_name_a)

    pic_name_b = str(pic_id) + 'b.png'
    if pic_name_b not in pic_list:
        download_pic(pic_name_b)
