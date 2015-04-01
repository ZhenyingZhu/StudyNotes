#! /usr/bin/env python

AMT = 5
sum = 0

list = []
cnt = 1
while cnt <= AMT: 
    input = int( raw_input('input element %d: ' % cnt) )
    list.append(input)
    cnt += 1
    sum += input

# print list

print "average :", float(sum).__div__(AMT)
