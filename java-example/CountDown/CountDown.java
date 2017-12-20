import java.util.*;
import java.io.*;

import sun.audio.*;

public class CountDown {

	static class UIThread extends Thread {
		Timer cdTimer = new Timer();

		public void run() {
			BufferedReader userIn = new BufferedReader(new InputStreamReader(
					System.in));
			while (true) {
				String cmd = null;
				try {
					cmd = userIn.readLine();
				} catch (IOException e2) {
					e2.printStackTrace();
				}
				if(cmd.equals("exit")){
					System.exit(0);
				}
				int t = 1;
				try {
					t = Integer.parseInt(cmd);
				} catch (NumberFormatException e1) {
					e1.printStackTrace();
				}
				try {
					FileInputStream file = new FileInputStream("explosion.wav");
					cdTimer.schedule(new BeepTask(cdTimer, file, t),
							t * 60 * 1000);
					System.out.println(new Date(System.currentTimeMillis()));
					//this.sleep(t*60*1000+10);
				} catch (FileNotFoundException e) {
					e.printStackTrace();
				}

			}
		}
	}

	static class BeepTask extends TimerTask {
		Timer timer;
		long time;
		FileInputStream file;

		public BeepTask(Timer timer, FileInputStream file, int t) {
			this.timer = timer;
			this.time = t * 1000;
			this.file = file;
		}

		public void run() {
			AudioStream au = null;
			try {
				au = new AudioStream(file);
			} catch (IOException e) {
				e.printStackTrace();
			}
			AudioPlayer.player.start(au);
			System.out.println("Time out! ");
			System.out.println("Time: ");
		}
	}

	public static void main(String[] args) {
		System.out.println(new Date(System.currentTimeMillis()));
		new UIThread().start();
		System.out.println("Time: ");
	}
}
