require 'socket'
require 'timeout'

SIZE = 1024

if __FILE__ == $0

    begin
    
        server = TCPServer.open(12000)
        client = server.accept
        
        file = File.open('binary.dat', 'rb')

        while chunk = file.read(SIZE)
            client.write(chunk)
        end

        server.close

    rescue Exception => e
        puts e.message
        puts e.backtrace
        exit
    end

end

