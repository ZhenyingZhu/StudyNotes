import java.io.*;
import java.util.*;

public class TreadEx {
	static boolean out = false;
	static Calendar cal = Calendar.getInstance();
	static Date date;

	public static void main(String[] args) {
		long timeStart = System.currentTimeMillis();
		cal.setTimeInMillis(timeStart);
		 date = cal.getTime();
		System.out.println(date);

		long timeInterval = 1000;
		TestThread treadEx = new TestThread(timeStart, timeInterval);
		treadEx.start();
		BufferedReader keyInput;
		String readKey = null;
		keyInput = new BufferedReader(new InputStreamReader(System.in));
		while (true) {
			try {
				readKey = keyInput.readLine();
			} catch (IOException e) {
				e.printStackTrace();
			}
			if (readKey.equals("p")) {
				treadEx.timeInterval += 500;
				System.out.println("plus");
			}
			if (readKey.equals("m")) {
				if(treadEx.timeInterval>500){
					treadEx.timeInterval -= 500;					
					System.out.println("minus");
				}else{
					System.out.println("Too fast");					
				}
			}
			if (readKey.equals("e")) {
				out = true;
				break;
			}			
		}

	}

	static class TestThread extends Thread {
		long timeStart = 0;
		long timeNow = 0;
		long timeInterval = 0;

		public TestThread() {
		}

		public TestThread(long s, long it) {
			this.timeStart = s;
			this.timeInterval = it;
		}

		public void run() {
			int j = 0;
			while (!out) {
				timeNow = System.currentTimeMillis();
				if (timeNow - timeStart > timeInterval) {
					j++;
					cal.setTimeInMillis(timeNow);
					date = cal.getTime();
					System.out.println("Thread " + j + "th   "+date);
					timeStart = timeNow;
				}
			}
			System.out.println("stop");
		}
	}

}
