package hello;

/* The following is an example about SpringBoot. It works with hello.rest to create an RESTful server
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class Application {

    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }
}
*/

import hello.beans.SpringSessionContextFactory;
import org.springframework.context.support.AbstractApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

public class Application {

    public static void main(String[] args) {
        AbstractApplicationContext context = new ClassPathXmlApplicationContext("application.xml");

//        SpringSessionContextFactory springSessionContextFactory = (SpringSessionContextFactory) context.getBean("SpringSessionContextFactory");
//        springSessionContextFactory.newContext("test");


    }
}
