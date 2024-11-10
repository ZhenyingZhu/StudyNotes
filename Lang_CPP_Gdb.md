http://wiki.ubuntu.org.cn/index.php?title=%E7%94%A8GDB%E8%B0%83%E8%AF%95%E7%A8%8B%E5%BA%8F&variant=zh-hans

[Start with args](https://stackoverflow.com/questions/6121094/how-do-i-run-a-program-with-commandline-args-using-gdb-within-a-bash-script)
```
gdb --args executablename arg1 arg2 arg3
```

To make GDB work in Docker: 
```
sudo docker run -it -p 2220:22 --security-opt seccomp=unconfined --name ${container name} -v ${local path}:${mount path} ${image} bash
```
