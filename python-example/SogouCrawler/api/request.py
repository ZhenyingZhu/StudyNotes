# -*- coding: utf-8 -*-
from collections import OrderedDict
from urllib import urlencode

from constants import (
    SearchArticleType,
    SearchArticleTime,
    HotArticleType
)

_search_gzh = 1
_search_article = 2


class SogouRequest:

    def __init__(self):
        pass

    # 生成抓取首页10篇文章的链接，通过放入一个Dict内，转换成url的形式
    @classmethod
    def generate_search_article_url(cls, keyword, page,
                                    time=SearchArticleTime.ANYTIME, article_type=SearchArticleType.ALL):
        assert isinstance(page, int) and page > 0
        assert time in SearchArticleTime.__dict__.values()
        query = OrderedDict()
        query['type'] = _search_article
        query['page'] = page
        query['ie'] = 'utf-8'
        query['tsn'] = time
        query['query'] = keyword
        if article_type == SearchArticleType.ALL:
            query['interation'] = '%s,%s' % (SearchArticleType.IMAGE, SearchArticleType.VIDEO)
        else:
            query['interation'] = article_type

        return 'http://weixin.sogou.com/weixin?%s' % urlencode(query)

    # 生成搜索公众号主页的链接
    @classmethod
    def generate_search_gzh_url(cls, keyword, page=1):
        assert isinstance(page, int) and page > 0

        query = OrderedDict()
        query['type'] = _search_gzh
        query['page'] = page
        query['ie'] = 'utf8'
        query['query'] = keyword

        return 'http://weixin.sogou.com/weixin?%s' % urlencode(query)

if __name__ == '__main__':
    url = SogouRequest.generate_search_gzh_url('九章算法')
    print url