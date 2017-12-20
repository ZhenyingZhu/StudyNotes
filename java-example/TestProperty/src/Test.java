
public class Test {
static String a = "S-a";
static String b;
String c = "S-c";
String d;
static{
	printStatic("Before Static");
	b="S-b";
	printStatic("After Static");
}
public static void printStatic(String title){
	System.out.println(a);
	System.out.println(b);
}
public Test(){
	print("before constructor");
	d = "S-d";
	print("after constructor");
}
public void print(String title){
	System.out.println(a);
	System.out.println(b);
	System.out.println(c);
	System.out.println(d);
}
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		new Test();

	}

}
