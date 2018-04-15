# -*- coding: utf-8 -*-
from api.fetch import SogouAPI

from Queue import Queue
from threading import Thread

import json
import hashlib


class CrawlerThread(Thread):

    def __init__(self, queue):
        # 创建多线程的共享队列
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
                articles = self.sogou_api.fetch_history_urls_from_profile(info['url'])
                f = open(u'content/%s' % unique_name, 'a')
                f.write(json.dumps(articles).encode('utf-8'))
                f.close()

                for article in articles:
                    self.queue.put({'url': article['content_url'],
                                    'title': article['title']})
            else:
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
            # 在线程池中生成thread_num个多线程，然后依次开启
            self.thread_pools.append(CrawlerThread(self.queue))
            self.thread_pools[i].start()

    def start(self):
        print 'Start to processing...'
        # 先抓取公众要信息，然后通过公众要页面的profile链接去抓取最近的10篇文章
        gzh_info = self.sogou_api.fetch_gzh_info(keyword='九章算法')
        for info in gzh_info:
            self.queue.put({'url': info['profile_url'],
                            'title': info['wechat_id']})
        self.queue.join()
        print 'Finish!'
