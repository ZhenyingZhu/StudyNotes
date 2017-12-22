import java.io.*;

class WriteFile {
	public boolean write(String wriStr) {
		boolean succ = false;
		try {
			File txtFile = new File("tryChar.txt");
			if (txtFile.exists()) {
				System.out.println("Refresh");
			} else {
				txtFile.createNewFile();
				if (!txtFile.createNewFile()) {
					System.out.println("Error");
				}
			}
			BufferedWriter outLine = new BufferedWriter(new FileWriter(txtFile));
			outLine.write(wriStr);
			outLine.newLine();
			outLine.close();
			succ = true;
		} catch (IOException e) {
			e.printStackTrace();
		}
		return succ;
	}
}

public class SpeChar {
	public static void main(String[] args) {
		String s1 = "AB\fC";
		System.out.println(s1);
		new WriteFile().write(s1); // /Doesn't work as described
	}
}
