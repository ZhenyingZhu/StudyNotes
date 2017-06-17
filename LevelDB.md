## Links
https://github.com/google/leveldb
https://github.com/0x00A/ldb
https://github.com/0x00A/ldb/issues/48

## Concepts
- Keys and values are arbitrary byte arrays.
- Data is stored sorted by key.
- Callers can provide a custom comparison function to override the sort order.
- The basic operations are Put(key,value), Get(key), Delete(key).
- can create a transient snapshot to get a consistent view of data.
- Data is automatically compressed using the Snappy compression library.
- version: could store a version number at the end of each key (one byte should suffice for most uses). When you wish to switch to a new key format (e.g., adding an optional third part to the keys processed by TwoPartComparator), (a) keep the same comparator name (b) increment the version number for new keys (c) change the comparator function so it uses the version numbers found in the keys to decide how to interpret them.
- snapshot: Snapshots provide consistent read-only views over the entire state of the key-value store
- WriteBatch: Atomic Updates so that several updates can either all success or fail
- Synchronous Writes: return only when all data written

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

To create DB
```
ldb ./testdb --create
```

To read DB
```
ldb /path/
```
where path has MANIFEST-000002

actions
- get
- put
- del
- ls
- in: search
  - `in keys <regex>` search in keys. Example: `in keys f*`
  - `in values <regex>`
- upper: current range boundary
- lower
- limit: size of current range in bytes


