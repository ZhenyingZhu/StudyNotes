balance = 320000
annualInterestRate = 0.2

fixedPayment = 0
while True: 
    remainBalance = balance
    
    for i in range(12): 
        remainBalance = (remainBalance - fixedPayment) * (1 + annualInterestRate/12.0)
    
    if remainBalance <= 0.0: 
        break
    else: 
        fixedPayment += 10

print "Lowest Payment: " + str(fixedPayment)