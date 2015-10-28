print("Please think of a number between 0 and 100!")
low = 0
high = 100

check = "" 
while check != "c": 
    ans = (high + low) / 2
    print "Is your secret number", 
    print ans, 
    print "?"
    
    check = raw_input("Enter 'h' to indicate the guess is too high. Enter 'l' to indicate the guess is too low. Enter 'c' to indicate I guessed correctly. ")
    if check == "c": 
        continue
    elif check == "h": 
        high = ans
    elif check == "l": 
        low = ans
    else: 
        print "Sorry, I did not understand your input." 

print "Game over. Your secret number was:", 
print ans