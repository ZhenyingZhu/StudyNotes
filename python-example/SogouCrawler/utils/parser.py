# -*- coding: utf-8 -*-
from lxml import etree

import json
import re

article_list_re = re.compile('var msgList = (.*?)}}]};')


class Parser:

    def __init__(self):
        pass

    @staticmethod
    def parse_gzh(text):
        page = etree.HTML(text)
        lis = page.xpath('//ul[@class="news-list2"]/li')
        results = []
        for li in lis:
            url = Parser.get_element(li, 'div/div[1]/a/@href')
            info = Parser.get_elem_text(Parser.get_element(li, 'div/div[2]/p[2]'))
            qrcode = Parser.get_element(li, 'div/div[3]/span/img[1]/@src')
            results.append({
                'profile_url': url,
                'wechat_id': info.replace(u'微信号：', ''),
                'qrcode': qrcode,
            })
            print results
        return results

    @staticmethod
    def get_element(element, sub, contype=None):
        content = element.xpath(sub)
        return Parser.list_or_empty(content, contype)

    @staticmethod
    def get_elem_text(elem):
        return ''.join([node.strip() for node in elem.itertext()])

    @staticmethod
    def list_or_empty(content, contype=None):
        assert isinstance(content, list), 'content is not list: {}'.format(content)

        if content:
            return contype(content[0]) if contype else content[0]
        else:
            if contype:
                if contype == int:
                    return 0
                elif contype == str:
                    return ''
                elif contype == list:
                    return []
                else:
                    raise Exception('only can deal int str list')
            else:
                return ''

    @staticmethod
    def parse_urls_from_profile(content):
        results = []

        articles = article_list_re.findall(content)
        if not articles:
            return []
        articles = articles[0] + '}}]}'
        articles = json.loads(articles)
        for article in articles['list']:
            if str(article['comm_msg_info'].get('type', '')) != '49':
                continue

            comm_msg_info = article['comm_msg_info']
            app_msg_ext_info = article['app_msg_ext_info']
            datetime = comm_msg_info.get('datetime', '')
            type = str(comm_msg_info.get('type', ''))

            results.append({
                'datetime': datetime,
                'type': type,
                'main': 1,
                'title': app_msg_ext_info.get('title', ''),
                'abstract': app_msg_ext_info.get('digest', ''),
                'fileid': app_msg_ext_info.get('fileid', ''),
                'content_url': 'https://mp.weixin.qq.com' + Parser.__replace_str_html(app_msg_ext_info.get('content_url')),
                'source_url': app_msg_ext_info.get('source_url', ''),
                'cover': app_msg_ext_info.get('cover', ''),
                'author': app_msg_ext_info.get('author', ''),
                'copyright_stat': app_msg_ext_info.get('copyright_stat', '')
            })

            if app_msg_ext_info.get('is_multi', 0) == 1:
                for multi_dict in app_msg_ext_info['multi_app_msg_item_list']:
                    results.append({
                        'datetime': datetime,
                        'type': type,
                        'main': 0,
                        'title': multi_dict.get('title', ''),
                        'abstract': multi_dict.get('digest', ''),
                        'fileid': multi_dict.get('fileid', ''),
                        'content_url': 'https://mp.weixin.qq.com' + \
                                       Parser.__replace_str_html(multi_dict.get('content_url')),
                        'source_url': multi_dict.get('source_url', ''),
                        'cover': multi_dict.get('cover', ''),
                        'author': multi_dict.get('author', ''),
                        'copyright_stat': multi_dict.get('copyright_stat', '')
                    })

        return [item for item in results if item['content_url'] != '']

    @staticmethod
    def __replace_str_html(content):
        transfer = [
            ('&#39;', '\''),
            ('&quot;', '"'),
            ('&amp;', '&'),
            ('amp;', ''),
            ('&lt;', '<'),
            ('&gt;', '>'),
            ('&nbsp;', ' '),
            ('\\', '')
        ]
        for item in transfer:
            content = content.replace(item[0], item[1])
        return content


if __name__ == '__main__':
    parser = Parser()


