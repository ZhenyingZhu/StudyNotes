# Django

http://www.jiuzhang.com/tutorial/django-101/236

## Books
- Write ldiomatic Python
- MDN Document: JavaScript
- Professional JavaScript for Web Developers

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

HTML
- element: `<html></html>`
- class: `<div class="class1 class2"></div>`
- ID: `<div id="unique-value"></div>`
- link: `<a href="http://somelink.com">Some Link</a>`
- tag: `<b>bold</b>`
- children: `<ul id="parent"><ui id="child"></ui></ul>`
- attribute and content: `<div attr="attr">content</div>`

[Django manage static](https://docs.djangoproject.com/en/dev/howto/static-files/#basic-usage)

[Django search path](https://stackoverflow.com/questions/42826048/how-to-load-external-html-into-html-inside-django-template)

Django has `template` dir to put html, and `static` dir to put css, picture, and javascript.

Cascading Style Sheets(CSS)

define `<style>` or `<link rel="stylesheet" href="/static/css/bootstrap.min.css">` in header.

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





HERE:
http://www.jiuzhang.com/tutorial/django-101/108
