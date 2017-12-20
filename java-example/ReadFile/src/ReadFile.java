import java.io.*;


public class ReadFile {

	public static void main(String[] args) {
//	public static void readFile(String fileName) {
		String userList[][] = new String[9][2]; 
		File file = new File("E:\\userpass.txt");
		BufferedReader reader = null;
		try {
			reader = new BufferedReader(new FileReader(file));
			String tempString = null;
			int line = 0;
			while ((tempString = reader.readLine()) != null) {
				userList[line] = tempString.split(" ");
				line++;
				}
			reader.close();
			} catch (IOException e) {
				e.printStackTrace();
				} finally {
					if (reader != null) {
						try {
							reader.close();
							} catch (IOException e1) {
							}
					}
				}
		System.out.println(userList[0][1]);
	}
		
}
