package com.tutorialspoint;

import org.springframework.beans.factory.annotation.Autowired;

public class AutowiredB {
	private String name;
	private AutowiredA autowiredA;
	
	@Autowired
	public AutowiredB(AutowiredA autowiredA) {
		name = "AutowiredB inited in AutowiredB";
		//autowiredA = new AutowiredA("AutowiredA which is inited in AutowiredB");
		this.autowiredA = autowiredA;
	}
	
	public void setName(String name) {
		this.name = name;
	}
	
	public String getName() {
		return name;
	}

	public void setAutowiredA(AutowiredA autowiredA) {
		this.autowiredA = autowiredA;
	}
	
	public String getAutowiredAName() {
		return autowiredA.getName();
	}
}
