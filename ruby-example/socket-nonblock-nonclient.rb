require 'socket'
require 'timeout'
require 'thread'
require 'time'

include Socket::Constants

@@connection_timeout_interval = 0.1

def do_connect(port)
    sockaddr = Socket.sockaddr_in(port, 'localhost')
    socket = Socket.new(AF_INET, SOCK_STREAM, 0)
    begin
        socket.connect_nonblock(sockaddr)
        puts "get #{port} done"
    rescue IO::WaitWritable
        if IO.select(nil, [socket], nil, @@connection_timeout_interval)
            puts "Actually in rescue"
            begin
                socket.connect_nonblock(sockaddr)
            rescue Errno::EISCONN
                puts "if #{port} alreay connect, then call it good"
            rescue Errno::ECONNREFUSED
                puts "Connection Refused"
                raise "Connection Refused"
            end
        else
            socket.close
            raise "Connection timeout"
        end
    end

    if socket
        socket.puts "Helloooo! "
        #socket.puts "quit"
        #puts "server #{port} #{socket.readline.chomp}"
    end
    socket.close
end

port = 2195
begin
    do_connect(port)
rescue Exception => e
    if( e.message == "Connection Refused" )
       port += 1
       if port > 2205
           port = 2200
       end
       puts "work on #{port}"
       retry
    end
end

