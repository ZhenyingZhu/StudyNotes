import java.io.*;
import java.net.*;

public class Server {
	public static void main(String[] args) throws IOException {
		// Establish server
		ServerSocket serverSocket = null;
		BufferedReader in = null;
		PrintWriter out = null;
		try {
			serverSocket = new ServerSocket(7002); // !!!
		} catch (IOException e) {
			System.err.println("Cannot listen on this port. ");
			System.exit(1);
			e.printStackTrace();
		}
		Socket incomeSocket = null;
		// Send
		incomeSocket = serverSocket.accept(); // !!!
		out = new PrintWriter(incomeSocket.getOutputStream(), true); // !!!
		in = new BufferedReader(new InputStreamReader(
				incomeSocket.getInputStream())); // !!!
		out.println("Hello");
		out.flush();
		while (true) {
			String str = in.readLine(); // !!!
			if (str == null) {
				break;
			} else {
				out.println("Echo" + str);
				out.flush();
				// Ignore spaces before bye and big case
				if (str.trim().equalsIgnoreCase("BYE")) { 
					break;
				}
			}
		}
		// close
		in.close();
		out.close();
		incomeSocket.close();
		serverSocket.close();
		System.out.println("loop finish");
	}

}
