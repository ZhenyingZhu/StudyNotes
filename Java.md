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
[src](http://tomaszdziurko.com/2012/01/google-guava-eventbus-easy-elegant-publisher-subscriber-cases/)

Listener(receive events, register itself)
- `@Subscribe` on a method which accpect an event, and do filter on it

EventBus
- `register` register the listener
- `post`: post an event to registered listeners

