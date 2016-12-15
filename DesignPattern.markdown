

### Producer-consumer
[Producer-consumer problem in Python](http://agiliq.com/blog/2013/10/producer-consumer-problem-in-python/)

consumer:
- acquire condition
- if queue empty, condition wait, which release the lock in the condition, and block waiting. Else goto get item
- get condition notification, wake up
- when the condition is released by producer, continue
- get item
- release the condition

producer:
- acquire condition
- put item in the queue
- notify condition
- release condition

If need put a max size on the queue, make producer wait until consumer notify on the same condition


