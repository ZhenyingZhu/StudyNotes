public class StringByte {

	static public void main(String[] args) {

		String mess = "Hello world";
		byte[] code = new byte[1024];
		code = mess.getBytes();
		String decode = new String(code);
		System.out.println("Original: " + mess);
		System.out.println("Decode: " + decode);
	}
}
