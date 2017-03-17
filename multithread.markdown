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

## Semaphore
A counter. When wait, counter-1; when release, counter+1. 

When counter=0, nonsignaled, threads want to wait/acquire on this semaphone paused; when counter larger than 0, signaled.

mutex is a semaphore that counter is only 0 or 1.

a lock can only be locked and unlocked by the same thread. But semaphone can acquired and released by different threads (Java)

## Challenges
- Race
- Deadlock
- starvation
- livelock

## Message Queue
A queue that can block when full or empty, and insert or pull when available. Use condition variable wait to implement. Need use unique ptr to maintain objects, so that threads can access objects without duplicate them.


