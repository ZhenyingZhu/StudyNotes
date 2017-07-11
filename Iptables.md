## Summary
/usr/sbin/iptables 
iptables applies to IPv4, ip6tables to IPv6, arptables to ARP, and ebtables to Ethernet frames  
Use kernel module `x_tables`.  
In the future use nftables instead.  

## Concepts
Use tables, chain, rules to treat packets.  

- table: a kind of packet processing.  
- chain: packet ar eprocessed by traversing rules in chains. Based on the origin of packets.   
- rule: cause a goto or jump to another chain.  

### Chain
Predefined chains (map to Netfilter hooks):  
- PREROUTING: packets enter before routing desicion made.  
- INPUT: packets locally delivered. Don't need open sockets. Routing table: `ip route show table local`.  
- FORWARD: routed packets. 
- OUTPUT: send out from the machine.  
- POSTROUTING: routing desicion has been made. Packets enter before go to hardware.  

Chain has a default policy like DROP or ACCEPT, which is used when all rules are not appliable.  
If a packet reach the end of a chain, or the rule calls RETURN target, it goes back to the previous chain.  

### Rule
Rules: 
- target or verdict define decision: `ACCPET`, `DROP`, `RETURN`, `CONTINUE`.   
- prot define protocol
- source and destination filter packages.  
- specify the condition to check packets: `--mac-source`, `-p tcp --dport parameters`, `-m time`


## Front-end
show iptables:  
```
Chain INPUT (policy ACCEPT)
target     prot opt source               destination         
ACCEPT     all  --  localhost            anywhere            
ACCEPT     all  --  10.0.0.0/24          anywhere            
ACCEPT     tcp  --  anywhere             anywhere             state RELATED,ESTABLISHED
LOGNEW     tcp  --  anywhere             anywhere             tcp dpts:tcpmux:17471

Chain FORWARD (policy ACCEPT)
target     prot opt source               destination         

Chain OUTPUT (policy ACCEPT)
target     prot opt source               destination         

Chain LOGNEW (1 references)
target     prot opt source               destination         
LOG        all  --  anywhere             anywhere             limit: avg 1/sec burst 5 LOG level error prefix " INBOUND TCP "
ACCEPT     all  --  anywhere             anywhere            
```

show more information:  
```
zhenyinz@uc4346b7191ad54db70fc:~/GitHub/StudyNotes$ sudo iptables-save -c >rules
*filter
:INPUT ACCEPT [1031492:170813312]
:FORWARD ACCEPT [0:0]
:OUTPUT ACCEPT [7216242:3017325377]
:LOGNEW - [0:0]
[101260:14322418] -A INPUT -s 127.0.0.1/32 -j ACCEPT
[906:44527] -A INPUT -s 10.0.0.0/24 -j ACCEPT
[0:0] -A INPUT -s 10.0.0.1/24 -j ACCEPT
[7006321:3685742906] -A INPUT -p tcp -m state --state RELATED,ESTABLISHED -j ACCEPT
[13:676] -A INPUT -p tcp -m tcp --dport 1:17471 -j LOGNEW
[13:676] -A LOGNEW -m limit --limit 1/sec -j LOG --log-prefix " INBOUND TCP " --log-level 3
[13:676] -A LOGNEW -j ACCEPT
COMMIT
```

## Flow
https://unix.stackexchange.com/questions/189905/how-iptables-tables-and-chains-are-traversed

https://en.wikipedia.org/wiki/Netfilter

upload.wikimedia.org/wikipedia/commons/3/37/Netfilter-packet-flow.svg

http://www.iptables.info/en/structure-of-iptables.html

A packet hits
1. Firewall
2. the proper device driver in kernel
3. iptables
4. either local app or forward to another host

Dist local host
1. raw PREROUTING: before connection tracking
2. conntrack
3. mangle PREROUTING: change packets IP header
4. nat PREROUTING: DNAT. Don't filter in this chain
5. routing decision: local host or forward
6. mangle INPUT: after routed, but before send to the process on the machine
7. filter INPUT: all packets dest for this host no matter which interface or which direction

Source local host
1. routing decision: What source address to use, what outgoing interface to use
2. raw OUTPUT
3. conntrack
4. mangle OUTPUT
5. nat OUTPUT: for NAT
6. routing decision: route based on mangle and nat changes
7. filter OUTPUT
8. mangle POSTROUTING: mangle packets for both packets hit the firewall and packets created by firewall
9. nat POSTROUTING: SNAT. Don't do filtering here.

Forwarded: Table 6-3

mangle table
- TOS
- TTL
- MARK: add marks that recognized by `iproute2`. Can do bandwidth limiting and Class Based Queuing based on these marks.
- SECMARK: marks for SELinux
- CONNSECMARK: similar to SECMARK

nat table
- only the first packet in a stream will hit this table. After this, the rest of the packets will automatically have the same action
- DNAT: ingress
- SNAT: egress
- MASQUERADE: similiar to SNAT, but do more to support DHCP
- REDIRECT

raw table
- set a mark on packets to make them not handled by conntrack
- using the NOTRACK target on the packet
- PREROUTING
- OUTPUT
- the `iptable_raw` module must be loaded. It will be loaded automatically if `iptables` is run with the `-t raw`

filter table
- DROP
- ACCEPT

user specified chains
- can specify a jump rule to a different chain within the same table
- new chain must be userspecified
- only built in chains can have a default policy
- user chain can add a jump rule at the end that jump back

[conntrack](https://access.redhat.com/documentation/en-US/Red_Hat_Enterprise_Linux/6/html/Security_Guide/sect-Security_Guide-Firewalls-IPTables_and_Connection_Tracking.html)
- connection tracking: store info about incoming connections. States: NEW, ESTABLISHED, RELATED, INVALID

[Masquerade](http://www.tldp.org/HOWTO/IP-Masquerade-HOWTO/ipmasq-background2.1.html)
- similar to 1-to-many NAT. If a host connect to internet, internal computers can connect to this host and then connect to the internet



www.csie.ntu.edu.tw/~b93070/CNL/v4.0/CNLv4.0.files/Page697.htm

