# Elements of Programming Interviews
On [Google Play](https://play.google.com/books/reader?id=y6FLBQAAQBAJ&printsec=frontcover&output=reader&hl=en&pg=GBS.PP3)

## Introduction
### An interview problem
Interview steps: 
1. Clarify the problem: input/output format? 
2. Brute force way and its time and space complexity.  
3. Find a better solution: D&C  
4. Test  

Maximum profit in single share of stock:  
- Find the max and min value: wrong. 
- Brute force: T(n)=O(n^2) and S(n)=O(n). 
- Divide-and-conquer: min in first subarray and max in second subarray. T(n)=2T(n/2)+O(n)=O(nlogn). 
- Tracking the previous min: T(n)=O(n). S(n)=O(1).  

In a 45-60" interview: 
- develop algorithm
- implement
- test
- analysis complexities

### Book organization
(!!skiped!!)  

### The EPI Editorial Style
(!!skiped!!)  

### Level and prerequisites
- data structures and algorithms  
- locks, distributed systems, OS  
- dynamic programming, graphs, greedy algorithms  

## The Interview

### Chapter 1
(!!skipped!!)

### Chapter 2
(!!skipped!!)

### Chapter 3
(!!skipped!!)

### Chapter 4
Data structure  
(!!skipped!!)

## Problems
### Chapter 5
In C++ integer is 32 or 64 bits, while in Java it is always 32 bits. So not hard code bit number.
- can call `sizeof(an_int) * CHAR_BIT` in `<climits>` to find out how many bits it has
- or `while(an_int) {++bit_num; an_int >>= 1;}` to find out the place of the last bit that is one

For Primitive types, need to know:
- size: char is 4 bit/1 byte
- ranges: char is -128 to 127
- signedness: unsigned short is 0 to 65535 while short is -32768 to 32767. How it influence shifting.
- operators: char + char return a short. Espacially bitwise operators.
- functions in `<cmath>`: `abs`, `fabs`, `ceil`, `floor`, `min`, `max`, `pow`, `log`, `sqrt`.
- `<random>`: `uniform_int_distribution<> dis(1, 6)`, `uniform_real_distribution<double> dis(1.0, 6.0)`, `generate_canonical<double, 10>`
- `~0`: if short, it is -32768
- create masks
- clear the lowermost set bit, get its index
- using cache to accelerate
- use commutativity and associativity to operate in parallel or reorder operations.
- max or min values of primitive types: `numeric_limits<float>::max()`, `numeric_limits<double>::infinity()`
- compare floats
- interconvert int, char and string: `x - '0'` convert a char into int

#### 5.1
Compute parity of a 64-bit binary word.
- Brute force: walk through the number by `&1` and `>>=`. Stop when `x == 0`,  Then modula 2. T(n)=O(n), where n is the first 1 to the end.
- <b>Notice</b>: `x&=(x-1)` remove the last 1 bit. T(n)=O(n), where n is the number of 1 bits.
- Bit fiddling computation: 1. process multiple bits; 2. cache results in an array-based lookup table. Need to find a reasonable length of cache. 16-bit is reasonable because 0xFFFF is easy to fit in memory. T(n)=O(n/L), n is the length of word and L is the length of key.
- <b>Notice</b>: Order of XORs doesn't change the result. Parity of a number equals to XORs between its parts. T(n)=O(logn).

#### 5.2
View a 64-bit integer as an array, LSB as index 0, MSB as index 63. Swap the bit at i and j.
- Brute force: first check if two bits differ. If differ, flip them(XOR 1). T(n)=O(1)

#### 5.3
Reverse a 64-bit word from LSB to MSB.
- swap 0-31 bits with 32-63 bits. Good if only need run once. T(n)=O(n)
- use a lookup table, record all 16-bits numbers and its reversed numbers. T(n)=O(n/L), n is the length of bits, L is the key length.

#### 5.4
Find the closest number that has the same weight(same number of set bits) as the given number.
- Brute force: try (x-1), (x+1), (x-2), (x+2) until reach a same weight number. T(n)=O(2^n)
- Swap the first two consecutive different bits from LSB. T(n)=O(n).
- T(n)=O(1) solution: use `x^(x>>1)` to find out the last different bit.<b>Notice</b>: cannot do `x^(x<<1)` because shift in a 0 from right.

#### 5.5
Compute x multiple y with assignment, bitwise operators, boolean combination.
- Brute force: repeat addition. T(n)=O(n^2) 
- Decimal multiplication with shift. T(n)=O(n^2).

#### 5.6
Compute quotient between x and y use only addition, subtraction and shifting.
- Brute force: repeat subtract y from x until less y.
- Substract `2^k*y` from x each time. T(n)=O(n), n is the bit number of x.

#### 5.7
Given double x and integer y, compute x^y. Assume won't overflow or underflow.
- When y is nonnegative, brute force: x multiple itself for y-1 times. T(n)=O(2^n), where n is the bit number of y.
- Recursive: x ^ (1010)base2 = x ^ (101)base2 * x ^ (101)base2. T(n)=O(n). When y is negative, multiple (1/x)^(-y).

#### 5.8
Reverse digits of an integer, but leave the sign unchanged.
- Brute force: change to string and change back. T(n)=O(logn).
- Modulo 10. T(n)=O(n), where n is the number of digits.

#### 5.9
Check if a decimal integer is a palindrome.
- Brute force: return false if the integer is negative. Then convert the integer to string. T(n)=O(n), S(n)=O(n).
- log10(x)+1 is the number of digits. Use this to create a max significent digits mask. T(n)=O(n), S(n)=O(1).

#### 5.10
Generate uniform random numbers between a and b.
- use smallest i that let 2^i-1 > b-a. Then call random zero-one generator i times. If out-of-bound, retry. T(n)=O(lg(a-b+1)). 

#### 5.11
Find rectangle intersection. Rectangles are parallel to x-axis. Need ask if boundary counts. Here it counts.
- Draw 5 squares to see how rectangles intersects. Use up-left vertical and height, width to present a rectangle. T(n)=O(1)

### Chapter 6
Array:
- contiguous block of memory
- Insertion into an array take O(n) time and O(n) space because of the operation that copy array into a new array(array resizing).

C++
- `array<int, 3> A = {1, 2, 3};`
- `vector<int> sub_array(A.begin() + i, A.begin() + j);`
- `vec.push_back(42)`, `vec.emplace_back(42)`
- Understand what "deep" means when checking equality of arrays, and hashing them. <b>?</b>

C++ algorithm:
- `binary_search(A.begin(), A.end(), 42)`
- `lower_bound(A.begin(), A.end(), 42)`: the first element that is not less than 42
- `upper_bound(A.begin(), A.end(), 42)`
- `fill(A.begin(), A.end(), 42)`
- `swap(x, y)`
- `min_element(A.begin(), A.end())`
- `max_element(A.begin(), A.end())`
- `reverse(A.begin(), A.end())`
- `rotate(A.begin(), A.begin() + shift, A.end())`: shift become the first element
- `sort(A.begin(), A.end())`

#### 6.1
Dutch National Flag program. Quick sort with equal band in the middle. Notice when use `size_t` as index, cannot do `for (size_t i = A.size(); i >= 0; --i)`. If so, when i is 0, then `--i` still larger than 0 and out of bound.
- Brute force: swap all numbers larger than pivot after i to before i. and traverse i through 0 to end. Then from the end reversely do again. T(n)=O(n^2), S(n)=O(1).
- Quick sort, first recursive from the begining, and then from the end. T(n)=O(n), S(n)=O(1).
- Quick sort one pass. Maintain smaller, equal, unsorted and larger four bands. Note when swap between larger and current, current should not move forward. T(n)=O(n), S(n)=O(1).

Variant:
- Four keys: not traverse from begin and end, but set all pointers from begin.
- Two keys remain one keys in order: do traverse from the end. notice the swap-to keys are always remain in order, while swap-from could be messed up.
```
1. TF<a>FF<b>T
2. TTF<b>FF<a>
```

#### 6.2
Increment an arbitrary-precision integer. E.g. Input {1,2,9}, Output {1,3,0}
- Brute force: convert the array into number. Then convert back. Could overflow. T(n)=O(n)
- Use a carry-out. T(n)=O(n).

Variant:
- two string addition: digit wise operate.

#### 6.3
Multiple two arbitary-precision array presented integers. Negative number has `-` in the first cell.
- Use reversed vector to record input numbers. Add carry-out to the previous cell. T(n)=O(n^2).

#### 6.4
Advancing through an array. Integer in array indicate how far it can go from this cell. Check if it is able to reach the end.
- Iterate through the array, record the furthest position it can reach from current cell. T(n)=O(n), S(n)=O(1).

Variant:
- Output the min steps to reach the end: DP. Use the step number that reach current cell to update all cells in the range as step+1. T(n)=O(n^2), S(n)=O(n).

#### 6.5
Delete duplicates from a sorted array
- Use a pointer to indicate the insert position, and another one to traverse. T(n)=O(n). S(n)=O(1).

Variant:
- remove all occurance of a key from an array: two pointers. Note array may not be sorted. T(n)=O(n), S(n)=O(1).
- make all elements in a sorted array show no more than m times: Use a counter. Need reset it correctly. T(n)=O(n), S(n)=O(1).

#### 6.6
The max profit can make when buy and sell stock once
- Tracking the current seen min value, update max profit using current-min. T(n)=O(n), S(n)=O(1).

Variant:
- Find longest subarray that all entries are equal: a pointer point to the start point of qulified subarray. T(n)=O(n), S(n)=O(1)

#### 6.7
The max profit can make when buy and sell stock twice
- Brute force, traverse through all singer day, compute previous max profit and next max profit. T(n)=O(n^2), S(n)=O(1).
- Dynamic program, record max profit frontward and backward. Then add them together and find the max profit. T(n)=O(n). S(n)=O(n).

Variant: 
- Solve in O(n) time and O(1) space <b>?</b>

#### 6.8
Enumerate all primes to n
- Brute force, trial division: divide each i from 2 to sqrt(i) to see if i is prime or not. T(n)=O(n^(3/2)), S(n)=O(1).
- sifting approach: Keep an array where the index means the number. Set false to all multiples of each prime number. T(n)=O(n/2+n/3+...)=O(nloglog(n)). S(n)=O(n).
- Notice each time when sift, all non-prime numbers between i to i^2 are already sifted. So no need to check again. Time complicity <b>?</b>

#### 6.9
Permute the elements of an array
- Rearrange the array by the permute. Brute force: copy and paste back. T(n)=O(n), S(n)=O(n).
- Permutation are cylics, which means several elements are go in cycle, and an element will always finally put into the start cell of a cycle. So if find an number in permutation is not the start of a cycle, it means this cycle is already done. T(n)=O(n), S(n)=O(1).
- Set permutation to be negative number after cycled. T(n)=O(n), S(n)=O(1). 

Variant: 
- Given an array A of integers representing a permutation, update A to represent the inverse permutation using only constant additional storage. <b>?</b>

#### 6.10
Compute the next permutation
- Brute force: present the permutation as a number, increase it until it is a permutation again. T(n)=O(n^10), S(n)=O(n).
- From the end, seach backward, find the first element that is smaller than the next one. Then find the first element backwards that is larger than it. Swap them, and then reverse all elements after the swap point. T(n)=O(n), S(n)=O(1).

Variant:
- Compute the kth permutation: k-1=2^a+2^b+2^c. Means the ath element should be at the first place. Remove a, then the bth element should be at the second place.
- Previous permutation: reverse steps.

#### 6.11
Sample offline data
- Get elements one by one. T(n)=O(k), S(n)=O(1).

Variant:
- Does `rand() % n` return uniformed distributed number between o and n-1? yes.

#### 6.12
Sample online data
- Record all read-in data, in size of n. Then random pick k data from it. T(n)=O(nk), S(n)=O(n).
- First get k data from read-in data, then when a new data comes, random remove one from the previous set. T(n)=O(n), S(n)=O(k).

#### 6.13
Compute a random permutation
- Brute force: random pick an element. If already picked, discarded, otherwise add to the permutation. T(n)=O(nlogn) known as Coupon Collector’s Problem, S(n)=O(n).
- Swap the picked value to the front, and pick random one in the left over part. T(n)=O(n), S(n)=O(1).

#### 6.14
Compute a random subset with k elements from 1~n
- Brute force: random pick one element and move to the front. T(n)=O(k), S(n)=O(n).
- To avoid store all 1 to n, use a hash table to record each swap. T(n)=O(k), S(n)=O(k).

#### 6.15
Generate number in a set that are not uniform distributed
- Split 0~1 into spans that are follow the distribution. T(n)=O(logn) by using binary search. S(n)=O(n).

Variant:
- Generate number follow exponenical distribution <b>?</b>

#### 6.16
Sudoku checker program, 0 means empty
- Check line by line, row by row, and block by block. T(n)=O(n^2), S(n)=O(n).

#### 6.17
Compute the spiral ordering of a 2D array
- Iterate solution. Be aware of corner case. Consider matrix size 3*3, 4*4, 3*4, 3*5, 4*5, 5*3. T(n)=O(n^2), S(n)=O(n^2).
- Use a direction vector. T(n)=O(n^2), S(n)=O(n^2).

Variant:
- Generate a matrix in spiral order: fill in the blanks using spiral orders.
- Use a sequence to generate spiral order matrix: same as above.
- Write a program to enumerate the first n pairs of integers (a,b) in spiral order, starting from (0,0) followed by (1,0). <b>?</b>
- m*n matrix: done. 
- Compute the last element in spiral order of a m*n matrix: same as below.
- Compute the kth element in spiral order in O(1) time: find a relation between offset and the first element of each offset.

#### 6.18
Rotate a 2D Array 90 degree clockwise
- Brute force: assign a new 2D array and copy to it. T(n)=O(n^2), S(n)=O(n^2)
- In place rotate 4 elements each time. T(n)=O(n^2), S(n)=O(1).
- Can also reassign pointers. Create a class for this special kind of matrix, if read/write (i, j), return pointer to (j, size - i - 1). T(n)=O(1), S(n)=O(n^2).

Variant:
- Reflect a 2D array horizontaly: line by line. 

#### 6.19
Compute rows in Pascal's triangle
- Use previous row. T(n)=O(n^2), S(n)=O(n^2).
- Use C(m,n), T(n)=O(n^3) based on C(m,n) time complexity, S(n)=O(n^2).

Variant:
- Compute nth row use O(n) space: only record previous rows.

### Chapter 7
Palindromic string: read same when it is reversed.
```
bool isPalindrome(const string &s) {
    for (int i = 0, j = s.size() - 1; i < j; ++i, --j) {
	    if (s[i] != s[j])
		    return false;
	}
	return true;
}
```

String type is immutable. Alternatives that are mutable include char array, StringBuilder in Java.

C++ string library:
- `append("Gauss")`
- `push_back('c')`
- `pop_back()`
- `insert(s.begin() + shift, "Gauss")`
- `substr(pos, len)`
- `str1.compare(str2)`: return int. 0 means two strings are same. Negative means char in str2 is smaller or str2 is shorter. Positive vice versa.
- `str1 < str2`: `==` test logic equality not pointer.

#### 7.1
Interconvert strings and integers (stoi)
- Digit by digit. Be ware of 1. 0, 2. INT_MAX, 3. INT_MIN, 4. overflow. T(n)=O(log10(n)), S(n)=O(log10(n)).

#### 7.2
Base conversion from b1 to b2
- First convert from base b1 to base 10, and then convert from base 10 to base b2. T(n)=O(n+nlogb2(b1)), S(n)=O(nlogb2(b1)).

#### 7.3
Compute the spreadsheet column encoding
- 26 base to decimal. Notice A mapping to 1 not 0. T(n)=O(n), S(n)=O(n).

Variant:
- A correspond to 0: result - 1.

#### 7.4
Replace and remove. Replace 'a' with 2 'd', and remove 'b'
- First pass remove 'b' and count final length, second pass replace 'a'. T(n)=O(n), S(n)=O(1).

Variant:
- Replace special characters with their names: first pass compute the length, then modify string. 
- Merge two sorted arrays: compare two pointers.

#### 7.5
Test palindromicify ignoring case and special characters
- Two pointers from beginning and ending. T(n)=O(n), S(n)=O(1).

#### 7.6
Reverse all the words in a sentence
- Do it in two pass. First pass reverse all words inplace. Then reverse the whole sentence. T(n)=O(n), S(n)=O(1).

#### 7.7
Compute all mnemonics for a phone number
- Each digit mapping to 3 or 4 characters. Use recursive. T(n)=O(n*4^n), S(n)=O(4^n).

Variant:
- without recursive: use for loops <b>?</b>

#### 7.8
The look-and-say problem <1, 11, 21, 1211, 111221, ...>
- do it n times. T(n)=O(n*2^n), S(n)=O(n).

#### 7.9
Convert from roman to decimal: roman number is IVXLCDM in non-increase order, with exceptions that each number can be before the next two numbers, which means destract. 
- Add current number. If find a number smaller than the next one, destract it twice from the result. T(n)=O(n), S(n)=O(1).

Variant:
- Check if the roman number is valid: not increase in two characters.
- return the shortest valid roman number: divide by larger roman number first. If the result is 9, then in increase order, otherwise decrease order.

#### 7.10
Compute all valid IP addresses
- each part should be between 0 to 255, and cannot be 01. Use recursive call. Note need use all chars. T(n)=O(n^4), S(n)=O(n^4).

Variant:
- period number is k and string is unbounded: use recursive.

#### 7.11
Write a string sinusoidally
- In a snake shape. Start from middle line, go first to upper line then lower line. T(n)=O(n), S(n)=O(n).

#### 7.12
Implement run-length encoding
- encode converts a string with no digits from 'aabbb' to '2a3b', and decode do it reversely. T(n)=O(n), S(n)=O(n).

#### 7.13
Find the first occurrence of a substring
- Brute force: two for loops. T(n)=O(mn), S(n)=O(1).
- Rabin-Karp: Use a rolling hash on each substring. Update this rolling hash need O(1) time. T(n)=O(n+m), S(n)=O(m).

### Chapter 8
Singly/Doubly linked list.

Sentinel node.

```
template <typename T>
struct ListNode {
    T data;
	shared_ptr<ListNode<T>> next;
};
```

Operations:
- search: T(n)=O(n)
- add: T(n)=O(1)
- delete: T(n)=O(1)

Use a dummy head to get rid of checking empty list.

C++ librarys
- `list`: doubly linked list
- `forward_list`: singly linked list
- `push_front()`, `emplace_front()`
- `pop_front()`
- `push_back()`, `emplace_back()`
- `pop_back()`
- `list1.splice(iter, list2)`: add list2 to the next of iterator in list1
- `list1.plice_after(iter, list2)`: for `forward_list` to use
- `reverse()`
- `sort()`
- `insert_after(iter, ele)`, `emplace_after(iter, ele)`
- `erase_after(iter)`: delete one element
- `erase_after(iter1, iter2)`: delete between iter1+1 and iter2.

#### 8.1
Merge two sorted lists
- Pick the smaller node in two lists to add to the new list. T(n)=O(n), S(n)=O(1)

Variant:
- Merge two doubly linked lists: not forget about prev pointer.

#### 8.2
Reverse a single sublist
- Move each node start from start node to the next of finish node. T(n)=O(n), S(n)=O(1)
- Move each node before start until reach finish. T(n)=O(n), S(n)=O(1). <b>Not implemented</b>

Variant:
- Revese a singly linked list: start = 0 and finish = end
- reverse each k nodes: multiple start and finish

#### 8.3
Test for cyclicity
- Brute force: use hash table to record all nodes. T(n)=O(n), S(n)=O(n)
- Brute force2: while one node traverse through the list, another node start from the head and try to reach the previous node. If they meet with different step numbers, cyclicity find.
- fast and slow pointers: when they met, another pointer start from beginning and move one by one, fast move one by one and when they meet, it is the start of the cycle. Prove:
```
a: length before entering cycle
b: length when fast and slow meet start from the start of cycle
c: length of cycle

2n = a + xc + b // fast move distance, x unknown
n = a + yc + b // slow move distance, y unknown
n = a + yc + b = (x-y)c
(x-2y)c = a + b
```
So start from b point, move a steps must reach the start point of cycle.

#### 8.4
Test for overlapping lists—lists are cycle-free
- Use hash table to has all nodes of a list, and traverse another list. T(n)=O(n), S(n)=O(n).
- Brute force: two loops. T(n)=O(n^2), S(n)=O(1).
- First count the list lengths. Then traverse the longer one first. T(n)=O(n), S(n)=O(1).

#### 8.5
Dump question. The need is too complicate that author cannot describe it clearly. Ignored.

#### 8.6
Delete a node from a singly linked list
- To update the predecessor, need T(n)=O(n) to find it, S(n)=O(1)
- Can copy data from next node and delete next node. T(n)=O(1), S(n)=O(1).

#### 8.7
Remove the kth last element from a list
- Don't count the lenth of list, but use two pointers. One move k steps first. T(n)=O(k), S(n)=O(1).

#### 8.8
Remove duplicates from a sorted list
- traverse until find another node that is different. T(n)=O(n), S(n)=O(1).

Variant:
- remove elements that appear more than m times: set a counter

#### 8.9
Implement cyclic right shift for singly linked lists
- Right shift the list (length n) by k mod n nodes. T(n)=O(k), S(n)=O(1).

#### 8.10
Implement even-odd merge
- maintain two headers. T(n)=O(n), S(n)=O(1).

#### 8.11
Test whether a singly linked list is palindromic
- Use iterative call. T(n)=O(n), S(n)=O(1).

Variant:
- check doubly linked list: find the mid point. 

#### 8.12
Implement list pivoting
- Maintain three lists. T(n)=O(n), S(n)=O(1).

#### 8.13
Add list-based integers
- LSD is the head of the list. Use a variable to record carry over. T(n)=O(n), S(n)=O(n).

Variant:
- LSD is at the tail. Reverse lists.

### Chapter 9
Stack
- Can reverse the order of a sequence.
- parsing typically benefits from a stack.

C++ librarys:
- `stack`
- `top()`: throw exception when null
- `push(42)`, `emplace(42)`
- `pop()`: throw exception when null
- `empty()`

Queue
- enqueue to the tail, dequeue from the head.

Deque(double end queue)
- use doubly linked list
- push: to the head
- inject: to the tail
- pop: from the head<b>?</b>
- eject

C++ librarys:
- `front()`
- `back()`
- `push(42)`, `emplace(42)`
- `pop()`
- `push_back(42)`, `emplace_back(42)`, only deque
- `push_front(42)`, `emplace_front(123)`
- `pop_back()`
- `pop_front()`

#### 9.1
Implement a stack with max API
- search for the max value in stack each time. T(n)=O(logn), S(n)=O(n).
- Push same or larger value to another private stack. T(n)=O(1), S(n)=O(n).

#### 9.2
Evaluate RPN(Reverse Polish notation) expressions
- Use a stack to record operants. When an operator shows, pop two operants. T(n)=O(n), S(n)=O(1).

Variant:
- solve the problem with expression in op, num1, num2 order: stack the ops.

#### 9.3
Test a string over {,},(,),[,] for well-formedness
- Check the top is match with the income char. Check at the end the stack is empty. T(n)=O(n), S(n)=O(1).

#### 9.4
Normalize pathnames
- corner case is '/' with '..'. Use a vector to perform stack operations. Note result should not start with "//". T(n)=O(n), S(n)=O(1).

#### 9.5
Search a postings list
- Traverse the list. Check jump node first. If jump to explored node, fall back to check next node. T(n)=O(n), S(n)=O(1).

#### 9.6
Compute buildings with a sunset view
- west tall buildings will block east buildings views. Somehow the dumb question says that input is from east to west. T(n)=O(n), S(n)=O(n).

#### 9.7
Compute binary tree nodes in order of increasing depth
- Return a vector that contains vectors that have node value of same levels. Use two queues to record level i and level i + 1. T(n)=O(n), S(n)=O(n).

#### 9.8
Implement a circular queue
- Use an vector to store elements. Two pointers indicate start and end. When vector is full and need enque, resize. T(n)=O(1) if not need to resize, T(n)=O(n) otherwise. S(n)=O(n).

#### 9.9
Implement a queue using stacks
- Use two stacks. T(n)=O(1) if not first dequeue, T(n)=O(n) if first dequeue when second stack is empty, O(1) for others, amortized T(n)=O(1). S(n)=O(n).

#### 9.10
Implement a queue with max API
- Maintain a queue with max value. If the next element is bigger than the tail, remove elements until the previous element is bigger or equal. enqueue: T(n)=O(n) in max, O(1) amortized. dequeue: T(n)=O(1). Max: T(n)=O(1). S(n)=O(n).
- Use two MaxStack(9.1) to implement queue(9.9). Max is the max of the dequeue stack and enqueue stack. Amortized T(n)=O(1), S(n)=O(n).

### Chapter 10
Search path: from root to a node.

Ancestor-descendant: a node is both ancestor and descendant of itself.

Depth start from 0.

- Full binary tree: all nodes have two leaves.
- Complete binary tree: all levels are filled except the last level, where all nodes are as far left as possible.
- Perfect binary tree: all leaves are at same level.

Full binary trees have N(non-leaf) + 1 = N(leaf). Perfect binary trees with height h have N(node) = 2^(h+1) - 1, and N(leaf)=2^h.

Complete binary trees with n nodes have height = floor(log2(n)).

Left-skewed tree: no nodes have right child.

- inorder traversal: left-root-right
- preorder: root-left-right
- postorder
- all of them have T(n)=O(n), S(n)=O(h), if nodes don't have parent fields.

Min height of a tree is log2(n), prefect tree. Max height of a tree is n, skewed tree.

#### 10.1
Test if a binary tree is height-balanced
- The height difference of left and right subtree is at most one. Recursive compute height, and return -1 if the subtree is not balance. T(n)=O(n), S(n)=O(h).

Variant:
- return tree height: same.
- return a node if its left and right subtree have height difference more than k: same.

#### 10.2
Test if a binary tree is symmetric
- Recursive check if the left subtree has the same right node as the right subtree left node, and vice visa. T(n)=O(n), S(n)=O(h).

#### 10.3
Compute the lowest common ancestor(LCA) in a binary tree
- traverse to the first node and record the path. traverse to the second node and see the first common node. T(n)=O(n), S(n)=O(h).
- check if the two nodes are in different subtrees of a node. If so, the node is LCA. T(n)=O(h), S(n)=O(h).

#### 10.4
Compute the LCA when nodes have parent pointers
- Compute the height of the first node and the second node, then use runner pointer. T(n)=O(h), S(n)=O(1).

#### 10.5
Sum the root-to-leaf paths present in binary in a binary tree
- Pass a variable to the child node. Sum left path and right path. T(n)=O(n), S(n)=O(h).

#### 10.6
Find a root to leaf path with specified sum
- Return true if find a path. Use recursive calls. T(n)=O(n). S(n)=O(h).

Variant:
- return all paths: use a vector to save path.

#### 10.7
Implement an inorder traversal without recursion
- Use a stack. If the left child of a node is already traversed, output this node, otherwise push it back. T(n)=O(n), S(n)=O(h).
- To not push to stack twice, and not use visited field, use nullptr wisely. Push cur into stack, and then cur=cur.left. If cur is null, it means the top of stack doesn't have left node; Now get the top of stack and push top.right. If get a nullptr from stack, it means it is pushed from a node that doesn't have right node. Skip to the next stack.top() is okay. T(n)=O(n), S(n)=O(h).

#### 10.8
Implement a preorder traversal without recursion
- Use a stack. Notice when push to stack, first push right, then push left. T(n)=O(n), S(n)=O(h).

#### 10.9
Compute the kth node in an inorder traversal
- Compute the number of nodes in the subtree of a node for each node. Then use D&Q to solve the problem. T(n)=O(h) not consider update nodes with size. S(n)=O(1).

#### 10.10
Compute the inorder successor
- Nodes have parent pointer. If this node doesn't have right child, successor is its parent; otherwise is its left most node of its right subtree. T(n)=O(h), S(n)=O(1).

#### 10.11
Implement an inorder traversal with O(1) space
- Node has its parent pointer. Use a prev ptr indicate where does traverse come from(3 cases). Use a next ptr to predict where should go based on if nodes have left/right child(2 cases). T(n)=O(n), S(n)=O(1).

#### 10.12
Reconstruct a binary tree from traversal data
- Given inorder and preorder traveral, The first node of preorder is the root, and in inorder sequence, before this node is the subtree part. T(n)=O(nlogn) because of search. Use hash table can reduced, S(n)=O(n).

Variant:
- Inorder and postorder: same.
- O(n) algorithm to build a max-tree: find the max node: find max node need O(n). <b>?</b>

#### 10.13
Reconstruct a binary tree from a preorder traversal with markers
- Use a static pointer to point to the current index of the array. Use recursive call to add nodes. T(n)=O(n), S(n)=O(1).

Variant:
- Postorder: create node, then move pointer forward.
- Can inorder be solved: No, because the first node could be a left child, but also can be a node without left child.

#### 10.14
Form a linked list from the leaves of a binary tree
- Use recursive call to traverse. If the node doesn't have left and right child, append to the list. T(n)=O(n), S(n)=O(n).

#### 10.15
Compute the exterior of a binary tree
- Simple and easy way to solve it is first find first leaf and add the path from root to it, and then add all leaves except the first one, and then add the path from last leaf to root reversely. T(n)=O(n), S(n)=O(n).
- Use a flag to indicate if it is boundary. T(n)=O(n), S(n)=O(n).

BTW: Never saw a question can be such nonsense. There is also a definition problem: "(By leftmost (rightmost) leaf, we mean the leaf that appears first (last) in an inorder traversal.)", but the program provided is actually printing boundary nodes. Even if the question is finding the boundary, when the root doesn't have left/right subtree, the solution only return half of the exterior. More details is discussed in http://articles.leetcode.com/print-edge-nodes-boundary-of-binary/

#### 10.16
Compute the right sibling tree
- level order traverse the tree using a queue. At the current level, link all next level nodes. T(n)=O(n), S(n)=O(1).

Variant:
- without updating next field, update right field: from bottom up. but what kind of brain-dead would do that?
- for a general tree: actually did in my source code.

#### 10.17
Implement locking in a binary tree
- If any of the node's ancestor or descendant is locked, this node cannot be locked. So to test if it can be locked or not, check all nodes in subtree and the path to root. T(n)=O(m+h)=O(n). S(n)=O(1).
- Use a counter to indicate how many nodes are locked in subtree. So only if this value is 0, and no nodes locked on the path to root, this node can be locked. T(n)=O(h), S(n)=O(n).

### Chapter 11
Heap: priority queue. 
- A complete binary tree that the key at each node is larger or equal to its children.
- Since it is complete binary tree, can use array to store it. The children of node i is node 2i + 1 and node 2i + 2.
- O(logn) to insert and delete, O(1) to return max, O(n) to find a node.
- Min/Max-heap is good for computing k largest/smallest elements.

C++ librarys
- `priority_queue`: default is max heap. To create a min heap, need to provide a `greater<>` in constructor.
- `push()`, `emplace()`
- `top()`: throw exception
- `pop()`: throw exception

#### 11.1
Merge sorted files
- Use min heap to store the first element of each file. Pop an element and refill with the file that contains it. T(n)=O(nlogk), where k is the number of files, and n is the number of records, S(n)=O(k), because records write to disk, not in memory.
- Can merge two files each time. T(n)=O(nlogk), S(n)=O(n).

#### 11.2
Sort an increasing-decreasing array
- The array is first increasing, then decreasing, then increasing again for k times. Sort it by first breaking it into k pieces, then use merge sort to sort k subarray. T(n)=(nlogk). S(n)=O(k).

#### 11.3
Sort an almost-sorted array
- The right position of a number is at most k away from its current position(k-sorted). Use a min-heap to record k numbers, and each time extract the min number out. T(n)=O(nlogk), S(n)=O(k).

#### 11.4
Compute the k closest stars
- Since there are 10^12 stars, RAM cannot fit. Use a k size max heap. T(n)=O(nlogk), S(n)=O(k).

Variant:
- Print the k largest number from an online stream: use max heap. Worse case is the input is always increasing.

#### 11.5
Compute the median of online data
- Use a max and a min heap. Keep both heaps have the same size. T(n)=O(logn) for each entry. S(n)=O(n).

#### 11.6
Compute the k largest elements in a max-heap
- the k elements must shows in the first k level of BT of the heap. The input is an array. Use index to determine the level. T(n)=O(klogk), S(n)=O(k).

#### 11.7
Implement a stack API using a heap
- The element is the value with a timestamp. So when pop, pop the lastest one. T(n)=O(1) for pop(), O(logn) for push(). S(n)=O(n).

Variant:
- Implement queue: use min heap. 

### Chapter 12
Binary search
```
int bsearch(const vector<int> &A, int val) {
    int L = 0, U = A.size() - 1;
    while (L <= U) {
        int M = L + (U - L) / 2; // to avoid (L + U) overflow but (L + U) / 2 not
        if (A[M] == val)
            return M;
        else if (A[M] < val)
            L = M + 1;
        else
            U = M - 1;
    }
    return -1;
}
```

Generalized search
- Forcus on tradeoffs between RAM and comparasion time.

C++ library:
- `find(A.begin(), A.end(), target)`: defined in `<algorithm>`
- `binary_search(A.begin(), A.end(), target)`: return bool.
- `lower_bound(A.begin(), A.end(), target`: the first element not less than target.
- `upper_bound(A.begin(), A.end(), target`: the first element that is greater than target.

#### 12.1
Search a sorted array for first occurrence of k
- When find k, not stop but check if left element is also k or not. If so, move upper to left to continue search. T(n)=O(logn), S(n)=O(1).

Variant:
- implement `upper_bound`: if mid <= target, goes to right, until ed <= st
- A[0] ≥ A[1] and A[n − 2] ≤ A[n − 1]. <b>?</b>
- find the first and last index of a series of value: between `lower_bound` and `upper_bound`
- find if 'p' is a prefix of a sorted string: search for string start with 'p'

#### 12.2
Search a sorted no dup array for entry equal to its index
- If a cell has element that is already larger than its index, all elements after it cannot be the answer. T(n)=O(logn), S(n)=O(1).

Variant:
- array contains duplicates: Cannot solve in O(logn). So traverse.

#### 12.3
Search smallest element in a cyclically sorted array
- if st < ed, return st; if st >= ed, if md > st, in the right; if md < st, in the left; if md == st, if md > ed, in the right; if md == ed, traverse. T(n)=O(n) if the array is same. S(n)=O(1).
- Use recursive call to deal with it is better. So when A[st] == A[md] == A[ed], check st to md, and md + 1 to ed and return the smaller value. T(n)=O(n), S(n)=O(logn).

Variant:
- An array contains two sub arrays, one strictly ascending and the other strictly descending. Find peek: check if A[md-1] < A[md] < A[md+1].
- find an element in cyclically sorted array: if A[st] < A[ed], fall back to normal BS.

#### 12.4
Compute the integer square root
- Binary search between 1 and x/2. Be careful of the case when md == st and md^2 is smaller than x. T(n)=O(logn), S(n)=O(1).
- Use st = md + 1 to change range. Use md^2 <= k as the check. So return md - 1. T(n)=O(logn), S(n)=O(1).

#### 12.5
Compute the real square root
- Since st == md won't happen, binary search with a double comparation. T(n)=O(log(x/s)), where s is torlerance. S(n)=O(1).

Variant:
- compute float x/y: if x > y, from 1.0 to x, else from 0 to 1.0, search mid * y == x.

#### 12.6
Search in a 2D sorted array
- Each row and column is nondecreasing. Divide the matrix into four matrixs. Compare the value with the up-left and down-right elements to find out which blocks(at most three) it could be in. T(n)=O(mlogn), S(n)=O(1).<b>?</b>
- Walk from top-right. If larger, move left; If smaller, move down. T(n)=O(m+n), S(n)=O(1).

#### 12.7
Find the min and max simultaneously
- Store every two elements in order as min then max, so that compare min with min and max with max to save half comparasions. T(n)=O(n), S(n)=O(1).

Variant:
- least number of comparasion required: O(3n/2)

#### 12.8
Find the kth largest element
- Randomly pick a number, seperate the array into larger or smaller than it sub arrays. See if it is the kth element. If it is larger than kth element, search the smaller sub array. T(n)=O(n) to O(n^2). S(n)=O(1).

Variant:
- find median: k = n / 2
- vector has duplicates and keep order: so dumb to trying to keep order. <b>?</b>
- mailbox <b>?</b>

#### 12.9
Find the missing IP address
- The input is in a file and is very large. To same RAM, scan it twice. First scan prefixes of all IP address and count the number of each prefix. If the count is not same as it should be, rescan the file and use a hash table(bit set) to record which has shown. Then scan the bitset to return the result. T(n)=O(n), S(n)=O(2^k), where k is the prefix length.

#### 12.10
Find the duplicate and missing elements
- One element is replaced by another element. To seperate the vector from a vector that only contains the missing A, and a vector that only contains the duplicate B, do xor for all elements lead to A ^ B. So we know which bits are different in A and B. Now search for all the elements that have that bit set, can tell us either A or B. Traverse the array to find out another one. T(n)=O(n), S(n)=O(1).

### Chapter 13
Hash table
- Collision: use a linked list.
- Lookup, insertion and deletion: T(n)=O(1+n/m), where n is the number of elements, and m is the size of the hash table storage.
- rehash: when load n/m grows large. T(n)=O(m + n), which is expensive.
- rolling hash: for strings, when remove and add a character, hash code can be computed by the previous string not start from scratch.

Anagrams: two strings contain same letters.

Multimap: multiple values for a single key.

C++ library:

`unordered_set`:
- `insert(42)`, `emplace(42)`: return `<iterator, boolean>`
- `erase(42)`
- `find(42)`: return `end()` if not found.
- `size()`

`unordered_map`:
- `insert({42, "value"})`, `emplace({42, "value"})`: return `pair<iterator, boolean>`
- `erase(42)`
- `find(42)`: return `end()` if not found.
- `size()`

`hash()` in `<functional>` header contains hash for primary types.

#### 13.1
Test for palindromic permutations
- Check if all chars are show in pairs except one. T(n)=O(n), S(n)=O(n).

#### 13.2
Is an anonymous letter constructible?
- Hash the letter character counts. Iterate the magazine, if all characters are available, return true. T(n)=O(n), S(n)=O(m), where m is the letter and n is the magazine.

#### 13.3
Implement an ISBN cache
- Key is ISBN, value is price. Insert until cache is full, then use LRU to remove entry by implement a linked list queue with hash table record key and list node.
- lazy garbage collection: when cache size is doubled, abandon half at once. This will increase lookup failure rate. 

#### 13.4

## Notation, Language and Index
### Notation
Cardinality: the number of elements

### C++11
C++ best practice:
- `deque<bool>` instead of `vector<bool>` because vector is not STL container and cannot hold vector.
- input should be either value or const reference, except `swap()`, that can use non-const reference.

C++11 constructs:
- `auto`
- enhanced range-based for-loop
- `emplace`, `emplace_front` and `emplace_back`, which are better than `insert`, `push_front` and `push_back`.
- array supports `size()` and boundary checking
- `tuple`
- lambdas(Anonymous functions) in `[]` notation
- initializer using `{}` for list

C++ for Java developers
- operator overload
- `unordered_map` => `HashMap`, `unordered_set` => `HashSet`, `set` => `TreeSet`, `map` => `TreeMap`
- `set` and `map` comparator
- `unordered_set` and `unordered_map` hash function, equal function
- iostream
- `::`
- `pair`
- `static_cast`
- `unique_ptr`: destroy when out of scope
- `shared_ptr`: has a reference count


# Notes
## Questions: 
### how does signed number shift

### what "deep" means when checking equality of arrays

## Technique:
`x&=(x-1)` remove last bit of `x` that is `>0`

Thinking about if input is sparse or random.

<b>Notice</b>: `x & ~(x - 1)` extracts the lowest set bit of x.  

<b>Notice</b>: while design an algorithm, run once and run many times affects.

template defination can not been put into source file.

`std::find_if_not (foo.begin(), foo.end(), [](int i){return i%2;} );` return the iterator of the first element that make return clause false.

subarray means elements that are consecutive; subsequence are not required to occupy consecutive positions within the original sequences.

Adding elements to the beginning of arrays are expensive. To improve the time complexity, adding elements to the end and then reverse the array.

`str.find(' ')` return `string::npos` when cannot find space. `algorithm::find(str.begin(), str.end(), ' ')` return `str.end()` when cannot find.

[Difference between push and emplace](http://stackoverflow.com/questions/26198350/c-stacks-push-vs-emplace)

`make_unique<T>(T())` return a `unique_ptr<T>`.

`std::function<Return(Args...)>` defined in `<functional>` can create a function class.

[std::function](http://en.cppreference.com/w/cpp/utility/functional/function)

[Lambda function](http://en.cppreference.com/w/cpp/language/lambda)

`const_reverse_iterator` start from `end()`, which is sential iterator, and end at `begin()`, which contains the first element.

a ^ b = c, then a ^ c = b


