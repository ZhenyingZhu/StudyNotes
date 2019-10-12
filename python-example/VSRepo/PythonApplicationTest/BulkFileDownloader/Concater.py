import os

diskpath = 'D:/Downloads'
savepath = os.path.join(diskpath, 'test.ts')

with open(savepath, 'wb') as outfile:
    for i in range(0, 255):
        filename = '' + str(i).zfill(3) + '.ts'
        fname = os.path.join(diskpath, 'test', filename)

        with open(fname, 'rb') as infile:
            outfile.write(infile.read())