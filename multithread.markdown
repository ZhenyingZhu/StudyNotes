## Deadlock
Source: CTCI v5 Chapter 16

4 condition
- mutual exclusion
- hold and wait
- no preemption
- circular wait

## Process vs Thread
Source: CTCI v5 Chapter 16.1

Process is an instance of a program, and an entity of system resources. Need use files, pipes and sockets to communiate with other processes.

A thread is an execution path in a process

## Resource
Each thread has its own stack, but share heap

Thread within a same process share the same memory space

### thread blocking
Every synchronization primitive has a waiting list associated with it. When the resource is not available, the requesting thread will be moved from the running list of processor to the waiting list of the synchronization primitive. When the resource is available, the higher priority thread on the waiting list gets the resource. [src](http://www.geeksforgeeks.org/mutex-vs-semaphore/)

## Mutex and Semaphore
Semaphore has a counter for number of available access. When wait/acquire, counter-1; when release, counter+1.

When counter=0, nonsignaled, threads want to wait/acquire on this semaphore paused; when counter larger than 0, signaled.

Mutex: mutual exclusion semaphore, is a lock. Counter can only be 0 or 1. Used for protect a resource. Should acquire and release by the same thread. [src](http://stackoverflow.com/questions/62814/difference-between-binary-semaphore-and-mutex)

Binary Semaphore: Used for notification. A thread can take the semaphore, and wait until another thead release the semaphore, then continue work. Use case: producer-consumer

A lock can only be locked and unlocked by the same thread. But semaphone can acquired and released by different threads (Java)

### recursive mutex
As any mutex, it has only lock/unlock state. But it can be locked multiple times, and need be unlocked as many times as it is locked. [src](http://www.geeksforgeeks.org/mutex-vs-semaphore/)

### critical section
Disable interrupt to achieve atomic access, as mutex does. But normally mutex is expensive.

## Challenges
- Race
- Deadlock
- starvation
- livelock

## Message Queue
A queue that can block when full or empty, and insert or pull when available. Use condition variable wait to implement. Need use unique ptr to maintain objects, so that threads can access objects without duplicate them.

## CPP programming
[More details](https://github.com/ZhenyingZhu/StudyNotes/blob/master/CPP.markdown)

thread
- constructor: 1. a function, 2. an instance of a class that implement `void operator()()`
- `t.join()` or `t.detach()`
- `std::thread t(f, a, ref(b))` to create a thead with a function `void f(int a, int &b)`

mutex
- `lock()` and `unlock()`
- a class `std::lock_guard<mutex> 

- `std::future`
- `std::async`

