# System Design Course

<http://www.jiuzhang.com/course/2/>

## Chapter 1

SRE: 运维

### Normal needs

Design System:

- Twitter: post tweet, follow/unfollow, timeline/news feed
- Facebook: `<b>?</b>`
- Instagram: `<b>?</b>`
- Friend Circle: `<b>?</b>`
- Google Reader(RSS Reader): `<b>?</b>`
- Uber: `<b>?</b>`
- Whatsapp: `<b>?</b>`
- Yelp: `<b>?</b>`
- Design Tiny URL: `<b>?</b>`
- Design NoSQL: `<b>?</b>`

Trouble Shooting:

- What happened if we can not access a website: `<b>?</b>`
- What happened if a webserver is too slow: `<b>?</b>`
- What should we do for increasing traffic: `<b>?</b>`

### Design standard

Start from simple case, for example only two users. Don't over-optimize at the begining.

A good design contains:

- Work Solution
- Analysis
  - 4S
- Special Case
  - If have enough experience, you can predict special cases, which are best.
- Tradeoff
- Knowledge Base

Tradeoff example: local storage or remote storage

- local: fast write/read
- remote: need sync between different hosts and data often change

[Standard of good designer](https://www.linkedin.com/pulse/design-interview-from-interviewers-perspective-joey-addona?forceNoSplash=true)

- gather proper requirements by asking clarifying questions
- build a high level design and can fix design flaws quickly
- can explaining each component
- can expend the design with new requirements by modify the design cleanly, or if necessary, re-design components

### 4S Analysis

#### Scenario

Features/Interfaces.

E.g. Enumerate Twitter features:

- Register / Login
- User Profile Display / Edit
- Upload Image / Video
- Search
- Post / Share a tweet: Important
- Timeline / News Feed: Important
- Follow / Unfollow a user: Important

<https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/MiniTwitter.cpp>

Data:

- QPS(quest/s): need gather first. Can be estimated from DAU and interfaces. Read QPS and Write QPS can affect design tradeoff.
- DAU(daily active users), MAU is different from DAU * 30. [Find user habit from data](http://wechatinchina.com/thread-28820-1-1.html)
- Concurrent User: DAU x (daily avg quest per user) / (24 x 3600 = 86400 ~= 100k). Daily Peak: Concurrent User * 3(experience number, pick from 2 to 9). Fast growing production, peak increase 2 times every 3 months.

Server type:

- QPS < 1000, PC is enough. QPS < 1M, couple servers are enough. QPS > 1M, web server cluster.
- a web server can handle 1k QPS (bottlenecks are logic process time and database query)
- a SQL DB: 1k QPS, less if needs a lot of JOIN and INDEX query
- a NoSQL DB like Cassandra: 10k QPS
- a NoSQL DB like Memcached: 1M QPS

Need consider Single Point Failure, and how to maintainance.

#### Service

Split design into different applications/modules

[SOA](https://en.wikipedia.org/wiki/Service-oriented_architecture)

Services can seperate bugs

Split design by objects. E.g. Twetter `<b>Draw a Graph?</b>`

- Receptionist: Often a router. Pass data to other services
- User Service: register, login
- Tweet Service: post tweets, news feed, timeline. Those data are structurable data, so can be stored in DB.
- Media Service: upload image/video. Those data are store on FS.
- Friendship Service: follow, unfollow

Replay all requirements and merge same requirements(by logic) into a single service.

#### Storage

How to access data(SQL, NoSQL, File System only, In Memory)

Storage type:

- SQL DB: User table which need [ACID](https://en.wikipedia.org/wiki/ACID)
- NoSQL DB: Tweets, social graph which need fast and frequent R/W
- FS: Picture, media files which don't have structure

Design Schema: E.g. Tweet table

- id
- ```user_id``` is a foreign key from user table
- content
- ```created_at``` is timestamp

Not like algorithms, the time complexity is computing DB read times, because DB IO time is much larger than memory access. O(n) DB read/write is not acceptable.

[Fanout](http://stackoverflow.com/questions/22190885/what-is-fanout-in-r-tree)

[Denormalization](https://en.wikipedia.org/wiki/Denormalization): adding redundant copies of data or by grouping data. Disadvantage is not sync at real time.

[Asynchronous](https://en.wikipedia.org/wiki/Asynchrony_(computer_programming)): not block user quest.

##### Design rules

While system design, don't ambivalent. Change plan is very expansive, since if the system is online, changing DB tables needs a lot of work.

Need always consider tradeoff.

The design should be able to, or can be extend to deal with special case.

Draw schema, draw graphs to show storage blocks and data flow.

##### Pull Model

- Friendship table
- Tweet table

Get news feed

1. Query following users
2. Query recent 100 tweets post by those users
3. sort those tweets by time and return recent 100

Time complexity: if N following users

- news feed: O(N) DB reads and O(NlogN) merge K sorted arrays time(which can be neglect). Users wait while this is procceeding.
- post a tweet: O(1) DB write

##### Push Model

- News feed table: each user has a copy of tweet in its news feed. Fields: user id, tweet(which include the tweet author info), tweet create time
- Friendship table

Get news feed

1. find the user in the news feed table and return all tweets in his news feed

Post a tweet has fanout N

1. Query to get all followers
2. add tweet to the news feed table one copy for each followers

Time complexity:

- news feed: O(1) DB read to get tweets
- post a tweet: O(1) DB read to get followers, O(N) DB write to add tweets. But this can be done async in background. Though some users might see the tweet a little later than others, they won't notice.

Disadvantage:

- Some users cannot see updated tweet in time
- Redundancy
- The user with a lot of followers has very slow experience when post tweets

#### Scale

- Optimize
  - Special ways to deal with Special Cases: users with lot of followers, inactive users
  - Add more features
  - Solve design flaws: Indexing, Normalize
- Maintenance
  - Robust: if servers/DBs down
  - Scalability

DB Optimize: [Sharding](https://en.wikipedia.org/wiki/Shard_(database_architecture))

Scale up Pull Model:

- Cache users' timeline. Tradeoff: how many to cache.
- Cache news feed.

Cache is a key-value pair in memory.

Can use in-memory distributed cache: [Memcached](https://memcached.org/), or use disk to cache (disk is cheap).

[Message queue](https://zh.wikipedia.org/wiki/%E6%B6%88%E6%81%AF%E9%98%9F%E5%88%97): For async tasks, each worker get a task from the queue, and report results when done.

Scale up Push Model:

- Inactive users: order by the last login time. If not active, stop his service until he login.
- Users which follwers >> followings: Rank followers by weight. If over a threadshold, use pull model instead.
- Issues with new solution:
  - How to deal with users cross threadshold: One 大V suddenly goes below threadshold. 1. Still pull, 2. Start pushing when he goes below
- Follow/unfollow: Async merge the following's timeline into user's news feed. Evatually sync.

How to store likes:

- denormalize like nums, comment nums, retweet nums from Like table into Tweet table.

How to deal with [Hot spot](https://en.wikipedia.org/wiki/Thundering_herd_problem), when a 大V post a tweet

- Load Balancer? Doesn't help since all LB hosts still need query this DB for the tweet.
- Sharding? Doesn't help since the tweet only store on a single DB
- Consistent hashing? Doesn't help
- Add cache
  - like, retweet, comment changes this tweet, how to update? [write through, write back](http://www.computerweekly.com/feature/Write-through-write-around-write-back-Cache-explained), [look aside](http://whatis.techtarget.com/definition/translation-look-aside-buffer-TLB)
  - How to solve Cache invalid? happened when server down, or cache retirement solution wrong. Use [Facebook Lease Get](http://www.cs.utah.edu/~stutsman/cs6963/public/papers/memcached.pdf)

Guide lines for solve scaling issues:

- Made change as small as possible
- Adding machines is always a solution
- Estimate with numbers

### Details

[Mutual friend](http://www.jiuzhang.com/qa/954/)

- Same amount of friends: 1. sort friend list; 2. find common friends with two points
- A has much less friends than B: Use friends of A as key, query in B friends
- Follow up: top 10 friends that have mutual friends: 1. Use a table always store top 10; 2. When add new friends, async update this table

[How to pagination](http://www.jiuzhang.com/qa/1839/)

- Can only be done by pull
- Use the timestamp of 100th tweet, query 101 tweets from all friends after it, then merge

## Points

### Points Chapter 1

Design System

4S Analysis

Compute read/write QPS from DAU

- a SQL DB: 1k QPS, less if needs a lot of JOIN and INDEX query
- a NoSQL DB like Cassandra: 10k QPS
- a NoSQL DB like Memcached: 1M QPS

Atomicity, Consistency, Isolation, Durability

Denormalization

Asynchronous

Time complexity of DB

write through(slow but I/O safe), write back(fast but not I/O safe)

### Chapter 2

User system:

- Cookie and session
- Friendship

- [Memcached](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/Memcache.cpp): in ram. Support read >>> write. Cache aside.
- Redis: in ram, keep data on disk. Cache through.
- [Cassendra](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/MiniCassandra.cpp): on disk. Raw, column key.

- Cache aside: DB, cache not communicate
- Cache through: first cache, then DB

SQL benefit: Transaction, Query, Campatiable, Sequencial id, Serialization, Secondary Index ...

Column family No SQL benefit: Replica, Sharding

Single point failure:

- Sharding
- Replica

Vertical sharding: seperate table from frequent change parts and non-frequent change parts

Horizontal Sharing:

- Consistent Hashing [1](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/ConsistentHashing.cpp), [2](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/ConsistentHashingII.cpp)

Master - Slave for SQL replica: write ahead log

### Chapter 3

[web crawler](https://github.com/ZhenyingZhu/StudyNotes/blob/master/java-example/WebpageCrawlerSolution.java)

- [URL parser](https://github.com/ZhenyingZhu/StudyNotes/blob/master/python-example/UrlParser.py)
- TaskService - Worker - StorageService
- thread-safe Producer Consumer Pattern: from Message Queue to DB

TaskService: Schema, sharding, Exponential backoff, quota on website

Multi-thread:

- sleep
- condition variable
- semaphore

`_BigTable_`

[Typeahead](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/Typeahead.cpp)

- QueryService: [Trie](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/ImplementTrie.cpp) with top n words on nodes. Store in both memory and disk. [Serialization](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/TrieSerialization.cpp)
- DataCollectionService

Pre-fetch

Probabilistic logging

KB, MB, GB, TB, PB

### Chapter 4

1. Distributed File System: metadata + chunk
2. Map Reduce
3. _Bigtable_

[GFS](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/GFSClient.cpp)

- Peer to Peer
- Master Slave: master for metadata, slave for ChunkServer

Master bottleneck

Checksum

Three Backup location

[Heartbeat](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/HeartBeat.cpp)

### Chapter 5

web system

- DNS: domain, IP Address, URL
- Web server
- HTTP server: on port tcp 80 of web server
- web application: framework

[tiny url](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/TinyURL.cpp)

- QPS, peak QPS, storage size
- GET(short url) return long url, POST(long url) return short url

Solve hash function conflict: add timestamp and retry

Base62

Increase read speed:

- Use DNS to direct requests to different regions, and in each region there is a memcached
- sharing with id, and use Zookeeper to generate sequencial id
- hash(long url) get the sharding key, which is the machine id

[tiny url support customer url](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/TinyURLII.cpp)

[Web logger](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/WebLogger.cpp)

[Rate Limiter](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/RateLimiter.cpp)

### Chapter 6

Map reduce:

- Count word frequency
- Build inverted index

Map reduce framework Steps

1. Input
2. Split
3. Map: find out which machine deal with which part
4. Transmit: On map machine, run external sort; on reduce machine, run merge sort.
5. Reduce: merge the result from map
6. Output

Machine number:

- max is the number of key
- machine boot time should not be too long compare to task running time on each machine

[External Sorting](https://zh.wikipedia.org/wiki/%E5%A4%96%E6%8E%92%E5%BA%8F)

Lookup service

- Master: consist hashing hash the (latitude, longitude) pair to a slave machine
- Slave Lookup system: a map use geo location as key and GFS chunk id as value
- Storage:
  - Big table is optimize for write not read
  - GFS + binary search on file(on disk) directly
- Cache: before GFS

### Chapter 7

Uber

- RingPop: distributed
- TChannel: RPC Protocol
- Google S2: Location query
- Riak: Dynamo DB

[Uber](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/MiniUber.cpp) Services

- Geo
- Dispatch

Geohash [encode](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/Geohash.cpp), [decode](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/GeohashII.cpp)

- Base32 hashcode
- Split an area into 32 pieces
- If the prefixes of two hashcode are same, they are in the nearby location

Query prefix:

- SQL: SELECT * FROM table WHERE geohash LIKE "prefix%". Slow
- Cassandra: range query prefix + 0 to z. The key shouldn't change too ofen
- Memcached: levels of keys, like 1. prefix, 2. prefix+a, 3.prefix+ab. When update a set, need rewrite the whole set

So Redis is the best solution for Uber

Geo Fence: use polygon to define an area

Check if a point is in polygon: from this point emit a line. If the line interset with borders odd times, the point is inside

[Yelp](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/MiniYelp.cpp)

### Chapter 8

Bigtable

- scale up for NoSQL
- optimize for write

Write Ahead Log: The unsorted partition that in memory needs to be written to disk at the same time

Build index

B-Tree

- Sorted list in memory: Skip List
- SStable on disk: Sorted string table, has B-Tree index

Bloom filter: quickly check if a string is inside a SStable

Lock: Chubby, Zookeeper

### Chapter 9

WhatsApp

- Message service
- Real-time service

Message Table

Thread table: Need index by several fields, so choose SQL

- Need index
  - Owner ID
  - Thread ID
  - Participants hash
  - Updated time
- Other fields
  - Participants ID: can be stored in json

Push service + socket

Channel service for Thread

- each client subscribe to channel through push service
- information can be stored in memory only

Rate Limiter

- Need faster than SQL, so use Memcached
- Not need to be very precise
- Key is the current timestamp, and value is the count of this second
- Each key pair is a bucket. Set ttl to be 1 minutes
- When get counts for previous 1 minutes, sum up previous 60 buckets
- Use level buckets

Datadog

- Level bucket
- Aggregate counts in memory, and write to NoSQL DB every 15 seconds, to reduce the QPS to 1/15
- Aggregate Retention

### OOD

Singleton

Factory

OOD is to design the message flow

Manager controls how objects interact with each other

### Summary

Twitter

- post tweet, follow/unfollow, timeline/news feed
- Cassendra

View a web page

User system for web app

- Reg/modify, login, friend
- Service: User, Auth, Friendship
- Cache for large amount of read: Session & cookie
- Load Balancer
- Vertical sharding, user table in a DB, friend table in a DB
- Horizontal sharding use consistent hashing
- SQL master - slave replica

Friend Circle (Friendship Service)

- Cassendra can range query
- Get degree 2 and 3 friends from degree 1 friends
  - Distributed Graph BFS: Producer push degree 1 and 2 friends to queue. Consumer pull a friend and call function to get friends
  - Scaling
    - Consistent Hashing Cache cluster stores n degree friends of each user
  - If cache not hit, query distributed DBs. Before send back result, first merge.
  - Greedy set cover algorithm: find the smallest subset that cover the max number of uncovered points in a large set.
  - Group consistent hashing codes and query each DB with as many users as possible

Web crawler

- TaskService, StorageService, CrawlerService
- Producer consumer
- Message queue to DB
- thread-safe

Google Suggestion (Typeahead)

- Query service using trie. Each node store top n words
- Data collection service with probailistic logging
- Pre-fetch on web browser

DFS

- metadata + chunk with checksum
- Master has metadata, slave has chunk
- backup
- health check using heartbeat

Search on Google

- Domain to IP
- Firewall and Load Balancer
- HTTP request to server (apache)
- framework (Django) use on HTTP Server

Tiny URL

- Scenario:
  - POST a long URL, GET a short URL
  - Redirect short URL to long URL
  - Base62, 5 digits can handle 200 billion URLs
- Service:
  - Tiny URL service
    - assign each URL with an 1. increasing ID(hard to maintain if cross machine, can use another server as master to control assigning id) 2. generate a random number that is not collide with existing URLs
    - encode ID to a string using base62
- Storage:
  - Read QPS: 10 * 500K / 86400 = 50 can be handled by single SQL
  - Write QPS: 1 * 500K / 86400 = 5 can be handled by single SQL
  - 1 URL 100 byte, 1 x 500K x 100 = 50 MB increasing every day
- Scale:
  - Sharding to different machine, key is 1. consist hashing on short URL, with Zookeeper to maintain global id 2. hash long url to get the machine id, and add machine id to short URL
- Source:
  - <http://www.1point3acres.com/bbs/thread-227305-1-1.html>

Lookup Service

- A lookup Service, a storage service
- Need a lot of disk space, so Lookup service use Master slave
- sharding key is consist hasing of lat lon pair
- store on DFS
- Master keeps a key (lat lon) to slave server
- slave server keeps a key to DFS chunk map

Uber (Yelp)

- Geo Service: geohash by search for same prefix
- Dispatch service
- Driver report location needs a lot of write
- Use redis to store geohash using levels of keys

Big Table (Dynamo DB)

- Storage: Distributed file system
- Files are sorted by the index
- Changes are appended not modify previous files
- Bloom filter
- Memory keeps metadata
- Need a distributed lock

WhatsApp

- Message Service, Realtime Service
- Message Table (NoSQL), Thread table with paticipants hash(multiple index, SQL)
- Push service with socket
- Group chat, maintain channels for live users

Rate Limiter

- Memcached
- each second is a bucket
- level buckets

Datadog

- Storage: NoSQL
- level bucket
- first store counts in memory
- every 15s write to DB

Web Logger： real time, last 1 hour, most frequent 10 IP

- Solve in memory, less than 1G is fine. write and read back is too expensive
- Use hash count, don't matter too much if collide. O(1) get IP and update frequent, O(logk) to get top k frequency
- reread the logs in previous 1 hour to reduce count

Amazon product page

- Scenario:
  - show product info using 1 DB read
  - show product pictures using n disk IO
  - QPS
- Service：
  - Product storage
  - Product page render: getItem, getPic
  - Suggest product: other people review history， Product weighted graph, and use BFS to get a list
  - Master slave + LB
- Storage:
  - SQL + memcached + GFS
- Scale:
  - Sharding by item name

Linkedin user profile

- Scenario
  - Create/update user profile: 1 DB write, FS write
  - search user: 1 DB read, n FS read
- Service
  - master looks up DB using consistent hashing
  - distributed DB: SQL
  - DFS for rich media file
- Storage
  - SQL and cache should be fine
  - If search for users in a company or so, use Column family NoSQL
  - Distributed File system for files
- Scale
  - sharding by user name, company

Evernote (Dropbox)

- Scenario
  - Write note 1 DB write, 1 FS write
  - Search note with n words, O(n) DB read, then O(1) merge
  - Collaborate: 1 DB write, 3 ways merge/lock
- Service
  - Note storage: SQL vs NoSQL for metadata, FS for rich format. Can append only diff, or split into blocks
  - User system: SQL
  - Friendship: SQL, can merge into user system if not too complicate
  - Search: async build inverted index
  - Conflict resolver: get two files and resolve difference
- Storage
  - SQL for metadata, DFS for content
- Scale
  - DFS writes file in pieces. When modify, read all pieces, split again after modified, and write back
  - To resolve conflict, only lock when check in. Use 3 ways different to merge

Weekly digest

- Scenario
  - repost, 1 DB write
  - Weekly digest: O(k) DB reads
- Service
  - Post storage service: consistent hash by artical name
  - Digest service: get top 100 artical from each server, and merge sort
- Storage
  - Redis: artical 2 count
  - A priority queue to store only 100 article
- Scale
  - sharding
  - Each server only report a small portion

Delay Scheduler

- Scenario
  - place an event: a timer class: name, time, run()
  - execute the event: Execute in a thread
  - cancel a thread
- Service
  - Scheduler: a min heap on time and value is thread. Add or cancel.
  - Executor: a dispatch thread. sleep from call to call. Waken up every time a thread is added or deleted. Then check the top of the min heap, and sleep or execute it.
- Scaling
  - Need lock the min heap
  - Use `condition_variable::wait_for(unique_lock, time, func);` and `condition_variable::notify_one()`. Or `thread::interrupt()`

Kafka

- Source: <https://zhuanlan.zhihu.com/p/20545422>
- Service
  - Producer: gather logs and send to broker. Push.
  - broker: distributed logs to consumers. Use 2 buckets, which are rolling list of sub level buckets
  - consumer group： pull from broker
  - zookeeper
- Scaling
  - Broker needs throttler to delay producer
  - Producer use round robin to push to different brokers
  - Message from producer use CRC to identify error package
  - zookeeper is a distributed DB, maintains which message is on which broker. Consumers query zookeeper to get the message?
  - message is ordered in memory and stored on disk. Timestamp is stored in message
  - Offsets on a broker can be binary searched to get
  - Broker backup: Primary sync each message to slave

Calendar

- Scenario
  - Insert event, delete event: DB O(1) write
  - Query event/Check available:: DB O(1) read
  - Invite others: DB O(1) read and write
  - Notify users: DB O(n) read
- Service
  - Calendar
  - Notification: Subscription
- Storage
  - MySQL: event name, start, end, user set. Repeat event: event id, metadata key, metadata value, where meta key is repeat start and repeat interval num, and value is start time and length.
  - Memcached
  - Candance: partition key: email, clustering key: start time
- Scale
  - Sharding by user/event
  - memcached for event query

- Facebook: `<b>?</b>`
- Instagram: `<b>?</b>`
- Google Reader(RSS Reader): `<b>?</b>`

## NineChapter

cache:

- what: a mem
- why: reduce read time
- how: read: write:
- TTL:
- modes:
  - through: write: cache -> DB write num is less
  - back:
  - aside: server write to DB/cache separately. memcached+mysql
- hit rate:

Session:

- what: store user login info
- why: reduce user login
- how: a DB: store: session key, user id, TTL. session key return to cx and store in cookie.
- questions:
  - cookie retain how long
  - change device
  - when to delete
