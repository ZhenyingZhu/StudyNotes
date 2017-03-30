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

Can define `default-init-method` and `default-destroy-method` for beans node

### Post Processors/Callback


