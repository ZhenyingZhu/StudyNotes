#!/usr/bin/python

import urllib2
import re


class Crawler:
    def __init__(self, url, debug=False):
        self.url = url
        if debug:
            # read from html_page which is stored by previous parse
            debug_file = open('html_page', 'r')
            self.html_page = debug_file.read()
            self.html_lines = self.html_page.split('\n')
            debug_file.close()
        else:
            try:
                response = urllib2.urlopen(url)
                self.html_page = ""
                while 1:
                    data = response.read()
                    if not data:
                        break
                    self.html_page += data
                if not self.html_page.endswith('</html>\n'):
                    # maybe it is because memory not enough?
                    print('partial read')

                self.html_lines = self.html_page.split('\n')
                debug_file = open('html_page', 'w')
                debug_file.write(self.html_page)
                debug_file.close()
            except urllib2.HTTPError:
                raise
            except ValueError:
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
            similar_question_urls.append('https://leetcode.com' + href)
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


def main():
    my_url = 'https://leetcode.com/problems/two-sum/'
    c = Crawler(my_url)
    print(c.get_title())
    print(c.get_question_id())
    print(c.get_difficulty())
    print(c.get_similar_questions())
    print(c.get_tags())

if __name__ == '__main__':
    main()
