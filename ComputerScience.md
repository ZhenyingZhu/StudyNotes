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
