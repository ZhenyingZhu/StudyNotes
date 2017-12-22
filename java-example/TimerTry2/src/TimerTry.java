import java.util.*;

public class TimerTry {
	Timer timer;

	public TimerTry(int seconds) {
		timer = new Timer();
		timer.schedule(new TimerTryTask(), seconds * 1000);
	}

	class TimerTryTask extends TimerTask {
		public void run() {
			System.out.format("Time's up!%n");
			timer.cancel(); // Terminate the timer thread
		}
	}

	public static void main(String args[]) {
		new TimerTry(1);
		System.out.format("Task scheduled.%n");
	}
}