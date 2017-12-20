import java.io.*;
import java.net.*;

public class Client {
	public static void main(String[] args) throws IOException {
		Socket rackSocket = null;
		BufferedReader ackIn = null;
		try {
			rackSocket = new Socket("127.0.0.1", 7001);
			ackIn = new BufferedReader(new InputStreamReader(
					rackSocket.getInputStream()));
		} catch (UnknownHostException e) {
			System.err.println("Don't know about the IP.");
			System.exit(1);
		} catch (IOException e) {
			System.err.println("Can't get I/O.");
			System.exit(1);
		}
		for (int i = 0; i < 3; i++) {
			String getACK = ackIn.readLine();
			int ackValue = Integer.parseInt(getACK);
			System.out.println(ackValue);
		}
		ackIn.close();
		rackSocket.close();
	}

}
