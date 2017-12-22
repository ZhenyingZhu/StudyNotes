public class IntToArray {
	public static void main(String[] args) {
		int x = 12;
		int i = 20001;
		byte[] b = toByteArray(x, 2);
		int a = toInt(b, 2);

		System.out.println(a);

	}

	static byte[] toByteArray(int number, int arrayLen) {
		byte[] arrayByte = null;
		arrayByte = new byte[arrayLen];
		int mod = number;
		for (int i = arrayLen - 1; i >= 0; i--) {
			arrayByte[i] = (byte) (mod % 255);
			mod = mod / 255;
		}
		return arrayByte;
	}

	static int toInt(byte[] arrayByte, int arrayLen) {
		int number = 0;
		int mod = 0;

		for (int i = 0; i < arrayLen; i++) {
			mod = (int) arrayByte[i];
			for (int j = 1; j < arrayLen - i; j++) {
				mod *= 255;
			}
			number += mod;
		}
		return number;
	}
}
