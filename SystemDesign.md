# System Design Course
http://www.jiuzhang.com/course/2/

## Chapter 1
SRE: 运维

### Normal needs
Design System:
- Twitter: post tweet, follow/unfollow, timeline/news feed
- Facebook: <b>?</b>
- Instagram: <b>?</b>
- Friend Circle: <b>?</b>
- Google Reader(RSS Reader): <b>?</b>
- Uber: <b>?</b>
- Whatsapp: <b>?</b>
- Yelp: <b>?</b>
- Design Tiny URL: <b>?</b>
- Design NoSQL: <b>?</b>

Trouble Shooting:
- What happened if we can not access a website: <b>?</b>
- What happened if a webserver is too slow: <b>?</b>
- What should we do for increasing traffic: <b>?</b>

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

https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/MiniTwitter.cpp

Data:
- QPS(quest/s): need gather first. Can be estimated from DAU and interfaces. Read QPS and Write QPS can affect design tradeoff.
- DAU(daily active users), MAU is different from DAU * 30. [Find user habit from data](http://wechatinchina.com/thread-28820-1-1.html)
- Concurrent User: DAU * (daily avg quest per user) / (24 * 3600 = 86400 ~= 100k) . Daily Peak: Concurrent User * 3(experience number, pick from 2 to 9). Fast growing production, peak increase 2 times every 3 months.

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

Split design by objects. E.g. Twetter <b>Draw a Graph?</b>
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


##### Pull Model:
- Friendship table
- Tweet table

Get news feed
1. Query following users
2. Query recent 100 tweets post by those users
3. sort those tweets by time and return recent 100

Time complexity: if N following users
- news feed: O(N) DB reads and O(NlogN) merge K sorted arrays time(which can be neglect). Users wait while this is procceeding.
- post a tweet: O(1) DB write


##### Push Model:
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

DB Optimize: [Sharding](https://en.wikipedia.org/wiki/Shard_(database_architecture)

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
### Chapter 1
Design System:
- Twitter: post tweet, follow/unfollow, timeline/news feed
- Facebook: <b>?</b>
- Instagram: <b>?</b>
- Friend Circle: <b>?</b>
- Google Reader(RSS Reader): <b>?</b>
- Uber: <b>?</b>
- Whatsapp: <b>?</b>
- Yelp: <b>?</b>
- Design Tiny URL: <b>?</b>
- Design NoSQL: <b>?</b>

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
Cookie and session

- [Memcached](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/Memcache.cpp): in ram. Support read >>> write. Cache aside.
- Redis: in ram, keep data on disk. Cache through.
- [Cassendra](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/MiniCassandra.cpp): on disk. Raw, column key.

- Cache aside: DB, cache not communicate
- Cache through: first cache, then DB

SQL benefit: Transaction, [Serialization](https://github.com/ZhenyingZhu/CppAlgorithms/blob/master/src/lintcode/TrieSerialization.cpp), Secondary Index ...

# P34

_Sharding_

_Replica_

_Consistent Hashing_

