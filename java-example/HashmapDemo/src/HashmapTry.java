import java.util.*;
import java.util.Map.Entry;


public class HashmapTry {
	public static void main(String[] args){
		HashMap<String, Double> hm=new HashMap<String, Double>();
		hm.put("John", new Double(34.3));
		hm.put("Tom", new Double(45.6));
		Set<Entry<String, Double>> set=hm.entrySet();
		Iterator<Entry<String, Double>> i=set.iterator();
		while(i.hasNext()){
			Map.Entry me=(Map.Entry)i.next();
			System.out.print(me.getKey()+" ");
			System.out.println(me.getValue());
		}
		System.out.println();
		double balance=((Double)hm.get("John")).doubleValue();
		hm.put("John",new Double(balance+10));
		System.out.println("After change");
		System.out.println("John "+hm.get("John"));
	}

}
