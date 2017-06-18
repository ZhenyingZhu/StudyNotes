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


