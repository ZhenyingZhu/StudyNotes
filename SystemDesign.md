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



#### Scale
DB Optimize: [Sharding](https://en.wikipedia.org/wiki/Shard_(database_architecture)

Special way to deal with Special Case

