Use aws cli filter:http://serverfault.com/questions/560337/search-ec2-instance-by-its-name-from-aws-command-line-tool    
```
aws ec2 describe-subnets --filters 'Name=vpc-id, Values=vpc-xxxxxxxx'
```

awk: `ls -l | awk 'NR>=122 && NR<=129 { print }'`  http://unix.stackexchange.com/questions/89640/how-to-run-awk-for-some-number-of-lines  

Python singleton:  
```
    def __init__(self):
        if self.__initialized:
            return

```

Python difference between dict["key"] and dict.get("key"): http://stackoverflow.com/questions/7631929/python-dictionary-datastructure-which-method-d-or-d-get  

git submodules: https://devcenter.heroku.com/articles/git-submodules  

git tag: https://git-scm.com/book/en/v2/Git-Basics-Tagging  

python: pyc files won't update after git branch switch or checkout. So need to checkout again a clean repo and rerun.  

git diff --no-color -U100000 reversion1 reversion2 -- /path > diff.patch  

C++ length: http://stackoverflow.com/questions/589575/what-does-the-c-standard-state-the-size-of-int-long-type-to-be  

Mix bash history:  http://askubuntu.com/questions/80371/bash-history-handling-with-multiple-terminals   

python Lambda: http://stackoverflow.com/questions/10668282/one-liner-to-check-if-at-least-one-item-in-list-exists-in-another-list  

http://stackoverflow.com/questions/8902206/subprocess-popen-io-redirect  

How to read source code:  
http://www.zhihu.com/question/19637879  


http://stackoverflow.com/questions/11968689/python-multithreading-wait-till-all-threads-finished  

http://stackoverflow.com/questions/1783405/checkout-remote-git-branch  

`clinked_insts = { c.id: c for c in config.ec2.get_all_classic_link_instances(instance_ids=instance_ids) }` change map to list.  

git delete a branch:  http://stackoverflow.com/questions/2003505/delete-a-git-branch-both-locally-and-remotely  

`svn checkout url://repository/path@1234` or `svn checkout -r 1234 url://repository/path` svn checkout a specific reversion.  

svn conflict solve: http://stackoverflow.com/questions/2113430/purpose-of-the-mine-full-and-theirs-full-commands  

create UML graph from codes: http://bouml.fr/features.html  

under /var/run/xxx.pid record pid of xxx, which should be same as `pgrep xxx`  

`sort -kn` start from the nth character.   


```
zhenyinz@uc4346b7191ad54db70fc:~$ strace ./hello.sh 
execve("./hello.sh", ["./hello.sh"], [/* 39 vars */]) = -1 EACCES (Permission denied)
dup(2)                                  = 3
fcntl(3, F_GETFL)                       = 0x8002 (flags O_RDWR|O_LARGEFILE)
fstat(3, {st_mode=S_IFCHR|0620, st_rdev=makedev(136, 4), ...}) = 0
mmap(NULL, 4096, PROT_READ|PROT_WRITE, MAP_PRIVATE|MAP_ANONYMOUS, -1, 0) = 0x7f9511ae0000
lseek(3, 0, SEEK_CUR)                   = -1 ESPIPE (Illegal seek)
write(3, "strace: exec: Permission denied\n", 32strace: exec: Permission denied
) = 32
close(3)                                = 0
munmap(0x7f9511ae0000, 4096)            = 0
exit_group(1)                           = ?

``` 


`lsmod` show Modules that are loaded.  

`last reboot` see the last several reboot time.  

shell read line by line: http://stackoverflow.com/questions/1521462/looping-through-the-content-of-a-file-in-bash  

grep exclude: https://coderwall.com/p/7nylkw/exclude-directory-from-grep-e-g-git-svn  

`git submodule update --init`  https://devcenter.heroku.com/articles/git-submodules    

`pip freeze` show list.  http://stackoverflow.com/questions/6600878/find-all-packages-installed-with-easy-install-pip  

`git checkout commit#` to go back to a commit, and `git checkout branch_name` to go back to head.  http://stackoverflow.com/questions/10228760/fix-a-git-detached-head  


http://stackoverflow.com/questions/4114095/revert-to-a-previous-git-commit  

increase bash scroll history: EDIT->Profile Perference->Scrolling   

*.cpp doesn't need `#IFNDEF` at all*.  


pickle module: http://stackoverflow.com/questions/1939058/simple-example-of-use-of-setstate-and-getstate  


Multi threads use same heap of memory, while multi process use different heap of memory.  
Multi process for vpc combined  
https://docs.python.org/2/library/multiprocessing.html  

Best Python IDE: pycharm.  
Add an import path: File -> Settings -> Project:[Project Name] -> Project Structure -> Add Content Root -> Source Folder


```
git stash save
git pull -- rebase
git stash pop
```

win7 set up FTP: (http://blog.sina.com.cn/s/blog_3f7e47f20100haur.html) 
- Control Panel -> Windows Features: check 1. Internet Information Services/FTP Server, 2. Internet Information Services/Web Management Tools/IIS Management Console.    
- Control Panel -> System and Security -> Administrative Tools -> Internet Information Services (IIS) Manager: Sites: add FTP Site  
- name: demo, Physical Path: Download  
- IP address: 10.0.0.1. SSL: no. 
- Authorize.  

C++ on eclispe: http://stackoverflow.com/questions/4971926/launch-failed-binary-not-found-cdt-on-eclipse-helios

install eclispe on ubuntu and config: http://difusal.blogspot.com/2015/06/how-to-install-eclipse-mars-45-on-ubuntu.html  

`ssh apple uptime` can show the execute a cmd like uptime and return the result   

`git reset origin/master` to make all commit gone but file change left.  
http://makandracards.com/makandra/527-squash-several-git-commits-into-a-single-commit  

`DISPLAY=:0.0 zenity --warning --text="Hello"`  show a popup message  

`echo "Hello"|wall` show a message to every users' terminal   

`diff < (cat file1) < (cat file2)`  

`sudo monit summary`  

to see the largest file: 
```
sudo du -sx /* 2>/dev/null | sort -n| tail
```

see which device a folder on
```
df /
```

`iptables -nxvL` shows all tables  

```
yum clean all # avoid rpm pkg checksum error
```

```
rpm -ev --nodeps akg # remove a package without dependency check
```

```
diff -ur --exclude=".svn" folder1 folder2 > diff.patch
cd folder1
patch -p1 < ../diff.patch # 1 means sub folder is in
```

svn suck: http://askubuntu.com/questions/21237/problems-with-subversion-in-gnome-keyring-maybe-user-null  

```
svn import -m "Adding just a file" file_name http://path/to/svn/repo/file_name
```

```
vi /etc/fstab
remote-location local-location cifs uid=USER,credentials=~/.smbcredentials,iocharset=utf8,sec=ntlm 0 0 
vi ~/.smbcredentials
username=
passwd=
```

alais ls='ls --color=auto'

Ruby thread: http://stackoverflow.com/questions/9095316/handling-exceptions-raised-in-a-ruby-thread  

P99: 99th percentile. https://en.wikipedia.org/wiki/Percentile  

ruby: if string contain \n, it will also put out like this way. So need `\` to esape it.  

`grep`: http://www.shellhacks.com/en/Using-BASH-Grep-OR-Grep-AND-Grep-NOT-Operators  

`atd`: http://www.korske.com/technology/atd000000-file-is-in-wrong-format-aborting.html  
      https://en.wikipedia.org/wiki/At_%28Unix%29  

`yum clean all` to avoid unexpected error  

`common` find the new added lines in newer file compare to an older file.  

```
rpm -qa | grep -i package
rpm -e package-name #uninstall
```

`ls -LR` list all files
`lsmod`: show modules.  

RPM: http://www.tummy.com/blogs/2007/10/31/getting-rpm-to-list-packages-by-install-date/  

`curl -O URL` can save the file.  

to see the usage of memory `top`

http://lukaszwrobel.pl/blog/tmux-tutorial-split-terminal-windows-easily  

http://askubuntu.com/questions/167468/where-can-i-find-the-source-code-of-ubuntu  
`http://bazaar.launchpad.net/~ubuntu-branches/ubuntu/wily/iputils/wily/view/head:/ping.c`

`env` `history`  

http://packetbomb.com/how-can-the-packet-size-be-greater-than-the-mtu/  

Scapy tur: http://www.secdev.org/projects/scapy/doc/usage.html  

`ip link show eth0`  
`sudo ip link set dev eth0 mtu 1500`  
`tracepath www.dest.com` PMTUD  

http://www.webupd8.org/2014/09/dual-boot-fix-time-differences-between.html  

Are the usb ports onboard devices or are the usbs part of a PCI card? Some usb PCI have this problem. type, #sudo lsusb, to get a listing of all usb ports on your system. Also type, sudo lshw -short to get a short listing all devices on you system. If neither program shows you the usbs on the system then you are using a different kernel or the drives did not get install. Type, uname -a, and give us the kernel version. If you do see the usbs, then type, sudo lspci -v, and see if drives have been installed for them. - See more at: http://www.linux.com/learn/answers/view/762-i-just-loaded-ubuntu-and-it-doesnt-recognize-the-usb-ports-what-do-i-do#sthash.bg370RMi.dpuf  

```
go to http://steamcommunity.com/minigame/towerattack/
than(in google chrome) press F12 and then click Console.
There you can write one of the commands like"for(i=4000;i<4723;i++){JoinGame(i);}"
```

http://stackoverflow.com/questions/252703/python-append-vs-extend  

`dmesg` check system ring  

refer to https://github.com/mojombo/mojombo.github.io/tree/master/_posts  

http://stackoverflow.com/questions/4304438/gem-install-failed-to-build-gem-native-extension-cant-find-header-files  
sudo yum install libxml2-devel 
sudo yum install nodejs 

ask.fedoraproject.org/en/question/9769/sofia-is-not-in-the-sudoers-file/  

http://apple.stackexchange.com/questions/88598/how-to-have-sshd-re-read-its-config-file-without-killing-ssh-connections  

http://askubuntu.com/questions/369389/how-do-i-check-that-a-configuration-file-has-been-read  

https://ask.fedoraproject.org/en/question/9623/terminal-key-shortcut/  

http://askubuntu.com/questions/131168/how-do-i-uninstall-grub

https://help.ubuntu.com/community/Grub2/Installing  

`arping`: send arping request.  

`bootp` protocol: DHCP?  

`dig` DNS lookup service.  
http://dougblack.io/words/a-good-vimrc.html  

http://stackoverflow.com/questions/16222738/how-do-i-install-ruby-2-0-0-correctly-on-ubuntu-12-04  

[Vundle](https://github.com/gmarik/Vundle.vim)  
```
Plugin 'scrooloose/nerdtree'
Plugin 'godlygeek/tabular'
Plugin 'plasticboy/vim-markdown'
```
```
:so ~/.vimrc
:PluginInstall
```

[vim plugin](http://joelhooks.com/blog/2013/04/23/5-essential-vim-plugins/)  

http://searchnetworking.techtarget.com/definition/Port-Address-Translation-PAT  

http://stackoverflow.com/questions/6541109/send-string-to-stdin  

http://sc.tamu.edu/help/general/unix/redirection.html  
http://linuxcommand.org/lts0060.php  

ruby: `str[//]` to execute regular express.  

http://askubuntu.com/questions/526082/2-usb-ports-stopped-working  

http://stackoverflow.com/questions/12309269/write-json-data-to-file-in-python  

`du` diskusage.  

[ssh disable timeout](https://docs.oseems.com/general/application/ssh/disable-timeout)  

[Xfinity](http://customer.xfinity.com/help-and-support/internet/wireless-gateway-username-and-password?ts=2&CCT=53BA3D76CB1473BFF49C79FE4AA86DFF1EE2DE626F409A5997049C511025D7B2204A7E8531C277E2BCBE6703AA1E9BA0ED1CCE9EEACAF2E341918A8498DFEA031067DB3AB0550914C343562D66B6C738E6B3B5DD54EE00895D57EEDDAF943A63D722FEA4741C2635BFA54500D1EBC9A31BB35ED9541A9164590253CE54D95ED9   )

http://android.stackexchange.com/questions/2984/how-can-i-see-what-ip-address-my-android-phone-has  

http://stackoverflow.com/questions/4370960/what-is-attr-accessor-in-ruby  

http://stackoverflow.com/questions/151505/difference-between-a-class-and-a-module  

Port 0 is reserved.  

http://stackoverflow.com/questions/18193424/using-gsub-in-ruby-strings-correctly  
`dirname = File.basename(Dir.getwd)` find the current dir name in ruby  

[net bean extension](http://stackoverflow.com/questions/4134137/syntax-highlighting-php-shell-scripts-without-extension-in-netbeans)  
~/.netbeans/8.0.2/config/Services/MIMEResolver  


```
from subprocess import call
call("ls") # success
call("ls -l") # failed
call("ls .") # failed
call(["ls", "-l"]) # success
```

[Ruby XML](http://www.tutorialspoint.com/ruby/ruby_xml_xslt.htm)  
http://stackoverflow.com/questions/11315295/ruby-finding-elements-with-rexml  
http://www.germane-software.com/software/rexml/docs/tutorial.html  

[netstat](http://www.cyberciti.biz/faq/how-do-i-find-out-what-ports-are-listeningopen-on-my-linuxfreebsd-server/)  
[background](http://www.kossboss.com/linux---move-running-to-process-nohup)  
killall nc
Ctrl+z and fg
service lightdm stop to stop X windows.
who -b

sh-bang: 可用指令<code>env</code>，如#! /usr/bin/env python。

http://serverfault.com/questions/416222/concatenate-first-line-to-the-end-of-second-line-in-a-text-file

<code>grep -f Key.txt All.txt</code>
<code>sleep 60</code> to sleep 60 seconds.
<code>mail -s "test script" account@mail.com < /dev/null</code> send an email with title "test script".
<code>head -q -n 2 *.txt</code> Output several files without show the file name.* 
<code>sed -e 'N;s/\(.*\)n\(.*\)/\1\2/' </code> let the second lines append to the first lines. 

```
while read line; do
    SOMETHING
done < readfile
```

SSH:
```
eval `ssh-agent -s`
ssh-add ~/.ssh/id_rsa
ssh -A $HOST
```

Vim:
<code> set paste</code> and <code> set nopaste</code>

https://docs.python.org/2/library/subprocess.html  
http://effbot.org/zone/python-with-statement.htm  

[hping3](http://0daysecurity.com/articles/hping3_examples.html)  

`sudo yum -y --enablerepo="epel" install hping3`  

`$ dpkg -l | grep python`  

[DF flag](http://yurisk.info/2009/09/01/ping-setting-dont-fragment-bit-in-linuxfreebsdsolarisciscojuniper/)  
[wireshark](http://anonscm.debian.org/viewvc/collab-maint/ext-maint/wireshark/trunk/debian/README.Debian?view=markup)  

[Understand TCP](http://packetlife.net/blog/2010/jun/7/understanding-tcp-sequence-acknowledgment-numbers/)  

[nuttcp](http://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=1&ved=0CB8QFjAA&url=http%3A%2F%2Fwww.nuttcp.net%2F&ei=sHhKVbmcAcX8oATbkoCACw&usg=AFQjCNE2ezCkj69-haln6xb3TvnffAbx0w&bvm=bv.92291466,d.cGU)  and http://manpages.ubuntu.com/manpages/precise/man8/nuttcp.8.html  

http://lukaszwrobel.pl/blog/tmux-tutorial-split-terminal-windows-easily  

[split terminal](http://ubuntuforums.org/showthread.php?t=1813803)  

http://stackoverflow.com/questions/7188191/copy-file-over-tcp-socket-in-ruby-slow  

http://stackoverflow.com/questions/1661367/specify-packet-size-with-ruby-tcpsocket  

http://www.linuxquestions.org/questions/linux-networking-3/gigabit-ethernet-performance-643750/  

[web browser](http://www.tutorialspoint.com/ruby/ruby_socket_programming.htm)  

```
dd if=/dev/zero of=binary.dat bs=1c count=1 
dd if=/dev/zero oflag=append conv=notrunc of=binary.dat bs=1c count=1 #To append it to file
```

https://iperf.fr/#tuningtcp  

[TCP MTU](http://blogs.technet.com/b/onthewire/archive/2014/06/18/checking-your-tcp-packets-are-pulling-their-weight-tcp-max-segment-size-or-mss.aspx)  

[tcpdump](http://www.thegeekstuff.com/2010/08/tcpdump-command-examples/)

[eclipse](http://stackoverflow.com/questions/11596194/how-does-one-show-trailing-whitespace-in-eclipse)  

transport file (http://www.netperf.org/netperf/training/Netperf.html#0.2.2Z141Z1.SUJSTF.6R2DBD.2) 
```
netserver
netperf 127.0.0.1 -t udp_rr -- -r 1473,1473 -u

netperf -H remotehost -t UDP_STREAM -- -m 1024 # test udp
netperf -H remotehost -t TCP_RR

```

[python **args](http://www.saltycrane.com/blog/2008/01/how-to-use-args-and-kwargs-in-python/)  

[tcpdump](https://danielmiessler.com/study/tcpdump/)  

`iperf -s` on server and `iperf -c host` on client  

soft reboot: `sudo reboot`  

if `sudo -u user` failed, try `sudo logbash` and in the new bash, run that again.  

[find which shell](http://www.cyberciti.biz/tips/how-do-i-find-out-what-shell-im-using.html)  

[sudo -u](http://apple.stackexchange.com/questions/82438/allow-sudo-to-another-user-without-password)  

[shell wait background](http://unix.stackexchange.com/questions/124106/shell-script-wait-for-background-command)  

[MTU size](http://www.cyberciti.biz/faq/centos-rhel-redhat-fedora-debian-linux-mtu-size/)

[rename remote branch](http://www.benjaminlhaas.com/blog/locally-and-remotely-renaming-branch-git)  

[pull from fork](http://stackoverflow.com/questions/14383212/git-pulling-a-branch-from-another-repository)
```
git remote add fork *.git
git fetch fork
git checkout -b personal_branch fork/branch
```


[rebase](http://git-scm.com/book/en/v2/Git-Branching-Rebasing)

[change master branch](http://stackoverflow.com/questions/2862590/how-to-replace-master-branch-in-git-entirely-from-another-branch)
[branches](http://gistflow.com/posts/909-how-to-replace-the-master-branch-on-git)  

[Jumbo](http://en.wikipedia.org/wiki/Jumbo_frame)
[test](http://www.mylesgray.com/hardware/test-jumbo-frames-working/)
see MTU: ifconfig  
Need ICMP port open to access ping.  
http://serverfault.com/questions/234311/testing-whether-jumbo-frames-are-actually-working  

Add in the last line of a plain text file.  
```
# vim: set ts=4 sw=4 sts=4 et:
```

`dig` DNS lookup  
`set -e` ? 
`set -x` show all steps when executes a command

(EINTR)[http://250bpm.com/blog:12]  

http://stackoverflow.com/questions/1434451/what-does-connection-reset-by-peer-mean  

python utc time: 
```
from datetime import datetime
datetime.utcnow()
```
http://stackoverflow.com/questions/15940280/utc-time-in-python  

time format output
```
time.strftime('%Y-%m-%d %H:%M:%S UTC')
```
http://stackoverflow.com/questions/3961581/in-python-how-to-display-current-time-in-readable-format  

`/usr/bin/file`: output the message of a file.  

Wrapper: ensure have the right path and enviroment.  
Output of programs should goes to /var/your-program/xxx.yaml
Put first execute file into bin. Modules into lib  

ruby enviroment for eclipse https://eclipse.org/dltk/install.php

[pgrep](http://linux.die.net/man/1/pgrep)
search processes  

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

Switch: MAC bridge, connect devices together.   
Router: send packages.   
Internet Protocol address version 4: 32-bit number=FF.FF.FF.FF.   
IPv6:128 bits.   
CIDR: Classless internet domain routing   
DHCP: Dynamic host configuration protocol   
VIF: virtual interface. Protocol layer. Has network address.   
ASIC: application specific integrated circuit.   
RPM: RPM package manager for install software in Linux.   
Metric: The set of properties of a router.   
NAT: Network address translation   
Scope: The main feature of a product.   
Wrapper: a subroutine in a software library or a computer program whose main purpose is to call a second subroutine. a system call with little or no additional computation. To make the call of updated package much easier.   
verbose: debug or not.   
Subnet: a range of IP address.   
Security Group: a white list that allow which protocol(TCP or UDP) can access which port(e.g. SSH 22) from where(e.g. 0.0.0.0/0 or another SG). It is stateful.   
Stateless: treat every requests as independent transaction. Like HTTP.   
Stateful: create an interactive session with user. Like FTP.   
MIS: Management Information System  
Open Systems Interconnection model (OSI Model): 
1. physical layer: transmittion medium. 
2. data link layer: node to node. MAC. 
3. network layer: datagrams(variable length data sequences). Routing. IP is in internet layer.  
4. transport layer: while transmit, how to maintain quailty. TCP(on top of IP). 
5. session layer: dialogues between computers. Full/Half-duplex. 
6. presentation layer: syntax and semantics for app layer to present. 
7. application layer: show to end user. 

TCP/IP model: 
1. Link layer: how host attached. Hardware independent. 
2. Internet layer: routing, IP protocol. 
3. Transport layer: establish a basic data channel. TCP or UDP. 
4. Application layer: HTTP, FTP, SMTP, DHCP. 

IRQ: interrupt request.   
p55 latency: 55% of the targets should be faster than the given latency.  
SLA: service-level agreement. where a service is formally defined. Particular aspects of the service - scope, quality, responsibilities - are agreed between the service provider and the service user.  core dumps: record working state.  

# MTU
In IP protocol, layer 3.  
Hosts know own MTU, but don't know the smallest MTU on the path.  
IPv4 allows Fragmentation. 

The term datagram is often considered synonymous to packet but there are some nuances. The term datagram is generally reserved for packets of an unreliable service, which cannot notify the sender if delivery fails, while the term packet applies to any packet, reliable or not. Datagrams are the IP packets that provide a quick and unreliable service like UDP, and all IP packets are datagrams;[4] however, at the TCP layer what is termed a TCP segment is the sometimes necessary IP fragmentation of a datagram,[5] but those are referred to as "packets".[6]
