#!/usr/bin/python

import MySQLdb
import traceback
from datetime import datetime


class Question:
    def __init__(self, url, name, qid, difficulty):
        self.url = '"' + url + '"'
        self.name = '"' + name + '"'
        self.qid = str(qid)
        self.difficulty = '"' + difficulty + '"'


class Metadata:
    def __init__(self, id, priority, last_touch):
        self.id = str(id)
        self.priority = str(priority)
        self.last_touch = '"' + last_touch.strftime("%Y-%m-%d") + '"'


class MySQLDriver:
    def __init__(self, user, passwd):
        print "Connecting to DB..."
        self.conn = MySQLdb.connect(host="localhost", user=user, passwd=passwd, db="LEETCODEDB")

    def __exit__(self, exc_type, exc_value, traceback):
        # doesn't work
        print "Closing DB..."
        self.conn.close()

    def mutate_sql(self, sql):
        # print sql
        cursor = self.conn.cursor()
        try:
            cursor.execute(sql)
            self.conn.commit()
            return True
        except MySQLdb.Error as e:
            self.conn.rollback()
            print(e[0])
            print(traceback.format_exc())
            return False

    def query_sql(self, sql):
        # print sql
        cursor = self.conn.cursor()
        try:
            cursor.execute(sql)
            return cursor.fetchall()
        except MySQLdb.Error as e:
            print(e[0])
            print(traceback.format_exc())

    def insert_question(self, question):
        values = [question.url, question.name, question.qid, question.difficulty]
        # INSERT INTO Questions (url, name, qid, difficulty) VALUES (url, name, 1, easy)
        sql = 'INSERT INTO Questions (url, name, qid, difficulty) VALUES (' + ', '.join(values) + ')'
        if not self.mutate_sql(sql):
            return False
        return True

    def delete_question(self, url):
        sql = 'DELETE FROM Questions WHERE url="' + url + '"'
        self.mutate_sql(sql)

    def describe_questions(self):
        sql = 'SELECT * FROM Questions'
        for row in self.query_sql(sql):
            print row

    def describe_question(self, url):
        sql = 'SELECT * FROM Questions WHERE url="' + url + '"'
        results = self.query_sql(sql)
        # for result in results:
        #     print result

        if len(results) == 0:
            return False
        return True

    def get_unique_id(self, url):
        sql = 'SELECT id FROM Questions WHERE url="' + url + '"'
        try:
            result = self.query_sql(sql)[0]
            return int(result[0])
        except IndexError:
            print("Cannot find " + url + " in DB")
            return -1

    def insert_metadata(self, metadata):
        values = [metadata.id, metadata.priority, metadata.last_touch]
        sql = 'INSERT INTO Metadata (id, priority, last_touch) VALUES (' + ', '.join(values) + ')'
        self.mutate_sql(sql)

    def delete_metadata(self, uid):
        sql = 'DELETE FROM Metadata WHERE id=' + str(uid)
        self.mutate_sql(sql)

    def insert_tag(self, uid, tag):
        values = [str(uid), '"' + tag + '"']
        sql = 'INSERT INTO Tag (id, tag) VALUES (' + ', '.join(values) + ')'
        self.mutate_sql(sql)

    def delete_tags(self, uid):
        sql = 'DELETE FROM Tag WHERE id=' + str(uid)
        self.mutate_sql(sql)


def main():
    user = raw_input("username:")
    password = raw_input("password:")
    sql_driver = MySQLDriver(user, password)

    q = Question('url', 'name', 1, 'easy')
    sql_driver.insert_question(q)
    sql_driver.describe_questions()
    print sql_driver.describe_question('url')
    print sql_driver.describe_question('url2')
    sql_driver.delete_question('url2')

    uid = sql_driver.get_unique_id('url')
    if uid == -1:
        exit()

    m = Metadata(uid, 5, datetime.now())
    sql_driver.insert_metadata(m)

    sql_driver.insert_tag(uid, 'test')

    sql_driver.delete_metadata(uid)
    sql_driver.delete_tags(uid)
    sql_driver.delete_question('url')

    sql_driver.conn.close()


if __name__ == '__main__':
    main()
