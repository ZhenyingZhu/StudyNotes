## Links
https://github.com/google/leveldb
https://github.com/0x00A/ldb
https://github.com/0x00A/ldb/issues/48

## Steps

### ldb
to build ldb on Ubuntu 16.04
```
sudo apt-get autoremove libsnappy-dev
cd ./deps/leveldb
make clean && make
cd ../../
make
```

To read DB
```
ldb /path/
```
where path has MANIFEST-000002


