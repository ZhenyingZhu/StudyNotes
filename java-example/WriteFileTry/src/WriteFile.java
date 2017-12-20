import java.io.*;
import java.util.ArrayList;


public class WriteFile {

	public static void main(String[] args) {
		String inputation = "Columbia log in. "; 
		ArrayList<String> logInfo = new ArrayList<String>();
		logInfo.add(inputation);
		inputation = "log out. ";
		logInfo.add(inputation);
		write("E:\\123.txt", logInfo);

	}
	
	public static void write(String path, ArrayList<String> content) {
		String s1 = new String();
		try {
			File recordLog = new File(path);
			if (recordLog.exists()) {
				System.out.println("Exist. ");
			} else {
				recordLog.createNewFile();
				if (!recordLog.createNewFile()) {
					System.out.println("Error. ");
				}
			}
			BufferedWriter writeOut = new BufferedWriter(new FileWriter(recordLog));
			for(int i=0; i<content.size(); i++){
				s1 = content.get(i);
				writeOut.write(s1);
				writeOut.newLine();
				writeOut.flush();
			}
			writeOut.close();
			} catch (Exception e) {
				e.printStackTrace();
			}
		}

}
/*
bufferedWriter.write(data);bufferedWriter.newLine();//换行
 刷新该流的缓冲。
 关键的一行代码。如果没有加这行代码。数据只是保存在缓冲区中。没有写进文件。
 加了这行才能将数据写入目的地。
bufferedWriter.flush();
write.close();
bufferedWriter.close();
 */
