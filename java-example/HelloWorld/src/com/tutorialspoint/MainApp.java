package com.tutorialspoint;

import org.springframework.context.support.AbstractApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

public class MainApp {

	public static void main(String[] args) {
		AbstractApplicationContext context = new ClassPathXmlApplicationContext("Beans.xml");
		
		HelloWorld objA = (HelloWorld) context.getBean("HelloWorld");
		String message = objA.getMessage();
		System.out.println(message);
		objA.setMessage("Changing To ObjA");
		
		HelloWorld objB = (HelloWorld) context.getBean("HelloWorld");
		System.out.println(objB.getMessage());
		
		context.registerShutdownHook();
	}

}
