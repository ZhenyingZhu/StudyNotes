# Multi-threading

## CC189

Implement interface `Runnable`

- This interface has a `void run()`
- Create a runnable class and instanize it: `MyRunnable instance = new MyRunnable();`
- In the `main()`, create a `Thread` object: `Thread thread = new Thread(instance);`
- `thread.start();`
- Advantage beyond the next solution is that the class can inherit other classes

Extend the `Thread` class

- override the `void run()` method
- In the `main()`, create an instance: `MyThread instance = new MyThread();`
- Start: `instance.start();`

`synchronized` method

- a keyword that can be applied to methods and code blocks
- restricts multiple threads from executing the code simultaneously on the same object
- static methods are synchronize on the class lock, i.e. no two static synchronized methods on the same class can be called simultaneously

`Lock`

- In the resource class, define a property: `private Lock lock;`
- In the ctor, init the lock: `lock = new ReentrantLock();`
- In the method, at the begining, `lock.lock(); try { ... }`, at the end, `finally { lock.unlock(); }`. Also `lock.tryLock()`

Deadlock conditions

1. Mutual exclusion: a resource has limit access
2. Hold and wait: processes wait before relinquishing their current resources
3. No preemption: one process cannot remove another's resource
4. Circular wait

Measure context switch time

- Need to make sure P2 is started immediately after P1 is ended: use data token to let two processes play ping-pong
- [block vs wait](https://stackoverflow.com/questions/15680422/difference-between-wait-and-blocked-thread-states)
- [wait and notify and notifyAll](https://www.baeldung.com/java-wait-notify)
  - Need to be used in a method with `synchronized` keyword, so they are also based on monitor lock, so they also locked on the same object.
  - The example here can be used to solve the context switch question.

Solve Deadlock for Dining Philosophers

- Release resource if cannot lock.
- break the cycle by letting the last philosopher pick up chopsticks in a reverse order

Detect deadlock

- Need processes declare upfront their lock resources orders
- In a process, between each two resources, draw a vector from the prev to next resource, so that we can draw a graph
- check if there is a cycle

**HERE**: P190

## Old Notes

### Challenges

Source: EPI Chapter 20 intro

- Race
- Deadlock
- starvation
- livelock

Source: CTCI v5 Chapter 16

### Deadlock

4 condition

- mutual exclusion
- hold and wait
- no preemption
- circular wait

### Process vs Thread

Source: CTCI v5 Chapter 16.1

Process is an instance of a program, and an entity of system resources. Need use files, pipes and sockets to communiate with other processes.

A thread is an execution path in a process

### Resource

Each thread has its own stack, but share heap

Thread within a same process share the same memory space

### thread blocking

Every synchronization primitive has a waiting list associated with it. When the resource is not available, the requesting thread will be moved from the running list of processor to the waiting list of the synchronization primitive. When the resource is available, the higher priority thread on the waiting list gets the resource. [src](http://www.geeksforgeeks.org/mutex-vs-semaphore/)

### Mutex and Semaphore

Semaphore has a counter for number of available access. When wait/acquire, counter-1; when release, counter+1.

When counter=0, nonsignaled, threads want to wait/acquire on this semaphore paused; when counter larger than 0, signaled.

Mutex: mutual exclusion semaphore, is a lock. Counter can only be 0 or 1. Used for protect a resource. Should acquire and release by the same thread. [src](http://stackoverflow.com/questions/62814/difference-between-binary-semaphore-and-mutex)

Binary Semaphore: Used for notification. A thread can take the semaphore, and wait until another thead release the semaphore, then continue work. Use case: producer-consumer

A lock can only be locked and unlocked by the same thread. But semaphone can acquired and released by different threads (Java)

#### recursive mutex

As any mutex, it has only lock/unlock state. But it can be locked multiple times, and need be unlocked as many times as it is locked. [src](http://www.geeksforgeeks.org/mutex-vs-semaphore/)

#### critical section

Disable interrupt to achieve atomic access, as mutex does. But normally mutex is expensive.

### Message Queue

A queue that can block when full or empty, and insert or pull when available. Use condition variable wait to implement. Need use unique ptr to maintain objects, so that threads can access objects without duplicate them.

### Thread pool

[src](http://tutorials.jenkov.com/java-concurrency/thread-pools.html)

A size n array full of threads. A job queue shared by all threads. Threads waiting for queue being filled, then pull one job and execute.

### Readers writers problem

Source EPI 20.6,7, [wiki](https://en.wikipedia.org/wiki/Readers%E2%80%93writers_problem)

When read, there could be someone writing, leads to a out-of-date read. So need protect the data.

1. lock the resource when read and write. Too much because read doesn't change the resource
2. Reader perference: create a reader cnt, and read lock and write lock. Every time when start read, lock read lock, increaste the cnt, unlock read lock, and read, and lock read lock, decrease the cnt after read, and unlock read lock; when write, lock write lock, wait reader notified and check if reader cnt is 0, then write and unlock write lock.
3. Writer perference: create a write lock. When read, first get the lock, then release and read; when write, first get the lock, then write and release

## CPP programming

[More details](./CPP.md)

`thread`

- constructor: 1. a function, 2. an instance of a class that implement `void operator()()`
- `t.join()` or `t.detach()`
- `std::thread t(f, a, ref(b))` to create a thead with a function `void f(int a, int &b)`

`mutex`

- [src](http://www.cplusplus.com/reference/mutex/mutex/lock/)
- `mutex::lock()` and `mutex::unlock()`: try to lock the mutex, otherwise block the thread until the mutex is unlocked. dangerous because it could be not unlock when exception happen.
- `mutex::try_lock()`: if the mutex is locked by another thread, return fail not block. If the mutex is locked by the same thread, it create a deadlock, use recursive mutex instead.
- a class `recursive_mutex`: only the same thread can lock the mutex several times
- a class `unique_lock<mutex> lck(mtx, std::defer_lock);` is safer than just call `mutex::lock()`. It locked mtx on constructor, and unlock on destructor.
- a class `std::lock_guard<mutex> lck(mtx);`: lock mtx on constructor, and unlock it on destructor. It can only used to lock and unlock in the same code block.

`lock` function (not mutex membership `mutex.lock()`)

- `std::lock(mtx1, mtx2, ...)`: lock all the mutex thread-safely

lock type, can be used as `lock_guard` and `unique_lock` constructor

- `defer_lock`: not lock the mutex on constructor
- `adopt_lock`: use exsiting lock
- [example](http://en.cppreference.com/w/cpp/thread/lock_tag)

`condition_variable`

- block threads until be notified
- `wait(unique_lock<mutex>& lck)`: 1. `lck.unlock();`, 2. if be notified, `lck.lock();`, 3. notice `unique_lock` lck will unlock itself in the destructor. Normally put into a while loop waiting for some condition to become false.
- `wait_for(lck, time)`: when time duration time out or notified, move on. Return `cv_status::timeout` or `cv_status::no_timeout`
- `wait_until(lck, time)`: time is a time point not duration
- `notify_all()`, `notify_one()`: unlock threads/thread that are/is waiting for this condition variable

`std::future`

`std::async`

## Python programming

### Sogou Crawler

Crawler: a message queue, a thread pool, an API.

CrawlerThread: the thread to put into the pool. They share the same queue.

[Why use hashlib.md5().update](https://www.techopedia.com/definition/4024/message-digest)
