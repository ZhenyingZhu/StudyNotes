from bs4 import BeautifulSoup
import re

with open(r'D:\Downloads\GBA game list.html') as fp:
    soup = BeautifulSoup(fp, 'html.parser')
    
    #print(soup.title.name)
    #print(soup.title.string)
    #font_list = soup.final_all('td')
    font_list = soup.find_all('b')
    for font in font_list:
        if re.match(r'\d\d\d\d - .*', font.text):
            print(font.text)


