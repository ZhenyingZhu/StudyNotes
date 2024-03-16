# System Design

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
2. 7/2/2023

- databases: multiple apps can read and write
- caches
- search indexes
- stream processing: another process handle message async
- batch processing: periodically crunch large amount of data.

Common services:

- Redis: datastores that are also used as message queues. **[KEY]**
- Apache Kafka: message queues with database-like durability guarantees. **[KEY]**
- Memcached: application-managed caching layer.
- Elasticsearch/Solr: full-text search server.

3 concerns **[KEY]**:

- Reliability
- Scalability
- Maintainability

Netflix Chaos Monkey:  trigger fault deliberately.

##### Reliability

Common faults:

MTTF: mean time to fail

- Hardware: hard disk, RAM, power, network
  - add redundency: RAID, dual power supplies, hot swappable CPU, backup generators. Good for single machine. Downtime could be long.
  - software fault tolerance: for system prioritize flexibility and elasticity over single-machine reliability. No downtime for the whole system.
- Software: bug, too much resource consumption, dependency failure, cascading failures (a fault triggers another fault) **[KEY]**
  - check the assumptions are still true **[KEY]**
  - process isolation **[KEY]**
  - watchdog **[KEY]**
- Human error
  - well designed API, UI **[KEY]**
  - sandbox **[KEY]**
  - auto tests: UT, intergration test, manual test **[KEY]**
  - easy recovery: rollback, gradually rollout, data integrity check. **[KEY]**
  - detailed and clear monitor/telemetry: performance metrics and error rates. **[KEY]**
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
- elastic **[KEY]**: automatically add computing resources when they detect a load increase
- distribute stateless system is easy, but stateful data system could be hard.
- Early-stage should iterate quickly on product features than it is to scale to some hypothetical future load.

##### Maintainability

- Operability
  - monitor health, visible to the runtime behavior and internal of the system. restore service
  - track down system failures or degraded performance
  - keep software and platform up to date
  - keep check how different services affect each other, to avoid one service completely break another one
  - anticipate future problem and solve them (e.g., cap planning) **[KEY]**
  - establish good practice and tools for deploy, config
  - perform maintenance tasks, e.g., migrate platform
  - maintain security
  - define process to make ops predictable **[KEY]**
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
2. 7/6/2023

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
- Object-relational mapping (ORM) frameworks **[KEY]**, e.g., ActiveRecord and Hibernate
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
- use document reference (similar to foreign key) to represent many-to-one and many-to-many **[KEY]**
- reference identifier is resolved at read time, using join or follow up queries

Relational vs. Document databases:

- fault tolerance
- concurrency handling
- schema flexibility
- performance

Doc model limitation: **[KEY]**

- cannot directly refer to a nested document. Need to first find its parent node. But unless it is deeply nested, it normally don't cause an issue.
- join is not good supported. If really needed, would need to denormalize data consistent. Graph model is more nature for such case

XML supports in relational database comes with optional schema validation.

document databases are schema-in-read: the structure of the data is implicit, and only interpreted when the data is read.

When need to update the schema:

- Document database: start writing new data and let application deal with both old and new data **[KEY]**
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
  - use two relational DB tables to store vertex and edge. **[KEY]**
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

Review

1. 7/16/2023

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
- index keys are not necessary uniq. The secondary index can be stored as key-value pairs:
  - Either make the values contain the list of all the matching entry ids
  - Or make each entry uniq by appending row ids to the key

Value store with the index:

- either store the index, or store the reference.
- when store ref, actual rows are stored in heap file, without order **[KEY]**
- heap works well with secondary indexes because row positions are not change
- if an update changes the value to larger than old value, then it needs to be moved. Either update all the indexes, or leave a forwarding point at the old place
- clustered index **[KEY]**: Store the indexed row ref within an index. e.g., InnoDB. secondary keys refer to primary key instead of heap.
- covering index **[KEY]**: store some columns within the index.
- clustered and covering indexs require more data usage, more writes, and transactional guarantees complicate.

Multi-column indexes

- concatenated index **[KEY]**: index defination defines the order to append indexes. Can only search by the index appears earlier in the concatended index.
- multi-dimensional index:
  - important for geospatial data. R-tree **[KEY]** is one spatial index.
  - HyperDex is an example of 2D index.
- Full-text search and fuzzy indexes:
  - Lucene is able to search text for words within a certain edit distance. It use a SSTable-like structure. the in-mem index is a finate state automaton over the chars in the key, like a trie. It can be transform into Levenshtein automaton for search. **[KEY]**
  - Document classification
  - machine learning

In-memory databases

- RAM is getting cheaper.
- Memcached **[KEY]**: for caching use only. So data don't need durable.
- Can also use battery powered RAM
- to achieve data durability **[KEY]**, write logs of the changes to disk, but merely not use the logs on disk
- Another approach is to periodically write snapshots to disk. **[KEY]**
- replicate in-mem state to other machines
- Redis: write to disk async. **[KEY]**
- encoding in-mem data structure to write to disk also needs overheads, so in-mem database is faster. Some data structure like priority queue is hard to serialized **[KEY]**
- anti-caching approach: evicting least recently used data to disk when mem is not enough. But all the keys are still need to be fit in memory **[KEY]**

Transaction processing

- Doesn't necessarily have ACID. **[KEY]**
- A group of low latency read and writes as a unit
- vs. batch processing: runs periodically **[KEY]**
- online transaction processing (OLTP) **[KEY]**
- online analytic processing (OLAP)
- data analytics: scan over a huge number of records, only reading a few columns per record, and calculates aggregate statistics
- OLAP does bulk import or event stream
- data warehouse: a separate DB for doing OLAP

Data Warehousing

- contains a read-only copy of the data in all the various OLTP systems
- process: Extract–Transform–Load (ETL) **[KEY]**
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
  - dimension table **[KEY]**: the table that contain the possible values for a colume in the fact table. The fact table uses foreign keys to the fact tables.
  - have a table to store date so that holidays can be captured
  - snowflake schema: dimensions are further broken down to subdimensions, so it is more normalized but hard to work with.

Column-Oriented Storage

- fact table is usually big, while dimension table is usually small.
- fact tables normally have a lot of columns, but only use 4-5 at a time.
- row oriented storage engine (relational and no-sql DBs) loads the whole row even with index appears.
- column-oriented storage stores each column in a file

Column Compression

- each fact table column normally have quite a few repeative (for example 0), so compression has good effect.
- bitmap encoding **[KEY]**: record how many 0, then how many 1, then how many 0 ...
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

#### Chapter 4. Encoding and Evolution

Review

1. 7/23/2023

Client side app change: need to count in that users might not upgrade any time soon.

- Backward compatibility: newer code read data written by older code.
- Forward compatibility: Older code can read data that was written by newer code.
- encoding format should be able to handle the compatibility.
- Representational State Transfer (REST)
- remote procedure calls (RPC)
- message-passing systems: actors and message queues.

Formats for Encoding Data

- In mem, data structure stored as pointer. Optimized to be read and manipulate by CPU
- write to file, or send through network, need to encode it to self-contained sequence bytes

Language-Specific Formats

- Lang build in serialize support
- hard to be used by another lang.
- Security concern if the bytes are captured by an attacker
- versioning could be hard, so compatibility is not maintained.
- efficiency might be poor.

JSON, XML, and Binary Variants

- XML: too verbose and complicate
- CSV is less powerful
- JSON can distinct number from string, but not integer vs. float. Since it is come from JavaScript, JS has inaccurate issue with large int because it uses floating-point numbers. **[KEY]**
- XML, JSON support unicode string, but not binary string (without char encoding). Need to use Base64 to encode the binary data first, which increase the size by 33%. **[KEY]**
- XML, JSON have opt schema support. XML schema is widely adopted, but not JSON. CSV doesn't support that. Making a change would be hard, and also might face issues when value has a delimeter char. **[KEY]**
- XML, JSON and CSV are widely used because different orgs agree on them. It is not easy.

Binary encoding: used within an org

- for large data set, need to use a format that is more compact or faster to parse
- JSON in binary: MessagePack, BSON, BJSON, BISON, etc. Some supports datatypes.
- Since not using schema, field names are also stored in the data.
- Need a byte before each field/value to tell what is the type and the length.

Thrift and Protocol Buffers: binary encoding libraries. require a schema. In the data, refer the fields as tags not real names. **[KEY]**

- When changing the schema,
  - the field names can be easily changed, but not the field tags. When adding a new field, it cannot be a required field. Old code just ignore it. When remove a tag, it must be optional because old code still requires it. The tag cannot be reused.
  - changing the data type is doable, but might loss precision or values get truncated by old code.
  - changing a single-value property to multi-value property: old code reads the last one. **[KEY]**

Avro: used in Hadoop. Also use a schema. When encoding, don't record which field but only value type and the order. **[KEY]**

- decode uses reader schema, which might be old. The writer schema should be compatible with old reader schema. Avro resolve the difference with both schema side by side. **[KEY]**
- Can only add or remove a field that has default value.
- null must be used only by field with value type `union { null, <value type> }`.
- How the writer schema gets to the decoder **[KEY]**
  - For transfering large file with lot of records: encode the writer schema in beginning the file
  - For database that writes each record once at a time, might use different write schema: store an encoding version, and store the corresponding schemas in DB.
  - Keep sending records over network: negotiate the schema version on connection setup.
- Dynamically generated schemas: can generate the schema from a relational DB's schema, so the data can be convert to an Avro object and send over the network.
  - If the DB schema changed, Avro can easily adopt the change.

Code generation and dynamically typed languages

- Thrift and Protocol Buffers can auto generate code in staticically typed lang to decode data.
- Avro doesn't need code generation. Avro reads the data file as a JSON file and connect it to the writer schema. Dynamically typed data processing lang like Apache Pig suits it because Pig doesn't care about the schema.

The Merits of Schemas

- schema for binary encoding is used by ASN.1, which is used to encode SSL cert (X.509) with its binary encoding (DER)
- relational DBs have a network protocol to transfer data. Drivers like ODBC APIs can decode the binary data into in-mem data structure.

Modes of Dataflow: when encode data, who decodes it?

- Via DBs
- Via service calls
- Via async message passing

Dataflow Through Databases

- The data is stored and read later
- The services might using newer code on some machines, and old code on other machines. So fore and backward compatibility both needed.
- An edge case: A new field is added. A newer record writes the field, then an older code updates this record without this field. The field needs to be retained. So the encoding of the old code should let unknown fields untouched. **[KEY]**
- migrating data into new schema is expensive for large data set, so new fields should have default values.
- While dumping the data, use the latest schema to backfill old data.

Dataflow Through Services: REST and RPC

- Expect data to be transferred ASAP
- HTTP can be used to transfer HTML, CSS, JS, image, JSON, etc. by `GET`
- AJAX: a client-side JavaScript application running in Web browser can use XMLHttpRequest to become an HTTP client **[KEY]**
- service-oriented architecture (SOA)/microservices architecture: a large app is decomposed to smaller services by area of functionality. So each service could be both the server and client for some other services at the same time. **[KEY]**
- services can impose fine-grained restrictions on what clients can and cannot do by defining the API.
- services independently deployable and evolvable so easy to make changes
- middleware: a service running inside an org to communicate between services. Traffic not go through internet.
- Services in different orgs need to communicate through internet with public APIs. Like OAuth that sharing access to user data, credit card processing system. **[KEY]**
- Web services commonly use either REST or SOAP.
  - REST: a design philosophy build upon HTTP principals. It emphasizes simple data formats, using URLs for identifying resources and using HTTP features for cache control, authentication, and content type negotiation. An API designed according to the principles of REST is called RESTful. **[KEY]**
  - Swagger, aka. OpenAPI, can be used to describe APIs and generate docs. **[KEY]**
  - SOAP is an XML-based protocol for making network API requests. It aims to be independent from HTTP. It uses Web Services Description Language(WSDL) to define APIs, and can auto generate codes. It relies on tools and IDEs so not friendly for simple apps. **[KEY]**

The problems with remote procedure calls (RPCs)

- Enterprise JavaBeans (EJB) and Java’s Remote Method Invocation (RMI) are limited to Java. The Distributed Component Object Model (DCOM) is limited to Microsoft platforms. **[KEY]**
- RPC model tries to make a request to a remote network service look the same as calling a function or method in your programming language, within the same process (this abstraction is called location transparency). **[KEY]**
- But not like local call, not all the parameters, like network, are predictable.
- local call either return a result or throw exception, or not return in an inf loop. Network call can return without a result, for example timeout.
- When retry a failed network call, the previous call could already go through. Need to maintain idempotence.
- the duration for a same network call may vary from time to time
- network call cannot use data structure, so big objects could be problematic.
- client and server might use different lang.

Current directions for RPC

- use futures (promises) to encapsulate asynchronous actions that may fail. **[KEY]**
- Futures also simplify situations where you need to make requests to multiple services in parallel, and combine their results. **[KEY]**
- gRPC supports streams, a series of requests and reponses over time.
- service discovery: client to find which IP and which port for a service **[KEY]**
- a RESTful API is good for experimentation and debugging because it is widely supported by standard tools, langs, platforms. So use REST for public APIs, while RPC for internal data transfer. **[KEY]**

Data encoding and evolution for RPC

- Can assumpt that all servers update first, then clients. So only backward compatibility is needed
- RPC can be used across org boundaries so cannot force clients to upgrade. So need to maintain compatibility for longer time. If need to make a breaking change, need to maintain different API versions side by side.
- For RESTful APIs, append the API version to the URL or in the HTTP `Accept` Header. **[KEY]**
- For services that use API keys to identify a particular client, another option is to store a client’s requested API version on the server and to allow this version selection to be updated through a separate administrative interface **[KEY]**

Message-Passing Dataflow

- data is transferred async
- message: a client request **[KEY]**
- message broker: aka. message queue or message-oriented middleware. message transferred through it and stored temporarily **[KEY]**
- work as a buffer. improve system reliability **[KEY]**
- It can automatically redeliver messages to a process that has crashed, and thus prevent messages from being lost. **[KEY]**
- sender doesn't need to know the IP and the port of the receiver. In cloud it is useful. **[KEY]**
- one message can be sent to multiple recipient. **[KEY]**
- sender doesn't need to know who is the receiver **[KEY]**
- But sender usually doesn't expect to wait and receive response in message dataflow. It is one-way. **[KEY]**

Message brokers

- RabbitMQ, Apache Kafka **[KEY]**
- Terms: producer - queue - consumer. producer - topic - subscriber. **[KEY]**
- one consumer might public to another queue as a producer. **[KEY]**
- to broker, message is just bytes, so any encoding works

Distributed actor frameworks

- actor model: a programming model for concurrency in a single process. Each actor represents one client, have some local state and communicates with other actors by sending and receiving asynchronous messages. Since each actor processes only one message at a time, it doesn’t need to worry about threads. **[KEY]**
- Message delivery is not guaranteed
- used to scale an application across multiple nodes. No matter whether the clients are on the same node or not, use MQ to communicate.

### Part 2. Distributed Data

reasons to distribute a database across multiple machines

- Scalability: volume, R/W load
- Fault tolerance/high availability: redundancy
- Latency: geo redundancy

vertical scale up (better performance machine using shared-memory architecture)

- cost grow faster than linearly. twice the cost cannot handle twice load
- limited fault tolerance

horizontal scaling (shared-nothing architectures)

- node: a virtual machine
- coordination between nodes are done in software level, using conventional network
- incurs additional complexity for applications and sometimes limits the expressiveness of the data models **[KEY]**

Replication Versus Partitioning

- Replication: copy of same data. provides redundancy
- Partitioning: sharding
- can be used together

#### Chapter 5. Replication

Review

1. 8/7/2023

The difficulty of replication: handling changes to replicated data. three popular algorithms:

- single-leader
- multi-leader
- leaderless replication

trade-offs to consider:

- use synchronous or asynchronous replication
- handle failed replicas

eventual consistency issues:

- read-your-writes
- monotonic reads guarantees **[KEY]**

leader-based replication(active/passive or master–slave replication) **[KEY]**

- writes send to leader
- followers (hot standby) get changes from leader via replication log or change stream
- reads can be done from leader or followers

Synchronous Versus Asynchronous Replication

- one follower needs to get the change sync, others could do it async **[KEY]**
- no guarantee how long can replication take
- sync replication failure on one node could cause the whole system halted
- semi-synchronous: to solve this issue, an async replica becomes sync one when the sync one is not available **[KEY]**
- leader-based replication is often configured to be completely asynchronous.
- alternative to avoid leader failure cause the write data loss: chain replication

Setting Up New Followers **[KEY]**

- cannot lock the DB because the requirement for high availability
- Take a consistent snapshot of the leader’s database
- Restore the snapshot on the follower
- Follower requests changes after the snapshot from leader.
- Each snapshot should associate with a position. It can be called as log sequence number, binlog coordinates.
- follower is caught up

Handling Node Outages **[KEY]**

- Follower failure: Catch-up recovery. Follower maintain a log of data changes it has received from leader.
- Leader failure: Failover. one of the followers needs to be promoted to be the new leader. Config client to send traffic to the new leader.
- Use a timeout to detect node failure. Planned mantainance shouldn't trigger alert.
- Use an election process to choose the new leader by the majority replicas, or appointed by the previous controller node. Should choose the most up-to-date replica
- Through consensus to let all nodes agree on it
- Let client send traffic to the new leader. The old leader might still think it is the leader after it comes back so need to make it become a follower

faults can happen during failover **[KEY]**

- In async replication design, the new leader might miss some writes. To not cause write conflicts when the old leader comes back, those writes can be discarded, but will cause data loss
- For data needs to be coordinate outside the system, discard writes is dangeous
- split brain: two nodes both believe they are the leader. To avoid write conflicts, can use a mechanism to shut one down. But be careful not to shut both down
- How long should be the timeout? Longer means long fail duration. Shorter means might un-necessarily failover. Can make the failover manual

Implementation of Replication Logs

- Statement-based replication: forward the request statement to all the followers
  - But for non-deterministic statements like `NOW()` or `RAND()` won't write the same change on the followers
  - If multiple concurrently executing transations use auto-incrementing column, running them in different order leads to different results
  - statements have side effects, like triggers, stored procedures, user-defined functions, may result differently
  - Can solve those issues by the leader converts such statements to deterministic statements, but too many edge cases
- Write-ahead log (WAL) shipping **[KEY]**
  - log is an append-only sequence of bytes containing all writes to the database. Can be used to build a replica
  - disadvantage: the log is very low level, which byte changed in which disk blocks. Closely couple to the storage engine. If the engine needs to be upgrade, there would be a downtime to let leader and followers both upgrade at the same time
- Logical (row-based) log replication **[KEY]**
  - logical log: a sequence of records describing writes to database tables at the granularity of a row
  - each create/update/delete generates a log
  - a transaction modify multiple rows has a log for each row update, follow by a transaction commit log
  - change data capture: the logical log format is easy to be interpreted by external systems, so building custom indexes and caching would be easier
- Trigger-based replication
  - let application level not DB to control the replication, when need to replicate a special subset of data, replicate between different DBs, run conflict resolution logic
  - trigger: register code to execute when data change happens
  - has greater overhead, and error proning

Problems with Replication Lag

- replication suits for read-scaling architecture. But async replication cause reads might land on an out-of-date replica
- eventual consistency: when the replication lag is more than couple secs, could cause issues
- Reading Your Own Writes: can solve by read-after-write consistency. The user writes the data can read the data immediately, while other users need wait **[KEY]**
  - Approach 1: the writer reads from leader. The system needs to know something has been modified without querying. Suitable for DB that only a few fields can be modified by users
  - Approach 2: all reads in a duration after writes read from leader. Prevent reads to the followers that are this duration behind
  - Approach 3: client remembers the write seq number, and read from followers not behind it, or wait until the follower catch up.
  - In a multi-geo design, the requests need to be served by the leader needs to be routed to the leader's DC first
  - If the user uses multiple devices, the metadata to identify the user and the timestamp/seq number needs to be centerialized. Also need to route the requests to the leader's DC because user's devices might be in different geo.
- Monotonic Reads: to prevent when several reads occurs, later query return previous state **[KEY]**
  - a guarantee that the level is between strong consistency and eventually consistency
  - the same user always read from the same replica. Chosing the replica based on a hash of the user id (but need to consider if the replica failed, how to re-route the user)
- Consistent Prefix Reads: when multiple writes happen in a seq, prevents a reader seeing them out of order **[KEY]**
  - only happened in partitioned DBs. In the same partition the writes can be kept in order, but hard to coordinate in different partitions
  - Can make writes that order matters write to the same partition.
  - Another solution: use algorithms to keep track of causal dependencies
- Transaction is the DB provided solution to deal with replication lag **[KEY]**
  - Single-node transactions is a solution, but cannot be used in distributed system

Multi-Leader Replication

- Single leader cons: all writes go through the same leader
- multi-leader (active-active replication): each leader acts as others' follower

Use cases for multi-leader **[KEY]**

- multi-DC operation: each DC has a leader and some followers. Between leaders across DCs, there is a conflict resolver
  - performance is better from user point-of-view because cross DC replication is hidden from user
  - single DC outage won't affect other DCs
  - cross DC traffic normally goes through public internet. With multi-leader the dependency on public internet is reduced
  - cons: same data can be concurrently modified in multiple DCs. Need to resolve conflicts
  - it is often considerred as dangeous and need to be avoid
- Clients with offline operation: an app that needs to work even disconnect from internet.
  - write changes can be synced after next time connected to internet.
  - Every client has a local DB worked as leader. The replication lag could be days.
  - CouchDB is designed for this case
- Collaborative editing: need auto conflict resolution algorithm
  - edits made to local replica and then async replicated to server.
  - to achieve fast collaborate, need make the change unit very small and avoid locking

Handling Write Conflicts

- in single leader, the conflict write would be blocked or aborted.
- in multi leader, the conflict only detects during replication, and too late to ask the user to resolve
- conflict avoidance: require all the writes to a same record goes to a same leader **[KEY]**
  - when the DC is failing or the record partition is changed, the writes need to be re-route and can have write conflicts
- Converging toward a consistent state: all replicas must arrive to the same final state after all changes have been replicated
  - single leader, last write is the latest state.
  - multi leader, writes have no defined ordering. The database must resolve the conflict in a convergent way **[KEY]**
  - Can give each writes a UUID, and use the highest number as the final state. Can lead to data loss
  - Can give replica ids, the changes on the higher number replica wins. Can lead to data loss
  - merge the value together by concat them
  - use a data structure to record the conflict details and promote to the user to resolve the conflict in a later time
- Custom conflict resolution logic: DB provided features to hook the custom code
  - On write: when DB detects a conflict, run the conflict handler. Runs in the background and should be quick.
  - On read: store all the conflict writes. When read the data, present all the versions of the data to the user. Prompt the user to resolve it.
- Conflict resolution applies at the level of an individual row/document. In a transaction, each write needs to resolve conflicts separately **[KEY]**
  - Automatic Conflict Resolution: Conflict-free replicated datatypes (CRDTs), Mergeable persistent data structures, Operational transformation

Multi-Leader Replication Topologies **[KEY]**

- Circular topology: each leader sends writes to it (including writes from previous leaders) to its next leader
- Star topology: a leader acked as a centeral (root node). All other leaders first send writes to it, and then it sends writes to others. This topology can be generized to a tree.
- All-to-all topology: a mesh. Each leader sends writes to all other leaders.
- In Circular and Star topology, since changes are forward through multiple nodes, there could be infinite replication loop. To avoid that, each node has a uuid, and changes are tagged with the uuid. If a node receives changes with its own uuid, ignore the changes.
- In Circular and Star topology, a single node failure could halt the whole replication. Need manual operation to reconfig. All-to-all doesn't have this issue.
- All-to-all topology could have some nodes seeing changes in reversed order, because changes might be replicated through different paths to a node. Using timestamp to tag each write is not sufficient, because clock could be skew. Version vectors can solve the issue.

Leaderless Replication

- allow any replica to directly accept writes from clients. Used by Dynamo, Cassandra **[KEY]**
- Client either write changes to all the replicas, or a coordinator node does it on behalf of client
- does not enforce write orders
- Writing to the Database When a Node Is Down:
  - client writes changes to all 3 replicas in parallel. When it receives 2 ok, it treats the write as succeed, even more replicas are failing.
  - When read, reads from all 3 replicas. Use the version number to determine which is the most up-to-date writes.
  - To let once unavailable node catch up, there are 2 ways: **[KEY]**
    - Read repair: when the client detects a node is left behind, makes a writes to let it catch up. Works well when values are frequently read
    - Anti-entropy process: a background process constantly looks for differences between replicas and copy data around. Write orders are not guaranteed. Also could have a huge delay.
  - Quorums for reading and writing: if there are n replicas, every writes must be confirmed by w nodes to be considered as successful. Must query at least r nodes for each read. w + r >= n. Normally n is a odd number (3 or 5). w = r = (n + 1) / 2. When read, we can tolerate n - r nodes to be unavailable. When writes, n - w. **[KEY]**
  - Limitations of Quorum Consistency: **[KEY]**
    - to gain lower latency and higher availability, can let w + r < n, if client doesn't that cares about not reading stale data
    - even with w + r > n, if sloppy quorum is used (when quorum cannot maintain, writes to nodes that are not belongs to w and r nodes), there is no guaranteed overlap between read and write nodes.
    - concurrent writes may succeed on different nodes, but winner is chosen by timestamp. Before the conflict is resolved, writes can be lost due to clock skew.
    - if a write happens concurrently with a read, some nodes have the write while others don't.
    - if a write succeed on less than n - r nodes, it should be rolled back, but some nodes might not rolled back yet when read occurs
    - if a node carrying the new write fails, and it restored from an old replica, then less than n - r nodes have the latest value
    - can have timing issue ?? Linearizability and quorums
  - Dynamo-style databases are generally optimized for use cases that can tolerate eventual consistency. Normally don't guarantee reading your writes, monotonic reads, or consistent prefix reads.

Monitoring staleness

- even client can tolerate staleness, the service needs to aware of the replication health. **[KEY]**
- Leader based replication: can emit metrics for the replication lag. Substract the followers replication timestamp with the leaders write timestamp.
- Leaderless replication, if not use anti-entropy, the replication lag could be huge. No good pratice yet.

Sloppy Quorums and Hinted Handoff

- Leaderless replication has the benefits that when a node fails, no failover is needed.
- But during an internet interruption, if more than w or r nodes are not available, need to trade off:
  - return error that quorum cannot meet
  - sloppy quorum: accept write anyway on those reachable nodes, even if they are not among the normal w and r nodes **[KEY]**
- hinted handoff: after network interruption is fixed, the tempoary node writes the changes to the apporate node **[KEY]**
- But when read, the stale data might return as the write can landed in a node outside n, until hinted handoff is complete
- Multi-datacenter operation: Cassandra: n includes nodes in all DCs. When write, writes to all DCs, but client only waits for local DC quorum. **[KEY]**

Detecting Concurrent Writes

- Dynamo-style DB allows clients to concurrently write. Conflicts could arise during read repair and hinted handoff.
- Last write wins (LWW)/discarding concurrent writes: need to have a unambiguously determining which write is last. But for concurrent writes, the orders are undefined. LWW can lose writes. In cache it is fine. Otherwise need to make a key can only be write once. Cassandra recommend to use an UUID for each write. **[KEY]**
- The "happens-before" relationship and concurrency: one write causally dependent on another, means the write is based on the others' result. If two writes don't know each other, and neither happens before the other, then they are concurrent. It doesn't necessary mean two writes are happened with time overlapping. **[KEY]**
- Capturing the happens-before relationship: **[KEY]**
  - server maintains a version number for each write to a key, with the written value
  - when client reads a key, return all the values that have not been overwritten (a version is overwritten when the same client sends a new write)
  - a client must first read a key before writes to it
  - when write, the client needs to include the version number of the previous read, with all the values merged
  - when server receive a write with a version, it overwrites the key with all the values from this version or below, and bump up the version
  - if the write doesn't include a version, the server assign a new version and don't overwrite any values, as this is the first write from a client
- Merging concurrently written values: **[KEY]**
  - concurrent values are siblings
  - When a value is deleted, need to mark it as deleted with a version number. It is called tombstone.
  - CRDTs is a data structure to support merge concurrent writes
- Version vectors: **[KEY]**
  - a version number per replica per key
  - The versions collection from all replicas are the version vector. dotted version vector is the most common one
  - client can read from one replica and write to another with the version vector, and don't need to worry about data loss

Replication purpose:

- High availability: when some nodes down
- Disconnected operation: during a network interruption
- Latency: different Geo
- Scalability

#### Chapter 6. Partitioning

Review

1. 8/13/2023

each partition is a small database of its own, although the database may support operations that touch multiple partitions at the same time.

Different partitions can be placed on different nodes in a shared-nothing (node and node are all standalone) cluster.

Each node can execute queries for its partition. By adding more nodes, the throughput can be scaled.

Partition and replication **[KEY]**

- A node may store more than one partition.
- Each node can have a leader of a partition while followers for other partitions
- Repliaction scheme can be chosen independently with partition scheme

Partitioning of Key-Value Data

- goal with partitioning is to spread the data and the query load evenly across nodes
- skewed: some partitions have more data or queries than others **[KEY]**
- hot spot: A partition with disproportionately high load
- Partitioning by Key Range **[KEY]**:
  - The partition boundaries can be chosen by DB automatically
  - Within each partition, we can keep keys in sorted order. Makes binary search and range scan easier
  - certain access patterns can lead to hot spots
- Partitioning by Hash of Key: makes keys uniformly distributed across partitions **[KEY]**
  - the hash function need not be cryptographically strong. MongoDB uses MD5
  - but hash value needs to be consistent. Java hash has different values on different processors
  - consistent hashing: partition boundaries can be chosen pseudorandomly. It in theory can be worked for rebalance, but in pratice not work well.
  - But lose the ability to do range scan
  - Cassandra can declare compound primary key. First part is hashed, to determine the partition. Other columns are used for sorting data in SSTables.

Skewed Workloads and Relieving Hot Spots

- celebrity activity could still cause hot spot even use hashing partition
- can append a 2-digit random number to a hot key. But it requires read to retrieve all those new keys. Also need a bookkeeping tech to track such special hot keys **[KEY]**

Partitioning and Secondary Indexes

- NoSQL doesn't support secondary index to avoid complexity, but it is useful so Elasticsearch is developed **[KEY]**
- the complexity is that secondary index cannot be neatly map to partitions. Two main approaches are **[KEY]**
  - Partitioning Secondary Indexes by Document: let DB create another table to track the seconary index to the entries' primary keys within the partition. It is also called local index. When search, need combine results from all partitions. It is called scatter/gather. It is expensive and is prone to tail latency amplification
  - Partitioning Secondary Indexes by Term: create a global index that is partitioned by the term, which is different from the partition of the primary key. It is called term-partitioned. Term can be partitioned by its value to support range scan, or by hash to evenly distributed the load.
    - The complexity is added to write. It requires a distributed transaction to update all partitions, which is not supported by DBs. So the global index is async

Rebalancing Partitions **[KEY]**

- After rebalancing, the load (data storage, read and write requests) should be shared fairly between the nodes in the cluster
- While rebalancing is happening, the database should continue accepting reads and writes
- No more data than necessary should be moved between nodes, to make rebalancing fast and to minimize the network and disk I/O load

Strategies for Rebalancing

- don't use hash mod N: when N changes, too much data needs to move
- Fixed number of partitions: more partitions than nodes. When a node comes up, only few partitions need to move. **[KEY]**
  - entire partition is moved.
  - during the move, the old node still have the data, so read is still working. only after the partition is fully moved, the partition assignment change can be done
  - for mismatched hardwares, stronger nodes can take more partitions
  - can support split and merge partitions, but normally they are not supported. The total partition count remains same
  - too many partitions could cause the management overhead also high
  - partition size is hard to define when dataset is too variable

Dynamic partitioning

- with the fixed partition boundaries, reconfig manually would be tedious
- dynamically create partitions: when a partition exceed certain size, plit into two. If a partition shrinks below certain threshold, merged it with adjacent partition **[KEY]**
- when a big partition is splitted into two, one can be moved to another node to balance the load **[KEY]**
- reduce the overhead of managing too many partitions
- one caveat: before the first partition gets splitted, all writes go to the same node. Pre-splitting: allows an init set of partitions on an empty DB. **[KEY]**

Partitioning proportionally to nodes

- make the number of partitions proportional to the number of nodes. Each node contains certain number of partitions **[KEY]**
- when increase nodes, partition size become smaller. Size of each partition is stable
- when adding a new node, randomly choose fixed number of partitions to split, and move half of them to the new node. Even it is unfair split, with 256 partitions on a node, the load is distributed evenly. **[KEY]**
  - This approach is close to Consistent hashing.
  - it requires to use Hash-base parititioning, so the boundaries can be picked up from hash-generated numbers **[KEY]**

Operations: Automatic or Manual Rebalancing

- Fully automated rebalancing can be unpredictable. Because rebalance is an expensive operation due to need reroute requests and move data. Alone with automatic failure detection, which might unnecessarily mark a slow node as unhealthy and move data out, causing a cascading failure.
- good to have human in the rebalancing loop

Request Routing (service discovery) **[KEY]**

- one approach: allow clients to talk to any node via a round-robin LB. then the node forward the request to the appropriate node
- another: send requests to a routing tier first. The tier acts as a partition-aware LB
- another: let clients aware of the partitioning and the node assignment
- the challenge: how to know the assignment changes of nodes. All participants need to agree, i.e., achieving consensus following some protocols
- ZooKeeper: a seperate coordination service used by distributed data system. It keeps tracking cluster metadata
- Cassandra uses a gossip protocol among nodes so that each node knows the partition assignment. It adds complexity to nodes but remove the dependency on an external service
- find the ip of the nodes are through DNS as it is less freq changes

Parallel Query Execution

- massively parallel processing (MPP) used by analytics breaks complex queries into a number of execution stages and partitions, which can be run in parallel
- scaning over large dataset can be benefit

#### Chapter 7. Transactions

Review

1. 9/3/2023

harsh reality of data systems **[KEY]**

- DB or hardware failures in the middle of a write
- application crashs during a series of operations
- network interruption between app and DB, or DB nodes
- clients can overwrite each others
- client gets partial result
- multiple clients cause race condition

Transaction simplify these issues

- all the operations either commit, or abort and rollback
- app can safely retry, ignore certain potential error scenarios and concurrency issues, because DB provides safety guarantees
- sometimes there are advantages to weakening transactional guarantees or abandoning them entirely, to achieve high performance and availbility
- databases isolation levels: read committed, snapshot isolation, and serializability

The Slippery Concept of a Transaction

- NoSQL use new data models than relational DB, and include replication and partition by default, causing support of transaction lost or weaken

The Meaning of ACID **[KEY]**

- Atomicity, Consistency, Isolation, and Durability.
- BASE: Basically Available, Soft state, and Eventual consistency. not meet ACID criteria
- Atomicity: a fault occurs during several writes, DB needs to undo writes already made in the transaction
- Consistency: certain statements about data must always be true. The data should be valid after the writes
- Isolation: concurrently executing transactions are isolated from each other. One solution by introduce the serializability of the transactions have performance penalty. So a weaker guarantee snapshot isolation is used.
- Durability: the promise that once a transaction has committed successfully, the data won't lose even if there is a hardware fault or the database crashes. Use write-ahead logs for recovery. Use replication.

Single-Object and Multi-Object Operations

- multi-object transactions are needed if several pieces of data need to be kept in sync
- which read and write operations belong to the same transaction: based on the client’s TCP connection to the database, between a BEGIN TRANSACTION and a COMMIT statement.
- NonSQL multi-put operation doesn't necessarily mean it is atomic

Single-object writes

- Atomicity and isolation also apply when a single object is being changed, because the write might take some time
- Atomicity can be implemented using a log for crash recovery
- isolation can be implemented using a lock on each object
- more complex atomic operations: increment operation that can removes the need for a read-modify-write cycle

The need for multi-object transactions

- it is challenging in distributed DB, because it needs across partitions, and could affect performance and high availbility
- transaction is useful for foreign reference **[KEY]**
- in document data model, fields need to be update together should be within the same document, so it can be treated as single object write **[KEY]**
- for denormalization, several documents might need to be updated for a single change, so transaction is useful **[KEY]**
- for secondary index as well **[KEY]**
- if not use transaction, error handling for those cases would be hard

Handling errors and aborts

- ACID philosophy: if the database is in danger of violating ACID, rather abandon the transaction entirely than allow it to remain half-finished
- leaderless replication DB works on best-effort, so the philosophy doesn't apply
- retrying aborted transaction can also have issues **[KEY]**
  - if transaction succeed but network error occurs, then retry could cause dup
  - if the error is caused by overload, retrying cause it worse. Need to limit retry count, with exponential backoff, and not retry on overload issue
  - retry on transient error is fine, but doesn't help on permantent error
  - there could be side effect outside DB when retry
  - if client fails while retrying, then it could lost the data

Weak Isolation Levels

- happened when two transactions update the same data
- transaction isolation: isolate concurrency issues from application developers
- serializable isolation: transactions have the same effect when run one at a time

Read Committed

- most basic level of transaction isolation
- when read, only data that has been committed is read **[KEY]**
- when write, only overwrites committed data **[KEY]**
- dirty reads: another transaction sees uncommitted data
- 2 guarantees for no dirty reads: 1. before commit is done, no partial change is returned; 2. if a transaction is abort, all changes are rolled back **[KEY]**
- prevent dirty writes: delay the second transaction until the first is either committed or aborted **[KEY]**
- read commit doesn't prevent race condition **[KEY]** between two counter increments (two writes in transaction 1 happens before and after two writes in transaction 2, causing transaction 2 totally lost, because transaction 2 doesn't see transaction 1 as it was not committed)
- Implementing read committed: default in Oracle, PostgreSQL **[KEY]**
  - using row level locks (for an object) when write. Only one lock can be hold per object. If acquired, before transaction committed/aborted the lock is not released
  - to prevent dirty read, one approach is to acquire the same lock when read. But one long read could block writes
  - better approach is let the DB remember the value before uncommitted writes, and return that value

Snapshot Isolation and Repeatable Read

- read skew: **[KEY]** nonrepeatable read for two different data (some relation between them) sees inconsistant results, if there is a transaction ongoing, causing timing anomaly. It is mostly toleratable from client under read committed guarantee, because retry the reads can get the correct result
- not tolerate situation:
  - backup: the writes during the backup are inconsistent. If restore it, the inconsistency could be permanent.
  - Analytic queries and integrity checks: could return nonsensical results
- Snapshot isolation **[KEY]** can solve the issue: each transaction reads from a consistent snapshot (the committed data at the start of the transaction). Even the data is subsequently changed by another transaction, each transaction sees the old data.
- Implementing snapshot isolation **[KEY]**: use write lock. Read not require lock. readers never block writers. writers never block readers.
  - multi-version concurrency control (MVCC): DB potentially keep committed versions of an object.
  - when a transaction starts, a unique, always-increasing transaction ID (txid) is assigned.
  - each row has a createdBy field, records the txid that insert it, and a deletedBy field records when the entry is marked as deleted.
  - an update is translated to a created and deleted record

Visibility rules for observing a consistent snapshot **[KEY]**

- at the start of each transaction, DB lists all the ongoing transactions, and ignore their writes
- any writes made by aborted transactions are ignored
- any transactions with a later txid are ignored
- all other writes are visible to the app's queries
- never updating values in place but instead creating a new version every time a value is changed
- garbage collector removes old versions that are not visible to any transactions

Indexes and snapshot isolation

- simple solution: an index query to filter out any object versions that are not visible to the current transaction. Remove index entries after GCs remove those versions **[KEY]**
- to improve the performance for multi-version concurrency control, avoid index updates if all the versions of an object can be fit in one page. Used by PostgreSQL
- another approach: CouchDB uses an append only B-tree. every write transaction creates a new B-tree root, and each root presents a consistant snapshot. Need a background process to compact and GC. **[KEY]**

Repeatable read and naming confusion

- snapshot isloation is also called as serializable, repeatable read.

Preventing Lost Updates **[KEY]**

- two transactions writing concurrently: dirty write is one of the write-write conflict
- Lost update: two read-modify-write cycles happen concurrently, causing one writes lost
- happened in scenarios: 1. increase counter/account balance, 2. making a local change to a complex value (need parse-change-write), 3. edit wiki page

Atomic write operations

- DB provided function to avoid app needs to write the read-modify-write cycle. It is concurrently safe **[KEY]**
- for SQL `UPDATE counters SET value = value + 1 WHERE key = 'foo';`
- for MongoDB, supports atomic modify JSON; Redis for update data structure like priority queue
- but wiki would be hard to support atomic write. So atomic write is not supported in all the scenarios
- cursor stability: take an exclusive lock on the object when it is read, until the update is applied
- another option is to force single thread update
- But ORM (object-relational mapping) frameworks can accidently generate code that make read-modify-write cycle instead of using atomic operation. Without looking into the details it is a hard bug to detect

Explicit locking **[KEY]**

- In an transaction, `SELECT * FROM figures WHERE name = 'robot' AND game_id = 222 FOR UPDATE;` The `FOR UPDATE` puts a lock on all the rows returned by this query
- before the update, there could be other operations that the app does (like validating some rules), so need to put a lock explicitly

Automatically detecting lost updates

- other than atomic operations, another way is to force the read-modify-write cycles happen sequentially. if the transaction manager detects lost updates, abort the transaction and lets the client to retry
- the check can work efficiently with snapshot isolation
- it happens automatically so is better than explicit lock

Compare-and-set **[KEY]**

- atomic compare-and-set op are supported in some DBs that don't support transaction
- only allow writes to object that are not changed after read. Wiki can use this approach

Conflict resolution and replication **[KEY]**

- in replicated DB, the same data could be modified on different nodes. Lock or compare-and-set doesn't work here
- allow conflict versions for the data, and use app code to resolve and merge the conflicts
- if operations are commutative (excute in different orders can still get to the same result), DB can auto merge the changes to prevent lost update

Write Skew and Phantoms

- two transactions are updating two different objects, but one has to be run after another (for example in the transaction there is a check for the latest statuses of those objects), then the race condition could cause write skew **[KEY]**
- Automatically preventing write skew requires true serializable isolation
- DB can config constraints (uniq, foreign key, value range). Can also use trigger or materialized views to prevent write skew **[KEY]**
- explicitly lock rows the transaction depends on can help in some cases, but if the requirement needs check the absense of some rows (like a user name is not taken), then it cannot lock those non-exist rows
- pattern: a `SELECT` query check some requirements, then the app code decides whether to make a write. The write changes the pre-condition.
- phantom: a write in one transaction changes the result of a search query in another transaction **[KEY]**

Materializing conflicts

- artificially introduce locks for non-exist rows **[KEY]**
- but hard figure out what needs to lock, and also a leak in the data model, so can only be used as a last resort

Serializability **[KEY]**

- isolation levels: read committed, snapshot isolation. But they are hard to understand and different DBs have different implementation
- from app code, it is hard to tell which isolation level is enough
- no good tools to detect race conditions
- use serializable isolation can solve those issues
- 3 techniques that each can implement serializable isolation
  - executing transactions in serial order
  - two phase locking
  - Optimistic concurrency control techniques such as serializable snapshot isolation (SSI)

Actual Serial Execution

- can use single thread because:
  - Keep the entire active dataset in memory **[KEY]**
  - OLTP transactions only makes a small number of reads and writes **[KEY]**. BTW, OLAP can be run against snapshot isolation so don't need serialization
- Redis use single thread
- Save the overhead of using lock, so could have better performance over concurrency. But throughput is limited to single CPU core **[KEY]**
- transactions need to be structured differently to make them single thread **[KEY]**

Encapsulating transactions in stored procedures

- to avoid a transaction be idle when wait for human actions, normally let a transaction committed within a same HTTP request
- transactions normally are interactive style: app client query something, and use the result to decide what to do next. There are network delay between app and DB. The throughput would be dreadful if execute transactions one by one
- single-threaded serial transaction processing don’t allow interactive multi-statement transactions **[KEY]**. App needs to submit the entire transaction code to DB as a stored proecdure

Pros and cons of stored procedures

- different DBs have different lang for stored procedures
- hard to debug. No version control, no metrics
- a bad stored procedure could impact all apps accessing the DB
- to overcome the cons, modern DBs use Java or other standard langs

Partitioning **[KEY]**

- Since single-thread transaction processing only use single CPU core, to avoid waste resource, read-only transactions can be execute somewhere else use snapshot isolation
- for writes, can scale to multiple cores/nodes by partitioning the data that each transaction only needs to read and write from a single partition
- if a transaction needs to access multiple partitions, need the stored procedure to use locks to coordinate across all the partitions
- The throughput is lower because the overhead of coordinate partitions, and also cannot be increased by adding more nodes
- simple key-value pair data can be easily partitioned, while secondary indexes would be hard

Two-Phase Locking (2PL) **[KEY]**

- if nobody writes to an object, transactions concurrently read the object are allowed
- if transation A has read an object, transaction B that writes to the object must wait until A is committed or aborted
- if transaction B has written to an object, before B commits or aborts, A cannot read
- writes also block other writes
- vs. snapshot isolation, where reads never block writes, and writes never block reads

Implementation of two-phase locking **[KEY]**

- have a lock on each object in DB. Lock can be in shared or exclusive mode
- when a transaction wants to read an object, acquire the lock in shared mode. Multiple transactions can hold the lock in shared mode, but if there is another transaction gets the lock in exclusive mode, then this transaction needs to wait
- when a transaction wants to write an object, it needs acquire the lock in exclusive mode
- if a transaction first read then write, it can upgrade the lock to exclusive
- after a transaction gets a lock, it must hold the lock until the transaction ends
- two phases: 1. when gets the locks, 2. when all locks are released
- deadlock could happen. DB detects the transactions that cause deadlocks and abort one of them

Performance of two-phase locking

- transaction throughput and response time of quries are much worse than weak isolation
- performance degrade due to wait time for acquiring the lock. There could be a queue for transactions on an object. The latency could be unstable
- if deadlocks are happening quite often, transactions would need to be aborted and retry all the works they have already done. Causing long latency for high percentile

Predicate locks

- solve phantom problem
- the predicate lock belongs to all the objects that match some search condition
- if a transaction wants to read objects matching some conditions, it acquires a shared-mode predicate lock on the conditions of all the objects (based on the value). If another transaction holds an exclusive lock on any of those objects, this transaction needs wait **[KEY]**
- if a transaction want to insert/update/delete any object, it needs to check whether there is a lock on old and the new values
- The predicate lock applied to objects that not exist yet

Index-range locks

- one transaction can add too many predicate locks and cause checking locks become time consuming
- index-range lock (next-key lock): blocking greater set of objects than predicate lock is safe. So put a shared lock on the index entry or a range of values in the index. When another transaction needs to update the index entries, it needs to wait **[KEY]**
- If no index is used during the query, DB can fall back to lock the whole table

Serializable Snapshot Isolation (SSI) **[KEY]**

- SSI provides full serializability but only small performance penalty.

Pessimistic versus optimistic concurrency control

- 2PL is pessimistic concurrency control mechanism: if anything might go wrong, wait until safe again
- Serial execution: also pessimistic, requires the whole DB to be locked, and operations need to be quick
- SSI is an optimistic concurrency control technique: transactions concurrently run, and only check if bad things happened during commit **[KEY]**
- it perform badly if there is high contention (a lot of transaction access the same object) **[KEY]**
- if there is enough spare capacity, and contention is not too high, SSI performance better
- Contention can be reduced with commutative atomic operations (like increase counter) **[KEY]**
- SSI is based on snapshot isolation

Decisions based on an outdated premise

- phantom transaction takes action based on a premise that is no longer true when commit **[KEY]**
- DB assumes if a transaction makes a query, when the premise changes, writes in the transaction are invalid and the transaction needs to abort
- two cases when a query result could be changed **[KEY]**
  - Detecting reads of a stale MVCC object version (uncommitted write occurred before the read)
  - Detecting writes that affect prior reads (the write occurs after the read before the commit)

Detecting stale MVCC reads **[KEY]**

- the DB tracks when a transaction ignores other transactions' writes due to MVCC, and when the transaction wants to commit, if there are any ignored writes are committed, abort
- For read only transactions, no need to abort the transaction

Detecting writes that affect prior reads **[KEY]**

- Use the SSI lock (index range lock) but not block other transactions
- after multiple transaction searchs for an index, the index is locked and the transactions are recorded. When a transaction is finished, the record can be removed
- when a transaction writes, check any other transactions hold the shared lock on the index. notify those transactions that the premise might out dated. When the transaction commits, those transactions that get notifications need to abort

Performance of serializable snapshot isolation **[KEY]**

- to reduce the number of unnecessary aborts, if can prove that the result of the execution is nevertheless serializable, no need to abort
- SSI is not limited to the throughput of a single CPU core: the detection of conflicts can across multiple machines
- The aborts can impact the performance a lot. So SSI requires that read-write transactions be fairly short. long-running read-only transactions may be okay

Summary

- **TODO**: a good summary, better to revisit

#### Chapter 8. The Trouble with Distributed Systems

Review

1. 10/06/2023

things that may go wrong in a distributed system

- network issues
- clocks and timing issues

Faults and Partial Failures

- single machine hardware problems can cause kernel panic
- good software either fully functional or entirely broken
- distributed system has a wide range of things can go wrong
- Datacenter backbone: A backbone connects local area networks (LAN) to wide area networks (WAN)
- partial failures are nondeterministic but will happen

Cloud Computing and Supercomputing

- high-performance computing (HPC) vs. cloud computing
- supercomputer: has checkpoints for computation work. Can be used to restore
- internet-related applications: stopping the cluster for repair is not acceptable
- Supercomputers: also have multiple nodes with special network topologies. nodes communicate through shared memory and remote direct memory access (RDMA)
- Large datacenter networks are based on IP and Ethernet, arranged in Clos topologies to provide high bisection bandwidth **[KEY]**
- distributed systems must be a reliable system from unreliable components
- suspicion, pessimism, and paranoia pay off

Unreliable Networks

- The internet and most internal networks in datacenters (often Ethernet) are asynchronous packet networks **[KEY]**
- Ethernet is the technology to make internet works. Ehternet vs. wifi
- the network gives no guarantees as to when it will arrive, or whether it will arrive at all
- Issues could be
  - request lost due to network cable unplugged
  - request wait in the queue due to recipient overloaded
  - remote node failure due to power outage
  - remote node temporarily stop responding due to process pauses
  - the response lost due to a network switch misconfigured
  - response delayed due to network congestion
- timeout is the usual way to handle such issues **[KEY]**

Network Faults in Practice

- adding redundant networking gear doesn’t reduce faults since it doesn’t guard against human error
- a network link works in one direction doesn’t guarantee it’s also working in the opposite direction, inbound traffic works but outbound might not
- network fault (network partition or netsplit) **[KEY]**: one part of the network is cut off from the rest due to a network fault
- error handling of network faults needs to be defined and tested, otherwise could cause
  - a cluster could become deadlocked and permanently unable to serve requests, even when the network recovers
  - delete all the data
- simple approach is to show an error message
- Chaos Monkey: deliberately trigger network problems

Detecting Faults

- no process is listening on the destination port **[KEY]** (system sends a `RST` or `FIN` packet to let TCP connections close or refuse)
- a node process crashed but the node’s operating system is still running **[KEY]**, a script can notify other nodes about the crash so that another node can take over
- query management interface of the network switches to detect hardware link failures (e.g., machine power down). But the interface itself might not reachable
- a router may reply with an ICMP Destination Unreachable packet
- negative feedback can help quicker fail over, but cannot be rely on it to detect faults. should assume no response at call when failure occurs **[KEY]**
- to make sure a request is successful, need a positive response **[KEY]**
- TCP retries automatically, app can also retry, until timeout

Timeouts and Unbounded Delays

- if the timeout is too short, the node treated as dead might just processing some actions. If another node taken over, the actions might be processed twice **[KEY]**
- if the node is slow due to high load, declaring it as dead spread the load to other nodes and cause cascading failure **[KEY]**
- asynchronous networks have unbounded delays

Network congestion and queueing

- packets wait in the queue of the destination's network switch. If the queue is filled up, further packets dropped **[KEY]**
- virtual machine needs wait for the CPU cycle so it could increase the network delay
- TCP performs flow control(i.e., congestion avoidance/backpressure) **[KEY]**: a node can limit its sending rate. There is a queue on the sender
- If TCP doesn't get ack within a timeout (round trip time), it would retransmit and increase the delay **[KEY]**
- if a packet loss is worthless, can use UDP that doesn't have flow control
- a noisy neighbor **[KEY]** can used up shared resources like network links and switches
- can measure data points before setting the timeout
- systems can continually measure response times and their variability (jitter), and automatically adjust timeouts **[KEY]**

Synchronous Versus Asynchronous Networks

- fixed line telephone network: a fixed, guaranteed amount of bandwidth is allocated. It is synchronous. It has bounded delay.

Can we not simply make network delays predictable?

- circuit-switched networks vs. packet-switched protocols: IP suffer from queueing, but optimized for bursty traffic
- ATM has hybrid network supports both circuit and packet switching **[KEY]**
- Use quality of service (QoS, prioritization and scheduling of packets) and admission control (rate-limiting senders), can provide statistically bounded delay, but it is not used in multi-tenant DCs and public clouds
- guaranteed latency vs. resource utilization is the key difference

Unreliable Clocks

- In a distributed system, time is used to determine the order of requests/responses
- each machine on the network has its own clock that are not perfectly accurate
- Network Time Protocol (NTP) **[KEY]**

Monotonic Versus Time-of-Day Clocks

- mordern computers have both
- Time-of-day clocks: could be reset when sync with NTP. unsuitable for measuring elapsed time. **[KEY]**
- Monotonic clocks: suitable for measuring a duration, e.g., timeout. Guaranteed to always move forward. shouldn't compare it between different machines **[KEY]**
  - NTP might change the speed of monotonic clock

Clock Synchronization and Accuracy

- Clock drift varies depending on the temperature of the machine
- if a node is accidentally firewalled off from NTP, the misconfig could get unnoticed for a while
- NTP also could be affected by network congestion, even give up
- NTP could also give wrong time, even though the node queries multiple NTP servers
- leap seconds messed up timing assumptions and crash large systems. smearing **[KEY]**: let NTP adjustment the leap second gradually over the course of a day
- the hardware clock in virtual machine is virtualized, so if CPU cycle is paused for this node, the clock would jump forward suddenly
- the clock on mobile or embedded devices are not trusted, because user can change the time **[KEY]**
- for system requires extreme clock accuracy, can use GPS receivers, the Precision Time Protocol (PTP) **[KEY]**

Relying on Synchronized Clocks

- robust software needs to be prepared to deal with incorrect clocks
- clock issue is hard to detect and the system could have subtle data loss
- need to carefully monitor the clock offsets between all the machines

Timestamps for ordering events

- multi-leader replication use the time-of-day on the node to determine where the write is originated
- last write wins (LWW): can use the timestamp generated by the client not the server, but still cannot solve time skew issue **[KEY]**
  - writes on a node with slower clock might silently disappear because writes from faster nodes always win
  - LWW cannot distinguish between writes that occurred sequentially in quick succession (one by one) and writes that were truly concurrent. alone with causality tracking mechanisms, like version vector, are the in place
  - two nodes generate writes with same timestamp cause the tie breaker (append a random number) is needed
- logical clocks: based on incrementing counters, like USN.

Clock readings have a confidence interval

- the best possible accuracy is probably to the tens of milliseconds, and the error may easily spike to over 100 ms when there is network congestion **[KEY]**

Synchronized clocks for global snapshots

- snapshot isolation requires a monotonically increasing transaction ID. It is hard to get a global increasing transaction id. **[KEY]**
- since generating global transaction id needs coordinate across machines, it could be the bottleneck of the performance
- Use google TrueTime API **[KEY]**, which returns the timestamp with a confident internal range, to get more accurate timestamps, and use them as the transaction ids
- But timestamp ranges can overlap with each other and cause the order cannot be determined. So the DB needs to let read-write commits always after the confidence interval **[KEY]**

Process Pauses

- In a single leader replication topology, how to determine which replica is the leader:
  - leader obtain a lease (a lock with timeout) from other nodes **[KEY]**. It needs to periodically renew the lease before expire. If fail to renew, another node take over
  - when to renew the lease is based on local synchronized clock
  - the renew might take some time. If the lease expired during that, other node might take over **[KEY]**
  - the long running renewal could be caused by
    - the lang runtime GC occasionally pause all running threads
    - VM suspended to save everything in memory to disk
    - user put the client to sleep
    - slow disk I/O
    - OS can do swapping to disk (paging). Thrashing **[KEY]**: when memory pressure is high, OS keeps swapping pages in and out of memory. On server paging is often disabled
    - Unix process can be stopped when it received `SIGSTOP` until `SIGCONT`
- Get thread-safe on a single machine **[KEY]**: use mutexes, semaphores, atomic counters, lock-free data structures, blocking queues, etc.
- distributed system doesn't have shared memory so those methods are not there. Distributed systems rely on messages send over unreliable network. So pause can happen any time while others still running. The paused node might wake up and not know it was asleep **[KEY]**

Response time guarantees

- hard real-time systems: carefully designed and tested to meet specified timing guarantees in all circumstances
- real-time operating system (RTOS) **[KEY]**: allows processes to be scheduled with a guaranteed allocation of CPU time in specified intervals; lib functions doc their worst case exectuion times; dynamic memory allocation restricted
- such system may has low throughput

Limiting the impact of garbage collection

- process pause can be mitigated without using RTOS
- treat GC pause as a brief planned outage of a node **[KEY]**. let other nodes handle requests during the time. runtime needs to warn the app that it will do a GC soon
- another idea, let GC only run for short lived objects that are fast to collect. Restart process periodically to avoid a full GC for long lived objects **[KEY]**. Other nodes handle requests like a rolling upgrade

Knowledge, Truth, and Lies

- a node can only make guesses based on the messages it receives or doesn’t receive, but not for sure
- system model: state the assumptions about the behavior. Algorithms can be proved to function correctly within a certain system model, even if the underlying system model provides very few guarantees

The Truth Is Defined by the Majority

- one node can receive inbound traffic but cannot send outbound traffic
  - other nodes will declare it as dead even it is running
  - if using ack message, the node can detect that some fault happens with the network **[KEY]**
- a node experience a process pause. other nodes declare it as dead, but it come back after a while and didn't realize some time has passed
- in those scenarios, node cannot trust its own judgement, so many distributed algorithms rely on a quorum
- the quorum is an absolute majority of more than half the nodes in normal case **[KEY]**

The leader and the lock

- uniq in distributed system needs to be agreed by a quorum of nodes **[KEY]**
- a client gets a lock from a lock system, but before it writes the data, the process paused and lease expired. So when it start writes, other clients might get the lock **[KEY]**

Fencing tokens **[KEY]**

- when accquire a lock, it also returns a fencing token, which is a self-increasing number
- when a client sends a write request, needs to provide the token
- if the storage sees a token that is smaller than previous write, the client lost the token already, so reject
- ZooKeeper can be used as lock service. the transaction id `zxid` or the node version `cversion` are guaranteed to be monotonically increasing
- it requires the resource to check the token. If DB doesn't support that, can append the token version to the filename

Byzantine Faults

- client can send fake fencing token
- Byzantine fault **[KEY]**: a node claims it received a message when in fact it didn’t
- Byzantine Generals Problem **[KEY]**: reaching consensus in an untrusting environment
- the cost of deploying Byzantine fault-tolerant solutions is high, so not commonly used in server-side data systems
- Web applications need to expect arbitrary and malicious behavior of clients that are under end-user control
  - input validation, sanitization, and output escaping are needed **[KEY]**
- make the server the authority on deciding what client behavior is allowed **[KEY]**
- peer-to-peer networks needs to solve Byzantine problem as it doesn't have a central authority **[KEY]**
- Byzantine fault-tolerant algorithms require a supermajority of more than two-thirds of the nodes to be functioning correctly **[KEY]**

Weak forms of lying

- corrupted packets are caught by the checksums built into TCP and UDP. Can also have app level checksums to validate the whole request **[KEY]**
- A publicly accessible app must carefully sanitize any inputs from users. Can do protocol parsing **[KEY]**
- NTP clients can be configured with multiple server addresses **[KEY]**

System Model and Reality

- Algorithms should not depend too heavily on the hardware and software configuration
- need formalize the kinds of faults that we expect to happen in a system
- system model: an abstraction describes what things an algorithm may assume
- timing assumption system models
  - Synchronous model: assumes bounded network delay, process pauses, and clock error. not a realistic model
  - Partially synchronous model **[KEY]**: sometimes exceeds the bounds for network delay, process pauses, and clock drift. Most useful
  - Asynchronous model **[KEY]**: an algorithm is not allowed to make any timing assumptions. Doesn't have a clock
- node failure system models
  - Crash-stop faults: node failures always lead to crash
  - Crash-recovery faults **[KEY]**: after crash, the node might start responding after unknown time. Storage is stable. Memory is lost. Most useful
  - Byzantine (arbitrary) faults: nodes can do anything

Correctness of an algorithm

- Need describe the properties of the output, then check if the algorithm is correct
- For fencing token:
  - token should be uniq
  - token should be monotonic seq
  - availability: a node request a token can get it if not crash

Safety and liveness **[KEY]**

- Safety properties: nothing bad happens
- liveness properties: something good eventually happens

Mapping system models to the real world

- Some realisitic issues can still happen other than what the system models expect, for example HDD failure

Summary

- timeouts can’t distinguish between network and node failures
- some node can be still running but not healthy

#### Chapter 9. Consistency and Consensus

**[HERE]**: **[KEY]**

tolerating faults: keeping the service functioning correctly, even if some internal component is faulty

- find some general-purpose abstractions with useful guarantees, implement them, and then let applications rely on those guarantees
- one abstraction: consensus. reliably reaching consensus in spite of network faults and process failures
- consensus can be used in electing the leader of replicas, and quite a few other places

Consistency Guarantees

- convergence **[KEY]**: eventual consistency. But this guarantee is weak because it doesn't tell well the converge is done
- systems with stronger guarantees may have worse performance or be less fault-tolerant
- distributed consistency vs. transaction isolation **[KEY]**: focus on different concerns, consistency is coordinate the replication state when delay and fault happen

Linearizability **[KEY]**

- strongest consistency model. other names: atomic consistency, strong consistency, immediate consistency, or external consistency
- the DB shows only one replica to the client. All operations on it are atomic
- recency guarantee: the value read is the most up-to-date value

What Makes a System Linearizable?

- in distributed sytem, one stored object is called as a register
- one constraint: once a version of a register is returned, all the following read needs return that version
- atomic compare-and-set operation: if x == some value, then set x to another value, otherwise not change x

Linearizability vs. Serializability **[KEY]**

- Serializability can read/write multiple objects/rows. Serial orders are fine to be different from the actual orders of the transactions
- Linearizability only R/W single register/object. Doesn't group operations into transactions. Doesn't solve isolation issues
- strict serializability/strong one-copy serializability: DB provides both serializability and linearizability, using 2P locking or actual serial execution. SSI does not

Relying on Linearizability

- Locking and leader election: when every node starts it accquires a lock. The lock must be linearizable
  - Coordination services **[KEY]**: Apache ZooKeeper and etcd. Can be used to implement distributed lock and leader election. They use consensus algorithms. Apache Curator lib is built on ZooKeeper and can be used. Underlayer build on linearizable storage service
  - to implement distributed lock in a more granular level, can have a dedicated cluster interconnect network for communication between database nodes
- Constraints and uniqueness guarantees **[KEY]**: enforce the constraints when data is written, need the write accquires a lock, and do a atomic compare-and-set operation
  - uniq must need linearizability
  - foreign key or attribute constraints sometime can be implemented without linearizability
- Cross-channel timing dependencies **[KEY]**: for example store a file in a storage, and put an instruction in a message queue to let a background task to do something for the file. The instruction might be picked up earlier than the file replication

Implementing Linearizable Systems

- single leader replication: read from leader or synchronously updated followers **[KEY]**, it could be linearizable. But if use SSI, or if there are concurrency bugs, then it is not
  - need the client knows who is the leader, which needs to solve consensus problem
- Consensus algorithms **[KEY]**: similar to single leader replication, but contains measures to prevent split brain and stale replicas. ZooKeeper and etcd work in this way
- Multi-leader replication: cannot be linearizable
- Leaderless replication: requring quorums can not achieve linearizable completely. Because LWW conflict resolution is based on time-of-day. Sloppy quorum also ruin linearizable

Linearizability and quorums

- because of the network delay, the quorum might not have enough replicas that get the latest value. If the read gets to the replica has the latest value once, but not next time, then it is not linearizable
- if reduce the performance and force a read repair, and also the writer needs to read the quorum before a write
- during concurrent writes, LWW makes Cassandra lose linearizability
- the write cannot be compare-and-set operation, otherwise it lose linearizability because it needs consensus algorithms

The Cost of Linearizability

- if there is a network interruption between DCs **[KEY]**
  - multi-leader replication: each DC performs fine. The changes need to be replicated over are queued up
  - single-leader replication: writes will be broken in the follower DC. reads are not linearizble from the follower. Client can connect directly to the leader

The CAP theorem

- any linearizable database has the issue (network interupt between inter-connection of nodes)
- if the app needs linearizability, during the network issue it is unavailable, because write fails
- app doesn't need linearizability can still perform write, just the read is not linearizable. The availbility is better
- CAP theorem: Consistency, Availability, Partition tolerance can only pick 2 out of 3 (but partition fault happens any way so only choose 1 out of the first 2). need to choose between linearizability and availbility

Linearizability and network delays

- even within a machine, the memory access is not linearizable with multi-core doing concurrent writes to their own memory cache first

Ordering Guarantees **[KEY]**

- using a single leader replication is to make the write order easier to define
- Serializability ensures transactions are behave as they are running in orders
- The timestamp and clock are to introduce order

Ordering and Causality

- ordering helps preserve causality
- Casual dependencies **[KEY]**: 1. response need after the request, 2. update need after the create, 3. B happenes after A: so B knows about A
- consistent means consist with causality **[KEY]**. Read skew violate causality
- snapshot isolation provides causal consistency: if see a data, then any data precedes it also be able to see

The causal order is not a total order **[KEY]**

- total order: any two elements can be compared
- partially ordered: based on some rules, one is greater than another, but not always
- linearizable system: total order
- casuality: only two events are casually related, they have order, otherwise they can be concurrent so incomparable
- in linearizable datastore, there is no concurrent. Events can be queued but only one occurs at a time

Linearizability is stronger than causal consistency

- linearizability implies causality
- linearizability is not the only way of preserving causality, so no need to spend a lot to implement linearizability
- causal consistency is the strongest possible consistency model that don't get slow down by nerwork latency. performance and availability are similar to eventual consistent systems. Still new tech

Capturing causal dependencies **[KEY]**

- when a replica processes an operation, it must ensure that all causally preceding operations have already been processed, otherwise need to wait
- can use version vectors track casual dependencies across the DB
- when write, the client pass in the version, and the DB use it to track which data has been read by which transaction

Sequence Number Ordering **[KEY]**

- impracticable to keep track of all casual dependencies, because cannot track all the reads
- use sequence numbers/timestamps from a logical clock to order events. It should be small in size and every op has an uniq seq num
- in a single leader replication, if the follower applies the writes in the order as the replication log, then the follower is always causally consistent

Noncausal sequence number generators **[KEY]**

- for non-single leader DB (e.g., partitioned DB, multi-leader or leaderless DB), serveral different ways
  - each node has it own seq num and stored with some bits indicate which node
  - attach the timestamp from time-of-day clock. LWW uses it
  - preallocate some ranges of seq nums for each node. If used up, allocate a new range
- those ways cannot generate casual consistent seq nums for ops across nodes

Lamport timestamps **[KEY]**

- sequence numbers that is consistent with causality: a pair of [counter, node ID]
- the one with a greater counter is the greater timestamp or the one with the greater node ID is the greater timestamp
- every node and every client keeps track of the maximum counter value it has seen so far, and update to the max if one write gives a bigger counter
- lamport timestamp vs. version vector
  - version vector is used to distinguish whether two ops are concurrent or have casual dependence
  - lamport timestamp enforce a total ordering, but cannot tell if two ops are concurrent, but it is more compact

Timestamp ordering is not sufficient

- Lamport timestamps cannot solve uniq constraint problem: because only when nodes compare with each other, then the timestamp can be used for LWW
- need to use total order broadcast

Total Order Broadcast

- single-leader replication: the throughput might not be able to handled on a single node; when the leader fail, needs failover
- Partitioned databases only has ordering per partition. Total ordering across all partitions requires additional coordination
- total order broadcast (atomic broadcast) **[KEY]**: a propotcol for exchanging message across nodes. requires two safety properties
  - reliable delivery: if a message is delivered to one node, it is delivered to all nodes
  - totally ordered delivery: all the nodes have same order

Using total order broadcast **[KEY]**

- Consensus services (ZooKeeper and etcd) use it
- state machine replication: every replica processes the same writes in the same order, then replicas remain consistent just with temp lag
- same for serializable transactions
- not allow to insert a message between delivered ordered messages
- it is a way of creating logs
- can also be used for implement a lock. The accquire lock op can be a message in the log. The seq num can be used as a fencing token

Implementing linearizable storage using total order broadcast **[KEY]**

- linearizable system vs. total order broadcast
  - total order broadcast is async. linearizablity is a rencency guarantee
- linearizable storage can be build on top of total order broadcast
  - uniq constraint: each object has it's own register with atomic compare-and-set op. initially they have value null. So if two clients create the same value, one will see the value is not null so fail the compare-and-set
  - Append a log for the creation to a node. Then read the object. If the returned first log is not the log we create, then fail the op. Since all nodes have the same order of writes, if there is a concurrent write, LWW can drop following writes
  - same for implementing serializable multi-object transactions
  - but it only provides sequential consistency (timeline consistency), doesn’t guarantee linearizable reads
- to make read linearizable, few options
  - sequence read as a log. When the message returns, returned the log, so all the previous writes are already replicated (Quorum reads in etcd)
  - fetch the position of the latest log message in a linearizble way, and wait for all entries up to that position to be returned (ZooKeeper `sync()`)
  - read from a replica syncly updated on writes (chain replication)

Implementing total order broadcast using linearizable storage **[KEY]**

- vise versa, when have a linearizable register with compare-and-set ops, can implement total order broadcast
- when write, append the seq num, then send the message to all nodes. Each node applies the message based on the seq num. There should be no gap in the seq nums
- the linearizable compare-and-set register and total order broadcast have the same consensus challenge to solve

Distributed Transactions and Consensus

- FLP result: there is no algorithm that is always able to reach consensus if there is a risk that a node may crash, under async system model (cannot use clock)
- if can use timeout or some other way to detect crashed nodes (even it is not actually crash), consensus can be achieved

Atomic Commit and Two-Phase Commit (2PC)

- used for atomic commit (a transaction span across multiple nodes). Not a good consensus algorithm
- atomic commit is useful in multi-object transactions and secondary index

From single-node to distributed atomic commit

- on a single node, when commit a transaction, first writes to write-ahead log, then append a commit record. If the node crashes in the middle, if the commit is not written, then rollback; otherwise it is durable. The controller (the device manage the disk) makes the commit atomic
- for multiple nodes, cannot send commits to all nodes. Failure cases cause inconsistency could happen:
  - some nodes can hit constraint violations
  - some commits lost due to network
  - Some nodes might crash
- compensating transaction: un-commit a commit transaction, but itself is another transaction

Introduction to two-phase commit **[KEY]**

- Needs a coordinator (transaction manager). Can be a lib or a service
- nodes involved are participants
- app reads and writes as normal. When commit
  1. sends a prepare request to each node. Coordinator tracks the responses
  2. If all participants return yes, then send a commit request, if any replies no, send an abort request to all nodes

A system of promises **[KEY]**

- when app starts a transaction, the coordinator gives a transaction id that is globally uniq
- Read/Write are done on one of the participant with the transaction id appended. The coordinator replicate it as a single-node transaction across all the nodes
- when commit, all nodes get the prepare request with the transaction id, and verify disk space, constaints etc. to make sure it can commit
- the coordinator gets the responses and makes a decision, and writes it to its transaction logs. This is the commit point
- when send commit request, if any nodes fail, the coordinator keep retrying

Coordinator failure

- before coordinator sends prepare requests, any participant can abort
- if a participant sends a yes response for the prepare request, then it cannot abort by itself (so timeout cannot be used) **[KEY]**, and must wait for coordinator to send commit/abort request. This is doubt/uncertain state
- participants can communicate with each other to find the decision when coordinator is not responding, but it is not in 2PC protocol
- after recover, coordinator reads its transaction logs to see the decision **[KEY]**. If it see any participant doesn't response to commit, the transaction is abort

Three-phase commit

- 2PC is blocking atomic commit protocol. 3PC makes it non-blocking, but not easy to implement in practical
- need to have a network with bounded delay and response time
- adds a perfect failure detector to detect node failure

Distributed Transactions in Practice

- compare to single node transaction, distributed transaction is 10x slower, because it needs disk forcing for recovery, and additional network RTT
- Database-internal distributed transactions: in replication and partitioned DBs, because all nodes run the same software, it is supported well
- Heterogeneous distributed transactions **[KEY]**: participants can use different techs, such as message broker. Supporting distributed trasaction is challenging

Exactly-once message processing

- Heterogeneous distributed transactions: diverse systems integrated together
- message queue + DB: a transaction needs atomically committing the message and the DB write in one transaction. Then the message is ensured that it is effectively (not retry) processed exactly once
- requires all systems use the same atomic commit protocol. Some described below

XA transactions

- X/Open XA (eXtended Architecture) **[KEY]**: a standard for 2PC across heterogeneous techs
- It is a C API for interfacing with a transaction coordinator. The drivers of DB, MQ implements XA, and call the API to figure out if an op belongs to a distributed transaction
- The transaction coordinator has XA built in as a lib. Normally the coordinator itself is a lib loaded by the app

Holding locks while in doubt

- during a transaction, some rows get locked before committed
- if coordinator crashes, the lock won't be released

Recovering from coordinator failure **[KEY]**

- Orphaned in-doubt transactions: coordinator cannot decide the outcome because transaction log lost, or software bug. They cannot commit or abort foever
- heuristic decisions: a participant can commit or abort an in-doubt transaction without getting decision from the coordinator. It is an emergency escape hatch

Limitations of distributed transactions

- the coordinator is also kind of a DB, so needs care as DBs
- Need replication
- Since coordinator is not stateless **[KEY]**, as its logs is a crucial part of the durable system state, deploying new software version to it would be tricky
- Since XA needs to support a wide range of systems, it is a lowest common denominator. it cannot detect deadlock or work with SSI, because those are protocols implement on top of the systems **[KEY]**
- can cause amplifying failures and impact the fault tolerant, because block wait **[KEY]**

Fault-Tolerant Consensus **[KEY]**

- consensus problem: nodes propose values, the consensus algorithm decides on one of those values
- a consensus algorithm must satisfy properties:
  - Uniform agreement
  - Integrity: no node decide twice
  - Validity: the decided value must be proposed by some nodes
  - Termination: every node that is not crashed eventually decides some value (including coordinator)
- fault tolerance: termination property formalize it. a consensus algorithm must make progress even some nodes fail (but still majority nodes are working and form a quorum)

Consensus algorithms and total order broadcast

- Well known consensus algorithms: Viewstamped Replication (VSR), Paxos, Raft, Zab
- they are also total order broadcast algorithms. they decide on a sequence of values
- total order broadcast is equivalent to repeated rounds of consensus **[KEY]**

Single-leader replication and consensus

- if lead is chose by human, then it is already a consensus algorithm, but not satisfy termination property

Epoch numbering and quorums **[KEY]**

- consensus protocols make a weaker guarantee: leader is not uniq. define an epoch num. Within each epoch, the leader is uniq
- if the current leader is thought to be dead, nodes start a vote, and the election gives an incremented epoch num. Epoch nums are totally ordered
- if there is a conflict between two leaders (maybe caused by the previous leader not dead), the higher epoch wins
- before a leader decides, it must collect votes from a quorum of nodes. A note votes only when it doesn't know a higher epoch
- two rounds of votes: 1. Decide leader, 2. votes on leader's decision. The quorums must overlap.

Limitations of consensus **[KEY]**

- nodes vote proposal is sync replication, so it is high cost
- it use strict majority, so need a min of 3 nodes to have the quorum
- most consensus algorithms are static membership algorithms. They assume fixed of nodes. Dynamic membership extensions for those algorithms are not well known so far
- rely on timeouts to detect failed nodes. But in an env with highly variable network delays (such as geographically distributed systems), it could cause frequent leader elections and harm performance

Membership and Coordination Services

- distributed key-value stores/coordination and configuration services **[KEY]**: ZooKeeper, etcd. Their APIs are similar to DB
- HBase, Hadoop YARN, OpenStack Nova, and Kafka all rely on ZooKeeper running in the background
- they are only designed for small amount of data that can fit in memory
- the data is replicated to all nodes using a fault-tolerant total order broadcast algorithm **[KEY]**
- ZooKeeper is modeled after Google’s Chubby lock service. It also implements **[KEY]**:
  - Linearizable atomic operations: can be used for a lock with compare-and-set op. The lock is implemented as a lease. The client fails after the expiry time
  - Total ordering of operations: can implement a fencing token using the zxid (transaction id) and cversion
  - Failure detection: clients maintain long-lived session with ZooKeeper server, so ZooKeeper can gather heartbeats. Release locks if a session times out for an ephemeral node
  - Change notifications: a client can find out if another client joins the cluster or fails. Can subscribe to notifications

Allocating work to nodes

- leader election is also useful for job schedulers
- node assigments for partitions changes can also use ZooKeeper. During the change, some nodes need to take loads from other nodes
- perform mojority votes on a 1000+ nodes are inefficient, so ZooKeeper only votes among 3 to 5 nodes **[KEY]** but supporting a large number of clients by outsourcing some coordinating works (consensus, operation ordering, and failure detection) to an external service
- data managed by ZooKeeper normally change not frequently (1 w per min/hour). It should not store app runtime states

Service discovery

- to reach to a service, which IP to use
- when a node starts, reg its network endpoint in a service registy, and use ZooKeeper to let other services find it **[KEY]**
- If not need consensus, DNS is the old and better way. It uses multi-level caching so data could be stale **[KEY]**
- but to tell who is the leader, need consensus
- can use read-only caching replicas. Those nodes not participate in votes, but serve read requests

Membership services

- determines which nodes are currently active and live members of a cluster
- hard to detect fail nodes, but with consensus, can come to agree on which node alive

### Part 3. Derived Data

- a large app needs to access and process data in different ways using different datastores
- need implement mechanisums for moving data from one to another
- Systems of record: source of truth. new data first write here exactly once and are normalized
- Derived data systems: getting existing data and process it. e.g., cache, denormalized values, index, materialized views, predictive summary from a recommendation system

#### Chapter 10. Batch Processing

Different types of systems

- Services (online systems): response time and availability is the primary measure of the performance
- Batch processing systems (offline systems): measures throughput as the performance **[KEY]**. e.g., Hadoop implements MapReduce algorithm
- Stream processing systems (near-real-time systems): consume input and operate on event soon **[KEY]**. Build on Batch processing system

Batch Processing with Unix Tools

- can be used in log system

Simple Log Analysis

- Unix tools: `awk`, `sed`, `grep`, `sort`, `uniq`, `xargs` can process GB files in seconds

Chain of commands versus custom program

- program is more readable, but not concise then unix pipe

Sorting versus in-memory aggregation

- for word count solutions, sorting to group same entries together vs. using hash table to aggregation in-mem
- if the different words count is small, the working set of the job is small. They can fit in the memory allocated for the hash table
- if the working set is large, sorting can make the efficient usage of disks with merge sort **[KEY]**. The bottleneck is the disk read performance

The Unix Philosophy

- Make each program do one thing well
- Expect the output of every program to become the input to another. Don’t clutter output with extraneous information. Make output columns flexiable. Don't insist on interactive input
- Build and try small parts quickly. Don’t hesitate to throw away the clumsy parts and rebuild them
- Use tools to help. Build if necessary, even it needs to drop current work

A uniform interface

- to pipeline all programs, need to have a uniform interface
- In unix, the interface is a file descriptor: can be a file, a communication channel to another process, a device driver, a TCP socket

Separation of logic and wiring

- use `stdin`, `stdout` They can attach to other processes, like keyboard, file, screen, etc. There is an in-mem buffer
- similar to loose coupling/late binding/inversion of control
- but it is hard to wire multiple input/output. Need some tricks and configs

Transparency and experimentation

- input is immutable
- the pipeline can be stopped anywhere, and pipe the output to `less`
- can write the output to a file so it can be restarted later **[KEY]**
- but UNIX tools can be only run on one machine, so need Hadoop

MapReduce and Distributed Filesystems **[KEY]**

- MapReduce doesn't have side effects on input. The output files are written in a sequential fashion (not modify written parts)
- Read and write on HDFS, similar to Object storage services (Amazon S3, Azure Blob Storage, and OpenStack Swift)
- HDFS is shared-nothing. Different from shared-disk approach Network Attached Storage (NAS) and Storage Area Network (SAN)
- HDFS has a daemon process running on each machine to expose a network service
- NameNode is a central server that tracks the which file blocks stores on which machines
- File blocks are either replicated or use erasure coding scheme (replicate on another disk on the same machine)

MapReduce Job Execution **[KEY]**

- Read a set of input files, and break them into records done by MapReduce
- Mapper function: extract a key-value pair from each record
- sort key-value pairs by key done by MapReduce
- Reducer function: iterate over key-value pairs
- Can create a second job to process the output of the first job

Distributed execution of MapReduce

- MapReduce handles the parallelism
- The mapper and reducer don't care about the input source or output target, so it can handle moving data between machines
- Hadoop parallelization is based on partitioning
- Map task/Reduce task: including mapper/reducer callbacks, but also do other things
- Map task side, each file block is a partition and can be proceeded by one map task. each input is 100 MB **[KEY]**
- putting the computation near the data: scheduler lets mapper run on the machine stores a replica of the input file **[KEY]**
- MapReduce framwork 1. copies the mapper code to the machine, 2. run the map task to read the input, 3. passing one record to the mapper callback each time
- Reduce task side, job author decides number of reduce tasks to have **[KEY]**
- framework hashes the key and send the same key to same reduce task
- shuffle: reduce task sorts map task output based on reducer partition and writes to map task's local disk. When a mapper finishes the work, the scheduler notifies the reducer to fetch the output
- Reduce task merge sorts the map tasks' output files, then let the reducer iterates and processes each record. It can generates multiple records per input key-value pair, and writes to HDFS on the reduce task machine

MapReduce workflows

- workflow: multiple jobs chained together. Hadoop doesn't natively support it, so need to setup the jobs to output to and input from a same folder
- if a job failed, the output would be discarded, so next job cannot starts. Workflow schedulers like Oozie have been developed
- recommendation systems can have 50 to 100 jobs **[KEY]**. High level tools like Pig, Hive can be used to setup workflows

Reduce-Side Joins and Grouping

- relational model: foreign key; document model: document reference; graph model: edge. DB would need lookup indexes multiple times for queries involve join
- in batch processing, join means all the occurance of an association across the whole dataset, not an individual entry

Example: analysis of user activity events

- there is a user table, contains the birth date. A user activity table. Now want to find out which activities are common for an age group
- Making random-access requests over the network for each record has a poor performance for batch processing. Also the data could change during the process
- process on the backup/snapshot of the DB

Sort-merge joins

- an algorithm used in MapReduce for job requires join
- can have two mappers: 1. map uid to activity, 2. map uid to dob (date of birth)
- secondary sort **[KEY]**: the reducer gets two inputs, put the uid dob records earlier than the activities, while the activities are sorted by the framework already
- Reducer stores the dob as a local variable, then iterate the activies of the uid to generates a map between dob and activity
- then pass the result to a subsequence job to compute the distribution

Bringing related data together in the same place

- the keys in the mapper results are the destination of which reduce task to receive
- MapReduce programming model separate the physical network communication aspects, from the app logic

GROUP BY

- Group records by some key clause together. Follow by aggregation
- Set up mapper to let the results using the grouping key **[KEY]**
- sessionization **[KEY]**: the sequence of actions that a user take. Can implement it by using a session cookie so even requests across partitions

Handling skew

- linchpin objects/hot keys: some keys have way more records
- can cause hot spot/significantly for a single reducer. Subsequent job would need to wait for this slowest reducer to complete
- Pig skewed join method first runs a sampling job to determine which keys are hot, send them to several random picked reducers. Other inputs for the join replicates to all the reducers **[KEY]**
- Hive’s skewed join optimization requires the hot keys to be specified explicitly. Records stored separately. Use map-side join **[KEY]**
- Each reducer groups a subset of the records for the hot key, then a second MapReduce job combines the values

Map-Side Joins

- previous join is done at reducer side. The advantage is that no need to have assumption on the input, so it can be anything. The downside is that sorting, copying to reduce tasks, merging the reducer inputs are expansive
- if the input can have assumption, then no need to use reducer and sorting, only mapper reads input, processes and writes to the filesystem

Broadcast hash joins

- one map-side join case: a large dataset join with a small dataset that can be loaded entirely in RAM **[KEY]**
- store the small dataset as a hash table, then process the large dataset and do join inline
- can have multiple map tasks. Each process a small set of the large dataset
- broadcast: each map task has one copy of the small dataset **[KEY]**
- Pig replicated join uses it

Partitioned hash joins

- the map task partition can be same as the input partition, so one map task only needs to read from one input partition **[KEY]**
- Hive bucketed map joins

Map-side merge joins

- if the inputs are both partitioned and sorted, no need to have the small dataset fits in the memory. Can iterate over both datasets
- the inputs are normally generated by previous MapReduce jobs **[KEY]**

MapReduce workflows with map-side joins

- reduce-side join outputs are sorted by the join key
- map-side join outputs are sorted same as the large input
- to know the dataset physical layout, like number of partitions, by which keys the data are partitioned and sorted, etc., can use HCatalog or Hive metastore to store such metadata

The Output of Batch Workflows

- batch workflows is not exactly same as analytic queries. The queries are not SQL queries
- the output of batch processing is not reports, but some structures

Building search indexes

- full-text search index **[KEY]** is a file stores the term dictionary. Keyword as the key and document IDs (posting list) containing this keyword as value
- when documents change, one expensive approach is to periodically rerun the workflox for all documents and replace existing indexes
- can also incrementally adding new docs to indexes

Key-value stores as batch process output

- classifiers machine learning system **[KEY]**: recommendation systems, spam filters, anomaly detection, image recognition
- output is a DB that can be queried with userId/productId etc.
- can direct the DB client to mapper or reducer, processing one record as a time, but it is a bad idea
  - making a network request for each record is not scale
  - batch process can cause concurrent writes
  - the DB writes could fail causing MapReduce job needs to retry, and can be hard to make sure each record only procceed once
- so create the DB inside the batch job, write the result to a read-only file, and expose to read only queries. HBase bulk loading uses it **[KEY]**
- since they don't frequently write, and the data structure is simple, they don't need a lot of protection, like write-ahead log

Philosophy of batch process outputs **[KEY]**

- treating input as immutable, so the batch process can be run repeatly without side effects
- human fault tolerance: if introduced a bug, can easily switch back to the old outputs, vs. DB that fixing bug cannot fix the data easily
- minimizing irreversibility: beneficial for Agile software development
- if a mapper or reducer fails, the framework retries couple times
- same input can be used by multiple jobs, including monitoring metrics, compare the results for expected characteristics
- separation of concerns: jobs can be wired. Code can be reused
- Hadoop has structured file formats: Avro is used to provide schema-based encoding

Comparing Hadoop to Distributed Databases

- massively parallel processing (MPP): the idea of MapReduce in more earlier time

Diversity of storage

- Hadoop can store any format of data, vs. MPP needs the producer to standarize the input
- data lake/data hub **[KEY]**: collecting data in its raw form, and worrying about schema design later, allows the data collection to be speeded up
- the dataset consumer can interpret the data. Producer has other priority to deal with

Diversity of processing models

- MPP are efficient for certain queries, but not all
- MapReduce can easily run customized code
- Hive build a SQL query execution engine on Hadoop
- other processing models can all run on a single shared-user cluster of machines accessing the same files

Designing for frequent faults

- Batch process are less sensitive to faults as no immediately impact to users
- MPP aborts the whole query when failures happen
- MapReduce can tolerate the failure of a map/reduce task. It eager to write data to disk
- MapReduce is target for large jobs, so even recovery from an individual task introduce a lot of overheads
- Online production services and offline batch jobs runnnig on same machines. Each task has a priority. Higher priority task can terminate lower priority tasks on the same machine **[KEY]**
- the task owners need to pay for the resources, and higher priority tasks are more expensive. MapReduce tasks are normally low priority
- Low-priority computing resources can be overcommitted, to better utilize resources better, but it also means tasks can be terminated any time
- MapReduce is designed to tolerate frequent unexpected task termination

Beyond MapReduce

- Implementing a complex processing job using raw MapReduce APIs is hard

Materialization of Intermediate State

- The complex system that quites a lot of jobs in the workflow produces a lot of output/input folders as intermediate states
- materialization: writing the intermediate state to files **[KEY]**
- fully materializing intermediate state vs. stream the output to next job (UNIX)
  - a MapReduce job can only start when all tasks in the preduding jobs all completes
  - Stream solution makes next job starts as soon as input comes
  - some mappers are redundant. They just read the output of the previous job
  - intermediate states are replicated across nodes unecessarily. they are just temp data

Dataflow engines

- Spark: handle an entire workflow at one job **[KEY]**
- Repeatly calls a user-defined function to process one record at a time. Partition the input to make work parallel
- The output of one function becomes the input of another
- functions/operations are more flexible than mapper or reducer **[KEY]**
- different ways to connect outputs to another inputs **[KEY]**
  - repartition and sort by key. Enables sort-merging and grouping
  - directly dump the output as the input, without sorting. When the paritioning hashing is more important than the order of records, use this option
  - broadcast hash joins
- Dataflow engine benefits are:
  - expensive works like sorting are only done when required
  - no unnecessary map tasks
  - scheduler has a view of where the data is required, so it can optimize. Since all joins and data dependencies are explicitly declared
  - intermediate states can be keep in memory or local disk
  - when input is ready, operations can start ASAP
  - existing JVM can be reused for new operators
- Pig, Hive workflows can convert to Spark with just a config change

Fault tolerance

- Spark avoid writing intermidiate state to HDFS, but recomputed from other data when machine fails
- uses the resilient distributed dataset (RDD) abstraction for tracking the ancestry of data **[KEY]**
- Flink checkpoints operator state **[KEY]**
- whether the computation is deterministic: if not, then downstream might have some previous compute results but some have new results
- not deterministic operations: iterate through hash table, probabilistic and statistical algorithms relying on radom numbers, using system clock, using external data source **[KEY]**
- if recompute is very expensive, better to materialize the result

Discussion of materialization

- Flink uses pipelined execution: incrementally pass the output to other operators, and start processing ASAP
- sort needs all the input so it cannot be pipelined though **[KEY]**

Graphs and Iterative Processing

- graph batch processing: used in recommendation engine or ranking systems **[KEY]**
- Spark arrange the operations in a job as a directed acyclic graph (DAG), but the data is still relational. While graph processing is on the graph strutured data
- graph algorithms normally need iterate the data multiple times, but MapReduce only perform single pass. Can make MapReduce run repeatly, but inefficient

The Pregel processing model **[KEY]**

- bulk synchronous parallel (BSP): Apache Giraph implements it
- one vertex sends a message to another through the edge
- in each iteration, a function is called for each vertex and process all the messages to it. The state is saved in memory. In next iteration, the vertex only process new messages

Fault tolerance **[KEY]**

- not let vertex query neighbors but pass messages to neighbors save time
- the prior iteration must fully complete before the next one can start
- the framework transparently recovers from faults in order to simplify the programming model for algorithms
- periodically checkpointing the state of all vertices at the end of an iteration. When any node in one iteration fails, rollback the whole system to the previous checkpoint

Parallel execution

- when sending a message, just need to use the vertexID. the framework can partition the graph
- neighbor vertexes can be colocated on the same machine, but this way is complicate. Random assign machines is easier **[KEY]**
- intermediate state is normally bigger than the orignal data
- overhead of sending message can slow down the algorithms. So if the graph algorithm can be done on a single machine, it would be quicker. Only when the data is too big, then use Pregel

High-Level APIs and Languages

- now can store and process many petabytes of data on clusters of over 10,000 machines
- dataflow APIs like Hive and Pig use relational-style building blocks to express a computation: joining datasets on some field, grouping by key, filtering, then aggregating
- less code, can see intermidiate state, make the job execution efficiency at machine level

The move toward declarative query languages

- making joins as relational operators, help framework analyze the properties of the join inputs and use algorithms to optimize **[KEY]**
- Hive, Spark have cost-based query optimizers, and can change the order of joins
- the developer needs to specify the joins in a declarative way
- but the mapper and reducer are arbitrary code. The benefit is that it can reuse existing codes, but not declarative like SQL
- For example, filtering some fields by the system can leverage column-oriented storage layouts
- vectorized execution: iterating over data in a tight inner loop that is friendly to CPU caches

Specialization for different domains

- statistical and numerical algorithms that are needed for machine learning apps, like classicification and recommendation, can be reused
- Mahout implements various algorithms for machine learning on top of MapReduce **[KEY]**
- spatial algorithms such as k-nearest neighbors: searches for items that are close to a given item in some multi-dimensional space to find similarity **[KEY]**
- Approximate search is also important for genome analysis algorithms **[KEY]**

Summary

- the input data is bounded in batch processing: a job knows when it has finished reading the entire input
- stream processing: input is unbounded

#### Chapter 11. Stream Processing

stream: incrementally made available over time. `stdin` and `stdout`. lazy lists in program lang, filesystem APIs, TCP connections, audio and video over the network

event streams: a data management mechanism

Transmitting Event Streams

- event: a small, self-contained, immutable object containing the details of something that happened at some point in time
- event is encoded so it can be stored and send over the network
- an event is generated by a producer/publisher/sender, and processed by multiple consumers/subscribers/recipients
- related events group into a topic/stream
- pulling all events from datastore is too expensive for low delay requirement of stream processing
- consumers are notified when new events appear

Messaging Systems

- notifying consumers about new events. Most simple impementation: TCP connection between producer and consumer
- to choose which publish-subscribe model system, answer two questions: **[KEY]**
  1. what to do when producer sends messages faster than consumer can handle? drop, buffer (when the queue filled up, what to do), or backpressure (flow control)?
  2. if nodes crash, can messages lose?

Direct messaging from producers to consumers

- UDP multicast: used when low latency is important. Application-level protocol can recovery lost packets **[KEY]**
- Brokerless messaging libraries: implementing publish/subscribe messaging over TCP or IP multicast **[KEY]**
- StatsD: use unreliable UDP messaging for collecting metrics **[KEY]**
- webhooks: consumer expose a network service. Producer directly makes HTTP or RPC requests to push messages. When ever an event occurs, make a request **[KEY]**
- those systems require the app code to be aware of possible message loss

Message brokers **[KEY]**

- a database for handling message steams. Producers and consumers connect to it as clients This is AMQP/JMS(Java Message System)-style messaging
- can tolerate clients come and go including crash. The broker maintains the durability
- consumers are generally asynchronous: the producer only waits the broker to confirm that the message is buffered, not wait for the message delivery

Message brokers compared to databases **[KEY]**

- Some message brokers can participate in two-phase commit protocols
- Message brokers delete a delivered message
- working set for mesage brokers are small, the queues are short
- message brokers support subscribing to a subset of topics matching some pattern
- brokers notify clients when changes occur

Multiple consumers **[KEY]**

- Load balancing: one message deliver to one consumer. shared subscription
- Fan-out: each message deliver to all consumers
- can also combine above two patterns together

Acknowledgments and redelivery **[KEY]**

- Consumer can crash and never process the message broker deliver. Broker use ack to make sure the message not lose
- needs an atomic commit protocol to make sure the message is not process twice
- for load balancer, the messages might delivered not in the original order
- to keep the order, can have a separate queue per consumer for messages have causal depdencies

Partitioned Logs

- sending message is normally not permanent traced
- receiving a message is destructive as the message is deleted from broker **[KEY]**

Using logs for message storage **[KEY]**

- a producer sends a message by appending it to the end of the log, and a consumer receives messages by reading the log sequentially
- consumer waits for a notification after it reaches the end of the log
- logs can be partitioned. A topic can be defined as a group of partitions
- within each partition, the broker assigns each message a monotonically increaing seq num, as offset, to give totally order of messages
- Apache Kafka, Amazon Kinesis Streams, and Twitter’s DistributedLog work like this, achieve throughput of millions of messages per second by distributed across multiple machines. Achieve fault tolerance by replicating

Logs compared to traditional messaging

- log based approach supports fan-out because read message doesn't delete it
- to achieve coarse-grained load-balancing, assign partitions to different nodes. Downsides are: **[KEY]**
  - num of nodes cannot be more than partitions
  - can cause head-of-line blocking: a single slow process message holds up all subsequence messages
  - parallelize processing is not possible
- when need high throughput, ordering is important, and each message can be processed quick, then log-based approach is good **[KEY]**
- if message processing is expensive, and want to parallelize the work, while ordering is not important, then use JMS/AMQP style of message broker
- consistant order only preserved in the same partition. Can use the client id of the consumer that requires to ordering to sharding the events it subscribes to

Consumer offsets

- the broker doesn't need to track ack for each message, just need to periodically record the consumer offsets to tell which messages have been processed **[KEY]**
- if a consumer node fails, another consumer node is assigned the partition, and start consuming the messages at the last recorded offset. But some message that are processed but not recorded will be processed twice

Disk space usage

- the logs are divided into segments to reclaim spaces. The old segments are moved to archieve storage
- if a slow consumer is stilling reading an archieved segment, it will missed some messages
- circular buffer: keep a bounded size buffer and discard old messages when full **[KEY]**
- log based message system always writes to disk. Another type of message system keeps messages in ram until ram is full, then start writing to disk. Its throughput depends on the amount of history

When consumers cannot keep up with producers

- the log based approach is choosing the buffering way
- need to monitor how far a consumer is behind and raise alert **[KEY]**
- the operational advantage is that one slow consumer won't block other consumers. So can debug with one consumer

Replaying old messages

- just move the offset can replay, make the log based approach like batch processing

Databases and Streams

- a write to a DB can also be an event. They are recorded in the replication log of the DB leader

Keeping Systems in Sync

- using ETL to keep data in different data systems sync, which is a batch process
- periodic full DB dump can be slow
- dual write: the app writes to each data system. Problems: **[KEY]**
  - race condition: concurrent writes causing a data in one system updated by client A and another by client B, cause permenant inconsist
  - fault tolerance problem: one write fails while other succeed and lose atomic
  - conflicts can occur because different data systems don't have a single leader

Change Data Capture

- change data capture (CDC): extract all data changes in a DB and replicate them into another data system, as a stream **[KEY]**

Implementing change data capture

- CDC makes one DB as the leader, other derived data systems are followers/consumers. Using log based message broker as it perserve the order
- DB trigger can be reged to changes and add entries to a changelog table, but it is fragile and slow
- Parsing replication log is better, but have challenges: how to deal with schema change **[KEY]**
- LinkedIn’s Databus uses it
- CDC is async. It can have replication lag

Initial snapshot

- replication logs are truncated, but build full-text index needs all changes
- can create snapshot corresponding to a change log offset **[KEY]**

Log compaction

- check logs with the same key, and throw away duplicates
- update a key with a special value `null` (a tombstone) means the key is removed **[KEY]**
- CDC can give every change a primary key, and every update overrides the previous value, then can keep the latest values. Don't need to take snapshots **[??]**
- It is supported by Apache Kafka

API support for change streams

- Firebase provide data sync based on change feed as first-class interface
- Kafka Connect can integrate CDC tools for other database systems

Event Sourcing

- a tech developed by domain-driven design (DDD) community
- store changes as log of events
- not like CDC, event sourcing are based on immutable events to the application, not the low level DB
- it is more meaningful as it records the user activities **[KEY]**
- similar to the chronicle data model and star schema fact table

Deriving current state from the event log

- application needs to get enough event logs from event sourcing to show customer a present view of the state. The result should be deterministic
- CDC shows the latest status, while event sourcing only express the intention not the state, so need to retrieve the whole history
- apps using event sourcing normally use snapshots, but still need to store the full history of events

Commands and events

- the customers send the application commands, but they can fail. The events are the succeed facts. They are durable and immutable
- validations of a command need to be sync and before it becomes an event **[KEY]**
- one user request can be split to multiple events. For example, first a tentative reservation, then a confirmation event. The validation become async

State, Streams, and Immutability

- DB storing current state is optimized for reads
- changelog represents the evolution of state over time

Advantages of immutable events

- event logs are also useful for auditing. It is used in accounting ledger

Deriving several views from the same event log

- when implementing a new feature that presents the existing data in new way, use the event log to build a separate read-optimized view for the feature is easier than migrate the DB schema, and let the old and new systems running side by side until migration is done **[KEY]**
- command query responsibility segregation (CQRS) **[KEY]**: to support cerntain queries and access patterns, complicate schema design, indexing and storing engines are needed. Using event logs are more flexible to allow different read views
- twitter home timeline is an example of the read-optimized state

Concurrency control

- consumers are async, so they can see read-your-own-writes problem
- solution: make the read view update sync with the event log write. **[KEY]** Need to either make the read view and the event log in the same system, or do a distributed transaction, or following the way described in Implementing linearizable storage using total order broadcast
- most of the need for multi-object transactions can be translated to an event that contains a self-contained description (?? my understanding is that it involves multiple object updates in one event)
- no concurrency control needed if the event log and the application state are partitioned in the same way

Limitations of immutability

- even systems don't use event sourced model also reply on immutability for creating version snapshots
- dataset with a lot of updates and deletes can get performance and garbage collection issues using event sourcing **[KEY]**
- for legal reasons, some data needs to be deleted **[KEY]**

Processing Streams **[KEY]**

- can write the data in the event to DB, cache, index, storage
- sending the event through emails, notifications, dashboards to users
- process multiple input streams and generate multiple output streams. Input is read-only. Output is append-only
- similar to MapReduce and data folow engines. Mapping operations such as transforming and filtering records work same

Uses of Stream Processing

- used for monitors
- requires sophisticated pattern matching and correlations

Complex event processing **[KEY]**

- Complex event processing (CEP): searching for certain event patterns while analysis event streams
- provides a high level declarative query
- queries submit to a process engine, consumes input streams, maintain a state machine perform the matching, emits a complex event when detect the pattern

Stream analytics

- vs. CEP: stream analytics focus on aggregation metrics, measure the rate, calculate the rolling average, compare statistics between different intervals
- use probabilistic algorithms, such as Bloom filters, HyperLogLog, percentile estimation algorithms **[KEY]**

Maintaining materialized views **[KEY]**

- deriving an alternative view onto some dataset so that you can query it efficiently, and updating that view whenever the underlying data changes
- application state is also a kind of materialized view

Search on streams **[KEY]**

- full-text search queries for individual events like a percolator feature
- Elasticsearch can be used
- cannot use index because when the event occurs, the index is not added yet
- if there are a lot of queries, run the document against each query could be slow. So index the queries and the documents, and narrow down the num of queries that could match

Message passing and RPC

- message passing and RPC are not stream processors: they are mainly used in actor frameworks as a communication module to handle concurrency and distributed execution
- comm between actors are one-to-one and ephemeral **[KEY]**. Stream processor event logs are durable and multi-subscriber
- actors can comm in arbitary ways, for example cyclic request/response, while stream processor are set up in acyclic pipelines and input streams are well defined
- actor frameworks do not guarantee message delivery

Reasoning About Time

- needs to look at the event timestamp
- the timeline of interest process in a few mins might need to read a year worth of events. The result needs to be deterministic

Event time versus processing time **[KEY]**

- processing time might be delayed, causing unpredictable message orders
- monitoring service restart could cause the event counts dropped 0 but sudden spike after it comes back

Knowing when you’re ready **[KEY]**

- cannot tell if all the events in a window has been received
- while coming with statics of a time window, for straggler events that have a big delay, they can be 1. ignored in the window but emit a metric and alert when it becomes too many, 2. publish a correction
- can publish a special message indicates there will be no more events before a timestamp, so the consumers can trigger windows

Whose clock are you using

- end devices could publish very out-of-date events
- the event time should be the timestamp on the end devices, but end device clock is not trustworth
- to adjust incorrect device clocks, can log 3 timestamps: 1. when the event occurs on the device, 2. when the event reachs the server according to the device, 3. the server timestamp. Substract 3 and 2 to calculate an offset **[KEY]**

Types of windows **[KEY]**

- Tumbling window: fixed length. Each event belongs to only one window
- Hopping window: fixed length. Windows can overlap. e.g., 5-min window with 1-min hop size. Provide smoothing. Calculate the 1-min windows and aggregate them
- Sliding window: no fixed boundaries. As long as two events occurred with in the interval, they are in the same window. Use a buffer and remove old events to calculate
- Session window: no fix duration. Group all the events for a user til the user is inactive for some time

Stream Joins

- events can come anytime so join is harder than in batch jobs

Stream-stream join (window join) **[KEY]**

- use case: calculate the click counts of web pages returned by a search query. Search is an event, each web page click is another event
- the click could arrive before the search query event. Need to join a click for a search within an hour
- cannot embedded the search info into the click event, since if there is no click event in a search, then the search won't appear
- stream processor tracks all the events in the last hour indexed by session id

Stream-table join (stream enrichment) **[KEY]**

- enriching the activity events with information from the database: given user info in a DB, and a stream of user activities, output a stream with single user scoped activities
- quering DB for each event is slow. The stream processor can maintain a copy
- but batch job can use a snapshot of the user DB, stream processor is long running so it cannot. Need use CDC to capture user DB changes
- join between the user activities stream and user profile changes stream

Table-table join (materialized view maintenance) **[KEY]**

- twitter: maintain a timeline cache per user. events are:
  - a new tweet adds to followers' timelines
  - delete a tweet removes from all timelines
  - a new follower gets the tweets
  - unfollow removes tweets
- join tweet stream with follow relationship stream

Time-dependence of joins **[KEY]**

- slowly changing dimension (SCD): concurrent events on different streams would affect the join result based on which event is considerred as the first one, cause the result non-deterministic
- using a unique identifier for a particular version of the joined record to solve the issue: when an event occurs, give it an id, and put the id into the events from the other stream, so the join is determinitic, but log compaction cannot be done afterwards

Fault Tolerance

- exactly-once semantics/effectively-once **[KEY]**: even retries occur, the results are seem like only processed once
- for stream process, task won't be finished, so won't be able to retry like batch process

Microbatching and checkpointing **[KEY]**

- microbatching: used in Spark Streaming. Batch size is 1 sec. Smaller batch introduce more scheduling and coordination overhead, while larger batch make the results have longer delay
- tumbling window with batch size length, based on processing time not event timestamp
- Apache Flink uses rolling checkpoints of state. When crash, discard changes after crash and restart from last checkpoint. Checkpoints are generated by barriers in the message stream
- But if the outputs are written outside the system, for example external DB, then cannot discard changes

Atomic commit revisited

- need to make sure all outputs and side effects of processing an event take effect if and only if the processing is successful
- Apache Kafka implements the atomic distributed transaction by managing both state changes and messaging within the stream processing framework

Idempotence

- other than using distributed transaction to make sure an event is only processed once successfully, another approach is using idempotence
- for operations that are not naturally idempotent, adding some extra metadata. In Kafka, each message has a offset, and record the processed offset **[KEY]**
- Fencing might needed during failover to prevent a node loses network connection but not dead

Rebuilding state after a failure

- keep checkpoint state in remote DB, or keep state locally but periodically replicate. The state change can be sent through CDC
- for states that can recompute fairly quick, no need to replicate

#### Chapter 12. The Future of Data Systems

##### Data Integration

- in complex applications, data is often used in several different ways

###### 1. Combining Specialized Tools by Deriving Data

- search indexes are not very suitable as a durable system of record, so normally stored in a different system

a) Reasoning about dataflows

- Need to be clear: 1. where is the data source, 2. where is each representation derived from, 3. what places do the data need to be, in what format
- shouldn't have multiple source of authorities for a data, otherwise there could be write conflicts **[KEY]**
- If there is an order on the events in one place, other representations can use the same order
- need maintain deterministic and idempotent for fault torlerance

b) Derived data versus distributed transactions

- Distributed transactions decide on an ordering of writes by using locks for mutual exclusion. use atomic commit to ensure that changes take effect exactly once
- CDC and event sourcing use a log for ordering. Can have deterministic retry and idempotence **[KEY]**
- derived data systems are updated async, so don't support read-your-own-write **[KEY]**
- XA is high cost so need a better distributed transaction protocol. Use log-based derived data is a direction

c) The limits of total ordering **[KEY]**

- to generate total ordering event logs, requests need to go through a single leader node. But if throughput is higher than a node capacity, need to partitioning to multiple nodes
- mullti geo servers have seperate leader nodes in each DC
- microservices design is to deploy each service and its durable state independently, so events across services have no defined orders
- applications that maintain client side states could have different orders of events from the server side
- need to have consensus algorithms to support geographically distributed services, as total order broadcast is based on consensus

d) Ordering events to capture causality

- when events for different objects have causal link, total orderring is important
- logical timestamps can provide orderring without coordination needed, but that requires additional metadata and the receiptant needs to have logic to deal with out-of-order deliveries
- if log an event to record the state that a user saw, later events can refer to this event, then the causal dependency can be captured
- conflict resolution algorithms can help to deal with out-of-order delivery, but not help if the actions have side effects (like send an notification)

###### 2. Batch and Stream Processing

- batch and stream processings can emulate each other. But if implement one with the other, the performance might be poor. e.g., microbatching may perform poorly on hopping or sliding windows **[KEY]**

a) Maintaining derived state

- Batch process is using functional flavor: using deterministic functions whose output depends only on input and not affect other outputs
- derived data systems are run async to provide fault torlerance
- secondary indexes often cross partition boundaries. Async makes them more reliable and scalable

b) Reprocessing data for application evolution

- Stream process is low latency, while batch process allows large amounts of accumulated historical data
- reprocessing is needed for maintaining the system when changes occurs, e.g., schema is changed
- Derived views allow gradual evolution: can maintain old and new schemas side by side

c) The lambda architecture **[KEY]**

- an architecture to combine batch and stream processing together
- read-optimized views are derived from incoming data
- stream processor quickly generates an approximate update to the view use fast approximate algorithms
- batch processor generates a corrected version uses slower exact algorithms
- but it needs additional resources. If using the same system that can run in dual mode, then it is hard to debug
- merging outputs of two systems could be complicate when the outputs are from complex operations
- reprocessing all the historical data could be time consuming

d) Unifying batch and stream processing

- required features are
  - replay historical events that stream processor already processed
  - Exactly-once semantics even when fault occurs
  - Tools for windowing by event time. Processing time is meaningless when reprocessing historical events

##### Unbundling Databases

- Unix vs. relational databases information management problem philosophies
  - unix: logical but low level hardware abstraction
  - DB: high level abstraction hiding the data struction complexity, concurrency, crash recovery, etc.

###### 1. Composing Data Storage Technologies

- DB features: secondary indexes, materialized views, replication logs, full-text search indexes

a) Creating an index

- need first create index on a snapshot of the DB, and then process writes after the snapshot
- the process is similar to setup a new follower replica, and bootstrapping CDC in a streaming system

b) The meta-database of everything

- batch, stream, ETL process can act like subsystems of a huge DB to keep index/materialized views up to date
- can composed them into a cohesived system
- Federated databases/polystore: unifying reads. Provide uniform query interface
- Unbundled databases: unifying writes: synchronize writes across disparate technologies. Following Unix: communicate through a uniform low-level API (pipes), and that can be composed using a higher-level language (shell)

c) Making unbundling work **[KEY]**

- can implement unbundled DB writes by using an asynchronous event log with idempotent writes
- it is loose coupling between the various components
- async makes the system robust
- unbundled systems can be managed by different teams easier

d) Unbundled versus integrated systems

- unbundled system won't retire individual DBs
- the complexity of runnnig different systems, learn them, config them, ops for them are high. So unless really needed, use a single system would be better

###### 2. Designing Applications Around Dataflow

- database inside-out approach: compose specialized storage and processing systems with app code
- dataflow languages have overlay with the unbundled idea

a) Application code as a derivation function **[KEY]**

- create a secondary index is a native feature for a lot of DB
- but creating full text search index, machine learning model, cache, etc. are not easy to be implemented
- DBs can provide hooks to custom code for those requirements

b) Separation of application code and state

- DBs cannot be easily deployed. No support for dependency management, version control, rolling upgrade, evolvability, monitoring, metrics, calls to network services, and integration with external systems **[KEY]**
- because DB couples app code with data durable together
- stateless services are easy to manage because nodes can be removed at any time
- observer pattern: get noticed when data changes

c) Dataflow: Interplay between state changes and application code

- dataflow: renegotiating the relationship between app code and state management
- app code responds to state changes in one place by triggering state changes in another place
- maintaining derived data is not the same as asynchronous job execution (MQ)
  - derived data need keep the event order
  - derived data cannot have event loss
- stream processors can be used for mantaining derived data, with cheaper cost than distributed transaction **[KEY]**

d) Stream processors and services

- service-oriented architecture/microservice: break down functionality into services that communicate via synchronous network requests
- Composing stream operators into dataflow systems is similar, but the communication is single-directional and async

###### 3. Observing Derived State

- write path: create derived states
- create derived states are for read path
- similar to functional programming, write path is eager evaluation, and the read path is lazy evaluation **[KEY]**
- derived dataset is a trade off between the amount of work done during read vs. write **[KEY]**

a) Materialized views and caching

- only create cache of common queries (also a materialized view), so both read and write are not too expensive **[KEY]**

b) Stateful, offline-capable clients

- client/server model: clients are stateless and servers have the authority over data
- new model: offline/on-device state as a cache of state on the server

c) Pushing state changes to clients **[KEY]**

- clients can have stale cache until next poll
- server-sent events (EventSource API) and WebSockets can maintain TCP communication channels so the changes can be subscribed
- if client goes offline, using log based message broker can let the consumer not miss messages

d) End-to-end event streams

- server pushes state-change events into client side event pipeline as an extension of end-to-end write path, with low latency **[KEY]**
- instant message and online game use such real-time architecture
- but it is not widely supported by existing DBs, which are more request-response model, not publish-subscribe model

e) Reads are events too

- stream processor maintains states while performing aggregation and join
- recording read events can handle joins across different DBs easiler, and track causual dependencies and data provenance **[KEY]**

f) Multi-partition data processing **[KEY]**

- complex queries might need combine data from multiple partitions
- stream processors provide message routing, partitioning, joining which can help
- distributed RPC supports combining results from many partitions
- fraud prevention: need join a purchase with IP, email, billing, shipping address, etc.

##### Aiming for Correctness

- the old meaning of reliable and correct applications: atomicity, isolation, durability of transactions
- for better performance and availbility, those foundations are weaker
- it is difficult to determine if an app is correct while under high concurrency and faulty environment

###### 1. The End-to-End Argument for Databases

- DB provides strong safety doesn't mean app is free of data loss or corruption. The app might write wrong data

a) Exactly-once execution of an operation

- Processing twice is a form of data corruption

b) Duplicate suppression **[KEY]**

- TCP uses seq num to handle lost or dup packages
- 2PC (two-phase commit) allows a transaction coordinator to reconnect after a network fault, but cannot prevent the client side sends dup requests

c) Uniquely identifying requests

- use a request id to prevent duplicate requests. It can be a UUID or a hash of the request
- the request table can be seperated from actual transaction

d) The end-to-end argument **[KEY]**

- a general principle: The function in question (e.g., duplicate suppression) can completely and correctly be implemented only with the knowledge and help of the application standing at the endpoints of the communication system
- another question: checking the integrity of data. Using end-to-end checksums not the message system checksums

e) Applying end-to-end thinking in data systems

- one direction is to wrap up the high-level fault-tolerance machinery in an abstraction for applications to adopt
- but the abstraction shouldn't be too expansive

###### 2. Enforcing Constraints

- another common constraint is uniqness constraint
- tech used in uniq constraint can be used for other constraints like max count

a) Uniqueness constraints require consensus

- common solution is to partition the uniq value to a same node
- async multi-master replcation cannot be used

b) Uniqueness in log-based messaging

- total order broadcast is equivalent to consensus
- can use the order of logs to make consensus of which log to pick and others to reject

c) Multi-partition request processing **[KEY]**

- achieve atomicity not through atomic commit but through partitioned logs
- given a transaction a request id and log an event with all the operations in one node
- for each register involved in the transaction, log the operation with the request id. Because the data for a single register normally appears in one node, no need to worry about atomicity
- if one part of the transaction failed, retry the process. Since the request id is not changed, no need to worry about duplicates
- can use another stream processor to validate the transaction first. Only validate transaction can be committed in the request id table

###### 3. Timeliness and Integrity

- transactions are linearizable. the writer waits until the transaction is complete
- in unbundled system, it is async. The writer doesn't wait. The read can wait for a message instead **[KEY]**
- consistency is actually two requirements **[KEY]**
  - timeliness: user observe the system in an up-to-date state. If a user see an inconsist state, it can wait for the replication to happen. eventual consistency
  - integrity: absence of corruption. the derived data must be correct. Wait doesn't help if integrity is violated. Need check and repair. perpetual inconsistency

a) Correctness of dataflow systems

- event-based dataflow systems decouple timeliness and integrity
- integrity is preseved by Exactly-once/effectively-once semantics
- using a combination of mechanisms: **[KEY]**
  - the write op is present as a single message, to make it easily become atomic
  - using deterministic derivation functions to derive other state updates
  - Passing a client-generated request ID end-to-end for duplicate suppression+idempotence
  - Making messages immutable and easy to reprocess

b) Loosely interpreted constraints

- apps can use weaker notion of uniqueness
- compensating transaction: after conflicts happen, ask the customer to correct later **[KEY]**

c) Coordination-avoiding data systems

- such systems can have better performance and can be used in more flexible way, like multi-DC/leader replication
- Synchronous coordination can still be used in the system at a smaller scope

###### 4. Trust, but Verify

- rowhammer: certain pathological memory access patterns can flip bits even in memory that has no faults, for hacker to use

a) Maintaining integrity in the face of software bugs

- Even DBs can have bugs. Apps can also use DB wrongly

b) Don’t just blindly trust what they promise

- auditing: a way to find out if data has corrupted

c) A culture of verification

- one direction is to build self-validating or self-auditing systems

d) Designing for auditability **[KEY]**

- the transaction logs might not explain why those updates are made. Cannot redo the transaction because state might already changed
- in event-based systems, the user input is an immutable event, and resulting state updates are derived from the event. The derivation can be deterministic and repeatable
- can use hashes to check that event storage is not currupt
- rerun batch/stream processors and compare the results with the derived values, to check the integrity

e) The end-to-end argument again

- to check the data integrity, including more systems, fewer oppotunity for data corrpution could happen

f) Tools for auditable data systems

- signing transaction logs can make them tamper-proof
- one direction is to use cryptographic tools prove the integrity of a system
- Merkle trees: trees of hashes that can be used to efficiently prove that a record appears in some dataset **[KEY]**
- certificate transparency: a security technology that relies on Merkle trees to check the validity of TLS/SSL certificates **[KEY]**

##### Doing the Right Thing

- every system is built for a purpose
- every action has both intended and unintended consequences
- ACM Code of Ethics and Professional Conduct

###### 1. Predictive Analytics

- can make bad impact to individuals if the predict is wrong

a) Bias and discrimination

- basing decisions on data, rather than subjective and instinctive assessments could be more fair
- if there is a systematic bias in the input to an algorithm, the system will most likely learn and amplify that bias in its output

b) Responsibility and accountability

- when algorithms make mistakes, who to be response
- using static from a lot of data to predict a single case could get wrong result

c) Feedback loops

- recommendation system can lead to echo chambers in which stereotypes, misinformation, and polarization can breed, due to self-reinforcing feedback loops
- systems thinking: thinking about the entire system, not just the computerized parts, but also the people interacting with it

###### 2. Privacy and Tracking

- if the data is not entered by customers to the system, then the relationship between the customer and the service need to be clear, otherwise could have interest conflict

a) Surveillance

- examining if the data collection can be used for surveillance can help us understand our relationship with the data collector

b) Consent and freedom of choice

- user cannot give meaningful consent if they don't fully understand how their data is used
- a user might give data from another user who hasn't give consent

c) Privacy and use of data

- privacy doesn't mean keeping everything secret. It means have the freedom to choose
- even internal system needs to have control on the users' privacy

d) Data as assets and power

- should not build a system that can give the data into wrong hand even in the future

e) Remembering the Industrial Revolution

- collection and use of the data is like a pollution problem during industrial revolution and need to solve

f) Legislation and self-regulation

- Over-regulation may prevent innovation and improvements

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
- Use binary search to find closest locations

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
- Consumer group

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

## Think Software Learning

### URL Shortening Service

Functional requirement:

1. from big URL to uniq short URL
2. from short URL to big URL or not found
3. short URL is 6 chars
4. short URL is random
5. two short URLs for the same big URLs from two different users are different
6. only users with account to create the URL
7. URL with expiration date
8. monitor the health and metrics

Non-funcitonal requirements

1. highly available: 99.999%
2. fault tolerant
3. R/W ops minimal latencies
4. scalable
5. min cost possible
6. strong consistency as a whole
7. durable

APIs

1. userLogon(): return token
2. userLogoff()
3. createShortURL(userToken, bigURL, option expirationTime)
4. getBigURL(shortURL)

High level design:

- a LB in front of 3 app servers.
- distributed DB
- In-memory cache: use LRU.
  - local vs. global: local is quicker, but can cause dup copy
  - global: Memcache

Detailed Design:

- base64: (0-9, a-z, A-Z, -, _). + and / are unsafe for HTTP request.
- 6 chars, so 64^6=68.7B
- Generate short URL options
  1. Use MD5 hash of the big URL: MD5 is 128 bits and encoded to 22 chars. So 16 chars would be dropped
  2. zookeeper to get a range of counter: need to maintain another service
  3. Use key-generation service (KGS)
  4. store a counter in DB, each app server reserves a block of numbers until exhausted, then get another block. Need a lock to make sure no race condition
- use 3 DB machines with replication

DB design

- Use NoSQL to store: 1. URL mapping, Users
- Document type NoSQL can search by secondary key
- Need a transaction to update the short URL mapping and the user URL count together
- If the NoSQL DB doesn't provide transaction, use a persistent queue

Scale the datastore

- partitioned by the short URL. Options: 1. Hash based, 2. Range based. Hash based is better because the URLs are a counter value

Purge the DB

- Option 1: only delete an expired URL when GetBigURL() call
- Option 2: run a background jobs. That needs the DB to support range query

Global cache vs. Local Cache

- to prevent DDoS, can add an entry in the global cache to say the URL hasn't have a map yet

### Distributed Cache Design

Purpose:

- DB caching
- Storing User Sessions

R/W Policies

1. Cache Aside: writes go to DB. Cache is filled when read. For system that not updated after write
2. Read Through: app only read from cache
3. Write Through: two approaches
   1. write to cache the DB at the same time. If write to DB succeed, return succeed and retry on cache asyncly
   2. write to cache and let cache write to DB. Return succeed only when cache and DB both are succeed
4. Write back/behind: write to cache and cache write to DB async (normal periodically). Can have data loss and out of order updates
5. Read/Refresh Ahead: use machine learning to load cache prior to user access. Used in a write-heavy system using eventual consistency

Cache Eviction Policies

- LRU: data access again and again
- LFU: quickly a lot of elements are accessed. To avoid a recent added data get removed, it needs to set a time-bound
- MRU (Most recently Used): Used in read ahead
- FIFO

Requirements

1. user can insert a key-value record
2. retrieve a record by key if it exists
3. invalidate a record
4. specify the cache read policy (by default cache aside)
5. specify the cache evitction policy (by default LRU)

Non Functional requirements

1. highly available and fault-tolerant
2. performant: minimal latencies
3. scalable
4. affordability
5. can be configed to be strong or eventual consistency: if the cache content require updating. But strong consistency could cause write failure due to cache node unavailable

API

- insert(key, value)
- get(key)
- delete(key)

Design

- a server with hashmap and a double linked list
- use replicas to distribute the read load
- use partitions by hash key value range

Partitioning of Key-Value Data

1. by key range: not evenly distriuted. Good for range search
2. by hash of key: a good hash makes the data uniformly distrubted

Write partition approaches

1. single-master/leader writes: replicates to secondary. select leader logic is hard. Whether the replicate is sync depends
2. multi-lead writes: replicate writes in a mesh
3. leaderless writes: no particular order enforced

CAP: consistent, available, partition-tolerance

- R+W>N

Secondary replica do not evict entries due to LRU. It waits for primary to notify it to evict

- secondary use TTL to make sure even the primary evict doesn't get replicated, the data is still gone

Topology

- can have a lot of logical partitions (1k) so data move can be easier
- mmechanism
  1. app talk to topology manager to get replica, then write to primary replica. Upgrading the cache SDK would be hard to maintain compatability
  2. a thin SDK talk to proxy layer like LB. The LB maintains the topology. LB redirect the R/W traffic to the replicas

### Twitter

Function Requirements

1. post tweets
2. size of a tweet is 140 chars
3. user can delete tweet but cannot update (a write)
4. favorate tweets
5. follow another user
6. two type of timelines: 1. the last N of his tweets; 2. popular tweets of the user is following
7. search tweets based on keywords
8. create account
9. add pic, videos
10. analytics load, health, functionality
11. others: recommendation, replying, sharing, notification, trending

Non-functional requirements

1. highly available
2. generate timeline within half a sec
3. eventual consistency is fine: see a twitter a little late is fine, search index is built async. But user should see her just posted tweet
4. high scalable
5. user data durable

APIs

1. postTweet(token, tweetData)
2. deleteTweet(token, tweetId)
3. like/UnlikeTweet(token, tweetId, like)
4. followUser(token, followUserId)
5. readHomeTimeline(token, pageSize, option pagetoken)
6. readUserTimeline(token, userId, pageSize, opt pageToken)
7. searchTweets(token, search_terms, max_count, pageToken, sortType)

High level design: all of them have LB

- Tweet service: receive, forward tweets to timeline and service services. Store tweet info, nums of tweet from a user, user likes, etc.
  - Tables: Users: isHotUser; Tweet: SK UserId, CreationTime; Favorite_Tweet: likeTime
  - Generate tweet id: option 1: use UUID, 2: use user id append a monotonically increasing counter.
  - use write through cache because tweet is read heavy. If write to cache fails, still return success if write to DB is succeed
  - scaling up: partition cache and datastore. User table shard by hash of userId. Tweet table shard by userId can cause hotspot. Add more replicas to the hotspot partition. Can throttle user who creates too many tweets. Can move old tweet to cold storage. Shard by tweet id makes get tweets for a user across partitions. Two layer sharding: first by userId and then by tweet id.
  - find liked users: op1: run a scatter-gather on all partitions. op2: build a secnary global index on the tweenId.
- Social graph service:
  - followUer(), readUserFollowers(), readUserFollowee()
  - use write through cache
  - scaling: op1: shard by followeeUserId, op2: dup the data in both follower and followees' partition. Write is slower but read is quicker
- User timeline service: only uses app servers and distributed in-memory cache.
  - A linked list of user tweets and other user's info. Only keep 100 tweets in user timeline. Use LRU. Can preload before user logon.
- Fanout service: two queues using Kafka
  - first queue gets each posted tweet, put it in the search service and store it. Then pass it to the second queue.
  - seems like the shard is different: the first one shard by user id, the second one shard by follower id and have a copy of the message for each follower. use consistent hashing
  - fanout service is async, so cannot use for user timeline because it needs strong consistency
  - hot user has more than 10K followers. Their tweets don't add to the second queue
- Home timeline service: also in cache with linked list. When remove tweets, need to rank the tweets
  - prefer tweets that are recently liked, shared, replied or searched
  - based on followees' reputation
  - celebrity users' tweets: queried when home tineline is rendering the list
- Search service: ingester -> search index (talking to datastore with cache) -> blender -> to user performing search queries
  - ingester: tokenizing tweets. Remove the stop words like "a". Then stemming to reduce the inflected
  - serch index: create inverted index. the storage stored creationTime. op1: Shard by work, makes the ingester needs to write to a lot of partitions in a transaction, also create a hotspot; op2: shard by tweet id: causing high load for search; op3: shard by two levels: words and tweet ids
  - Blender: also fact in the tweet metadatas. Cache so other users with same search terms can get the results
- Database and caching layer: not a single component
- Analytic service

### Ride-Sharing Service

Function requirements

1. Account: Passenger: create a account with credit card. Driver: driver verification
2. passenger see her location and nearby drivers
3. passenger request a ride: pick up location and destination address
4. passenger see ETA
5. passenger see previous trips
6. driver notify uber for available for a ride
7. nearby driver reveive a request for a ride
8. when driver accept, passenger see the ETA when driver arrive
9. driver and passenger can see location when trip connected
10. driver marks the trip as complete
11. driver see driver past trips
12. driver and passenger rate each others
13. analytics/monitoring
14. uber pool
15. different types of rides

Non-function requirements

1. highly available
2. passenger can get a driver assigned within 30 sec threshold
3. strongly consistent for the trip, rates can be eventually consistent
4. scalable
5. durable

API

1. getMap(apiKey, centerLocation, long mapSize, MapType type): raster or vector map
2. getLocation(apiKey, address)
3. getTravelPathWithETA(apiKey, startLocation, endLocation)
4. requestTrip(userToken, currentLocation, destinationLocation) -> tripId
5. getPendingTripRequest(userToken)
6. setDriverStatus(userToken, status)
7. acceptTrip(userToken, tripId)
8. getTripStatus(userToken, tripId): status: pending, waiting, onGoing, completed, cancelled
9. cancelTrip(userToken, tripId, CancelType type, cancellationReason): driver cannot call this API
10. completeTrip(userToken, tripId)
11. updateLocation(userToken, currentLocation, optional tripId)
12. rateUser(userToken, userId, rating)
13. getTripsInfo(token, pageSize, token, sortType): tripType, tripStatus, passenger info, driver info, ETA for the trip

High level design

1. Map service:
   1. raster map: satellite image at various resolutions. Stored in BLOBs with spatial info recognize map area
   2. vector map: road maps with form of coordinates, edges, intersections. Store in GeoJSON/graph DB
   3. convert GPS to address
   4. calculate ETA: Geographic information system. Point of interest (POIs). Use graph algorithms and daily driver data to get the ETA. Divide the city into segments with Points of Entries and Exits.
2. routing service: routes requests between users and BE. Dispatch messages betwen user apps and uber services
   1. passenger app sends a trip request, go to trip service and respond back a trip id.
   2. after passenger app requests a trip, waits for the driver assignment notification
   3. driver app starts, report location and wait for trip requests
   4. passenger app starts, see nearby drivers
   5. Op1: use HTTP long pool. Op2: Web sockets
      1. Op1: two types of web servers with a distributed cache: 1. server farm for the coneections, 2. dispatch servers route requests from internal services to the web servers. Cache has the look up table to find where is a request hold. User Connection table: PK: UserId; Host, CreationTime, LastPingTime
      2. Op2: instantiating a bi-direction connection by upgrade the HTTP request. It doesn't time out so need less handshake. Instead of use route robin, LB can listen to the connections eash server holds.
   6. If not use routing service, some services need to wait until user send some requests before they can send response to them
3. trip service: 3 services
   1. dispatch service: query driver location service to get nearby available drivers, then rank using K-Nearest Neighbor search. If the driver doesn't response with in 3 sec or reject the trip, her rank is affected and the request send to the next driver
   2. recorder service: gets trip updates from the driver app and stor the trip time series locations. a table for trip info shard by tripId, secondary index by passenger id and driver id. Another table for timeseries
   3. history service: sharing the datastore with dispatch and recorder services. Sol1: partition secondary index (driver id, passenger id) by document (trip info) - local index: partitioned locally; Sol2: global index for driver id. Search trips by driver is quicker but write is slower. Need a distributed transaction.
4. driver service: driver location. Trip service use it to find nearby drivers
   1. Not stored in DB
   2. data structure: a table driver_location: currentGuidId, previousGridId, latitude, longitude
   3. Quick lookup: a table grids: quadtrees, R-trees, Kd-trees, etc.
   4. C-squares: divide the map into rectangular grids with ids. Store drivers as a double list with guid id as the key
   5. parititon by the range of grid ids first, then shard by product type. No need to update if the driver is still within the same grid.
   6. When find nearby drivers, query nearby grids. if not find enough drivers, query larger set.
5. billing service
6. driver/passenger info service: keep track of user rating. stateless app server with cache. Shard by user type then user id.

### Whatsapp Messenger

Functional requirements

1. send and receive messages
2. group chat with 2-256 users
3. user can track the status of the message
4. contacts are online
5. regiester the account with phone num
6. analytics/monitoring
7. receive pending messages when online
8. push notification when offline
9. keep message history for 60 days
10. send media files
11. E2E encryption
12. web app support

Non-func requirements

1. available
2. min latency
3. aync message delivery
4. durable

APIs

1. registerAccount(apiKey, userInfo) -> token
2. validateAccount(apiKey, userRegToken, validationCode)
3. initiateDirectChatSession(token, otherUserId, initialHandshakeInfo) -> chatSessionId: info: initial set of keys to exchanged for a secure connection
4. sendMessage(token,sessionId, message) -> messageId: combination of sessionId + timestamp
5. receiveNewMessages(token, sessionId, lastTimestamp)
6. initiateGroupChatSession(token, groupInfo): info contains groupId, sessionId, num of users, etc.
7. addUserToGroup(token, groupSessionId, userId, initialHankShakeInfo)
8. removeUserFromGroup(token, groupSessionId, userId)
9. promoteUsertoAdmin(token, groupSessionId, userId)

High level design

1. Routing service: using web-sockets. Connection created when client app starts
2. group: maintain group membership. Groups table and GroupMembership table shard by groupId. Group table stores SessionId. Membership table has userId as secondary index. Op1: local index, Op2: global index. Because writes to group membership is very few, global index is better. It can be updated async
3. session: store session metadata and messages
   1. Session table shard by sessionId. Op1: SessionId is a UUID; Op2: hash UserIds.
   2. Session_message table: Store senderId, receipantId, messageId combine sessionId and timestamp. Shard by sessionId. Timestamp is a unique monotonically increasing integer. Op1: use a nanosec time along with a counter. When collide, retry; Op2: Use a seq number that is stored in Session table; Op3: Use DB seq num.
   3. message type: message to a single recipient; message to group; message to a single recipient with deliverred status; status message to a group
   4. Session service sends message to Fanout service
   5. Receipient sends a last messageId to request for more message
   6. background processes running once a week to delete records older than 2 months
   7. Need to validate the sessionId the client passed in
4. fanout: a distributed message queue. Agents are listening to the queues. Shard by sessionId. If routing service says user is unavailable, send to push notification service. The client app gets the event and query session service to get the new messages and sorted with the time order.
5. user: store user info and publick keys. Primary key is the phone num. Shard by userId.
6. user registration: stateless app servers with DB and cache. RegAcount() API also creates a validation code sent through SMS.
7. push notification: use external push notification servers, i.e., Google Cloud Messaging (GCM). On the client, there is a daemon connecting to the server. Reg the app to it, which generates a token for that device and forward to Push notification service. The push notification server records the recipientId to token map. When push notifications, the second notification overrides the first one, so the app needs to query the push notification service to get all messages. Also store a table for receipentId+sessionId to creationTime map.
8. SMS Gateway
9. DB and caching
10. Analytics

### Online Dating Service - Tinder

Geosocial network

Func requirements

1. sign up account
2. upload at most 5 pics
3. provide user profiles based on criteria
   1. distance
   2. gender
   3. age ranges
   4. race
   5. interests, activities, career paths, etc.
4. swiping right for like
5. unliked profiles not show again within a period
6. two users like each other, they both receive a notificaction. Communicate via chat messages
7. monitor
8. video chat with each other

Non-func requirements

1. available
2. scalable
3. real-time experience when swipe
4. profiles are durable
5. eventual consistency

APIs

1. updateSearchPreference(token, searchQueryPreferences)
2. getProfiles(token, count)
3. rateProfiles(token, listOfProfileRatings): not make a call for every swipe

Design

1. Routing: use web-socket. Also used to init chat sessions once two user profiles matches
2. Chat
3. Swipe: client app store like info and periodically send it to swipe service. A MQ to push notification service. Matched_Users table. PK is user pair and user1 is alphabetically less than user2. Data: creationTime, user1Liked, user2Liked. The liked info can be archived in cold storage and doesn't count any more after certain time.
4. Search: types of perferences
   1. Hard preferences: distance, gender, age range. Used to filter out.
   2. Soft preferences: activities, interests, career path. Use to rank
   3. much active users get more chance to show up
   4. data is geo sharded using grids. Grid size is the max distance range that can be searched. Can also use Google S2 or Quad tree.
   5. USA data can store on the same machine as India data, as peak time is different
   6. database schema: table 1: User_Profile: PK: userId, data: currentGridId, previousGuid, LastLoginTime, Age, Gender, interests (JSON). Use secondary indexes and multi-level sharding. LastLoginTime is used in rank. Uses Elasticsearch with Lucene inddices. Use bloom filter (if an element is definately not in a set or maybe in) to not send seen profiles again. table 2: store search results for caching. Store LastLoginTime, SearchResult and BloomFilter. This table is periodically updated in the background. More the user login, more the table updated. A MQ is to queue the run. The cache policy is read-ahead polcity with MRU. The search is batched for multiple users. Use ML to categorize: content-based and collabortive filtering methods.
5. User Profile: also store user search preferences. Shard by user id. Opts to store images
   1. in the same data store as Blob: when update one field, all the fields need to be rewrite and replicate. Image can be updated in a transaction. Won't have orphaned files. The system can use a single seciruty model.
   2. in a file server and store location in the DB: need to maintain the durable and deletion
   3. in object storage service like S3: use Content delivery network (CDN)
6. User Reg + SMS gateway
7. Push Notification
8. Analytics

### Netflix

Func req

1. Users: 1. Content providers, 2. Viewers/subscribers
2. content provider can upload a video
3. content provider can provide metadata
4. users can watch a video
5. search for a video
6. rate a video
7. recommend videos based on watch history and rating
8. keep track of all the analytics of a video

Non-func req

1. available
2. scalable
3. min latency
4. durable and reliable

APIs

1. uploadVideo(token, videoMetadata, videoContent): video can be private. return 202
2. updateVideoMetadata(token, videoId, videoMetadata)
3. deleteVideo(token, videoId)
4. listVideos(token, pageSize, pageToken): content provider calls this API
5. searchVideo(token, searchTags, pageSize, pageToken): tags include different keywords. return thumbnails
6. getWatchHistory(token, count)
7. getVideoRecommendations(token, count)
8. getVideoInfo(token, videoId)
9. getVideoStreamSessionUrl(token, videoId): return the stream session URL that can retrieve the video stream
10. rateVideo(token, videoId, rating)

Open Connect Appliances (OCA): to not overloading the ISP, uses CDN (content delivery network) within each ISP network. Those CNDs are OCAs

Design

1. Gateway service: used for TLS termination, dynamic routing, stress testing, analytics, static response, rate limiting, multi-region resiliency
2. Content Provider Profile: composite by a LB, app servers, distributed cache, distributed data store.
   1. account table: capture company info
   2. user table: capture departments info in the company. Shard by accountId.
3. Video service: composite of two services.
   1. Video playback service: store metadata, video segment info, rating. Shard by video id.
   2. Cache-control service: ensure OCAs receive the videos. Write-behind/back cache policy to avoid write load on data store ??.
      1. ContentDeliveryAppliances table: PK: CNDId, data: name, lastStatus, Region
      2. VideoCacheStatus: PK: VideoId+CNDId, SK: regionId, URI
      3. CDN periodically check cache control service to see if a new video is available, then a small set of CDNs across regions downloads from video playback service with a random delay. Other CDNs can retrive from those CDNs.
      4. When customer wants to watch a video, request come to video playback service with rejion info. It check cache-control service to find a list of near CDNs. Client find a best one. If no CDN available, playback service notify cache control service to upload video to certain CDNs.
      5. client app informs the network and supported bitrates. OCA use adaptive bitrate streaming based on HTTP to provide best experience. If network is good, start from low bitrate, increasing to better bitrate if network is good. Since over HTTP, pure on client and no diffculties travering firewalls and NAT devices. No need to persist state.
4. object storage
5. Upload service: to object-storage service. Once upload is done, put event in the queue to notify post-processing service. Use multi-part upload to increase throughput and quick recovery.
   1. table: uploadId, videoId (get from video service, also give video service a state)
6. Post processing service: 1. break the video into 3-10 sec apart and put in another queue, 2. dequeue and transcoding to support different devices, also include DRM info, 3. send a message through another queue to video service and notification service, notify the content provider and update the video state.
7. Search service: work on the video metadata to get different keywords and build inverted search index. Shard by keyword makes search faster but write slower. Can also has hot keyword. If shard by video id, can have long tail latency amplification due to the scatter/gather.
8. User profile: user_interaction table: PK: userId+videoId, video rating and watch history including watched sec. History can be cleaned off after 6 monthes.
9. Homepage generation service: for a user, get recommendation videos + newly released videos. Also listen to videos that users watched.
10. Recommendation service: results passed to home page generation service. A lot of agents to run the recommendation algorithms periodically. Can generate the list once a week. No need to process all history of a user. Use map-reduce + pipeline.
    1. Collaborative filtering: build a model from users' past behavior. Recommend videos from videos viewed by other users with similar rating. Use KNN algorithm and Pearson correlation. Challenges: users with less rating have a large and sparse filtering and cold start problem for a new video.
    2. Content-based filtering: keywords describe the videos. Build user profile to indicate the type of items users likes. Use TF-IDF (weighted vector space representation). Bayesian classfiers, cluster analysis, decision trees, articial nerual networks.
    3. Popularity based/trending video: for new users who don't have a lot of history/rating.
11. Billing Service
12. Push notification

### Dropbox

Func req

1. accounts: free vs. premium
2. user can create root folder (a.k.a, namespace) and map it to devices. Changes sync to all devices
3. max file size is 1GB. It is artificial limit that can be changed.
4. share files or folders. Changes in shared folders auto synced to all other users' devices.
5. share folder shares all sub folders. But no matter if it is a subfolder in the home user, it is under root folder of other users.
6. shared folder can be R/W or read-only
7. support storing files up to 1GB and limited by the storage capacity
8. allow offline file changes. Sync as soon as online again
9. extended requests: file covery and version history. op1: any updates create a new ver. op2: daily
10. analytics stats of all storage and network consumptuion
11. resolve conflicts
12. data security use encrptuion
13. search

Non-func req

1. available
2. scalable
3. minimize network bandwidth consumption
4. min file transfer latency
5. ACID:
   1. Atomicity: use a temp file first
   2. consistency: no partially changed files
   3. isolation: multiple devices updating a same file should not cause conflicts
   4. durability
6. eventual consistency

APIs

1. uploadFile(token, metadata, content): metadata includes crypto hash of the file
2. updateFileMetadata(token, fileId, metadata)
3. updateFile(token, fileId, changeMetadata, fileSegments): only changedParts.
4. deleteFile(token, fileId)
5. listFiles(token, rootFolder, pageSize, pageToken)
6. shareFileOrFolder(token, fileOrFilderId, otherUserId, sharingPermission)
7. stopSharing(token, sharedNamespaceId, optionalUserId)
8. listSharedNamespaces(token)

Design

1. client-side app: monitor a folder, replicate changes
   1. Chunker: 4MB. Use cryptographic hash (SHA-256) to identify if file has been changed. Chunk size determined by internet connection (slow device can use multi-part upload/download). underlying block storage tech. file size to upload (should keep chunk count less than 1024).
   2. indexer: get notified by watcher. Use Chunker to identify modified chunks. Update local metadata database. Inform synchronizer.
   3. watcher: reg to OS change notifications. notify indexer about file/folder actions.
   4. internal metadata DB: stores: file name, file size, chunk sizes, chunk # and location, cryptographic hash of each chunk
   5. Synchronizer: upload/download only changed chunks in parallel. Queries sync service about newly added files from other devices. Get all the chunks, then copy the local file and switch in atomic fashion. Op1: Periodically polling the sync services, Op2: use Http Long pool, Op3: use Web Socket.
2. gateway service
3. synchronization service: maintains a file changelog for every namespace. Uses change notification queues. All devices with those namespaces have agents listening to the queues.
   1. Namespace_ChangeLog table: PK: namespaceId, Timestamp, data: CreationTime, type (Folder create/del, File create/del/updated), userId, data (the changed blocks).
   2. If device is offline, go through notification service
   3. Chunks are send to sync service then upload to the block service
4. File/Folder metadata: folder hierachy and sharing info.
   1. Namespaces types: Home namespace, Shared namespace: the shared folder in the owner's account; Proxy namespace: the shared folder in other users
   2. Namespace table schema: PK: namespaceId, type, UserId; Data: parentNamepaceId; When shared a folder with multiple users, each user has one proxy namespace with the same namespaceId but different type
   3. Namespace_objects table schema: store file and folder metadata. Each is an object. Ways to store hierachical data:
      1. Parent reference adjacency list model: each item contains a pointer to its parent. Root folder has null parent. Data stores ChunkCount, LastModifiedTime, LastModifiedByUserId. Shard by userId.
      2. Child refernece adjacency list model: has ChildrenIds as CLOB. Only folder has this field. Has upper limit. When one child update, the whole list updates.
      3. Materialized path model: store all the parents on the path. Easy for finding nodes by partial paths. Can create index on it. But if folders can be moved, with this model, all children need update
      4. Nested sets model: for each folder, visit it twice. Record its pre-order ids as left and right. Good for find subfolders but inefficient for modifying folder structure
      5. Closure table: only supported in SQL
   4. When share, a background workflow first creates a shared namespace in home user, then a proxy namespace for each target user. Then traverse all the files and folders to change the namespaceIds. Lookup the permission for those files are easier then. Stop sharing deletes the shared namespace.
5. User & Devices: user table and device table are shard by userId.
6. Notification Service
7. Object Storage
8. Chunk/Block service: keeps track of chunk history. Versions are kept up to a certain configuraable threshold.
   1. Database schema: Op1: chunks info of a file stored as a JSON in the Chunk_Information table. An update to a file creates a new info. Op2: individual chunk record stored separately. Stores nextChunkIds for easy traverse. Getting a new version of a file is harder, because only some chunks in the file is updated, then need to have the ability to use topology sort to find the chunks for a specific version
   2. data deduplication: don't store duplicate chunks
9. Billing

### Distributed Message Queue

Components

1. sender or producer
2. Receiver or Consumer
3. Message broker
4. Filter: 1. topic based filtering: publisher define the topic, 2. content based filtering: subscriber classify the message

Delivery models in distributed message queues

1. at least once delivery: could be delivered more than once. Consumer needs to implement idempotent
2. at most once delivery: highest throughput available. Need keep state and an ack mechanism
3. exactly once delivery: almost impossible, high overheads.

Types

1. Based on num of cunsumers:
   1. P2P MQ: only a single consumer can get a specific message. Could have multiple consumers. Absolute order cannot be guarantee because receivers can process in different speed. Message can have states: 1. READY, 2. PROCESSING, 3. PROCESSED, 4. WAITING, 5. EXPIRED
      1. single sender single receiver
      2. multiple senders single receiver
      3. single sender multiple receivers
      4. multiple senders multiple receivers
   2. Publish-subscriber MQ: multiple consumers can receive the same message for a topic. Exact order can be guarantee. Subscriber doesn't need to ack. If subscriber fails to get a message, can replay the stream from the point of the failure
      1. single publisher
      2. multiple publisher
2. Based on the order of consumption:
   1. FIFO Queue: used when order is critical or cannot tolerate dups. Need consumer to ack to make sure not miss a message.
   2. Standard Queue: provide best-effort ordering. At least once dilivery. Compare to FIFO, it can have multiple active consumers. Once a consumer ack a message, the message is removed.
3. Based on Message durability
   1. Non-Durable queue: in-memory only queue. OK to lost events but high throughput. Future events will cover lost events. At most once delivery.
   2. Durable queue: durability level varies.
      1. Single node queue: still can have data loss if disk fails
      2. Mirrored queue: one master and multiple mirrors. One mirror synchronizely update, others async.
      3. Quorum queue: 1 primary and 2+ seconary. When writes succeeded on quorum machines, write succeed. Used when fault tolerance is more important than latency.

Purpose:

- Loose coupling
- Better performance via async communication
- User responsiveness
- Increased reliablity
- Handling traffic spikes: queue-based load leveling pattern
- Implementing SAGA transaction: when all operations succeed, succeed, if failed, keep retrying until threshold, then all rolled back. Used for heterogeneous data store scenario. Need use compensating transaction.
- Competing consumers, ordering guarantee and concurrency control: competing consumer pattern

Func Requirements

1. publish message to a logical channel (topic)
2. a publisher is responsible for the topic lifetime: create a topic and delete it
3. the publisher publishes messages to a topic
4. a consumer read messages from a topic
5. multiple consumers can subscribe to a topic
6. consumers are responsible for keeping track of the read positions
7. an upper limit to individual message size
8. delete messages in a topic after expiration time or reach certain size limit

Extended func requirements

1. The publisher defines the number of publisher instances and the expected publising rate, or defines the # of partitions
2. The MQ guarantees FIFO order of messages from a publisher instance. Doesn't guarantee order between multiple publishers
3. Consumer can query # of partitions and allocate a suitable # of instances to read from the topic
4. Support multi-tenant architecture: users in a tenant share a common access with specific privileges. Different customers are isolated from each other

Non-func req:

1. highly available
2. minimal latencies
3. highly scalable
   1. introducing topics: each topic isolate from others
   2. introducing multiple partitions in a topic. Consumers can define consumer groups so each consumer instance can read from a partition
4. afforablity
5. strongly consistent
6. durable

APIs

1. createTopic(accessKey, publisherId, topicProperties: publisherInstanceCount, rateOfMessages, otherProperties)
2. deleteTopic(accessKey, publisherId, topicId): async, touch multiple partitions
3. getTopicInformation(accessKey, topicId)
4. publishRecords(accessKey, topicId, publisherId, listOfRecords): support batch call to reduce overhead
5. readRecords(accessKey, topicId, partitionId, lastRecordPosition, count)

Detailed Design

1. Control plane: LB, app servers, distributed cache, distributed data store
   1. configs: security (authN, authZ, encryption), resources, data retention
2. data plane: each node hosts multiple partitions that belong to different topics
   1. proxy/routing layer: talk to topology sync
   2. message broker cluster
   3. message durability: can use one of
      1. database based persistence layer: SSTable is better than B/B+-tree
      2. file-based persistence layer: kafka uses it. Each file has an offset records a range of the messageIds based on partitionId.
3. topology synchronization: Zookeeper

### Payment Gateway System

Card Network Association (Scheme): Visa, master card

Main functional requirements

1. charge customer without being PCI-DSS compliant
2. merchant can reg an account
3. charge and refund
4. view transaction details
5. support one-time/periodic purchase
6. securly store payment info

Extended functional requirements

1. generate invoice
2. report in a time period
3. multiple ways of payments (debit, credit)
4. audit support
5. discount coupon

Non-functional requirements

1. highly available and fault tolerant
2. highly reliable: lack of, dup, incorrect, dangling authZ
3. scalable
4. durable
5. strongly consistent
6. security

Merchant APIs:

- `createCheckoutSession(mechartId, mode, lineItemDetails, successUri, failureUri)`
- `expireSession(sessionId)`
- `getCheckoutSession(sessionId)`
- `listCheckoutSession(optoinal mode)`

Services:

- Merchant Profile Service: cache, 3 app servers, distributed DB. Shard by user id hash
- Checkout page generation service: storage a page for each sessionId; store logo, CSS and validate CSS for a CSS style. 3 webservers, distributed cache, datastore. Object storage service.
- Checkout session service: store the merchant, payment details and webhooks. call to page generation service. insert a transaction record in the pending queue for the payment processor service. Can store one-time session and period session in two table
- Payment processor service:
  1. get a transaction from the queue, send to external payment procesor adapter for auth,
  2. move the trans record to completed queue when external processor authZ/decline the trans
  3. external payment service provider (PSP) might not be using the trans id due to id space, generate temp id.
  - Use a ACID compliant DB. also have a retry queue. Dead letter queue (DLQ) is used for message that cannot retry and order doesn't matter
  - for idempotency: use local DB. need send the trans id to PSP when retry. Either get the previous response or get a dup error
  - state: pending, authz, denied, failed. if authz succeed, there is authz code
- Payment settlement service: consumer of the completed queue. Store authzed payment in DB. Workflows perform settlement EOD
  1. group by payment provider type (Visa, Master card, etc.)
  2. write transaction info and authZ code in a file
  3. use SFTP to transfer the file to the scheme for settlement
  4. once received a file with settled transaction details, insert records to the settled transaciton queue
- Transaction Service: keep track transaction. consume from completed transaction and settled transaction queues. Store data to the same ACID DB for transaction. Call the Webhook that is reg by merchant. Idempotent.
- Webhook callback service: The webhook for the same trans can be called multiple times, so merchant needs to be able to idempotently reg to the callback. Also need to retry with expoential backoff. Can have 2 solutions
  1. listen to a transaction notification queue. Not use completed transaction queue is to make sure the transaction service already get the message
  2. Use FaaS
  - For security, the webhook endpoint needs to be HTTPS, and also put a signature in the header with a key pair. The Key Management Service generates the key.
  - to avoid replay attack, add the timestamp in the header
- Payment profile service: store customer's payment info. Decrypetion keys store in Key management service. The keys used by this service are not shared by other services. Store PK profile id, data: CreationTime, EncryptedData, keyId, keyVer.
  - tokenization: use a non-sensitive equivalent data (the token) to replace the data
- Key management service: handle encryption keys.

- Auth phase
- clearing and settlement phase

Stripe makes the customer no need to be PCI-DSS compliant

- checkout session:
  - inputs: 1. mode: one time, recurring, future charge. 2. lineItemDetails: detail of the transaction

## System Design Interview: An insider's Guide

### Ch1: Scale from Zero to Millions of users

Mobile app: Use HTTP + JSON for API response.

NoSQL: CouchDB, Neo4j, Cassandra, HBase, Amazon DynamoDB

- Key-value stores
- graph stores
- column stores
- document stores

Load balancer + database replication

- better performance
- reliability: data persist
- High availbility: single machine offline

Cache (Memcached)

- advantage
  - better performance
  - reduce DB workloads
  - can scale cache tier independently
- read-through: server reads from cache first, then read from DB and save to cache
- considering
  - Read heavy
  - Expiration policy: not too short but not stale
  - Consistency
  - One cache server become SPOF
  - Eviction policy

CDN

- cache static contents
- Consideration
  - cost: charged for IO
  - cache expiry
  - fallback
  - Invalidating files by increase version
- Stateless web tier: move state out from web tier (user session data, etc.) to DB so all web server can access

Data centers

- GeoDNS: split traffic based on geo. Challenges to solve are:
  - Traffic redirection
  - data sync
  - test and deployment
- Decouple components for scale up. Can use message queue.

Message queue

- can scale producer and consumer independently

Logging, metrics, automation

- error logs: use a centralized service or monitor at server level
- metrics: CPU, Memory, disk I/O, DAU, retention, revenue
- automation: CI

Database scaling

- shards challenges
  - resharding data: consistent hashing
  - celebrity problem: dedicate shard
  - join: use de-normalization

### Ch2: Back-of-the-envelope Estimation

Power of 2

- 1 byte: 8 bits. 1 ASCII char.
- 2^10: 1 KB
- 2^20: 1 MB
- 2^30: 1 GB/Billion
- 2^40: 1 TB/Trillion
- 2^50: 1 PB/Quadrillion
- long: 4 bytes
- float: 4 bytes
- double: 8 bytes
- Media: 1 MB

Latencies

- ns < us < ms
- L1 cache ref: 0.5 ns
- Branch mispredict: 5 ns
- L2 cache ref: 7 ns
- mutex lock/unlock: 100 ns
- main mem ref: 100 ns
- compress 1KB: 10 us
- send 2KB through 1 Gbps network: 20 us
- read 1MB from mem: 250 us
- round trip in same DC: 500 us
- disk seek: 10 ms
- read 1MB from network: 10 ms
- read 1MB from disk: 30 ms
- send packet across DC in the same geo: 150 ms

Conclusions from latencies

- avoid disk seeks if possible
- compression is quick so compress before send data through internet
- cross region DC is slow

Availability numbers

- 99% = 14.4 mins downtime per day
- 99.99% = 8.64 sec
- Peak QPS: 2 * normal QPS

### Ch3: A Framework for system design interviews

Avoid:

- over engineering
- ignore tradeoffs
- narrow mindedness
- stubbornness

4 steps

1. Understand the problem and establish design scope:
   1. can make assumption
   2. what features?
   3. how many users?
   4. how fast to scale up?
   5. what existing services to leverage?
2. Propose high-level design and get buy-in
3. Design deep dive
4. Wrap up

### Ch4: Design a rate limiter

Requirements

- server vs. client side: client side is unreliable. Can also be a middleware: API Gateway.
- based on IP/userId
- standalone service vs. baked in the app
- inform throttled client? return 429

Algorithms

- token bucket: bucket size, refill rate/sec. Different bucket for different API
- leaking bucket: bucket (queue) size, outflow rate (req/sec)
- Fixed window counter: has edge burst problem.
- Sliding window log: log timestamp of the earlier request in the window. Consume a lot memory.
- Sliding window counter: use percentage. smooth out spike but not so precise.

Design

- Use in-mem cache (redis) to store counters for user + IP
- when exceed rate limit: 1. return 429, 2. queue for process later
- rate limiter header: X-Ratelimit-Remaining, X-Ratelimit-Limit, X-Ratelimit-Retry-After
- Rules are stored on disk and loaded in cache

Distributed env

- race condition: can solved by Lua script or sorted set
- sync issue: use centralized data store

Performance Optimization

- multi-DC/edge server
- eventual consistency

Monitoring

- rate limit algorithm effective but not too strict
- rule effective

Others

- hard vs. soft rate limit: whether to allow exceed threshold for a short period
- at different network stack levels

### Ch5: Design a consistent hashing

Consistent hashing: when a hash table is re-sized, only k/n keys need to be remapped

- SHA-1’s hash space goes from 0 to 2^160 - 1
- Hash server based on server metadata
- Hash key, clockwise find the closest server to store the data

Virtual nodes

- interleave virtual nodes for servers

### Ch6: Design a key-value store

A short key works with better performance.

Requirements

- a key-value pair size: < 10KB
- automatic scaling
- Tunable consistency
- High availability, high scalability, low latency

Single server

- Data compression
- store freq used data in mem and others on disk

Distributed

- CAP: Consistency, availability, partition tolerance
- choose C over A: block write when replication fail

Components

- Data partition: evenly and minimize data move.
- Data replication: use the next N servers to store replication
- Consistency: quorum consensus: W+R>N guarantee strong consistency
- inconsistency resolution
- handling failures
- system architcture diagram
- write path
- read path

Consistent hashing adventages

- capable with auto scaling
- Heterogeneity (not evenly distributed): higher capability servers can have more virtual nodes

Consistency models

- Strong consistency: never sees out-of-date data. a replica not to accept new R/W until every replica has agreed on current write
- Weak consistency: subsequent read operations not see the most updated value
- Eventual consistency: after all updates are propagated, and all replicas are consistent. Need reconcile inconsistent values

Vector clock

- [server, version] pairs. D1([s1, v1]), D2([s2, v1]) reconcile on s1: D3([s1, v2], [s2, v1])
- When no conflict == an ancestor when versions in a version clock are >= versions in another clock
- has conflict == sibling when some versions > but some versions <.

Handle failures

- failure detection: need at least 2 independent sources to mark a server down
- Gossip protocol: each node maintain heartbeat table from other nodes, and send the table to a set of random nodes.
- sloppy quorum: R/W on the first servers, offline servers are ignored.
- hinted handoff: Another server process requests temporily until the offline server is back.
- A hash/Merkle tree: non-leaf node has hash for all child nodes. Used for verify contents of large data structures.
- anti-entropy protocol: keep replica in sync. comparing each piece of data on replicas and updating.

System architecture

- Decentralized: A coordinator can be any node acts as a proxy between client and the service

Write Path

- requests persist on a commit log file
- data is saved in the mem cache
- when mem cache is full, flush to SSTables on disk

Read Path

- Check if data in mem cache
- Use bloom filter to find which SSTable contains the data

### Ch7: Design a unique id generator in distributed system

Requirements

- id needs to be uniq and sortable
- id is num fits in 64-bit
- id increase over time
- 10k id generates per sec

multi-master replication: in total k servers, on a server, increase the generated id by k every request

universally uniq id: use 128-bit num to have low probability of collision

ticket server

twitter snowflake approach

## System Design Interview The Big Archive

### Data base isolation level

- Serializable
- Repeatable Read
- Read committed
- Read uncommitted

## CI/CD

<https://about.gitlab.com/topics/ci-cd/>

- Build
- unit test, Integration test, Regression test
- deploy
- Need a version control system

## Open Questions

How distributed lock works (ZooKeeper)

How cache works? (when to update TTL)

What are the QPS of our services

Services: SignalR, Gossip

How token works (OAuth2)

OOD Design Patterns

### Redis pub sub model

<https://redis.com/glossary/pub-sub/#:~:text=How%20Pub%2FSub%20works,of%20their%20particular%20use%20cases.>
