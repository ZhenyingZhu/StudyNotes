#!/usr/bin/python

import io
import os
from sys import argv
import pickle
import urllib
import urllib.request
import requests
import shutil
import re
import time

from utils import Utils

class Crawler:
    def __init__(self, first_url, save_path):
        self.first_url = first_url
        self.save_path = save_path

        self.last_url_id = self.get_url_pattern(first_url)
        self.last_url = first_url

    def get_url_pattern(self, url):
        if not '-' in url:
            return "", -1
        idx = url.rfind('-')
        try:
            url_id = int(url[idx + 1:])
        except ValueError:
            url_id = -1
        return url_id
			
    def get_title(self, html_page):
        try:
            title = re.findall('<title>(.*?)</title>', html_page)[0]
            return title
        except IndexError:
            print("Cannot find title")
            return "default title"

    def get_next_page(self, html_page):
        hrefs = re.findall('href="(.*?)">', html_page)
        candidate_hrefs = []
        for href in hrefs:
            id = self.get_url_pattern(href)
            if id == -1:
                continue
            candidate_hrefs.append(href)
            if id == self.last_url_id + 1:
                self.last_url_id = id
                self.last_url = href
                return href
        # So reach the end
        return ""

    def get_pic_url(self, html_page):
        try:
            pic_urls = re.findall('<img id=\"img\" src=(.*?)/>', html_page)
            for pic_url in pic_urls:
                if "style" in pic_url:
                    return re.findall('"(.*?)" style', pic_url)[0]
            print("Cannot find pic")
            return ""
        except IndexError:
            print("Cannot find right pic: " + pic_url)
            return ""

    def start(self):
        utils = Utils()

        html_page = utils.get_page_from_url(self.first_url)
        print(self.get_title(html_page))

        next_url = self.first_url
        num = 1
        failed_urls = []
        while next_url != "":
            html_page = utils.get_page_from_url(next_url)
            pic_url = self.get_pic_url(html_page)
            if pic_url:
                print(str(num) + " get " + pic_url + " from " + next_url)
                if not utils.download_pic(self.save_path, pic_url):
                    failed_urls.append(next_url)
                num += 1
            else:
                print("didn't get pic from " + next_url)
                failed_urls.append(next_url)

            next_url = self.get_next_page(html_page)
            #time.sleep(0.5)

        # TODO auto retries
        print(failed_urls)


def main():
    # TODO start from an info page, and a list of urls
    my_url = ''

    utils = Utils()
    download_path = os.path.join(utils.get_download_path('tmp'))
    c = Crawler(my_url, download_path)
    c.start()

if __name__ == '__main__':
    main()
