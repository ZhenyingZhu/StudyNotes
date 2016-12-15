#!/usr/bin/python

import glob
from os.path import getctime
from os.path import expanduser
import re
from datetime import datetime

from mysql_driver import MySQLDriver
from apis import insert_question_from_url

SUCCESS = 0
DUPINSERT = 1
HTTPERR = 2
DBERR = 3
IOERR = 4


def parse_file(cpp_file):
    with open(cpp_file) as file:
        # in the cpp file, there is a comment at the beginning contains url
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


def iterator_files(path, sql_driver, failed_files):
    # if the path is just a cpp file, read this file only
    cpp_files = [path]
    # otherwise it is a folder, read all cpp files in it
    if not path.endswith("cpp"):
        glob.glob(expanduser("~") + path + "*.cpp")
    for cpp_file in cpp_files:
        question_url = parse_file(cpp_file)
        if question_url == "":
            failed_files.append(cpp_file)
            continue
        modify_date = datetime.fromtimestamp(getctime(cpp_file))
        ret = insert_question_from_url(question_url, modify_date, sql_driver)
        if ret != SUCCESS and ret != DUPINSERT:
            failed_files.append(cpp_file)
            continue
    print "Iterate through " + str(len(cpp_files)) + " files"


def main():
    user = raw_input("username:")
    password = raw_input("password:")
    sql_driver = MySQLDriver(user, password)

    #path = "/Github/CppAlgorithms/src/leetcode/"
    path = "/home/zhu91/Github/CppAlgorithms/src/leetcode/NonOverlappingIntervals.cpp"
    failed_files = []
    iterator_files(path, sql_driver, failed_files)
    print failed_files

if __name__ == '__main__':
    main()
