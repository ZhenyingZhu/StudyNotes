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



## HERE
http://www.jiuzhang.com/tutorial/django-101/108



# Official tutorial
https://docs.djangoproject.com/en/2.0/intro/tutorial01/
