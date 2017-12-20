package org.lxh.enumdemo.demo;

public class EnumDemo {
	public static void main(String[] args){
		Color c=Color.RED;
		System.out.println(c);
		switch(c){
		case RED:{
			System.out.println("apple");
			break;
		}
		case GREEN:{
			System.out.println("lime");
			break;
		}
		case BLUE:{
			System.out.println("blood");
			break;
		}
		default:{
			System.out.println("unknown");
		}


		}
	}

}
