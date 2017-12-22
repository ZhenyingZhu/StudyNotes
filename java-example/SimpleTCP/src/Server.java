import java.io.*;
import java.util.*;
import java.net.*;

public class Server {
	static ArrayList<byte[]> packList = new ArrayList<byte[]>(); //Packet list. 
	static int headSet = 0; //header length.
	static int contentLen = 100; //

	public static void main(String[] args) {
		readByte("E:/test.txt", 100);
		String pt = new String();
		System.out.println("******");
		pt = new String(packList.get(0));
		System.out.println(pt);
		System.out.println("******");
		pt = new String(packList.get(packList.size()-1));
		System.out.println(pt);

		InetAddress remoteIP = null;
		try {
			remoteIP = InetAddress.getByName("127.0.0.1");
		} catch (UnknownHostException e) {
			e.printStackTrace();
		}
		int listenPort = 9000;

		DatagramSocket ds = null;
		DatagramPacket dp = null;
		try {
			ds = new DatagramSocket(3000);
		} catch (SocketException ex) {
		}
		dp = new DatagramPacket(pt.getBytes(), pt.length(), remoteIP,
				listenPort);
		try {
			ds.send(dp);
		} catch (IOException ex2) {
		}
		ds.close();

	}

	public static void readByte(String fileName, int length) {
		InputStream readIn = null;
		byte[] tempBytes = new byte[length];
		File file = new File(fileName);
		try {
			readIn = new FileInputStream(file);
			int byteRead = 0;
			while ((byteRead = readIn.read(tempBytes)) != -1) {
				creatPack(tempBytes,byteRead);
			}
		} catch (Exception e1) {
			e1.printStackTrace();
		} finally {
			if (readIn != null) {
				try {
					readIn.close();
				} catch (IOException e1) {

				}
			}
		}
	}

	public static void creatPack(byte[] tempBytes,int contLen) {
		int packLen=0;
		packLen=contLen+headSet;
		byte[] packContent = new byte[packLen];
		System.arraycopy(tempBytes, 0, packContent, headSet, contLen);
		packList.add(packContent);
	}
}
