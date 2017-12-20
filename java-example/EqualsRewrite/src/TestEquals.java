class Person{
	private String name;
	private int age;
	public Person(String s, int n){
		this.name=s;
		this.age=n;
	}
	public boolean equals(Object o){
		boolean temp=true;
		Person p1=this;
		if(o instanceof Person){
			Person p2=(Person)o;
			if(!(p1.name.equals(p2.name)&&p1.age==p2.age))
			temp=false;
		}
		else{temp=false;}
		return temp;
	}
}

public class TestEquals {
	public static void main(String[] args) {
		Person t_p1=new Person("Zhu",22);
		Person t_p2=new Person("Zhu",22);
		System.out.println(t_p1.equals(t_p2)?"True":"Flase");
	}

}
