import java.io.*;
import java.util.*;

public class TimerUse {
	static boolean skip=false;
	static Timer timerT;

	public static class TimerUseTask extends TimerTask {
		Timer timer;
		int time;

		public TimerUseTask(Timer timer, int time) {
			this.timer = timer;
			this.time = time;
		}

		public void run() {
			if(!skip){
				System.out.println("T"+System.currentTimeMillis());
				timer.schedule(new TimerUseTask(timer, 1), time * 1000);
			}else{
				skip=false;
				this.cancel();
			}
		}
	}
	
	public static class CloseTask extends TimerTask {
		Timer timer;
		int time;

		public CloseTask(Timer timer, int time) {
			this.timer = timer;
			this.time = time;
		}

		public void run() {
			if(!skip){
				System.out.println("C"+System.currentTimeMillis());
				timer.schedule(new CloseTask(timer, 1), time * 1000);
			}else{
				skip=false;
				this.cancel();
			}
		}
	}

	public static void main(String[] args) {
		timerT = new Timer();
		timerT.schedule(new TimerUseTask(timerT, 1), 1000);
		timerT.schedule(new CloseTask(timerT, 1), 5000);
		System.out.println("Finish");
		BufferedReader userIn=new BufferedReader(new InputStreamReader(System.in));
		String line = null;
		while(true){
			try {
				line=userIn.readLine();
			} catch (IOException e) {
				e.printStackTrace();
			}
			if(line.equals("s")){
				skip=true;
				System.out.println("skip!");
				System.out.println(System.currentTimeMillis());
				timerT.schedule(new TimerUseTask(timerT, 1), 1000);
			}
			if(line.equals("e")){
				System.exit(0);
			}
		}
	}

}
