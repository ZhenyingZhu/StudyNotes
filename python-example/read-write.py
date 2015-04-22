
f = open('old', "r")
status = f.readline()
f.close()

f = open('old', "a")
r = open('addition', "r")
for line in r:
    f.write(line)
f.close()

