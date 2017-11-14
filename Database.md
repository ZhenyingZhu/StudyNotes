## MySQL
[Install](https://support.rackspace.com/how-to/installing-mysql-server-on-ubuntu/)

[Visual studio support](https://bugs.mysql.com/bug.php?id=85908)

### Start
Start mysql shell:
```
sudo service mysql start
/usr/bin/mysql -u root -p
```

### Queries

#### DB
```
SHOW DATABASES;
CREATE DATABASE test;
USE test;
```

Describe all tables in a db
```
SELECT * FROM information_schema.columns WHERE table_schema = 'db_name';
```

#### User
```
SELECT User, Host FROM mysql.user;
SET global validate_password_policy=LOW;
GRANT ALL PRIVILEGES ON dbTest.* To 'user'@'hostname' IDENTIFIED BY 'password';
```

#### create table and schema
```
CREATE TABLE test (id INT NOT NULL AUTO_INCREMENT, name VARCHAR(20), PRIMARY KEY(id) );
DESCRIBE test;
INSERT INTO test (id, name) VALUES(1, "hello");
INSERT INTO test (name) VALUES("world");
SELECT * FROM test;
```

#### migrate db
```
mysqldump -u root -p[root_password] [database_name] > dumpfilename.sql
mysql -u root -p[root_password] [database_name] < dumpfilename.sql
```

#### Select on Date
```
SELECT Questions.qid, Questions.url, Questions.difficulty, Metadata.priority, Metadata.last_touch FROM Questions LEFT JOIN Metadata ON Questions.id=Metadata.id WHERE DATE(last_touch) >= DATE('2016-12-15');
```

#### Count numbers
```
SELECT COUNT(*) FROM Questions LEFT JOIN Metadata ON Questions.id=Metadata.id WHERE DATE(last_touch) >= DATE('2016-12-15');
```

#### Limit result
```
SELECT Questions.qid, Questions.url, Questions.difficulty, Metadata.priority, Metadata.last_touch FROM Questions LEFT JOIN Metadata ON Questions.id=Metadata.id WHERE priority=5 ORDER BY qid LIMIT 10;
```

## Memcached
[Install](https://www.liquidweb.com/kb/how-to-install-memcached-on-ubuntu-14-04-lts/)

### Start
[Command line](http://www.alphadevx.com/a/90-Accessing-Memcached-from-the-command-line)

```
service memcached restart
telnet localhost 11211
stats

```

### miscellaneous
[Schema vs Database](https://stackoverflow.com/questions/11618277/difference-between-schema-database-in-mysql)

[Auto increase ID size](https://stackoverflow.com/questions/3562737/what-size-int-should-i-use-for-my-autoincrement-ids-mysql)

[VCHAR size](https://stackoverflow.com/questions/13506832/what-is-the-mysql-varchar-max-size)

[VCHAR vs TEXT](https://stackoverflow.com/questions/25300821/difference-between-varchar-and-text-in-mysql)

[INT(7) is display width](https://stackoverflow.com/questions/5562322/difference-between-int-and-int3-data-types-in-my-sql)

https://en.wikipedia.org/wiki/InnoDB
