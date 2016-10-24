# System Design Course
http://www.jiuzhang.com/course/2/

## Chapter 1
SRE: 运维

### Normal needs
Design System:
- Twitter: post tweet, follow/unfollow, timeline/news feed
- Facebook: <b>?</b>
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

Data:
- QPS(quest/s): need gather first. Can be estimated from DAU and interfaces. Read QPS and Write QPS can affect design tradeoff.
- DAU(daily active users), MAU is different from DAU * 30. [Find user habit from data](http://wechatinchina.com/thread-28820-1-1.html)
- Concurrent User: DAU * (avg quest per user) / (2400 * 3600) . Daily Peak: Concurrent User * 3(experience number, pick from 2 to 9). Fast growing production, peak increase 2 times every 3 months.

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

[Denormalization](https://en.wikipedia.org/wiki/Denormalization): adding redundant copies of data or by grouping data

[Asynchronous](https://en.wikipedia.org/wiki/Asynchrony_(computer_programming)): not block user quest.

While system design, don't ambivalent. Change plan is very expansive. Need always consider tradeoff. The design should be able to , or can be extend deal with special case.



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
DB Optimize: [Sharding](https://en.wikipedia.org/wiki/Shard_(database_architecture)

Special way to deal with Special Case

