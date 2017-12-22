import java.io.*;
import java.net.*;

public class Client {

	public static void main(String[] args) {
		DatagramSocket ds = null;
		byte[] buf = new byte[1024];
		DatagramPacket dp = null;
		try {
			ds = new DatagramSocket(9000);
		} catch (SocketException ex) {
		}
		dp = new DatagramPacket(buf, 1024);
		try {
			ds.receive(dp);
		} catch (IOException ex1) {
		}

		String str = new String(dp.getData(), 0, dp.getLength()) + " from "
				+ dp.getAddress().getHostAddress() + " : " + dp.getPort();
		System.out.println(str);

	}

}
