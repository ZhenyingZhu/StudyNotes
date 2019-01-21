Hyper-V doesn't have a valid IP configuration

https://post.smzdm.com/p/481040/

Virtual box cannot start: [raw-mode unavailable courtesy of Hyper-V](https://discuss.erpnext.com/t/virtualbox-wont-run-raw-mode-unavailable-courtesy-of-hyper-v/34541)

- CMD Administrator run `bcdedit`
- `bcdedit /set hypervisorlaunchtype off`
- reboot and run Virtual box
- `bcdedit /set hypervisorlaunchtype auto`