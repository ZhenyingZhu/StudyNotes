import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;

public class ReadByte {
	static ArrayList<byte[]> packList = new ArrayList<byte[]>(); // Packet list.
	
	public static void main(String[] args){
		readByte("test.jpg");
		writeByte("testr.jpg",packList);
	}	
	
	public static void readByte(String fileName) {
		int seqNum = 0; // sequence number of bytes.
		InputStream readIn = null;
		byte[] tempBytes = new byte[556];
		File file = new File(fileName);
		try {
			readIn = new FileInputStream(file);
			int byteRead = 0; // content length
			while ((byteRead = readIn.read(tempBytes)) != -1) {
				// Create packet content.
				creatPack(tempBytes, byteRead, seqNum, false);
				seqNum += byteRead;
			}
			// last pack, no content and sequence number is 0
			creatPack(tempBytes, 0, seqNum, true);
		} catch (Exception e1) {
			e1.printStackTrace();
		} finally {
			if (readIn != null) {
				try {
					readIn.close();
				} catch (IOException e1) {
				}
			}
		}
	}

	// Create packet
	public static void creatPack(byte[] tempBytes, int contLen, int seqNum,
			boolean flag) {
		int packLen = contLen;
		byte[] packContent = new byte[packLen];
			System.arraycopy(tempBytes, 0, packContent, 0, contLen);
			packList.add(packContent);
		}
	
	public static void writeByte(String path, ArrayList<byte[]> content) {
		try {
			File file = new File(path);
			if (file.exists()) {
				System.out
						.println("The file is exist and has been refreshed. ");
			} else {
				if (!file.createNewFile()) {
					System.out.println("Error. ");
				}
			}
			byte[] byteOut = new byte[1024];
			int length = 0; // Content length.
			FileOutputStream writeOut = new FileOutputStream(file);
			for (int i = 0; i < content.size(); i++) {
				byteOut = content.get(i);
				length = content.get(i).length;
				writeOut.write(byteOut, 0, length);
				writeOut.flush();
			}
			writeOut.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
}
