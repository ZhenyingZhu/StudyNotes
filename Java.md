## Start
[How to execute a java class](http://stackoverflow.com/questions/18093928/what-does-could-not-find-or-load-main-class-mean)
1. `java [ <option> ... ] <class-name> [<argument> ...]`
  - class name includes package name
  - execute in the root folder, which is where contains com folder
  - example: `java -Xmx100m com.acme.example.ListUsers fred joe bert`
(NOT DONE)

## Gramma

[`...`](http://stackoverflow.com/questions/11640507/difference-between-arrays-and-3-dots-in-java)

[`->`](http://stackoverflow.com/questions/15146052/what-does-the-arrow-operator-do-in-java)


## Lambda
Syntax: `(args1, args2, args3, ...) -> { body }`

When there is a single parameter, no need to use brace and class name.

Can only work with a class that has only one or less abstract method. 

Functional interface: an interface with only one abstract method. `@FunctionalInterface`

[src](https://blog.idrsolutions.com/2015/02/java-8-method-references-explained-5-minutes/)
method reference can all methods in Lambda. Also in Lambda you can use variables from outter place.

## Default method
[src](https://blog.idrsolutions.com/2015/01/java-8-default-methods-explained-5-minutes/)

In an interface, implement some methods to make it work with lambda

```
interface InterfaceA {
    public void foo1(); // lambda implement it

    default public void foo2() { // inherit class can use it, or implement their own
        // do something
    }
}
```

If a class implement from two interfaces that have default methods with same name, the class needs to override it
```
@Override
public void overlapDefaultMethod {
    InterfaceA.super.overlapDefaultMethod();
}
```

## Interface
### Predicate
A function interface that has `test` abstract method.

### Function

### Future
[src](https://docs.oracle.com/javase/7/docs/api/java/util/concurrent/Future.html)
- `get` can have a timeout
- `cancel`
- `isDone`
- `isCancelled`

### Stream
[src](https://blog.idrsolutions.com/2014/11/java-8-streams-explained-5-minutes/)

- intermediate operations are lazy evaluate. Only execute when terminal op is called.
  - `filter(Predicate)`: create a new stream
  - `map(a lambda exp)`: lambda create a class instance of `Function` which act as a mapper (for map-reduce)
  - `sorted()`
- terminal operation
  - `forEach(a lambda exp)`: lambda create a class instance of `Consumer`

`filter()` and `map()` are stateless, `sorted()` is stateful.

`Predicate` has a abstract function `test`


### Supplier and Consumer
[src](https://blog.idrsolutions.com/2015/03/java-8-consumer-supplier-explained-in-5-minutes/)

[src](https://dzone.com/articles/supplier-interface)  
A Supplier `Supplier<? extends Vehicle> supplier` can be passed to a function and `supplier.get()` return an instance or an inherit instance of `Vehicle` 

Supplier:
- is a `FunctionalInterface`
- has abstract method `T get()`

Consumer:
- is a `FunctionalInterface`
- abstract method `accept`, default `andThen` makes it work with stream
- can use a static function to init it: `Comsumer comsumer = foo;`
- assignment target for a lambda expression or method reference


### Executor interface
decoupling task submission from the mechanics of how each task will be run, including details of thread use, scheduling, etc.

- `execute`: handle how to run, and how to schedule next runnable task

### ExecutorService interface
[src](https://docs.oracle.com/javase/7/docs/api/java/util/concurrent/ExecutorService.html)
It can be used as thread pool. Can be shutdown
- `submit`: args is a `Callable<T>` instance, return a `Future`
- `execute`: inherit from `Executor`
- `shutdown`
- `invokeAll`: return a list of `Future`

Init: `private final ExecutorService pool = Executors.newFixedThreadPool(poolSize);`

### ScheduledExecutorService interface
can schedule commands to run after a given delay, or to execute periodically.
- `schedule`: args are a callable, a delay and a time unit. Return `ScheduledFuture`

init: `private final ScheduledExecutorService scheduler = Executors.newScheduledThreadPool(1);`

### AbstractExecutorService
Implementation of ExecutorService

### ThreadPoolExecutor
extend `AbstractExecutorService`

executes each submitted task using one of possibly several pooled threads

Address two problems:
- improved performance when executing large numbers of asynchronous tasks
- provide a means of bounding and managing the resources, including threads, consumed when executing a collection of tasks

Use exiting methods to init
- `Executors.newCachedThreadPool()`
- `Executors.newFixedThreadPool(int)`
- ` Executors.newSingleThreadExecutor()`

Init args
- corePoolSize, maximumPoolSize
- ThreadFactory: create new thread. By default `Executors.defaultThreadFactory()`
- keepAliveTime: how long a thread can be idle before be terminated

### ScheduledThreadPoolExecutor
extends ThreadPoolExecutor and implements ScheduledExecutorService

Delayed tasks execute no sooner than they are enabled, but without any real-time guarantees. Tasks scheduled for exactly the same execution time are enabled in first-in-first-out (FIFO) order of submission.

### EventBus
[src1](http://tomaszdziurko.com/2012/01/google-guava-eventbus-easy-elegant-publisher-subscriber-cases/)
[src2](http://blog.webagesolutions.com/archives/1027)

Listener/Subscriber(receive events, register itself)
- `@Subscribe` on a method which accpect an event, and do filter on it
- It is a callback method
- return void

EventBus
- `register` register the listener
- `post`: post an event to registered listeners

Event
- The default one is `DeadEvent`

[src](http://codingjunkie.net/guava-eventbus/)

The EventBus allows for objects to subscribe for or publish events, without having explicit knowledge of each other.

- The EventBus will dispatch all events serially, so it is important to keep the event handling methods lightweight
- If you need to do heavier processing in the event handlers, AsyncEventBus. It takes an ExecutorService as a constructor argument to allow for asynchronous dispatching of events

Event listener: take some action when a desired event occurs

Event subscriber: `@Subscribe` on a method which does something when event occurs. It needs to be register to event bus. `@AllowConcurrentEvents` on it makes it threadsafe when use AsyncEventBus

`eventBus.post(evnet)` will post events to all subscribers of this event.

`EventBus eBus =  new AsyncEventBus(java.util.concurrent.Executors.newCachedThreadPool());`



# Junit
[Create a Junit test](https://www.tutorialspoint.com/junit/junit_suite_test.htm):
1. A TestRunner `Result result = JUnitCore.runClasses(JunitTestSuite.class);`
2. A JunitTestSuite `@Suite.SuiteClasses({Test1.class, Test2.class})`
3. public Test classes `@Test`

`ArgumentCaptor`

`when`

`verify`

`@Rule`: [src](https://carlosbecker.com/posts/junit-rules/)



# JMX
Java Dynamic Management Kit: Monitor resources represented by Managed Beans (MBeans)
- Probe/Instrumentation level: MBeans, needs to reg to MBean server
- Agent level: MBeanServer. core of JMX
- Remote management level: enable remote apps to access MBeanServer through connectors and adaptors. JMX Connectors like JConsole

[src](http://www.journaldev.com/1352/what-is-jmx-mbean-jconsole-tutorial)
1. Create a MBean
2. Create MBeanServer use `MBeanServer mbs = ManagementFactory.getPlatformMBeanServer();`
3. reg `mbs.registerMBean(mBean, objectName);`


[Client](https://docs.oracle.com/javase/tutorial/jmx/remote/custom.html)


