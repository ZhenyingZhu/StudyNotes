require 'socket'

if __FILE__ == $0
    begin

        socket = TCPSocket.open("127.0.0.1", 12000)
        socket.puts "from client"
        response = socket.gets
        puts response

        socket.close

    rescue Exception => e
        puts e.message
        puts e.backtrace
        exit
    end

end

        
