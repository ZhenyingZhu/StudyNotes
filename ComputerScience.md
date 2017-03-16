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
