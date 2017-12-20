class Person{
	String name;
	private static final Person p = new Person();
	private Person(){
		name = "AAA";
	}
	public static Person getP(){
		return p;
	}
}
public class TestSingleDemo2 {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		Person p = null;
		p = Person.getP();
		System.out.println(p.name);
	}
}
