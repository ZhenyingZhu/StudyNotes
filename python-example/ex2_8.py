#! /usr/bin/env python
AMT = 5

sum = 0
tuple = ()
for i in range(AMT):
    ele = int( raw_input("Input element %d: " % i) )
    sum += ele
    tmp = (ele,)
    tuple += tmp

# print tuple
print sum
