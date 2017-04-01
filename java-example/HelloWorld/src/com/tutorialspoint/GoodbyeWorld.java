package com.tutorialspoint;

public class GoodbyeWorld {
	private String message1;
	private String message2;
	private String message3;
	private SubGoodWorld subGoodWorld;
	
	public void setMessage1(String message) {
		this.message1 = message;
	}

	public void setMessage2(String message) {
		this.message2 = message;
	}
	
	public void setMessage3(String message) {
		this.message3 = message;
	}

	public void setSubGoodWorld(SubGoodWorld subGoodWorld) {
		this.subGoodWorld = subGoodWorld;
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

	public SubGoodWorld getSubGoodWorld() {
		return subGoodWorld;
	}

	public void init(){
		System.out.println("GoodbyeWorld Bean is going through init.");
	}
	
	public void destroy() {
		System.out.println("GoodbyeWorld Bean will destroy now.");
	}
	
}
