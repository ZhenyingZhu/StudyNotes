import java.util.*;

public class CollIterator {
	public static void main(String[] args) {
		ArrayList<String> al=new ArrayList<String>();
		al.add("A");
		al.add("B");
		al.add("C");
		System.out.print("Before change: ");
		Iterator<String> itr=al.iterator();
		while(itr.hasNext()){
			Object element=itr.next(); //Notice
			System.out.print(element+" ");			
		}
		System.out.println();
		
		ListIterator<String> litr=al.listIterator();
		while(litr.hasNext()){
			Object element2=litr.next();
			litr.set(element2+"'");
		}
		System.out.print("After change: ");
		Iterator<String> itrc=al.iterator();
		while(itrc.hasNext()){
			Object element3=itrc.next(); //Notice
			System.out.print(element3+" ");			
		}		
	}

}
