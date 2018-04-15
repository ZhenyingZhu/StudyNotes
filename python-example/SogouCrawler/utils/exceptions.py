# -*- coding: utf-8 -*-


class SogouCrawlerException(Exception):
    """Crawler Based Exception"""
    pass


class SogouCrawlerVerificationCodeException(SogouCrawlerException):
    """Crawler Verification code Exception"""
    pass


class SogouCrawlerRequestsException(SogouCrawlerException):
    """Crawler Requests Exception"""

    def __init__(self, error_message, response):
        """
        :param error_message: a string, error message
        :param response: requests.models.Response, return of requests
        """
        SogouCrawlerException("%s <url: %s> <content: %s>" % (
            error_message, response.url, response.content))
