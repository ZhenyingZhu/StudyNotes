import java.io.*;
import java.net.*;

public class Client {
	public static void main(String args[]){
		int serverPort;
		if(args.length==0){
			System.out.println("You should use one argument to connect to server.");
			return;
		}
		serverPort=Integer.parseInt(args[0]);
		try{
			Socket socket=new Socket("127.0.0.1", serverPort);
			Thread userInput = new UserInputThread(socket);
			userInput.start();
			BufferedReader in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
			while(true){
				String tmp = in.readLine();
				if(tmp!=null){
					if(tmp.equals("CLOSESOCKET")){
						System.out.println("Socket closed. Press ENTER to exit.");
						socket.close();
						userInput.interrupt();
						userInput.join();
						return;
					}
					System.out.println(tmp);
				}
			}
			
		}catch(Exception e){
			System.out.println("Error"+e);
		}
	}
	
	
	public static class UserInputThread extends Thread{
		private Socket socket;
		private BufferedReader userIn;
		PrintWriter dataOut;
		
		public UserInputThread(Socket socket){
			this.socket = socket;
		}
		
		public void run(){
			try{
				userIn=new BufferedReader(new InputStreamReader(System.in));
				dataOut=new PrintWriter(socket.getOutputStream());	
				String readLine = null;
				while(true){
					readLine = userIn.readLine();
					if(Thread.interrupted()) break;
					dataOut.println(readLine);
					dataOut.flush();
				}
			}catch(Exception e){
				System.out.println(e);
			}
			
		}
		
	}
}
