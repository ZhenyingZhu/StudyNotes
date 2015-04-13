
Use <code>Class.instance\_methods(false) to not see the ancestors. 
<code>Class.to\_s</code> convert to string. 

See if a class has a method: <code>g.respond\_to?("method")</code>

Modify class. Will affect instances that has built and new in the furture. 
<code>
class Greeter
    attr_accessor :name
end
</code>

<code>
if @name.nil?
    puts "..."
elsif @name.respond\_to?("each") # So it is a list
    @name.each do |name|
        puts "Hello #{name}"
    end
else
    puts "Hello #{name}"
end
</code>

List has each and join method: 
<code>
if @name.response\_to?("join")
    puts "Hello @name.join(", ")" # this is one string after join
end
</code>
<code>
@name.each do |name|
    puts "#{name}" # run this block of code for each elements in name. 
end
<code>

Comment: after hash mark #


<code>\_\_FILE\_\_</code> contains the name of this script. 
<code>$0</code> is the file that start the program. 
<code> if \_\_FILE\_\_ == $0 </code> means it is the main function 
in irb mode, \_\_FILE\_\_ is (irb) while $0 is irb, which is not same. 

#!/usr/bin/env ruby

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

http://stackoverflow.com/questions/3672586/what-is-the-difference-between-require-relative-and-require-in-ruby




