
## Chapter 0
计算机五个部分:输入单元、 输出单元、CPU内部的控制单元、算数逻辑单元与主存储器  

容量1GB=1024^3，速度1GHz=1000^3  

Intel主板上的芯片组:
- 北桥:负责链接速度较快的CPU、主存储器和显示适配器等组件，为系统总线，速度FSB，总线带宽FSB×总线宽度:每秒可传输最大数据量
- 南桥:负责连接速度较慢的周边接口如硬盘，为I/O总线；  
AMD主板:无南北桥，将内存控制组件整合到CPU中。  
不同主板支持CPU不同。  

CPU种类:精简指令集 (RISC) ，复杂指令集 (CISC) 系统。  
CPU频率:外频:与外部各组件传输数据的速度；外频×倍频=主频。Word size:32/64位，CPU每次能处理的数据量。  

内存:DRAM，断电丢失。外频与CPU的外频相同时最佳。  
DDR:一次工作周期内2次数据传输。  
二级缓存:L2 cache，CPU内部，为SRAM。  

Firmware: 如BIOS，ROM/EEPROM内。  

PCI:适配卡。  

显卡:VGA，内有3D加速芯片GPU。PCI-Express是新的显卡规格，带宽速度大。全彩的每个像素占用3Bytes容量。显存32MB以上。  

IDE/SATA:磁盘，台式机3.5 inch，手提2.5寸。上有缓冲存储器。  
Sector 512B, 组成圆环track, 不同磁盘上同一位置的track组成Cylinder，分割磁盘最小单位。  

网卡:Ethernet规格。  
网络头:RJ-45。8M/1M ADSL传输速度=1Mbyte/s的上传和125Kbytes/s的下载。如Realtek 的RTL8139。  

I/O地址:硬件门牌。  

IRQ:中断。  

电源:最大500W。20/24pin接口。  

- ASCII: 英文编码表，1byte，256种。  
- 中文Big5，2bytes。  
- Unicode:因特网用通用编码。  

OS Kernel:驱动硬件。在内存中受保护，并且常驻。核心功能如下:  
- System call:OS提供，用以开发软件的接口。  
- Process control，Memory management, Filesystem management, Device drivers。  

查阅组件型号  
- `cat /proc/cpuinfo`
- `lspci`  


## Chapter 1
GNU重要软件:
- Emacs
- GCC
- glibc
- Bash shell  

GUI:XFree86的X Window System。  

Assembly Language:汇编语言。  

POSIX:规范核心与应用程序之间的接口。  

Linux 版本号:偶数为稳定版。  
Linux distribution:Kernel + Softwares + Tool  

Apache:网页服务器。  
Postfix/sendmail:电子邮件服务器。  
Samba:文件服务器。  

LSB:Linux开发标准。  
FHS:目录架构标准。  

安装方式:不同Distribution的主要区别，分RPM和dpkg，Tarball原始码。  

Linux可以多人同时在线。  

GUI:X Window，KDE，GNOME。  

查看核心版本:
- `uname -r`: 可查看Distribution版本
- `lsb_release -a` 可查看Linux Standard Base版本。  


## Chapter 2
网络服务器:WWW, Mail Server, File Server。 

Cluster:云计算机平行运算能力。  

Pidgin:实时通讯软件。 

各个Distribution: 
- Ubuntu
- OpenSuSE
- Fedora
- Mandriva

软件列表: 
- Open Office
- Free Maid:组织结构绘制软件。 
- AbiWord  
- Tex/LaTeX
- Dia:类似Visio。 
- Scribus
- GanttProject:时程表绘制。
- GIMP

基础概念:使用者/群组，权限，程序。

文书编辑器vi:会被很多软件调用。

Shell:文字接口软件。有正则表示法，管线命令，数据流重导向。Shell scripts也重要。  

FAQ和How-To:安装软件的帮助文档放在`/usr/share/doc/`下，或http://www.linux.org.tw/CLDP/或http://www.tldp.org/  

提示的网络服务错误信息，可到`/var/log/`里查看。  
How To: http://www.tldp.org  


## Chapter 3
图形接口运算:X Window内的Open GL。  

RAID:多个磁盘接成阵列。  

硬件配置在linux下都是档案。 
- IDE: `/dev/hd[a-d]`
- SCSI, SATA, USB: `/dev/sd[a-p]`
- 软盘驱动器: `/dev/fd[0-1]`
- 打印机: `/dev/lp[0-15]`
- 鼠标: `/dev/usb/mouse[0-15]` 或`/dev/psaux`
- DVD: `/dev/cdrom/`
- 网卡:/dev/eth[0-n]。

磁盘分区:Partition在windows下为C, D, E，Linux下SATA按侦查到的顺序分配sda，sdb。  
磁盘第一个扇区，记录:

1. Master Boot Record (MBR) ，安装开机管理程序。
2. Partition table。总共记录4个Primary+ Extended，记作P1: `/dev/hda1`。Extended内的logic partition记录在额外的扇区，从`/dev/hda5`开始。


开机流程:

1. BIOS
2. MBR
3. boot loader
4. 核心档案。


CMOS记录硬件参数的存储器。  

BIOS开机时执行的第一个程序，找到MBR。然后执行boot loader。  
Boot loader提供多重引导开机选单Grub，载入核心档案，转交其他loader。  
Grub软件:开机启动选单软件。  

文本登录后的程序就是Shell。  

Mirror site:当地的下载较快的分流。  

FTP:客户端如Filezilla，可断点续传。传输再怎么地下化也容易被捉到。  
NAT:IP分享器，内网多用户连接外网时对外的IP分享给内部。  
SAMBA:加入Windows网络邻居。  
邮件服务:Sendmail和Postfix等Mail Server软件。  
WWW服务器:Web功能，许多软件用WWW作为显示接口。Apache软件提供WWW网站功能。  
DHCP:客户端自动获取IP功能。  
Proxy:有效解决带宽不足问题。  
硬盘问题有些可用`fsck`软件解决。  

Directory tree:从root directory `/`开始分支。  
<img src='./LinuxStudy_files/chapter3-02.png' />  

- 需`mount`和硬盘档案联系。`/mnt/`内的文件在硬盘上可能在别的地方。  
- 挂载:利用目录作为进入点，将磁盘分区的数据放入该目录。  
- 如`/`挂载到P1，`/home`挂载到P2，则`/home`下的文件都在P2下，而`/etc`就在P1下。  

光盘内容:`/media/cdrom`。 

分区:一个分区内格式化等操作不影响别的分区。考虑所需容量，读写频繁度。  

目录结构: `man hier`
- `/`:根目录。  
- `/Swap`: 内存置换空间，即虚拟内存，无需挂载。理论上为1.5～2倍内存。  
- `/usr`: 软件信息Unix software resource。  
- `/home`: 不同使用者的数据存放。  
- `/var`: 网络相关。  
- `/boot`: 开机读取磁盘大小用，将启动扇区规范在1024个磁柱内，避免磁盘太大读取错误造成的无法开机。100MB就够，须在最前，强制成主要分割区。  

Quota:磁盘配额，分割磁盘后再改动。  

当机原因:除软件问题，可能机箱温度，CPU温度，不同厂商内存，电源供应。  


## Chapter 4
如无法使用DHCP取得IP: 参数设定为 IP: 192.168.1.100, mask:255.255.255.0。  

网卡卡号:Hardware address: 08:00:27:B9:01:BC  

主机名:通常为主机名.网域名，可以有句号，www.vbird.tsai。  

RAID:硬盘特殊应用，软件仿真磁盘阵列。建立两个硬盘分区然后合并，`/dev/md0`，  
LVM:弹性调整系统容量。  
Ext3:文件系统类型，有日志记录。系统默认。  
传统的文件格式为:ext2。Journaling有ext3及Reiserfs等。  
Vfat:linux，windows共用文件系统。  

Grub装载在MBR里。可以设置开机密码，但这样就无法远程登录了。  

系统安装过程写入`/root/install.log`，选择的软件写入`/root/anaconda-ks.cfg`。  

安装包分为多个档案，按需安装。  

memtest86:内存压力测试。  

如因硬件配置无法安装: DVD开机时，`boot: linux nofb apm=off acpi=off pci=noacpi`  
- apm, acpi:电源管理模块；
- nofb:取消显卡缓存侦测；   

SELinux:Access control设定，不是防火墙，推荐安装。  
Kdump:核心出错是将内存写入档案。较消耗硬盘空间。  

Windows双系统时，Linux所在分区在windows下不要挂载，以免被格式化。  

## Chapter 5
Linux使用异步的磁盘/数据传输模式，不能非正常关机。  

GNOME和KDE:Window Manager，图形接口。  

在线升级:yum机制。  

文件名开头为小数点的，即为隐藏文件。  

SCIM:中文输入法软件，`Ctrl+Space`唤出。  

`Alt+Ctrl+Backspace`:重启X Window。或者`startx`.  

- `Ctrl+Alt+F1~6`:tty1~6的文字接口，run level 3；
- `Ctrl+Alt+F7~8`: 切回图形接口，run level 5。  

执行等级: 
- run level 0:关机，
- run level 6:重启。
- 用`init 0`切换模式。

修改`tty1`的显示提示:修订`/etc/inittab`文件内容。   

- `Tab`: 自动补全。  
- `Ctrl+C`: 当前程序中断。  
- `Ctrl+D`: 键盘输入结束。  

终端界面下:`startx`启动图形界面。需tty7空，X server能启动，已启动如X Font Server和xfs等服务，并有Window Manager。  

- Terminal提示格式:`[User@Localhost ~]$`  
- Localhost:主机名，取小数点前的名称。  
- @之前的为登录的用户名。

`~`: 用户的主目录（工作目录），即`/home/`用户名，是个变量。对于`root`用户，`~user`是user用户的家目录。  
`$`: 一般用户的提示字符，#是root用户的提示字符。  

Shell:文字接口程序，默认的是bash。  
指令格式:`command [-opt] par1 par2`，选项`--`后是全称。多个空格视为1个，区分大小写。  
- `\`接特殊字符换行。  
- 提示指令未发现，可能是bash没将该指令添加入搜索path。  
- 指令选项前常有`-`或`+`，选项全称带`--`，如`—help`。选项间可加可不加-。  

列出文件夹列表: `ls -al /home/` ，`-a`显示隐藏文件，`-l`以列表形式显示。`-d`显示目录。位置为`/bin/ls`。  

登录的login也是一个程序。  

- 查看当前有谁在线: `who`  
- 查看网络联机状态: `netstat -a`。Sockets and ports.   
- 查看背景执行的程序: `ps -aux`。

由以上返回信息判断是否可关机。  

数据同步写入硬盘: `sync`，将内存中的数据写入硬盘，默认情况下不会写入，关机前执行。  

注销:`exit`。  

关机:`shutdown`，只有root可以，用远程登录如pietty用ssh登录须进入root权限。  
- `/sbin/shutdown -t 秒 -a 时间 [讯息]`  
- `-t sec`: `-t` 后面加秒数，过几秒后关机。  
- `-k`: 不要真的关机，只是发送警告讯息出去！   
- `-r`: 在将系统的服务停掉后就重启 (常用)   
- `-h`: 将系统的朋务停掉后，关机。(常用) 。  
- `-n`: 不经过 init 程序，直接以shutdown 的功能来关机。  
- `-f`: 关机并开机之后，强制略过`fsck` 的磁盘检查。  
- `-F`: 系统重启后，强制`fsck` 的磁盘检查。  
- `-c`:  取消进行的 shutdown。  
- 时间: `now` 立即，`21:00` 定时，`+10` 10分钟后。  
- 重启关机: `reboot`, `halt` 硬件强行关机, `poweroff`。  

常用工具: 
- 显示时间日期: `date +%H:%M%Y%m%d`, `%H"char"%M` shows hour and miniute.   
- 显示日历: `cal 10 2009`  
- 显示系统语言: `echo $LANG`  
- 修改系统语言: `LANG=en_US`，`LANG=zh_TW.UTF-8`, `LANG="en"`。默认语系选择存于`/etc/sysconfig/i18n`。  
- 计算器: `bc`，`%`求余。`scale=小数点后位数`。`quit`退出。 

查看指令手册: `man` 指令。说明文件在`/usr/share/man/`内，可通过修改`/etc/man.config`或`manpath.conf`改变搜索路径。  
- `man -f man`搜索多个数值指令 show all the numbers of a command。
- `man -k man`查找关键字。
- `man -K man`查询整个系统。
- 有多个数值的指令可通过`man 7 man`查看。  
- `whatis`相当于man -f，但是需用root身份`makewhatis`建立数据库。  

指令手册名称边括号里的数值:  
- 1: shell 下可执行指令
- 2: 系统核心呼叫的函数，工具
- 3: 常用的function 和library, 多为libc
- 4: `/dev` 下装置档案的说明
- 5: 配置或档案格式
- 6: games
- 7: 惯例或协议，如文件系统，网络协议，ASCII code等
- 8: 系统管理员可用指令。  
- 9: kernel相关文件

man手册查看快捷键:  
- 空格键, `PageDown`, `PageUp`翻页
- `Home` 第一页
- `End` 最后一页
- q退出。
- `/string`, `?string`搜索。`n`, `N`查找下一个。  
  
在线查看指令帮助:`info` 指令
- 输出的为含链接的段落，一篇为一个node。
- `/usr/share/info/`。  
- `tab`可在node间切换，`Enter`进入。  
- `b`到当前info的第一个node处，`e`最后一个。  
- `n`, `p`去上, 下一个node。  
- `u`去上一层node.  
- `h`帮助。  
- `?`指令列表。 

`/usr/share/doc/`介绍packages，如`/usr/share/doc/bash-3.2/`介绍bash。  

简单文本编辑器`nano text.txt`:
- `^`代表ctrl，`M`代表alt。
- `^O`存档，`^X`退出。  
- `ctrl-G`: 取得联机help。  
- `ctrl-X`: 离开naon软件，若有修改过档案会提示是否需要储存喔！   
- `ctrl-O`: 储存档案，若你有权限的话就能够储存档案了；   
- `ctrl-R`: 从其他档案读入资料，可以将某个档案的内容贴在本档案中；   
- `ctrl-W`: 搜寻字符串。  
- `ctrl-C`: 说明目前光标所在处的行数与列数等信息；   
- `ctrl-_`: 可以直接输入行号，让光标忚速移动到该行；   
- `alt-Y`: 校正语法功能开启关闭 (单击开、再单击关) 。  
- `alt-M`: 可以支持鼠标来移动光标的功能。  

文件系统错误:
- 如根目录未损毁，登入root，`fsck /dev/sda7` 修复错误的partition。  
- 根目录损毁，不mount该硬盘，执行`fsck /dev/sdb1`。  

主机通电后尽量不动，降低温度。  

忘记root密码:重启时按`e`进入grub编辑模式，在kernel行按`e`，输入`single`，回车后按`b`启动。  

修改密码:`passwd`。  

欢迎画面:`/etc/issue`。  

"\"用escape表示。  


## Chapter 6
档案读写权限:Owner, group, others. Read, write, execute.   

用户帐号: 
- 一个账号可以属于多个群组。  
- 所有账号信息存于`/etc/passwd`，密码存于`/etc/shadow`, 所有组名存于`/etc/group`。  

档案属性: 在`ls -al`列表中显示, 如`-rw-r--r-- 1 root root 42304 Sep 4 18:26 install.log`这七个字段.   

1. 档案类型与权限:
  - 共十个字符`-rw-r--r--`
  - 1: `d`是目录，`-`是档案，`l`是link file，`b`是可随机存储装置，`c`是串行端口如鼠标键盘(一次性读取)，`s`是资料接口，`p`是数据传输文件。
  - 2-4: `r`读`w`写`x`执行。如无该权限该处为`-`。这里是owner的权限。
  - 5-7: group内成员的权限。
  - 8-10: others 的权限。对于目录，如果权限是`r`而不是`x`的话，则无法进入。
2. 连结数: 有多少档案名连结到该inode（一个号码），即Hard link数。文件系统用inode记录档案的权限与属性。
3. 档案拥有者。
4. 所属群组。
5. 档案容量:单位byte。
6. 最后被修改的时间:`ls -l -full-time`显示完整的包括年的时间格式。
7. 档名。

权限操作
- 改变档案所属群组: `chgrp [-R] group dirname/filename filename ...`。加入`R`可令次目录下文件亦改变群组
- 改变档案拥有者: `chown [-R] user dirname/filename filename ...`。`chown user:group filename`。
- 改变档案的权限: `chmod`。还可用于改变SUID，SGID，SBIT。
  1. `chmod a+/-x file`。`+, -`前可为`ugoa`: user, group, owner, all.
  2. 数字模式:`r4w2x1`，一组三个相加。`chmod [-R] 750 file`使file权限变为`drwxr-x---`。编写shell以后，将`664`改为`chmod 755 *.sh`才可运行。
  3. 特殊权限设置: 四位数，最前加4: SUID；2: SGID；1: SBIT，可相加。如`chmod 4755 filename`产生`-rwsr-xr-x`。`chmod 6755 test`产生`-rwsr-sr-`。 
  4. 空权限: 当档案不可执行却又特殊权限时，该权限为空。如`chmod 7666 test`产生`-rwSrwSrwT`。  


档案:
- 可为文本文件（ASCII），数据库文件data file，binary program（须有x权限）等，与扩展名无关。
- rwx权限都是对档案内容而言，w不包括删除档案的权限。删除档案的权限在目录里。


目录:
- 记录文件名列表。`r`可否`ls`（可读但不可执行, 则可`ls`不可`cd`）. `w`可否新建删除重命名移动，`x`可否进入（使该目录成为当前的work directory）。
- 建站时开放目录，`r-x`。`w`不能给，避免删除该目录下别人的档案。

所有人都拥有权限的目录:`/tmp`。


常用操作
- 复制档案: `cp soucename destname`。
- 新建空档案: `touch directory/documentname`。
- 新建floopy: `mkdir floopyname`。
- 切换账号: `su vbird 。
- 进入目录: `cd /root`: into root。`cd ..`: 回到上一层。`cd -`: 回到上一个工作目录。
- 用纯文本方式读取档案: `cat ~/.bashsc`可看到`/home/用户名/`下隐藏的bashsc文件内容。
- 读取data file: 特定格式需用不同指令来查看
- `last` 来查看过去登陆日志，即`/var/log/wtmp`的内容，该档案记录登录的数据。  


特殊档案类型
- 装置文件device: 在`/dev/`下。分为Block（存储，如`/dev/sda`）和Character（串口，一次性读取，不能截断输出）。
- 资料接口Sockets: 在`/var/run/`下，客户端通过它进行数据沟通。
- 数据传输pipe: FIFO，处理多个程序同时读一个文件时的操作。
- 扩展名: sh: 脚本或批处理(Script)； Z,tar,tar.gz,zip,tgz: 压缩文件；html,php: 网页  

网上下载的档案属性会改变。

文件，档案名长度: 255字符，包含路径4096字符。避免特殊字符 
- &#42;
-  `?`
- `>` 
- `<`
- `;`
- `&`
- `!`
- `[`
- `]`
- `|`
- `\`
- `'`
- `"`
- &#96;
- `(`
- `)`
- `{`
- `}`


FHS:Linux目录配置标准。[FHS 3.0](http://refspecs.linuxfoundation.org/FHS_3.0/fhs-3.0.html)   

可分享的(shareable): 可给其他系统挂载
- 不变的(static): `/usr`(软件放置处), `/opt`(第三方协力软件)
- 可变动的(variable): `/var/mail`(使用者邮件信箱), `/var/spool/news`(新闻组)

不可分享的(unshareable): 仅与自己机器有关
- 不变的(static): `/etc`(配置文件), `/boot`(开机与核心档)
- 可变动的(variable): `/var/run`(程序相关), `/var/lock`(程序相关)  

其中不变: 函式库，文件说明，主机服务配置等；可变动: 经常写入的文件。

三层目录:
- `/`:root, 与开机系统有关，分割槽越小越好，不要在此处安装软件；
- `/usr`:软件安装执行有关；
- `/var`:系统运作有关。  

建议:
- `/bin`: 放置的是可以被root与一般账号所使用指令，主要有:`cat,chmod,chown,date,mv,mkdir,cp,bash`等。
- `/boot`: 开机会使用到的档案。Linux kernel常用的档名为vmlinuz，则还会存在`/boot/grub/`。
- `/dev`: 任何装置与接口设备都是以档案的型态存在于这个目录下。有`/dev/null`, `/dev/zero`, `/dev/tty`, `/dev/lp0`, `/dev/hd0`, `/dev/sd0` 等。
- `/etc`: 系统主要的配置文件，如人员的账号密码文件、 各种服务的初始档等。文件可以让一般使用者查阅的，但是只有root有权力修改。FHS 建议不要放置binary。`/etc/inittab`, `/etc/modprobe.conf`, `/etc/fstab`, `/etc/sysconfig/`
  - `/etc/init.d/`: 所有服务的预设启动script。如要启动或关闭iptables: `/etc/init.d/iptables start`、`/etc/init.d/iptables stop`
  - `/etc/xinetd.d/`: super daemon管理的服务的配置文件目录。
  - `/etc/X11/`: X Window 有关的各种配置文件都在这里，如`xorg.conf`。
- `/home`: home directory。变量`~`代表目前用户，`~dmtcai`代表dmtsai的home directory。
- `/lib`: 开机时会用到的函式库，和在`/bin`，`/sbin`底下的指令会呼叫的函式库。`/lib/modules/`放置核心相关的模块 (驱动程序) 。
- `/media`: 放置可移除的装置。有`/media/floppy`, `/media/cdrom`等。
- `/mnt`: 暂时挂载的额外装置放置到这里。
- `/opt`: 第三方协力软件放置的目录。自行安装的软件也安装到这里，或放置在`/usr/local/`下。
- `/root`: 管理员的家目录与根目录放置在同一个分割槽中。
- `/sbin`: 设定系统环境的指令只有root才能使用。`/sbin/`底下的指令为开机过程中所需要的，包括开机、修复、还原系统所需要的指令。服务软件程序，放置到`/usr/sbin/`当中。自行安装的软件所产生的system binary，放置到`/usr/local/sbin/`当中。包括:`fdisk, fsck, ifconfig, init, mkfs`等。
- `/srv`: 一些网络服务启动后所需取用的数据目录。如WWW, FTP等。如WWW服务器需要的网页资料就放置在`/srv/www/`里面。
- `/tmp`: 一般用户或正在执行的程序暂时放置档案的地方。任何人都能够存取的，需要定期的清理一下。FHS建议在开机时，要将`/tmp/`下的数据都删除。

FHS未定义但常见的目录: 
- `/lost+found`: 标准的ext2/ext3文件系统格式产生的，通常会在分割槽的最顶层，即`/disk/lost+found/`。
- `/proc`: virtual filesystem。数据在内存中，如系统核心、行程信息(process)、周边装置的状态及网络状态等。不占任何硬盘空间。如:`/proc/cpuinfo`, `/proc/dma`, `/proc/interrupts`, `/proc/ioports`, `/proc/net/`等。
- `/sys`: 也是一个虚拟的文件系统，记彔与核心相关的信息。包括目前已加载的核心模块与核心侦测到的硬件装置信息等。不占硬盘空间。

函数库: 除了在`/lib/`下还有许多。指令调用的函数。

开机过程中仅有根目录会被挂载，其他分割槽在开机完成后挂载。与开机过程有关的目录，不可与根目录分开:
- `/etc`: 配置文件；
- `/bin`: 重要执行档；
- `/dev`: 所需要的装置档案；
- `/lib`: 执行档所需的函式库与核心所需的模块；
- `/sbin`: 重要的系统执行文件。  

`/usr`: 软件开发时合理地将数据分别放置到该目录的子目录中。  
- `/usr/X11R6/`: 放置X Window System重要数据最后的X版本为第11版，第6次释出。
- `/usr/bin/`: 大部分用户可使用的与开机无关的指令。
- `/usr/include/`: 放置c/c++等程序的header和include，用在以tarball方式 (tar.gz 的方式安装软件) 安装某些数据时。
- `/usr/lib/`: 各应用软件的函式库、object file，以及不被一般使用者用的执行档或script。如64位系统有`/usr/lib64/`，和一些特殊的，经常被系统管理员操作的，进行服务器的设定指令。
- `/usr/local/`: 系统管理员在本机自行安装的软件。如distribution提供的软件较旧，想安装较新的软件但又不移除旧版，该目录下也有bin, etc, include, lib...的子目录。
- `/usr/sbin`: 非系统正常运作所需要的系统指令。如某些网络服务器软件的服务指令（daemon）。
- `/usr/share/`: 放置共享文件。数据几乎是不分硬件架构均可读取的数据，常见子目录: 
  - `/usr/share/man`: 联机帮助文件；
  - `/usr/share/doc`: 软件杂项的文件说明；
  - `/usr/share/zoneinfo`: 时区档案。
- `/usr/src/`: 一般原始码放置到这里。核心原始码放置到`/usr/src/linux/`下。

`/var`:系统运行产生的文件，如cache、log file和软件运行时所产生的档案，如lock file, run file，MySQL数据库的档案等。
- `/var/cache/`: 应用程序本身运作过程中会产生的一些暂存档。
- `/var/lib/`: 放置程序执行过程中需使用的数据文件。各软件有各自的目录。如MySQL的数据库放置到`/var/lib/mysql/`，rpm的数据库则放到`/var/lib/rpm/`。
- `/var/lock/`: 装置或档案资源一次只能被一个应用程序所使用，将该装置lock。
- `/var/log/`: 放置登彔文件。如`/var/log/messages`, `/var/log/wtmp`(记彔登入者的信息)等。
- `/var/mail/`: 放置个人电子邮件信箱，与`/var/spool/mail/`通常是链接文件。
- `/var/run/`: 某些程序或服务运行后，会将它们的PID放置在这个目录。
- `/var/spool/`: 放置一些队列数据（排队等待其他程序使用的数据）。这些数据被使用后通常都会被删除。如收到新邮件会放置到`/var/spool/mail/`中，使用者收下该信件后就会被删除。信件暂时寄不出去会被放到`/var/spool/mqueue/`中，工作排程数据(crontab)，会被放置到`/var/spool/cron/`。

Directory tree:
- 根为`/`；
- 每个目录可使用本地partition的文件系统和网络上的 file system，如Network File System (NFS) 服务器挂载的目录等；
- 每个档案在此树中的完整路径都是独一无二的。
 
`/selinux/`的内容在内存中。  

路径
- 绝对路径: 由`/`开始写起的文件名或目彔名称。  
- 相对路径: 相对于目前路径的文件名写法。如`../../home/dmtsai/`等。 
  - `.`: 代表当前的目录，也可以使用 `./` 来表示；
  - `..`: 代表上一层目录，也可以 `../` 来代表。

正规的执行目录:`/bin/`，`/usr/bin/`下的指令可直接执行。  


## Chapter 7
写Shell Scripts时需用绝对路径。

- 特殊目录: `.`，`..`，`-`前一个目录，`~`，`~user`。
- 根目录的`.`和`..`属性权限完全相同，是同一个目录。

环境变量`$PATH`: 
- 自动去这些目录里搜索指令。同名的指令，执行先搜到的。
- `/usr/local/bin:/bin:/usr/bin:/home/vbird/bin`，中间用`:`隔开。
- 不在`$PATH`下的指令，能用绝对路径访问，如`/sbin/ifconfig eth0`查看IP。
- 安全起见，不在搜索路径中加入`.`，以免有人在`/tmp/`中覆写常用指令。

档案的各种属性都存放于i-node中。

指令前加`\`可忽略alias的指定选项。

档案的时间参数:
- modification time (mtime): 内容数据变更的时间。`ls`默认显示的时间。
- status time (ctime): 权限或属性被更改的时间。
- access time (atime): 内容被取用的时间。

输入指令时;可连续执行。

Ext2/Ext3文件的隐藏属性: 如可以设定档案不可修改，连拥有者都不能修改。用以确保security。其他系统没有改属性。

特殊权限: 出现在档案属性的本该是x的地方上，在属性上显示Set UID/GID的s和Sticky Bit的t。
- Set UID(SUID)仅对binary program有效；
  - 执行者为others，对该程序需有x权限；
  - 本权限仅在run-time中有效；执行者在执行过程中具有owner组的权限，可修改一些本来others无法修改的, owner才能修改的文件。
  - 如修改密码指令`/usr/bin/passwd`。Owner为root，权限为`-rwsr-xr-x 1 root root`，用以修改`-rw-r----- 1 root shadow /etc/shadow`。所以Others可以用该指令改密码。
- Set GID(SGID): 与SUID类似，可用于目录，获得群组权限。
  - 如权限为`-rwx--s--x 1 root slocate /usr/bin/locate`可搜寻`-rw-r----- 1 root mlocate /var/lib/mlocate/mlocate.db`。
  - 目录有SGID属性: 用户在此目录下的effective group将会变成该目录的群组；
  - 若用户在此目录下具有w的权限，则使用者所建立的新档案的群组与此目录的群组相同。
- Sticky Bit:用户对目录有w,x权限；用户在该目录下建立的档案或目录仅有自己与root可删除。
  - 如`/tmp`的权限是`drwxrwxrwt`。

指令
- 显示当前目录: `pwd [-P]`。P显示正确的完整路径（对连接目录有效，如`/var/mail/`）。
- 建立新目录: `mkdir [-mp]`。`mkdir -m 711`直接设置权限而不遵守预设umask。`mkdir -p test1/test2`递归建立目录，即多层结构的目录。
- 删除空目录: `rmdir [-p]`。P连同上层的空目录一起删除。
- 删除目录下所有东西: `rm -r test`。
- 显示变量值:`echo $PATH`。变量名区分大小写。
- 移动文件: `mv /bin/ls /root`。如果`mv filename1 filename2`则是重命名。
- 显示文件信息:`ls [-aAdfFhilnrRSt] 目录名称`；`ls [--color={never,auto,always}] 目录名称`；`ls [--full-time] 目录名称`；`ls -l 档案名1 档案名2`
  - `-a`:全部的档案，连同隐藏档一起列出来。
  - `-A`: 同上，但不包括.和..。
  - `-d`: 查看目录的属性。
  - `-f`: 直接列出结果，而不进行排序 (预设会以档名排序) 。
  - `-F`: 显示附加数据结构，如`*`代表可执行文件；`/`代表目录；`=`代表socket档案；`|`代表FIFO档案。
  - `-h`: 将档案容量以GB, KB等单位显示。
  - `-i`: 列出inode 号码。
  - `-l`: 列出个档案属性。
  - `-n`: 列出UID与GID而不是使用者和群组名称。
  - `-r`: 将排序结果反向输出。
  - `-R`: 连同子目录内容一起列出。
  - `-S`: 以档案容量大小排序。
  - `-t`: 依时间排序。
  - `--color`: never:不要依据档案特性给予颜色显示；always:显示颜色；auto:系统自行判断是否给予颜色。蓝色为档案，白色为目录。
  - `--full-time`: 以完整时间模式 (年、月、日、时、分) 输出。
  - `--time={atime,ctime}`: 输出 access 时间或改变权限属性时间(ctime)而非内容变更时间(modification time)。
- Bash Shell的`alias`: 设定特定的字符串某代表包含参数的指令。
- 复制档案: 只能复制有read权限的。`cp [-adfilprsu] source destination`。`cp [options] source1 source2 source3 .... directory`。如果source有两个以上，则destination一定要是目录。
  - `-a`: 相当于`-pdr`。
  - `-d`: 若source为link file，则复制链接文件属性而非档案本身。
  - `-f`: force，若目标档案已经存在且无法开启，则移除后再尝试一次。
  - `-i`: 若destination已经存在时，在覆盖时会先询问。
  - `-l`: 进行hard link的连结档建立，而非复制档案本身。
  - `-p`: 连同档案的属性一起复制，而非使用默认属性（备份用）。默认属性是`644 复制者 复制者`。
  - `-r`: 递归持续复制，用于目录的复制，不然无法复制。`cp -r /etc/ /tmp`，但属性会改变。`cp -a /etc /tmp`可保证属性不变。
  - `-s`: 复制为symbolic link。
  - `-u`: 若destination比source旧才更新。
- 移除档案:`rm [-fir] 档案或目录`。档案名中`*`代表0到无穷多个字符。
  - `-f`: 忽略不存在的档案。
  - `-i`: 在删除前会询问使用者。默认情况rm指令alias为`rm -i`。
  - `-r`: 递归删除，用于目录的删除，危险。
- 移动档案: `mv [-fiu] source destination`。`mv [options] source1 source2 source3 .... directory`。
  - `-f`: 如果目标档案已经存在，不会询问而直接覆盖。
  - `-i`: 会询问是否覆盖。
  - `-u`: 只有source比destination新时才会update。
- 重命名: `rename`。
- 获得档名: `basename /etc/sysconfig/network` 返回`network`。
- 获得目录名: `dirname /etc/sysconfig/network` 返回`sysconfig`。
- 由第一行开始显示档案内容: `cat [-AbEnTv] 档案名`。
  - `-A`: 等于`-vET`。
  - `-b`: 列出非空白行行号。
  - `-E`: 显示断行字符`$`。Windows的文档断行符为`^M$`。
  - `-n`: 打印所有行行号。
  - `-T`: 将`tab`键以`^I`显示。
  - `-v`: 列出特殊字符。
- 从最后一行开始显示: `tac`。用法同上。
- 显示时输出行号:`nl [-bnw] 档案`。
  - `-b`: 指定行号出现方式，`-b a`: 空行也显示；`-b t`: 不显示，默认值。
  - `-n`: 行号显示方式，`-n ln`: 最左方；`-n rn`: 最右方，不补0；`-n rz`: 最右方补0。
  - `-w`: 行号字段占用的位数。
- 分页显示: `more 档案名`。底下显示百分比。可执行操作:
  - `space`: 向下翻页。
  - `Enter`: 向下翻一行。
  - `/字符串`: 向下搜寻关键词。
  - `:f`: 显示文件名及目前的行数。
  - `q`: 离开。
  - `b`或`[ctrl]-b`: 往回翻页，只对档案有用，对管线无效。
- 可往前翻页显示: `less`。同上，操作:
  - 空格键或`[pagedown]`: 向下翻页。
  - `[pageup]`: 向上页。
  - `/字符串`: 向下搜寻。
  - `?字符串`: 向上搜寻。
  - `n`: 回到前一个搜寻。
  - `N`: 反向的前一个搜寻。
  - `q`: 离开。
- 只看头几行: `head [-n number] 档案1 档案2`。
  - `-n`: 后面接数字显示几行。不加数字默认显示10行。接负数显示总行数减后的行数。如-100显示41行（总长141行）。
- 只看末几行: `tail [-n number] 档案`。
  - `-n`: 从第几行开始显示，前面的number-1行不显示。
  - `-f`: 表示持续侦测该档案，一有变动就显示，直到按下`[ctrl]-c`为止。
- 读取二进制档案: `od [-t TYPE] [oC TYPE] 档案` 
  - `-t`: 接输出TYPE: `a`:默认；`c`:用ASCII；`d[size]`:利用十进制(decimal)，每个整数占用size bytes；`f[size]`:利用floating；`o[size]`:用八进制(octal)；`x[size]`:用十六进制(hexadecimal)。
  - `oC`: 显示两种TYPE并比较。如`od -t oCc /etc/issue`。
- 修改档案时间（可建立空档案）: `touch [-acdmt] 档案`。
  - `-a`: 修改access time为当前时间。
  - `-c`: 修改档案的时间，不存在则不建立新档案。
  - `-d`: 后面接欲修订日期，等于--date="日期或时间"。如`touch -d "2 days ago" bashrc`。
  - `-m`: 仅修改mtime。
  - `-t`: 后面接欲修订的时间，格式为`[YYMMDDhhmm]`。如`touch -t 0709150202 bashrc`。
- 档案预设权限: `umask [-S]`查看。umask 三位数设定。
  - `S`: 用符号显示。
  - 共四位数，第一位为特殊权限。
  - 新建档案，权限默认为666，目录缺省为777。
  - 数值显示的是剪掉的权限。如，umask显示0022则表明现在默认的权限为建立档案时644，建立目录时755。
  - 设定时为缺省值需要剪掉的数。如`umask 002`令档案默认为664,目录为775，
  - root的umask为022，一般users为002，预设在`/etc/bashrc`，不建议修改。
- 配置隐藏属性: `chattr [+-=][ASacdistu] 档案或目录`。
  - `+`: 增加参数。
  - `-`: 移除参数。
  - `=`: 重新设定全部参数。
  - `A`: atime不会被修改。
  - `S`: 将该档案异步存取模式改为同步写入磁盘。
  - `a`: 档案只能增加数据，只有root才能设定。
  - `c`: 自动压缩，读取时自动解压缩。
  - `d`: 用dump程序备份时，该档案或目录不会被备份。
  - `i`: 档案不能被删除、改名、设定连结、写入或新增内容。只有root能设定。
  - `s`: 档案被删除时完全地移除。
  - `u`: 档案被删除时内容还存在磁盘中。
- 查看隐藏属性: `lsattr [-adR] 档案或目录`。
  - `-a`: 显示隐藏文件。
  - `-d`: 显示目录。
  - `-R`: 递归显示子目录。
- 观察文件类型: `file`。属于ASCII或data，有否用到share library。
- 寻找指令位置: `which [-a] command`。
  - `-a`: 列出所有PATH目录中可找到的指令而非第一个。
- 寻找档案: `whereis [-bmsu] 档案或目录`。在系统维护的数据库中找，速度快。
  - `-b`: 只找binary格式的档案。
  - `-m`: 只找在说明文件manual路径下的档案。
  - `-s`: 只找source来源档案。
  - `-u`: 搜寻不在上述三个项目中的其他特殊档案。
- 更新数据库`/var/lib/mlocate/`: `updatedb`。
- 关键字查找档案名，即档案名查找包含该词的档案: `locate [-ir] keyword`。数据库`/var/lib/mlocate/`一天只更新一次，新建的找不到。
  - `-i`: 不区分大小写。
  - `-r`: 启用正则表达式。
- 硬盘中查找档案: `find [PATH] [option] [action]`。
  - 支持通配符如`find /etc -name '*httpd*'`。速度慢。`*` 对应任意个字符，`?`对应任意一个字符。
  - 与时间有关的选项: 
    - `-atime,-ctime，-mtime`。`-mtime 数字n`:在n天前的当天被更改过内容的档案；`+n`:列出n天前不含第n天的档案；`-n`:列出n天内含n天的档案档名。
    - `-newer file`: 列出比file新的档案。`find /etc -newer /etc/passwd`。
  - 使用者或组名有关: 
    - `-uid n`:n是用户的账号UID，记录在`/etc/passwd`里面。
    - `-gid n:组名的GID。记录在/etc/group。
    - `-user name`: 使用者的账号名称。
    - `-group name`: 组名。
    - `-nouser`: 寻找不存在于/etc/passwd的档案拥有者。
    - `-nogroup`: 寻找不存在于`/etc/group`的拥有群组的档案。发生于安装软件时。
  - 与档案权限及名称有关: 
    - `-name filename`: 搜寻文件名为 filename的档案。
    - `-size [+-]SIZE`: 搜寻比SIZE大(+)或小(-)的档案。如`-size +50k`。单位`c`代表byte，`k`代表1024bytes。
    - `-type TYPE`: 搜寻档案的类型为: 一般档案`f`,装置档案`b`、`c`,目录`d`,连结档`l`, socket `s`,及FIFO `p`。
    - `-perm mode`: 搜寻档案权限等于mode的档案，如`-rwsr-xr-x=4755`。
    - `-perm -mode`: 权限有并高于mode的档案。
    - `-perm +mode`: 搜寻档案权限包含任一mode权限的档案，如`-perm +755`可找到`-rw-------`。
  - 额外动作:
    - `-print`: 将结果打印到屏幕上。
    - `-exec command`: 用额外的指令（不支持alias）处理搜寻到的结果。如`find / -perm +7000 -exec ls -l '{}' \;`。`{}`代表find找到的内容。`-exec`到`\;`之间是额外动作。因为`;`在bash下有特殊意义，因此用反斜杠跳脱。等同于`ls -l 第一个找到的;ls -l 第二个找到的;`
  - 逻辑表达式: 
    - `-a`: and。如`find /etc -size +50k -a -size -60k -exec ls -l '{}' \；` 
    - `!`: 反向选择。如`find /etc -size +50k -a ! -user root -exec ls -ld '{}' \;`找到大于50k且不是root的档案。
    - `-o`: or。
- `groupadd group`添加组，
- `useradd user`添加用户。
- `id user`查询用户权限情况。

添加一个组公用文件夹，需要设置SGID，不然某用户默认建立的档案别人无法访问。用来设置公共开发目录。


## Chapter 8
inode, superblock 
- 档案的硬盘位置：inode存属性与权限，在硬盘中的位置，一个档案一个。
- 实际数据放data block中。一个不够时会占多个。大小为1K，2K或4K。格式化时固定。每个block内只能存一个档案的数据。
- superblock记录file system的格式、信息、inode和block总量、使用量、剩余量。
- Ext2用indexed allocation存取数据：
  - inode中记录所有block地址。
  - 格式化时区分多个block group, 每个group有独立的inode,superblock,block。
  - group信息记录在boot sector中。
- FAT用链表形式记录。需碎片整理。
- inode：记录：
  1. 存取模式(read/write/excute)；
  2. 拥有者与群组(owner/group)；
  3. 档案容量；
  4. ctime，atime，mtime；
  5. 档案的flag如SetUID；
  6. block的位置pointer。
  - 大小均固定为128 bytes。
- Superblock：记录:
  1. block与inode的总量；
  2. 未使用和已使用的inode/block数量；
  3. block与inode的大小；
  4. filesystem的挂载时间、写入数据时间、fsck（检查硬盘）时间等；
  5. 一个valid bit：系统已被挂载为0否则为1。
  - 大小为1024 bytes。
  - 可以用`dumpe2fs`查看。
  - 第一个block group内有superblock，后续的不一定有，有也是备份。

EXT2区块: 
- Filesystem description：每个block group的开始与结束的block号码，每个区段的superblock,bitmap,inodemap,data block分别在哪个block。用`dumpe2fs`查看。
- Block bitmap：知道哪些block是空的，可快速找到可使用的空间。
- Inode bitmap：记录使用与未使用的inode号码。最顶层目录的inode一般为2。

查看文件系统信息：`dumpe2fs [-bh] 装置文件名`。
- `-b`：列出保留为坏轨的部分。
- `-h`：仅列出 superblock的数据。可显示文件系统的label。

查看目前的挂载装置：`df`。

ext2文件系统建立目录：
- 分配一个inode与至少一块block。
- inode记录相关权限与属性和block号码；
- block记录在这个目录下的文件名与该文件名占用的inode号码。

用`ls -i`查看，最左边是inode号码。

中介数据：Super block，block bitmap，inode bitmap不存放实际数据的区段为metadata。

Inconsistent：写入数据时更新metadata的步骤未做完。由`e2fsck`检查valid bit是否有挂载和filesystem state是否clean。慢。

Journaling filesystem：在filesystem中用一个区块专门记录修订档案的步骤。Ext3实现。

Asynchronously处理存取：档案加载到内存后，在未改动前为clean，改动后dirty，系统不定时写回磁盘。为了效率尽量利用内存。用sync将所有内存写回。

常见的文件系统：
- 传统文件系统：ext2/minix/MS-DOS/FAT(用vfat模块)/iso9660(光盘)等。
- 日志式文件系统：ext3/ReiserFS/Windows' NTFS/IBM's JFS/SGI's XFS。
- 网绚文件系统：NFS/SMBFS。
- 查看支持的文件系统`ls -l /lib/modules/$(uname -r)/kernel/fs`。
- 系统目前已加载到内存中支持的文件系统：`cat /proc/filesystems`。

Virtual Filesystem Switch：Linux由VFS去读取filesystem。

列出文件系统的整体磁盘使用量：`df [-ahikHTm] [目彔或文件名]`。不去文件系统搜索。
- `-a`：出所有的文件系统，包括`/proc`等不在硬盘中的挂载点。
- `-k`：以KB为单位。
- `-m`：以MB为单位。
- `-h`：自动选择GB,MB,KB等单位。
- `-H`：M=1000K而非M=1024K。
- `-T`：同时列出partition的filesystem名称如ext3。
- `-i`：用inode的数量来显示。

`/dev/shm/`是利用内存虚拟出来的磁盘空间。

推测目录所占容量：`du [-ahskm] 档案或目录名称`。默认单位为1K。可用通配符`*`。去硬盘搜索，慢。
- `-a`：列出所有的档案与目录容量。默认仅统计目录底下的档案量。
- `-h`：以G/MB单位显示。
- `-s`：列出总量，而非每个目录的占用容量。
- `-S`：不包括子目录的总计。
- `-k`：以KB为单位。
- `-m`：以MB为单位。

读取档案顺序：由super block 里的档名找到inode，再由inode 找到区块。

Hard link：多个档名对应一个inode。档名存于目录中。档名的block中存的是实际数据的inode，指向实际数据的block。两个档名的信息完全相同。
- 不能跨Filesystem。不能link目录
- 在当前目录建立:`ln /etc/crontab .`。`.`代表相同档名。
- 新增一个目录时，由于自动生成了`.`和`..`，所以子目录link数为2，父目录link数加一。
- 删除任何一个档名都不会删除数据，而任何一个档名都能改动数据。
- 新建一个hard link只是在目录中增加一个档名，不占用新的inode和block。

Symbolic link：建立一个独立的档案指向link档的档名。原始档删除后无法打开。
- 可链接目录。
- 修改Symbolic link的档案，其实改动的是原档案。
- 在当前目录建立：`ln -s /etc/crontab crontab2`。
- 不管是连接的档案还是目录，由`rm /root/bin`移除。

建立链接：`ln [-sf] 来源文件 目标文件`。
- `-s`：如果不加任何参数是hard link，`-s`是symbolic link。
- `-f`：如果目标文件存在，覆盖。

磁盘分区：`fdisk [-l] 装置名称`。
- `-l`：输出装置中所有的partition。不接装置名时显示所有装置。
- 装置名：如`fdisk /dev/hdc`，不要加数字。之后进入指令模式：
  - `d` delete a partition
  - `n` add a new partition
  - `p` print the partition table
  - `q` quit without saving changes
  - `w` write table to disk and exit
- 扩展分区删除时，它的逻辑分区自动删除。
- 新增分区：Last cylinder or +size or +sizeM or +sizeK (1-5005, default 5005): +512M：+512M让系统自动找最接近的磁柱。

系统重新读取partition table：`partprobe`。新增分区后需要reboot。

格式化：`mkfs [-t 文件系统格式] 装置文件名`。综合指令。
- `-t`：文件系统格式，如ext3,ext2,vfat等如`mkfs -t ext3 /dev/hdc6`。

制定文件系统的细节：`mke2fs [-b block大小] [-i block大小] [-L 标头] [-cj] 装置`。

系统救援：`fsck [-t 文件系统] [-ACay] 装置名称`。
- 被检查的partition不可挂载。
- 有问题的数据存于lost+found目录。

硬盘挂载：
- 一个文件系统不应该被重复挂载；
- 一个目录不应该重复挂载多个文件系统；
- 作为挂载点的目录应该为空，不然内容会暂时消失。

指令：`mount -a`
- `-a`：依照配置文件`/etc/fstab` 的数据将所有未挂载的磁盘都挂载。
- `-l`：输入mount会显示目前挂载的信息。加上`-l`可同时显示Label。如`/dev/hdc2 on / type ext3 (rw) [/1]`中`/1`为label。
- `mount [-t 文件系统] [-L Label名] [-o 额外选项] \ [-n] 装置文件名 挂载点`。
- `-o`：重新挂载，在单人维护模式下`/`为只读，需重新挂载。如`mkdir /mnt/hdc6; mount /dev/hdc6 /mnt/hdc6`。

文件系统的驱动在`/lib/modules/$(uname -r)/kernel/fs/`下。

取消挂载：`umount [-fn] 装置文件名或挂载点`。
- `-f`：强制卸除。
- `-n`：不更新`/etc/mtab`的情况下卸除。

装置档案的Major和minor：决定这个档案是哪个装置。如22, 10代表`/dev/hdc10`。

手动设置装置档案：`mknod 装置文件名 [bcp] [Major] [Minor]`。

修改Label：`e2label 装置名称 新的Label名称`。

更改文件系统格式：`tune2fs [-jIL] 装置代号`。

调整IDE参数：`hdparm [-icdmXTt] 装置名称`。

开机挂载：记录于`/etc/fstab`和`/etc/mtab`。
- 六个字段为Device，Mount point，filesystem，parameters，dump，fsck。
- `cat /etc/fstab`: 
- `LABEL=/home /home ext3 defaults 1 2`。
- `/dev/hdc6 /mnt/hdc6 ext3 defaults 1 2`

自动mount规则：
- `/`必项挂载且先于其它mount point；
- 其它mount point必为已建立的目录遵守架构原则;
- mount point只能挂载一次；
- partition只能挂载一次；
- 进行卸除时工作目录需在mount point及子目录外。

用loop装置挂载iso：
```
mkdir /mnt/centos_dvd;
mount -o loop /root/centos5.2_x86_64.iso /mnt/centos_dvd;
umount /mnt/centos_dvd/
```

挂载档案已虚拟一个分割槽：

1. 建立空档案：`dd if=/dev/zero of=/home/loopdev bs=1M count=512`。
  - `if`：input file，`/dev/zero`：一直输出0的装置。
  - `of`：output file。
  - `bs`：block大小。
  - `count`：共几个bs。
2. 格式化：`mkfs -t ext3 /home/loopdev`。
3. 用loop挂载：`mount -o loop /home/loopdev /media/cdrom/`。用`xen`软件可以进行根目录挂载，相当于虚拟机。

建置swap：

1．分割：`fdisk /dev/hdc;n;[enter];[+256M];p;t;7//新建的分割槽;82//swap的Id;w;partprobe;`
2．格式化：`mkswap /dev/hdc7`。
3．启用：`swapon /dev/hdc7`。
4．用`free`观察内存使用量。
5．列出所有`swap：swapon -s`。
6．关闭：`swapoff /dev/hdc7`。

可用loop建置swap。

休眠时，内存数据记录于swap。

目录总block数：`ll`结果的total。

分割出超过2TB的分割槽：`parted [装置] [指令 [参数]]`。

数值模式输出档案的处理：`PAVE`软件。

## Chapter 9
压缩文件：tar（只是打包）, tar.gz（用gzip）, tgz, gz, Z, bz2。

gzip和bzip2将目录内所有档案分别压缩。

gzip：可解compress,zip,gzip等档案。gz为档名。`gzip -v man.config`。源文件删除。可被Winrar解压。
- `-c`：将压缩的数据输出到屏幕上，可重导向。保留源文件。`gzip -9 -c man.config > man.config.gz`。
- `-d`：解压缩参数。`gzip -d man.config.gz`。
- `-t`：检验一个压缩文件的一致性，有无错误。
- `-v`：显示压缩比等信息。
- `-#`：压缩等级，`-1`最快，`-9`压缩比最好，预设`-6`。
- 读取纯文本压缩文件的内容：`zcat man.config.gz`。压缩文件删除。

bzip2优于gzip：`bzip2 [-cdkzv#] 档名`。
- `-c`：数据输出。
- `-d`：解压缩的参数。
- `-k`：保留源文件。
- `-z`：压缩的参数。
- `-v`：显示压缩比等信息。
- `-#`：压缩比的参数，`-9`最佳，`-1`最快。
- 读取内容：`bzcat 档名.bz2`。

打包与压缩：`tar [-j|-z] [cv] [-f 建立的档名] filename`。`tar -jcv -f filename.tar.bz2 target`。打包文件称为tarfile。经过压缩的打包称为tarball。
- 察看档案：`tar [-j|-z] [tv] [-f 建立的档名]`。`tar -jtv -f filename.tar.bz2`。
- 解压缩：`tar [-j|-z] [xv] [-f 建立的档名] [-C 目录]`。`tar -jxv -f filename.tar.bz2 -C 目的地目录`。
- `-c`：建立打包档案，搭配`-v`察看被打包的档名。
- `-t`：察看打包档案的内容有哪些档名。
- `-x`：解压缩。`-C`在特定目录解开。
- `-c`, `-t`, `-x`不可同时出现。
- `-j`：用bzip2压缩/解压缩，档名为`*.tar.bz2`。
- `-z`：用gzip压缩/解压缩，档名`*.tar.gz`。
- `-v`：在压缩/解压缩的过程中显示文件名.
- `-f filename`：被处理的档名。
- `-C 目录`：用在解压缩到特定目录。无该选项则解压到当前目录。`tar -jxv -f /root/etc.tar.bz2 -C /tmp`。
- `-p`：保留备份数据的原本权限与属性，用于备份配置文件。备份etc目录：`tar -zpcv -f /root/etc.tar.gz /etc`。会自动删除`/`根目录：因为这样如在`/tmp`下解开，则成为`/tmp/...`。
- `-P`：保留绝对路径。所以会保留`/`，直接解到`/...`下且覆盖同名文件。
- `--exclude=FILE`：不将FILE打包。`tar -jcv -f /root/system.tar.bz2 --exclude=/root/etc* --exclude=/root/system.tar.bz2 /etc /root`。
- `--newer-mtime`：仅备份新的档案，不接mtime的话则同时包括ctime和mtime。`tar -jcv -f /root/etc.newer.then.passwd.tar.bz2 --newer-mtime="2008/09/29" /etc/*`。
- 解开单一档案：`tar -jtv -f /root/etc.tar.bz2 | grep 'shadow'`找到路径，再`tar -jxv -f /root/etc.tar.bz2 etc/shadow`解压。
- 磁带机：因为一次性读取，无法用`cp`复制。将/home,/root,/etc备份到磁带：`tar -cv -f /dev/st0 /home /root /etc`。
- 用Standard input/standard output重导向边压缩边解压：`tar -cvf - /etc | tar -xvf -`。将`/etc`下所有文件打包成内存中的一片地址（即-）然后在当前目录下解压。

系统备份范例：
```
tar -jcv -f /backups/backup-system-20091130.tar.bz2 --exclude=/root/*.bz2 --exclude=/root/*.gz --exclude=/home/loop* /etc /home /var/spool/mail /var/spool/cron /root
```

备份文件系统工具：`dump`可以备份整个文件系统和制定等级。`dump [-Suvj] [-level] [-f 备份档] 待备份资料`。`dump -W`。
- 第一次备份为level 0，以后递增。`dump -0u -j -f /backups/myproject.dump /srv/myproject`。
- 目录有限制
- `-S`：列出待备份数据需要多少磁盘空间。
- `-u`：将这次的时间记录到`/etc/dumpdates`中。
- `-v`：显示过程。
- `-j`： 用bzip2压缩，默认为2-level.
- `-f`：后面接产生的档案或如`dev/st0`的装置文件名。
- `-W`：列出在`/etc/fstab`里面的具有dump设定的partition是否备份过。

备份文件复原：
- 查看：`restore -t [-f dumpfile] [-h]`。
- 比较dump与实际档案：`restore -C [-f dumpfile] [-D 挂载点]`。
- 互动模式：`restore -i [-f dumpfile]`。
- 还原整个文件系统：`restore -r [-f dumpfile]`。
- 模式无法混用。
- `-t`：此模式察看备份文件中有什么数据。
- `-C`：此模式将dump的数据与实际文件系统比较列出不一致的档案。
- `-i`：互动模式，可以仅还原部分档案。
- `-r`：将整个filesystem还原的一种模式。
- `-h`：察看完整备份数据中inode与文件系统label等信息。
- `-f`：后面就接要处理的dump档案。
- `-D`：与`-C`搭配可查接的挂载点与dump内有不同的档案。

制作映像档iso：`mkisofs [-o 映像档] [-rv] [-m file] 待备份文件.. [-V vol] -graft-point isodir=systemdir ...`。
- `-o`：后面接你想要产生的那个映像档档名。
- `-r`：透过Rock Ridge产生支持Unix/Linux的档案数据，可记录较多信息，包括UID/GID与权限等。
- `-v`：显示建置 ISO 档案的过程。
- `-m file`：排除档案。
- `-V vol`：建立Volume，即CD的title。
- `-graft-point`：默认都放置于映象的根目录，加此选项后使目录为绝对地址.

刻录至光盘：
- 查询刻录机位置：`cdrecord -scanbus dev=ATA`。
- 抹除重复读写片：`cdrecord -v dev=ATA:x,y,z blank=[fast|all]`。
- 格式化DVD+RW: `cdrecord -v dev=ATA:x,y,z -format`。`cdrecord -v dev=ATA:x,y,z [选项] file.iso`。
- `-scanbus`：扫瞄磁盘总线找出刻录机，装置为ATA接口。
- `-v`：显示过程而已。
- `dev=ATA:x,y,z`： x, y, z为刻录机所在位置。
- `blank=[fast|all]`：抹除可重复写入的CD/DVD-RW.
- `-format`：仅针对 DVD+RW 这种格式。选项：
- `-data`：以数据格式写入，不是CD音轨(`-audio`)。
- `speed=X`：刻录速度，CD可用 `speed=40`。DVD则可用`speed=4`。
- `-eject`：完毕后自动退出光盘。
- `fs=Ym`：映像档先暂存至缓冲存储器。预设为 4m。
- `driveropts=burnfree`：用于DVD，打开Buffer Underrun Free模式的写入功能。
- `-sao`：支持 DVD-RW 的格式。

备份装置：
- `dd`可以读取磁盘装置的内容(直接读取sector)，然后将整个装置备份成一个档案。`dd if=input_file of=output_file bs=block_size count=number`。
- `dd if=/dev/hdc of=/tmp/mbr.back bs=512 count=1`。还原则反向操作。不需格式化。
- `if`： input file。可以是装置。
- `of`： output file。可以是装置。
- `bs` ：一个block的大小，预设是512bytes(一个sector的大小)。
- `count`：多少 bs。
- 可以用于复制boot sector区块，而cp和tar不行。

任何东西备份：`cpio`。
- 备份：`cpio -ovcB > [file|device]`。
- 还原：`cpio -ivcdu < [file|device]`。
- 察看：`cpio -ivct < [file|device]`。
- `/boot/initrd-xxx`档案由cpio建立。

## Chapter 10
文本编辑器：emacs, pico, nano, joe。

`crontab`, `visudo`, `edquota` 等指令会主动调用vi。

vi模式：
- 一般模式：可移动光标，删除，复制粘贴字符或整行。
- `hjkl`：左下上右。`30k`向上30行。
- `ctrl+F/B`：下/上翻页。`ctrl+d/u`：下/上半页。
- `-/+`：上/下一行。
- `20space`：后移20个字符, 包括换行符。
- `0`：等同于home，`$`：等同于end。
- `H`：当前屏幕第一行头，`M`：中间行头，`L`：最下行头。
- `G`：文档最后一行，`20G`：第20行，`gg`：第一行。`set nu`：显示行号。
- `20enter`：下移20行。
- `x/X`：向后/前删除一个字符。`10x`：删除10个。
- `dd`：删除一整列。`20dd`：删除20列。
- `d1G`：删除光标到文档第一行的数据。`dG`：删除光标所在到文档最后的数据。
- `d$/0`：删除光标到该行最后/前。
- `yy`：复制一行。`20yy`复制20行。
- `y1G`:复制光标所在列到第一列的数据。`yG`：到最后列。
- `y$/0`：复制光标到最后/前。
- `p`：粘贴到光标下一行。`P`：贴到前一行和当前行中间。
- `J`：将当前列与下一列变成一列。
- `c`：重复删除多个数据。例如向下删除10行：`10cj`
- `u`：撤销，`ctrl+r`：重做上一个动作。`.`：重复前一个动作。

编辑模式：
- 按`i`（目前光标处插入）, 
- `I`（该行第一个非空格处插入）, 
- `o`（该行后插入一行）, 
- `O`（该行上插入一行）, 
- `a`（目前光标的下一个字符插入）, 
- `A`（行末插入）, 
- `r`（取代光标处字符一次）, 
- `R`（取代至按Esc）中的一个进入。
- 按`ESC`退回一般模式。

指令模式：
- `:`, `/`, `?`中的一个进入，可搜索，读取，存盘，替换，离开，显示行号等。
- `/word`：向下查找word这个字。`?word`：向上。然后`n`向下找下一个，`N`向上找。
- `:w`保存，`:w!`强制保存（当权限为特殊权限），`:q`退出，`:q!`不保存退出，`:e!`恢复成档案初始值。
- `:1,5s/word1/word2/gc`：从第一行到第5行（可省略），查找word1并替换成word2。`c`为显示确认信息（可省略）。
- `ZZ`：如修改，保存离开，不然不保存离开。
- `:w filename`：另存为。
- `:n1, n2 w filename`：将第n1 到n2 行的文本另存为。
- `:r filename`：将filename加到当前光标处之后。
- `:! command`：vi中查看执行某命令的结果，如`:! ls /home`。
- `:set nu`：显示行号。
- `:set nonu`：取消行号。
- `:set all`：显示所有设定值。


`ctrl+Z`：将程序在背景执行。`bg`可查看背景中的程序。`fg`将背景中的程序恢复。

`kill -9 %1` 停止进程。

Vim备份机制：
- vim的暂存档：`.filename.swp`。
- Recovery是恢复之前未保存的。然后需要删除暂存档。
- Delete是删除暂存档。

`.config`文件：批注以`#`开头。

通过鼠标复制粘贴会将tab转换为空格。

vim功能：
- Visual Block 功能：
  - `v`：字符选择，会将光标经过的地方反白选择。
  - `V`：行选择，会将光标经过的行反白选择。
  - `Ctrl+v`：区块选择，可以用长方形的方式选择资料。
  - `y`：将反白的地方复制起来。
  - `d`：将反白的地方删除掉。
- 多档案编辑：同时开启多个档案进行编辑。可以进行档案间复制。
  - `:n`：编辑下一个档案。
  - `:N`：编辑上一个档案。
  - `:files`：列出目前这个 vim 开启的所有档案。
  - `:sp filename`：上下双窗口显示。如果不输入filename 为本文档。`ctrl+w+↑`或`ctrl+w+k`到上窗口，`ctrl+w+↓`或`ctrl+w+j`到下窗口。`:q`或`ctrl+w+q`离开。

环境设置：`~/.vimrc`，`~/.viminfo`。
- 搜索字符串、重新打开编辑过的档案，都有记录存在于`.viminfo`中。
- `.vimrc`：整体的设置在`/etc/vimrc`中，`.vimrc`覆写。

中文环境：`LANG=zh_TW.big5`。但是推荐用`utf8`。

`iconv`：编码转换。
- `iconv -f big5 -t utf8 filename -o newfile`
- `--list`：显示支持的语系。

断行符：用`cat -A`查看文档可发现，DOS使用`^M$`即`CR(^M)`和`LF($)`两个符号。而Linux只使用`LF`。
- `dos2unix [-kn] file [newfile]`：转换。`k`保留原mtime。`n`保留原文档。
- `unix2dos`：相反。

## Chapter 11
透过Shell将我们输入的指令与Kernel沟通。由指令（如`man`）去调用程序（`man.sh`）。

有讲多的版本，如Bourne SHell(`sh`)，在Sun里的C SHell，商业用的K SHell，TCSH等。Linux 使用的为Bourne Again SHell(简称 `bash`)。

`/etc/shells`中记录可用的shells：
- `/bin/sh` (已经被`/bin/bash`所取代)；
- `/bin/bash`(预设shell)；
- `/bin/ksh` (Kornshell，来自AT&T Bell lab)；
- `/bin/tcsh` (C Shell升级版)；
- `/bin/csh` (已经被`/bin/tcsh`所取代)；
- `/bin/zsh`(ksh的升级版)。
- 给使用者一些自定的shell（如`/sbin/nologin`）让使用者无法以其他服务登入主机。

bash的历史记录：`~/.bash_history`。在注销系统后才会写入新的历史。

设置别名：
- `alias lm='ls -al|more'`。用`more`指令来令`ls`的结果可一页一页查看。
- `alias rm='rm -i'`避免错删。
- `alias`：查看别名。
- 取消设置的别名：`unalias lm`。

通配符：wildcard。`ls -l /usr/bin/X*` 以X开头。`*`代表0到无穷多个任意字符。

bash的内建命令：不需要调用外部程序。如`cd`，`umask`。用`type`可以查看。
- `type [-tpa] name`
- 不加任何选项参数，显示name是外部还是内建.
- `-t`：显示意义：`file`：外部；`alias`别名；`builtin`：内建；
- `-p`：是外部指令，才显示完整文件名；
- `-a`：会由环境变量`PATH`定义的路径中所有含name的指令都列出。

收邮件：`mail`指令。调用`MAIL`变量决定显示哪个邮箱的文件。`MAIL=/var/spool/mail/username`。

变量
- 环境变量：`PATH`, `HOME`, `MAIL`, `SHELL`等。用大写来表示环境变量，小写为自定义变量。
- 取用变量：`echo $var` 或`echo ${var}`。如果该变量未定义，则显示空字符串。
- 变量赋值：`var=content`。不能有空格。
- 变量名不能以数字开头。

字符串：
- `""`中的特殊字符有自己特殊的作用，如`var="lang is $LANG"`，则`var`为`lang is en_US`。
- `''`中的特殊字符还是字符。
- 跳脱字符：`\`可以将后一个特殊字符变为普通字符，如`$\空格`等，对回车符，则没有内容。所以
```
cp /var/spool/mail/root /etc/crontab \
> /etc/fstab /root
```
  是一个指令，因为`\`后面的那个enter在执行时被跳脱了。

反单引号&#96;指令&#96;或`$(指令)`可以显示该指令的结果。如
```
version=$(uname -r)
cd /lib/modules/`uname -r`/kernel
ls -l `locate crontab`
```

赋值时字符串变量间直接相连则会拼接，如`PATH="$PATH":/home/bin`。

变量要在子程序执行，则需`export`令其变为环境变量，如`export PATH`。

删除变量：`unset myvar`。

空格是特殊字符，不能赋值给字符串变量，需`\`。

单双引号必须成对。如果不成对，enter后等待输入。

要输入有单引号的字符串做变量值，可使用双引号括起来，或`\`。

再开一个子bash：`bash`。

查看环境变量：`env`可查看所有环境变量，`export`不接变量名也可。
- `HOSTNAME`：主机名
- `TERM=xterm`：终端机环境是什么类型
- `SHELL=/bin/bash`
- `HISTSIZE=1000`：指令历史数
- `USER=root`
- `MAIL=/var/spool/mail/root`：当前用户mailbox位置
- `INPUTRC=/etc/inputrc`：键盘按键功能有关。可以定特殊按键
- `PWD=/root`：目前用户所在的工作目录
- `LANG=en_US`
- `HOME=/root`：家目录
- `_=/bin/env`：上一次使用的指令最后的一个参数或指令本身。
- `RANDOM`：由`/dev/random`档案生成的介于0～32767的随机数。生成0~9之间的数值：`declare -i number=$RANDOM*10/32768`, `declare -i`用以声明整数。


查看所有变量，即环境和自定义变量：`set`。可列出最近设定的变量。
- `MAILCHECK=60`：每60秒去扫描信箱有无新信
- `OLDPWD=/home`：上个工作目录。可用 `cd -` 来调用这个变量
- `PPID=20025`：父程序的PID
- `$`：目前这个shell所使用的PID。echo $$可显示当前使用的shell。
- `?`：刚刚执行的指令回传值。成功执行为0，不然为错误代码。
- `PS1`：提示字符，即常见的`root@www~`。可设定。`PS1`不是环境变量，但是是bash的操作环境设置之一。

设定指令输入输出环境：`set [-uvCHhmBx]`。默认不启用。可以由`echo $-` 查看，如显示`himBH`，则表示`himBH`这5个选项被开启。

建立一个子程序，如bash，之前的父程序会sleep。子程序继承环境变量global variable不继承自定义变量local variable。因为每启动一个shell，OS分配一片内存。子程序只导入父程序的环境变量内存区域。

查看支持的语系： `locale -a`查看所有。`locale`可查看有关语言的各设置，都是用变量存储的。
- 整体系统默认的语系定义在`/etc/sysconfig/i18n`文档里。

读取键盘输入：`read [-pt] variable`。`read -p "Please keyin your name: " -t 30 named`。选项：
- `-p`：后面可以接提示字符。
- `-t`：后面可以接等待的秒数。到时停止。

定义变量类型：`declare`返回所有变量名与值。变量类型默认为字符串。`declare [-aixr] variable`。选项：
- `-a`：将后面名为variable的变量定义成为数组(array)。
- `-i`：将后面名为variable的变量定义成为整数数字(integer)。
- `-x`：变为环境变量。+x则变回局部变量。
- `-r`：将变量设为readonly类型，不可被更改内容或unset。需注销后重新登录才能恢复。
- `-p`：列出一个变量的类型。`declare -p sum`。

一般选项中 `-参数` 代表设置该选项，`+参数` 代表取消设置。

数组：赋值：`var[index]=content`。从0开始。读取：`echo "${var[1]}, ${var[2]}, ${var[3]}"`或`${数组}`显示第一个元素。

定义变量类型：`typeset`。与`declare`类似。

限制用户使用系统资源的额度：`ulimit [-SHacdfltu] [配额]`。选项：
- `-H`：hard limit，必定不能超过这个数值。
- `-S`：soft limit，可以超过，但是有警告讯息。通常soft比hard小
- `-a`：后面不接任何选项参数，可列出所有的限制额度。0代表无限制。
- `-c`：限制每个核心档案的最大容量。当某程序出错时，系统可能会将该程序在内存中的信息写成档案以便除错。这种档案就被称为核心档案(core file)。
- `-f`：此 shell 可以建立的最大档案容量(一般可能为 2GB)单位为 Kbytes。非root用户只能设定更小的值。
- `-d`：程序可使用的最大断裂内存(segment)容量。
- `-l`：可用与锁定(lock)的内存量。
- `-t`：可使用的最大 CPU 时间 (单位为秒)。
- `-u`：单一用户可以使用的最大程序(process)数量。

变量值修改：
- 从左开始删除最短的匹配：用`#`。`${var#delete_string}`。`delete_string` 可以用通配符，如`echo ${path#/*kerberos/bin:}`可删除从头开始的`/...bin:`这串字符。
- 从左开始删除最长的匹配：用`##`。`${path##/*:}`。
- 从右往左删除：`%`和`%%`。`${path%:*bin}`删除最后的一个`:`到`bin`之间的字符。`${path%%:*bin}`删除第一个`:`到`bin`之间的字符串。
- 取代一次：`${var/ori/new}`。将`var`中的第一个`ori`的字符串变为`new`的字符串。
- 取代所有：`${var//ori/new}`。
- 未设定则赋新值：`new_var=${old_var-content}`。如果`old_var`已设定，就是`old_var`，不然为`content`。注意空字符`""`不算未设定。
- 未设或为空字符：`new_var=${old_var:-content}`。
- 已设定或为空则赋值：`new_var=${old_var+content}`。
- 已设则赋值：`new_var=${old_var:+content}`。
- `var=${str=expr}`：`str`未设：`str=expr; var=expr`；`str`为空：`str`不变，`var=` 。`str`已设：`str`不变，`var=$str`。
- `var=${str:=expr}`：`str`未设：`str=expr; var=expr`；`str`为空：`str=expr; var=expr`；`str`已设：`str`不变，`var=$str`。
- `var=${str?expr}`：`str`未设：`expr`输出至`stderr`；`str`为空：`var=` ；`str`已设：`var=$str`；
- `var=${str:?expr}`：`str`未设：`expr`输出至`stderr`；`str`为空：`expr`输出至`stderr`；`str`已设：`var=$str`；

`stderr`的error code可用`?`变量来查看。

清除画面：`clear`。

查询曾经下达过的指令：`history`。
- `history [n]`：列出最近的n笔命令。第一栏为shell中的编号。
- `history [-c]`：将目前shell中的所有 history 内容全部消除。
- `history [-raw] histfiles`。选项：
- `-a`：将目前新增的history指令写入histfiles中。预设写入`~/.bash_history`。其中最多记录的历史数由`$HISTFILESIZE`决定。
- `-r`：将histfiles的内容读入目前这个shell的history 记忆中。
- `-w`：将目前的history记忆内容全写入histfiles中。

执行历史命令：要注意安全问题，不能让`.bash_history`受到黑客攻击。
- `!number`：执行编号为number的指令。
- `!command`：搜索最近的开头为command的指令串并执行。
- `!!`：执行上一个指令(相当于↑+Enter)。
- 当同时开了数个bash时，后关闭的bash会覆写之前的history。而单一bash登入，用`job control`来切换工作可避免该问题。
- 可修改`~/.bash_logout` 记录bash退出的时间。

同名指令的执行顺序：等同于`type -a command` 找到的顺序。

1. 以相对/绝对路径执行的指令。如`/bin/ls`或`./ls`。
2. 由`alias`找到的指令。
3. bash内建的(builtin)指令。
4. `$PATH`按序搜寻到的第一个指令。

Shell:
- login shell：如tty1取得的bash，需要登录密码。
- non-login shell：不需重复登录，如X window登录后取得的bash。
- bash的欢迎信息：在`/etc/issue`, `/etc/motd`档案内。
- bash环境配置：注销bash后在bash内的别名，变量均失效，除非写入配置文件。
- login shell只读取这两个配置文件：
  1. `/etc/profile`：这是系统整体的设定，不要修改这个档案。
  2. `~/.bash_profile`或`~/.bash_login`或`~/.profile`：使用者个人设定。

`/etc/profile`：login shell才会读。
使用使用者的标识符UID决定变量值，如PATH，MAIL，USER，HOSTNAME，HISTSIZE。
读入文档数据：`/etc/inputrc`的键盘设置， `/etc/profile.d/*.sh`决定bash的设置，`/etc/sysconfig/i18n`的语言设置。

个人配置：依次查找：`~/.bash_profile`，`~/.bash_login`和`~/.profile`，如存在则读取，并忽略之后的文档。

让配置文件立即生效：`source 配置文件` 或 `. 配置文件`。注销后登录也可。

`~/.bashrc`：non-login shell会读。是`/etc/skel/.bashrc`复制。

`/etc/bashrc`：由OS读取。根据UID决定umask；提示字符PS1变量，并呼叫`/etc/profile.d/*.sh`的设定。

其他配置：
- `/etc/man.config`
- `~/.bash_history`
- `~/.bash_logout`

终端机设定：`stty -a`显示设定，`stty 功能 键盘输入`。
- `^`：就是ctrl。
- `^?`：backspace。
- 功能列表：
  - `eof`：End of file，代表结束输入。
  - `erase`：向后删除字符，
  - `intr`：送出一个 interrupt的讯号给目前正在run的程序；
  - `kill`：删除在目前指令列上的所有文字；
  - `quit`：送出quit的讯号给目前正在run的程序；
  - `start`：在某个程序停止后，重新启动它的output。
  - `stop`：停止目前屏幕的输出；
  - `susp`：送出一个terminal stop讯号给正在run的程序。

bash组合键：
- `Ctrl + C`：终止目前的命令
- `Ctrl + D`：输入结束 (EOF)，例如邮件结束的时候；
- `Ctrl + M`： 就是 Enter；
- `Ctrl + S`： 暂停屏幕的输出
- `Ctrl + Q`： 恢复屏幕的输出
- `Ctrl + U`： 在提示字符下，将整列命令删除 
- `Ctrl + Z`： 暂停目前的命令

bash的wildcard：
- `*`：0到无穷多个任意字符。
- `?`：一定有一个任意字符。如`ll -d /etc/?????` 找出`/etc/`下文件名刚好是五个字母的文件。
- `[ ]`：一定有一个在括号内的字符。如`[abcd]`代表一定有一个可能是a, b, c, d中的一个的字符。
- `[ - ]`：在编码顺序内的所有字符。如`[0-9]`代表0到9间的所有数字，因为数字的语系编码是连续的。`ll -d /etc/*[0-9]*`。
- `[^ ]`：反向选择，如`[^abc]`代表一定有一个不是a, b, c的字符。`cp -a /etc/[^a-z]* /tmp`。

bash中的特殊字符：
- `#`：注释符号
- `\`：跳脱符号，将特殊字符或通配符还原成一般字符
- `|`：管线(pipe)，分隔两个管线命令的界定； 
- `;`：连续指令下达分隔符：
- `~`：用户的家目录 
- `$`：取用变量前导符：
- `&`：工作控制(job control)，将指令放于背景下工作
- `!`：非逻辑运算符
- `/`：目录符号，即路径分隔的符号
- `>`：数据流重导向，输出导向，取代。`ll / > ~/rootfile.txt`。如存在同名文档，会覆盖。
- `>>`：数据流重导向，输出导向，累加。如存在同名文档，会追加到结尾。
- `<`：数据流重导向，输入导向
- `<<`：数据流重导向：输入导向的结束字符
- `' '`：单引号，不具有变量置换的功能
- `" "`：具有变量置换的功能
- &#96; &#96;：中间为可以先执行的指令，等同于`$( )`
- `( )`：在中间为子shell的起始与结束
- `{ }`：在中间为命令区块的组合

数据流输出重导向：将本应输出到屏幕上的数据传输到别处，即standard output(stdout)和standard error output(stderr)传输到当然或装置。
- stdin：代码为0，使用`<`或`<<`。
- stdout：代码为1，使用`>`或`>>`。
- stderr：代码为2，使用`2>`或`2>>`。`find /home -name .bashrc > ~/list_right 2> list_err`
将正确和错误的数据写入同一个档案：`find /home -name .bashrc > list 2> &1`。或 `find /home -name .bashrc &> list`。

由键盘输入创建档案：
```
cat > catfile # cat without file can cat from stdin
testing
cat file test
^D
```

`/dev/null`：垃圾桶黑洞装置，所有导向这个装置的信息都会丢弃。

数据流输入重导向：将本键盘输入的数据由档案内容取代。
全部导入：`cat > catfile < ~/.bashrc`。
导入至结束字符串：`cat > catfile << "eof"`。则当输入`"eof"`时自动结束（不会输入`"eof"`到文档）而不需`ctrl+D`。

条件执行：利用`$?`的回传值决定指令是否执行。
- `cmd1 && cmd2`：cmd1执行成功才执行cmd2。`ls /tmp/abc && touch /tmp/abc/hehe`。
- `cmd1 || cmd2`：cmd1执行失败才执行cmd2。`ls /tmp/abc || mkdir /tmp/abc`
- 可混合执行：`ls /tmp/abc || mkdir /tmp/abc && touch /tmp/abc/hehe`。前一个操作`$?`的值传到后一个操作。

pipe：处理经前一个指令传来的stdout，不能处理stderr。管线后可接`less`，`more`，`head`，`tail`等可接受stdin的指令，而`ls`，`cp`，`mv`等不行。`ls -al /etc | less`。即`less ll /etc`的结果。

截取指令：将一段数据分析后取出需要的一行，配合管线使用。
- `cut -d '分隔字符' -f fields`：依据`-d`的分隔字符将一段讯息分割成为数段，用`-f`取出第几段。`echo $PATH | cut -d ':' -f 3,5`即取出PATH变量中的第3和第5个变量。注意每一个空格算一个`' '`。
- `cut -c 字符区间`：以字符(characters)为单位取出固定字符区间。`export | cut -c 12-` 即输出所有变量的从第12个字符开始的设置。`12-20`则定义了个区间。

`grep [-acinv] [--color=auto] '搜寻字符串' filename`：搜索存在搜索信息的一行并取出。`last | grep 'root' | cut -d ' ' -f 1`
- `-a`：将binary档案以text档案的方式搜寻数据。
- `-c`：计算找到搜寻字符串的次数。
- `-i`：忽略大小写的不同。
- `-n`：输出行号。
- `-v`：显示出没有搜寻字符串内容的那一行。`last | grep -v 'root'` 显示过去登录的非root信息。`grep -vn 'the' file`。
- `--color=auto`：可以将找到的关键词部分加上颜色的显示。`grep --color=auto 'MANPATH' /etc/man.config`
- `-l`: 只显示档案名. `find / -type f | grep -l '字符串'`。


排序：`sort`，先用`LANG=C` 来保证编码。`sort [-fbMnrtuk] [file or stdin]`。
- `-f`：忽略大小写的差异。
- `-b`：忽略最前面的空格符部分；
- `-M`：以月份的名字来排序。
- `-n`：使用纯数字进行排序，默认以文字型态排序。
- `-r`：反向排序。 
- `-u`：就是`uniq`，相同的数据中，仅出现一行。
- `-t`：分隔符，预设是用tab分隔； 
- `-k`：以那个区间(field)来排序。`cat /etc/passwd | sort -t ':' -k 3 -n` 则按由`:`分隔的第3栏排序。注意如果按文字排序，则是0,10,11,1的顺序。

相同行只显示一行：`uniq [-ic]`，用在排序之后。
- `-i`：忽略大小写字符的不同；
- `-c`：进行计数。`last | cut -d ' ' f 1 | sort | uniq -c`

文档信息统计：`wc [-lwm]`。`cat /etc/man.config | wc`。输出的三个数依次代表行数，词数，字符数。
- `-l`：仅列出行；
- `-w`：仅列出多少字(英文单字)；
- `-m` ：多少字符；

输出同时导向文档与屏幕：`tee [-a] file`。`last | tee last.lst | cut -d " " -f 1`
- `-a`：累加(append)数据到file中。

字符转换：
- 删除或替换：`tr [-ds] SET1 ...`。`last | tr '[a-z]' '[A-Z]'`。没有单引号也可执行。
- `-d`：删除讯息当中SET1字符串；`cat /etc/passwd | tr -d :`
- `-s`：取代重复的字符。
- 转换大小写：`tr 'a-z' 'A-Z'`

一个栗子：
```
cp /etc/passwd /root/passwd && unix2dos /root/passwd
file /etc/passwd /root/passwd
cat /root/passwd | tr -d '\r' > /root/passwd.linux # \r is the ^M$
```

将特殊格式的文档转换为纯文本：`col [-xb]`。
- `-x`：将tab 键（`^I`）转换成对等的空格键。`cat /etc/man.config | col -x | cat -A | more`
- `-b`：在文字内有反斜杠`/`时，仅保留反斜杠最后接的那个字符。`man col | col -b > /root/col.man`。对在bash中有颜色显示的字有效。

合并相同数据：`join [-ti12] file1 file2`。需要文档已经排过序。
- `-t`：join默认以空格符分隔数据，并比对第一个字段的数据。如果两个档案相同，则将两笔数据联成一行。`join -t ':' /etc/passwd /etc/shadow`。
- `-i`：忽略大小写的差异；
- `-1`：第一个档案要用该字段来分析。
- `-2`：第二个档案要用该字段来分析。`join -t ':' -1 4 /etc/passwd -2 3 /etc/group`。

拼接行：`paste [-d] file1 file2`。将两个文档中的每行拼成一行，用分隔字符隔开。
- `-d`：分隔字符。预设是tab。`cat /etc/group | paste /etc/passwd /etc/shadow - | head -n 3`

将tab转换为space：`expand [-t] file`。并不是每个tab都会转为固定数目的space
- `-t`：接数字表示一个tab代表几个space。预设是8个。`grep '^MANPATH' /etc/man.config | head -n 3 | expand -t 6 - | cat -A`

space转换为tab：`unexpand`。

分割文档：`split [-bl] file PREFIX`
- `-b`：分隔档案大小，可加单位 b, k, m 等；`split -b 300k /etc/tempcap termcap; ll -k termcap*`。文档会按prefix+a,b,c…的顺序产生。
- `-l`：以行数进行分割。`ls -al | split -l 10 - lsroot`。
- `PREFIX`：决定分割档案的前导文字。

参数代换：`xargs [-0epn] command`。`cut -d ':' -f 1 /etc/passwd | head -n 3 | xargs finger`。
- 对不支持管线的命令非常实用，如`ls`。注意档案名中最好不能有空格，不然会误判。
- `-0`：如果输入的stdin有特殊字符，如&#96;, `\`, 空格键等，可以将他还原成一般字符。
- `-e`：EOF的字符串，分析到这个字符串就停止。`cut -d ':' -f 1 /etc/passwd | xargs -p -e'lp' finger`。中间没有空格。
- `-p`：在执行每个指令时都会询问。
- `-n`： command指令执行时，要几个参数。`cut -d ':' -f 1 | xargs -p -n 5 finger`。
- 当 xargs 后面没有接任何的指令时，默认是以`echo`来进行输出
- `find /sbin -perm +7000 | xargs ls -l`。

取得账号的信息：`finger`。

行首的代表标注为`^`。

`-`：某些指令要用文件名做参数，如`tar`。则如果file 部分写成`-`，则为stdin或者stdout。
```
tar -cvf - /home | tar -xvf -
```
前一个`-`为stdout，后一个为stdin，即文件移动。

在`~/.bash_history`中记录时间：
```
# vim ~/.bash_logout 
date >> ~/.myhistory 
history 50 >> ~/.myhistory
```

```
A=B # $A is B
B=C
unset $A # Equal to unset B, now $B unset
```


## Chapter 12
正则表达式Regular Expression：以行为单位处理字符串。

不是程序而是字符串处理标准。需要支持RE的工具程序，如`vi`，`sed`，`awk`。而`cp`, `ls`等指令不支持，只能使用bash自身的通配符。

BASH: linux基本指令。

邮件服务器软件：`sendmail`与`postfix`。

扩展正则表达式：通过`(`和`|`等进行或逻辑的多字符串查找。

编码：`LANG=C`（兼容POSIX标准）：`ABC...Zabc...z`；`LANG=zh_TW`：`aAbBcC...zZ`。所以用`[A-Z]`抓取大写字母，台湾语系会抓出小写。

特殊字符串：
- `[:alnum:]`：代表英文大小写字符及数字,即0-9, A-Z, a-z。
- `[:alpha:]`：代表任何英文大小写字符,即A-Z, a-z。
- `[:blank:]`：代表空格键和tab按键。
- `[:cntrl:]`：代表键盘上面的控制按键,包括CR,LF,Tab,Del...。
- `[:digit:]`：代表数字0-9。
- `[:graph:]`：除了空格符和Tab键外的其他所有按键。
- `[:lower:]`：代表小写字符a-z。
- `[:print:]`：代表任何可以被打印出的字符。
- `[:punct:]`：代表标点符号(punctuation symbol),即`" ' ? ! ; : # $...`。
- `[:upper:]`：代表大写字符A-Z。
- `[:space:]`：会产生空白的字符如空格键,Tab,CR等。
- `[:xdigit:]`：代表16进制数字类型,包括:0-9,A-F,a-f。

`grep [-A] [-B] [--color=auto] '搜寻字符串' filename`。不一定需要字符串一定是词。
- `-A n`：after,列出该行及后续的n行；`dmesg | grep -n -A3 -B2 -color=auto 'eth'` 显示网卡信息。
- `-B n`：before，列出该行及前面的n行；
- `--color=auto`：将正确的数据列出颜色。

列出核心配置信息：`dmesg`。

练习正则表达式：`wget http://linux.vbird.org/linux_basic/0330regularex/regular_express.txt`
- 虽然类似bash的wildcard，但是不完全相同，比如`*`和`?`无用，而有`.`和`*`。而且不以单词（及需要左右都为空格）为单位，搜索的是字符串。
- `grep -n 't[ae]st' file` 得到的是包含test 或taste 的行。
- `grep -n '[^a-z]oo' file` 或 grep -n '[^[:lower:]]oo' file`。
- `grep -n '[a-zA-Z0-9]' file` 搜索所有字母数字字符。
- `grep -n '^the' file` 利用制表符`^`代表行首。注意放在中括号中才是取反。
- `grep -n '\.$' file` 利用制表符`$`代表行尾。而小数点是特殊字符，需要反斜杠。这种方法windows平台的句尾无法搜索到。
- `grep -v '^$' file | grep -v '^#'` 去除空行和注释。
- 一个任意字符：`.`。`grep -n 'g..d' file`。
- 重复前面的那个字符0到无穷次：```*```。
- ```grep -n 'goo*g' file``` 查找至少2个连续的o。如果```'o*'```的话会输出全部内容。
- ```grep -n 'g.*g' file``` 查找由g开头和结尾的字符串。
- ```grep -n '[0-9][0-9]*' file``` 等同于`grep -n '[0-9]' file`。
- 重复一定范围的次数：用`{a,b}`，因为`{`是特殊字符，要跳脱。`grep -n 'go\{3,5\}' file`。`{3,}`代表3个及以上。
- 注意`!`和`>`在这里不是特殊字符。

查看某目录下链接文件的属性：`ls -l /etc/ | grep '^l' | wc -l`

分析stdin：`sed [-nefr] [动作]`
- `-n`：silent模式。即不将STDIN的数据列到屏幕上，只有经sed处理的那一行。
- `-e`：直接在指令列模式上进行sed的动作编辑；
- `-f`：直接将sed的动作写在一个档案内。
- `-f filename`：执行filename里的sed动作；
- `-r`：支持扩展正则表达式。默认是基础正则表达式。
- `-i`：直接修改读取的档案内容，而不由屏幕输出。`sed -i 's/\.$/!/g' file`。
- 动作：`[n1][,n2] function`：n1，n2为行数范围。动作用单引号括住。function：
  - `a`：新增，字符串出现在每行下的新一行。`nl /etc/passwd | sed '2a drink tea\^Mdrink coffee?'`可以在原文件的第2行下增加两行。
  - `sed -i '$a # This is a test. ' file` 加到最后一行之后。
  - `c`：取代，字符串取代`n1,n2`之间的行。`nl /etc/passwd | sed '2,5c No 2-5 number'`。则第2到5行变为一行,且内容被替换。
  - `d`：删除。`nl /etc/passwd | sed '2,5d'`，`nl /etc/passwd | sed -e '2,$d'`。有无`-e`在管线方式下都一样。用正则表达式删除时d放在表达式后。
  - `i`：插入，字符串在当前的上一行出现。
  - `p`：打印，将某个选择的数据印出。与`sed -n`一起运作。`nl /etc/passwd | sed -n '5,7p'`。只显示5到7行。
  - `s`：取代，直接进行取代的工作，搭配正则表示法。`1,20s/old/new/g`。```/sbin/ifconfig eth0 | grep 'inet addr'| sed 's/^.*addr://g'```删除行首至`addr:`。

删除注释行：
```
cat /etc/man.config | grep 'MAN' | sed 's/#.*$//g' | sed '/^$/d'
```

可以并列多个动作：
```
cat /etc/passwd | sed -e '4d' -e '6c no six line' > passwd.new
```

扩展正则表达式：需要`grep -E` 或者指令`egrep`支持。
- 一个或一个前一个字符：`+`。`egrep -n 'go+d' file`。找到god, good, goood。
- 零个或一个前一个字符：`?`。`egrep -n 'go?d' file`。找到gd 和god。
- 用`|`代表or：`egrep -n 'gd|good|dog' file`。同时找三个词。`egrep -v '^$|^#' file`。排除空白行和注释行。`egrep -n 'g(la|oo)d' file`。找glad 和good。
- 字符串的重复：`echo 'AxyzxyzxyzC' | egrep 'A(xyz)+C'`。找的是A开始C结束，且当中有xyz重复1次以上的字符串。

格式化打印：`printf '打印格式' 实际内容`。不是管线命令。格式的几个特殊样式：
- `\a`：警告声音输出。
- `\b`：backspace。
- `\f`：清除屏幕(form feed)。
- `\n`：输出新的一行。
- `\r`：Enter。
- `\t`：水平tab按键。`printf '%s\t %s\t %s\t\n' $(cat printf.txt)`。将printf.txt的内容用每行三段的格式输出。
- `\v`：垂直tab按键。
- `\xNN`：两位数NN转换为ASCII中该16进制数值代表的字符。`printf '\x45\n'`。
- C语言的变量格式： 
  - `%ns`：该区间长度为n个字符；`printf '%10s\t %5i\t %5i\t %8.2f\n' $(cat printf.txt | grep -v Name)`。
  - `%ni`：长度为n个字符的整数；
  - `%N.nf`: 长度为N个字符的整数和小数点后n位的浮点数。

处理字段：`awk '条件类型1{动作1} 条件类型2{动作2} ...' filename`。可读取文件或stdin。字段需以空格或tab分隔。
- `{}`内的两条指令之间要用`;`或enter分隔。
- 内建变量：
  - `NF`：每一行的字段总数；
  - `NR`：目前处理行的行数；
  - `FS`：当前的分隔字符，默认是空格。
- `last -n 5| awk '{print $1 "\t lines:" NR "\t columes:" NF}'`。
- `last -n 5| awk '{print $1 "\t" $3}'`。打印出第1和第3个字段，并由tab分隔。`$0`代表一整行。
- 逻辑判断：
  - 需要在之前加入BIGIN，不然第一行会不能正确处理：`cat /etc/passwd | awk 'BEGIN {FS=":"} $3<10 {print $1 "\t" $3}'`。
  - `cat pay.txt | awk 'NR==1 {printf "%10s\t %10s\t %10s\n"} NR>=2 {total=$2+$3+$4; printf "%10s %10d %10.2f\n",$1,$2,total}'`。
  - if语句：`cat pay.txt | awk '{if(NR==1) printf "%10s %10s %10s\n",$1,$2,$3,"Total"} NR>=2 {total=$2+$3+$4; printf "%10s %10d %10d %10d", $1,$2,$3,total}'`。
- 还可循环。

文本档案差异对比：`diff [-bBi] from-file to-file`。
- 显示信息说明：`4d3`表明以右文档的第3行为基准，左文档的第4行被删。`6c5`表明左第6行被替代为右第5行。
- 以行作为单位。
- `from-file`：原始比对档案；可以`-`取代。
- `to-file`：目的比对档案；可以`-`取代。
- `-b`：忽略一行当中仅有多个空格的差异。
- `-B`：忽略空白行的差异。 
- `-i`：忽略大小写的差异。
- 文件夹差异对比：`diff /etc/rc3.d/ /etc/rc5.d/`。
- 根据新档案的不同处制作升级补丁：`diff -Naur passwd.old passwd.new > passwd.patch`。

在文件上打`diff`生成的补丁:
- 使用补丁：`patch -pN < patch_file`。`N`为目录删减数。0表示需打补丁的文件就在当前目录。`patch -p0 < passwd.patch`。
- 将升级过的文档恢复为旧文档：`patch -R -pn < passwd.patch`。`patch -R -p0 < passwd.patch`。
- 目录删减数：当patch是用`diff`比较的目录时，根据生成patch时所在的目录计算。

字节档案对比：`cmp [-s] file1 file2`。以字节为单位。结果为byte 和line 的位置。
- `-s`：将所有不同的字节处列出来。预设仅会输出第一个出现的不同点。

档案打印预览：`pr /etc/man.config`。

通配符与正则表达式搭配：```grep '\*' /etc/*```。

搜索子文件夹：```grep '\*' $(find /etc -type f)```。因为`/etc`下有太多文件，会因为指令长度太长而报错。

分批次搜索：```find /etc -type f | xargs -n 10 grep '\*'```。

设置个指令查看ip地址：
```
alias myip="ifconfig eth0 | grep 'inet addr' | sed 's/^.*inet addr://g' | cut -d '' -f1"
MYIP=$(myip)
```
将这两行写入`.bashrc`。

## Chapter 13
系统服务启动的接口：`/etc/init.d/`目录下，全是scripts。如启动系统注册表,`/etc/init.d/syslogd restart`。`/etc/init.d/syslog stop`。

防火墙连续规则：`iptables`。


Shell Script规则：

1. 从上而下、从左向右执行；
2. 指令、选项与参数间的多个空格视为1个；
3. 空白行忽略，tab视为空格；
4. 读取到Enter (CR)时开始执行命令；
5. 如果一行的内容太多，可以使用`\Enter`；
6. `#`是批注符。


执行script：
- 当shell.sh有`rx`权限，可直接通过档案名执行。
- 或通过bash程序来执行：`bash shell.sh`或`sh shell.sh`。
- `sh [-nx] shell.sh`可以检查语法。
- 这时是在一个子bash程序中执行的，在script中的变量是不会传回原bash的。
- 利用`source shell.sh`来执行，可保持在原bash中执行，所以各项指令结果都有效。

显示Hello World的script：
```
#!/bin/bash # 宣告使用的bash名称，就可以加载bash的相关环境，一般是non-login shell。
# Program: # 批注说明内容与功能，版本信息，作者联系方式，建档日期，历史记录。
#     This program shows "Hello World!" in your screen. 
# History: 
# 2005/08/23 VBird First release 
PATH=/bin:/sbin:/usr/bin:/usr/sbin:/usr/local/bin:/usr/local/sbin:~/bin # 将主要环境变量设置好，避免使用绝对路径。
export PATH
echo -e "Hello World! \a \n" # -e选项：发出提示音。
exit 0 # 可用$?查看。
```

将该脚本转为可执行脚本：`chmod a+x sh01.sh`;

读入键盘输入：
```
read -p "Please input your first name:" firstname
read -p "please input your last name:" lastname
echo -e "\nYour full name is $firstname $lastname"
```

设定文件名为日期：
```
read -p "please input your filename" fileuser
filename=${fileuser:-"filename"}
date1=$(date --date='2 days ago' +%Y%m%d) # --date用文字设定日期，如now。
date2=$(date --date='1 days ago' +%Y%m%d)
date3=$(date +%Y%m%d)
file1=${filename}${date1}
file2=${filename}${date2}
file3=${filename}${date3}
touch "$file1"
touch "$file2"
touch "$file3"
```

两数字相乘：
```
read -p "first number: " firstnu
read -p "second number" secondnu
total=$(($firstnu*$secondnu)) # 将两输入的字符串变为整数并运算，等同于declare -i total=$firstnu*$secondnu
echo -e "\nResult ==> $total"
```

`$((运算式))`支持运算：```+-*/%```。运算式中可以有空格。

测试档案是否存在：`test [-efdrwx] filename`。
- `-e`：是否存在。`test -e /dmtsai && echo "exist" || echo "Not exist"`。
- `-f`：是否存在且为档案(file)。
- `-d`：是否存在且为目录(directory)。
- `-r`：是否存在且具有可读权限。
- `-w`：是否存在且具有可写权限。
- `-x`：是否存在且具有可执行权限。

测试两个档案：`test file1 [-nt-ot-ef] file2`。
- `-nt`：(newer than) file1是否比file2新。
- `-ot`：(older than) file1是否比file2旧。
- `-ef`：file1与file2是否为同一档案，即是否hard link。

测试两个整数：`test n1 [-eq-ne-gt-lt-ge-le] n2`。

判断字符串：
- `test -z string`：判断字符串是否为空，为空输出true。
- `test -n string`：是否为空，为空输出false。
- `test str1 = str2`：相等返回true。
- `test str1 != str2`。

多重判断：`-a`：and。`-o`：or。`!`：not。
- `test -r filename -a -x filename`。同时具有可读和可执行权限时，返回true。

判断档案是否存在，什么类型和输出权限：
```
read -p "Input a filename: \n\n" filename
test -z $filename && echo "Empty String" && exit 0
test ! -e $filename && echo "Not exist" && exit 0
test -f filename && filetype="regular file"
test -d filename && filetype="directory"
test -r filename && perm="Readable"
test -w filename && perm="$perm Writeable"
test -x filename && perm="$perm Executable"
echo "$filename is $filetype"
echo "Permissions are $perm"
```

该脚本用root执行会与`ls -l`看到的不同，因为权限限制多数对root无效。

判断符号：
- bash中使用`[]`作为判断，类似于test。
- 为避免被误认为正则表达式，需要在括号中每个组件前后加入两个空格。
- 变量都以双引号括起来，常数用引号括起来，使得这些量被bash替换为值时视为一个量。（鸟哥的私房菜P456）

```
[ -z "$HOME" ]; echo $; # 判断是否为空。
[ "$HOME" == "$MAIL" ] # 判断是否相同。
```

判断输入的Y或N：
```
read -p "Please input (Y/N)" yn
[ "$yn" == "Y" -o "$yn" == "y" ] && echo "yes" && exit 0
[ "$yn" == "N" -o "$yn" == "n" ] && echo "no" && exit 0
echo "Invalid input" && exit 0
```

`/path/to/scriptname opt1 opt2 opt3 opt4`对应`$0,$1,$2,$3,$4`。即脚本档名为`$0`。

`$#`代表参数个数，`$@`代表`"$1" "$2" "$3" "$4"`，```$*```代表`$1分隔$2分隔$3分隔$4分隔`。分隔字符默认为空格。

```
echo "Script name is $0"
echo "Parameter number is $#"
[ $# -lt 2 ] && echo "too less parameters" && exit 0
echo "Your whole parameters are '$@'" 
```

参数编码偏移：`shift n0`，抛弃前n个参数。

```
echo "Parameter number is $#"
shift
echo "Parameters are '$@'"
shift 2
echo "Parameters are '$@'"
```

条件判断：
```
if [ 条件判断 ] && [ 条件判断 ]; then
    执行指令;
elif [ 条件判断 ]; then
    执行指令; 
else
    执行指令;
fi
```

```
read -p "Please input Y/N" yn
if [ '$yn' == 'Y' ] || [ '$yn' == 'y' ]; then
    echo "yes"
elif [ '$yn' == 'N' ] || [ '$yn' == 'n']; then 
    echo "no"
else
    echo "Invalid input"
fi
```

```
if [ "$1" == "Hello" ]; then
    echo "Hello"
elif [ "$1" == "" ]; then
    echo "You must input parameters, ex> {$0 someword}" #输出结果为 You must input parameters, ex> {script.sh someword}。为很多启动系统服务的scripts的写法。
else
    echo "Can only input 'Hello', ex>{$0 hello}"
fi
```

查询主机开启的网络服务端口：`netstat -tuln`, 查看使用了tcp或udp的正在监听的端口, 显示端口号。

`127.0.0.1`对本机地址，`0.0.0.0`或`:::`对整个internet。

常见的网络服务port：`80`: WWW，`22`: ssh，`21`: ftp，`25`: mail，`111`: RPC(进程过程调用)，`631`: CUPS(打印服务功能), `53`: DNS。

```
testing=$(netstat -tuln | grep ":80 ")
if [ "$testing" != "" ]; then
    echo "Your WWW server is on. "
if
testing=$(netstat -tuln | grep ":22 ")
if [ "$testing" != "" ]; then
    echo "Your ssh server is on. "
if
```

计算日期相差时间
```
read -p "Please input date (YYYYMMDD ex> 20070401): " date2

date_d=$(echo $date2 | grep '[0-9]\{8\}')
if [ "$date_d" == "" ]; then
    echo "Invalid input"
    exit 1
fi

declare -i date_dem=`date --date="$date2" +%s`
declare -i date_now=`date +%s`
declare -i date_total_s=$(($date_dem-$date_now))
declare -i date_d=$(($date_total_s/60/60/24)) #转换为天数
if [ "$date_total_s" -lt "0" ]; then
    echo "Before" $((-1*$date_d)) "days ago" 
else
    declare -i date_h=$(($(($date_total_s-$date_d*60*60*24))/60/60))
    echo "After $date_d days and $date_h hours. "
fi
```

Switch 判断：
```
case $变量名称 in
 "变量值")
    执行指令
    ;;
 "变量值")
    执行指令
    ;;
 *) #代表其它值
    执行指令
    exit 1
    ;;
esac
```

```
case $1 in
 "Hello")
    echo "Hello, how are you?"
    ;;
 "")
    echo "You must input parameters, ex> {$0 someword}"
    ;;
 *)
    echo "Usage $0 {Hello}"
    ;;
esac
```

自定义函数：一定要在script调用该函数的部分的前面。`$0`代表fname，`$1`代表调用函数时后面接的第一个参数, `$2`依此类推...
```
function fname() {
    程序块
}
```

```
PATH=/bin:/sbin:/usr/bin:/usr/sbin:/usr/local/bin:/usr/local/sbin:~/bin
export PATH
function printit(){
     echo -n "Your choice is $1"
}
case $1 in
 "One")
    printit 1; echo $1 | tr 'a-z' 'A-Z' 
    ;;
 "Two")
    printit 2; echo $1 | tr 'a-z' 'A-Z'
    ;;
 *)
    echo "Usage $0 {one|two}"
    ;;
esac
```
输出则是Your choice is 1 ONE，或Your choice is 2 TWO。

循环：
```
while [ condition ]
do
    程序段落
done
```

或
```
until [ condition ] #条件达成是种值循环。
do
    程序块
done
```

或
```
for var in con1 con2 con3
do
    程序块
done
```

或
```
for ((初始值;限制值;步长))
do
    程序块
done
```

例子1：
```
while [ "$yn" != "yes" -a "$yn" != "YES" ]
do 
    read -p "please input yes"
done
echo "OK! "

until [ "$yn" == "yes" -o "$yn" == "YES" ]
do 
    read -p "please input yes"
done
echo "OK! "
```

例子2：
```
s=0
i=0
while [ "$i" != "100" ]
do
    i=$((i+1)) # doesn't matter if it is $i or i
    s=$(($s+$i))
done
echo "result is $s"
```

例子3：
```
for animal in dog cat elephant
do
    echo "There are ${animal}s"
done
```

例子4：
```
users=$(cut -d ':' -f1 /etc/passwd)
for  username in $users
do
    id $username
    finger $username
done
```

例子5：
```
network="192.168.1"
for sitenu in $(seq 1 100)
do
    ping -c 1 -w 1 $(network).$(sitenu) &> /dev/null && result=0 || result=1
    if [ "$result" == 0 ]; then
        echo "Server is running"
    else
        echo "Server is down"
    fi
done
```

例子6：
```
read -p "input a directory: " $dir
if [ "$dir" == "" -o ! -d "$dir" ]; then
    echo "not valid"
    exit
fi

filelist=$(ls $dir)
for filename in filelist # if files have space in name, this does not work
# for filename in $dir/* can do the trick
do
    perm=""
    test -r "$dir/$filename" && perm="$perm readable"
    test -w "$dir/$filename" && perm="$perm writeable"
    test -x "$dir/$filename" && perm="$perm executable"
    echo "The file is $perm"
done
```

例子7：
```
read -p "input end number" nu
s=0
for ((i=1;i<$nu;i=i+1))
do
    s=$(($s+$i))
done
echo "result $s"
```

Debug功能：`sh [-nvx] scripts.sh`。
- `-n`：检查语法； 
- `-v`：执行script前将脚本内容输出；即执行时输出原脚本，如有结果输出，一起输出。
- `-x`：将使用到的script内容显示出来。

>
## Chapter 14
Linux 账号管理与ACL 权限设定

!!Skip!!

## Chapter 15
磁盘配额(Quota)与进阶文件系统管理

!!Skip!!

## Chapter 16
例行性工作排程 (crontab)

!!Skip!!

## Chapter 17
进程(process): 
- 正在运作的, 在内存内的数据
- 触发一个事件后产生一个进程, 系统给其分配PID
- 可由PID 得到该进程的权限, 由执行者的UID和GID决定
- 由PID可知该进程所需的资料

父进程(Parent):
- 子进程的PPID就是父进程
- 子进程获得父进程的环境变量
- fork-and-exec: 父复制一个子进程再执行

守护进程(deamon): 常驻内存的进程
- crond: 每分钟扫描`/etc/crontab`
- syslog
- 网络进程: 会启动监听端口(port)
- Apache
- named
- postfix
- vsftpd

CPU调度: 轮换时间片

`/etc/inittab`管理七个终端。各终端可用`alt+F1~7`切换。

`/etc/security/limits.conf`设置使用者最多可同时登录数。

bash 工作管理(job control):
- 出现提示字符的是前台(foreground)
- `cp file1 file2 &`: 放于后台(background)执行。
- 有一个job number, 在`[]`中, 之后为PID。PID部分会变Done。
- stdin不能连接背景中的进程, 但stdout和stderr仍显示到terminal。
- 在前台执行的程序, `ctrl+z`进入后台, 并被暂停(stopped)。
- 使用`bg`继续运行后台工作, `fg`恢复
- 登出(脱机)时后台工作也终止, 因为工作管理在终端下。除非用`at`让工作在系统背景中运行, 或`nohup`。

观察后台工作: `jobs [-lrs]`
- `-l`: 列出PID
- `-r`: 列出正在run的
- `-s`: stop的
- 工作前的`+`表示最后一个放入后台的, `-`表示倒数第二个

恢复后台工作到前台: `fg %jobnumber`, 如`fg %1`, 或`fg -`

令后台工作恢复运行: `bg %num`

管理工作: `kill -signal %num`
- `kill -l` 列出所有signal
- `1`: 重新读取配置文件 SIGHUP
- `2`: `ctrl+c` SIGINT
- `9`: 强制结束 SIGKILL
- `15`: 正常结束。默认值 SIGTERM
- `17`: `ctrl+z` SIGSTOP

`killall`类似。

`nohup`不支持bash内建指令:

sleep500.sh
```
#!/bin/bash
/bin/sleep 500s
/bin/echo "Hello World"
```

`nohup ./sleep500.sh &` 会输出nohup.out, 不会因为脱机而终止。

静态观察进程: `ps`
- `ps aux`: 显示固定内存RSS(KB)和虚拟VSZ(KB), 和状态STAT(R,S,T,Z)
- `ps -lA`: 显示
  - F: process flags, 4 为root, 1为fork但未exec
  - S: STAT, 有R(running), S(sleep, 正在idle, 可被signal唤醒), D(不可唤醒, 等待I/O), T(stop, 可能被暂停或traced), Z(zombie, 无法移除)
  - UID, PID, PPID
  - C: CPU usage
  - PRI, NI: Priority/Nice, 优先级
  - ADDR: kernel function, 在内存的哪里。running的process显示`-`
  - SZ: 占用内存size
  - WCHAN: 是否运作, `-`表示running
  - TTY: pts/n为动态终端接口, 为远程登录
  - TIME： CPU 运行时间
  - CMD
- `ps axjf`: 显示树结构, 等于`pstree`
- `ps -l`: 查看当前用户的进程, 包含更多信息。WCHAN表明该进程当前状态。
- `-A`: 所有process, 等于`-e`
- `-a`: 不与terminal 有关的。
- `-u`: effective user
- `x`: 与`a`一起使用显示完整消息
- `l`: 长显示, 包括PID
- `j`: jobs format
- `-f`: 更完整

僵尸进程:
- 执行完毕, 父程序无法完全释放其内存。defunct。

动态观察进程: `top`
- `-d`: 更新频率
- `-b`: 批次执行, 多配合数据流重导向
- `-n`: 批次执行几次。`top -b -n2 -p $$ >/tmp/toplog`
- `-p`: 接PID
- 操作:
  - `?`: help
  - `P`: 以CPU排序
  - `M`: 内存排序
  - `N`: PID排序
  - `T`: TIME排序
  - `k`: 给某PID一个signal
  - `r`: 给PID一个nice
  - `q`
- 第一行中的load average: 1, 5 15分钟系统平均要负责运作几个进程
- `%CPU`中`%wa`是I/O wait, 数值过大会使系统变慢。`1`可显示每个CPU核的负载
- `%swap`过大说明物理内存过小

`$$`是当前bash的PID。

`kill -signal PID`, 注意`kill -signal %num`的是num是job的ID, 不是PID.
- `kill -SIGHUP $(ps aux|grep 'syslog'|grep -v 'grep'|awk '{print $2}')` 重启syslog。可`tail -5 /var/log/messages`查看有否restart.

`killall [-iIe] [command name]`
- `-i`: 显示提示字符
- `-e`: exact, 与后面的command name要一致
- `-I`: 忽略大小写
- `killall -1 syslogd`

CPU 排程: priority 与nice
- PRI: 内核动态调整, 用户无法设置PRI值。越小越高
- NI: 用户可设。可为-20~19。正值使程序优先级便低, 一般用于备份
- `PRI(new)=PRI(old)+nice`, 但是因为PRI动态, PRI(old)并不固定
- `nice [-n 数字] command`
- `renice [number] PID`
- nice 值会传递给子进程

观察内存: `free [-b|-k|-m|-g] [-t]`
- `-bkmg`: 切换为单位
- `-t`: 显示物理和swap内存
- buffers和cached用完内存是正常的, swap用超过20%表示物理内存不足

查阅内核信息: `uname [-asrmpi]`
- `-a`: all
- `-s`: 名称
- `-r`: 版本
- `-m`: 硬件架构, `i686`或`x85_64`
- `-p`: CPU类型
- `-i`: 硬件平台

启动时间与负载: `uptime`

监控网络: `netstat -[atunlp]`
- `-a`: 全部
- `-t`: tcp
- `-u`: udp
- `-n`: 显示port number
- `-l`: 列出listen的服务
- `-p`: 显示PID
- 输出1: Active Internet connections (w/o servers), 联网的服务
  - Proto
  - Recv-Q, Send-Q: 总byte数
  - Local Address, Foreign Address
  - State: Established, listen等
- 输出2: Active UNIX domain sockets (w/o servers), 本机的程序。程序间的通信也使用socket.
  - Proto
  - RefCnt: 连接到此socket的进程数
  - Flags
  - Type: STREAM: 需要联机确认, DGRAM: 不需要
  - State: CONNECTED
  - I-Node
  - Path: 相关进程或数据输出的路径

内核信息: `dmesg`

系统资源: `vmstat [-a] [延迟 [总计侦测次数]]`
- `-a`: 内存显示inactive/active, 而不是预设的buffer/cache。
- `-f`: 系统fork 的程序数
- `-s`: 内存变化情况
- `-S`: 单位
- `-d`: 磁盘读写总量
- `-p`: 分割槽
- `vmstat 1 3`指延迟1s, 总共3次。
- procs: 程序数量。r: running, b: 不可被唤醒的
- memory: 
  - swpd: si: 磁盘中取出的量, so: 内存不足时写入磁盘的量
  - io: bi: 读取磁盘的区块量, bo: 写入磁盘的区块量
  - system: in: 每秒被中断的程序数, cs: 切换数
  - CPU: us: 非内核层的使用状态, sy: 内核, id: 闲置, wa: 等待I/O，st: virtual machine使用

特殊档案与程序 !!Skip!!

## Chpater 18

(26)

## Chapter 19
认识与分析登录档

!!Skip!!

## Chapter 20
开机流程、模块管理与Loader

!!Skip!!

## Chapter 21
系统设定工具(网绚与打印机)与硬件侦测

!!Skip!!

## Chapter 22

(28)

## Chapter 23

(33)

## Chapter 24
X Window 设定介绍

!!Skip!!

## Chapter 25
Linux 备份策略

!!Skip!!

## Chapter 26

(27)

# Notes
ctrl+shift+up shell scroll up
PATH=$PATH:~/opt/bin, 或 PATH=~/opt/bin:$PATH
添加到默认搜索路径：Add export PATH=$PATH:/home/me/play to your ~/.bashrc.
bg:
suspend
software
wget

for f in Screen*; do echo $f; mv "$f" "$(echo $f|tr -d '\:\-[[:space:]]')"; done
