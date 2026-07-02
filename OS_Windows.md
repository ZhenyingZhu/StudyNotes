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

## Proc Mon

Can use process monitor to find what files and regkeys r/w for a process. Use filter for a process name and do the operations, and then save the logs to a PML file. AI can parse the PML file to a csv.

## VMWare

The vmx file can be updated. It records the file paths.

- Add `mks.enableVulkanRenderer = "FALSE"` can solve the WinXP cannot use 3D acclerate issue. [Workstation 17.6.0: Windows XP starts to a black screen after boot screen](https://www.reddit.com/r/vmware/comments/1fb86d5/workstation_1760_windows_xp_starts_to_a_black/)

## Login

To allow remote desktop connection, `settings > Accounts > Sign-in options`, turn off "Require WIndows Hello Sign-in for Microsoft accounts"

## Group Policy

`gpedit.msc` find configs. For example, `Computer Configuration > Administrative Templates > Windows Components > Remote Desktop Services > Remote Desktop Session Host > Device and Resource Redirection`

## Manage right click menu

1. Paste HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked into the Path at the top and press the Enter-key.
2. Select New > String value. Name it {1FA0E654-C9F2-4A1F-9800-B9A75D744B00}. Value to OneDrive.

## Copy files

robocopy is much faster when copying small files, because the default copy does extra things for each file.
