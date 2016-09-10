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

On client:  
Add to /etc/shadowsocks.json: 

```
{
    "server":"xx.xx.xx.xx",
    "server_port":xxxx,
    "local_address": "127.0.0.1",
    "local_port":1080,
    "password":"xxxxxxxx",
    "timeout":300,
    "method":"aes-256-cfb",
    "fast_open": true,
    "workers": 1
}
```

Then 
```
sslocal -c /etc/shadowsocks.json
```

Set proxy for browser to: 
```
Socks Host: 127.0.0.1
local Port: 1080
Socks v5
```

While using chrome, if proxyswitch does not work, it might caused by other extensions like unblock Youku.  

If on win7, shadowsocks block all traffic: go to internet setting, change the PAC script it use back to the default one.
