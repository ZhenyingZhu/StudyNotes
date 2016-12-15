#!/usr/bin/python

from urllib2 import HTTPError

from crawler import Crawler
from mysql_driver import Question, Metadata

SUCCESS = 0
DUPINSERT = 1
HTTPERR = 2
DBERR = 3
IOERR = 4


def insert_question_from_url(question_url, modify_date, sql_driver):
    # print question_url + " modified at " + str(modify_date)
    if sql_driver.describe_question(question_url):
        # already inserted
        # since companies tags need subscription, cannot add them through url
        return DUPINSERT

    try:
        crawler = Crawler()
        crawler.get_page_from_url(question_url)
        return insert_question_to_db(question_url, modify_date, crawler, sql_driver)
    except HTTPError:
        return HTTPERR
    except ValueError:
        return HTTPERR


def insert_question_from_file(path, question_url, modify_date, sql_driver):
    """
    First insert the question if not exist, otherwise skip, even modify_date is changed
    Then update company list
    Then update similar questions, notice dup can happened quite often at this step
    :param path: absolute file path
    :param question_url: url of the question, string
    :param modify_date: datetime object
    :param sql_driver: the conn to the db
    :return: a code
    """
    crawler = Crawler()
    try:
        crawler.get_page_from_file(path, question_url)
    except IOError:
        return IOERR

    ret = insert_question_to_db(question_url, modify_date, crawler, sql_driver)
    if ret != DUPINSERT and ret != SUCCESS:
        print('Failed to insert question...')
        return ret

    uid = sql_driver.get_unique_id(question_url)
    for company in crawler.get_companies():
        sql_driver.insert_company(uid, company)
    update_similar_questions(uid, crawler.get_similar_questions(), sql_driver)
    return SUCCESS


def insert_question_to_db(question_url, modify_date, crawler, sql_driver):
    if sql_driver.describe_question(question_url):
        # already inserted
        return DUPINSERT

    # insert to Questions, Metadata, and Tag
    question = Question(question_url, crawler.get_title(), crawler.get_question_id(), crawler.get_difficulty())
    if not sql_driver.insert_question(question):
        return DBERR

    uid = sql_driver.get_unique_id(question_url)
    metadata = Metadata(uid, 5, modify_date)
    sql_driver.insert_metadata(metadata)
    for tag in crawler.get_tags():
        sql_driver.insert_tag(uid, tag)
    return SUCCESS


def update_similar_questions(uid, similar_questions, sql_driver):
    # insert low_uid high_uid pair into SimilarRel
    for question_url in similar_questions:
        if not sql_driver.describe_question(question_url):
            print(question_url + " not exist yet, skipping")
        else:
            question_uid = sql_driver.get_unique_id(question_url)
            sql_driver.insert_similar(uid, question_uid)
    return 0