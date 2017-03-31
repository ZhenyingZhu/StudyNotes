package com.tutorialspoint;

public class GoodbyeWorld {
	private String message1;
	private String message2;
	private String message3;
	
	public void setMessage1(String message) {
		this.message1 = message;
	}

	public void setMessage2(String message) {
		this.message2 = message;
	}
	
	public void setMessage3(String message) {
		this.message3 = message;
	}
	
	public void getMessage1() {
		System.out.println("Goodbye " + message1);
	}
	
	public void getMessage2() {
		System.out.println("Goodbye " + message2);
	}
	
	public void getMessage3() {
		System.out.println("Goodbye " + message3);
	}
	
	public void init(){
		System.out.println("GoodbyeWorld Bean is going through init.");
	}
	
	public void destroy() {
		System.out.println("GoodbyeWorld Bean will destroy now.");
	}
	
}
