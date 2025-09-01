# Python

## Python核心编程

### Chapter 01

源文件.py，编译文件.pyc或.pyo。

命令行启动python的选项:
-d: 提供调试输出
-O: 生成优化的字节码(生成 .pyo 文件)
-S: 不导入 site 模块以在启动时查找 Python 路径
-v: 冗余输出(导入语句详细追踪)
-m mod: 将一个模块以脚本形式运行
-Q opt: 除法选项?
-c cmd: 运行以命令行字符串形式提交的 Python 脚本
file: 从给定的文件运行 Python 脚本

执行脚本: python script.py,或在脚本第一行加入sh-bang:
`#!/usr/local/bin/python`
指定执行脚本的程序。

保持脚本运行后不关闭: `raw_input()`。

各种语言介绍: (Python核心编程P25)

栈帧: 用stackless的解释器来避开C 函数调用的限制。(Python核心编程P29)

### Chapter 02

语句用关键字组成命令。
`print 'Hello World'`
表达式没有关键字，可以用运算符或括号调用的函数。
`abs(-4)`
执行后可以有输出，可以没有(返回None)。

`_`表示前一个表达式的值。

变量赋值: `mystring = 'Hello World!'`

用print显示字符串,结果没有引号,调用了str()函数。
`print mystring`
直接用变量名，为区分其他类型变量，字符串型变量有单引号，调用了repr()函数。
`mystring # 'apple'`

print语句：用%d表示整数，%s表示字符串，%f表示浮点数。

```python
who = 'knights'
what = 'Ni!'
print 'We are the %s who say %s' % (who, ((what + ' ') * 4))
```

得到：We are the knights who say Ni! Ni! Ni! Ni!

在print 语句最后加`,`，可以取消换行。

`print "%s is number %d" % ("Python", 1)`
可以用`>>`重导向导出到屏幕上作为系统报错

```python
import sys
print >> sys.stderr, 'Error'
```

或导出到文件，这里是增加到文本末尾。当关闭文件后才会写入。

```python
logfile = open('/tmp/mylog.txt', 'a')
print >> logfile, 'Error'
logfile.close()
```

输出多字符串：,自带空格，+不带。

```python
print a, b
print a + b
```

键盘输入：

```python
num = raw_input('Please input a number: ')
if num.isdigit():
    print 'double the number is %d' % (int(num) * 2)
```

帮助：`help(func)`
注释：`#`到行尾。
在线文档：在函数定义起始的字符串。

```python
def foo():
    "This is a function"
    return True
```

### 运算符

`+`, `-`, `\*`, `//`整形除(当其中有浮点数时仍下取整), `\*\*`指数, `/`浮点除(但两数均为整型时仍为整型除), `%`。
几乎所有数据类型都可以用+运算符。数组和字符串为串接操作。
`<`, `<=`, `>`, `>=`, `==`, `!=`, `<>`等同于前者。支持`3<4<5`的操作，用and连接。
and, or, not。
变量名：大小写敏感，字母开始。
动态类型语言：不需要先声明。
不支持自增++，支持增量赋值 `n \*= 10`
数字：int(0x80, -0X92), long(-20L, 0xAEL), bool(True = 1), float(-6.0e23, 1.5E-19), complex(0+1j, -1.23-875J)。导入后可用decimal类型，用以处理十进制小数的误差（outdate）。
Python 支持使用成对的单引号或双引号，三引号（三个连续的单引号或者双引号）可以用来包含特殊字符。
第一个字符的索引是0，最后一个字符的索引是-1。字符串连接运算+，字符串重复`*`。
使用[ ]和[:]可以得到子字符串。

```python
pystr = 'python'
pystr[0] # 'p'
pystr[:2] # 'py'
'-' * 20 # '------------'
pystr = '''python
...is cool''' # 'python\nis cool'
print pystr # python
is cool
```

### 数组

可以混合储存任何类型。
List用`[]`创建，可以通过下标重新赋值。
Tuple用()表示，不可以重新赋值。

```python
aList = [1, 2, 3, 4] # Create a list
aList[1] = 5

aTuple = ('apple'， 2， 3) # cannot do aTuple[1] = 5
```

元组可用加法相连。

```python
tuple = (for ele in list,) # without ',' it is list element not tuple
for ele in list:
    tmp = (ele,)
    tuple += tmp
```

Tuple can use for several varables.

```python
def change(a, b): 
    a = 5
    b = 4
    return a, b # return a tuple

c, d = change(a, b) # get value from tuple
```

### 字典

键值对用{}创建。

```python
aDict = {'host': 'earth'}
aDict['port'] = 80 # Add a tuple
aDict.keys()
for key in aDict: 
print key, aDict[key]
```

### 条件

```python
if x < .0: print 'x is smaller than 0'
elif x < 1: print 'x is smaller than 1'
else: print 'x is larger than 1'
```

### 循环

While:

```python
counter = 0
while counter < 3:
print 'loop #%d' % (counter)
counter += 1
```

Iterator For:

```python
for item in ['email', 'phone', 'work']:
print item,
```

Numerical For:

```python
str = 'abc'
for i in range(len(str)): 
print '(%d)' % i, str[i], 
```

Enumerate:

```python
for i, ch in enumerate(str):
print i, ch
```

实际上该函数返回的是一个元组(idx, element)集合。
用循环创建列表：

```python
sqdEven = [x ** 2 for x in range(8) if not x % 2]
```

`range([start, ]stop[, step])`：起始于start，结束于step - 1。默认start 为0，step 为1。

### 读取文件

```python
handle = open("file_name", 'a')
handle.readline() # Output shows '\n'
for eachLine in handle:
    print eachLine, # Output change lines automatically
    handle.close()
```

access_mode 默认为r，还有w, a为添加，U为读写，b为用二进制读取（过时）。
注意当有同名文件时，w会覆盖。
Attribute 可以是数据，也可以是方法。

### 处理异常

try-except块：

```python
try:
    filename = raw_input("Enter a filename: ")
    fobj = open(filename, 'r')
    for eachLine in fobj:
        print eachLine,
        fobj.close()
except IOError, e: # instance an IOError class e
    print "Error: ", e
```

或使用raise。

### 函数

必须在调用前定义。
Python 通过引用调用函数，对参数的改动会在函数外起作用。

```python
def add2Me(x = 1): # default value is 1
    "Add myself twice"
    return x + x
```

函数名中的括号一定需要。
无return语句时返回None 对象。
调用时，如果不带括号和参数时，显示函数来源。

### 类

没有定义基类的话，就使用object类。

```python
class FooClass(object):
    """my very first class: FooClass"""
    version = 0.1 # class (data) attribute
    def __init__(self, nm='John Doe'):
    """constructor"""
    self.name = nm # class instance (data) attribute
    print 'Created a class instance for', nm

def showname(self):
    """display instance attribute and class name"""
    print 'Your name is', self.name
    print 'My name is', self.__class__.__name__ # Show the class name

def showver(self):
    """display class(static) attribute"""
    print self.version # references FooClass.version

def addMe2Me(self, x): # does not use 'self'
    """apply + operation to argument"""
    return x + x
```

由`__`开头和结尾的方法是特殊方法或属性。
`__init__()`为构造函数，但是Python 中并不由该函数实例化，只是实例化后自动执行的动作。
self变量为类中方法必须的参数。
实例化：

```python
fool = FooClass('Jane')
fool.showname() # your name is Jane, My name is FooClass
```

### 模块

Module 就是module.py 这个文件里的组织形式。可import。
当import 一个脚本的时候，这个脚本的可执行部分就会自动执行。

```python
import sys

sys.stdout.write("Hello World\n")
sys.platform
sys.version
```

write()函数不会自动换行。
PEP(Python Enhancement Proposal)： 用以增加新特性。[PEP](http://python.org/dev/peps)。

```python
if __name__ == "__main__":
main()
```

Is a way to run the script with self defined function

### 内建函数

`dir(obj)`：显示对象的属性。不提供参数则显示全局对象名称。
`help(ojb)`：显示帮助。
`int(obj)`：转换为整型。
`str(obj)`：转换为字符串。
`len(obj)`：得到长度。
`type(obj)`：返回对象类型。返回值为type 类的实例。
`obj.__doc__`：显示帮助文档。

## 九章算法

### Class

Attributes: `hasattr`, `getattr`, `setattr`.

`_xxx`: protected. `__xxx`: private. `__xx__`: special methods.

`__init__`, `__doc__`, `__dict__`, `__name__`, `__str__`, `__add__`

`isinstance(obj, type)`: check if obj is an obj of type or a child type.

### Except

```python
try:
except:
else:
```

- ValueError
- IOError
- RuntimeError
- TypeError
- NameError

## [Web Spider](http://blog.csdn.net/column/details/why-bug.html)

### Introduction

Universal Resource Identifier URL是URI的一个子集。它是Uniform Resource Locator的缩写
URL format: `protocol :// hostname[:port] / path / [;parameters][?query]#fragment`

### urllib2

Get html stream:

```python
import urllib2
response = urllib2.urlopen('http://www.baidu.com/')
html = response.read()
print html
```

用你要请求的地址创建一个Request对象, 通过调用urlopen并传入Request对象，将返回一个相关请求response对象.

Interact with HTML:

- 发送一些数据到URL(通常URL与CGI`[通用网关接口]`脚本，或其他WEB应用程序挂接)。在HTTP中,这个经常使用熟知的POST请求发送。
- 一般的HTML表单，data需要编码成标准形式。然后做为data参数传到Request对象。

```python
values = {'name' : 'WHY',
'language' : 'Python' }
data = urllib2.urlencode(values)

# POST
req = urllib2.Request(url, data)
response = urllib2.urlopen(req)

# GET
data = urllib2.open(url + '?' + data)
```

GET和POST请求的不同之处是POST请求通常有"副作用"，它们会由于某种途径改变系统状态(例如提交成堆垃圾到你的门口)。
Data同样可以通过在Get请求的URL本身上面编码来传送。

有一些站点不喜欢被程序（非人为访问）访问，或者发送不同版本的内容到不同的浏览器。默认的urllib2把自己作为“Python-urllib/x.y”(x和y是Python主版本和次版本号,例如Python-urllib/2.7), 这个身份可能会让站点迷惑，或者干脆不工作。浏览器确认自己身份是通过User-Agent头，当你创建了一个请求对象，你可以给他一个包含头数据的字典。

### ERRORs

- URLError
- HTTPError: a subset of URLError.
- HTTP Code 401 means unauthorized. content in header is `Www-authenticate: SCHEME realm="REALM"`. Can instance a `HTTPBasicAuthHandler` to deal with it.

服务器上每一个HTTP 应答对象response包含一个数字"状态码"。
有时状态码指出服务器无法完成请求。默认的处理器会为你处理一部分这种应答。例如:假如response是一个"重定向"，需要客户端从别的地址获取文档，urllib2将为你处理。其他不能处理的，urlopen会产生一个HTTPError。典型的错误包含"404"(页面无法找到)，"403"(请求禁止)，和"401"(带验证请求)。

- 200：请求成功. 处理方式：获得响应的内容，进行处理
- 201：请求完成，结果是创建了新资源。新创建资源的URI可在响应的实体中得到. 处理方式：爬虫中不会遇到
- 202：请求被接受，但处理尚未完成. 处理方式：阻塞等待
- 204：服务器端已经实现了请求，但是没有返回新的信息。如果客户是用户代理，则无须为此更新自身的文档视图。处理方式：丢弃
- 300：该状态码不被HTTP/1.0的应用程序直接使用，只是作为3XX类型回应的默认解释。存在多个可用的被请求资源。处理方式：若程序中能够处理，则进行进一步处理，如果程序中不能处理，则丢弃
- 301：请求到的资源都会分配一个永久的URL，这样就可以在将来通过该URL来访问此资源. 处理方式：重定向到分配的URL
- 302：请求到的资源在一个不同的URL处临时保存. 处理方式：重定向到临时的URL
- 304 请求的资源未更新. 处理方式：丢弃
- 400 非法请求. 处理方式：丢弃
- 401 未授权. 处理方式：丢弃
- 403 禁止. 处理方式：丢弃
- 404 没有找到. 处理方式：丢弃
- 5XX 回应代码以“5”开头的状态码表示服务器端发现自己出现错误，不能继续执行请求. 处理方式：丢弃

`BaseHTTPServer.BaseHTTPRequestHandler.response`是一个很有用的应答号码字典，显示了HTTP协议使用的所有的应答号。当一个错误号产生后，服务器返回一个HTTP错误号，和一个错误页面。可以使用`HTTPError`实例作为页面返回的应答对象response。这表示和错误属性一样，它同样包含了read,geturl,和info方法。

```python
from urllib2 import Request, urlopen, URLError, HTTPError 
req = Request('http://bbs.csdn.net/callmewhy') 
try: 
response = urlopen(req) 
except HTTPError, e: 
print 'The server couldn\'t fulfill the request.' 
print 'Error code: ', e.code 
except URLError, e: 
print 'We failed to reach a server.' print 'Reason: ', e.reason 
else: 
print 'No exception was raised.' # everything is fine 
```

Get the true URL:

```python
req = Request(shorten_url) 
response = urlopen(req)
real_url = response.geturl()
```

Get the page header:

```python
response.info()
```

[How to Authorize](http://blog.csdn.net/pleasecallmewhy/article/details/8924889)

[Stop here](http://blog.csdn.net/pleasecallmewhy/article/details/8925978)

## Queue

encap concurrent condition behaviour.

```python
queue = Queue(size)
queue.put(item)# will wait until the queue size is less than the max size
item = queue.get()# will wait until the queue is not empty
queue.task_done()# inform that the get op is done
```

## Other tips

Python import path:

```python
import os
from os.path import *
import sys 

# currently in base/test/ need import base/src
src_path = os.path.join(dirname(dirname(realpath(__file__))), 'src')
sys.path.append(src_path)
```

[Python simulate web browser](http://stackoverflow.com/questions/14516590/how-to-save-complete-webpage-not-just-basic-html-using-python)

## os environment

`os.putenv` actually set a copy of sys env. So that if it call a script, the script has that env. But the main script doesn't have that env.

Can do a deep copy like this: `env = copy.deepcopy(os.environ)`

## inspect

To see all the members of an instance

```python
import inspect
from pprint import pprint

pprint(inspect.getmembers(my_instance))
```

## Star operator

`*` unpack a seq/collection to make it fit arguments:

```python
def sum(a, b):
return a + b

values = (1, 2)
return sum(*values)
```

`**` unpack a dict

```python
def sum(a, b, c, d):
return a + b + c + d

values1 = (1, 2)
values2 = { 'c': 10, 'd': 15 }
s = sum(*values1, **values2)
```

will execute as:

```python
s = sum(1, 2, c=10, d=15)
```

In arg list, `*args` means it accept arbitrary number of arguments, `**args` accepts dict

```python
def foo(*args):
for a in args:
print a
```

```python
def bar(**kwargs):
for a in kwargs:
print a, kwargs[a]

bar(name='one', age=27)
```

## Unicode does not work

```python
u.topic.encode('ascii','ignore').startswith("something")
```

## swap two elements in a list

```python
lst = [1,2,3]
lst[0], lst[2] = lst[2], lst[0]
```

## Element tree

[API](https://docs.python.org/2/library/xml.etree.elementtree.html)

Notice there is a namespace when search for element.

## API to modify PDF

<https://www.binpress.com/tutorial/manipulating-pdfs-with-python/167>

## UPS Crawler

<https://stackoverflow.com/questions/5879953/tracking-api-for-fedex-and-ups>

<https://stackoverflow.com/questions/46614788/python-ups-xml-tracking>

<https://www.ups.com/upsdeveloperkit?loc>

## Setting python on windows

<https://docs.python.org/3/using/windows.html>

```cmd
PATH=C:\Python 3.6;C:\Python 3.6\Scripts\
PYTHONPATH=C:\Python 3.6;C:\Python 3.6\Lib
```

If python is installed in "Program files", need change the permission of python folder to be write and readable by all users.

## Python import

import from local path??

## Python CSV coding

```python
try:
with open(path, 'r') as csv_file:
dict_reader = csv.DictReader(csv_file)
for entry in dict_reader:
print(entry['column_name'])

except FileNotFoundError:
print("File not found.")
```

## Scrapy Crawler

<https://medium.com/python-pandemonium/develop-your-first-web-crawler-in-python-scrapy-6b2ee4baf954>

<https://wiki.python.org/moin/WindowsCompilers>

- `pip install scrapy`

- Install VS C++.
- `pip install pypiwin32`

- `scrapy startproject olx`
- `scrapy genspider electronics www.olx.com.pk`
- `scrapy crawl electronics`

- update electronics.py
- update items.py with an OlxItem model.
- `scrapy crawl electronics -o data.csv -t csv`

Rule: if set follow to True, it will continue to run until the rule is no mean.

To test

```python
scrapy shell [url]
response.css('h1::text').extract()[0].strip()
```

To store with different JSON files: change setting.py

```python
FEED_URI = 'data/%(name)s/%(time)s.json' # name: crawler name. time: timestamp.
FEED_FORMAT = 'json'
```

Then run `scrapy crawl electronics`.

## CSV

[Merge CSV files](https://stackoverflow.com/questions/2512386/how-to-merge-200-csv-files-in-python)

[python move file](https://stackoverflow.com/questions/8858008/how-to-move-a-file-in-python)

## Datetime

strptime with microsecond can only deal with 6 decimals.

```python
datetime.strptime("2018-12-06 04:35:31.6808615"[:-1], "%Y-%m-%d %H:%M:%S.%f")
```

## Virtual Env

[venv](https://docs.python.org/3/library/venv.html)

- `python -m venv` or `python -m venv <envname>`

In vscode: [creating environments](https://code.visualstudio.com/docs/python/environments)

- Need to use latest version of VS code

[src](https://packaging.python.org/guides/installing-using-pip-and-virtual-environments/)

- Install: `py -m pip install --user virtualenv`
- Add the scripts path to `$PATH`. On windows its `%USERPROFILE%\AppData\Roaming\Python\Python37\Scripts`.
- Avtivate: `.\env\Scripts\activate`

pyenv-virtualenv: <https://github.com/pyenv/pyenv-virtualenv>

For windows: [Install virtualenv and virtualenvwrapper](https://docs.djangoproject.com/en/2.0/howto/windows/)

```python
pip install virtualenvwrapper-win
virtualenv -p "D:\Python3\python.exe" venv
workon venv
```

### Debug with VSCode

- Use `ctrl+shift+p` to open the command palette. Select the python interpreter.
- Install python debugger extention.
- Create a launch.json file.

### install second python

<https://github.com/yyuu/pyenv>

- `pyenv install 2.7.8` to install a new python
- `pyenv versions` to find all versions
- `pyenv local 2.7.8` to change local python version

The main python install libs under `/usr/lib/python2.7`

`pyenv` installed everything under `~/.pyenv/versions/2.7.8/lib/`

To make in python CLI, the move keys can actually move, [install libreadline](http://stackoverflow.com/questions/10765441/python3-2-can-not-recognize-up-down-left-right-keys-in-interpreter)

### Pipfile

<https://pipenv.pypa.io/en/stable/>

- `pip install pipenv`
- `pipenv sync`
- `pipenv install <package_name>`

## PyAutoGUI

Control mouse and keyboard: <https://pyautogui.readthedocs.io/en/latest/>
