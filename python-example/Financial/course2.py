from utils import maths

print("5 year", maths.compound(500.0, 3, 5))
print("10 year", maths.compound(500.0, 3, 10))
print("save 1k in 5 year", maths.discount(1000.0, 5, 5))

base = 500
target = 1000
rate = 5
duration = 5
payment = maths.backward_annuity(target, base, rate, duration)
print("payment to save 1k in 5 year", payment)
print("check if the payment can work", maths.annuity(base, payment, rate, duration))
