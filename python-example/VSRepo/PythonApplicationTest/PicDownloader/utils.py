#!/usr/bin/python

import os
import urllib.request
import requests
import shutil
from pathlib import Path

class Utils:
    def get_page_from_url(self, url, debug=False):
        try:
            hdr = {
                'User-Agent': 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11',
                'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8',
                'Accept-Charset': 'ISO-8859-1,utf-8;q=0.7,*;q=0.3',
                'Accept-Encoding': 'none',
                'Accept-Language': 'en-US,en;q=0.8',
                'Connection': 'keep-alive'
            }

            req = requests.get(url, headers=hdr)

            html_page = req.text

            if (debug):
                debug_file = open('html_page', 'w', encoding="utf-8")
                debug_file.write(html_page)
                debug_file.close()

            return html_page
        except ValueError:
            raise

    def get_page_from_file(self, path):
        # Read from the debug_file which is crawled previously. Used for debugging.
        try:
            debug_file = open(path, 'r')
            html_page = debug_file.read()
            debug_file.close()

            return html_page
        except IOError:
            print('failed to read ' + path)
            raise

    def download_pic(self, base_save_path, pic_url):
        filename = pic_url.split('/')[-1]
        save_position = os.path.normpath(os.path.join(base_save_path, filename))
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


def main():
    utils = Utils()

    # html_page = utils.get_page_from_url("https://www.google.com")
    # print(html_page)

    download_path = os.path.join(Path.home(), "Downloads")
    utils.download_pic(download_path, "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png")    

if __name__ == "__main__":
    main()