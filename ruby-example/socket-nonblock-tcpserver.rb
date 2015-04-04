require 'socket'

def tcpserver(port)    
        puts "Start server on #{port}"
        server = TCPServer.new(port)

    begin
        begin
            client_socket = server.accept_nonblock
        rescue IO::WaitReadable, Errno::EINTR
            IO.select([server], nil, nil, 3)
            puts "Server port #{port} retry"
            if $stop
                break; 
            else
                retry
            end
        end

        puts "#{port} now going to work"
        receive = client_socket.gets
        if receive.chomp == "quit"
            $stop = true
            puts "#{port} quit"
        else
            puts "Client: #{receive}"
            puts "see broken?"
            client_socket.puts "Hello! "
        end

        client_socket.close
    end while(not $stop)
end

$stop = false

threads_list = []
for i in 2200..2205 do
    threads_list << Thread.new { tcpserver(i) }
    sleep 0.01
end

threads_list.each { |thread| thread.join }

