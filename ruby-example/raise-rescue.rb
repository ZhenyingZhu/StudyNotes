cnt = 0

apple = 1

begin
    while true
        if apple != 2
            raise ArgumentError, "should be 1"
        end
    end
rescue => e
    puts "ERROR: #{e.message}"
    cnt += 1
    if e.is_a? ArgumentError
        puts apple
        exit
    end
    retry
end
    



