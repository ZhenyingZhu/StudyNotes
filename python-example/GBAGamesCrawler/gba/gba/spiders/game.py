# -*- coding: utf-8 -*-
import csv
import re
import scrapy
import urllib.request

class GameSpider(scrapy.Spider):
    name = 'game'
    allowed_domains = ['www.tgbus.com']
    url_prefix = 'http://www.tgbus.com/gba/GameInfo/gba/'
    start_urls = []
    # 7, 68, 70
    for i in [37]:
        start_urls.append(url_prefix + str(i) + '.htm')

    def parse(self, response):
        p = re.compile("游戏类型: (.*)<br>")

        all_tables = response.css('table > tbody > tr > td > table > tbody > tr > td > table > tbody')

        game_table = None
        for table in all_tables:
            if '的GBA游戏截图' in table.extract():
                game_table = table
                break
        
        if game_table == None:
            print('!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!')
            print(response.url)
            return

        game_data = []
        current_row = []
        for td_element in game_table.css('tr > td'):
            # First parse title.
            title_element_color = td_element.css('td > font::attr(color)')
            if title_element_color and title_element_color[0].extract() == 'blue':
                title = td_element.css('td > font::text')[0].extract().strip()
                current_row.append(title)
                continue
            
            # Then is category.
            if '游戏类型' in td_element.extract():
                intro_str = td_element.extract()
                category_search = p.search(intro_str)
                category = ""
                if category_search:
                   category = category_search.group(1)
                else:
                    print(intro_str)
                if category == "":
                    category = "NA"
                current_row.append(category)
                print(current_row)
                game_data.append(current_row)
                current_row = []
                continue
            
            # Then download pictures.
            if td_element.css('td > img'):
                url = td_element.css('td > img::attr(src)')[0].extract()
                try:
                    link = 'http://www.tgbus.com/' + url
                    urllib.request.urlretrieve(link, link.split('/')[-1])
                except urllib.error.HTTPError as e:
                    print('The server couldn\'t fulfill the request.')
                    print('Error code: ', e.code)
                except urllib.error.URLError as e:
                    print('We failed to reach a server.')
                    print('Reason: ', e.reason)
        
        game_data_file = open(response.url.split('/')[-1] + '.csv', 'w')
        with game_data_file:
            writer = csv.writer(game_data_file)
            writer.writerows(game_data)
