from threading import Thread, Condition
import time
import random

queue = []
condition = Condition()
MAX_NUM = 3

class ProducerThread(Thread):
    def run(self):
        nums = range(5)
        global queue
        while True:
            num = random.choice(nums)
            condition.acquire()
            if len(queue) == MAX_NUM:
                print "Queue full"
                condition.wait()
                print "Producer wake up"
            queue.append(num)
            print "Produced", num
            condition.notify()
            condition.release()
            time.sleep(random.random()) # sleep 0.0 to 1.0


class ConsumerThread(Thread):
    def run(self):
        global queue
        while True:
            condition.acquire()
            if not queue:
                print "Queue empty"
                condition.wait()
                print "Consumer wake up"
            num = queue.pop()
            print "Consumed", num
            condition.notify()
            condition.release()
            time.sleep(random.random())


ProducerThread().start()
ConsumerThread().start()

