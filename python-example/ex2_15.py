#! /usr/bin/env python

def swap(var1, var2): 
    tmp = var1
    var1 = var2
    var2 = tmp
    return var1, var2

def main():
    """Order three input number"""
    a = int( raw_input("Input var a: ") )
    b = int( raw_input("Input var b: ") )
    c = int( raw_input("Input var c: ") )
    
    if a > b: 
        a, b = swap(a, b)

    if b > c:
        b, c = swap(b, c)

    if a > b: 
        a, b = swap(a, b)

    print "From small to big:\n  %d < %d < %d" % (a, b, c)

if __name__ == "__main__": 
    main()

