balance = 999999
annualInterestRate = 0.18

lo = balance / 12.0
hi = (balance * (1+annualInterestRate/12.0)**12) / 12

while True: 
    fixedPayment = (lo + hi) / 2.0
    remainBalance = balance
    
    for i in range(12): 
        remainBalance = (remainBalance - fixedPayment) * (1 + annualInterestRate/12.0)
    
    if abs(remainBalance) < 0.01: 
        break
    elif remainBalance < 0:
        hi = fixedPayment
    else: 
        lo = fixedPayment

print "Lowest Payment: " + str(round(fixedPayment, 2))