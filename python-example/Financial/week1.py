from utils import calculator

# Video 1: Path to Financial Security
# compond growth rate
growth_rate = 0.087

# Ron start saving $2000 per year since his age 21 to 31, and then stop saving more until age 55.
ron_money = 0
for _ in range(21, 31):
    ron_money *= 1 + growth_rate
    ron_money += 2000

for _ in range(31, 55):
    ron_money *= 1 + growth_rate

print("Ron: ", ron_money)

# Jon start saving $2000 per year since age 35.
jon_money = 0
for _ in range(35, 55):
    jon_money *= 1 + growth_rate
    jon_money += 2000

print("Jon: ", jon_money)

# Video 2: Time Value of Money
# Compound value
print("5 year", calculator.calculateCompoundFutureValue(500.0, 3, 5))
print("10 year", calculator.calculateCompoundFutureValue(500.0, 3, 10))
print("save 1k in 5 year", calculator.calculateCompoundPastValue(1000.0, 5, 5))

base = 500
target = 1000
rate = 5
duration = 5
payment = calculator.calculateAnnuityPayment(target, base, rate, duration)
print("payment to save 1k in 5 year", payment)
print("check if the payment can work", calculator.calculateAnnuityFutureValue(base, payment, rate, duration))

# Rate of return
print("Car nominal rate of return", calculator.calculateNominalRateOfReturn(8500, 18500, 16))
print("Car real rate of return", calculator.calculateRealRateOfReturn(8500, 18500, 2, 16))

# Cal when can deposit $250k become $1m
print("How long to Get $1M", calculator.calculateAnnuityDuration(250000, 8, 0, 1000000))

# Cal how much to get
print("How much in 15 years pay at end", calculator.calculateAnnuityFutureValue(10000, 1500, 5.85, 15))
print("How much in 15 years pay at begin", calculator.calculateAnnuityFutureValue(10000, 1500, 5.85, 15, True))

# Rule 72
print("72 to double", calculator.calculateAnnuityDuration(10000, 6.5, 0, 20000, True), "vs.", 72 / 6.5)

print("stock 5 years", calculator.calculateCompoundFutureValue(2000, 8, 5))
print("IRA", calculator.calculateAnnuityFutureValue(0, 150, 10, 40, False))
print("buy a house", calculator.calculateCompoundPastValue(40000, 5, 10))
print("lucrative offer", calculator.calculateCompoundPastValueBasedOnBenefit(15000 * 20, 8, 20)) # No idea what the question is
print("inflation", 25000 - calculator.calculateCompoundFutureValue(25000, -2.5, 2)) # No choice match this value
print("real rate of return", calculator.calculateRealRateOfReturnFromNominalRate(8.5, 3))