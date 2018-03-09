from django.conf.urls import url
from . import views

urlpatterns = [
    url(r'^$', views.todo_list),
    url(r'^delete/(\d+)', views.delete),
    url(r'^complete/(?P<todo_id>\d+)', views.complete),

    url(r'^hello$', views.index, name='index'),
    url(r'^world$', views.template_inherit),
]
