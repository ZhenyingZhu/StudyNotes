require 'thread'

def recstop(mode)
    for i in 0..mode do 
        if @stop
            puts "#{mode} end"
            break
        else
            puts "#{mode} running #{i}"
            sleep 1
        end
     end

     @stop = true
end

modes = [5, 10]
test_threads = []

@stop = false

modes.each do |mode|
    test_threads << Thread.new {
        recstop(mode)
    }
end

test_threads.each do |thread|
    thread.join
end

