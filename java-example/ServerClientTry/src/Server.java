import java.io.*;
import java.net.*;

public class Server {
	public static void main(String[] args) throws IOException {

		ServerSocket listenSocket = null;

		PrintWriter ackOut = null;
		try {
			listenSocket = new ServerSocket(7001);
		} catch (IOException e) {
			System.out.println("Cannot listen on this port. ");
			System.exit(1);
			e.printStackTrace();
		}
		Socket remoteSocket = null;
		try {
			remoteSocket = listenSocket.accept();
		} catch (IOException e) {
			System.out.println("Accept failed. ");
			System.exit(1);
			e.printStackTrace();
		}
		int ack=3;

		ackOut = new PrintWriter(remoteSocket.getOutputStream(), true);
		for(int i=0;i<3;i++){
			ackOut.println(ack+i);	
		}
		remoteSocket.close();
		listenSocket.close();

	}
}
