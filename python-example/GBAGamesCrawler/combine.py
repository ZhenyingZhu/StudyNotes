import csv

fout=open("out.csv","a")
for num in range(1,91):
    try:
        f = open(str(num)+".htm.csv")
        for line in f:
            fout.write(line)
        f.close() # not really needed
    except:
        continue

fout.close()