#! /usr/bin/env python

info = "1. SUM of 5 numbers\n2. AVG of 5 numbers\n3. exit\n\nPlease make a choice: "
choice = raw_input(info)

if choice == '1': 
    import ex2_8
elif choice == '2': 
    import ex2_9
elif choice == '3':
    print "Exit"
else: 
    print "Unknown commend, exit"
