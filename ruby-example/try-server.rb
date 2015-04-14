require 'socket'
require 'timeout'

if __FILE__ == $0

    begin
    
        server = TCPServer.open(12000)
        client = server.accept
        client.puts("from server1")
        
        #response = client.gets
        #puts response

        client2 = server.accept

        #puts client2.gets

        #client.close
        server.close

    rescue Exception => e
        puts e.message
        puts e.backtrace
        exit
    end

end

