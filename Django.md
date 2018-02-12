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


HERE:
http://www.jiuzhang.com/tutorial/django-101/108