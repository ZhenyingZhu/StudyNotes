"""Poll App Views.
"""

from django.shortcuts import render
from django.http import HttpResponse

def index(request):
    '''Index view.'''
    return HttpResponse("Hello, world. You're at the polls index.")
