import java.io.*;
import java.net.*;
import java.util.ArrayList;
import java.util.Calendar;

public class Server {
	//Create Arrays to storage users login status.
	static ArrayList<User> connectingList = new ArrayList<User>();
	static ArrayList<User> historyList = new ArrayList<User>();
	static ArrayList<User> blockList = new ArrayList<User>();
	static String passwordList[][] = new String[9][2];
	//Create a class to show users' information.  
	public static class User {
		public String userName;
		public Socket socket;
		public Calendar loginTime;

		public User(String userName, Socket socket, Calendar loginTime) {
			this.userName = userName;
			this.socket = socket;
			this.loginTime = loginTime;
		}
	}

	public static void main(String args[]) {
		//Read port. 
		int listenPort = 7001;
		if (args.length == 1)
			listenPort = Integer.parseInt(args[0]);
		else {
			System.out.println("Wrong Listen Port. Use 7001 instead. ");
		}
		//Read users' name and passwords. 
		readFile(passwordList);

		// Listen to port.
		ServerSocket server = null;
		try {
			server = new ServerSocket(listenPort);
		} catch (IOException e) {
			e.printStackTrace();
			System.exit(-1);
		}
		System.out.println("Port: " + listenPort);

		//New socket thread. 
		try {
			while (true) {
				new SocketThread(server.accept()).start();
			}
		} catch (Exception e) {
			e.printStackTrace();
			System.exit(-1);
		} finally {
			try {
				if(!server.isClosed()) server.close();
			} catch (IOException e) {
				e.printStackTrace();
				System.exit(-1);
			}
		}

	}

	// Thread for accepting a client.
	public static class SocketThread extends Thread {
		private Socket socket;
		private Calendar loginTime;
		private BufferedReader input;
		private PrintWriter output;
		private User newUser;

		public SocketThread(Socket socket) {
			this.socket = socket;
		}

		public void run() {
			//Transmit information buffer. 
			try {
				input = new BufferedReader(new InputStreamReader(socket.getInputStream()));
				output = new PrintWriter(socket.getOutputStream());
			} catch (IOException e) {
				e.printStackTrace();
				System.exit(-1);
			}

			// The login user has been blocked.
			for (int i = 0; i < blockList.size(); i++) {
				User userIn = blockList.get(i);
				if (socket.getInetAddress().equals(userIn.socket.getInetAddress())) {
					long timeInterval = Calendar.getInstance().getTimeInMillis()-userIn.loginTime.getTimeInMillis();
					if (timeInterval <= 60000) {
						output.println("You have been blocked. ");
						output.println("QUIT");
						output.flush();
						try {
							socket.close();
						} catch (IOException e) {
							e.printStackTrace();
							System.exit(-1);
						}
						return;
					} else {
						//The login user can connect again. 
						blockList.remove(userIn);
					}
				}
			}

			//Login user input name and password.  
			String inputUsername = null;
			String inputPassword = null;
			String savedPassword = null;
			int location = 0; //Using for change password. 
			int tryTimes = 0;
			while (true) {
				//Check trying times.
				if (tryTimes == 3) {
					// Add this user to block list.
					User blockUser = new User(inputUsername, socket, Calendar.getInstance());
					blockList.add(blockUser);
					System.out.println("The IP address "+socket.getInetAddress()+" has been blocked.");
					output.println("You have input wrong password for 3 times. Please try again after 60 seconds.");
					output.println("QUIT");					
					output.flush();
					try {
						socket.close();
					} catch (IOException e) {
						e.printStackTrace();
						System.exit(-1);
					}
					return;
				}
				
				//User input name and password.
				try {
					output.println("Username:");
					output.flush();
					inputUsername = input.readLine();
					output.println("Password:");
					output.flush();
					inputPassword = input.readLine();
				} catch (IOException e) {
					e.printStackTrace();
					System.exit(-1);
				}

				//Get the user's password.  
				for(int i= 0; i<9; i++){
					if(inputUsername.equals(passwordList[i][0])){
						savedPassword = passwordList[i][1];
						location = i;
						break;
					}
				}
				
				// The login user has already logged in.
				boolean hasIn = false; 
				for (int i = 0; i < connectingList.size(); i++) {
					if (connectingList.get(i).userName.equals(inputUsername)) {
						hasIn = true; 
						break;
					}					
				}
				
				//The user name is wrong. 
				if (savedPassword == null) {
					output.println("Can't find your username in server. ");
					output.flush();
					tryTimes++;
					continue;
				} else if (hasIn){
					output.println("You have already logged in. ");
					output.println("QUIT");
					output.flush();
					try {
						socket.close();
					} catch (IOException e) {
						e.printStackTrace();
						System.exit(-1);
					}
					return;
				}else{
					//The password is wrong. 
					if (!savedPassword.equals(inputPassword)) {
						output.println("The password doesn't match your username.");
						output.flush();
						tryTimes++;
						continue;
					} else {
						// Record the login information. 
						System.out.println("User "+inputUsername+" has logged in.");
						loginTime = Calendar.getInstance();
						newUser = new User(inputUsername, socket, loginTime);
						connectingList.add(newUser);
						historyList.add(newUser);
						output.println("Welcome to simple server, "+inputUsername+"! ");
						output.println("waiting your command. "); 
						output.flush();
						break;
					}
				}
				
			}
			
			//Waiting commend. And count command interval. 
			Calendar commandTime = newUser.loginTime;
			long noAnswer = 0;
			while (true) {
				String command = null;
				//Input command. 
				try {
					command = input.readLine();
				} catch (IOException e) {
					e.printStackTrace();
					System.exit(-1);
				}
				noAnswer = (Calendar.getInstance().getTimeInMillis()-commandTime.getTimeInMillis());
				if(noAnswer < 20000){
					commandTime = Calendar.getInstance();
					switch (encodeMessage(command)) {
					case -1:// Command can't be recognized.
						output.println(command+" is not recognized as a command. ");
						output.flush();
						break;
					case 1:// whoelse
						if (connectingList.size() == 1) {
							output.println("You are the only user.");
							output.flush();
							break;
						}
						output.println("Display other connected users: ");
						for (int i = 0; i < connectingList.size(); i++) {
							if (!connectingList.get(i).equals(newUser)) {
								User userIn = connectingList.get(i);
								output.print(userIn.userName+", ");
							}
						}
						output.println("are online. [end]");
						output.flush();
						break;
					case 2:// wholasthr
						Calendar nowTime = Calendar.getInstance();
						output.println("Display users that connected within the last hour: ");
						for (int i = 0; i < historyList.size(); i++) {
							User userIn = historyList.get(i);
							long timeInterval = nowTime.getTimeInMillis()-userIn.loginTime.getTimeInMillis();
							if (timeInterval < 3600000) {
								output.println(userIn.userName+" has logged in. ");
							}
						}
						output.println(" [end]");
						output.flush();
						
						break;
					case 3:// broadcast
						String[] commandSplit = command.split(" ");
						if(commandSplit.length == 1){
							output.println("Please broadcast some message. ");
							output.flush();
							break;						
						}else{
							for (int i = 0; i < connectingList.size(); i++) {
								if (!connectingList.get(i).equals(newUser)) {
									try {
										User userTmp = connectingList.get(i);
										PrintWriter tmpOut = new PrintWriter(userTmp.socket.getOutputStream());
										tmpOut.println("Broadcast: "+commandSplit[1]);
										tmpOut.flush();
									} catch (IOException e) {
										e.printStackTrace();
										System.exit(-1);
									}
								}
							}
							break;
						}
					case 4:// chgpass
						try {
							output.println("Please input your current password.");
							output.println("Current password: ");
							output.flush();
							inputPassword = input.readLine();
							if(!inputPassword.equals(savedPassword)){
								output.println("The password is wrong. ");
								output.flush();
								break;
							}else{
								output.println("Please input new password. ");
								output.println("New password: ");
								output.flush();
								inputPassword = input.readLine();
								output.println("Confirm password: ");
								output.flush();
								String inputPassword2 = input.readLine();
								if(!inputPassword2.equals(inputPassword)){
									output.println("They are not same. ");
									output.flush();
									break;
								}else{
									passwordList[location][1] = inputPassword;
									System.out.println("User "+newUser.userName+" has changed password. ");
									output.println("You have changed password. ");
									output.flush();
									break;
								}
							}
						} catch (IOException e) {
							e.printStackTrace();
							System.exit(-1);
						}
						
					case 5:// logout
						removeUser(newUser);
						System.out.println("User "+inputUsername+" has logged out.");
						try {
							PrintWriter tmpOut = new PrintWriter(newUser.socket.getOutputStream());
							tmpOut.println("Logout successfully.");
							tmpOut.println("QUIT");
							tmpOut.flush();
							newUser.socket.close();
						} catch (IOException e) {
							e.printStackTrace();
							System.exit(-1);
						}
						return;
					}

				}else{
					//If client hasn't respond for a long time. 
					output.println("You have leave too long, please input your password to comfirm. ");
					output.println("Password: ");
					output.flush();
					try {
						inputPassword = input.readLine();
					} catch (IOException e) {
						e.printStackTrace();
						System.exit(-1);
					}
					if(inputPassword.equals(savedPassword)){
						output.println("Waiting your command. ");
						output.flush();
						commandTime = Calendar.getInstance();;
					}else{
						output.println("Wrong password. ");
						output.flush();
						removeUser(newUser);
						System.out.println("User "+inputUsername+" has logged out.");
						try {
							PrintWriter tmpOut = new PrintWriter(newUser.socket.getOutputStream());
							tmpOut.println("QUIT");
							tmpOut.flush();
							newUser.socket.close();
						} catch (IOException e) {
							e.printStackTrace();
							System.exit(-1);
						}
						return;
					}
				}
				
			}
		}
		//Remove user when log out. 
		private void removeUser(User user){
			connectingList.remove(user);
		}
	}
		
	// Function to decode message.
	public static int encodeMessage(String message) {
		String[] messageSplit;
		try {
			messageSplit = message.split(" ");
		} catch (Exception e) {
			e.printStackTrace();
			System.exit(-1);
			return -1;
		}
		if (messageSplit[0].equals("whoelse"))
			return 1;
		if (messageSplit[0].equals("wholasthr"))
			return 2;
		if (messageSplit[0].equals("broadcast"))
			return 3;
		if (messageSplit[0].equals("chgpass"))
			return 4;
		if (messageSplit[0].equals("logout"))
			return 5;
		return -1;
	}

		// Input the name and password list.
		public static void readFile(String userList[][]) {
			File file = new File("userlist.txt");
			BufferedReader reader = null;
			try {
				reader = new BufferedReader(new FileReader(file));
				String tempString = null;
				int line = 0;
				while ((tempString = reader.readLine()) != null) {
					userList[line] = tempString.split(" ");
					line++;
					}
				reader.close();
			} catch (IOException e) {
				e.printStackTrace();
				System.exit(-1);
			} finally {
				if (reader != null) {
					try {
						reader.close();
					} catch (IOException e) {
						e.printStackTrace();
						System.exit(-1);
					}
				}
			}
		}

}

