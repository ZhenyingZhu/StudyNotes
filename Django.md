# Jiuzhang Django from 0 to 1

http://www.jiuzhang.com/tutorial/django-101/236

## Books
- Write ldiomatic Python
- MDN Document: JavaScript
- Professional JavaScript for Web Developers

## Frameworks and tech stack
Front-end frameworks
- React
- Vue
- Angular

ES6+

PyCharm + WebStorm

Web framework
- Php: Thinkphp, Laravel
- Python: Django, Flask
- Ruby: rails
- Nodejs: express, koa

Python 3.4+ with virtual env.

## Django
Install Django
```
curl -L https://raw.githubusercontent.com/pyenv/pyenv-installer/master/bin/pyenv-installer | bash
# the following to ~/.bashrc:
export PATH="/home/zhu91/.pyenv/bin:$PATH"
eval "$(pyenv init -)"
eval "$(pyenv virtualenv-init -)"
pyenv install 3.4.6
pyenv local 3.4.6

python3 -m venv myvenv
# Windows
myvenv\Scripts\activate
# Linux
source ./myvenv/bin/activate

pip install django==1.8
```

Create Django project
```
django-admin startproject mysite .
python manage.py runserver 0.0.0.0:8000
```

Install pycharm
```
sudo snap install pycharm-community --classic
```

Start a new Django app:
```
python manage.py startapp todolist
```
and add 'todolist' to mysite/settings.py `INSTALLED_APPS`, add 'todolist.urls' to mysite/urls.py.

[Move out the secret key](http://fearofcode.github.io/blog/2013/01/15/how-to-scrub-sensitive-information-from-django-settings-dot-py-files/)

[Django manage static](https://docs.djangoproject.com/en/dev/howto/static-files/#basic-usage)

[Django search path](https://stackoverflow.com/questions/42826048/how-to-load-external-html-into-html-inside-django-template)

Django has `template` dir to put html, and `static` dir to put css, picture, and javascript.

### Template engine
Related to the `TEMPLATES` and `STATIC` in settings.py.

Folder:
```
mysite
  settings.py
todolist
  src
    pic
      kitten.jpg
```

Add to settings.py: `STATICFILES_DIRS` is for static assets that aren’t tied to a particular app.
```
STATIC_URL = '/todolist/src/'

STATICFILES_DIRS = [
    os.path.join(BASE_DIR, 'todolist/src/'),
]
```
It make `http://localhost:8000/todolist/src/pic/kitten.jpg` works.

Then can put pic, font, css, js folders into static folder.

In HTML, head and body both need to add
```
{% load static %}
<img src="{% static "pic/kitten.jpg" %}" />
```


Template HTMLs can be used to generate other pages.

In body of base.html, add a Django block, which is parsed as python code:
```
{% block content %}
{% endblock %}
```

Then create another html that inherit from this base.html
```
{% extends 'base.html' %}
{% block content %}
<table class="table">
  <thread>
    <tr>
	  <th>some entry</th>
	</tr>
  </thread>
  <tbody>
  </tbody>
</table>

{% endblock %}
```

Django Template configuration:
- `BACKEND`
- `DIRS`
- `APP_DIRS`: whether to search templates in APP.

Django Template tags:
- `extends`
- `block`, `endblock`
- `for`, `in`, `endfor`
- `cycle`, `as`
- `filter`, `endfilter`
- `if`, `elif`, `else`, `endif`
- `comment`, `endcomment`
- `debug`

Django Template Filter:
- Variable: `{{ variable }}`, `{{ variable.attr }}`
- Filter: `{{ name|lower }}`, `{{ text|escape|linebreaks }}`

Django template comment: `{# comment #}`


## HTML
- element: `<html></html>`
- class: `<div class="class1 class2"></div>`
- ID: `<div id="unique-value"></div>`
- link: `<a href="http://somelink.com">Some Link</a>`
- tag: `<b>bold</b>`
- children: `<ul id="parent"><ui id="child"></ui></ul>`
- attribute and content: `<div attr="attr">content</div>`

## CSS
Cascading Style Sheets(CSS)

define `<style>` or `<link rel="stylesheet" href="/static/css/bootstrap.min.css">` in header.

```
<head>
<style type="text/css">
    span.highlight {color:rgb(0,0,255)}
</style>
</head>
```

class selector
```
<p class="lfkdsk">Content</p>
.lfkdsk {
	background-color:yellow;
}
```

ID selector
```
<p id="lfkdsk">Content</p>
#lfkdsk { 
	background-color:yellow;
}
```

Tag selector
```
<p>Content</p>
p {
	background-color:yellow;
}
```

Selector gramma
```
p,div { 
}
<!-- select all p and div -->

div p {
}
<!-- all p elements that are in div -->

div > p {
}
<!-- all p elements that have div as parent -->

div + p {
}
<!-- all p that are next to div elements -->
```

Box Model
- margin
- border
- padding
- content: which is the element. Has height, width.

```
div {
    width: 300px;
    border: 25px solid green;
    padding: 25px;
    margin: 25px;
}
```

[Modal](https://getbootstrap.com/docs/4.0/components/modal/)


## JavaScript
ECMAScript ES6, ES7: define the standard of JavaScript.

primitive types
- Undefined
- Boolean
- Number
- String

Object
- Null

## DOM
In JavaScript
- `window`: properties `innerWidth`, `innerHeight`.
- `screen`: `width`, `height`.
- `location`: dealing with URL. `host`, `port`.
- `navigator`: deal with browser. `navigator.userAgent`.
- `document`: DOM. `document.title`.

In header define
```
<script>
    lfkdsk = document.getElementById("testid");
    document.write("<p> Got " + testid.innerHTML + "</p>");
</script>
```

Add a node
```
<script>
    var para = document.createElement("p");
    var node = document.createTextNode("Test paragraph.");
    para.appendChild(node);

    var element = document.getElementById("div1");
    element.appendChild(para); # Or insertBefore
</script>
```

Remove a node
```
<script>
    var element=document.getElementById("div1");
    var p1 = document.getElementById("p1");
    element.removeChild(p1);
</script>
```

Element has
- `innerHTML`: tags are tags.
- `innerText`: tags are also text.
- `textContent`: also include text that is hidden by CSS.
- `style.color`

Everything is an object.

Define a function:
- `function foo() {}`
- `var foo = function() {}`

`myobj.__proto__`

`Object` is a constructor, and it has the property `Object.prototype`.


## Database
Define classes in models.py.
```
title = models.CharField(max_length=255)
description = models.TextField(blank=True)
completed = models.BooleanField(default=False)
created_at = models.DateTimeField(auto_now_add=True)
updated_at = models.DateTimeField(auto_now=True)
```

`auto_now_add` is set only on when the entry is created. while `auto_now` is set when every time call `save()`.

Might not be a good idea to use them. [Django auto_now and auto_now_add](https://stackoverflow.com/questions/1737017/django-auto-now-and-auto-now-add)


A default property
```
id = models.AutoField(primary_key=True)
```

Define an inner class to control the object
```
class Meta:
    ordering = ('completed', '-updated_at',)
```

If settings.py `INSTALLED_APPS` has this project, then 
```
python manager.py makemigrations
python manager.py migrate
```
Can let Django update the db..sqlite3.

PyCharm has a DB tool to test connection.

## HTML form
```
<form action="/complete/{{ todo.id }}/">
    <button class="btn btn-primary" type="submit">Mark as complete</button>
</form>
```

Use bootstrap [input group](https://getbootstrap.com/docs/4.0/components/input-group/)

Add `{% csrf_token %}` to avoid attack.

backend code:
```
if request.method == 'POST':
    action = request.POST.get('action')
```

`django.shortcuts` methods:
- `get_object_or_404(MyModel, id=object_id)` can either get the object from DB or return 404 page.
- `return HttpResponseRedirect('url')` render that page.

Regular express: `()` is the capture mode. so `^/action/(?P<id>\d+)` matchs to `/action/123` and the view would be
```
def action(request, id):
    do something
```
Notice `?P<id>` is define what the param it is.


## Extended Read
- Python编程：从入门到实践
- 流畅的 Python
- CSS 设计指南
- Head First JavaScript
- JavaScript 高级编程
- Vue.js

# Official tutorial

## Overview
DB model:
- create an entry: `myClass = MyClass(field='value')` then `myClass.save()`
- Read from the DB: `MyClass.objects.get(id=myClass.id)` or `MyClass.objects.get(field__startswith='str')`, `field__contains='str'`. get can replace with filter.

Define a foreign key:
```
class UseAsFK(models.Model):
    ...

class MyClass(models.Model):
    field = models.ForeignKey(UseAsFK, on_delete=models.CASCADE)

k = UseAsFK()
myClass = MyClass(field=k)
myClass.save()
```

The foreign key object has API access to who refer to it.
```
k.myclass_set.all()
```

Filter vs Get:
- get returns one entry, while filter returns a set

Use admin
```
from django.contrib import admin
from . import models
admin.site.register(models.Article)
```

URLConf: matching is in-order, and return the python callback when hit the first match.
```
from django.urls import path
from . import views
urlpatterns = [
    path('articles/<int:year>/<int:month>/<int:pk>/', views.article_detail),
]
```
So with a URL: "/articles/2005/05/39323/", a call `views.article_detail(request, year=2005, month=5, pk=39323)` is made.

Views callbacks
- return a HTTPResonse or a HTTPException.

Templates
- Search path is `DIRS`.
- variables: `{{ var }}`. It is python variable.
- template filter is like unix pipe: `{{ article.pub_date|date:"F j, Y" }}`. Here is using [php date format](http://php.net/manual/en/function.date.php)
- inherit: `{% extend "something.html" %}`
- use static files: `{% load static %}`, and then `{% static "images/sitelogo.png" %}"` to refer to a png.

Python includes a lightweight database called SQLite.

Verify installation:
```
import django
print(django.get_version())
```

Start a project
```
django-admin startproject mysite
```

The inner `mysite/` directory is the actual Python package for your project. So can import `mysite.urls`.

`mysite/wsgi.py`: An entry-point for WSGI-compatible web servers

Don’t use this server in anything resembling a production environment.
```
python manage.py runserver 0:8000 # ip 0.0.0.0:8000
```
The runserver is auto reloading, but adding files need manually restart it.

Projects vs. apps: A project can contain multiple apps. An app can be in multiple projects.

Create an app call polls:
```
python manage.py startapp polls
```

URLconf is a urls.py file. It defines urlpatterns. It needs to be included in `mysite/urls.py` to use.
```
urlpatterns = [
    path('polls/', include('polls.urls')),
    path('admin/', admin.site.urls),
]
```

`path` method args:
- route: doesn't search GET and POST parameters.
- view
- kwargs: can be passed in a dictionary to the target view.
- name: Name of the URL.

Setting database
- `mysite/settings.py`
- To use other database: https://docs.djangoproject.com/en/2.0/topics/install/#database-installation
- ENGINE is the db. NAME is tha path.

INSTALLED_APPS contains apps which come with Django:
- django.contrib.admin – The admin site.
- django.contrib.auth – An authentication system.
- django.contrib.contenttypes – A framework for content types.
- django.contrib.sessions – A session framework.
- django.contrib.messages – A messaging framework.
- django.contrib.staticfiles – A framework for managing static files.

Run `python manage.py migrate` to create databases based on the apps.

Django follows the DRY Principle. The goal is to define your data model in one place and automatically derive things from it.

`pub_date = models.DateTimeField('date published')` Now 'date published' is the field name, instead of 'pub_date'.

Django apps are “pluggable”: You can use an app in multiple projects.

The `PollsConfig` class is in the `polls/apps.py` file, so its dotted path is 'polls.apps.PollsConfig'.
```
INSTALLED_APPS = [
    'polls.apps.PollsConfig',
    'django.contrib.admin',
...
]
```

To tell Django that you’ve made some changes to your models, and you’d like the changes to be stored as a migration.
```
python manage.py makemigrations polls
```

The migration file `polls/migrations/0001_initial.py` is human readable.

Run `python manage.py sqlmigrate polls 0001` can see how the SQL would look like, but won't actually execute them.

`python manage.py check;` can check problems.

Table `django_migrations` track migration process.

Summary
1. Change your models (in models.py).
2. Run `python manage.py makemigrations` to create migrations for those changes. The result should commit to git.
3. Run `python manage.py migrate` to apply those changes to the database.


HERE: https://docs.djangoproject.com/en/2.0/intro/tutorial02/
Playing with the API¶
