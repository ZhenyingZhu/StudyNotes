package com.tutorialspoint;

public class HelloWorld {
	private String message1;
	private String message2;
	
	public void setMessage1(String message) {
		this.message1 = message;
	}

	public void setMessage2(String message) {
		this.message2 = message;
	}

	public String getMessage1() {
		System.out.println("Hello " + message1);
		return message1;
	}
	
	public String getMessage2() {
		System.out.println("Hello " + message2);
		return message2;
	}

	public void init(){
		System.out.println("HelloWorld Bean is going through init.");
	}
	
	public void destroy() {
		System.out.println("HelloWorld Bean will destroy now.");
	}
	
}
