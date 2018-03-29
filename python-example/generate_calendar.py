import sys

i = 1
d = 4

while i <= 399:
    if d == 5:
        print ""
    else:
        for j in range(4):
            sys.stdout.write(str(i) + ',')
            i = i + 1
        print ""

    d = d + 1
    if d == 8:
        d = 1

