#!/usr/bin/env python

from threading import Thread
import time

def sleepsec(n): 
    time.sleep(n)

thread1 = Thread(target=sleepsec, args=(15, ))
thread2 = Thread(target=sleepsec, args=(20, ))

thread1.start()
thread2.start()

thread1.join()
thread2.join()

print "WTF"

