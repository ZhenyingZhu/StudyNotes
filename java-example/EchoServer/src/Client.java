import java.io.*;
import java.net.*;

public class Client {
	public static void main(String[] args) throws IOException {
		// Establish
		Socket cliSocket = null;
		BufferedReader in = null;
		PrintWriter out = null;
		try {
			cliSocket = new Socket("127.0.0.1", 7002); // !!!
			out = new PrintWriter(cliSocket.getOutputStream(), true); // !!!
			in = new BufferedReader(new InputStreamReader( // !!!
					cliSocket.getInputStream()));
		} catch (UnknownHostException e) {
			System.err.println("Don't know about the IP.");
			System.exit(1);
		} catch (IOException e) {
			System.err.println("Can't get I/O.");
			System.exit(1);
		}
		System.out.println(in.readLine());
		// Send
		BufferedReader keyIn = new BufferedReader(new InputStreamReader(
				System.in)); //Keyboard
		String userInput;
		while ((userInput = keyIn.readLine()) != null) {
			out.println(userInput);
			out.flush(); //if no flush, "BYE" to end doesn't work
			System.out.println(userInput);
		}
		out.close();
		in.close();
		cliSocket.close();
	}
}
