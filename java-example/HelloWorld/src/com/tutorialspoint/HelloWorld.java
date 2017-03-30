package com.tutorialspoint;

public class HelloWorld {
	private String message;
	
	public void setMessage(String message) {
		this.message = message;
	}
	
	public String getMessage() {
		System.out.println("Hello " + message);
		return message;
	}
	
	public void init(){
		System.out.println("HelloWorld Bean is going through init.");
	}
	
	public void destroy() {
		System.out.println("HelloWorld Bean will destroy now.");
	}
}
