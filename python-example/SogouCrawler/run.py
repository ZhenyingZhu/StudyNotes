import threading
import time


def func(name, delay):
    count = 0
    while count < 3:
        count += 1
        time.sleep(delay)
        print name, time.time(), count


class TestThread(threading.Thread):

    def __init__(self, name, delay):
        threading.Thread.__init__(self)
        self.name = name
        self.delay = delay

    def run(self):
        func(self.name, self.delay)


thread1 = TestThread('linpz-1', 2)
thread2 = TestThread('linpz-2', 3)

thread1.start()
thread2.start()

thread1.join()
thread2.join()

with open('filename', 'r') as f:
    f.read()

file = open()




