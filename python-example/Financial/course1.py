# compond growth rate
growth_rate = 0.087

# Ron start saving $2000 per year since his age 21 to 31, and then stop saving more until age 55.
ron_money = 0
for i in range(21, 31):
    ron_money *= 1 + growth_rate
    ron_money += 2000

for i in range(31, 55):
    ron_money *= 1 + growth_rate

print("Ron: ", ron_money)

# Jon start saving $2000 per year since age 35.
jon_money = 0
for i in range(35, 55):
    jon_money *= 1 + growth_rate
    jon_money += 2000

print("Jon: ", jon_money)