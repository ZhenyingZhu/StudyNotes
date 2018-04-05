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


class Crawler:
    def __init__(self, first_url, save_path):
        # TODO: Strong coupling between crawler and saving file. Need decouple
        self.first_url = first_url
        self.save_path = save_path

        self.html_page = ""
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

    def get_page_from_url(self, url):
        try:
            hdr = {
                'User-Agent': 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11',
                'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8',
                'Accept-Charset': 'ISO-8859-1,utf-8;q=0.7,*;q=0.3',
                'Accept-Encoding': 'none',
                'Accept-Language': 'en-US,en;q=0.8',
                'Connection': 'keep-alive'}

            req = requests.get(url, headers=hdr)

            self.html_page = req.text

            debug_file = open('html_page', 'w', encoding="utf-8")
            debug_file.write(self.html_page)
            debug_file.close()
        except ValueError:
            raise
			
    def get_page_from_file(self, path):
        # read from html_page which is stored by previous parse
        try:
            debug_file = open(path, 'r')
            self.html_page = debug_file.read()
            debug_file.close()
        except IOError:
            print('failed to read ' + path)
            raise

    def get_title(self):
        try:
            title = re.findall('<title>(.*?)</title>', self.html_page)[0]
            return title
        except IndexError:
            print("Cannot find title")
            return "default title"

    def get_next_page(self):
        hrefs = re.findall('href="(.*?)">', self.html_page)
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

    def get_pic_url(self):
        try:
            pic_urls = re.findall('<img id=\"img\" src=(.*?)/>', self.html_page)
            for pic_url in pic_urls:
                if "style" in pic_url:
                    return re.findall('"(.*?)" style', pic_url)[0]
            print("Cannot find pic")
            return ""
        except IndexError:
            print("Cannot find right pic: " + pic_url)
            return ""

    def download_pic(self, pic_url):
        filename = pic_url.split('/')[-1]
        save_position = os.path.normpath(os.path.join(self.save_path, filename))
        # TODO create folder
        try:
            local_filename, headers = urllib.request.urlretrieve(pic_url)
            shutil.move(local_filename, save_position)
            
            return True
        except IOError as e:
            print(e)
            print("Cannot download pic, skipping...")
            return False
        except KeyboardInterrupt:
            print("Key interrupt")
            return False

    def start(self):
        self.get_page_from_url(self.first_url)
        print(self.get_title())

        next_url = self.first_url
        num = 1
        failed_urls = []
        while next_url != "":
            self.get_page_from_url(next_url)
            pic_url = self.get_pic_url()
            if pic_url:
                print(str(num) + " get " + pic_url + " from " + next_url)
                if not self.download_pic(pic_url):
                    print("Failed to download pic")
                    failed_urls.append(next_url)
                num += 1
            else:
                print("didn't get pic from " + next_url)
                failed_urls.append(next_url)

            next_url = self.get_next_page()
            #time.sleep(0.5)

        # TODO auto retry
        print(failed_urls)


def main():
    # TODO start from an info page, and a list of urls
    my_url = ''
    c = Crawler(my_url, "D:/Downloads/tmp/")
    c.start()

if __name__ == '__main__':
    main()
