import java.util.*;


class AckTask extends TimerTask{
	int i;
	public AckTask(){		
	}
	public AckTask(int i){
		this.i=i;
	}
	public void run() {
		System.out.println(i+" "+System.currentTimeMillis());
	}
}

public class time {
	public static void main(String args[]) {
		Timer timerTry = new Timer();
		for(int i=0;i<3;i++){
			timerTry.schedule(new AckTask(i), i*1000);
		}
		System.out.println("Finish");
	}
}