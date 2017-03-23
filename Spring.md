## Source
https://spring.io/guides/gs/rest-service/

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

