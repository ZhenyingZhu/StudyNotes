import java.io.*;
import java.net.*;
import java.util.*;

public class Receiver {
	static ArrayList<byte[]> packList = new ArrayList<byte[]>(); // Packet list.
	static boolean finish = false; // whether transmit is over
	static boolean succ = true; // whether the packet is right
	static int predictSEQ = 0; // sequence number
	static ArrayList<Integer> ackList = new ArrayList<Integer>(); // ACK list.
	static File recordLog = null; // log file

	// Establish UDP link and receive packet.
	public static class RevrProcess {
		byte[] buf = new byte[1024]; // packet read in buffer
		static DatagramSocket revrSocket = null;
		static DatagramPacket revrPacket = null;
		static Socket ackSocket=null;

		// Initialize UDP
		public RevrProcess(int portNum,Socket ackSocket) {
			RevrProcess.ackSocket=ackSocket;
			try {
				revrSocket = new DatagramSocket(portNum);
			} catch (SocketException ex) {
				System.err.println("Cannot create socket. ");
				System.exit(1);
			}
		}

		// Receive new packet
		public void revrPack() {
			revrPacket = new DatagramPacket(buf, 1024);
			try {
				revrSocket.receive(revrPacket);
				// Read packet info
				int[] info = headInfo(buf);
				if (succ) {
					if (info[2] == 1) { // FIN flag
						finish = true;
					} else {
						int packLen = info[1]; // content length
						byte[] revrPack = new byte[packLen];
						System.arraycopy(buf, 20, revrPack, 0, packLen);
						packList.add(revrPack);
					}
				}
			} catch (IOException ex1) {
				System.err.println("Cannot read from socket. ");
				revrSocket.close();
			}
		}

	}

	// Get header info;
	public static int[] headInfo(byte[] packCont) {
		int[] infoArray = null;
		int segNum = toInt(packCont, 2, 14); // Content length
		int seqNum = toInt(packCont, 4, 4); // Sequence number
		//UDP link information
		String remotePortInfo = " " + RevrProcess.revrPacket.getAddress() + ":"
				+ RevrProcess.revrPacket.getPort() + "\t";
		String listenPortInfo = " " + RevrProcess.ackSocket.getLocalAddress()
				+ ":" + RevrProcess.revrSocket.getLocalPort() + "\t";
		if (seqNum != predictSEQ) {
			succ = false; // sequence number wrong
		} else {
			succ = true;
			predictSEQ += segNum;
		}
		int getCheckSum = toInt(packCont, 2, 16); // Checksum
		int finFlag = (int) packCont[13] & 0x01; // Finish flag
		if (finFlag != 1) {
			writeString(remotePortInfo + listenPortInfo + "\t" + seqNum + "\t"
					+ predictSEQ + "\t" + "0");
			// Compute checksum.
			int checkSum = 0;
			for (int i = 20; i < 20 + segNum; i++) {
				checkSum += (int) packCont[i];
			}
			int checkSumI = toInt(toByteArray(checkSum, 2), 2, 0);
			if (checkSumI != getCheckSum) {
				succ = false;
			}
		} else {
			writeString(remotePortInfo + listenPortInfo + "\t" + seqNum + "\t"
					+ predictSEQ + "\t" + "1");
		}
		int ackNum = seqNum + segNum;
		if (succ) {
			infoArray = new int[] { seqNum, segNum, finFlag, ackNum };
			ackList.add(infoArray[3]); // create ACK list
		}
		return infoArray;
	}

	public static void main(String[] args) throws IOException {
		// Judge if the invoke command is right.
		if (args.length < 6) {
			System.out.println("The invoke command is wrong. ");
			System.exit(1);
		}
		String fileName = args[1];
		int revrPort = Integer.parseInt(args[2]); // Use to receive packets
		String remoteIP = args[3];
		int ackPort = Integer.parseInt(args[4]); // Use to send ACK.
		String logName = args[5];
		// create log file.
		try {
			recordLog = new File(logName);
			if (recordLog.exists()) {
				System.out.println("The log has been refreshed. ");
			} else {
				if (!recordLog.createNewFile()) {
					System.out.println("Cannot create log file ");
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		BufferedWriter writeOut;
		try {
			writeOut = new BufferedWriter(new FileWriter(recordLog));
			writeOut.write("|||||RECEIVER LOG|||||");
			writeOut.newLine();
			writeOut.flush();
			writeOut.close();
		} catch (IOException e) {
			System.err.println("record log file error");
			e.printStackTrace();
		}

		// Establish client to send ACK.
		Socket ackSocket = null;
		BufferedReader svonIn = null;
		PrintWriter ackOut = null;
		try {
			ackSocket = new Socket(remoteIP, ackPort);
			ackOut = new PrintWriter(ackSocket.getOutputStream(), true);
			// tell sender can build UDP link
			svonIn = new BufferedReader(new InputStreamReader(
					ackSocket.getInputStream()));
		} catch (UnknownHostException e) {
			System.err.println("Don't know about the IP.");
			System.exit(1);
		} catch (IOException e) {
			System.err.println("Can't get ack I/O.");
			System.exit(1);
		}
		svonIn.readLine();

		// Establish UDP link
		RevrProcess r = new RevrProcess(revrPort,ackSocket);
		int i = 0; // the received packet number.
		// TCP link information
		String remoteAckInfo=" "+ackSocket.getInetAddress()+":"+ackSocket.getPort()+"\t";
		String listenAckInfo=" "+ackSocket.getLocalAddress()+":"+ackSocket.getLocalPort()+"\t";		
		while (!finish) {
			r.revrPack(); // Receive packet.
			if (succ) {
				i++;
			}
			if (finish) {
				ackOut.println("FIN"); // send ACK.
				writeString(listenAckInfo+remoteAckInfo+"\t"+
						+ predictSEQ + "\t"
						+ ackList.get(i - 1).intValue()+"\t"+"1");
			} else {
				if (i == 0) {
					ackOut.println(0); // The first packet lost.
				} else {
					// send last right ACK.
					ackOut.println(ackList.get(i - 1).intValue());
					writeString(listenAckInfo+remoteAckInfo+"\t"+
							+ predictSEQ + "\t"
							+ ackList.get(i - 1).intValue()+"\t"+"0");
				}
			}
		}

		// Transmission finish.
		svonIn.close();
		ackOut.close();
		ackSocket.close();
		RevrProcess.revrSocket.close();
		writeByte(fileName, packList); // Reconstruct the file from packet
		packList = null;
		ackList = null;
		System.out.println("Delivery completed successfully");
	}

	// Write to file.
	public static void writeByte(String path, ArrayList<byte[]> content) {

		try {
			File file = new File(path);
			if (file.exists()) {
				System.out
						.println("The file is exist and has been refreshed. ");
			} else {
				if (!file.createNewFile()) {
					System.out.println("Cannot create log file. ");
				}
			}
			byte[] byteOut = new byte[1024];
			int length = 0; // Content length.
			FileOutputStream writeOut = new FileOutputStream(file);
			for (int i = 0; i < content.size(); i++) {
				byteOut = content.get(i);
				length = content.get(i).length;
				writeOut.write(byteOut, 0, length);
				writeOut.flush();
			}
			writeOut.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	// Write to log.
	public static void writeString(String content) {
		BufferedWriter writeOut;
		try {
			writeOut = new BufferedWriter(new FileWriter(recordLog, true));
			writeOut.write(new Date(System.currentTimeMillis())+ " " + content);
			writeOut.newLine();
			writeOut.flush();
			writeOut.close();
		} catch (IOException e) {
			System.err.println("record log file error");
			e.printStackTrace();
		}
	}

	// Change integer to byte[]
	public static byte[] toByteArray(int number, int arrayLen) {
		byte[] arrayByte = null;
		arrayByte = new byte[arrayLen];
		int mod = number;
		for (int i = arrayLen - 1; i >= 0; i--) {
			arrayByte[i] = (byte) (mod % 256);
			mod = mod / 256;
		}
		return arrayByte;
	}

	// Change byte[] to integer
	public static int toInt(byte[] arrayByte, int arrayLen, int start) {
		int number = 0;
		int mod = 0;
		for (int i = 0; i < arrayLen; i++) {
			// To get unsigned number.
			mod = (int) (arrayByte[i + start] & 0x0FF);
			for (int j = 1; j < arrayLen - i; j++) {
				mod *= 256;
			}
			number += mod;
		}
		return number;
	}

}
