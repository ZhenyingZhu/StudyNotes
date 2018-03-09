from django.shortcuts import render, get_object_or_404, HttpResponseRedirect
from django.http import HttpResponse
import datetime
from .models import Todo

class MyClass:
    def __init__(self, title, field):
        self.title = title
        self.field = field


# A basic view.
def index(request):
    message = "当前时间:{}".format(datetime.datetime.now())
    return HttpResponse(message)


def todo_list(request):
    return render(request, 'tagTest.html', locals())


def delete(request, todo_id):
    todo_item = get_object_or_404(Todo, id=todo_id)
    todo_item.delete()
    return HttpResponseRedirect("/world")


def complete(request, todo_id):
    todo_item = get_object_or_404(Todo, id=todo_id)
    todo_item.completed = True
    todo_item.save()
    return HttpResponseRedirect("/world")


def template_inherit(request):
    if request.method == 'POST':
        if request.POST.get('action') == 'add':
            title = request.POST.get('title')
            Todo.objects.create(title=title)

    myclass_lst = []
    for value in range(0, 1):
        myclass_lst.append(MyClass("my class" + str(value), "value " + str(value)))

    # read from database.
    todo_lst = Todo.objects.all()

    # locals is the local objects.
    return render(request, 'templateInheritTest.html', locals())
