import java.util.*;

public class TimerUse {

	public static class TimerUseTask extends TimerTask{
		Timer timer;
		public TimerUseTask(Timer timer){
			this.timer=timer;
		}
		public void run(){
			System.out.println(System.currentTimeMillis());
			cancel();
		}
	}
	public static void main(String[] args) {
		Timer timer=new Timer();
		for(int i=0;i<5;i++){
			timer.schedule(new TimerUseTask(timer), i*1000);
		}
		System.out.println("Finish");
	}

}
