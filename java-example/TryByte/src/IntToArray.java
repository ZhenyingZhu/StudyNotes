public class IntToArray {
	public static void main(String[] args) {
		byte x=(byte) (0xFF & 0x11);
		System.out.println(x);

	}

	static byte[] toByteArray(int number, int arrayLen) {
		byte[] arrayByte = null;
		arrayByte = new byte[arrayLen];
		int mod = number;
		for (int i = arrayLen - 1; i >= 0; i--) {
			arrayByte[i] = (byte) (mod % 256);
			mod = mod / 256;
		}
		return arrayByte;
	}

	static int toInt(byte[] arrayByte, int arrayLen) {
		int number = 0;
		int mod = 0;

		for (int i = 0; i < arrayLen; i++) {
			mod = (int) arrayByte[i];
			for (int j = 1; j < arrayLen - i; j++) {
				mod *= 256;
			}
			number += mod;
		}
		return number;
	}
}
