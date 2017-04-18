## Gramma

[`...`](http://stackoverflow.com/questions/11640507/difference-between-arrays-and-3-dots-in-java)

[`->`](http://stackoverflow.com/questions/15146052/what-does-the-arrow-operator-do-in-java)


## Interface
### Predicate

### Consumer

### Function

### Supplier
[src](https://dzone.com/articles/supplier-interface)

A Supplier `Supplier<? extends Vehicle> supplier` can be passed to a function and `supplier.get()` return an instance or an inherit instance of `Vehicle` 

### Future
[src](https://docs.oracle.com/javase/7/docs/api/java/util/concurrent/Future.html)
- `get` can have a timeout
- `cancel`
- `isDone`
- `isCancelled`

### ExecutorService
[src](https://docs.oracle.com/javase/7/docs/api/java/util/concurrent/ExecutorService.html)
- `submit`: args is a `Callable<T>` instance.
- `execute`: inherit from `Executor`

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


