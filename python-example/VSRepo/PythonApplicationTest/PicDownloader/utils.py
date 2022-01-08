#!/usr/bin/python

import os
import urllib.request
import requests
import shutil
import ssl
from pathlib import Path

class Utils:
    def __init__(self, cookie):
        self.cookie = cookie

    def get_page_from_url(self, url, debug=False):
        try:
            hdr = {
                'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8Accept-Encoding',
                'Accept-Encoding': 'gzip, deflate, br',
                'Accept-Language': 'en-US,en;q=0.5',
                'Cache-Control': 'max-age=0',
                'Connection': 'keep-alive',
                'Cookie': this.cookie,
                'Sec-Fetch-Dest': 'document',
                'Sec-Fetch-Mode': 'navigate',
                'Sec-Fetch-Site': 'same-origin',
                'Sec-Fetch-User': '?1',
                'Upgrade-Insecure-Requests': '1',
                'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:95.0) Gecko/20100101 Firefox/95.0'
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
            Encoding = 'cp850'
            debug_file = open(path, 'r', encoding=Encoding)
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
            # TODO: fix the SSL issue <urlopen error [SSL: CERTIFICATE_VERIFY_FAILED] certificate verify failed: certificate has expired (_ssl.c:1129)>
            if (not os.environ.get('PYTHONHTTPSVERIFY', '') and getattr(ssl, '_create_unverified_context', None)):
                ssl._create_default_https_context = ssl._create_unverified_context

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

    def get_download_path(self, subfolder):
        """Returns the default downloads path for linux or windows"""
        if os.name == 'nt':
            import winreg
            sub_key = r'SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders'
            downloads_guid = '{374DE290-123F-4565-9164-39C4925E467B}'
            with winreg.OpenKey(winreg.HKEY_CURRENT_USER, sub_key) as key:
                location = winreg.QueryValueEx(key, downloads_guid)[0]
            return os.path.join(location, subfolder)
        else:
            return os.path.join(os.path.expanduser('~'), 'downloads', subfolder)


def main():
    utils = Utils("")

    html_page = utils.get_page_from_url("https://www.google.com/")
    print(html_page)

    # Under Windows, the downloads folder could be moved.
    # download_path = os.path.join(Path.home(), "Downloads")
    # download_path = utils.get_download_path()
    # utils.download_pic(download_path, "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png")

if __name__ == "__main__":
    main()