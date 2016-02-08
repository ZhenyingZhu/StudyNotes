
## Chapter 0
计算机五个部分：输入单元、 输出单元、CPU内部的控制单元、算数逻辑单元与主存储器  

容量1GB=1024^3，速度1GHz=1000^3  

Intel主板上的芯片组：
- 北桥：负责链接速度较快的CPU、主存储器和显示适配器等组件，为系统总线，速度FSB，总线带宽FSB×总线宽度：每秒可传输最大数据量
- 南桥：负责连接速度较慢的周边接口如硬盘，为I/O总线；  
AMD主板：无南北桥，将内存控制组件整合到CPU中。  
不同主板支持CPU不同。  

CPU种类：精简指令集 (RISC) ，复杂指令集 (CISC) 系统。  
CPU频率：外频：与外部各组件传输数据的速度；外频×倍频=主频。Word size：32/64位，CPU每次能处理的数据量。  

内存：DRAM，断电丢失。外频与CPU的外频相同时最佳。  
DDR：一次工作周期内2次数据传输。  
二级缓存：L2 cache，CPU内部，为SRAM。  

Firmware: 如BIOS，ROM/EEPROM内。  

PCI：适配卡。  

显卡：VGA，内有3D加速芯片GPU。PCI-Express是新的显卡规格，带宽速度大。全彩的每个像素占用3Bytes容量。显存32MB以上。  

IDE/SATA：磁盘，台式机3.5 inch，手提2.5寸。上有缓冲存储器。  
Sector 512B, 组成圆环track, 不同磁盘上同一位置的track组成Cylinder，分割磁盘最小单位。  

网卡：Ethernet规格。  
网络头：RJ-45。8M/1M ADSL传输速度=1Mbyte/s的上传和125Kbytes/s的下载。如Realtek 的RTL8139。  

I/O地址：硬件门牌。  

IRQ：中断。  

电源：最大500W。20/24pin接口。  

- ASCII: 英文编码表，1byte，256种。  
- 中文Big5，2bytes。  
- Unicode：因特网用通用编码。  

OS Kernel：驱动硬件。在内存中受保护，并且常驻。核心功能如下：  
- System call：OS提供，用以开发软件的接口。  
- Process control，Memory management, Filesystem management, Device drivers。  

查阅组件型号  
- `cat /proc/cpuinfo`
- `lspci`  


## Chapter 1
GNU重要软件：
- Emacs
- GCC
- glibc
- Bash shell  

GUI：XFree86的X Window System。  

Assembly Language：汇编语言。  

POSIX：规范核心与应用程序之间的接口。  

Linux 版本号：偶数为稳定版。  
Linux distribution：Kernel + Softwares + Tool  

Apache：网页服务器。  
Postfix/sendmail：电子邮件服务器。  
Samba：文件服务器。  

LSB：Linux开发标准。  
FHS：目录架构标准。  

安装方式：不同Distribution的主要区别，分RPM和dpkg，Tarball原始码。  

Linux可以多人同时在线。  

GUI：X Window，KDE，GNOME。  

查看核心版本：
- `uname -r`: 可查看Distribution版本
- `lsb_release –a` 可查看Linux Standard Base版本。  


## Chapter 2
网络服务器：WWW, Mail Server, File Server。 

Cluster：云计算机平行运算能力。  

Pidgin：实时通讯软件。 

各个Distribution: 
- Ubuntu
- OpenSuSE
- Fedora
- Mandriva

软件列表: 
- Open Office
- Free Maid：组织结构绘制软件。 
- AbiWord  
- Tex/LaTeX
- Dia：类似Visio。 
- Scribus
- GanttProject：时程表绘制。
- GIMP

基础概念：使用者/群组，权限，程序。

文书编辑器vi：会被很多软件调用。

Shell：文字接口软件。有正则表示法，管线命令，数据流重导向。Shell scripts也重要。  

FAQ和How-To：安装软件的帮助文档放在`/usr/share/doc/`下，或http://www.linux.org.tw/CLDP/或http://www.tldp.org/  

提示的网络服务错误信息，可到`/var/log/`里查看。  
How To: http://www.tldp.org  


## Chapter 3
图形接口运算：X Window内的Open GL。  

RAID：多个磁盘接成阵列。  

硬件配置在linux下都是档案。 
- IDE: `/dev/hd[a-d]`
- SCSI, SATA, USB: `/dev/sd[a-p]`
- 软盘驱动器: `/dev/fd[0-1]`
- 打印机: `/dev/lp[0-15]`
- 鼠标: `/dev/usb/mouse[0-15]` 或`/dev/psaux`
- DVD: `/dev/cdrom/`
- 网卡：/dev/eth[0-n]。

磁盘分区：Partition在windows下为C, D, E，Linux下SATA按侦查到的顺序分配sda，sdb。  
磁盘第一个扇区，记录：  
1.	Master Boot Record (MBR) ，安装开机管理程序。  
2.	Partition table。总共记录4个Primary+ Extended，记作P1: `/dev/hda1`。Extended内的logic partition记录在额外的扇区，从`/dev/hda5`开始。  

开机流程：
1. BIOS
2. MBR
3. boot loader
4. 核心档案。  

CMOS记录硬件参数的存储器。  

BIOS开机时执行的第一个程序，找到MBR。然后执行boot loader。  
Boot loader提供多重引导开机选单Grub，载入核心档案，转交其他loader。  
Grub软件：开机启动选单软件。  

文本登录后的程序就是Shell。  

Mirror site：当地的下载较快的分流。  

FTP：客户端如Filezilla，可断点续传。传输再怎么地下化也容易被捉到。  
NAT：IP分享器，内网多用户连接外网时对外的IP分享给内部。  
SAMBA：加入Windows网络邻居。  
邮件服务：Sendmail和Postfix等Mail Server软件。  
WWW服务器：Web功能，许多软件用WWW作为显示接口。Apache软件提供WWW网站功能。  
DHCP：客户端自动获取IP功能。  
Proxy：有效解决带宽不足问题。  
硬盘问题有些可用`fsck`软件解决。  

Directory tree：从root directory `/`开始分支。  
<img src='./LinuxStudy_files/chapter3-02.png' /><br />

- 需`mount`和硬盘档案联系。`/mnt/`内的文件在硬盘上可能在别的地方。  
- 挂载：利用目录作为进入点，将磁盘分区的数据放入该目录。  
- 如`/`挂载到P1，`/home`挂载到P2，则`/home`下的文件都在P2下，而`/etc`就在P1下。  

光盘内容：`/media/cdrom`。 

分区：一个分区内格式化等操作不影响别的分区。考虑所需容量，读写频繁度。  

目录结构: `man hier`
- `/`：根目录。  
- `/Swap`: 内存置换空间，即虚拟内存，无需挂载。理论上为1.5～2倍内存。  
- `/usr`: 软件信息Unix software resource。  
- `/home`: 不同使用者的数据存放。  
- `/var`: 网络相关。  
- `/boot`: 开机读取磁盘大小用，将启动扇区规范在1024个磁柱内，避免磁盘太大读取错误造成的无法开机。100MB就够，须在最前，强制成主要分割区。  

Quota：磁盘配额，分割磁盘后再改动。  

当机原因：除软件问题，可能机箱温度，CPU温度，不同厂商内存，电源供应。  


## Chapter 4
如无法使用DHCP取得IP: 参数设定为 IP: 192.168.1.100, mask：255.255.255.0。  

网卡卡号：Hardware address: 08:00:27:B9:01:BC  

主机名：通常为主机名.网域名，可以有句号，www.vbird.tsai。  

RAID：硬盘特殊应用，软件仿真磁盘阵列。建立两个硬盘分区然后合并，`/dev/md0`，  
LVM：弹性调整系统容量。  
Ext3：文件系统类型，有日志记录。系统默认。  
传统的文件格式为：ext2。Journaling有ext3及Reiserfs等。  
Vfat：linux，windows共用文件系统。  

Grub装载在MBR里。可以设置开机密码，但这样就无法远程登录了。  

系统安装过程写入`/root/install.log`，选择的软件写入`/root/anaconda-ks.cfg`。  

安装包分为多个档案，按需安装。  

memtest86：内存压力测试。  

如因硬件配置无法安装： DVD开机时，`boot: linux nofb apm=off acpi=off pci=noacpi`  
- apm, acpi：电源管理模块；
- nofb：取消显卡缓存侦测；   

SELinux：Access control设定，不是防火墙，推荐安装。  
Kdump：核心出错是将内存写入档案。较消耗硬盘空间。  

Windows双系统时，Linux所在分区在windows下不要挂载，以免被格式化。  

## Chapter 5
Linux使用异步的磁盘/数据传输模式，不能非正常关机。<br />
GNOME和KDE：Window Manager，图形接口。<br />
在线升级：yum机制。<br />
文件名开头为小数点的，即为隐藏文件。<br />
SCIM：中文输入法软件，Ctrl+Space唤出。<br />
<code>Alt+Ctrl+Backspace</code>：重启X Window。<br />
<code>Ctrl+Alt+F1~6</code>：tty1~6的文字接口，run level 3；Ctrl+Alt+F7~8切回图形接口，run level 5。<br />
	执行等级：run level 0：关机，run level 6：重启。用<code>init 0</code>切换模式。<br />
	修改默认登录方式：修订/etc/inittab文件内容。？<br />
<code>Tab</code>：自动补全。<br />
<code>Ctrl+C</code>：当前程序中断。<br />
<code>Ctrl+D</code>：键盘输入结束。<br />
终端界面下：startx启动图形界面。需tty7空，X server能启动，已启动如X Font Server和xfs等服务，并有Window Manager。<br />
Terminal提示格式：[User@Localhost ~]$。<br />
Localhost：主机名，取小数点前的名称。<br />
@之前的为登录的用户名。<br />
<code>~</code>：用户的主目录（工作目录），即/home/用户名，是个变量。对于root用户，~user是user用户的家目录。<br />
$：一般用户的提示字符，#是root用户的提示字符。<br />
Shell：文字接口程序，是bash。<br />
指令格式：command [-opt] par1 par2，--后是全称。多个空格视为1个，区分大小写。<br />
	\接特殊字符换行。<br />
	提示指令未发现，可能是bash没将该指令添加入搜索path。<br />
	指令选项前常有-或+，选项全称带--，如—help。选项间可加可不加-。<br />
列出文件夹列表：<code>ls –al /home/</code> ，-a显示隐藏文件，-l以列表形式显示。-d显示目录。位置为/bin/ls。<br />
登录的login也是一个程序。<br />
查看当前有谁在线：<code>who</code>。<br />
查看网络联机状态：<code>netstat –a</code>。Sockets and ports. <br />
查看背景执行的程序：<code>ps –aux</code>。<br />
由以上返回信息判断是否可关机。<br />
数据同步写入硬盘：<code>sync</code>，将内存中的数据写入硬盘，默认情况下不会写入，关机前执行。<br />
注销：<code>exit</code>。<br />
关机：<code>shutdown</code>，只有root可以，用远程登录如pietty用ssh登录须进入root权限。<br />
	/sbin/shutdown –t 秒 –a 时间 [讯息]<br />
	-t sec ： -t 后面加秒数，过几秒后关机。<br />
-k：不要真的关机，只是发送警告讯息出去！ <br />
-r：在将系统的服务停掉后就重启 (常用) 。<br />
-h：将系统的朋务停掉后，关机。(常用) 。<br />
-n：不经过 init 程序，直接以shutdown 的功能来关机。<br />
-f：关机并开机之后，强制略过fsck 的磁盘检查。<br />
-F：系统重启后，强制fsck 的磁盘检查。<br />
-c： 取消进行的 shutdown。<br />
时间：now 立即，21：00 定时，+10 10分钟后。<br />
重启关机：<code>reboot</code>, <code>halt</code>硬件强行关机, <code>poweroff</code>。<br />
显示时间日期：<code>date</code> +%H:%M%Y%m%d; %H"char"%M shows hour and miniute. <br />
显示日历：<code>cal</code> 10 2009<br />
显示系统语言：<code>echo $LANG</code><br />
修改系统语言：<code>LANG=en_US</code>，LANG=zh_TW.UTF-8, LANG="en"。默认语系选择存于/etc/sysconfig/i18n。<br />
计算器：<code>bc</code>，%求余。scale=小数点后位数。quit退出。<br />
查看指令手册：<code>man</code> 指令。说明文件在/usr/share/man/内，可通过修改/etc/man.config或manpath.conf改变搜索路径。<br />
<code>man –f man</code>搜索多个数值指令 show all the numbers of a command。<code>man –k man</code>查找关键字。<code>man –K man</code>查询整个系统。有多个数值的指令可通过<code>man 7 man</code>查看。<br />
<code>whatis</code>相当于man –f，但是需用root身份<code>makewhatis</code>建立数据库。<br />
指令手册名称边括号里的数值：<br />
 1可执行指令，5配置，8系统管理指令。<br />
 <img src="./LinuxStudy_files/chapter5-01.png" /> <br /><br />
 man手册查看快捷键：<br />
空格键翻页，q退出。/string搜索。<br />
<img src="./LinuxStudy_files/chapter5-02.png" /> <br /><br />
在线查看指令帮助：<code>info</code> 指令，输出的为含链接的段落。/usr/share/info/。<br />
	N, P, U去下、上、上一层node。<br />
<img src="./LinuxStudy_files/chapter5-03.png" /><br />
<img src="./LinuxStudy_files/chapter5-04.png" /> <br /><br />
/usr/share/doc/介绍packages，如/usr/share/doc/bash-3.2/介绍bash。<br />
简单文本编辑器<code>nano</code> text.txt：^代表ctrl，M代表alt。^O存档，^X退出。<br />
[ctrl]：取得联机help。<br />
[ctrl]-X：离开naon软件，若有修改过档案会提示是否需要储存喔！ <br />
[ctrl]-O：储存档案，若你有权限的话就能够储存档案了； <br />
[ctrl]-R：从其他档案读入资料，可以将某个档案的内容贴在本档案中； <br />
[ctrl]-W：搜寻字符串。<br />
[ctrl]-C：说明目前光标所在处的行数与列数等信息； <br />
[ctrl]-_：可以直接输入行号，让光标忚速移动到该行； <br />
[alt]-Y：校正语法功能开启关闭 (单击开、再单击关) 。<br />
[alt]-M：可以支持鼠标来移动光标的功能。<br />
文件系统错误：如根目录未损毁，登入root，<code>fsck /dev/sda7</code> 修复错误的partition。<br />
	根目录损毁，不mount该硬盘，执行<code>fsck /dev/sdb1</code>。<br />
主机通电后尽量不动，降低温度。<br />
忘记root密码：重启是按e进入grub编辑模式，在kernel行按e，输入single，回车后按b启动。<br />
	修改密码：<code>passwd</code>。（鸟哥私房菜P171）<br />
欢迎画面：/etc/issue。<br />
"\"用escape表示。<br />


<h3>Chapter 6</h3>
档案读写权限：Owner, group, others. Read, write, execute. <br />
一个账号可以属于多个群组。<br />
所有账号信息存于/etc/passwd，密码存于/etc/shadow 所有组名存于/etc/group。<br />
档案属性：在ls –al列表中显示，如-rw-r--r-- 1 root root 42304 Sep 4 18:26 install.log这七个字段：<br />
	档案类型与权限：共十个字符：<br />
1：d是目录，-是档案，l是link file，b是可随机存储装置，c是串行端口如鼠标键盘（一次性读取），s是资料接口，p是数据传输文件。<br />
		2-4：r读w写x执行。如无该权限该处为-。这里是owner的权限。<br />
		5-7：group内成员的权限。<br />
		8-9：others 的权限。对于目录，如果权限是r而不是x的话，则无法进入。<br />
	连结数：有多少档案名连结到该inode（一个号码），即Hard link数。文件系统用inode记录档案的权限与属性。<br />
	档案拥有者。<br />
	所属群组。<br />
	档案容量：单位byte。<br />
	最后被修改的时间：ls –l –full-time显示完整的包括年的时间格式。<br />
	档名。<br />
改变档案所属群组：chgrp [–R] group dirname/filename filename…。加入R可令次目录下文件亦改变群组。<br />
改变档案拥有者：chown [–R] user dirname/filename filename…。chown user:group filename。<br />
改变档案的权限：chmod。还可用于改变SUID，SGID，SBIT。chmod a+/-x file。ugoa: user, group, owner, all<br />
	数字模式：r4w2x1，一组三个相加。chmod [-R] 750 file使file权限变为drwxr-x---。 <br />
	编写shell以后，将664改为chmod 755 *.sh才可运行。<br />
	特殊权限设置：四位数，最前加4：SUID；2：SGID；1：SBIT，可相加。如chmod 4755 filename产生-rwsr-xr-x。chmod 6755 test产生-rwsr-sr-。(鸟哥的私房菜P222)<br />
	空权限：当档案不可执行却又特殊权限时，该权限为空。如chmod 7666 test产生-rwSrwSrwT。<br />

档案：可为文本文件（ASCII），数据库文件data file，binary program（须有x权限）等，与扩展名无关。rwx权限都是对档案内容而言，w不包括删除档案的权限。删除档案的权限在目录里。<br />
目录：记录文件名列表。r可否ls（可读但不可执行则显示?），w可否新建删除重命名移动，x可否进入（使该目录成为当前的work directory）。<br />
	建站时开放目录，r-x。w不能给，避免删除该目录下别人的档案。<br />
所有人都拥有权限的目录：/tmp。<br />
复制档案：cp soucename destname。<br />
新建空档案：touch directory/documentname。<br />
新建floopy：mkdir floopyname。<br />
切换账号：su vbird。<br />
进入目录：cd /root: into root。cd ..: return to father。cd – 回到上一个工作目录。<br />
用纯文本方式读取档案：cat ~/.bashsc可看到/home/用户名/下隐藏的bashsc文件内容。<br />
读取data file：特定格式，用last (鸟哥的私房菜 P401)来查看过去登陆日志，即 /var/log/wtmp的内容，该档案记录登录的数据。<br />
装置文件device：在/dev/下。分为Block（存储，如/dev/sda）和Character（串口，一次性读取，不能截断输出）。<br />
资料接口Sockets:在/var/run/下，客户端通过它进行数据沟通。<br />
数据传输pipe:FIFO，处理多个程序同时读一个文件时的操作。<br />
扩展名：sh:脚本或批处理(Script)；Z,tar,tar.gz,zip,tgz：压缩文件；html,php：网页<br />
网上下载的档案属性会改变。<br />
文件，档案名长度：255字符，包含路径4096字符。避免特殊字符* ? > < ; & ! [ ] | \ ' " ` ( ) { }。<br />
FHS：Linux目录配置标准。(鸟哥的私房菜 P198)<br />
	可分享的(shareable)	不可分享的(unshareable)<br />
不变的(static)	/usr (软件放置处)	/etc (配置文件)<br />
	/opt (第三方协力软件)	/boot (开机与核心档)<br />
可变动的(variable)	/var/mail (使用者邮件信箱)	/var/run (程序相关)<br />
	/var/spool/news (新闻组)	/var/lock (程序相关)<br />
	可分享：可给其他系统挂载；不可分享：仅与自己机器有关。<br />
	不变：函式库，文件说明，主机服务配置等；可变动：经常写入的文件。<br />
三层目录：/：root, 与开机系统有关，分割槽越小越好，不要在此处安装软件；/usr：软件安装执行有关；/var：系统运作有关。<br />
/bin: 放置的是可以被root与一般账号所使用指令，主要有：cat,chmod,chown,date,mv,mkdir,cp,bash等。<br />
/boot：开机会使用到的档案。Linux kernel常用的档名为vmlinuz，则还会存在/boot/grub/。<br />
/dev：任何装置与接口设备都是以档案的型态存在于这个目录下。有/dev/null,/dev/zero, /dev/tty,/dev/lp*,/dev/hd*,/dev/sd*等。<br />
/etc：系统主要的配置文件，如人员的账号密码文件、 各种服务的初始档等。文件可以让一般使用者查阅的，但是只有root有权力修改。FHS 建议不要放置binary。<br />
/etc/inittab,/etc/modprobe.conf,/etc/fstab,/etc/sysconfig/<br />
/etc/init.d/：所有服务的预设启动script。如要启动或关闭iptables：/etc/init.d/iptables start、/etc/init.d/iptables stop<br />
/etc/xinetd.d/：super daemon管理的服务的配置文件目录。<br />
/etc/X11/：X Window 有关的各种配置文件都在这里，如xorg.conf。<br />
/home：home directory。变量～代表目前用户，～dmtcai代表dmtsai的home directory。<br />
/lib：开机时会用到的函式库，和在/bin，/sbin底下的指令会呼叫的函式库。/lib/modules/放置核心相关的模块 (驱动程序) 。<br />
/media：放置可移除的装置。有：/media/floppy, /media/cdrom等。<br />
/mnt：暂时挂载的额外装置放置到这里。<br />
/opt: 第三方协力软件放置的目录。自行安装的软件也安装到这里，或放置在/usr/local/下。<br />
/root：管理员的家目录与根目录放置在同一个分割槽中。<br />
/sbin：设定系统环境的指令只有root才能使用。/sbin/底下的指令为开机过程中所需要的，包括开机、修复、还原系统所需要的指令。服务软件程序，放置到/usr/sbin/当中。自行安装的软件所产生的system binary，放置到/usr/local/sbin/当中。包括：fdisk, fsck, ifconfig, init, mkfs等。<br />
/srv：一些网络服务启动后所需取用的数据目录。如WWW, FTP等。如WWW朋务器需要的网页资料就放置在/srv/www/里面。<br />
/tmp：一般用户或正在执行的程序暂时放置档案的地方。任何人都能够存取的，需要定期的清理一下。FHS建议在开机时，要将/tmp/下的数据都删除。<br />
FHS未定义但常见的目录：<br />
/lost+found：标准的ext2/ext3文件系统格式产生的，通常会在分割槽的最顶层，即/disk/lost+found/。<br />
/proc：virtual filesystem。数据在内存中，如系统核心、行程信息(process)、周边装置的状态及网络状态等。不占任何硬盘空间。如：/proc/cpuinfo,/proc/dma,/proc/interrupts,/proc/ioports,/proc/net/*等。<br />
/sys：也是一个虚拟的文件系统，记彔与核心相关的信息。包括目前已加载的核心模块与核心侦测到的硬件装置信息等。不占硬盘空间。<br />
函数库：除了在/lib/下还有许多。指令调用的函数。<br />
开机过程中仅有根目录会被挂载，其他分割槽在开机完成后挂载。与开机过程有关的目录，不可与根目录分开：<br />
/etc：配置文件；/bin：重要执行档；/dev：所需要的装置档案；/lib：执行档所需的函式库与核心所需的模块；/sbin：重要的系统执行文件。<br />
/usr：软件开发时合理地将数据分别放置到该目录的子目录中。<br />
	/usr/X11R6/：放置X Window System重要数据最后的X版本为第11版，第6次释出。<br />
/usr/bin/：大部分用户可使用的与开机无关的指令。<br />
/usr/include/：放置c/c++等程序的header和include，用在以tarball方式 (*.tar.gz 的方式安装软件) 安装某些数据时。<br />
/usr/lib/：各应用软件的函式库、object file，以及不被一般使用者用的执行档或script。如64位系统有/usr/lib64/，和一些特殊的，经常被系统管理员操作的，进行服务器的设定指令。<br />
/usr/local/：系统管理员在本机自行安装的软件。如distribution提供的软件较旧，想安装较新的软件但又不移除旧版，该目录下也有bin, etc, include, lib...的子目录。<br />
/usr/sbin：非系统正常运作所需要的系统指令。如某些网络服务器软件的服务指令（daemon）。<br />
/usr/share/：放置共享文件。数据几乎是不分硬件架构均可读取的数据，常见子目录：<br />
	/usr/share/man：联机帮助文件；/usr/share/doc：软件杂项的文件说明；/usr/share/zoneinfo：时区档案。<br />
/usr/src/：一般原始码放置到这里。核心原始码放置到/usr/src/linux/下。<br />
/var：系统运行产生的文件，如cache、log file和软件运行时所产生的档案，如lock file, run file，MySQL数据库的档案等。<br />
	/var/cache/：应用程序本身运作过程中会产生的一些暂存档。<br />
/var/lib/：放置程序执行过程中需使用的数据文件。各软件有各自的目录。如MySQL的数据库放置到/var/lib/mysql/，rpm的数据库则放到/var/lib/rpm/。<br />
/var/lock/：装置或档案资源一次只能被一个应用程序所使用，将该装置lock。<br />
/var/log/：放置登彔文件。如/var/log/messages, /var/log/wtmp(记彔登入者的信息)等。<br />
/var/mail/：放置个人电子邮件信箱，与/var/spool/mail/通常是链接文件。<br />
/var/run/：某些程序或服务运行后，会将它们的PID放置在这个目录。<br />
/var/spool/：放置一些队列数据（排队等待其他程序使用的数据）。这些数据被使用后通常都会被删除。如收到新邮件会放置到/var/spool/mail/中，使用者收下该信件后就会被删除。信件暂时寄不出去会被放到/var/spool/mqueue/中，工作排程数据(crontab)，会被放置到/var/spool/cron/。<br />
Directory tree：根为/；每个目录可使用本地partition的文件系统和网络上的 file system，如Network File System (NFS) 服务器挂载的目录等；每个档案在此树中的完整路径都是独一无二的。<br />
 
/selinux/的内容在内存中。<br />
绝对路径：由/开始写起的文件名或目彔名称。<br />
相对路径：相对于目前路径的文件名写法。如../../home/dmtsai/等。 . ：代表当前的目录，也可以使用 ./ 来表示；.. ：代表上一层目录，也可以 ../ 来代表。<br />
正规的执行目录：/bin/，/usr/bin/下的指令可直接执行。<br />

Chapter 7
写Shell Scripts时需用绝对路径。<br />
特殊目录：.，..，-前一个目录，～，～account。<br />
根目录的.和..属性权限完全相同，是同一个目录。<br />
显示当前目录：pwd [-P]。P显示正确的完整路径（对连接目录有效，如/var/mail/）。<br />
建立新目录：mkdir [-mp]。mkdir -m 711直接设置权限而不遵守预设umask。mkdir –p test1/test2递归建立目录，即多层结构的目录。<br />
删除空目录：rmdir [-p]。P连同上层的空目录一起删除。<br />
删除目录下所有东西：rm –r test。<br />
环境变量$PATH：自动去这些目录里搜索指令。同名的指令，执行先搜到的。/usr/local/bin:/bin:/usr/bin:/home/vbird/bin，中间用:隔开。<br />
	不在$PATH下的指令，能用绝对路径访问，如/sbin/ifconfig eth0查看IP。<br />
	安全起见，不在搜索路径中加入.，以免有人在/tmp/中覆写常用指令。<br />
显示变量值：echo $PATH。变量名区分大小写。<br />
移动文件：mv /bin/ls /root。如果mv filename1 filename2则是重命名。<br />
显示文件信息：ls [-aAdfFhilnrRSt] 目录名称；ls [--color={never,auto,always}] 目录名称；ls [--full-time] 目录名称；ls –l 档案名1 档案名2。<br />
	-a：全部的档案，连同隐藏档一起列出来。<br />
-A：同上，但不包括.和..。<br />
-d：查看目录的属性。<br />
-f：直接列出结果，而不进行排序 (预设会以档名排序) 。<br />
-F：显示附加数据结构，如*代表可执行文件；/代表目录；=代表socket档案；|代表FIFO档案。 <br />
-h：将档案容量以GB, KB等单位显示。 <br />
-i：列出 inode 号码。<br />
-l：列出个档案属性。<br />
-n：列出UID与GID而不是使用者和群组名称。<br />
-r：将排序结果反向输出。<br />
-R：连同子目录内容一起列出。<br />
-S：以档案容量大小排序。 <br />
-t：依时间排序。<br />
--color：never：不要依据档案特性给予颜色显示；always：显示颜色；auto：系统自行判断是否给予颜色。蓝色为档案，白色为目录。<br />
--full-time：以完整时间模式 (年、月、日、时、分) 输出。<br />
--time={atime,ctime}：输出 access 时间或改变权限属性时间(ctime)而非内容变更时间(modification time)。<br />
档案的各种属性都存放于i-node中。<br />
Bash Shell的alias：设定特定的字符串某代表包含参数的指令。<br />
复制档案：只能复制有read权限的。cp [-adfilprsu] source destination。cp [options] source1 source2 source3 .... directory。如果source有两个以上，则destination一定要是目录。<br />
	-a：相当于-pdr。<br />
-d：若source为link file，则复制链接文件属性而非档案本身。<br />
-f：force，若目标档案已经存在且无法开启，则移除后再尝试一次。<br />
-i：若destination已经存在时，在覆盖时会先询问。<br />
-l：进行hard link的连结档建立，而非复制档案本身。<br />
-p：连同档案的属性一起复制，而非使用默认属性（备份用）。默认属性是644 复制者 复制者。<br />
-r：递归持续复制，用亍目录的复制，不然无法复制。cp –r /etc/ /tmp，但属性会改变。cp –a /etc /tmp可保证属性不变。<br />
-s：复制为symbolic link。<br />
-u：若destination比source旧才更新。<br />
移除档案：rm [-fir] 档案或目录。档案名中*代表0到无穷多个字符。<br />
-f：忽略不存在的档案。<br />
-i：在删除前会询问使用者。默认情况rm指令alias为rm –i。<br />
-r：递归删除，用于目录的删除，危险。<br />
指令前加\可忽略alias的指定选项。<br />
移动档案：mv [-fiu] source destination。mv [options] source1 source2 source3 .... directory。<br />
-f：如果目标档案已经存在，不会询问而直接覆盖。<br />
-i：会询问是否覆盖。<br />
-u：只有source比destination新时才会update。<br />
重命名：rename。<br />
获得档名：basename /etc/sysconfig/network。<br />
获得目录名：dirname /etc/sysconfig/network。<br />
由第一行开始显示档案内容：cat [-AbEnTv] 档案名。<br />
-A：等于-vET。<br />
-b：列出非空白行行号。<br />
-E：显示断行字符$。Windows的文档断行符为^M$。<br />
-n：打印所有行行号。<br />
-T：将tab键以^I显示。<br />
-v：列出特殊字符。<br />
从最后一行开始显示：tac。用法同上。<br />
显示时输出行号：nl [-bnw] 档案。<br />
-b：指定行号出现方式，-b a：空行也显示；-b t：不显示，默认值。<br />
-n：行号显示方式，-n ln：最左方；-n rn：最右方，不补0；-n rz：最右方补0。<br />
-w：行号字段占用的位数。<br />
分页显示：more 档案名。底下显示百分比。可执行操作：<br />
space：向下翻页。<br />
Enter：向下翻一行。<br />
/字符串：向下搜寻关键词。<br />
:f：显示文件名及目前的行数。<br />
q：离开。<br />
b或[ctrl]-b：往回翻页，只对档案有用，对管线无效。<br />
可往前翻页显示：less。同上，操作：<br />
空格键或[pagedown]：向下翻页。<br />
[pageup]：向上页。<br />
/字符串：向下搜寻。<br />
?字符串：向上搜寻。<br />
n：回到前一个搜寻。<br />
N：反向的前一个搜寻。<br />
q：离开。<br />
只看头几行：head [-n number] 档案1 档案2。<br />
	-n：后面接数字显示几行。不加数字默认显示10行。接负数显示总行数减后的行数。如-100显示41行（总长141行）。<br />
只看末几行：tail [-n number] 档案。<br />
-n：从第几行开始显示，前面的number-1行不显示。<br />
-f：表示持续侦测该档案，一有变动就显示，直到按下[ctrl]-c为止。<br />
读取二进制档案：od [-t TYPE] [oC TYPE] 档案。<br />
-t：接输出TYPE：a ：默认；c：用ASCII；d[size]：利用十进制(decimal)，每个整数占用size bytes；f[size]：利用floating；o[size]：用八进制(octal)；x[size]：用十六进制(hexadecimal)。<br />
	oC：显示两种TYPE并比较。如od -t oCc /etc/issue。<br />
档案的时间参数：<br />
modification time (mtime)：内容数据变更的时间。ls默认显示的时间。<br />
status time (ctime)：权限或属性被更改的时间。<br />
access time (atime)：内容被取用的时间。<br />
修改档案时间（可建立空档案）：touch [-acdmt] 档案。<br />
-a：修改access time为当前时间。<br />
-c：修改档案的时间，不存在则不建立新档案。<br />
-d：后面接欲修订日期，等于--date="日期或时间"。如touch -d "2 days ago" bashrc。<br />
-m：仅修改mtime。<br />
-t：后面接欲修订的时间，格式为[YYMMDDhhmm]。如touch -t 0709150202 bashrc。<br />
输入指令时;可连续执行。<br />
Ext2/Ext3文件的隐藏属性：如可以设定档案不可修改，连拥有者都不能修改。用以确保security。其他系统没有改属性。<br />
档案预设权限：umask [-S]查看。umask 三位数设定。<br />
	S：用符号显示。<br />
共四位数，第一位为特殊权限。<br />
新建档案，权限默认为666，目录缺省为777。<br />
数值显示的是剪掉的权限。如，umask显示0022则表明现在默认的权限为建立档案时644，建立目录时755。<br />
设定时为缺省值需要剪掉的数。如umask 002令档案默认为664,目录为775，<br />
root的umask为022，一般users为002，预设在/etc/bashrc，不建议修改。<br />

配置隐藏属性：chattr [+-=][ASacdistu] 档案或目录。<br />
+：增加参数。<br />
-：移除参数。<br />
=：重新设定全部参数。<br />
A：atime不会被修改。<br />
S：将该档案异步存取模式改为同步写入磁盘。<br />
a：档案只能增加数据，只有root才能设定。<br />
c：自动压缩，读取时自动解压缩。<br />
d：用dump程序备份时，该档案或目录不会被备份。<br />
i：档案不能被删除、改名、设定连结、写入或新增内容。只有root能设定。<br />
s：档案被删除时完全地移除。<br />
u：档案被删除时内容还存在磁盘中。<br />
查看隐藏属性：lsattr [-adR] 档案或目录。<br />
-a：显示隐藏文件。<br />
-d：显示目录。<br />
-R：递归显示子目录。<br />

特殊权限：出现在档案拥有者的x上， 在属性上显示Set UID/GID的s在User /group的位置，和Sticky Bit的t。<br />
Set UID仅对binary program有效；执行者对该程序需有x权限；本权限仅在run-time中有效；执行者具有owner的权限。即在执行过程中具有owner的权限，可修改一些本来无法修改的文件。<br />
	如修改密码指令/usr/bin/passwd。Owner为root，权限为-rwsr-xr-x 1 root root，用以修改/etc/shadow。所以Others可以用该指令改密码。<br />
Set GID：与SUID类似，可用于目录，获得群组权限。<br />
如权限为-rwx--s--x 1 root slocate 23856 Mar 15 2007 /usr/bin/locate可搜寻/var/lib/mlocate/mlocate.db。<br />
	目录有SGID属性：用户在此目录下的effective group将会变成该目录的群组；若用户在此目录下具有w的权限，则使用者所建立的新档案的群组与此目录的群组相同。<br />
Sticky Bit：用户对目录有w,x权限；用户在该目录下建立的档案或目录仅有自己与root可删除。<br />
	如/tmp的权限是drwxrwxrwt。<br />
观察文件类型：file。属于ASCII或data，有否用到share library。<br />
寻找指令位置：which [-a] command。<br />
	-a ：列出所有PATH目录中可找到的指令而非第一个。<br />
寻找档案：whereis [-bmsu] 档案或目录。在系统维护的数据库中找，速度快。<br />
-b:只找binary格式的档案。<br />
-m:只找在说明文件manual路径下的档案。<br />
-s:只找source来源档案。<br />
-u:搜寻不在上述三个项目中的其他特殊档案。<br />
关键字查找档案名，即档案名查找包含该词的档案：locate [-ir] keyword。数据库/var/lib/mlocate/一天只更新一次，新建的找不到。<br />
-i：不区分大小写。<br />
-r：启用正则表达式。<br />
硬盘中查找档案：find [PATH] [option] [action]。支持通配符如find /etc -name '*httpd*'。速度慢。* 对应任意个字符，?对应任意一个字符。<br />
与时间有关的选项：<br />
-atime,-ctime，-mtime。-mtime 数字n：在n天前的当天被更改过内容的档案；+n：列出n天前不含第n天的档案；-n：列出n天内含n天的档案档名。<br />
-newer file：列出比file新的档案。find /etc -newer /etc/passwd。<br />
使用者或组名有关：<br />
-uid n：n是用户的账号UID，记录在/etc/passwd里面。<br />
更新数据库/var/lib/mlocate/：updatedb。<br />
-gid n：组名的GID。记录在/etc/group。<br />
-user name：使用者的账号名称。<br />
-group name：组名。<br />
-nouser：寻找不存在于/etc/passwd的档案拥有者。<br />
-nogroup：寻找不存在于/etc/group的拥有群组的档案。发生于安装软件时。<br />
与档案权限及名称有关：<br />
-name filename：搜寻文件名为 filename的档案。<br />
-size [+-]SIZE：搜寻比SIZE大(+)或小(-)的档案。如-size +50k。单位c代表byte，k代表1024bytes。<br />
-type TYPE：搜寻档案的类型为：一般档案f,装置档案b、c,目录d,连结档l,socket s,及FIFO p。<br />
-perm mode：搜寻档案权限等于mode的档案，如-rwsr-xr-x=4755。<br />
-perm -mode：权限囊括mode的档案。？<br />
-perm +mode：搜寻档案权限包含任一mode权限的档案，如-perm +755可找到-rw-------。？<br />
额外动作：<br />
-print：将结果打印到屏幕上。<br />
-exec command：用额外的指令（不支持alias）处理搜寻到的结果。如find / -perm +7000 -exec ls -l '{}' \;。{}代表find找到的内容。-exec到\;之间是额外动作。因为;在bash下有特殊意义，因此用反斜杠跳脱。等同于ls -l 第一个找到的;ls -l 第二个找到的;<br />
逻辑表达式：<br />
-a：and。如find /etc -size +50k -a -size -60k -exec ls -l '{}' \；<br />
!：反向选择。如find /etc -size +50k -a ! -user root -exec ls -ld '{}' \;找到大于50k且不是root的档案。<br />
-o:or。<br />

groupadd group添加组，useradd user添加用户。id user查询用户权限情况。<br />
添加一个组公用文件夹，需要设置SGID，不然某用户默认建立的档案别人无发访问。（鸟哥的私房菜P239）<br />

