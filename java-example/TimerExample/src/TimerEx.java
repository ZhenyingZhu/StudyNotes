import java.util.*;

public class TimerEx {
	public static class Reminder{
		Timer timer;
		public Reminder(){			
		}
		public Reminder(int seconds,int seq) {
			timer = new Timer();
			timer.schedule(new AckTask(seq), seconds * 1000);
		}
		class AckTask extends TimerTask {
			int seq=0;
			public AckTask(){
			}
			public AckTask(int seq){
				this.seq=seq;
			}
			public void run() {
				System.out.println("Timer "+seq);
				timer.cancel(); // Terminate the timer thread
			}
		}		
	}

	public static void main(String args[]) {
		Reminder RTT=new Reminder();
		for(int i=0;i<4;i++){
			System.out.println("Task scheduled"+i);
			
			if(i==2){
				RTT.timer.cancel();;				
			}
		}
		System.out.println("Finish");
	}
}
