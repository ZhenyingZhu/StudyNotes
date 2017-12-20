package tryAPI;

public class ArrayString {
	public static void main(String[] args) {
		String nowString = "Hello World";
		// Change String to Array and output.
		char[] charArray = nowString.toCharArray();
		for (int i = 0; i < charArray.length; i++) {			
			System.out.print(charArray[i]);
			//System.out.print((int)charArray[i]);
		}
		System.out.println();
		for (char c : charArray) { // for array				
			System.out.print(c);			
		}
		System.out.println();
		System.out.println(charArray);
		// Change Array to String.
		StringBuffer aBuffer = new StringBuffer();
		for (int i = 0; i < charArray.length; i++) {
			aBuffer.append(charArray[i]);
		}
		String backToString = aBuffer.toString();
		System.out.println(backToString);		
	}

}
