# Ubuntu

1. Download Ubuntu image

Since your laptop is x64 system, you can download this image:

<https://www.ubuntu.com/download/desktop/contribute?version=16.04.3&architecture=amd64>

1. Download Rufus

<https://rufus.akeo.ie/>
Download "Rufus 2.18 Portable"
It is a tool to create live USBs.

1. Create the live USB
Follow steps 3-6 on <https://tutorials.ubuntu.com/tutorial/tutorial-create-a-usb-stick-on-windows#2>

1. Boot into Grub

Starting from this step, you won't have access to windows. So please open this file on your cellphone or write it down.

Restart your computer. While loading bios (before windows icon shows up), there should be a line on the screen indicates how you could access the boot setting, such as "Press F12 to setup" or "Press Enter". Follow the instrument and boot into your USB.

There would be 2 boot options for your USB. Ones start with "UEFI" an the other doesn't. You can try the "UEFI" one first. If it doesn't work, try the other one.

1. Start Ubuntu

If you boot into Grub successfully, you should see 4 options

- Try Ubuntu without installing
- Install Ubuntu
...

Choose the first one, and press enter.

Wait until the system starts. If it gets stuck and complains
"(initramfs) Unable to find a medium containing a live file system"
try another USB port.

1. Install useful packages

Run the command in the terminal
sudo apt update && apt install -y vim git g++

1. Setup git

First generate ssh keys and

1. Access files on your laptop

In file explorer you can find your windows drivers in the left panel.

1. Setup grub

<https://askubuntu.com/questions/148095/how-do-i-set-the-grub-timeout-and-the-grub-default-boot-entry>

1. Save changes

<https://askubuntu.com/questions/77714/how-can-i-save-settings-on-a-live-usb>
<https://www.howtogeek.com/howto/14912/create-a-persistent-bootable-ubuntu-usb-flash-drive/>

How to format a USB

<https://petejcullen.wordpress.com/2009/11/18/formatting-a-usb-drive-to-fat-32-using-ubuntu/>

How to install language

<https://ubuntuforums.org/showthread.php?t=2240589>

How to config RDP

<https://learn.microsoft.com/en-us/azure/virtual-machines/linux/use-remote-desktop?tabs=azure-cli>
<https://askubuntu.com/questions/1308551/xrdp-disconnects-immediately-after-correct-credentials>

- Don't need to use SSH Key. Just ssh username@ip address with the password works.

How to install java

<https://linuxcapable.com/how-to-install-openjdk-17-on-ubuntu-linux/>

- but can install 21

Install web browser

`sudo apt install chromium-browser`

Install VS code

<https://code.visualstudio.com/docs/setup/linux>
