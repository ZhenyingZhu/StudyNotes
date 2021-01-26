#!/usr/bin/python

# set temperature for the simulation
T = 300.0
V = 3.14159265358*8.48*8.48*78.7043762
# 0.001ps=1fs
dt = 0.0005
# correlation length
p = 200000
# sample interval
s = 5
# dump interval
d = p*s

# [J/K] Boltzmann
kb = 1.3806504e-23
ev2J = 1.60e-19
A2m = 1.0e-10
ps2s = 1.0e-12
convert = ev2J*ev2J/ps2s/A2m

scale = convert/kb/T/T/V*s*dt

line = ''
s1 = 0
s2 = 0
s3 = 0
m  =   int(line.split()[0])
t  =   int(line.split()[1])
nc =   int(line.split()[2])
xx = float(line.split()[3])
yy = float(line.split()[4])
zz = float(line.split()[5])
s1 += xx
s2 += yy
s3 += zz
s4 = (s1+s2+s3)/3.0

time = 1
with open('output_kt') as file_keto:
  file_keto.write('%4i '      %time)
  file_keto.write('%10.5f '   %(s1*scale))
  file_keto.write('%10.5f '   %(s2*scale))
  file_keto.write('%10.5f '   %(s3*scale))
  file_keto.write('%10.5f \n' %(s4*scale))

with open('output_km') as file_kimo:
  file_kimo.write('%4i '      %(s*m))
  file_kimo.write('%4i '      %t)
  file_kimo.write('%4i '      %nc)
  file_kimo.write('%10.5f '   %(s1*scale))
  file_kimo.write('%10.5f '   %(s2*scale))
  file_kimo.write('%10.5f '   %(s3*scale))
  file_kimo.write('%10.5f \n' %(s4*scale))
