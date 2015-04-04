require 'thread'

def worker(from, to)
#, mutex, resource)
    puts "From #{from} To #{to}"
    #mutex.synchronize {
        for i in from..to do
            if $myflag == true
                break; 
            end

            if i == 42
                puts "kill the rest of the threads"
                #resource.signal
                $myflag = true
                break; 
            end
            puts "now #{i} begins from #{from}"
            sleep 1
         end
         #resource.signal
    #}
 end

if __FILE__ == $0
    threads = []
#    mutex = Mutex.new
#    resource = ConditionVariable.new
    $myflag = false

    arr = [3, 4, 5]
    #mutex.synchronize {
        for t in arr do
            tmp = t+1
            threads << Thread.new{ worker(tmp * 10, tmp * 10 + 10) }
            #sleep 0.01 # Compile error? Without it doesn't work
             
        end
        #resource.wait(mutex)
    #}
    #threads[1].join
    threads.each{ |thread| thread.join }
end

