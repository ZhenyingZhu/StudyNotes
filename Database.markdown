## MySQL
[Install](https://support.rackspace.com/how-to/installing-mysql-server-on-ubuntu/)

### Start
Start mysql shell:
```
sudo service mysql start
/usr/bin/mysql -u root -p
```

Queries:
```
SHOW DATABASES;
CREATE DATABASE test;
USE test;
```

```
CREATE TABLE test (id INT NOT NULL AUTO_INCREMENT, name VARCHAR(20), PRIMARY KEY(id) );
DESCRIBE test;
INSERT INTO test (id, name) VALUES(1, "hello");
INSERT INTO test (name) VALUES("world");
SELECT * FROM test;
```
