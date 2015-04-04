require 'socket'
include Socket::Constants

sockets = []
for i in 0..5 do
    socket = Socket.new(AF_INET, SOCK_STREAM, 0)
    socket.bind( Socket.sockaddr_in(2200 + i, 'localhost') )
    socket.listen(5)
    sockets << socket
end

i = 0
begin 
    client_socket, client_addrinfo = sockets[i].accept_nonblock
rescue IO::WaitReadable, Errno::EINTR
    puts "port #{i} Now in rescue"
    IO.select(sockets)
    i += 1
    if i == 5
        i = 0
    end
    retry
end

puts "Client: #{client_socket.readline.chomp}"
client_socket.puts "Hello! "
socket.close

