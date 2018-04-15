# -*- coding: utf-8 -*-
from api.fetch import SogouAPI

from Queue import Queue
from threading import Thread

import json
import hashlib


class CrawlerThread(Thread):

    def __init__(self, queue):
        # The Message Queue is shared by all threads.
        self.queue = queue
        self.sogou_api = SogouAPI()
        Thread.__init__(self)

    def run(self):
        while True:
            info = self.queue.get()
            m = hashlib.md5()
            m.update(info['url'])
            unique_name = m.hexdigest()
            if 'profile' in info['url']:
                # It is an index page.
                articles = self.sogou_api.fetch_history_urls_from_profile(info['url'])
                f = open(u'content/%s' % unique_name, 'a')
                f.write(json.dumps(articles).encode('utf-8'))
                f.close()

                for article in articles:
                    self.queue.put({'url': article['content_url'],
                                    'title': article['title']})
            else:
                # It is an article page.
                article = self.sogou_api.fetch(info['url'])
                f = open(u'content/%s.html' % unique_name, 'w')
                f.write(article.encode('utf-8'))
                f.close()
            self.queue.task_done()


class Crawler:

    def __init__(self, thread_num):
        self.queue = Queue()
        self.thread_pools = []
        self.sogou_api = SogouAPI()

        for i in range(thread_num):
            # Start all threads. Each thread pull from the Queue when there is something.
            self.thread_pools.append(CrawlerThread(self.queue))
            self.thread_pools[i].start()

    def start(self):
        print 'Start to processing...'
        # Fetch the public info of the account.
        gzh_info = self.sogou_api.fetch_gzh_info(keyword='北美留学生日报')
        # Use the profile page to fetch articles.
        for info in gzh_info:
            self.queue.put({'url': info['profile_url'],
                            'title': info['wechat_id']})
        self.queue.join()
        print 'Finish!'
