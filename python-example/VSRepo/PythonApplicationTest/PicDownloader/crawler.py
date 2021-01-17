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
    def __init__(self, main_url, save_path):
        self.main_url = main_url
        self.save_path = save_path
        self.failed_urls = []
        self.utils = Utils()

    def get_title(self, html_page):
        # TODO: this is not used any more.
        try:
            title = re.findall('<title>(.*?)</title>', html_page)[0]
            return title
        except IndexError:
            print("Cannot find title")
            return "default title"

    def get_first_title(self, html_page):
        # TODO: combine it with get_second_title.
        try:
            title = re.findall('<h1 id="gn">(.*?)</h1>', html_page)[0]
            return title
        except IndexError:
            print("Cannot find first title")
            return "default first title"

    def get_second_title(self, html_page):
        try:
            title = re.findall('<h1 id="gj">(.*?)</h1>', html_page)[0]
            return title
        except IndexError:
            print("Cannot find second title")
            return ""

    def get_first_page_url(self, main_html_page):
        try:
            # Doesn't work when there is a warning message. In this case pass in "/?nw=always" to the end of the url
            gdtm_class = re.findall('div class="gdtm"(.*?)</a>', main_html_page)[0]
            first_page_url = re.findall('<a href="(.*?)">', gdtm_class)[0]
            return first_page_url
        except IndexError:
            print("Cannot find first page url")
            print(gdtm_class)
            raise

    def get_url_pattern(self, url):
        if not '-' in url:
            return "", -1
        idx = url.rfind('-')
        try:
            url_id = int(url[idx + 1:])
        except ValueError:
            url_id = -1
        return url_id

    def get_next_page(self, html_page):
        # Candidates of next page. For debugging purpose.
        candidate_hrefs = []

        hrefs = re.findall('href="(.*?)">', html_page)
        for href in hrefs:
            id = self.get_url_pattern(href)
            # Not a link for next page.
            if id == -1:
                continue

            candidate_hrefs.append(href)

            if id == self.current_pic_id + 1:
                self.current_pic_id = id
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

    def iterate_all_pages(self, first_url):
        current_url = first_url

        # In case if the first_url is not start from 1.
        self.current_pic_id = self.get_url_pattern(first_url)

        cnt = 1
        while current_url != "":
            html_page = self.utils.get_page_from_url(current_url)
            pic_url = self.get_pic_url(html_page)
            if pic_url:
                print(str(cnt) + " get " + pic_url + " from " + current_url)

                if not self.utils.download_pic(self.save_path, pic_url):
                    self.failed_urls.append(current_url)

                cnt += 1
            else:
                print("didn't get pic from " + current_url)
                self.failed_urls.append(current_url)

            current_url = self.get_next_page(html_page)
            #time.sleep(0.5)

    def parse_main_page(self):
        html_page = self.utils.get_page_from_url(self.main_url)

        lines = [self.get_first_title(html_page) + '\n', self.get_second_title(html_page) + '\n', '\n', self.main_url]

        note_file_path = os.path.join(self.save_path, 'note.txt')
        with open(note_file_path, 'w', encoding='utf-8') as note_file:
            note_file.writelines(lines)

        return self.get_first_page_url(html_page)

    def start(self):
        if not os.path.isdir(self.save_path):
            os.mkdir(self.save_path)

        first_page_url = self.parse_main_page()

        self.iterate_all_pages(first_page_url)

        # TODO add retries.
        print(self.failed_urls)


def main():
    print('Argument List:', str(argv))

    url = argv[1]
    folder = argv[2]

    utils = Utils()
    crawler = Crawler(url, os.path.join(utils.get_download_path(folder)))
    crawler.start()

if __name__ == '__main__':
    main()
