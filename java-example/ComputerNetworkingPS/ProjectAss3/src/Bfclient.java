import java.io.*;
import java.net.*;
import java.util.*;

public class Bfclient {
	static ArrayList<DisVector> DVList = new ArrayList<DisVector>();
	static int listenPort = 0; // Send out as local port.
	static boolean upDate = false; // Vector has updated.
	static boolean allDone = false; // When update finish, start transmit.
	static boolean shutDown = false;

	public static class DisVector {
		String destAdd = null;
		int destPort = 0;
		float weight = 0;
		String linkAdd = null;
		int linkPort = 0;
		int neiFlag = 0;// 1 neighbor, 2 local
		boolean infFlag = false;		

		public DisVector(String destAdd, int destPort, float weight,
				String linkAdd, int linkPort) {
			this.destAdd = destAdd;
			this.destPort = destPort;
			this.weight = weight;
			this.linkAdd = linkAdd;
			this.linkPort = linkPort;
		}
	}
	
	// Send periodically
	static class SendTask extends TimerTask {
		Timer timer;
		int time;

		public SendTask(Timer timer, int time) {
			this.timer = timer;
			this.time = time;
			if (DVList.size() > 1) {
				new SendProcess(7001).sendList();
			}
		}

		public void run() {
				if (DVList.size() > 1) {
					new SendProcess(7001).sendList();
				}
				timer.schedule(new SendTask(timer, time), time * 1000);
			this.cancel();
		}
	}

	static class UIThread extends Thread {
		public void run() {
			BufferedReader userIn = new BufferedReader(new InputStreamReader(
					System.in));
			String readLine = null;
			while (true) {
				try {
					readLine = userIn.readLine();
				} catch (IOException e) {
					System.err.println("Input error");
					e.printStackTrace();
				}
				if (Thread.interrupted())
					break;
				// Recognize command
				boolean recCmd = false;
				// Show DV list
				if (readLine.equals("SHOWRT")) {
					recCmd = true;
					System.out.println("<"
							+ new Date(System.currentTimeMillis())
							+ "> Distance vector list is: ");
					if (DVList.size() == 0)
						System.out.println("You don't know any neighbour. ");
					else {
						for (int i = 0; i < DVList.size(); i++) {
							String weightShow = null;
							if (DVList.get(i).infFlag) {
								weightShow = " Inf";
							} else {
								weightShow = " " + DVList.get(i).weight;
							}
							System.out.println("Destination = "
									+ DVList.get(i).destAdd + ":"
									+ DVList.get(i).destPort + ", Cost ="
									+ weightShow + ", Link = ("
									+ DVList.get(i).linkAdd + ":"
									+ DVList.get(i).linkPort + ")");
						}
					}
					continue;
				}
				// Close
				if (readLine.equals("CLOSE")) {
					recCmd = true;
					try {
						userIn.close();						
					} catch (IOException e) {
						e.printStackTrace();
					}
					shutDown = true;
					System.exit(2); // Actually others function don't close
					break;
				}
				if (readLine.equals("send")) {
					if (DVList.size() > 1) {
						new SendProcess(7001).sendList();
					}
				}
				String[] cmdIP = readLine.split(" ");
				// Link down
				if (cmdIP[0].equals("LINKDOWN")) {
					recCmd = true;
					if (cmdIP.length < 3) {
						System.out.println("Please give IP and PORT");
						continue;
					} else {
						// Search for this link
						boolean downNot = true;
						for (int i = 0; i < DVList.size(); i++) {
							if (cmdIP[1].equals(DVList.get(i).destAdd)
									&& Integer.parseInt(cmdIP[2]) == DVList
											.get(i).destPort
									&& DVList.get(i).neiFlag == 1) {
								DVList.get(i).infFlag = true;
								DVList.get(i).neiFlag = 0;
								System.out.println("This link is down");
								downNot = false;
								break;
							}
						}
						if (downNot) {
							System.out.println("You don't have that link.");
							continue;
						}
					}
				}
				// Restore a link down by client
				if (cmdIP[0].equals("LINKUP")) {
					recCmd = true;
					if (cmdIP.length < 3) {
						System.out.println("Please give IP and PORT");
						continue;
					} else {
						// Search for this link
						boolean upNot = true;
						for (int i = 0; i < DVList.size(); i++) {
							// Can't recovery original is infinite
							if (cmdIP[1].equals(DVList.get(i).destAdd)
									&& Integer.parseInt(cmdIP[2]) == DVList
											.get(i).destPort
									&& DVList.get(i).infFlag) {
								DVList.get(i).infFlag = false;
								DVList.get(i).neiFlag = 1;
								System.out.println("This link is recover");
								upNot = false;
								break;
							}
						}
						if (upNot) {
							System.out.println("This link hadn't down by you.");
							continue;
						}
					}
				}
				if (!recCmd) {
					System.out.println("Unknown command");
				}
			}
		}
	}

	static class SendProcess {
		int sendPort = 0;

		public SendProcess(int sendPort) {
			this.sendPort = sendPort;
		}

		public void sendList() {
			// First DV is self. Send to all neighbor
			for (int i = 1; i < DVList.size(); i++) {
				if (DVList.get(i).neiFlag == 1) {
					DatagramSocket sendSocket = null;
					try {
						// //send from 127.0.0.1
						sendSocket = new DatagramSocket(sendPort);
					} catch (SocketException e1) {
						System.err.println("Cannot open send socket. ");
						e1.printStackTrace();
					}
					InetAddress sendAdd = null;
					try {
						sendAdd = InetAddress.getByName(DVList.get(i).destAdd);
					} catch (UnknownHostException e) {
						System.err.println("Unknown address. ");
						e.printStackTrace();
						break;
					}
					// Send to neighbor
					int sendPort = DVList.get(i).destPort;
					byte[] sendData = makeDVList(DVList.get(i).destAdd,
							sendPort);
					DatagramPacket sendPacket = new DatagramPacket(sendData,
							sendData.length, sendAdd, sendPort);
					try {
						sendSocket.send(sendPacket);
					} catch (IOException e) {
						System.err.println("Send error. ");
						e.printStackTrace();
						break;
					}
					sendSocket.close(); // Close after send to a neighbor.
				}
			}
		}
	}

	public static void main(String[] args) {
		if (args.length < 2) {
			System.out.println("The invoke command is wrong. ");
		}
		if (Integer.parseInt(args[0]) == 7001) {
			System.out
					.println("7001 is used to send, please choose another port");
		}
		listenPort = Integer.parseInt(args[0]);
		int time = Integer.parseInt(args[1]);
		// Initial Distance vector.
		DVList.add(new DisVector("LocalHost", listenPort, 0, "LocalHost",
				listenPort)); // Local Distance Vector
		DVList.get(0).neiFlag = 2;
		if (args.length > 2) {
			// i+2=length means out bound.
			for (int i = 2; i < args.length - 2; i = i + 3) {
				String destAdd = args[i];
				int destPort = Integer.parseInt(args[i + 1]);
				float weight = Float.valueOf(args[i + 2]);
				DisVector distance = new DisVector(destAdd, destPort, weight,
						destAdd, destPort);
				distance.neiFlag = 1;
				DVList.add(distance);
			}
		}
		// UI interface
		new UIThread().start();
		Timer timer=new Timer();
		timer.schedule(new SendTask(timer, time), time * 1000);

		// Receive update
		DatagramSocket rcvrSocket = null;
		try {
			rcvrSocket = new DatagramSocket(listenPort);
		} catch (SocketException e) {
			System.err.println("Cannot open receive socket. ");
			e.printStackTrace();
			// // System.exit(-1);
		}
		while (!shutDown) {
			byte[] rcvrData = new byte[1024];
			DatagramPacket rcvrPacket = new DatagramPacket(rcvrData,
					rcvrData.length);
			try {
				rcvrSocket.receive(rcvrPacket);
				// //Read packet and insert to list
				String neiAdd = rcvrPacket.getAddress().toString().substring(1);
				if (getDVList(rcvrData, neiAdd) == -1) {
					System.err.println("receive unreadable message");
				}
			} catch (IOException e) {
				System.err.println("Receive error. ");
				e.printStackTrace();
				break;
			}
		}
		// //Cannot close while waiting receive!
		rcvrSocket.close();
		DVList = null;
		System.out.println("main close");

	}// main end

	static public int getDVList(byte[] readByte, String neiAdd) {
		String mess = new String(readByte);
		String[] readLine = mess.split("\n");
		// Judge receive readable packet
		String[] header = readLine[0].split(" ");
		if (!header[0].equals("FROM:")) {
			System.out.println("Packet header error");
			return -1; // error, abandon this packet
		}
		if (!readLine[readLine.length - 2].equals("END")) {
			System.out.println("Packet ending error");
			return -1;
		}
		// Get packet source
		int neiPort = Integer.parseInt(header[1]);
		// Get local host
		DVList.get(0).destAdd = header[3];
		DVList.get(0).destPort = Integer.parseInt(header[4]);
		DVList.get(0).linkAdd = header[3];
		DVList.get(0).linkPort = Integer.parseInt(header[4]);

		// Start Bellman-Ford
		// Get neighbor weight
		float cLocalTo = 255;
		boolean newNode = true; // This is not in vector
		for (int i = 0; i < DVList.size(); i++) {
			if (DVList.get(i).destAdd.equals(neiAdd)
					&& DVList.get(i).destPort == neiPort) {
				cLocalTo = DVList.get(i).weight;
				// It may recover
				DVList.get(i).neiFlag = 1;
				newNode = false;
				break;
			}
		}
		if (newNode) {
			for (int i = 1; i < readLine.length; i++) {
				if (readLine[i].equals("END")) {
					break;
				}
				String[] readWord = readLine[i].split(" ");
				// Find link to me vector in message
				if (readWord[1].equals(header[3])
						&& Integer.parseInt(readWord[2]) == Integer
								.parseInt(header[4])) {
					cLocalTo = Float.parseFloat(readWord[4]);
				}
			}
		}

		// Get DV List from packet
		for (int i = 1; i < readLine.length; i++) {
			if (readLine[i].equals("END")) {
				break;
			}
			String[] readWord = readLine[i].split(" ");
			boolean newVec = true; // Not in list
			for (int j = 0; j < DVList.size(); j++) {
				if (readWord[1].equals(DVList.get(j).destAdd)
						&& Integer.parseInt(readWord[2]) == DVList.get(j).destPort) {
					// If there is a better way
					float dToDest = 0;
					dToDest = cLocalTo + Float.parseFloat(readWord[4]);
					// //just see what weight
					System.out.println(neiPort + " " + DVList.get(j).weight
							+ " " + dToDest);
					if (dToDest < DVList.get(j).weight) {
						DVList.get(j).weight = dToDest;
						DVList.get(j).linkAdd = neiAdd;
						DVList.get(j).linkPort = neiPort;
						upDate = true;
					}
					newVec = false;
					break;
				}
			}
			if (newVec) {
				// New vector, add in
				// [1]destAdd,[2]destPort,[4]weight,[6]),[7]not mine
				DisVector getVector = new DisVector(readWord[1],
						Integer.parseInt(readWord[2]),
						Float.parseFloat(readWord[4]) + cLocalTo, neiAdd,
						neiPort);
				if (readWord[1].equals(neiAdd)
						&& Integer.parseInt(readWord[2]) == neiPort
						|| readWord[1].equals("LocalHost")) {
					// New neighbor
					getVector.destAdd = neiAdd;
					getVector.destPort = neiPort;
					getVector.neiFlag = 1;
				}
				DVList.add(getVector);
			}
		}
		return 1;
	}

	static public byte[] makeDVList(String toAdd, int toPort) {
		byte[] sendData = new byte[1024];
		String header = "FROM: " + listenPort + " To: " + toAdd + " " + toPort
				+ "\n";
		byte[] headerByte = header.getBytes();
		System.arraycopy(headerByte, 0, sendData, 0, headerByte.length);
		int pos = headerByte.length; // Array position
		for (int i = 0; i < DVList.size(); i++) {
			if (DVList.get(i).infFlag) { // disconnect link vector
				String listLine = "D " + DVList.get(i).destAdd + " "
						+ DVList.get(i).destPort + " W " + "Inf" + " L "
						+ DVList.get(i).linkAdd + " " + DVList.get(i).linkPort
						+ "\n";
				byte[] listLineByte = listLine.getBytes();
				System.arraycopy(listLineByte, 0, sendData, pos,
						listLineByte.length);
				pos += listLineByte.length;
			} else {
				String listLine = "D " + DVList.get(i).destAdd + " "
						+ DVList.get(i).destPort + " W " + DVList.get(i).weight
						+ " L " + DVList.get(i).linkAdd + " "
						+ DVList.get(i).linkPort + "\n";
				byte[] listLineByte = listLine.getBytes();
				System.arraycopy(listLineByte, 0, sendData, pos,
						listLineByte.length);
				pos += listLineByte.length;
			}
		}
		String ending = "END\n";
		byte[] endingByte = ending.getBytes();
		System.arraycopy(endingByte, 0, sendData, pos, endingByte.length);
		return sendData;
	}

}// class bf end
