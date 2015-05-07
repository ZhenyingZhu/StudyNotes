require 'socket'

SIZE = 1024

if __FILE__ == $0
    begin

        socket = TCPSocket.open("127.0.0.1", 12000)

        file = File.open('rec.dat', 'w')
        while chunk = socket.read(SIZE)
            puts chunk
            puts '\n'
            file.write(chunk)
        end


        socket.close

    rescue Exception => e
        puts e.message
        puts e.backtrace
        exit
    end

end

        
