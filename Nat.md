## background
https://en.wikipedia.org/wiki/Network_address_translation

One to one NAT:
- the IP addresses, IP header checksum and any higher level checksums that include the IP address are changed

One-to-many NAT
- multiple private hosts to one publicly exposed IP address
- When a reply returns to the router, it uses the connection tracking data it stored during the outbound phase to determine the private address on the internal network to which to forward the reply.

NAPT
- for TCP and UDP, the port numbers are changed so that the combination of IP address and port information on the returned packet can be unambiguously mapped to the corresponding private network destination


## Commands
Show ip tables
https://www.cyberciti.biz/faq/howto-iptables-show-nat-rules/

https://www.karlrupp.net/en/computer/nat_tutorial
```
sudo iptables -t nat -A POSTROUTING -o eth0 -j MASQUERADE -s 172.32.0.0/16 -d 0.0.0.0/0
```

Or use negative ip: `sudo iptables -A POSTROUTING ! -d 192.168.0.13 -o eth0 -j MASQUERADE`

