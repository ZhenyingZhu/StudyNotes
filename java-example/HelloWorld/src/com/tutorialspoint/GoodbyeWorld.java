package com.tutorialspoint;

public class GoodbyeWorld {
	private String message;
	
	public void setMessage(String message) {
		this.message = message;
	}
	
	public String getMessage() {
		System.out.println("Goodbye " + message);
		return message;
	}
}
