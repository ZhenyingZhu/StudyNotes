# -*- coding: utf-8 -*-
from constants import (
    SearchArticleTime,
    SearchArticleType
)
from request import SogouRequest
from utils.exceptions import SogouCrawlerRequestsException, SogouCrawlerVerificationCodeException
from utils.parser import Parser
from PIL import Image

# -*- coding: utf-8 -*-
import json
import requests
import tempfile
import time


cache = dict()


class SogouAPI:

    def __init__(self, retries=5):
        self.retries = retries

    def fetch(self, url):
        response = self.__get_and_unlock(
            url,
            unlock_function=self.__unlock_wechat,
            identify_image_callback=self.identify_image_callback_by_hand)
        return response.text

    def fetch_article(self, keyword, page=1,
                      time=SearchArticleTime.ANYTIME,
                      article_type=SearchArticleType.ALL):
        url = SogouRequest.generate_search_article_url(
            keyword, page, time, article_type)
        response = self.__get_and_unlock(
            url,
            unlock_function=self.__unlock_wechat,
            identify_image_callback=self.identify_image_callback_by_hand)
        return Parser.parse_article(response.text)

    # Fetch the public info of the account.
    def fetch_gzh_info(self, keyword):
        url = SogouRequest.generate_search_gzh_url(keyword)
        response = self.__get_and_unlock(url,
                                         self.__unlock_sogou,
                                         self.identify_image_callback_by_hand)
        return Parser.parse_gzh(response.text)

    def fetch_history_urls_from_profile(self, profile_url):
        response = self.__get_and_unlock(profile_url,
                                         unlock_function=self.__unlock_wechat,
                                         identify_image_callback=self.identify_image_callback_by_hand)
        return Parser.parse_urls_from_profile(response.text)

    # 通过识别reeonse的url或者text来判断当前我们的请求是否被禁止
    # 通过unlock_function, 我们手动输入验证码来解锁这个步骤
    def __get_and_unlock(self, url, unlock_function, identify_image_callback):
        session = requests.session()
        response = self.__get(url, session)

        if 'antispider' in response.url or u'请输入验证码' in response.text:
            unlock_function(url, response, session, identify_image_callback)
            response = self.__get(url, session)

        return response

    def __unlock_sogou(self, url, resp, session, identify_image_callback=None):

        millis = int(round(time.time() * 1000))
        r_captcha = session.get('http://weixin.sogou.com/antispider/util/seccode.php?tc=%s' % millis)
        if not r_captcha.ok:
            raise SogouCrawlerRequestsException('SogouAPI get verfication code faild:', resp)

        r_unlock = self.unlock_sogou_callback(url, session, resp, r_captcha.content, identify_image_callback)

        if r_unlock['code'] != 0:
            raise SogouCrawlerVerificationCodeException(
                '[SogouAPI identify image] code: %s, msg: %s' % (r_unlock.get('code'),
                                                                 r_unlock.get('msg')))

    def __unlock_wechat(self, url, resp, session, identify_image_callback=None):
        r_captcha = session.get('https://mp.weixin.qq.com/mp/verifycode?cert=%s' % (time.time() * 1000))
        if not r_captcha.ok:
            raise SogouCrawlerRequestsException('SogouAPI unlock_history get img failed', resp)

        r_unlock = self.unlock_wechat_callback(url, session, resp, r_captcha.content, identify_image_callback)

        if r_unlock['ret'] != 0:
            raise SogouCrawlerVerificationCodeException(
                '[SogouAPI identify image] code: %s, msg: %s, cookie_count: %s' % (
                    r_unlock.get('ret'), r_unlock.get('errmsg'), r_unlock.get('cookie_count')))

    def __get(self, url, session):
        response = session.get(url)

        retries = 0
        while not response.ok and retries < self.retries:
            response = session.get(url)
            retries += 1

        if not response.ok:
            raise SogouCrawlerRequestsException('Get error', response)

        return response

    def show_img(self, content):
        f = tempfile.TemporaryFile()
        f.write(content)
        return Image.open(f)

    def identify_image_callback_by_hand(self, img):
        im = self.show_img(img)
        im.show()
        return raw_input("please input code: ")

    def unlock_sogou_callback(self, url, req, resp, img, identify_image_callback):
        url_quote = url.split('weixin.sogou.com/')[-1]

        unlock_url = 'http://weixin.sogou.com/antispider/thank.php'
        data = {
            'c': identify_image_callback(img),
            'r': '%2F' + url_quote,
            'v': 5
        }
        headers = {
            'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
            'Referer': 'http://weixin.sogou.com/antispider/?from=%2f' + url_quote
        }
        r_unlock = req.post(unlock_url, data, headers=headers)
        if not r_unlock.ok:
            raise SogouCrawlerVerificationCodeException(
                'url: %s unlock[%s] failed: %s' % (unlock_url, r_unlock.text, r_unlock.status_code))

        return r_unlock.json()

    def unlock_wechat_callback(self, url, req, resp, img, identify_image_callback):
        unlock_url = 'https://mp.weixin.qq.com/mp/verifycode'
        data = {
            'cert': time.time() * 1000,
            'input': identify_image_callback(img)
        }
        headers = {
            'Host': 'mp.weixin.qq.com',
            'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
        }
        r_unlock = req.post(unlock_url, data, headers=headers)
        if not r_unlock.ok:
            raise SogouCrawlerVerificationCodeException(
                'unlock[%s] failed: %s[%s]' % (unlock_url, r_unlock.text, r_unlock.status_code))

        return r_unlock.json()


if __name__ == '__main__':
    api = SogouAPI()
    info = api.fetch_gzh_info(keyword='北美留学生日报')

    if 'profile' in info[0]['profile_url']:
        articles = api.fetch_history_urls_from_profile(info[0]['profile_url'])
        f = open(u'../content/%s' % info[0]['wechat_id'], 'a')
        f.write(json.dumps(articles).encode('utf-8'))
        f.close()

        for article in articles:
            print article
