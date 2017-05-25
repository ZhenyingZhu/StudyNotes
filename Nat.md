Show ip tables
https://www.cyberciti.biz/faq/howto-iptables-show-nat-rules/

https://www.karlrupp.net/en/computer/nat_tutorial
sudo iptables -t nat -A POSTROUTING -o eth0 -j MASQUERADE -s 172.32.0.0/16 -d 0.0.0.0/0

Or use negative ip: sudo iptables -A POSTROUTING ! -d 192.168.0.13 -o eth0 -j MASQUERADE
