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

For Virtual box

<https://askubuntu.com/questions/661414/how-to-install-virtualbox-extension-pack>

<https://superuser.com/questions/42134/how-do-i-enable-the-shared-clipboard-in-virtualbox>

- Run as software

<https://superuser.com/questions/1318231/why-doesnt-clipboard-sharing-work-with-ubuntu-18-04-lts-inside-virtualbox-5-1-2>

Default username: vboxuser, changeme

Install Wine

<https://phoenixnap.com/kb/how-to-install-wine-on-ubuntu>

## Gaming

Linux Mint XFCE Edition

- Base: Ubuntu LTS
- Why it’s great: Stable, very light on RAM, and has Ubuntu’s driver and package ecosystem (easy Steam setup).
- Performance: Excellent on CPUs from 2010s onward (even dual-core i3s or older AMD A-series).
- Ease of use: Very beginner-friendly.
- Install Steam Play: `sudo apt install steam`
- Then enable “Steam Play for all titles” in Steam’s settings.

Xubuntu (Ubuntu + XFCE)

- Very similar to Mint XFCE but a bit closer to “pure” Ubuntu.
- You get the same Proton/Steam package support, Ubuntu PPAs, and community help.
- Great balance of simplicity and modern driver support

Lubuntu: As an official, lightweight variant of Ubuntu, Lubuntu uses the LXQt desktop environment to run efficiently on machines with as little as 1GB of RAM. It still provides access to the vast Ubuntu software repositories, making it a good choice for gaming. You may need to manually install lib32 packages for Proton compatibility.

MX Linux: Based on Debian, MX Linux is known for its stability and user-friendliness. It uses a lightweight XFCE desktop by default and is often recommended for older computers due to its low resource usage. Comes with good tools for managing old hardware. Steam can be installed via MX Package Installer. Slightly less “plug-and-play” for gaming than Ubuntu-based ones, but very fast.

Zorin OS Lite: This distribution is specifically designed to be fast on older hardware with as little as 2 GB of RAM. It uses the XFCE desktop environment and offers a polished, familiar interface that can mimic Windows, which is helpful for new users.

<https://www.protondb.com/>

Steam can add game and select Proton. It creates a folder with a windows drive.

## Cloud Drive

rclone can be used for mounting OneDrive

```bash
sudo -v ; curl https://rclone.org/install.sh | sudo bash
rclone config
rclone mount onedrive: ~/OneDrive --vfs-cache-mode writes &
```

## Startup Application

Don't use `~`: `sh -c "sleep 10 && rclone mount onedrive: /home/USER_NAME/OneDrive --vfs-cache-mode writes"`

## Tips

Ubuntu user name lower case work

sudo systemctl restart gdm3 to restart GUI
