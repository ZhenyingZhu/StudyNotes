## Memory
address represents in hex.

1 byte = 8 bits

`malloc(size_t)` is in unit byte

## Tree
left rotate
```
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
  - http://www.geeksforgeeks.org/avl-tree-set-1-insertion/
  - http://www.geeksforgeeks.org/avl-tree-set-2-deletion/
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
See https://github.com/ZhenyingZhu/StudyNotes/blob/master/multithread.markdown

## Callback
[src](https://en.wikipedia.org/wiki/Callback_(computer_programming))

blocking/sync callback
- Pass a function as an argument

deffered/async callback
- The callback is executed after an event happened. [src](http://softwareengineering.stackexchange.com/questions/143623/what-are-deferred-callbacks)


