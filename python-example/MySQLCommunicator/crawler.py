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
            response = urllib2.urlopen(url)
            self.html_page = response.read()
            self.html_lines = self.html_page.split('\n')
            debug_file = open('html_page', 'w')
            debug_file.write(self.html_page)
            debug_file.close()

    def get_title(self):
        title = re.findall('<title>(.*?)</title>', self.html_page)[0]
        # e.g. 'Two Sum | LeetCode OJ', get rid of the tail
        return title[0:-14]

    def get_question_id(self):
        question_id = re.findall('name="question_id" value="(.*?)"', self.html_page)[0]
        return int(question_id)

    def get_difficulty(self):
        difficulty = re.findall('Difficulty: <strong>(.*?)</strong>', self.html_page)[0]
        return difficulty

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
