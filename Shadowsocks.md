https://plus.google.com/103234343779069345365/posts/Xce4EJpLGhX

On server:  
```
yum install epel-release
yum update
yum install python-setuptools m2crypto supervisor
easy_install pip
pip install shadowsocks
```

Add to /etc/shadowsocks.json:  
```
{
    "server":"0.0.0.0", # AWS use private IP
    "server_port":8388, # AWS need ingress open
    "local_port":1080,
    "password":"yourpassword",
    "timeout":600,
    "method":"aes-256-cfb"
}
```

Add to /etc/supervisord.conf:  
```
[program:shadowsocks]
command=ssserver -c /etc/shadowsocks.json
autostart=true
autorestart=true
user=root
log_stderr=true
logfile=/var/log/shadowsocks.log
```

Add to /etc/rc.local:  
```
service supervisord start
```

Then reboot.  

