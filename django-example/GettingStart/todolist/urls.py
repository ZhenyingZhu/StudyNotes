from django.conf.urls import url
from . import views

urlpatterns = [
    url(r'^$', views.index, name='index'),
    url(r'^hello$', views.todo_list),
    url(r'^world$', views.template_inherit)
]
