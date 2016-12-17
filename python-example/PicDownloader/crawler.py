#!/usr/bin/python

import urllib
import urllib2
import re
import time


def timeout(func, args=(), kwargs={}, timeout_duration=1):
    import signal

    class TimeoutError(Exception):
        pass

    def handler(signum, frame):
        raise TimeoutError()

    # set the timeout handler
    signal.signal(signal.SIGALRM, handler)
    signal.alarm(timeout_duration)
    try:
        func(*args, **kwargs)
        result = False
    except TimeoutError as exc:
        result = True
    finally:
        signal.alarm(0)

    return result


class Crawler:
    def __init__(self, first_url, save_path):
        self.first_url = first_url
        self.save_path = save_path
        if not self.save_path.endswith('/'):
            self.save_path += '/'
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
            response = urllib2.urlopen(url)

            # The problem here is that read() might not get the entire HTTP response
            self.html_page = ""
            while 1:
                data = response.read()
                if not data:
                    last_part = response.read()
                    break
                self.html_page += data
            if not self.html_page.endswith('</html>\n'):
                # maybe it is because memory not enough?
                print('partial read, the last part of the data: ')
                print(last_part)

            debug_file = open('html_page', 'w')
            debug_file.write(self.html_page)
            debug_file.close()
        except urllib2.HTTPError:
            raise
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
        hrefs = re.findall('a href="(.*?)">', self.html_page)
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
            pic_urls = re.findall('<img src=(.*?)/>', self.html_page)
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
        save_position = self.save_path + '/' + filename
        # TODO create folder
        try:
            if timeout(urllib.urlretrieve, args=(pic_url, save_position), timeout_duration=5):
                return False
            return True
        except IOError:
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
        print failed_urls


def main():
    my_url = ''
    c = Crawler(my_url, "/tmp/crawler/")
    c.start()

if __name__ == '__main__':
    main()
