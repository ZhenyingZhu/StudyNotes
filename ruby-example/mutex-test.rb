require 'thread'

mutex = Mutex.new
resource = ConditionVariable.new

a = Thread.new {
    mutex.synchronize {
        resource.wait(mutex)
    }
    puts "a get the resource"
}

b = Thread.new {
    mutex.synchronize {
        puts "b get the resource"
        sleep 3
        resource.signal
    }
}

a.join
b.join
