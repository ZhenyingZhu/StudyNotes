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
        print sql
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

    def insert_company(self, uid, company):
        values = [str(uid), '"' + company + '"']
        sql = 'INSERT INTO Company (id, company) VALUES (' + ', '.join(values) + ')'
        self.mutate_sql(sql)

    def delete_companies(self, uid):
        sql = 'DELETE FROM Company WHERE id=' + str(uid)
        self.mutate_sql(sql)

    def insert_similar(self, uid1, uid2):
        low_id = min(uid1, uid2)
        high_id = max(uid1, uid2)
        values = [str(low_id), str(high_id)]
        sql = 'INSERT INTO SimilarRel (low_id, high_id) VALUES (' + ', '.join(values) + ')'
        self.mutate_sql(sql)

    def delete_similar(self, uid1, uid2):
        low_id = min(uid1, uid2)
        high_id = max(uid1, uid2)
        sql = 'DELETE FROM Company WHERE low_id=' + str(low_id) + ' AND high_id=' + str(high_id)
        self.mutate_sql(sql)

    def update_priority(self, uid, priority):
        sql = 'UPDATE Metadata SET priority=' + str(priority) + ' WHERE id=' + str(uid)
        self.mutate_sql(sql)

    def update_last_touch(self, uid, modify_date):
        last_touch_date = '"' + modify_date.strftime("%Y-%m-%d") + '"'
        sql = 'UPDATE Metadata SET last_touch=' + last_touch_date + ' WHERE id=' + str(uid)
        self.mutate_sql(sql)

    def find_next_not_complete_question(self):
        sql = 'SELECT qid FROM Questions ORDER BY qid'
        qids = self.query_sql(sql)
        for i in range(1, len(qids)):
            prev = int(qids[i - 1][0])
            cur = int(qids[i][0])

            # missing 175-178, 180-185, 192-197, 262, 426-431, 433, 443, 457-458, 
            if cur == 179 or cur == 186 or cur == 198 or cur == 263 or cur == 432 or cur == 434 or cur == 444 or cur == 459:
                continue

            if cur != prev + 1:
                return prev + 1
        return -1


def main():
    user = raw_input("username:")
    password = raw_input("password:")
    sql_driver = MySQLDriver(user, password)

    print(sql_driver.find_next_not_complete_question())

    # q = Question('url', 'name', 1, 'easy')
    # sql_driver.insert_question(q)
    # sql_driver.describe_questions()
    # print sql_driver.describe_question('url')
    # print sql_driver.describe_question('url2')
    # sql_driver.delete_question('url2')
    #
    # uid = sql_driver.get_unique_id('url')
    # if uid == -1:
    #     exit()
    #
    # m = Metadata(uid, 5, datetime.now())
    # sql_driver.insert_metadata(m)
    #
    # sql_driver.insert_tag(uid, 'test')
    #
    # sql_driver.delete_metadata(uid)
    # sql_driver.delete_tags(uid)
    # sql_driver.delete_question('url')

    sql_driver.conn.close()


if __name__ == '__main__':
    main()
