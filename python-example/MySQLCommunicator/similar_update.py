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
from apis import insert_question_from_file

SUCCESS = 0
DUPINSERT = 1
HTTPERR = 2
DBERR = 3
IOERR = 4


def main():
    user = raw_input("username:")
    password = raw_input("password:")
    sql_driver = MySQLDriver(user, password)

    path = "Generalized Abbreviation | LeetCode OJ.html"
    url = 'https://leetcode.com/problems/generalized-abbreviation/'
    insert_question_from_file(path, url, datetime.now(), sql_driver)

if __name__ == '__main__':
    main()
