# -*- coding: utf-8 -*-
import requests
import scrapy
# import shutil

class MangaSpider(scrapy.Spider):
    var = str(3 * 6) + 'h'
    name = 'manga'
    allowed_domains = [var + '.animezilla.com']
    # start_urls = ['']

    def start_requests(self):
        urls = ['https://' + self.var + '.animezilla.com/manga/804']
        for url in urls:
            yield scrapy.Request(url=url, callback=self.parse)

    def parse(self, response):
        image_url = str(response.css('#comic::attr("src")').extract()[0])

        headers = {
            "Host": "m.iprox.xyz",
            "Connection": "keep-alive",
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36",
            "Accept": "image/webp,image/apng,image/*,*/*;q=0.8",
            "Referer": "https://" + self.var + ".animezilla.com/manga/",
            "Accept-Encoding": "gzip, deflate, br",
            "Accept-Language": "en-US,en;q=0.9,zh-CN;q=0.8,zh;q=0.7,zh-TW;q=0.6"
        }

        img_response = requests.get(image_url, headers=headers, stream=True)
        if img_response.status_code == 200:
            file_name = "1" if response.url.split("/")[-2] == "manga" else response.url.split("/")[-1]
            full_file_name = str(file_name) + '.jpg'

            with open(full_file_name, 'wb') as out_file:
                # response.raw.decode_content = True
                # shutil.copyfileobj(response.content, out_file)
                for chunk in img_response:
                    out_file.write(chunk)

        next_page = response.css('#page-current a')
        if next_page is not None:
            next_page_url = next_page[-1].attrib['href']
            yield scrapy.Request(next_page_url, callback=self.parse)
