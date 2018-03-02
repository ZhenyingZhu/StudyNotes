from django.shortcuts import render
from django.http import HttpResponse
import datetime


# A basic view.
def index(request):
    message = "当前时间:{}".format(datetime.datetime.now())
    return HttpResponse(message)


def todo_list(request):
    # return render(request, 'base.html')
    return render(request, 'tagTest.html')


def delete(request):
    message = "Delete"
    return HttpResponse(message)


def complete(request):
    message = "complete"
    return HttpResponse(message)


def template_inherit(request):
    return render(request, 'templateInheritTest.html')
