<h1>Python核心编程</h1>

<h2>Chapter 01</h2>
源文件.py，编译文件.pyc或.pyo。<br />
<br />
命令行启动python的选项:<br />
-d: 提供调试输出<br />
-O: 生成优化的字节码(生成 .pyo 文件)<br />
-S: 不导入 site 模块以在启动时查找 Python 路径<br />
-v: 冗余输出(导入语句详细追踪)<br />
-m mod: 将一个模块以脚本形式运行<br />
-Q opt: 除法选项(参阅文档)<br />
-c cmd: 运行以命令行字符串形式提交的 Python 脚本<br />
file: 从给定的文件运行 Python 脚本(参阅后文)<br />
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
<h2>Chapter 02</h2><br />
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
print语句：用%d表示整数，%s表示字符串，%f表示浮点数。<br />
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

