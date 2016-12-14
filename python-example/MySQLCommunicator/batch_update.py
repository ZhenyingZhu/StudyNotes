#!/usr/bin/python

import glob
from os.path import getctime
from os.path import expanduser
import re
import traceback
from datetime import datetime
from urllib2 import HTTPError

from crawler import Crawler
from mysql_driver import Question, Metadata, MySQLDriver

SUCCESS = 0
DUPINSERT = 1
HTTPERR = 2
DBERR = 3


def parse_file(cpp_file):
    with open(cpp_file) as file:
        for line in file:
            if '[Source]' in line:
                question_url = re.findall('\[Source\](.*)', line.rstrip())[0].strip()
                if question_url.startswith(':'):
                    print(question_url + " has a colon")
                    return ""
                if 'leetcode' not in question_url:
                    print(question_url + " not a leetcode question")
                    return ""
                if question_url.endswith('problems/'):
                    print(question_url + " missing question name")
                    return ""
                return question_url
            elif line == " */\n":
                break


def update_db(question_url, modify_date, sql_driver):
    # print question_url + " modified at " + str(modify_date)
    if sql_driver.describe_question(question_url):
        # already inserted
        return DUPINSERT

    try:
        crawler = Crawler(question_url)
    except HTTPError:
        return HTTPERR
    except ValueError:
        return HTTPERR

    question = Question(question_url, crawler.get_title(), crawler.get_question_id(), crawler.get_difficulty())
    if not sql_driver.insert_question(question):
        return DBERR
    uid = sql_driver.get_unique_id(question_url)
    metadata = Metadata(uid, 5, modify_date)
    sql_driver.insert_metadata(metadata)
    for tag in crawler.get_tags():
        sql_driver.insert_tag(uid, tag)
    return SUCCESS


def iterator_files(path, sql_driver, failed_files):
    cpp_files = glob.glob(expanduser("~") + path + "*.cpp")
    for cpp_file in cpp_files:
        question_url = parse_file(cpp_file)
        if question_url == "":
            failed_files.append(cpp_file)
            continue
        modify_date = datetime.fromtimestamp(getctime(cpp_file))
        ret = update_db(question_url, modify_date, sql_driver)
        if ret != SUCCESS and ret != DUPINSERT:
            failed_files.append(cpp_file)
            continue
    print "Iterate through " + str(len(cpp_files)) + " files"


def main():
    user = raw_input("username:")
    password = raw_input("password:")
    sql_driver = MySQLDriver(user, password)

    path = "/Github/CppAlgorithms/src/leetcode/"
    failed_files = []
    iterator_files(path, sql_driver, failed_files)
    print failed_files

if __name__ == '__main__':
    main()
