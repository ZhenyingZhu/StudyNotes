input = int( raw_input("Please input a number between 1 and 100: ") )

while (input < 1 or input > 100): 
    input = int( raw_input("%d is not between 1 and 100\nPlease input again: " % input) )

print "%d is between 1 and 100, congratulation! " % input
