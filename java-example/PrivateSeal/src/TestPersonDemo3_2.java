class Person{
	private String name;
	private int age;
	void talk(){
		System.out.println("My name is "+name+". I am "+age+" years old. ");
	}
	public void setName(String str){
		name = str;
	}
	public void setAge(int num){
		if (num>0) age = num;
		else System.out.println("Are you kidding? ");
	}
	public String getName(){
		return name;
	}
	public int getAge(){
		return age;
	}
}
public class TestPersonDemo3_2 {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		Person p = new Person();
		p.setName("AAA");
		p.setAge(-12);
		p.talk();
	}
}
