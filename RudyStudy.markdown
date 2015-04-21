http://stackoverflow.com/questions/4190797/how-can-i-remove-the-string-n-from-a-ruby-string  

http://stackoverflow.com/questions/15769739/determining-type-of-an-object-in-ruby  

(yield)[http://www.tutorialspoint.com/ruby/ruby_blocks.htm]_

(Ruby Socket)[http://ruby-doc.org/stdlib-1.9.3/libdoc/socket/rdoc/Socket.html#method-i-accept]   
```
# Pull down Google's web page
require 'socket'
include Socket::Constants
socket = Socket.new(AF_INET, SOCK_STREAM, 0)
sockaddr = Socket.sockaddr_in(80, 'www.google.com')
begin # emulate blocking connect
  socket.connect_nonblock(sockaddr)
rescue IO::WaitWritable
  IO.select(nil, [socket]) # wait 3-way handshake completion
  begin
    socket.connect_nonblock(sockaddr) # check connection failure
  rescue Errno::EISCONN
  end
end
socket.write("GET / HTTP/1.0\r\n\r\n")
results = socket.read
```

Thread.join: http://ruby-doc.org/core-1.9.3/Thread.html
http://stackoverflow.com/questions/3672586/what-is-the-difference-between-require-relative-and-require-in-ruby
http://stackoverflow.com/questions/2416372/static-variables-in-ruby
(http://en.wikibooks.org/wiki/Ruby_Programming/Syntax/Variables_and_Constants)_
http://stackoverflow.com/questions/2199282/killing-all-thread-workers-when-one-thread-found-the-answer-ruby
[mutex](http://ruby-doc.org/core-1.9.3/Mutex.html)
`<=>`: Combined comparison operator. Returns 0 if first operand equals second, 1 if first operand is greater than the second and -1 if first operand is less than the second.  
`=~`: string regular express. (http://stackoverflow.com/questions/3025838/what-is-the-operator-in-ruby)
`%r{}`: regular express (http://stackoverflow.com/questions/12384704/the-ruby-r-expression)  
[ways to excute](http://tech.natemurray.com/2007/03/ruby-shell-commands.html)  

```
kid_io = IO.popen(cmd)
o = IO.read()
kid_io.close() if kid_io and !kid_io.closed?
```
```
if array.find { |c| dosomething?(c) }.nil? # if find, execute next
    puts "null"
end
```

`@@var` is class var: (http://stackoverflow.com/questions/5890118/what-does-variable-mean-in-ruby)  

`$&` the match part of previous regular express: (http://jimneath.org/2010/01/04/cryptic-ruby-global-variables-and-their-meanings.html)  

### Enviroment: 
To run a single commend: 
```
ruby -e 'puts "Hello World"'
```

To run input/evaluation loop:  
```
ruby
puts "Hello World" # print and return nil. 
ctrl + D # EOF, then the previous lines will excuted. 
```

To use interpreter: `irb`  
To run a script: 
```
ruby my-script.rb
```

Replaces foo with bar in all C source and header files in the current working directory, backing up the original files with ".bak" appended:  
```
ruby -i.bak -pe 'sub "foo", "bar"' *.[ch]
```

### Variable: 
Integer in ruby is allowed by memory. 400! can also be caculated.  
String: 'str' doesn't [STOP HERE http://www.rubyist.net/~slagell/ruby/strings.html]


### Syntax: 
Comment: after hash mark # 
+, -, or backslash at the end of a line, they indicate the continuation of a statement.  
a + b is interpreted as a+b ( Here a is a local variable)  
a  +b is interpreted as a(+b) ( Here a is a method call)  

Reserved Words:  
```
BEGIN, do, next, then, END, else, nil, true, alias, elsif, 
not, undef, and, end, or, unless, begin, ensure, redo, until, 
break, false, rescue, when, case, for, retry, while, class, 
if, return, while, def, in, self, __FILE__, defined?, module, 
super, __LINE__
```

Output: 
print and return nil:  
```
puts "Hello World"
```

turn var into string and append to outside:  
```
puts "Hello #{var}"
```

Operations: 
Math is a build-in module. Operations return float numbers. 
```
a = 3 ** 2
Math.sqrt(6)
```

Convert to int: `to_i`. Convert to string:`to_s`:  
```
Class.to_s
```

### Judge: 
```
if @name.nil?
    puts "..."
elsif @name.respond_to?("each") # So it is a list
    @name.each do |name|
        puts "Hello #{name}"
    end
else
    puts "Hello #{name}"
end
```

### Loop: 
List has each and join method: 
```
if @name.response_to?("join")
    puts "Hello @name.join(", ")" # this is one string after join
end
```

```
@name.each do |name|
    puts "#{name}" # run this block of code for each elements in name. 
end
```

### Method: 
define a method: 
```
def h
    puts "Hello World"
end
```

or
```
def h(name = "World")
    puts "Hello #{name}!"
end
```

By default ruby function returns the last thing that was evaluated in it. 

excute a method:  
```
h var
h(var)
```

### Class: 
define a class: 
```
class Greeter
    def initialize(name = "World") #must have one
        @name = name
    end
    def say_hi
        puts "Hello #{@name}"
    end
end
```

Create an Object: 
```
g = Greeter.new("name")
```
which will call initialize method. 

```
g.say_hi
```
to call say\_hi method.  
But cannot access to @name directly.  

See All the methods: 
```
Class.instance_methods(false)
```
`true` can see methods from the ancestors and protected methods. 

See if a class has a method:  
```
g.respond_to?("method")
```


### Script: 
Define Script interpreter: 
```
#!/usr/bin/env ruby
```

Ruby don't use main to find where to start. It start from the first line that is not in a method.  
`ARGV` is the array of inputs.  
```
ruby fact.rb 1 # ARGV[0] is 1
```

`__FILE__` contains the name of this script. 
`$0` is the file that start the program. 
```
if __FILE__ == $0 # it is the main function 
```
in irb mode, `__FILE__` is `(irb)` while `$0` is `irb`, which is not same. 

# Reference: 
http://www.rubyist.net/~slagell/ruby/examples.html




















???
Modify class. Will affect instances that has built and new in the furture. 
```
class Greeter
    attr_accessor :name
end
```

# Real Code
```
require 'date'
require\_relative 'taskqueue'

Logger::INFO, 

$excluded\_tests = []
$excluded\_tests += ''.split(' ').map { |i| i.to\_i }

$guard = Mutex.new

.empty?
.include?

in shell 2&>1 let standard err to output and output them together

File::open(logfile + '.run', 'w') do |f| 
    f.puts(Time.now) 
    f.puts(cmd)
end 

rc = $?.success? 
if rc
    new_logfile = logfile + '.pass'
end

File::rename(logfile + '.run', new\_logfile)

$guard.synchronize do
    $tests_done += 1
    $tests_pass += 1 if rc
    $tests_fail += 1 if not rc
    $log.info "[#{100*$tests_done/$tests_total}%] Test #{test_id} #{rc ? 'PASSED' : 'FAILED'}"
end 
```



```
class Greeter
    attr_accessor :name
    def initialize(name = "World")
        @name = name
    end
    def say_hi
        if @name.nil?
            puts "..."
        elsif @name.respond_to?("each")
            @name.each do |name|
                puts "Hello #{name}"
            end
        else
            puts "Hello #{@name}"
        end
    end
    def say_goodbye
        if @name.nil? 
            puts "..."
        elsif @name.respond_to?("join")
            puts "Goodbye #{@name.join(', ')}"
        else
            puts "Goodbye #{@name}"
        end
    end
    if __FILE__ == $0
        mg = Greeter.new()
        mg.name = ["Albert", "Brando"]
        mg.say_hi
        mg.say_goodbye

    end
end
```

