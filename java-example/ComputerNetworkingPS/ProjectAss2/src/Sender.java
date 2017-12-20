import java.io.*;
import java.util.*;
import java.net.*;

public class Sender {
	static final int paSize = 576 - 20; // Content length without header.
	static ArrayList<byte[]> packList = new ArrayList<byte[]>(); // Packet list.
	// Record packet sequence number.
	static ArrayList<PackInfo> packInfo = new ArrayList<PackInfo>();
	// Port information.
	static int remotePort = 3000;
	static int listenPort = 9000;
	// Sending indicate.
	static int nextSend = 0;
	static boolean fin = false; // Whether transmit is finish
	static File recordLog = null;
	static int i = 0;
	static int errTime = 0; // The retransmitted number.
	static int totalSend = 0;

	// Establish and send packet.
	public static class SendProcess {
		InetAddress remoteIPn = null;
		DatagramSocket sendSocket = null;
		DatagramPacket sendPacket = null;

		// Construct SendProgress
		public SendProcess(String remoteIPNum) {
			try {
				remoteIPn = InetAddress.getByName(remoteIPNum);
			} catch (UnknownHostException e) {
				System.err.println("The remote IP cannot be used. ");
				e.printStackTrace();
			}
			try {
				sendSocket = new DatagramSocket(listenPort);
			} catch (SocketException ex) {
				sendSocket.close();
			}
		}

		// send the nth packet
		public void sendNth(int segNum) {
			sendPacket = new DatagramPacket(packList.get(segNum),
					packList.get(segNum).length, remoteIPn, remotePort);
			try {
				sendSocket.send(sendPacket);
			} catch (IOException ex2) {
				sendSocket.close();
			}
		}
	}

	// Create ACK list
	public static class PackInfo {
		int seqNum = 0;
		int ackNum = 0;

		public PackInfo(int seqNum, int conLen) {
			this.seqNum = seqNum;
			this.ackNum = conLen + seqNum;
		}
	}

	// Timer
	static class TimerThread extends Thread {
		public static long timeStart = 0;
		long timeNow = 0;
		long tInterval = 100;
		SendProcess s = null;
		int winBase = 0;
		int winSize = 0;
		boolean reACK = false;
		ComRTT t;
		Socket inSocket = null;

		public TimerThread(SendProcess s, int winSize, ComRTT t, Socket inSocket) {
			this.s = s;
			this.winSize = winSize;
			this.t = t;
			this.inSocket = inSocket;
		}

		public void run() {
			while (!fin) { // Send to window size
				timeStart = System.currentTimeMillis(); // Timer start point
				reACK = false;
				//
				String remotePortInfo = null;
				String listenPortInfo = null;
				while (nextSend < winBase + winSize) {
					// this error happens because thread runs without main
					// steady.
					if (nextSend >= packList.size()) {
						nextSend = packList.size() - 1;
					}
					s.sendNth(nextSend);
					// UDP link information.
					remotePortInfo = " " + s.sendPacket.getAddress() + ":"
							+ s.sendPacket.getPort() + "\t";
					listenPortInfo = " " + inSocket.getLocalAddress() + ":"
							+ s.sendSocket.getLocalPort() + "\t";
					if (nextSend < packList.size() - 1) {
						writeString(listenPortInfo + remotePortInfo + "\t"
								+ packInfo.get(nextSend).seqNum + "\t"
								+ packInfo.get(nextSend).ackNum + "\t" + "0"
								+ "\t" + ComRTT.estimatedRTT);
					} else {
						writeString(listenPortInfo + remotePortInfo + "\t"
								+ packInfo.get(nextSend).seqNum + "\t"
								+ packInfo.get(nextSend).ackNum + "\t" + "1");
					}
					totalSend++;
					tInterval = t.tInter;
					nextSend++;
				}
				while (!fin && !reACK) {
					timeNow = System.currentTimeMillis();
					if (timeNow - timeStart >= tInterval) {
						nextSend = winBase;
						errTime++;
						break;
					}
				}
			}
		}
	}

	static class ComRTT {
		static public long estimatedRTT = 100;
		long sampleRTT;
		long devRTT = 0;
		long tInter = 100;

		public void comTimeInt(long timeNow) {
			sampleRTT = timeNow - TimerThread.timeStart;
			if (sampleRTT > estimatedRTT) {
				devRTT = 3 * devRTT / 4 + (sampleRTT - estimatedRTT) / 4;
			} else {
				devRTT = 3 * devRTT / 4 + (estimatedRTT - sampleRTT) / 4;
			}
			estimatedRTT = 7 * estimatedRTT / 8 + sampleRTT / 8;
			tInter = estimatedRTT + 4 * devRTT;
			if (tInter <= packList.size() / 20) {
				tInter = packList.size() / 20;
			}
		}
	}

	public static void main(String[] args) throws IOException {
		// Judge if the invoke command is right.
		if (args.length < 7) {
			System.out.println("The invoke command is wrong. ");
			System.exit(1);
		}
		String fileName = args[1];
		String remoteIP = args[2];
		remotePort = Integer.parseInt(args[3]);
		listenPort = Integer.parseInt(args[4]);
		int windowSize = Integer.parseInt(args[5]);
		String logName = args[6];

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
			writeOut.write("|||||SENDER LOG|||||");
			writeOut.newLine();
			writeOut.flush();
			writeOut.close();
		} catch (IOException e) {
			System.out.println("record log file error");
			e.printStackTrace();
		}

		// Read in sending file.
		readByte(fileName);
		if (windowSize > packList.size()) { // Won't out bound.
			windowSize = packList.size();
		}

		// TCP server, read in ACK, need run first.
		ServerSocket ackSocket = null;
		BufferedReader ackIn = null;
		PrintWriter rvonOut = null;
		try {
			ackSocket = new ServerSocket(listenPort);
		} catch (IOException e) {
			System.err.println("Cannot listen on this port");
			System.exit(1);
			e.printStackTrace();
		}
		Socket inSocket = null;
		try {
			inSocket = ackSocket.accept();
		} catch (IOException e) {
			System.err.println("Accept failed. ");
			System.exit(1);
		}
		// Make sure receiver was turned on to get UDP packet.
		rvonOut = new PrintWriter(inSocket.getOutputStream(), true);
		ackIn = new BufferedReader(new InputStreamReader(
				inSocket.getInputStream()));
		rvonOut.println("zz2283");
		rvonOut.flush();

		// Establish UDP link.
		SendProcess s = new SendProcess(remoteIP);
		ComRTT t = new ComRTT();
		TimerThread timerSend = new TimerThread(s, windowSize, t, inSocket);
		timerSend.tInterval = t.tInter;
		timerSend.start();

		while (!fin) {
			if (timerSend.winSize > (packList.size() - timerSend.winBase)) {
				timerSend.winSize = packList.size() - timerSend.winBase;
			}
			// Receive ACK and decide windows base.
			String getACK;
			int ackValue = 0;
			getACK = ackIn.readLine();
			// TCP link information.
			String remoteAckPortInfo = " " + inSocket.getInetAddress() + ":"
					+ s.sendPacket.getPort() + "\t";
			String listenAckPortInfo = " " + inSocket.getLocalAddress() + ":"
					+ inSocket.getLocalPort() + "\t";
			if (getACK.equals("FIN")) { // Receive finish.
				fin = true;
				timerSend.reACK = true;
				writeString(remoteAckPortInfo + listenAckPortInfo + "\t"
						+ ackValue + "\t"
						+ packInfo.get(timerSend.winBase).ackNum + "\t" + "1");
			} else {
				ackValue = Integer.parseInt(getACK);
				writeString(remoteAckPortInfo + listenAckPortInfo + "\t"
						+ ackValue + "\t"
						+ packInfo.get(timerSend.winBase).ackNum + "\t" + "0");
				if (ackValue == packInfo.get(timerSend.winBase).ackNum) {
					timerSend.winBase++;
					t.comTimeInt(System.currentTimeMillis());
					timerSend.reACK = true;
				} else {
					for (int j = 1; j < timerSend.winSize; j++) {
						if (ackValue == packInfo.get(timerSend.winBase + j).ackNum) {
							timerSend.winBase = timerSend.winBase + j + 1;
							t.comTimeInt(System.currentTimeMillis());
							timerSend.reACK = true;
							break;
						}
					}
					// ignore ACKs received after.
				}
			}

		}

		// Finish transmit.
		rvonOut.close();
		ackIn.close();
		ackSocket.close();
		inSocket.close();
		s.sendSocket.close();
		System.out.println("Delivery completed successfully");
		System.out.println("Total Bytes sent = "
				+ packInfo.get(packInfo.size() - 2).ackNum);
		System.out.println("Segment sent = " + totalSend);
		System.out.println("Segments retransmitted = " + errTime);
		packList = null;
		packInfo = null;
	}

	public static void readByte(String fileName) {
		int seqNum = 0; // sequence number of bytes.
		InputStream readIn = null;
		byte[] tempBytes = new byte[paSize];
		File file = new File(fileName);
		try {
			readIn = new FileInputStream(file);
			int byteRead = 0; // content length
			while ((byteRead = readIn.read(tempBytes)) != -1) {
				// Create packet content.
				creatPack(tempBytes, byteRead, seqNum, false);
				PackInfo packI = new PackInfo(seqNum, byteRead);
				packInfo.add(packI);
				seqNum += byteRead;
			}
			// last pack, no content and sequence number is 0
			creatPack(tempBytes, 0, seqNum, true);
			PackInfo packI = new PackInfo(seqNum, 0);
			packInfo.add(packI);
		} catch (Exception e1) {
			System.err.println("Cannot find the file to transmit. ");
		} finally {
			if (readIn != null) {
				try {
					readIn.close();
				} catch (IOException e1) {
				}
			}
		}
	}

	// Create packet
	public static void creatPack(byte[] tempBytes, int contLen, int seqNum,
			boolean flag) {
		int packLen = 20 + contLen;
		byte[] packContent = new byte[packLen];
		// Fin flag is 0100,0000,0000,000(FIN)
		byte[] finFlagOn = { (byte) 0x40, (byte) 0x01 };
		byte[] finFlagOff = { (byte) 0x40, (byte) 0x00 };
		// header
		System.arraycopy(toByteArray(listenPort, 2), 0, packContent, 0, 2);
		System.arraycopy(toByteArray(remotePort, 2), 0, packContent, 2, 2);
		System.arraycopy(toByteArray(seqNum, 4), 0, packContent, 4, 4);
		System.arraycopy(toByteArray(contLen, 2), 0, packContent, 14, 2);
		// Input header length and FIN flag,
		if (flag) {
			System.arraycopy(finFlagOn, 0, packContent, 12, 2); // FIN flag on
		} else {
			System.arraycopy(finFlagOff, 0, packContent, 12, 2);
			// Compute checksum.
			int checkSum = 0;
			for (int i = 0; i < contLen; i++) {
				checkSum += (int) tempBytes[i];
			}
			System.arraycopy(toByteArray(checkSum, 2), 0, packContent, 16, 2);
			// Application data
			System.arraycopy(tempBytes, 0, packContent, 20, contLen);
		}
		packList.add(packContent);
	}

	// Write to log.
	public static void writeString(String content) {
		BufferedWriter writeOut;
		try {
			writeOut = new BufferedWriter(new FileWriter(recordLog, true));
			writeOut.write(new Date(System.currentTimeMillis()) + " " + content);
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
