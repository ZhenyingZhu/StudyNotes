
<h3>Chapter 0</h3>
计算机五个部分：输入单元、 输出单元、CPU内部的控制单元、算数逻辑单元与主存储器<br />
容量1GB=1024^3，速度1GHz=1000^3<br />
Intel主板上的芯片组：北桥：负责链接速度较快的CPU、主存储器和显示适配器等组件，为系统总线，速度FSB，总线带宽FSB×总线宽度：每秒可传输最大数据量；南桥：负责连接速度较慢的周边接口如硬盘，为I/O总线；<br />
AMD主板：无南北桥，将内存控制组件整合到CPU中。<br />
不同主板支持CPU不同。<br />
CPU种类：精简指令集 (RISC) ，复杂指令集 (CISC) 系统。<br />
CPU频率：外频：与外部各组件传输数据的速度；外频×倍频=主频。Word size：32/64位，CPU每次能处理的数据量。<br />
内存：DRAM，断电丢失。外频与CPU的外频相同时最佳。<br />
DDR：一次工作周期内2次数据传输。<br />
二级缓存：L2 cache，CPU内部，为SRAM。<br />
Firmware: 如BIOS，ROM/EEPROM内。<br />
PCI：适配卡。<br />
显卡：VGA，内有3D加速芯片GPU。PCI-Express是新的显卡规格，带宽速度大。全彩的每个像素占用3Bytes容量。显存32MB以上。<br />
IDE/SATA：磁盘，台式机3.5 inch，手提2.5寸。上有缓冲存储器。Sector 512B, 组成圆环track, 不同磁盘上同一位置的track组成Cylinder，分割磁盘最小单位。<br />
网卡：Ethernet规格。网络头：RJ-45。8M/1M ADSL传输速度=1Mbyte/s的上传和125Kbytes/s的下载。如Realtek 的RTL8139。<br />
I/O地址：硬件门牌。<br />
IRQ：中断。<br />
电源：最大500W。20/24pin接口。<br />
ASCII: 英文编码表，1byte，256种。中文Big5，2bytes。Unicode：因特网用通用编码。<br />
OS Kernel：驱动硬件。在内存中受保护，并且常驻。核心功能如下：<br />
System call：OS提供，用以开发软件的接口。<br />
Process control，Memory management, Filesystem management, Device drivers。<br />
查阅组件型号：查看/cat/proc/cpuinfo,或<code>lspci</code> <br />


<h3>Chapter 1</h3>
GNU重要软件：Emacs, GCC, glibc, Bash shell。<br />
GUI：XFree86的X Window System。<br />
Assembly Language：汇编语言。<br />
POSIX：规范核心与应用程序之间的接口。<br />
版本号：偶数为稳定版。<br />
Linux distribution：Kernel + Softwares + Tool<br />
Apache：网页服务器。<br />
Postfix/sendmail：电子邮件服务器。<br />
Samba：文件服务器。<br />
LSB：Linux开发标准。<br />
FHS：目录架构标准。<br />
安装方式：不同Distribution的主要区别，分RPM和dpkg，Tarball原始码。<br />
Linux可以多人同时在线。<br />
GUI：X Window，KDE，GNOME。<br />
查看核心版本：<code>uname -r</code>可查看Distribution版本，<code>lsb_release –a</code>可查看Linux Standard Base版本。<br />


<h3>Chapter 2</h3>
网络服务器：WWW, Mail Server, File Server。<br />
Cluster：云计算机平行运算能力。<br />
Pidgin：实时通讯软件。<br />
各个Distribution。（鸟哥私房菜P68）<br />
软件列表（鸟哥私房菜P68）<br />
Free Maid：组织结构绘制软件。<br />
Dia：类似Visio。<br />
GanttProject：时程表绘制。<br />
基础概念：使用者/群组，权限，程序。<br />
文书编辑器vi：会被很多软件调用。<br />
Shell：文字接口软件。有正则表示法，管线命令，数据流重导向。Shell scripts也重要。<br />
FAQ和How-To：安装软件的帮助文档放在/usr/share/doc/下，或http://www.linux.org.tw/CLDP/或http://www.tldp.org/ <br />
提示的网络服务错误信息，可到/var/log/里查看。<br />
How To: http://www.tldp.org<br />


<h3>Chapter 3</h3>
图形接口运算：X Window内的Open GL。<br />
RAID：多个磁盘接成阵列。<br />
查看硬件支持：（鸟哥私房菜P80）。<br />
硬件配置在linux下都是档案。<br />
<img src='./LinuxStudy_files/chapter3-01.png' /><br />
网卡：/dev/eth[0-n]。<br />
磁盘分区：Partition在windows下为C, D, E，Linux下SATA按侦查到的顺序分配sda，sdb。<br />
磁盘第一个扇区，记录：<br />
1.	Master Boot Record (MBR) ，安装开机管理程序。<br />
2.	Partition table。总共记录4个Primary+ Extended，记作P1: /dev/hda1。Extended内的logic partition记录在额外的扇区，从/dev/hda5开始。<br />
开机流程：BIOS->MBR->boot loader->核心档案。<br />
CMOS记录硬件参数的存储器。<br />
BIOS开机时执行的第一个程序，找到MBR。然后执行boot loader。<br />
Boot loader提供多重引导开机选单Grub，载入核心档案，转交其他loader。<br />
Grub软件：开机启动选单软件。<br />
文本登录后的程序就是Shell。<br />
Mirror site：当地的下载较快的分流。<br />
FTP：客户端如Filezilla，可断点续传。传输再怎么地下化也容易被捉到。<br />
NAT：IP分享器，内网多用户连接外网时对外的IP分享给内部。<br />
SAMBA：加入Windows网络邻居。<br />
邮件服务：Sendmail和Postfix等Mail Server软件。<br />
WWW服务器：Web功能，许多软件用WWW作为显示接口。Apache软件提供WWW网站功能。<br />
DHCP：客户端自动获取IP功能。<br />
Proxy：有效解决带宽不足问题。<br />
硬盘问题有些可用fsck软件解决。<br />
Directory tree：从root directory /开始分支。<br />
<img src='./LinuxStudy_files/chapter3-02.png' /><br />


需mount和硬盘档案联系。mnt/内的文件在硬盘上可能在别的地方。<br />
挂载：利用目录作为进入点，将磁盘分区的数据放入该目录。<br />
如/挂载到P1，/home挂载到P2，则/home下的文件都在P2下，而/etc就在P1下。<br />
光盘内容：/media/cdrom。<br />
分区：一个分区内格式化等操作不影响别的分区。考虑所需容量，读写频繁度。<br />
/：根目录。<br />
/Swap：内存置换空间，即虚拟内存，无需挂载。理论上为1.5～2倍内存。<br />
/usr: 软件信息Unix software resource。<br />
/home：不同使用者的数据存放。<br />
/var：网络相关。<br />
/boot：开机读取磁盘大小用，将启动扇区规范在1024个磁柱内，避免磁盘太大读取错误造成的无法开机。100MB就够，须在最前，强制成主要分割区。<br />
Quota：磁盘配额，分割磁盘后再改动。<br />
当机原因：除软件问题，可能机箱温度，CPU温度，不同厂商内存，电源供应。<br />


<h3>Chapter 4</h3>
无法使用DHCP取得IP，参数设定为： IP: 192.168.1.100, mask：255.255.255.0。<br />
网卡卡号：Hardware address: 08:00:27:B9:01:BC<br />
主机名：通常为主机名.网域名，可以有句号，www.vbird.tsai。<br />
RAID：硬盘特殊应用，软件仿真磁盘阵列。建立两个硬盘分区然后合并，/dev/md0，<br />
LVM：弹性调整系统容量。<br />
Ext3：文件系统类型，有日志记录。系统默认。<br />
传统的文件格式为：ext2。Journaling有ext3及Reiserfs等。<br />
Vfat：linux，windows共用文件系统。<br />
Grub装载在MBR里。可以设置开机密码，但这样就无法远程登录了。<br />
系统安装过程写入/root/install.log，选择的软件写入/root/anaconda-ks.cfg。<br />
安装包分为多个档案，按需安装。<br />
memtest86：内存压力测试。<br />
因硬件配置无法安装：DVD开机时，boot: linux nofb apm=off acpi=off pci=noacpi<br />
	apm, acpi：电源管理模块；nofb：取消显卡缓存侦测； <br />
SELinux：Access control设定，不是防火墙，推荐安装。<br />
Kdump：核心出错是将内存写入档案。较消耗硬盘空间。<br />
Windows双系统时，Linux所在分区在windows下不要挂载，以免被格式化。<br />

<h3>Chapter 5</h3>
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
档案读写权限：Owner, group, others. Read, write, execute. 
一个账号可以属于多个群组。
所有账号信息存于/etc/passwd，密码存于/etc/shadow 所有组名存于/etc/group。
档案属性：在ls –al列表中显示，如-rw-r--r-- 1 root root 42304 Sep 4 18:26 install.log这七个字段：
	档案类型与权限：共十个字符：
1：d是目录，-是档案，l是link file，b是可随机存储装置，c是串行端口如鼠标键盘（一次性读取），s是资料接口，p是数据传输文件。
		2-4：r读w写x执行。如无该权限该处为-。这里是owner的权限。
		5-7：group内成员的权限。
		8-9：others 的权限。对于目录，如果权限是r而不是x的话，则无法进入。
	连结数：有多少档案名连结到该inode（一个号码），即Hard link数。文件系统用inode记录档案的权限与属性。
	档案拥有者。
	所属群组。
	档案容量：单位byte。
	最后被修改的时间：ls –l –full-time显示完整的包括年的时间格式。
	档名。
改变档案所属群组：chgrp [–R] group dirname/filename filename…。加入R可令次目录下文件亦改变群组。
改变档案拥有者：chown [–R] user dirname/filename filename…。chown user:group filename。
改变档案的权限：chmod。还可用于改变SUID，SGID，SBIT。chmod a+/-x file。ugoa: user, group, owner, all
	数字模式：r4w2x1，一组三个相加。chmod [-R] 750 file使file权限变为drwxr-x---。 
	编写shell以后，将664改为chmod 755 *.sh才可运行。
	特殊权限设置：四位数，最前加4：SUID；2：SGID；1：SBIT，可相加。如chmod 4755 filename产生-rwsr-xr-x。chmod 6755 test产生-rwsr-sr-。(鸟哥的私房菜P222)
	空权限：当档案不可执行却又特殊权限时，该权限为空。如chmod 7666 test产生-rwSrwSrwT。

档案：可为文本文件（ASCII），数据库文件data file，binary program（须有x权限）等，与扩展名无关。rwx权限都是对档案内容而言，w不包括删除档案的权限。删除档案的权限在目录里。
目录：记录文件名列表。r可否ls（可读但不可执行则显示?），w可否新建删除重命名移动，x可否进入（使该目录成为当前的work directory）。
	建站时开放目录，r-x。w不能给，避免删除该目录下别人的档案。
所有人都拥有权限的目录：/tmp。
复制档案：cp soucename destname。
新建空档案：touch directory/documentname。
新建floopy：mkdir floopyname。
切换账号：su vbird。
进入目录：cd /root: into root。cd ..: return to father。cd – 回到上一个工作目录。
用纯文本方式读取档案：cat ~/.bashsc可看到/home/用户名/下隐藏的bashsc文件内容。
读取data file：特定格式，用last (鸟哥的私房菜 P401)来查看过去登陆日志，即 /var/log/wtmp的内容，该档案记录登录的数据。
装置文件device：在/dev/下。分为Block（存储，如/dev/sda）和Character（串口，一次性读取，不能截断输出）。
资料接口Sockets:在/var/run/下，客户端通过它进行数据沟通。
数据传输pipe:FIFO，处理多个程序同时读一个文件时的操作。
扩展名：sh:脚本或批处理(Script)；Z,tar,tar.gz,zip,tgz：压缩文件；html,php：网页
网上下载的档案属性会改变。
文件，档案名长度：255字符，包含路径4096字符。避免特殊字符* ? > < ; & ! [ ] | \ ' " ` ( ) { }。
FHS：Linux目录配置标准。(鸟哥的私房菜 P198)
	可分享的(shareable)	不可分享的(unshareable)
不变的(static)	/usr (软件放置处)	/etc (配置文件)
	/opt (第三方协力软件)	/boot (开机与核心档)
可变动的(variable)	/var/mail (使用者邮件信箱)	/var/run (程序相关)
	/var/spool/news (新闻组)	/var/lock (程序相关)
	可分享：可给其他系统挂载；不可分享：仅与自己机器有关。
	不变：函式库，文件说明，主机服务配置等；可变动：经常写入的文件。
三层目录：/：root, 与开机系统有关，分割槽越小越好，不要在此处安装软件；/usr：软件安装执行有关；/var：系统运作有关。
/bin: 放置的是可以被root与一般账号所使用指令，主要有：cat,chmod,chown,date,mv,mkdir,cp,bash等。
/boot：开机会使用到的档案。Linux kernel常用的档名为vmlinuz，则还会存在/boot/grub/。
/dev：任何装置与接口设备都是以档案的型态存在于这个目录下。有/dev/null,/dev/zero, /dev/tty,/dev/lp*,/dev/hd*,/dev/sd*等。
/etc：系统主要的配置文件，如人员的账号密码文件、 各种服务的初始档等。文件可以让一般使用者查阅的，但是只有root有权力修改。FHS 建议不要放置binary。
/etc/inittab,/etc/modprobe.conf,/etc/fstab,/etc/sysconfig/
/etc/init.d/：所有服务的预设启动script。如要启动或关闭iptables：/etc/init.d/iptables start、/etc/init.d/iptables stop
/etc/xinetd.d/：super daemon管理的服务的配置文件目录。
/etc/X11/：X Window 有关的各种配置文件都在这里，如xorg.conf。
/home：home directory。变量～代表目前用户，～dmtcai代表dmtsai的home directory。
/lib：开机时会用到的函式库，和在/bin，/sbin底下的指令会呼叫的函式库。/lib/modules/放置核心相关的模块 (驱动程序) 。
/media：放置可移除的装置。有：/media/floppy, /media/cdrom等。
/mnt：暂时挂载的额外装置放置到这里。
/opt: 第三方协力软件放置的目录。自行安装的软件也安装到这里，或放置在/usr/local/下。
/root：管理员的家目录与根目录放置在同一个分割槽中。
/sbin：设定系统环境的指令只有root才能使用。/sbin/底下的指令为开机过程中所需要的，包括开机、修复、还原系统所需要的指令。服务软件程序，放置到/usr/sbin/当中。自行安装的软件所产生的system binary，放置到/usr/local/sbin/当中。包括：fdisk, fsck, ifconfig, init, mkfs等。
/srv：一些网络服务启动后所需取用的数据目录。如WWW, FTP等。如WWW朋务器需要的网页资料就放置在/srv/www/里面。
/tmp：一般用户或正在执行的程序暂时放置档案的地方。任何人都能够存取的，需要定期的清理一下。FHS建议在开机时，要将/tmp/下的数据都删除。
FHS未定义但常见的目录：
/lost+found：标准的ext2/ext3文件系统格式产生的，通常会在分割槽的最顶层，即/disk/lost+found/。
/proc：virtual filesystem。数据在内存中，如系统核心、行程信息(process)、周边装置的状态及网络状态等。不占任何硬盘空间。如：/proc/cpuinfo,/proc/dma,/proc/interrupts,/proc/ioports,/proc/net/*等。
/sys：也是一个虚拟的文件系统，记彔与核心相关的信息。包括目前已加载的核心模块与核心侦测到的硬件装置信息等。不占硬盘空间。
函数库：除了在/lib/下还有许多。指令调用的函数。
开机过程中仅有根目录会被挂载，其他分割槽在开机完成后挂载。与开机过程有关的目录，不可与根目录分开：
/etc：配置文件；/bin：重要执行档；/dev：所需要的装置档案；/lib：执行档所需的函式库与核心所需的模块；/sbin：重要的系统执行文件。
/usr：软件开发时合理地将数据分别放置到该目录的子目录中。
	/usr/X11R6/：放置X Window System重要数据最后的X版本为第11版，第6次释出。
/usr/bin/：大部分用户可使用的与开机无关的指令。
/usr/include/：放置c/c++等程序的header和include，用在以tarball方式 (*.tar.gz 的方式安装软件) 安装某些数据时。
/usr/lib/：各应用软件的函式库、object file，以及不被一般使用者用的执行档或script。如64位系统有/usr/lib64/，和一些特殊的，经常被系统管理员操作的，进行服务器的设定指令。
/usr/local/：系统管理员在本机自行安装的软件。如distribution提供的软件较旧，想安装较新的软件但又不移除旧版，该目录下也有bin, etc, include, lib...的子目录。
/usr/sbin：非系统正常运作所需要的系统指令。如某些网络服务器软件的服务指令（daemon）。
/usr/share/：放置共享文件。数据几乎是不分硬件架构均可读取的数据，常见子目录：
	/usr/share/man：联机帮助文件；/usr/share/doc：软件杂项的文件说明；/usr/share/zoneinfo：时区档案。
/usr/src/：一般原始码放置到这里。核心原始码放置到/usr/src/linux/下。
/var：系统运行产生的文件，如cache、log file和软件运行时所产生的档案，如lock file, run file，MySQL数据库的档案等。
	/var/cache/：应用程序本身运作过程中会产生的一些暂存档。
/var/lib/：放置程序执行过程中需使用的数据文件。各软件有各自的目录。如MySQL的数据库放置到/var/lib/mysql/，rpm的数据库则放到/var/lib/rpm/。
/var/lock/：装置或档案资源一次只能被一个应用程序所使用，将该装置lock。
/var/log/：放置登彔文件。如/var/log/messages, /var/log/wtmp(记彔登入者的信息)等。
/var/mail/：放置个人电子邮件信箱，与/var/spool/mail/通常是链接文件。
/var/run/：某些程序或服务运行后，会将它们的PID放置在这个目录。
/var/spool/：放置一些队列数据（排队等待其他程序使用的数据）。这些数据被使用后通常都会被删除。如收到新邮件会放置到/var/spool/mail/中，使用者收下该信件后就会被删除。信件暂时寄不出去会被放到/var/spool/mqueue/中，工作排程数据(crontab)，会被放置到/var/spool/cron/。
Directory tree：根为/；每个目录可使用本地partition的文件系统和网络上的 file system，如Network File System (NFS) 服务器挂载的目录等；每个档案在此树中的完整路径都是独一无二的。
 
/selinux/的内容在内存中。
绝对路径：由/开始写起的文件名或目彔名称。
相对路径：相对于目前路径的文件名写法。如../../home/dmtsai/等。 . ：代表当前的目录，也可以使用 ./ 来表示；.. ：代表上一层目录，也可以 ../ 来代表。
正规的执行目录：/bin/，/usr/bin/下的指令可直接执行。
