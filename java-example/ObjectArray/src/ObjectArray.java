
class Person{
	String name;
	int age;
//	public Person(){			
//	}
	public Person(String name, int age){
		this.name = name;
		this.age = age;
	}
	public String talk(){
		return "My name is "+this.name+", my age is "+this.age+". ";
	}
}
	
public class ObjectArray {
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		Person p[] = {
				new Person("Aa",10),new Person("Bb",9),new Person("Cc",11)
				};
		for(int i=0; i<p.length; i++){
			System.out.println(p[i].talk());
		}

	}

}
