#! /usr/bin/ruby

def func1
    i = 0
    while i <= 10
        puts "func1 at: #{Time.new}"
        sleep(2)
        i += 1
    end
end

def func2
    j = 0
    while j <= 10
        puts "func2 at: #{Time.now}"
        sleep(1)
        j += 1
    end
end

t1 = Thread.new{func1()}
t2 = Thread.new{func2()}

t1.join
t2.join
puts "End at #{Time.now}"


