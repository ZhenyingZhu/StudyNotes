killall nc
Ctrl+z and fg
service lightdm stop to stop X windows. 

sh-bang: 可用指令<code>env</code>，如#! /usr/bin/env python。

http://serverfault.com/questions/416222/concatenate-first-line-to-the-end-of-second-line-in-a-text-file

<code>grep -f Key.txt All.txt</code>
<code>sleep 60</code> to sleep 60 seconds. 
<code>mail -s "test script" account@mail.com < /dev/null</code> send an email with title "test script". 
<code>head -q -n 2 *.txt</code> Output several files without show the file name. 
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
