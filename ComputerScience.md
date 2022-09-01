# Computer Science Domain knowledges

## Cracking the Coding Interview 6th

### Arrays and Strings

Hash table

- array of linked lists + hash code function
- resize the array when too many collision ??
- use a balalnced binary search tree ??

Resizable array

- Resize insertion amortized to O(1)

StringBuilder: a resizeable array and convert to string when necessary. Implemetation ??

String encodings

- ASCII has in total 128 chars, including control chars
- extended ASCII has 256 chars
- one char is 1 byte/8 bits
- Unicode:
  - has different char encoding: UTF-8, UTF-16, GB18030
  - In total 2 bytes. First byte: code points, represent ASCII chars

Permutation

Sort: what are the common sort methods ??

- quicksort
- mergesort

IsSubstring: what is the best algorithm ??

Delete a node

- check for null if the node doesn't exist
- update head and tail nodes
- dealloc the node

Runner technique = second pointer

Recursion

- O(n) space where n is the depth of the recursive call
- can be implmented iteratively

### Linked Lists

Singly vs. doubly linked list

### Stacks and Queues

Stack: LIFO

- OPs: pop, push, peek, isEmpty
- Can use a linked list to implement

QueueL FIFO

- OPs: add, remove, peek, isEmpty

### Trees and Graphs

ternary tree: 3 children

binary search tree:

- each node n: left descendents <= n < right descendents
- But definitions can be different so need to discuss

balanced:

- ensure O(logn) for insert and find

Complete binary tree:

- all levels are fully filled unless the last one. The last level is filled from left to right

Full binary tree:

- every node has either 0 or 2 children

Perfect binary tree:

- both full and complete. Exactly 2 ^ k - 1 nodes

Use stack to do in-order traversal ??

Use stack to do pre-order traversal ?? Also a method to relink

Use stack to do post-order traversal ??

Binary Heaps (min heap)

- a complete binary tree
- each node is smaller than its children
- ops: insert, extract_min
- insert: first add to bottom right, then swap it with its parent
- extract_min: return root, and swap it with the last element, then bubble it down

Tries (Prefix trees)

- null (*) node to indicates a complete word

Graphs

directed vs. undirected

connected graph: there is a path between every pair of vertices

a graph doesn't necessary to be a connected graph (so can be multiple isolated subgraphs)

acyclic graph: no cycles

Represent graph:

- adjacency list
- adjacency matrices: true indicates an edge from node i to j

Depth-first search (DFS): recursive and need to mark a node as visisted before recursive call

Breadth-first search (BFS): use a queue and mark a node as visisted before enqueue

Bidirectional search: to find the shortest path. Start BFS from both source and target

Topology sort ??

- <https://www.geeksforgeeks.org/topological-sorting/>
- build a graph (not necessary to be connected)
- if there is a cycle, then cannot sort
- node without income edge can be in the first. remove them
- then find new nodes that don't have income edges

### Bit Manipulation

Tricks:

- Create a mask that sets the last n digits to 0: x & (~0 << n)
- get last bit: (x - 1) ^ (~x)
- ~x = x ^ (~0)
- get bit: (num & (1 << i)) != 0
- set bit: num | (1 << i)
- clear a bit: num & (~(1 << i))
- clear bits from most significant bit to i: num & ((1 << i) - 1)
- clear bits from i to 0: num & (-1 << (i + 1))
- update a bit to x: clear bit then set bit if x is 1

two's complement representation

- convert a positive number into a negative number with greatest place value indicates the sign
- invert all bits, then +1
- complement + original number = the max number

Right shift

- Arithmetic (>>): devices by 2. It shifts the sign bit but fill in the sign again
- logical (>>>): the sign bit also shift

Decimal number: `0.101 = 1*1/2^1 + 0*1/2^2 + 1*1/2^3`. Keep multiple a real number with 2. If it is > 1, then the digit there is 1; otherwise 0

Best Conceivable Runtime (B.C.R)

Check if a number is 2 power: (n & (n - 1)) == 0

Clear last 1: n & (n - 1)

### Math and Logic Puzzles

Prime number

- every positive integer can be decomposed into a product of primes
- prime number can only be divided by 1 and itself
- prime number law: for a num x to divide y, mod(y, x) = 0, all primes in x's prime factorization must be in y's
- GCD: greatest common divisor, LCM: least common multiple. `gcd(x, y) * lcm(x, y) = x * y`
- 0, 1 is not prime
- check for primality: from 2 to sqrt(n), if n cannot devide, then n is prime
- The Sieve of Eratosthenes to generating a list of primes: cross out all num divisible by 2, then next non-crossed off number. Can only list odd nums
- given 2 prime numbers, using plus and minus, can get any values between 1 and sum of them

Probability

- Venn diagram
- P(A|B): probability of A given B is true
- `P(A and B) = P(B|A) * P(A) = P(A|B) * P(B)`
- Bayes' Theorem: `P(A|B) = P(B|A) * P(A) / P(B)`
- `P(A or B) = P(A) + P(B) - P(A and B)`
- independence: A happens has nothing to do with B. `P(B|A) = P(B)`, so `P(A and B) = P(A) * P(B)`
- Mutual exclusive: `P(A and B) = 0`, `P(A or B) = P(A) + P(B)`

Worst-case minimization problem

- nine balls, find the one ball that is heavier given a balance. In total 2 times. Divide by 3
- drop 2 eggs to find floor n (CC189 6th 6.8): find the drop strategey, that in worst case, dropping the total drops for the first egg and second egg are same

### Object-Oriented Design

six Ws: who, what, where, when, how, why

- define core objects
- analyze relationships
- Consider key actions
- design patterns: singleton and factory method

### Recursion and Dynamic Programming

Recursive solution:

- bottom up: from simple case
- top down: divide problem into sub problems. be caureful of overlap between cases
- half-and-half: binary search

Fibonacci number

- O(2^n) without DP, a tree with n as hight
- O(2n) with DP

Catalan numbers:

- when n is the num of operators
- the number of ways of parenthesizing an expression: C(n) = (2n)! / ((n+1)! * n!)

### System Design and Scalability

Key goals

- Commincate with questions. Be open about the issues
- don't get excessively focused on one part
- draw picture of what is proposing
- Validate concerns people bring up. Make changes accordingly
- Stating out and questioning all assumptions
- Estimate when necessary
- driving through questions
- tradeoffs and make improvements

Design steps:

1. Scope the problem: what features/use cases are supported?
2. Make reasonable assumptions. Need some product sense (timeout for the requests)
3. Draw major components. Start from simple case. Don't worry about scaliability challenges too early
4. Identify key issues: bottlenecks, major challenges
5. Redesign for the key issues. Be open about limitations. Might need a whole redesign, but could also be just a minor tweaking (e.g., use a cache)

Algorithms that scale:

1. figure out the real problem
2. first solve the problem in the most simple case
3. Think about how much data can fit on one machine. What can happen when split up the data? How to identify the machine for data
4. Find solutions that might remove the issue or mitigate the issue
5. New issues might occur. Use iterative approach. Goal is not to re-architect a complex system

Horizontal vs. Vertical Scaling: add more nodes vs. increase resources for a node

Load Balancer

DB denormalization: save redundant data to avoid join, so make reads quicker

NoSQL

DB partitioning/sharding

- vertical partitioning: break down colums into different tables based on features
- key(hash)-based partitioning
- directory-based partitioning: maintain a lookup table. But the table can be a SPOF and the performance bottle neck

Caching

Asynchronous processing & queues

- in some cases, can do pre-processing

Networking metrics

- 1. Bandwidth, 2. throughput, 3. latency
- Make transmit speed quicker not only short the latency, but also increase bandwitdh and throughput
- data compression can reduce the latency

Map Reduce

## Terms

- Idempotency
- Sanity check

## Memory

address represents in hex.

1 byte = 8 bits

`malloc(size_t)` is in unit byte

## Tree

left rotate

```text
    4         2
   / \       / \
  2   5  => 1   3
 / \           / \
1   3         4   5
```

1. put left child as root
2. put root as right child of new root
3. put the right subtree of new root as the left subtree of old root

AVL: Self-balancing binary search tree.

- Source:
  - <http://www.geeksforgeeks.org/avl-tree-set-1-insertion/>
  - <http://www.geeksforgeeks.org/avl-tree-set-2-deletion/>
- When insert how to balance:
  1. Find the lowest node that offend height balance
  2. if its left height is larger than right height, right rotate
  3. else if its right height is larger than left height, left rotate
- When deleting
  1. NOT COMPLETE

B-Tree: self-balancing tree, n-ary. Store keys in nodes without duplicate keys in the leaves
B+ tree: one of B-Tree. Each node contains key, but only leaves contain value

## C++ key word

`volatile`: the value can be changed from outside, so compiler load it from memory. multi-thread programming can change a variable stored in a memory in another process. But if not use `volatile` to define the variable, compiler might hard code the value

`virtual`: with it, dynamic binding; without it, static binding

`(void*)malloc(size_t)`: `free`

`void** arr = (void**)(size_t offset);` create access the memory block offset as a pointer vector, so that `arr[-1]` is actually accessing offset-1 memory.

## Smart pointer

membership:

- `T * obj;`
- `unsigned * ref_cnt;`

Constructor

- assign obj
- `ref_cnt` set to 1

Copy constructor

- point to the obj
- `ref_cnt` inc

operator=

- check if same
- dec `ref_cnt` of current obj. If `ref_cnt` is 0, delete
- assign obj
- assign `ref_cnt` to others
- inc `ref_cnt`

destructor

- dec `ref_cnt`
- if 0, release both obj and `ref_cnt`

## Multi-thread

See <https://github.com/ZhenyingZhu/StudyNotes/blob/master/multithread.markdown>

## Callback

[src](https://en.wikipedia.org/wiki/Callback_(computer_programming))

blocking/sync callback

- Pass a function as an argument

deffered/async callback

- The callback is executed after an event happened. [src](http://softwareengineering.stackexchange.com/questions/143623/what-are-deferred-callbacks)
