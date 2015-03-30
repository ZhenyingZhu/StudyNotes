https://docs.python.org/2/library/subprocess.html

#Python核心编程

##Chapter 01
源文件.py，编译文件.pyc或.pyo。<br />
<br />
命令行启动python的选项:<br />
-d: 提供调试输出<br />
-O: 生成优化的字节码(生成 .pyo 文件)<br />
-S: 不导入 site 模块以在启动时查找 Python 路径<br />
-v: 冗余输出(导入语句详细追踪)<br />
-m mod: 将一个模块以脚本形式运行<br />
-Q opt: 除法选项?<br />
-c cmd: 运行以命令行字符串形式提交的 Python 脚本<br />
file: 从给定的文件运行 Python 脚本<br />
<br />
执行脚本: python script.py,或在脚本第一行加入sh-bang:<br />
<code>#!/usr/local/bin/python</code><br />
指定执行脚本的程序。<br />
<br />
保持脚本运行后不关闭: <code>raw_input()</code>。<br />
<br />
各种语言介绍: (Python核心编程P25)<br />
<br />
栈帧: 用stackless的解释器来避开C 函数调用的限制。(Python核心编程P29)<br />
<br />
##Chapter 02
语句用关键字组成命令。<br />
<code>print 'Hello World'</code><br />
表达式没有关键字，可以用运算符或括号调用的函数。<br />
<code>abs(-4)</code><br />
执行后可以有输出，可以没有(返回None)。<br />
<br />
<code>\_</code>表示前一个表达式的值。<br />
<br />
变量赋值: <br />
<code>mystring = 'Hello World!'</code><br />
<br />
用print显示字符串,结果没有引号,调用了str()函数。<br />
<code>print mystring</code><br />
直接用变量名，为区分其他类型变量，字符串型变量有单引号，调用了repr()函数。<br />
<code>mystring</code><br />
<br />
print语句：用%d表示整数，%s表示字符串，%f表示浮点数。
```
who = 'knights'
what = 'Ni!'
print 'We are the %s who say %s' % (who, ((what + ' ') * 4))
```
得到：We are the knights who say Ni! Ni! Ni! Ni!

在print 语句最后加`,`，可以取消换行。

<code>print "%s is number %d" % ("Python", 1)</code><br />
可以用>>重导向导出到屏幕上作为系统报错<br />
<code>import sys</code><br />
<code>print >> sys.stderr, 'Error'</code><br />
<br />
或导出到文件，这里是增加到文本末尾。当关闭文件后才会写入。<br />
<code>logfile = open('/tmp/mylog.txt', 'a')</code><br />
<code>print >> logfile, 'Error'</code><br />
<code>logfile.close()</code><br />
<br />
输出多字符串：,自带空格，+不带。<br />
<code>print a, b</code> <br />
<code>print a + b </code><br />
<br />
键盘输入：<br />
<code>num = raw_input('Please input a number: ')</code><br />
<code>print 'double the number is %d' % (int(num) * 2)</code><br />
<br />
帮助：<code>help(func)</code> <br />
注释：<code>#</code>到行尾。<br />
在线文档：在函数定义起始的字符串。 <br />
```
def foo():
    "This is a function"
    return True
```

### 运算符：
+, -, \*, /整形除, \*\*指数, //浮点除, %。

几乎所有数据类型都可以用+运算符。数组和字符串为串接操作。


<, <=, >, >=, ==, !=, <>等同于前者。支持3<4<5的操作，用and连接。

and, or, not。

变量名：大小写敏感，字母开始。

动态类型语言：不需要先声明。

不支持自增++，支持增量赋值 <code>n *= 10 </code>

数字：int(0x80, -0X92), long(-20L, 0xAEL), bool(True = 1), float(-6.0e23, 1.5E-19), complex(0+1j, -1.23-875J)。导入后可用decimal类型，用以处理十进制小数的误差（outdate）。

Python 支持使用成对的单引号或双引号，三引号（三个连续的单引号或者双引号）可以用来包含特殊字符。
第一个字符的索引是0，最后一个字符的索引是-1。字符串连接运算+，字符串重复*。
使用[ ]和[:]可以得到子字符串。
```
pystr = 'python'
pystr[0] # 'p'
pystr[:2] # 'py'
'-' * 20 # '------------'
pystr = '''python
...is cool''' # 'python\nis cool'
print pystr # python
is cool
```

### 数组：
可以混合储存任何类型。

List用[]创建，可以通过下标重新赋值。

Tuple用()表示，不可以重新赋值。
```
aList = [1, 2, 3, 4] # Create a list
aList[1] = 5

aTuple = ('apple'， 2， 3) # cannot do aTuple[1] = 5
```

### 字典：
键值对用{}创建。

```
aDict = {'host': 'earth'}
aDict['port'] = 80 # Add a tuple
aDict.keys()
for key in aDict: 
    print key, aDict[key]
```

### 条件：
```
if x < .0: print 'x is smaller than 0'
elif x < 1: print 'x is smaller than 1'
else: print 'x is larger than 1'
```

### 循环：
While: 
```
counter = 0
while counter < 3: 
    print 'loop #%d' % (counter)
    counter += 1
```

Iterator For: 
```
for item in ['email', 'phone', 'work']: 
    print item, 
```

Numerical For: 
```
str = 'abc'
for i in range(len(str)): 
    print '(%d)' % i, str[i], 
```

Enumerate: 
```
for i, ch in enumerate(str): 
    print i, ch
```
实际上该函数返回的是一个元组(idx, element)集合。

用循环创建列表：

```
sqdEven = [x ** 2 for x in range(8) if not x % 2]
```

### 读取文件： 
```
handle = open("file_name", 'a')
handle.readline() # Output shows '\n'
for eachLine in handle:
    print eachLine, # Output change lines automatically
handle.close()
```
access_mode 默认为r，还有w, a为添加，U为读写，b为用二进制读取（过时）。

注意当有同名文件时，w会覆盖。

Attribute 可以是数据，也可以是方法。

### 处理异常：
try-except块：
```
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

### 函数：
必须在调用前定义。

Python 通过引用调用函数，对参数的改动会在函数外起作用。
```
def add2Me(x = 1): # default value is 1
    "Add myself twice"
    return x + x
```
函数名中的括号一定需要。

无return语句时返回None 对象。

### 类：
没有定义基类的话，就使用object类。
```
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
```
fool = FooClass('Jane')
fool.showname() # your name is Jane, My name is FooClass
```

### 模块：
Module 就是module.py 这个文件里的组织形式。可import。
```
import sys

sys.stdout.write("Hello World\n")
sys.platform
sys.version
```
write()函数不会自动换行。

PEP(Python Enhancement Proposal)： 用以增加新特性。[PEP](http://python.org/dev/peps)。


P48