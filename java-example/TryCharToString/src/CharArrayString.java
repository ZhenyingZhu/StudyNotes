import java.util.Arrays;


public class CharArrayString {
	public static void main(String[] args){
		String a="Hello World";
		char[] b=a.toCharArray();
		Arrays.sort(b);
		String f=new String(b);
		System.out.println(f);
	}

}
