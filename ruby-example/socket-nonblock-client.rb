require 'socket'
include Socket::Constants
socket = Socket.new(AF_INET, SOCK_STREAM, 0)
sockaddr = Socket.sockaddr_in(2201, 'localhost')
socket.connect(sockaddr)
socket.puts "Helloooo! "
puts "server #{socket.readline.chomp}"
socket.close

