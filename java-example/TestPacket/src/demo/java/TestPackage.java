package demo.java;

class Show{
	public String talk(){
		String s="In testPackage";
		return s;
	}
}

public class TestPackage {
	public static void main(String[] args){
		System.out.println(new Show().talk());
	}

}
