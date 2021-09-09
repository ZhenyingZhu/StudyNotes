# System Design

## NineChapter

<http://www.jiuzhang.com/course/2/>

### Chapter 1 Notes

SRE: 运维

#### Normal needs

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

#### Design standard

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

#### 4S Analysis

##### Scenario

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

##### Service

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

##### Storage

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

###### Design rules

While system design, don't ambivalent. Change plan is very expansive, since if the system is online, changing DB tables needs a lot of work.

Need always consider tradeoff.

The design should be able to, or can be extend to deal with special case.

Draw schema, draw graphs to show storage blocks and data flow.

###### Pull Model

- Friendship table
- Tweet table

Get news feed

1. Query following users
2. Query recent 100 tweets post by those users
3. sort those tweets by time and return recent 100

Time complexity: if N following users

- news feed: O(N) DB reads and O(NlogN) merge K sorted arrays time(which can be neglect). Users wait while this is procceeding.
- post a tweet: O(1) DB write

###### Push Model

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

##### Scale

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

#### Details

[Mutual friend](http://www.jiuzhang.com/qa/954/)

- Same amount of friends: 1. sort friend list; 2. find common friends with two points
- A has much less friends than B: Use friends of A as key, query in B friends
- Follow up: top 10 friends that have mutual friends: 1. Use a table always store top 10; 2. When add new friends, async update this table

[How to pagination](http://www.jiuzhang.com/qa/1839/)

- Can only be done by pull
- Use the timestamp of 100th tweet, query 101 tweets from all friends after it, then merge

### Points

#### Chapter 1

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

#### Chapter 2

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

#### Chapter 3

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

#### Chapter 4

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

#### Chapter 5

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

#### Chapter 6

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

#### Chapter 7

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

#### Chapter 8

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

#### Chapter 9

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

#### OOD

Singleton

Factory

OOD is to design the message flow

Manager controls how objects interact with each other

#### Summary

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

### Review

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

[Cassendra](https://cassandra.apache.org/):

- what: no relationship db
- why: write enhance. high volume data
- how:
  - Keyspace: defines how a dataset is replicated, for example in which datacenters and how many copies. Keyspaces contain tables.
  - Table: defines the typed schema for a collection of partitions. Cassandra tables have flexible addition of new columns to tables with zero downtime. Tables contain partitions, which contain partitions, which contain columns.
  - Partition: defines the mandatory part of the primary key all rows in Cassandra must have. All performant queries supply the partition key in the query.
  - Row: contains a collection of columns identified by a unique primary key made up of the partition key and optionally additional clustering keys.
  - Column: A single datum with a type which belong to a row.
- questions:
  - transaction
  - update schema is hard
  - query is hard

Sharding/partition

- what: separate data
- why: distributed
- how: hash
- questions:
  - single point of failure
  - virtial sharing: waste space, query on two machines need more time

Replica

- what: copy
- why: prevent single machine failure cause system failure
- how: primary - secondary
- questions:
  - how to write

News feed

- tables:
  - feed: author, time, article
  - user: user id, user name
  - friendship: from, to. single direction, double direction.

Distributed file system

- what: big file
- why: cheap
- how:
  - main: metadata (position), worker: chunk
  - read: if the worker is not available, client go to backup.

Collab file editor

- what:
  - create
  - editor, remote, real time, save
- why: remote work
- how:
  - store: distributed file system. every 3 mins merge and store
  - connection: use WebSocket.
  - redis: store the temp change
  - session: store the current content.
  - Use [Operational Transformation](https://zhuanlan.zhihu.com/p/30890457)
  - each line is a linked list node
  - a hash table to store node pointers
  - send request: the diff metadata: the offset, the op (add, delete, replace).
  - receive request: updates as well
  - message queue: to prevent too many changes cannot be processed.
  - also need to deal with failure case.

Spring boot

- M: Java bean
- V: use FE tech
- C: java
- cache: <https://spring.io/guides/gs/caching/>

Big table

- what: a huge table. support query, update (add, update, delete).
- why:
- how:
  - a machine with index, record the machine id.
  - store in the mem first, when mem is full, start writting to disk. The chunk is written that is sorted.
  - Sharding: ring.

Online chat

- what: real time, with or without notification, multiple (>2) people, always with same thread
- why:
- chanllege: real time.
- how:
  - cookie, session
  - message queue
  - serialization
  - operation:
    - create chat
- **TODO**

Stream

- what: upload, view, share, thumbnail, progress bar, up/down vote, subscription, watchlist, recommandation, report, search, comments, notification.
- why: encoding to give the same experise
- challenge: upload, encoding, video, thumbnail.
- how: first upload with resume and break point, then on server side encoding
  - worker server: give a video id, distributed lock.
  - cache store the resume and break point.
  - video table: name, video id as the key, hash of the video content, so that users can have multiple video id, resolution, size, duration, language
  - trunk table: video id, trunk id, start/end time, folder+machine
  - preload

Map Reduce

- what: programming model, processing and generating big data sets, parallel, distributed algorithm
- why: cost
- challenge: data size, time bond
- how:
  - where is the input come from: web crawler. sharding using content hash/shuffle/message queue
  - process: get cutom map algorithm
  - map output: stream, dispatcher, reducer input
  - main: maintain queue for mapper, reducer. mapper should store the output.

Twitter

- what: weibo
- why: news feed
- challenges:
  - distributed id: [snow flake](https://zhuanlan.zhihu.com/p/85837641)
  - R/W QPS: retweet
  - hot spot
  - reverse index: big size
  - big storage: media content
  - Ranking: hot, whether subscription, Timeliness
  - tokenizer: parsing the message.
- Components
  - Index
  - Search
  - Ranking
- Redis: cache
- File store: blob store
- Hadoop: anal user info
- MySQL: strutured data
- hot cold data isolation
- read, write isolation
- early bird: search engine
- HDFS store the index

B+ Tree

- what: each node points to a pointer
- why: make the search
- how: **TODO**
  - add
  - search
  - rebalance

Index

- what: a data struture to help with search
- MyISAM, InnoDB: storage engine.
- MyISAM:
  - secondary key: store data address
- InnoDB:
  - secondary key: store primary key

Transaction

- what: a set of operation
- why: consistency, atomic, isolation, durability.
- how:
  - isolation: can reuse multi-threading design.
    - level1: read uncommit: a transaction can see another transaction's uncommitted change.
    - level2: read commit
    - level3: repeat read
    - level4: serialzible
  - Concurrency: dirty write.

Search engine

- what: given a string, return the sorted result.
- why: find resources.
- how:
  - bubble sort.
  - top k.
  - key value store.
  - reverse index.
  - web crawler:
    - 1. grab page, 2. save, 3. find more link.
    - speed, storage, robot, circular
    - timestamp: if the page get updated. exponantial backoff when page changes.
    - scheduler.
  - index
    - tokenize: where to break the sentencite
  - query engine.
  - type-ahead

## Designing Data-Intensive Applications

<https://learning.oreilly.com/library/view/designing-data-intensive-applications/9781491903063/preface01.html>

### Preface

- NoSQL
- Big Data
- Web-scale
- Sharding
- Eventual consistency
- database ACID: atomicity, consistency, isolation, and durability.
- CAP theorem: impossible to provide all 3: Consistency, Availability, Partition tolerance
- Cloud services
- MapReduce
- Real-time

- message queues
- caches
- search indexes
- batch and stream processing

distributed system: consistency and consensus.

heterogeneous system

### Part 1. Foundations of Data Systems

#### Chapter 1. Reliable, Scalable, and Maintainable Applications

Review

1. 9/2/2021

- databases: multiple apps can read and write
- caches
- search indexes
- stream processing: another process handle message async
- batch processing: periodically crunch large amount of data.

Common services:

- Redis: datastores that are also used as message queues. **[KEY]**
- Apache Kafka: message queues with database-like durability guarantees. **[KEY]**
- Memcached: application-managed caching layer. **[KEY]**
- Elasticsearch/Solr: full-text search server. **[KEY]**

3 concerns **[KEY]**:

- Reliability
- Scalability
- Maintainability

Netflix Chaos Monkey:  trigger fault deliberately. **[KEY]**

##### Reliability

Common faults:

MTTF: mean time to fail **[KEY]**

- Hardware: hard disk, RAM, power, network
  - add redundency: RAID, dual power supplies, hot swappable CPU, backup generators. Good for single machine. Downtime could be long.
  - software fault tolerance: for system prioritize flexibility and elasticity over single-machine reliability. No downtime for the whole system.
- Software: bug, too much resource consumption, dependency failure, cascading failures (a fault triggers another fault)
  - check the assumptions are still true
  - process isolation **[KEY]**
  - watchdog
- Human error
  - well designed API, UI
  - sandbox
  - auto tests: UT, intergration test, manual test
  - easy recovery: rollback, gradually rollout, data integrity check.
  - detailed and clear monitor/telemetry: performance metrics and error rates.
  - training people

##### Scalability

Cannot make a system generically scale. Need to discuss a particular way.

load parameters **[KEY]**:

- QPS
- R/W ratio
- simultaneously active user
- cache hit rate
- hot spot

fan out: 1 write leads to k writes.

twitter **[KEY]**:

- tweet write: 4.6k/s, timeline read: 300k/s
- first approach: timeline query reads the database. Cannot stand with timeline query requests.
- second approach: when post a tweet, insert it to each user's timeline cache. Because timeline read request is two order (100x) higher than post a tweet. But write a tweet averagely fan out to 345k/s.
- the distribution of followers per user is important, because most of the users only have ~75 followers, while some has 30M followers.
- final approach: hybrid both approach 1 and 2.

Measure performace:

- throughput **[KEY]**: the number of records we can process per second, or the total time it takes to run a job on a dataset of a certain size.
  - Critical for batch processing system (e.g., Hadoop)
- response time **[KEY]**: what client see.
  - vs. latency **[KEY]**, which is when the request is awaiting in the system.
  - could be vary from time to time because it includes other facts like background process context switch, network package loss, TCP retransmission, garbage collection pause, page fault cause re-read from disk, mechanical vibrations in the server rack
  - Need to measure the distribution of value **[KEY]**: average, P50, P95, P99, P99.9 percentiles. tail latencies: high percentile of response time.
  - service level objectives (SLOs) and service level agreements (SLAs) use percentiles
- Queueing delays **[KEY]**: normally affect high percentile. head-of-line blocking effect: it only takes a small number of slow requests to hold up the processing of subsequent requests.
- to measure performance under high load, the load generation client should send requests in parallel not one by one.
- tail latency amplification: a slow call increases when client needs multiple backend calls for a single request, a higher proportion of the whole request requests end up being slow.
- use a rolling window **[KEY]** to calculate percentile in the last 10 mins for the monitor. To calculate that efficiently, can use algorithm: forward decay, t-digest, or HdrHistogram.

Handle load increase

- scaling up (vertical scaling, moving to a more powerful machine)
- scaling out (horizontal scaling, distributing the load across multiple smaller machines)
- elastic: automatically add computing resources when they detect a load increase
- distribute stateless system is easy, but stateful data system could be hard.
- Early-stage should iterate quickly on product features than it is to scale to some hypothetical future load.

##### Maintainability

- Operability
  - monitor health, visible to the runtime behavior and internal of the system. restore service
  - track down system failures or degraded performance
  - keep software and platform up to date
  - keep check how different services affect each other, to avoid one service completely break another one
  - anticipate future problem and solve them (e.g., cap planning)
  - establish good practice and tools for deploy, config
  - perform maintenance tasks, e.g., migrate platform
  - maintain security
  - define process to make ops predictable
  - share knowledge
  - provide good default behavior for tooling, but also provide the flexibility
  - self-healing, but also allow admin control
- Simplicity
- Evolvability: equals to extensibility, modifiability, or plasticity

big ball of mud: A software project mired in complexity

- explosion of the state space
- tight coupling of modules
- tangled dependencies
- inconsistent naming and terminology
- hacks aimed at solving performance problems
- special-casing to work around issues

Use abstraction to hide the complexitity.

Agile:

- TDD
- Refactoring

skew **[KEY]**: data not being spread evenly across worker processes

#### Chapter 2. Data Models and Query Languages

Review

1. 9/3/2021

Data models: how we think about the problem that we are solving

Layers **[KEY]**:

- application: real world to data strutures, and APIs to manipulate them
- store: general-purpose data model, e.g., JSON, XML, tables, graph model
- database: bytes in RAM, disk, network.
- hardware

data models **[KEY]**:

- relational model
- document model
- graph-based data models

Relational Model **[KEY]**:

- relational database management system: (RDBMS)
- transaction processing
- batch processing
- competitor: network model, hierarchical model

NoSQL **[KEY]**:

- open source, distributed, nonrelational databases
- greater scalability: very large datasets or very high write throughput
- Specialized query operations
- a more dynamic and expressive data model

Object-Relational Mismatch:

- between SQL and OOD need to have a translation layer
- Object-relational mapping (ORM) frameworks, e.g., ActiveRecord and Hibernate
- SQL supports structured datatypes and XML data now. This allowed multi-valued data to be stored within a single row, with support for querying and indexing inside those documents.
- Document-oriented databases **[KEY]**: e.g., MongoDB, RethinkDB, CouchDB, and Espresso support document model (JSON), which is native to OOD.
- Lack of schema is good in some cases.
- Require less queries for document model.
- Easy to represent tree struture **[KEY]** = one-to-many with document model

normalization **[KEY]**: use id to store object while give the human interept info easy changeable.

Many to many:

- document model couldn't support well
- the join logic needs to be shifted to application

Information Management System (IMS):

- hierarchical model
- to support join: whether to duplicate (denormalize) data or to manually resolve references from one record to another.

Network model:

- a.k.a, CODASYL model
- a record could have multiple parents

Relational databases:

- open data: a table is simply a collection of tuples
- access path **[KEY]**: query optimizer decides which part to query, in which order.
- the query won't change even adding a new index, access path will change automatically
- general purpose optimizer is very complicate

Document databases:

- hierarchical model: store nested records
- use document reference (similar to foreign key) to represent many-to-one and many-to-many
- reference identifier is resolved at read time, using join or follow up queries

Relational vs. Document databases:

- fault tolerance
- concurrency handling
- schema flexibility
- performance

Doc model limitation:

- cannot directly refer to a nested document. Need to first find its parent node. But unless it is deeply nested, it normally don't cause an issue.
- join is not good supported. If really needed, would need to denormalize data consistent. Graph model is more nature for such case

XML supports in relational database comes with optional schema validation.

document databases are schema-in-read: the structure of the data is implicit, and only interpreted when the data is read.

When need to update the schema:

- Document database: start writing new data and let application deal with both old and new data
  - So if the data in a table could be different types, or structure is determined by external system, use schemaless DB is better
- relational database: perform a migration.
  - most DBs can handle ALTER TABLE (to add a new field) quickly, but MySQL would copy the whole table so it is slow
  - UPDATE to change the values of each row is slow

Data locality: all the data of an object stores in one place, encoded in JSON.

- If the data often needs to read together, this can save some queries across different tables
- but when only a small part is read, or need to update, the whole document needs to R/W. So need keep the document small. **[KEY]**
- when document size increases, it cannot be updated in place.
- not only in document type database, but also in:
  - Google spanner DB: interleaved table rows.
  - Oracle: multi-table index cluster table
  - Bigtable used by Cassandra and HBase: column-family

Query lang:

- SQL: Declarative query. specify the pattern of the data and how the data transformed (sorted, grouped, aggregated)
  - DB can do improvements and don't worry about breaking queries
  - can parallel excute
- IMS, CODASYL: imperative code. Define what steps to do.

MapReduce: programming model

- MongoDB and CouchDB also use it to perform RO queries
- map: collect. Emit a key to group and a value. **[KEY]**
- reduce: fold/inject. Called once for each key. **[KEY]**
- map and reduce must be pure functions: can only use pass-in data as RO, not do DB queries. So it can be used anywhere.
- SQL can also use this framework.
- aggregation pipeline: MongoDB uses it to do declarative queries over MapReduce rather than JS.

Graph-Like Data Models

- page rank: uses web pages as vertices and links as edges **[KEY]**
- vertices don't necessary to be the same object types.
- property graph model: Neo4j, Titan, and InfiniteGraph **[KEY]**
  - vertex: a uniq id, outgoing edges, incoming edges, properties (key-value pairs)
  - edge: a uniq id, tail vertex (start), head vertex (end), label (kind of the relationship), properties (key-value)
  - use two relational DB tables to store vertex and edge.
  - can traverse both forward and backward
  - Neo4j use Cypher Query Language: declarative. define vertex name and its json properties, then use the names to create relations.
    - CREATE clause defines vertices and edges. MATCH clause finds vertices based on properties on vertices and edges.
  - SQL to query graph:
    - challenge is not knowing how many vertices on the path, so the JOIN number is not fixed.
    - recursive common table expressions: WITH RECURSIVE to traverse the edges
- triple-store model: Cypher, SPARQL, and Datalog
  - similar to property graph model.
  - all info stores in 3-part statements: subject, predicate, object.
  - subject: like vertex
  - object:
    - when predicate is a property, then object is its value
    - when predicate is an edge, object is another vertex
  - semantic web: publish information as machine-readable data
    - Resource Description Framework (RDF): out-of-date. subject, predicate, object could be URLs with domains to resolve conflicts.
  - SPARQL query: query RDF. Powerful even without RDF.
  - Datalog **[KEY]**: foundation. Hadoop uses it to query large datasets. Define rules to query which can be reused. So one-off query is hard to write but query in complex data can be break down into steps and reuse rules.
- graph query languages: Gremlin
- graph processing frameworks: Pregel

#### Chapter 3. Storage and Retrieval

Storage engines by purpose:

- optimized for transactional workloads
- optimized for analytics

Storage engines by strcture:

- log-structured storage engines, and
- page-oriented storage engines such as B-trees.

Data Structures **[KEY]**

- simplist log DB: a text file. each line contains a key-value pair. the old values are not overwritten, so search from end.
  - Real DB needs to deal with concurrency control, reclaim disk space, handle errors and partially written data.
- index: keep metadata aside. increase overhead on write

Hash indexes

- simplist solution for log DB: keep a hash map: key is the key, value is the offset.
  - All indexes store in RAM. Values store on disk and a part of it loaded in FS cache. **[KEY]**
  - good for scenarios where values of each key needs to be updated freq, but not too many uniq keys
  - avoid running out of disk space: break log into segments
  - compaction: throw away dup keys. Can merge multiple segments into one in parallel with compaction. While doing so, old segments are still available for read. After it is done, read from new segments.
  - Each segment has its own in-mem hash table
  - File format: csv is inefficent. use an encoded binary string. The length follow by the raw column. **[KEY]**
  - deletion: append a special record (tombstone). **[KEY]**
  - crash recovery: to recovery the index, re-iterate all the segments is slow, so store a snapshot on disk **[KEY]**
  - partial writes: maintain checksums and ignore corrupted logs **[KEY]**
  - concurrency control: use only one write thread. Segments are read only. Reads can be served concurrently. **[KEY]**
  - Benefits:
    - fast writes on HDD and SSD.
    - Simple for concurrency and crash recovery
    - merging old segments to avoid files getting fragmented.
  - limitations:
    - key size
    - range queries not efficient

SSTables and LSM-Trees

- sorted string table (SSTable): key value pairs sorted by key in each segment **[KEY]**
- write using sequential I/O. First write to input segment. After certain threshold, input segment saves to disk.
- Use merge sort to merge multiple segments into one.
- while doing the merge, no need to load whole segments from disk, but the first key in each segment, so use less memory.
- Sparse index: no need to index of all the keys, but one key for every few kilobyte blocks. **[KEY]**
- Each block can be compressed. It can reduce the disk space and I/O bandwidth.
- memtable: use red-black trees or AVL trees to make the inserting keys are done in sorted order in memory. **[KEY]**
- When read, first search in the memtable, then the most recent on-disk segment, then older segments.
- periodically merge segments.
- to recover from crash, while the memtable is in memory, every writes also write to a disk file that is not sorted.
- Log-structured Merge-Tree (LSM-Tree): the indexing structure. write throughput is high. **[KEY]**
- full-text index: given a word in a search query, find all the documents have this word (term) using a term dictionary.
- Q: the input segment might have some duplicate appears in other segments, how to maintain a global index? From below the index is not global.
- Use bloom filters: a mem-efficient data struture for approximating the contents of a set. Tells if a key doesn't appear in the DB. A false positive check (can determine key definately not exist). **[KEY]**
- size-tiered tied compaction: newer and smaller SSTables merged into older and larger SSTables **[KEY]**
- leveled compaction: key range is split into smaller SSTables with no overlapping keys. Older data is moved into separate levels of files. **[KEY]**

B-Trees

- DB is break down into fix sized (4KB) pages. **[KEY]**
- R/W one page at a time. Correspond to disk.
- each page is identified by the address. One page can be referenced by another. **[KEY]**
- root: also a page.
- Each page starts and ends with ref and boundaries with in, except leaf pages which have keys and values in line. **[KEY]**
- branching factor: number of references in one page. Depends on the total space needed for store all the data. Normally couple hundreds. **[KEY]**
- When add a new key, if the leaf doesn't have enough space, it will be broken into two half-full pages, and update parent page references. **[KEY]**
- the depth for n keys: O(logn). It affects the speed of the search. Should be 3~4 levels. If factor is 500, and page size is 4KB, then 4-level DB can store 500^4*4KB=256TB data.
- While split a page, if DB crashes, to avoid leaving orphan pages, use a write-ahead log
- write-ahead log (WAL): every B-tree modification first writes to it, so it can be used to restore. **[KEY]**
- Copy-on-write scheme: not overwrite existing pages but write a new version. Another approach to restore from crash. **[KEY]**
- latches: a light weight lock to make B-Tree thread safe. **[KEY]**
- store abbrivate keys that are enough to present a boundary. It can pack more keys into a page, so the level can be reduced. **[KEY]**
- move pages with seq keys together on disk, so range search reads quicker **[KEY]**
- create pointers to sibling pages, so scanning keys don't need to go back to parent page again and again **[KEY]**
- fractal trees borrow some ideas from LSM trees.

Comparing B-Trees and LSM-Trees

- In general: LSM tree fast for write, B-Tree fast for read. But need to test for the real workload since different data might have different characterics. **[KEY]**
- write amplification: 1 DB write becomes multiple writes. The write bandwidth is limited so more writes reduce the throughput of a DB.
- B-tree update: 1 write to the write ahead log, 1 write for a page update (2 if split)
- LSM tree update: multiple writes but not done during the DB update: repeated compaction, merge of SSTables. A concern for SSD, which can overwrite blocks a limit of times.
- LSM has less write amplification. It also does seqential writes, which works better on magetic disk which handles seq writes better than random writes.
- SSD internally uses LSM to convert random writes to seq writes, so the benefit is not so obvious.
- LSM use less space. B-tree needs to maintain fragmentation.
- LSM compaction can impact ongoing R/W performance causing high percentile response time long. B-tree is more perdictable.
- So for LSM, needs to config the compaction and monitor it. **[KEY]**
- SSTable based DB don't throttle writes.
- Index for B-Trees exist in one place in the index. Transaction can be supported well. LSM has the indexes for same keys appear in multiple place. **[KEY]**

Secondary index: **[KEY]**

- they are important for join operation performance
- index keys are not necessary uniq.
  - Either make the values contain the list of matching entry ids
  - Or make each entry uniq by appending row ids to the key

Value store with the index:

- either store the index, or store the reference.
- when store ref, actual rows are stored in heap file, without order **[KEY]**
- heap works well with secondary indexes because row positions are not change
- if an update changes the value to larger than old value, then it needs to be moved. Either update all the indexes, or leave a forwarding point at the old place
- clustered index **[KEY]**: Store the indexed row within an index. e.g., InnoDB. secondary keys refer to primary key instead of heap.
- covering index **[KEY]**: store some columns within the index.
- clustered and covering indexs require more data usage, more writes, and transactional guarantees complicate.

**HERE**

Multi-column indexes

- concatenated index: index defination defines the order to append indexes. Can only search by the index appears earlier in the concatended index.
- multi-dimensional index:
  - important for geospatial data. R-tree is one spatial index.
  - HyperDex is an example of 2D index.
- Full-text search and fuzzy indexes:
  - Lucene is able to search text for words within a certain edit distance. It use a SSTable-like structure. the in-mem index is a finate state automaton over the chars in the key, like a trie. It can be transform into Levenshtein automaton for search
  - Document classification
  - machine learning

In-memory databases

- RAM is getting cheaper.
- Memcached: for caching use only. So data don't need durable.
- Can also use battery powered RAM
- log changes to disk
- write periodic snapshots to disk
- replicate in-mem state to other machines
- Redis: write to disk async.
- encoding in-mem data structure to write to disk also needs overheads, so in-mem database is faster. Some data structure like priority queue is hard to serialized
- anti-caching approach: evicting least recently used data to disk when mem is not enough. But all the keys are still need to be fit in memory

Transaction processing

- Doesn't necessarily have ACID.
- A group of low latency read and writes as a unit
- vs. batch processing: runs periodically
- online transaction processing (OLTP)
- online analytic processing (OLAP)
- data analytics: scan over a huge number of records, only reading a few columns per record, and calculates aggregate statistics
- OLAP does bulk import or event stream
- data warehouse: a separate DB for doing OLAP

Data Warehousing

- contains a read-only copy of the data in all the various OLTP systems
- process: Extract–Transform–Load (ETL)
  1. Data is extracted either periodic or a continuous stream of updates
  2. transform to a different schema
  3. clean up
- indexing for OLTP doesn't work well for data warehousing
- SQL is commonly used for dataware house, because the data model is normally relational.
- have graphical data analysis tools to generate SQL queries, visualize results, explore the data by drilling down, slicing (filter) and dicing (select a set).
- hard to support transaction processing and analytics workloads in one system
- Microsoft SQL Server supports both, but internally use two different systems, with same interface
- Amazon RedShift: host data warehouse open-source software ParAccel
- Apache Hive, Spark SQL, Cloudera Impala, Facebook Presto, Apache Tajo, and Apache Drill, Google’s Dremel

Stars and Snowflakes: Schemas for Analytics

- star schema: aka, dimensional modeling.
  - fact table: each row is an event occurred at a time
  - dimension table: the tables that contain foreign keys of the fact table
  - have a table to store date so that holidays can be captured
  - snowflake schema: dimensions are further broken down to subdimensions, so it is more normalized but hard to work with.

Column-Oriented Storage

- fact table is usually big, while dimension table is usually small.
- fact tables normally have a lot of columns, but only use 4-5 at a time.
- row oriented storage engine (relational and no-sql DBs) loads the whole row even with index appears.
- column-oriented storage stores each column in a file

Column Compression

- each fact table column normally have quite a few repeative (for example 0), so compression has good effect.
- bitmap encoding: record how many 0, then how many 1, then how many 0 ...
- bitmaps can also be nested.

Warehouse bottleneck and solutions

- memory bandwidth to load the data is a bottleneck
- bandwidth between memory and CPU cache is a bottleneck
- Can use CPU feature single-instruction-multi-data (SIMD) **Q**: What does it mean "Developers of analytical databases also worry about efficiently using the bandwidth from main memory into the CPU cache, avoiding branch mispredictions and bubbles in the CPU instruction processing pipeline, and making use of single-instruction-multi-data (SIMD) instructions in modern CPUs"?
- CPU cycles can be used efficiently by load a chunk of compressed column and iterate it in a tight loop without any function calls.
- Compressed column makes more data fits in L1 cache.
- vectorized processing: operations AND, OR in bitmap compression can be operate on compressed data directly in L1 cache.

Sort Order in Column Storage

- when move a value in a column, needs to move the whole row.
- Can specify the first column, and then the second column as sorted keys.
- if a column is sparse, after sort, the compression becomes more efficient.
- Vertica (a commercial data warehouse) uses C-Store. Sort the keys in several different ways on different machines for the replication.

Writing to Column-Oriented Storage

- B-tree approach doesn't work. Update one row means update all the column files
- LSM tree approach works. In-mem store doesn't need to be column oriented. When write to disk, write column files.

Aggregation: Data Cubes and Materialized Views

- materialized aggregates: speed up `COUNT`, `SUM`, `AVG`, `MIN`, `MAX` by using a materialized view.
- Not like the virtual view in relational data model (a table where contents are getting from queries), the materialized view actually copies the result to disk.
- when DB inserts values, the write is more expensive. So it is only used in read-heavy data warehouses.
- Data cube: a grid of aggregates grouped by different dimensions.

HERE: <https://learning.oreilly.com/library/view/designing-data-intensive-applications/9781491903063/ch03.html>

Summary
