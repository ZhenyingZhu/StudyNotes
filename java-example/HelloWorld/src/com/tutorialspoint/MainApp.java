package com.tutorialspoint;

import org.springframework.context.support.AbstractApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

public class MainApp {

	public static void main(String[] args) {
		AbstractApplicationContext context = new ClassPathXmlApplicationContext("Beans.xml");
		
//		HelloWorld objA = (HelloWorld) context.getBean("HelloWorld");
//		String message1 = objA.getMessage1();
//		System.out.println(message1);
//		objA.setMessage1("Changing To ObjA");
//		
//		HelloWorld objB = (HelloWorld) context.getBean("HelloWorld");
//		System.out.println(objB.getMessage1());
//
//		GoodbyeWorld objC = (GoodbyeWorld) context.getBean("GoodbyeWorld");
//		objC.getMessage1();
//		objC.getMessage2();
//		objC.getMessage3();
		
		context.start();

//		JavaCollection javaCollection = (JavaCollection) context.getBean("JavaCollection");
//		javaCollection.getAddressList();
//		javaCollection.getAddressSet();
//		javaCollection.getAddressMap();
//		javaCollection.getAddressProp();
		
//		GoodbyeWorld goodbyeWorld = (GoodbyeWorld) context.getBean("GoodbyeWorld");
//		goodbyeWorld.getSubGoodWorld().printMessage();
		
		AutowiredB autowiredB = (AutowiredB) context.getBean("AutowiredBBean");
		System.out.println(autowiredB.getName());
		System.out.println(autowiredB.getAutowiredAName());
		
		context.stop();

		context.registerShutdownHook();
	}

}
