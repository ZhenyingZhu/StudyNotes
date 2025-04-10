# Django

## Review

- `django-admin startproject mysite`
- `python manage.py runserver`
- `python manage.py startapp polls`
- Write Views for polls app.
- Add the view to URLConf.
- Write models.
- Add the app to mysite.setting.INSTALLED APPS.
- `python manage.py makemigrations polls`
- `python manage.py migrate`
- `python manage.py createsuperuser`: admin, zaq12345
- In poll.admin register the model `admin.site.register(Question)`.
- Create a folder "templates" under the app root and add html templates.
- In views, render templates.
- `python manage.py runserver`

## IDE

[VSCode](http://ruddra.com/2017/08/19/vs-code-for-python-development/)

[VSCode with Django](https://code.visualstudio.com/docs/python/tutorial-django)

[Install VS Code on ubuntu](https://dzone.com/articles/install-visual-studio-code-on-ubuntu-1804)

[workon doesn't work in powershell](https://stackoverflow.com/questions/38944525/workon-command-doesnt-work-in-windows-powershell-to-activate-virtualenv)

```bat
pip install virtualenvwrapper-win
mkvirtualenv venv
cmd /k workon venv
```

Install extention `Python`.

Update VSCode workspace settings

```yaml
"editor.rulers": [
        80,
        120
    ],
"files.exclude": {
        "**/.git": true,
        "**/.svn": true,
        "**/.hg": true,
        "**/CVS": true,
        "**/.DS_Store": true,
        ".vscode": true,
        "**/*.pyc": true
    },
```

Debug Section - Python Django.

Install code auto analysis

```bat
pip install pylint
pip install pylint-django
pip install autopep8
```

Add to workspace setting.

```yaml
    "python.linting.pylintArgs": [
        "--load-plugins=pylint_django"
   ],
```

## Jiuzhang Django from 0 to 1

<http://www.jiuzhang.com/tutorial/django-101/236>

### Books

- Write idiomatic Python
- MDN Document: JavaScript
- Professional JavaScript for Web Developers

### Frameworks and tech stack

Front-end frameworks

- React
- Vue
- Angular

JavaScript version: ES6+

PyCharm + WebStorm

Web framework

- Php: Thinkphp, Laravel
- Python: Django, Flask
- Ruby: rails
- Nodejs: express, koa

Python 3.4+ with virtual env.

### Start Using Django

```bash
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

```bash
django-admin startproject mysite .
python manage.py runserver 0.0.0.0:8000
```

Install pycharm

```bash
sudo snap install pycharm-community --classic
```

Start a new Django app

```bash
python manage.py startapp todolist
```

and add 'todolist' to mysite/settings.py `INSTALLED_APPS`, add 'todolist.urls' to mysite/urls.py.

[Move out the secret key](http://fearofcode.github.io/blog/2013/01/15/how-to-scrub-sensitive-information-from-django-settings-dot-py-files/)

[Django manage static](https://docs.djangoproject.com/en/dev/howto/static-files/#basic-usage)

[Django search path](https://stackoverflow.com/questions/42826048/how-to-load-external-html-into-html-inside-django-template)

Django has `template` dir to put html, and `static` dir to put css, picture, and javascript.

### Template engine

Related to the `TEMPLATES` and `STATIC` in settings.py.

Folder

```bash
mysite
  settings.py
todolist
  src
    pic
      kitten.jpg
```

Add to settings.py: `STATICFILES_DIRS` is for static assets that aren’t tied to a particular app.

```bash
STATIC_URL = '/todolist/src/'

STATICFILES_DIRS = [
    os.path.join(BASE_DIR, 'todolist/src/'),
]
```

It make `http://localhost:8000/todolist/src/pic/kitten.jpg` works.

Then can put pic, font, css, js folders into static folder.

In HTML, head and body both need to add

```html
{% load static %}
<img src="{% static "pic/kitten.jpg" %}" />
```

Template HTMLs can be used to generate other pages.

In body of base.html, add a Django block, which is parsed as python code:

```html
{% block content %}
{% endblock %}
```

Then create another html that inherit from this base.html

```html
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

### HTML

- element: `<html></html>`
- class: `<div class="class1 class2"></div>`
- ID: `<div id="unique-value"></div>`
- link: `<a href="http://somelink.com">Some Link</a>`
- tag: `<b>bold</b>`
- children: `<ul id="parent"><ui id="child"></ui></ul>`
- attribute and content: `<div attr="attr">content</div>`

### CSS

Cascading Style Sheets(CSS)

define `<style>` or `<link rel="stylesheet" href="/static/css/bootstrap.min.css">` in header.

```html
<head>
<style type="text/css">
    span.highlight {color:rgb(0,0,255)}
</style>
</head>
```

class selector

```html
<p class="lfkdsk">Content</p>
.lfkdsk {
    background-color:yellow;
}
```

ID selector

```html
<p id="lfkdsk">Content</p>
#lfkdsk {
    background-color:yellow;
}
```

Tag selector

```html
<p>Content</p>
p {
    background-color:yellow;
}
```

Selector gramma

```css
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

```css
div {
    width: 300px;
    border: 25px solid green;
    padding: 25px;
    margin: 25px;
}
```

[Modal](https://getbootstrap.com/docs/4.0/components/modal/)

### JavaScript

ECMAScript ES6, ES7: define the standard of JavaScript.

primitive types

- Undefined
- Boolean
- Number
- String

Object

- Null

### DOM

In JavaScript

- `window`: properties `innerWidth`, `innerHeight`.
- `screen`: `width`, `height`.
- `location`: dealing with URL. `host`, `port`.
- `navigator`: deal with browser. `navigator.userAgent`.
- `document`: DOM. `document.title`.

In header define

```html
<script>
    lfkdsk = document.getElementById("testid");
    document.write("<p> Got " + testid.innerHTML + "</p>");
</script>
```

Add a node

```html
<script>
    var para = document.createElement("p");
    var node = document.createTextNode("Test paragraph.");
    para.appendChild(node);

    var element = document.getElementById("div1");
    element.appendChild(para); # Or insertBefore
</script>
```

Remove a node

```html
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

### Database

Define classes in models.py.

```python
title = models.CharField(max_length=255)
description = models.TextField(blank=True)
completed = models.BooleanField(default=False)
created_at = models.DateTimeField(auto_now_add=True)
updated_at = models.DateTimeField(auto_now=True)
```

`auto_now_add` is set only on when the entry is created. while `auto_now` is set when every time call `save()`.

Might not be a good idea to use them. [Django auto_now and auto_now_add](https://stackoverflow.com/questions/1737017/django-auto-now-and-auto-now-add)

A default property

```python
id = models.AutoField(primary_key=True)
```

Define an inner class to control the object

```python
class Meta:
    ordering = ('completed', '-updated_at',)
```

If settings.py `INSTALLED_APPS` has this project, then

```bash
python manager.py makemigrations
python manager.py migrate
```

Can let Django update the db..sqlite3.

PyCharm has a DB tool to test connection.

### HTML form

```html
<form action="/complete/{{ todo.id }}/">
    <button class="btn btn-primary" type="submit">Mark as complete</button>
</form>
```

Use bootstrap [input group](https://getbootstrap.com/docs/4.0/components/input-group/)

Add `{% csrf_token %}` to avoid attack.

backend code:

```python
if request.method == 'POST':
    action = request.POST.get('action')
```

`django.shortcuts` methods:

- `get_object_or_404(MyModel, id=object_id)` can either get the object from DB or return 404 page.
- `return HttpResponseRedirect('url')` render that page.

Regular express: `()` is the capture mode. so `^/action/(?P<id>\d+)` matchs to `/action/123` and the view would be

```python
def action(request, id):
    do something
```

Notice `?P<id>` is define what the param it is.

### Extended Read

- Python编程：从入门到实践
- 流畅的 Python
- CSS 设计指南
- Head First JavaScript
- JavaScript 高级编程
- Vue.js

## Official tutorial

<https://docs.djangoproject.com/en/2.0/intro/>

### Overview

DB model:

- create an entry: `myClass = MyClass(field='value')` then `myClass.save()`
- Read from the DB: `MyClass.objects.get(id=myClass.id)` or `MyClass.objects.get(field__startswith='str')`, `field__contains='str'`. get can replace with filter.

Define a foreign key:

```python
class UseAsFK(models.Model):
    ...

class MyClass(models.Model):
    field = models.ForeignKey(UseAsFK, on_delete=models.CASCADE)

k = UseAsFK()
myClass = MyClass(field=k)
myClass.save()
```

The foreign key object has API access to who refer to it.

```python
k.myclass_set.all()
```

Filter vs Get:

- get returns one entry, while filter returns a set

Use admin

```python
from django.contrib import admin
from . import models
admin.site.register(models.Article)
```

URLConf: matching is in-order, and return the python callback when hit the first match.

```python
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

```python
import django
print(django.get_version())
```

Start a project

```bash
django-admin startproject mysite
```

The inner `mysite/` directory is the actual Python package for your project. So can import `mysite.urls`.

`mysite/wsgi.py`: An entry-point for WSGI-compatible web servers

Don’t use this server in anything resembling a production environment.

```bash
python manage.py runserver 0:8000 # ip 0.0.0.0:8000
```

The runserver is auto reloading, but adding files need manually restart it.

Projects vs. apps: A project can contain multiple apps. An app can be in multiple projects.

Create an app call polls:

```bash
python manage.py startapp polls
```

URLconf is a urls.py file. It defines urlpatterns. It needs to be included in `mysite/urls.py` to use.

```python
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
- To use other database: <https://docs.djangoproject.com/en/2.0/topics/install/#database-installation>
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

```python
INSTALLED_APPS = [
    'polls.apps.PollsConfig',
    'django.contrib.admin',
...
]
```

To tell Django that you’ve made some changes to your models, and you’d like the changes to be stored as a migration.

```bash
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

Get a python shell with the Django app environment: `python manage.py shell`

Database APIs:

```python
Question.objects.all() # return Question.__str__()
q = Question(question_text="", pub_date=timezone.now())
q.save()
q.id
q.pub_date
q.question_text="a"
q.save()
Question.objects.filter(question_text__startswith='a') # this call return empty QuerySet when not found.
Question.objects.get(id=2) # throw DoesNotExist
Question.objects.get(pk=1) # pk is primary key
q.choice_set.all() # access the set define Question as foreign key
c = q.choice_set.create(choice_text='b', votes=0)
c.question
Choice.objects.filter(question__pub_date__year=current_year) # double underscore access the property filed and create a filter.
```

`timezone.now()` vs `datetime.datetime.now()`

- Django expects a datetime with tzinfo.
- `timezone` needs `from django.utils import timezone`

Create an admin: `python manage.py createsuperuser`

Login through `localhost/admin/`

To allow admin interact with Question, update `polls/admin.py`

```python
from django.contrib import admin
from .models import Question
admin.site.register(Question)
```

Django handles different model field types (`DateTimeField`, `CharField`) with appropriate HTML input widget.

A view is a “type” of Web page. Each view is represented by a simple Python function.

A route:

```python
path('<int:question_id>/results/', views.results, name='results')
```

Match to

```python
def results(request, question_id):
    response = "You're looking at the results of question %s."
    return HttpResponse(response % question_id)
```

Django search route start from `ROOT_URLCONF`.

Each view is responsible for doing one of two things: returning an HttpResponse object containing the content for the requested page, or raising an exception such as Http404.

Use Django’s template system to separate the design from Python by creating a template that the view can use.

Your project’s TEMPLATES setting describes how Django will load and render templates. The default settings file configures a DjangoTemplates backend whose APP_DIRS option is set to True. By convention DjangoTemplates looks for a “templates” subdirectory in each of the INSTALLED_APPS.

`polls/templates/polls/index.html` can be refer to as polls/index.html. The second `polls` in the path is to namespacing the template. Django cannot distinguish same template files in different apps.

Load a template:

```python
from django.http import HttpResponse
from django.template import loader

from .models import Question

def index(request):
    latest_question_list = Question.objects.order_by('-pub_date')[:5]
    template = loader.get_template('polls/index.html')
    context = {
        'latest_question_list': latest_question_list,
    }
    return HttpResponse(template.render(context, request))
```

A shortcuts of the same logic

```python
def index(request):
    latest_question_list = Question.objects.order_by('-pub_date')[:5]
    context = {'latest_question_list': latest_question_list}
    return render(request, 'polls/index.html', context)
```

Raise an Exception

```python
from django.http import Http404

def detail(request, question_id):
    try:
        question = Question.objects.get(pk=question_id)
    except Question.DoesNotExist:
        raise Http404("Question does not exist")
```

A shortcut:

```python
from django.shortcuts import get_object_or_404, render
question = get_object_or_404(Question, pk=question_id)
```

Same is `get_list_or_404()` function, which use `filter()` instead of `get()`.

Template `polls/templates/polls/detail.html`

```html
<h1>{{ question.question_text }}</h1>
<ul>
{% for choice in question.choice_set.all %}
    <li>{{ choice.choice_text }}</li>
{% endfor %}
</ul>
```

`question.question_text`: Django first search if question is a dictionary and has a key `question_text`. No, then search properties. No, then search if `question` is a list.

Define URL by using `path()`

```python
path('<int:question_id>/', views.detail, name='detail'),
```

Then can change

```html
<li><a href="/polls/{{ question.id }}/">{{ question.question_text }}</a></li>
```

to

```html
<li><a href="{% url 'detail' question.id %}">{{ question.question_text }}</a></li>
```

In URLconf file `polls/urls.py`, add namespace by add `app_name`

```python
app_name = 'polls'
urlpatterns = [
    path('', views.index, name='index'),
    path('<int:question_id>/', views.detail, name='detail'),
    path('<int:question_id>/results/', views.results, name='results'),
    path('<int:question_id>/vote/', views.vote, name='vote'),
]
```

Then in the template file `polls/index.html`

```html
<li><a href="{% url 'polls:detail' question.id %}">{{ question.question_text }}</a></li>
```

<polls/templates/polls/detail.html>

```html
{% if error_message %}<p><strong>{{ error_message }}</strong></p>{% endif %}

<form action="{% url 'polls:vote' question.id %}" method="post">
{% csrf_token %}
{% for choice in question.choice_set.all %}
    <input type="radio" name="choice" id="choice{{ forloop.counter }}" value="{{ choice.id }}" />
    <label for="choice{{ forloop.counter }}">{{ choice.choice_text }}</label><br />
{% endfor %}
<input type="submit" value="Vote" />
</form>
```

Whenever you create a form that alters data server-side, use method="post".

POST forms that are targeted at internal URLs should use the `{% csrf_token %}` template tag, to deal with Cross Site Request Forgeries.

```python
def vote(request, question_id):
    question = get_object_or_404(Question, pk=question_id)
    try:
        selected_choice = question.choice_set.get(pk=request.POST['choice'])
    except (KeyError, Choice.DoesNotExist):
        # Redisplay the question voting form.
        return render(request, 'polls/detail.html', {
            'question': question,
            'error_message': "You didn't select a choice.",
        })
    else:
        selected_choice.votes += 1
        selected_choice.save()
        return HttpResponseRedirect(reverse('polls:results', args=(question.id,)))
```

This piece of code can cause race condition.

Always return an HttpResponseRedirect after successfully dealing with POST data. This prevents data from being posted twice if a user hits the Back button.

`reverse()` craft a URL for a view.

Use generic views: `ListView` and `DetailView`.

URLConf:

```python
app_name = 'polls'
urlpatterns = [
    path('', views.IndexView.as_view(), name='index'),
    path('<int:pk>/', views.DetailView.as_view(), name='detail'),
    path('<int:pk>/results/', views.ResultsView.as_view(), name='results'),
    path('<int:question_id>/vote/', views.vote, name='vote'),
]
```

Views

```python
class IndexView(generic.ListView):
    template_name = 'polls/index.html'
    context_object_name = 'latest_question_list'

    def get_queryset(self):
        return Question.objects.order_by('-pub_date')[:5]


class DetailView(generic.DetailView):
    model = Question
    template_name = 'polls/detail.html'


class ResultsView(generic.DetailView):
    model = Question
    template_name = 'polls/results.html'
```

Write test in app.tests.py

```python
from django.test import TestCase

class TargetTests(TestCase):
    def test_case(self):
        self.assertIs(somelogic, True)
```

Run test

```bash
python manage.py test app
```

- it created a special database for the purpose of testing
- it looked for test methods - ones whose names begin with test

Django test client: it use the existing DB.

```python
setup_test_environment()
client = Client()
response = client.get('/')
response.status_code # 404
```

[Filter by reference count](https://stackoverflow.com/questions/5080366/django-how-to-get-a-queryset-based-on-a-count-of-references-to-foreign-field)

```python
# Player refer to Game

from django.db.models import Count
Games.objects.annotate(num_players=Count('player')).filter(num_players__gt=10)
```

[Selenium](https://www.seleniumhq.org/) can use to automate web browers to perform tests. Use Django `LiveServerTestCase` to work with it.

Static files: `polls/static`.

A list of finders are setted in `STATICFILES_FINDERS`. `AppDirectoriesFinder` looks for `static` subfolders in each `INSTALLED_APPS`.

Use the css file in static folder.

```html
{% load static %}

<link rel="stylesheet" type="text/css" href="{% static 'polls/style.css' %}" />
```

Define how admin sees a schema:
`admin.site.register(Question, QuestionAdmin)`

HERE: <https://docs.djangoproject.com/en/2.0/intro/tutorial07/>
Adding related objects

HERE <https://docs.djangoproject.com/en/2.0/intro/tutorial05/>
