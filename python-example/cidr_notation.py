#!/usr/bin/python2

import sys
from socket import inet_aton

USAGE = 'usage: {0} ipaddr netmask\n'.format(sys.argv[0])

def get_net_size(netmask):
    binary_str = ''
    for octet in netmask:
        binary_str += bin(int(octet))[2:].zfill(8)
    return str(len(binary_str.rstrip('0')))

if len(sys.argv) != 3:
    sys.stderr.write(USAGE)
    sys.exit(1)

# validate input
try:
    inet_aton(sys.argv[1])
    inet_aton(sys.argv[2])
except:
    sys.stderr.write('IP address or netmask invalid\n')
    sys.stderr.write(USAGE)
    sys.exit(2)

ipaddr = sys.argv[1].split('.')
netmask = sys.argv[2].split('.')

# calculate network start
net_start = [str(int(ipaddr[x]) & int(netmask[x]))
             for x in range(0,4)]

# print CIDR notation
print '.'.join(net_start) + '/' + get_net_size(netmask)
