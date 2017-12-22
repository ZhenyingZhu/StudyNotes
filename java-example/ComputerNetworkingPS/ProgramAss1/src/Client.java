import java.io.*;
import java.net.*;
import java.util.ArrayList;

public class Client {

	//Record information from server. 
	static String message = null;
	static ArrayList<String> logInfo = new ArrayList<String>();

	public static void main(String args[]) {
		//Wrong host or port. 
		if(args.length <= 1){
			System.out.println("Please input host and port.");
			return;
		}
		//Get IP address and port. 
		String serverIP = args[0];
		int serverPort;
		serverPort = Integer.parseInt(args[1]);
		
		try {
			Socket socket = new Socket(serverIP, serverPort);
			Thread userInput = new UserInputThread(socket);
			userInput.start();
			BufferedReader input = new BufferedReader(new InputStreamReader(socket.getInputStream()));
			//Read in from Sever and record. 
			while(true){
				message = input.readLine();
				System.out.println(message);
				logInfo.add(message);
				//Transmission failed or finish. 
				if(message != null){
					if(message.equals("QUIT")){
						write("log.txt", logInfo);
						socket.close();
						userInput.interrupt();
						userInput.join();
						return;
					}
				}
			}
		}catch(Exception e){
			e.printStackTrace();
			System.exit(-1);
		}
	}
	public static class UserInputThread extends Thread{
		private Socket socket;
		private BufferedReader input;
		PrintWriter output;
		
		public UserInputThread(Socket socket){
			this.socket = socket;
		}
		
		public void run(){
			//Transmitting message. 
			try{
				input=new BufferedReader(new InputStreamReader(System.in));
				output=new PrintWriter(socket.getOutputStream());	
				String readLine = null;
				while(true){
					readLine = input.readLine();
					if(Thread.interrupted()) break;
					output.println(readLine);
					output.flush();
				}
			}catch(Exception e){
				e.printStackTrace();
				System.exit(-1);
			}			
		}		
	}
	
	//Write file. 
	public static void write(String path, ArrayList<String> content) {
		String lineOut = new String();
		try {
			File recordLog = new File(path);
			if (recordLog.exists()) {
				System.out.println("The log has been refreshed. ");
			} else {
				if (!recordLog.createNewFile()) {
					System.out.println("Error. ");
				}
			}
			BufferedWriter writeOut = new BufferedWriter(new FileWriter(recordLog));
			for(int i=0; i<content.size(); i++){
				lineOut = content.get(i);
				writeOut.write(lineOut);
				writeOut.newLine();
				writeOut.flush();
			}
			writeOut.close();
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
}
