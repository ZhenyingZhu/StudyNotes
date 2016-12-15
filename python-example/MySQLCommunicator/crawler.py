#!/usr/bin/python

import urllib2
import re


class Crawler:
    def __init__(self):
        self.url = ""
        self.html_page = ""
        self.html_lines = []

    def get_page_from_url(self, url):
        self.url = url
        try:
            opener = urllib2.build_opener()
            # try to add cookie, doesn't work. Maybe use requests instead
            # opener.addheaders.append(('Cookie', 'name=value'))
            response = opener.open(url)
            # response = urllib2.urlopen(url)

            # The problem here is that read() might not get the entire HTTP response
            last_part = ""
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

            self.html_lines = self.html_page.split('\n')
            debug_file = open('html_page', 'w')
            debug_file.write(self.html_page)
            debug_file.close()
        except urllib2.HTTPError:
            raise
        except ValueError:
            raise

    def get_page_from_file(self, path, url):
        # read from html_page which is stored by previous parse
        self.url = url
        try:
            debug_file = open(path, 'r')
            self.html_page = debug_file.read()
            self.html_lines = self.html_page.split('\n')
            debug_file.close()
        except IOError:
            print('failed to read ' + path)
            raise

    def get_title(self):
        try:
            title = re.findall('<title>(.*?)</title>', self.html_page)[0]
            # e.g. 'Two Sum | LeetCode OJ', get rid of the tail
            return title[0:-14]
        except IndexError:
            return raw_input("title of " + self.url + ": ")

    def get_question_id(self):
        try:
            question_id = re.findall('name="question_id" value="(.*?)"', self.html_page)[0]
            return int(question_id)
        except IndexError:
            return raw_input("question id of " + self.url + ": ")

    def get_difficulty(self):
        try:
            difficulty = re.findall('Difficulty: <strong>(.*?)</strong>', self.html_page)[0]
            return difficulty
        except IndexError:
            return raw_input("difficulty of " + self.url + ": ")

    def get_similar_questions(self):
        line_buffer = ""

        start = False
        for line in self.html_lines:
            if re.findall('<div id="similar"', line):
                start = True
            elif start and re.findall('/div', line):
                start = False
            elif start:
                line_buffer += line

        similar_question_urls = []
        for href in re.findall('href="(.*?)">', line_buffer):
            if not href.startswith('https://leetcode.com'):
                href = 'https://leetcode.com' + href
            similar_question_urls.append(href)
        return similar_question_urls

    def get_tags(self):
        line_buffer = ""

        start = False
        for line in self.html_lines:
            if re.findall('<div id="tags"', line):
                start = True
            elif start and re.findall('/div', line):
                start = False
            elif start:
                line_buffer += line

        tags = []
        for href in re.findall('/">(.*?)</a>', line_buffer):
            tags.append(href)
        return tags

    def get_companies(self):
        line_buffer = ""

        start = False
        for line in self.html_lines:
            if re.findall('<div id="company_tags"', line):
                start = True
            elif start and re.findall('/div', line):
                start = False
            elif start:
                line_buffer += line

        tags = []
        for href in re.findall('/">(.*?)</a>', line_buffer):
            tags.append(href)
        return tags


def main():
    my_url = 'https://leetcode.com/problems/two-sum/'
    # my_url = 'https://leetcode.com/problems/generalized-abbreviation/'  # need subscription
    c = Crawler()
    c.get_page_from_url(my_url)
    # c.get_page_from_file('Generalized Abbreviation | LeetCode OJ.html', my_url)
    print(c.get_title())
    print(c.get_question_id())
    print(c.get_difficulty())
    print(c.get_similar_questions())
    print(c.get_tags())

if __name__ == '__main__':
    main()
