# Windows

## File System

<https://support.microsoft.com/en-us/topic/use-the-system-file-checker-tool-to-repair-missing-or-corrupted-system-files-79aa86cb-ca52-166a-92a3-966e85d4094e>

## Find symlinks source

<https://www.howtogeek.com/763271/how-to-view-a-list-of-symbolic-links-on-windows-11/>

- `dir /AL /S <path>`

## Anti Virus

Windows defender: signture and agent versions are different.

## CPU-Z

PCE/IE socket is a key to check.

## VMWare

The vmx file can be updated. It records the file paths.

- Add `mks.enableVulkanRenderer = "FALSE"` can solve the WinXP cannot use 3D acclerate issue. [Workstation 17.6.0: Windows XP starts to a black screen after boot screen](https://www.reddit.com/r/vmware/comments/1fb86d5/workstation_1760_windows_xp_starts_to_a_black/)

## Login

To allow remote desktop connection, `settings > Accounts > Sign-in options`, turn off "Require WIndows Hello Sign-in for Microsoft accounts"
