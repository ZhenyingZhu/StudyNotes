import java.io.*;
import java.net.*;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;

public class Server {
	static int defaultPort = 20137;
	static boolean logFlag = true;
	static HashMap<String, String> userMap = new HashMap<String, String>();
	// Save the users who are connecting to server
	static ArrayList<User> userOnlineList = new ArrayList<User>();
	// Save the users who have connected to server
	static ArrayList<User> userTotalList = new ArrayList<User>();
	// Save clients who are blocked
	static ArrayList<User> userBlockList = new ArrayList<User>();

	// Class to describe a user, saving username, logintime, its socket and status.
	public static class User {
		public String userName;
		public Calendar loginTime;
		public Socket socket;
		public Boolean online;

		public User(String userName, Calendar loginTime, Socket socket) {
			this.userName = userName;
			this.loginTime = loginTime;
			this.socket = socket;
			this.online = true;
		}
	}

	// Read file to get usernames and passwords. Save them to a hashmap.
	public static void readFile(String file, HashMap<String, String> userMap) {
		try {
			FileReader f = new FileReader(file);
			BufferedReader br = new BufferedReader(f);
			
			String oneLine;
			while ((oneLine = br.readLine()) != null) {
				String[] lineSpirit = oneLine.split(" ");
				userMap.put(lineSpirit[0], lineSpirit[1]);
				//logOutput(lineSpirit[0]+lineSpirit[1]);
			}
			br.close();
			f.close();
		} catch (IOException e) {
			System.out.println("Can't open userpass.ini");
			e.printStackTrace();
		}
	}

	// Function to decode message.
	public static int decodeMessage(String message) {
		if (message == null)
			return 0;
		String[] messageSplit;
		try {
			messageSplit = message.split(" ");
		} catch (Exception e) {
			return -1;
		}
		if (messageSplit[0].equals("whoelse"))
			return 1;
		if (messageSplit[0].equals("wholasthr"))
			return 2;
		if (messageSplit[0].equals("broadcast"))
			return 3;
		if (messageSplit[0].equals("logout"))
			return 4;
		return -1;
	}

	public static void main(String args[]) {
		int listenPort;
		if (args.length != 0)
			listenPort = Integer.parseInt(args[0]);
		else {
			System.out.println("You didn't choose a port number. Server will listen port "+defaultPort+".");
			listenPort = defaultPort;
		}

		readFile("userpass.ini", userMap);
		logOutput("Reading User-Password file from userpass.ini...");
		// Listening to a port.
		ServerSocket server = null;
		try {
			server = new ServerSocket(listenPort);
			logOutput("Creating SeverSocket...");
		} catch (IOException e) {
			System.out.println("Cannot listen to Port " + listenPort + ". Please choose another port and try again.");
			System.exit(-1);
			// e.printStackTrace();
		}
		System.out.println("Server is listening port " + listenPort);

		// start a new socket thread to accept
		try {
			while (true) {
				new SocketThread(server.accept()).start();
				// System.out.println("Accepted.");
			}
		} catch (Exception e) {
			System.out.println("Error:" + e);
		} finally {
			try {
				if(!server.isClosed()) server.close();
			} catch (IOException e) {
				e.printStackTrace();
			}
		}

	}

	// Thread for accepting a client.
	public static class SocketThread extends Thread {
		private Socket socket;
		private Calendar loginTime;
		private BufferedReader in;
		private PrintWriter out;
		private User newUser;

		public SocketThread(Socket socket) {
			this.socket = socket;
		}

		public void run() {
			logOutput("A new client accepted.");
			try {
				in = new BufferedReader(new InputStreamReader(
						socket.getInputStream()));
				out = new PrintWriter(socket.getOutputStream());
			} catch (IOException e) {
				e.printStackTrace();
			}

			// Check whether this client is in the block list.
			for (int i = 0; i < userBlockList.size(); i++) {
				User userTmp = userBlockList.get(i);
				//Check the client if it meets any blocked user.
				if (socket.getInetAddress().equals(userTmp.socket.getInetAddress())) {
					Long timeDifference = Calendar.getInstance().getTimeInMillis() - userTmp.loginTime.getTimeInMillis();
					if (timeDifference < 60 * 1000) {
						out.println("You have been blocked at "+userTmp.loginTime.getTime());
						out.println("CLOSESOCKET");
						out.flush();
						logOutput("IP address "+ socket.getInetAddress() +" has been blocked.");
						try {
							socket.close();
						} catch (IOException e) {
							e.printStackTrace();
						}
						return;
					} else {
						//The block time passed. Remove it from block list for efficiency.
						userBlockList.remove(userTmp);
					}
				}
			}                                                 

			String inputUsername = null;
			String inputPassword = null;
			int tryTimes = 0;
			
			//Login process.
			while (true) {
				//Check if user has tried for 3 times.
				if (tryTimes == 3) {
					// Add this user to block list.
					User blockUser = new User(inputUsername, Calendar.getInstance(), socket);
					userBlockList.add(blockUser);
					logOutput("A client IP " + socket.getInetAddress() + " has been blocked.");
					out.println("Sorry, you have tried 3 times. Your IP address will be blocked for 60 seconds.");
					out.println("CLOSESOCKET");
					out.flush();
					try {
						socket.close();
					} catch (IOException e) {
						System.out.println("Something wrong happened when trying to close the socket. ERROR: "+e);
						//e.printStackTrace();
					}
					return;
				}
				
				//Ask user to input username and password.
				try {
					out.println("Username:");
					out.flush();
					inputUsername = in.readLine();
					out.println("Password:");
					out.flush();
					inputPassword = in.readLine();
					//Receiving a null indicating that the socket is closed.
					//So we close the socket and return.
					if(inputUsername==null || inputPassword==null){
						socket.close();
						return;
					}
				} catch (IOException e) {
					e.printStackTrace();
					return;
				}
				String savedPassword = userMap.get(inputUsername);
				
				//If we can't find the user
				if (savedPassword == null) {
					out.println("Username doen't exist!");
					out.flush();
					tryTimes++;
					continue;
				} else {
					//Successfully find the user in the table. Check the password.
					if (!savedPassword.equals(inputPassword)) {
						// System.out.println(savedPassword+inputPassword);
						out.println("Wrong password, please retry!");
						out.flush();
						tryTimes++;
						continue;
					} else {
						// Login successfully. Create a new user and insert it
						// in the table.
						logOutput("User "+ inputUsername + " login successfully.");
						loginTime = Calendar.getInstance();
						newUser = new User(inputUsername, loginTime, socket);
						//Add user to onlineList and totalList.
						userOnlineList.add(newUser);
						userTotalList.add(newUser);
						out.println("Login successfully. Welcome, " + inputUsername);
						out.flush();
						break;
					}
				}
			}
			
			//Main loop for serving a client.
			while (true) {
				String command = null;
				try {
					command = in.readLine();
					//If the client disconnect from server, the command is null.
					if(command==null){
					//	throw new SocketException();
					}
					logOutput("Command received from user " + inputUsername + ": " + command);
				} catch (IOException e) {
					removeUser(newUser);
					logOutput(inputUsername + "(" + socket.getInetAddress() + ":" + socket.getPort() + ") disconnected.");
					try {
						socket.close();
					} catch (IOException e1) {
						System.out.println("Something wrong happened when trying to close the socket. ERROR: "+e);
						//e1.printStackTrace();
					}
					return;
					//e.printStackTrace();
				}
				
				//Process the message from client.
				switch (decodeMessage(command)) {
				case 0:// Null command.
					break;
				case -1:// Command can't be decoded.
					out.println("Server cannot recognize your command. Please check and try again.");
					out.flush();
					break;
				case 1:// whoelse
					if (userOnlineList.size() == 1) {
						out.println("No other users online now.");
						out.flush();
						break;
					}
					out.printf("%-10s %-20s %-6s\n", "USERNAME", "IP ADDRESS",
							"PORT");
					for (int i = 0; i < userOnlineList.size(); i++) {
						if (!userOnlineList.get(i).equals(newUser)) {
							User userTmp = userOnlineList.get(i);
							out.printf("%-10s %-20s %-6s\n", userTmp.userName,
									userTmp.socket.getInetAddress(),
									userTmp.socket.getPort());
						}
					}
					out.flush();
					break;
				case 2:// wholasthr
					Calendar nowTime = Calendar.getInstance();
					out.printf("%-10s %-20s %-6s %-10s\n", "USERNAME",
							"IP ADDRESS", "PORT", "ACTIVE");
					for (int i = 0; i < userTotalList.size(); i++) {
						User userTmp = userTotalList.get(i);
						Long timeDifference = nowTime.getTimeInMillis()
								- userTmp.loginTime.getTimeInMillis();
						if (timeDifference < 1 * 60 * 60 * 1000) {
							out.printf("%-10s %-20s %-6s %-10s\n",
									userTmp.userName,
									userTmp.socket.getInetAddress(),
									userTmp.socket.getPort(), userTmp.online);
							out.flush();
						}
					}
					break;
				case 3:// broadcast
					String[] commandSplit = command.split(" ");
					for (int i = 0; i < userOnlineList.size(); i++) {
						if (!userOnlineList.get(i).equals(newUser)) {
							try {
								User userTmp = userOnlineList.get(i);
								PrintWriter tmpOut = new PrintWriter(
										userTmp.socket.getOutputStream());
								tmpOut.println("[Broadcast]["
										+ userTmp.userName
										+ userTmp.socket.getInetAddress() + ":"
										+ userTmp.socket.getPort() + "]"
										+ commandSplit[1]);
								tmpOut.flush();
							} catch (IOException e) {
								e.printStackTrace();
							}
						}
					}
					break;
				case 4:// logout
					removeUser(newUser);
					try {
						PrintWriter tmpOut = new PrintWriter(newUser.socket.getOutputStream());
						tmpOut.println("Logout successfully.");
						tmpOut.println("CLOSESOCKET");
						tmpOut.flush();
						newUser.socket.close();
					} catch (IOException e) {
						e.printStackTrace();
					}
					return;

				}
			}
		}
		private void removeUser(User user){
			userTotalList.get(userTotalList.indexOf(user)).online = false;
			userOnlineList.remove(user);
		}
	}
	
	//Convenient for output logs.
	static void logOutput(String msg){
		DateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		if(logFlag) System.out.println("[LOG]["+df.format(new Date())+"]"+msg);
	}
}
