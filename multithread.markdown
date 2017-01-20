## Deadlock
4 condition
- mutual exclusion
- hold and wait
- no preemption
- circular wait

## Resource
Each thread has its own stack, but share heap

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
(!!Review!!)