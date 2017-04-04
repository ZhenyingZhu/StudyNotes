## Source
https://spring.io/guides/gs/rest-service/

http://www.tutorialspoint.com/spring/

## Prerequist
1. [SDKMAN!](http://sdkman.io/install.html)
`curl -s "https://get.sdkman.io" | bash`

2. [Gradle](https://gradle.org/install)
`sdk install gradle 3.4.1`

3. A build.gradle file

4. [Gradle Wrapper](https://spring.io/guides/gs/gradle/)
`gradle wrapper --gradle-version 3.4.1`

## RESTful Web Service
[src](https://spring.io/guides/gs/rest-service/)

Service interactions:
- response to `GET` for `/greeting`
- return 200 OK, and a JSON

Steps
- Resource class
- resource controller: get a request, return an resource instance
- An executable
- A get request looks like `http://localhost:8080/greeting?name=User`

## Beans autowired
[Bean](https://en.wikipedia.org/wiki/JavaBeans)
- encapsulate many objects into a single object (the bean)
- serializable
- a zero-argument constructor
- allow access to properties using getter and setter methods.
- A bean may register to receive events from other objects and can generate events that are sent to those other objects

[src1](http://www.mkyong.com/spring/spring-auto-wiring-beans-in-xml/)
[src2](https://www.mkyong.com/spring/spring-auto-wiring-beans-with-autowired-annotation/)

## Notations
- `@RequestParam`
- `@RestController`=`@Controller`+`@ResponseBody`
- `@SpringBootApplication`=`@Configuration`+`@EnableAutoConfiguration`+`@EnableWebMvc`+`@ComponentScan`

## Tutorialspoint

### Concept
Dependency Injection (DI): one of concrete example of Inversion of Control (IoC)

Aspect Oriented Programming (AOP)

cross-cutting concerns are conceptually separate from the application's business logic

### Module
Core(IoC, DI), Bean(factory), Context, SpEL

JDBC, ORM(object-relational mapping), OXM(object/XML mapping), JMS(consuming messages), Transaction

Web(Servlet listeners), Web-MVC(Model-View-Controller), Web-Socket, Web-Portlet

AOP(method-interceptors and pointcuts), Aspects(AspectJ), Instrumentation, Messaging(STOMP), Test(JUnit or TestNG frameworks)

### Environment setup
JDK: upadte `PATH` and `JAVA_HOME`

[Apache common logging](https://commons.apache.org/proper/commons-logging/download_logging.cgi): put under "/usr/local/lib/java/commons-logging-1.1.1" and set `CLASSPATH`

[Spring](https://repo.spring.io/release/org/springframework/spring/): unzip dist and place to "/usr/local/", and set `CLASSPATH`

Eclipse: "Properties, Java Build Path, Libraries, Add External JARs" add all jars for spring and commons-logging

### Beans and Configs
Create a beans xml config under `src`. The config glues the beans. It assign ID to beans and control the creation of objects

Use `ClassPathXmlApplicationContext` to create an `ApplicationContext` from this config

Get a bean object from the Context


### Spring Container
Container creates objects(Spring Beans) and wire them together use DI.

The container gets its instructions on what objects to instantiate, configure, and assemble by reading the configuration metadata provided. The configuration metadata can be represented either by XML, Java annotations, or Java code.

Two types:
- Spring BeanFactory Container
- Spring ApplicationContext Container

### Spring Bean
Bean definition contains configuration metadata defines
- How to create a bean
- Bean's lifecycle details
- Bean's dependencies

Properties
- class
- name/id
- scope: singleton, prototype, request, session, global-session
- constructor-arg
- properties
- autowiring mode
- lazy-initialization mode
- initialization method: A callback when all necessary properties have been set by the container. The bean can implement `InitializingBean` or use xml "init-method" property
- destruction method: Same, implement `DisposableBean` or set "destroy-method"


### Lifecycle
JVM grace shutdown: `AbstractApplicationContext::registerShutdownHook()`

Can define attribute `default-init-method` and `default-destroy-method` for beans node

### Post Processors/Callback
Create a bean that implement `BeanPostProcessor`, then it will be called before and after each other bean init.

### Bean definition inheritance
Bean definition contains
- constructor arguments
- property values
- container-specific information such as initialization method, static factory method name

A child bean definition inherits configuration data from a parent definition. The child definition can override some values, or add others. Set `parent` attribute

A bean template
```xml
   <bean id = "beanTeamplate" abstract = "true">
      <property name = "message1" value = "Hello World!"/>
      <property name = "message2" value = "Hello Second World!"/>
      <property name = "message3" value = "Namaste India!"/>
   </bean>
```

### Dependency injection
Application classes should be as independent as possible of other Java classes to increase the possibility to reuse these classes and to test them independently of other classes while unit testing. Dependency Injection (or sometime called wiring) helps in gluing these classes together and at the same time keeping them independent.

In IoC, instead of init an object in another object, init objects in the container and inject objects to the objects that depend on them
- Constructor-based dependency injection: for mandatory dependencies
- Setter-based dependency injection: for optional dependencies.

### Inner beans
```
   <bean id = "outerBean" class = "...">
      <property name = "target">
         <bean id = "innerBean" class = "..."/>
      </property>
   </bean>
```


### Injecting collection
the property of bean can be
- value: primitive data
- ref: object reference
- list
- set
- map
- props: name-value pairs(just like hashmap) where name and values are both strings. Short for properties

Primitive data xml setting
```
<bean id = "..." class = "exampleBean">
   <property name = "email" value = ""/>
   <property name = "email"><null/></property>
</bean>
```

List xml setting
```
<property name = "addressList">
   <list>
      <ref bean = "address1"/>
      <value>Pakistan</value>
   </list>
</property>
```

Map xml setting
```
<property name = "addressMap">
   <map>
      <entry key = "1" value = "INDIA"/>
      <entry key = "two" value-ref = "address1"/>
   </map>
</property>
```

Prop xml setting
```xml
<property name = "addressProp">
   <props>
      <prop key = "one">INDIA</prop>
   </props>
</property>
```

### Auto-Wiring
The Spring container can autowire relationships between collaborating beans without using `<constructor-arg>` and `<property>` elements

autowire mode
- no: need config DI in beans config xml manually
- byName
- byType
- constructor
- autodetect

Limitation
- if still use `constructor-arg` and `property` setting at the same time, there chould be overriding
- cannot autowired primitives, Strings and Classes
- confusing than explicit wiring

### Annotation based config
Annotation injection is performed before XML injection, so latter one override the former one

Enable annotation:
```xml
<beans xmlns = "http://www.springframework.org/schema/beans"
   xmlns:xsi = "http://www.w3.org/2001/XMLSchema-instance"
   xmlns:context = "http://www.springframework.org/schema/context"
   xsi:schemaLocation = "http://www.springframework.org/schema/beans
   http://www.springframework.org/schema/beans/spring-beans-3.0.xsd
   http://www.springframework.org/schema/context
   http://www.springframework.org/schema/context/spring-context-3.0.xsd">

   <context:annotation-config/>
   <!-- bean definitions go here -->

</beans>
```

Annotations
- `@Required`: setter
- `@Autowired`: setter methods, non-setter methods, constructor and properties
- `@Qualifier`: along with `@Autowired` can be used to remove the confusion by specifiying which exact bean will be wired.
- JSR-250 Annotations: `@Resource`, `@PostConstruct` and `@PreDestroy` annotations.

### Java Based Configuration
`@Configuration` before a configuration class and `@Bean` before methods in this configuration class that return classes(create beans where ids are the method names) is same as define a bean in xml

`@import` before config class but after `@configuration`, then the import bean is not needed for instantiating the context.

Define init and destroy method: `@Bean(initMethod = "init", destroyMethod = "cleanup" )`

`@Scope("prototype")` make the bean not singleton


### Event handling
`ApplicationContext` manages the complete life cycle of beans. It publishes certain types of events when loading the beans
- `ContextStartedEvent`: also be raised when call `ConfigurableApplicationContext::start()`
- `ContextStoppedEvent`: the context can restart if it is stopped
- `ContextRefreshedEvent`: also be raised when call `ConfigurableApplicationContext::refresh()`
- `ContextClosedEvent`: cannot restart
- `RequestHandledEvent`: an HTTP request has been serviced

If a bean implements the `ApplicationListener`, then every time an `ApplicationEvent` gets published to the `ApplicationContext`, that bean is notified.

Spring's event handling is single-threaded so if an event is published, until and unless all the receivers get the message, the processes are blocked and the flow will not continue. 

### Custom Events
Create 
- a class, `CustomEvent`, that inherit `ApplicationEvent`: init from an object
- a class, `CustomEventPublisher`, implement `ApplicationEventPublisherAware`: has a `ApplicationEventPublisher` memeber and can publish a `CustomEvent`
- a class implement `ApplicationListener<CustomEvent>`: response to event and do something


### Aspect oriented programming 
[AOP](https://en.wikipedia.org/wiki/Aspect-oriented_programming)

crosscut: if two concerns have some same methods, they are corosscut

cross-cutting concerns: Functions that span multiple points of an app, and are conceptually separate from the application's business logic.

Spring AOP module provides interceptors to intercept an application. For example, when a method is executed, you can add extra functionality before or after the method execution.



