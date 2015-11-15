balance = 4842
annualInterestRate = 0.2
monthlyPaymentRate = 0.04

totalPayment = 0.0
for i in range(1, 13): 
    print "Month: " + str(i)
    minimumPayment = round(balance * monthlyPaymentRate, 2)
    totalPayment += minimumPayment
    print "Minimum monthly payment: " + str(minimumPayment)
    balance = round((balance-minimumPayment) * (1+annualInterestRate/12.0), 2)
    print "Remaining balance: " + str(balance)

print "Total paid: " + str(totalPayment)
print "Remaining balance: " + str(balance)