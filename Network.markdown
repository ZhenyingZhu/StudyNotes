[IptablesHowTo](https://help.ubuntu.com/community/IptablesHowTo)

`sudo iptables -L`

[monit](http://linux.die.net/man/1/monit)
```
monit unmonitor name
monit monitor name
```

[tcpdump](http://www.tcpdump.org/)
```
sudo tcpdump -A 
```

[openstack](http://ask.openstack.org/en/question/28335/you-should-rebuild-using-libgmp-5-to-avoid-timing-attack-vulnerability-_warnnot-using-mpz_powm_sec-you-should-rebuild-using-libgmp-5-to-avoid-timing/)
```
pip --trusted-host pypi.python.org install
```

Add a user to a group
```
useradd -G groupname username
```

[How to find]( http://stackoverflow.com/questions/6495501/find-paths-must-precede-expression-how-do-i-specify-a-recursive-search-that)

`ctrl+alt+numpad` to move the pad 

rpm: 
```
rpm -ql
```

ethtool

```
cat file | xargs
```

```
a='apple' run cmd
```
the enviroment will only exist in the cmd process. 


<code>sudo !!<code> ??? 

### VIM
```
:%s/old/new/g 
```
otherwise only on the current line

[vim search](http://vim.wikia.com/wiki/Search_and_replace>Replease and search tutorial)

```
:reg
```
to see the paste board

### SSH
##### Copy file:
With out ssh authentication:
```
nc host2 12345 &lt big-file
nc -p 12345 > big-file
```

Copy file from B to A while on B: `scp /path/file username@host:/path/destination`
Copy file from B to A while on A: `scp username@host:/path/file /path/destionation`

delete ssh-add keys:
```
ssh-add -D
```
[delete ssh key](http://stackoverflow.com/questions/25464930/how-to-remove-a-ssh-key)

SSH Forward: When ssh to another host, don't need send the keys to there to allow the new host ssh outside.  
[SSH Forward](https://w.amazon.com/index.php/EC2/Security/Howto/Setup_SSH_Credential_Forwarding)
Normal SSH:
```
ssh -i key.pem username@host
```

[ssh](http://injustfiveminutes.com/category/ssh/)
[ssh](http://ubuntuforums.org/showthread.php?t=1932058)
SSH with debug:
```
ssh -v username@host
```

### NetCat
On the listening host, i.e. on the server whose port needs to be checked, do the following:
```
nc -ul 7000
```
On the sending host, do the following - note that servname is the hostname of the listening host:
```
nc -u servname 7000
```

If text typed on the sending host (type something and hit enter) is displayed also on the listening host, then the UDP port 7000 is open.  
[netcat](http://en.wikipedia.org/wiki/Netcat)

### Python
[import module path](http://stackoverflow.com/questions/729583/getting-file-path-of-imported-module)
Compile Python:
1. config
2. sudo make -j8
3. make install

### Email
[email](http://tecadmin.net/ways-to-send-email-from-linux-command-line/)

### Networking
DNS use udp by default. when using tcp, send to many traffic.  

