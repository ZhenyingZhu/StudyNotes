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
- `insert({42, "value"})`, `emplace(42, "value")`: return `pair<iterator, boolean>`
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
Compute the LCA, optimizing for close ancestors
- traverse to root from both nodes at the same time. If find one node already in the hash map, return. T(n)=O(h), S(n)=O(h). 

#### 13.5
Compute the k most frequent queries
- Use a hash table, key is string and value is freq. Then use min heap to retrive the k largest strings. T(n)=O(n+mlogk), where n is the number of string, m is the number of uniq strings. S(n)=O(m).
- Use random pick pivot to find the most biggest k. T(n)=O(n+m), S(n)=O(m).

#### 13.6
Find the nearest repeated entries in an array
- Use hash table to record string and the index of its previous presence. T(n)=O(n), S(n)=O(m), where m is the number of disctinct string.

#### 13.7
Find the smallest subarray covering all values
- For each index i, find the first subarray that cover all strings. T(n)=O(n^2), S(n)=O(n).
- Moving window: the first subarray that covers all strings is between st and ed. Then move st and ed forward to find next subarray. T(n)=O(n), S(n)=O(n).
- When there is no duplicates of the target string, use a linked list to record all appearance of strings. So input could be a stream. Hash table records string to node mapping. T(n)=O(n), S(n)=O(m), where m is the number of target strings.

Variant:
- input is stream: second solution.
- find the shortest subarray that contains all the distinct elements in the array: two pass.
- rearrange the array so that the shortest subarray has the maximum length: put two least repeated elements at the head and the end.
- rearrange the array so that no two equal elements are k or less apart: <b>?</b>
- find the longest subarray that all the elements are distinct: moving window. When read an element that is already shows up, move the start until all this elements are excluded.

#### 13.8
Find smallest subarray sequentially covering all values
- Record 1. the shortest subarray length for each keyword; 2. the latest idx of each keyword. Then when find a new keyword, use the previous keyword length and index to compute the new length. T(n)=O(n), S(n)=O(m), where m is the number of keywords.

#### 13.9
Find the longest subarray with distinct entries
- Moving window. Record the last idx of each element, and a subarray start idx. When find a duplicate element has idx after start idx, move start idx after previous occurance of this element. T(n)=O(n), S(n)=O(m), where m is the number of longest distinct entries.

#### 13.10
Find the length of a longest contained interval
- Use a set to record all integers. Then start from any interger, check if its left and right intergers are exist. If exist, update the subset and remove them. T(n)=O(n), S(n)=O(n).

#### 13.11
Compute the average of the top three scores
- Store scores in a min heap for each student. T(n)=O(mlogk), where m is the student number and k is 3. S(n)=O(km).

#### 13.12
Compute all string decompositions
- Given a list of N same length m strings, find substrings from a n length sentence that contain all these strings exactly once. Check all substrings that have the expected length. T(n)=O(Nmn), S(n)=O(mn).

#### 13.13
Test the Collatz conjecture
- Test all numbers from 1 to n. Use a hash set to record tested value. Only test odd value because even value / 2 must has tested. Stop when the tmp value is smaller than the current test value. T(n)=O(n), S(n)=O(n).

#### 13.14
Implement a hash function for chess
- There are 64 positions. Each position has 13 states, which are empty, one of 6 class chesses * 2 sides. So can use a 13 base 64 digits number to record each state. Hash code is sum(ci * p ^ i), where i indicate the idx of digits. When update, substract two old states and compute two new states for those two position.
- Can use 64 base 13 digits. Same time complexity. But notice compute p ^ i is expensive.
- Can assign each state on each pisition a number, so there are in total 13 * 64 numbers. The hash code is the sum of all current state. So update the hash code only need 4 operations. There would be collide so I don't understand how this could be a solution. <b>?</b>

Variant:
- include castling right an en passant information: <b>?</b>

### Chapter 14
O(nlogn) sortings: 
- heapsort: not stable(equal entries show in the result as the same order)
- merge sort: not inplace
- quicksort: O(n^2) under worst case. Also the call stack is O(logn)

Alternatives:
- when array length less than 10, insertion sort is faster than quick sort.
- elements are at most k away from right position, use a k size heap.
- when the number of kinds of elements is small, use counting sort. radix sort is a special counting sort, which is based on radix(number of digits)

C++ library
- when define personal structure, implement `bool operator<(const T &other) const` to use sort.
- `sort()` in `<algorithm>` head file.
- `list::sort()`

#### 14.1
Compute the intersection of two sorted arrays
- brute force: for each entry in A, traverse through B to find same entries. T(n)=O(mn), S(n)=O(n).
- For each entry in A, use binary search on B to find same entries. T(n)=O(nlogm), S(n)=O(n). Best when one array is much smaller than another.
- Use two pointers point to current entries in two arrays. Increase the pointer which is smaller than the others until find same entries. T(n)=O(m+n), S(n)=O(n).

#### 14.2
Merge two sorted arrays
- Compare two pointers from the end of arrays. Use a pointer to indicate where to insert. T(n)=O(n), S(n)=O(1).

#### 14.3
Remove first-name duplicates
- Use hash set. T(n)=O(n), S(n)=O(n).
- First sort the array, then remove duplicates in place from beginning. T(n)=O(nlogn), S(n)=O(1).

#### 14.4
Render a calendar
- For each endpoint, check how many events include it. T(n)=O(n^2), S(n)=O(1).
- Sort endpoints of events. Iterate through endpoints, if there is a start endpoint, increase the counter; if it is an end endpoint, decrease the counter. Start endpoint should come before end endpoint. T(n)=O(nlogn), S(n)=O(n).

Variant:
- user i use bi bandwidth from time si to fi. Max bandwidth: sum bi instead of 1.

#### 14.5
Merging intervals
- start of the interval: if the start is between an interval, the new interveral start is the old start, otherwise the start is its start; if the end is between an interval, the new end is the end of the old interval, otherwise its end. T(n)=O(n), S(n)=O(1).
- 1. find first overlaping interval; 2. compute until last overlapping interval; 3. replace those intervals with the new interval. T(n)=O(n), S(n)=O(1).

#### 14.6
Compute the union of intervals
- for every interval, check if it intersect with other intervals. If it does, remove it and those intersected ones and add to the result. T(n)=O(n^2), S(n)=O(n).
- sort intervals based on their left end. iterate through them and find all intersected intervals in a roll, add the union result into  the result. T(n)=O(nlogn), S(n)=O(n).

#### 14.7
Partitioning and sorting an array with many repeated entries
- Sort the array. T(n)=O(nlogn), S(n)=O(1).
- In place bucket sort. Use a hashtable to record the end of each bucket. Each time pick the next cell of last element in the first bucket, and place it into the right bucket. When a bucket is full, remove the bucket from hashtable. At the end the array will be sorted. T(n)=O(n), S(n)=O(m), where m is the number of buckets.

#### 14.8
Team photo day 1
- Place two teams in two rows explicitly. Person in the second row should be taller than the first row. Sort two teams. Compare each pair to see if the taller one is in the same team. T(n)=O(nlogn), S(n)=O(n).

#### 14.9
Implement a fast sorting stable algorithm for lists
- Insert sort: traverse each node, find its place in the previous sorted list. T(n)=O(n^2), S(n)=O(1).
- Merge sort: seperate the list into two halves, and then merge. T(n)=O(nlogn), S(n)=O(logn).

#### 14.10
Compute a salary threshold
- Given a salary array and a target sum payroll. Set a cap so that salary above cap become cap, and the sum equle to payroll. First set cap to be each salary and compute the sum payroll. Find out cap should between which two salaries. Then use a formular to compute the exact cap. T(n)=O(n^2). S(n)=O(n).

Variant:
- Solve the problem use O(1) space: The answer is O(1).

### Chapter 15
Binary search tree: a node is >= its left subtree but <= its right subtree.
- Good to find min/max and the next min/max elements.

Red-black tree: height balanced tree, so that insertion and deletions are O(logn).

Augmented BSTs
- add a size field for BST Nodes so that compute number of entries become much easier.
- count for nodes in a range [L, U] can be done by count number of nodes < U and minus number of nodes < L.

C++ library:
- `set` and `map` are BST-based. Library BST use caching so T(n)=O(1).
- `begin()` traverse in ascending order, and `rbegin()` traverse in descending order.
- `lower_bound(a)` return the first element that is >= a; `upper_bound(a)` return > a.
- `equal_range(a)` return a pair of iterators.

#### 15.1
Test if a binary tree satisfies the BST property
- Use a struct to return the largest and smallest value of a subtree, and a bool to indicate if the tree is not BST. T(n)=O(n), S(n)=O(logn).
- Use a range (-limit, node) to check if left subtree nodes are in this range, and (node, +limit) to check right subtree. T(n)=O(n), S(n)=O(logn).
- BFS traverse the tree. T(n)=O(n), S(n)=O(logn).

#### 15.2
Find the first key greater than a given value in a BST
- Binary search. Store the latest seen larger value. If find another larger value but smaller than the previous one, return that one. T(n)=O(logn), S(n)=O(1). 

Variant:
- with same values, return the first node that equal to a value: if the left child is same, return left child, else return root.

#### 15.3
Find the k largest elements in a BST
- Reversed inorder traverse. T(n)=O(h+k), S(n)=O(logn).

#### 15.4
Compute the LCA in a BST with distinct value
- The first node that has value between two input nodes, comming from root, is the LCA. T(n)=O(h), S(n)=O(1).

#### 15.5
Reconstruct a BST from unique traversal preorder data
- Since inorder traverse of a BST is a sorted array. So preorder and inorder can reconstruct the BST. T(n)=O(nlogn), S(n)=O(n).
- Use a static pointer points to preorder vector elements. If the current element is added to the tree, pointer move on, otherwise stay and return a nullptr. T(n)=O(n), S(n)=O(n).

#### 15.6
Find the closest entries in three sorted arrays
- Merge three arrays into one, and use a moving window. T(n)=O(n), S(n)=O(n).
- Get first entries from trhee arrays. Count the distance. Then remove the smallest entry and add its successor from its array. T(n)=O(nlogk), S(n)=O(k).

#### 15.7
Enumerate first k numbers of the form a + b sqrt(2)
- Note if `a+b*sqrt(2)=a'+b'*sqrt(2)`, then a=a', b=b'. So compute all numbers of a=0, b=0 to a=k-1, b=k-1, and then sort them. T(n)=O(k^2log(k^2)), S(n)=O(k^2).
- Note `a + b * sqrt(2) < a + 1 + b * sqrt(2) < a + (b + 1) * sqrt(2)`. Use a BST to hold a set(key is unique, and BST as well) of candidates, and retrive the smallest one to push to the result, and add two new candidates into the set. T(n)=O(klogk), S(n)=O(k).
- Since new added value must be one previous value plus 1 or sqrt(2), record two pointers which point to previous add 1 and previous add sqrt(2) entries. If first pointer + 1 > second pointer + sqrt(2), move second pointer forward. T(n)=O(n), S(n)=O(k).

#### 15.8
The most visited pages problem
- Use hash table. T(n)=O(n), S(n)=O(n).
- Use height-balanced BST, with node as (page, visit count) pair, ordered by visit count. Also use a hash table to map page to BST node. Find operation use T(n)=O(k+logm), where k is the number of pages we want, and m is the total number of pages. 

Variant:
- a solution that T(n)=O(1) for read in pages, and T(n)=O(k) for find k pages: hash table <b>?</b>

#### 15.9
Build a minimum height BST from a sorted array
- Use the middle as the root. T(n)=O(n), S(n)=O(n).

#### 15.10
Insertion and deletion in a BST
- Keys are unique. Insert: try to find the key, if find, return; else, must at a leave, which can add node to; Delete: Find its successor in its right subtree. If no right subtree, move left subtree to it; Otherwise its successor must doesn't have left subtree. Replace successor's value to the deletion node, and then replace the successor with its right subtree. T(n)=O(h), S(n)=O(1).

#### 15.11
Test if three BST nodes are totally ordered
- Search from one node to mid, if find, then search from mid to the other node. Otherwise search from the other node to mid. T(n)=o(h), S(n)=O(1).

#### 15.12
The range lookup problem
- 3D question: Build two BST trees on X and Y coordinates. Set a D for distance range, so that search for X in [x-D, x+D] and Y in [y-D, y+D], and do intersect on results. Brute-force check all results to find the nearest one. If there is no result found, double the D.
- 3D question: Quadtrees and k-d trees.
- 2D: Recursive call to check if root is in the range. 

#### 15.13 
Add credits
- A data structure that support search by id, find the max credit, increase all credits and add/remove. Use hash table to store id-credit map, BST to store credit-ids map. Increase all credits can be implemented by using a global offset.

### Chapter 16
Recursion:
- base case
- ensuring process
- end point

The Euclidean algorithm for calculating the greatest common divisor (GCD)
- if y > x, the GCD of x and y is GCD of x and y - x. T(n)=O(n), n is the number of bits to represent the input. S(n)=O(1).
```
long long GCD(long long x, long long y) {
    return y == 0 ? x : GCD(y, x % y);
}
```

#### 16.1
The Towers of Hanoi problem
- Use recursion. Use 1. number of rings need to move, 2. stacks, 3. from peg, 4. to peg, 5. use peg as arguments. T(n)=O(2^n). 

Variant:
- without recursion: <b>?</b>
- other dumb quesitons: <b>?</b>

#### 16.2
Generate all nonattacking placements of n-Queens
- Use an array to record the positions. Place a queen on first column and recursive to check for other columns. T(n)=O(n!/(c^n)), S(n)=O(n^2)

Variant:
- number of solutions: return the result size
- other dumb quesitons: <b>?</b>

#### 16.3
Generate permutations
- Sort the input array, then use a bool vector to indicate which elements are already added, and call recursive. T(n)=O 
- for unique array, i indicate the processed entry, for j=i to size, swap i and j. If i=size, add vector to result. T(n)=O(n * n!)
- Call next permutation iteratively. T(n)=O(n * n!).

Variant:
- duplicate: the result.

#### 16.4
Generate power set
- All possible subsets with entries from input. Use recursion. T(n)=O(2^n), S(n)=O(2^n).
- Consider subnets as put 0 or 1 on each entry. So call recursive to either add current element or not add current element to the subnet.
- Use binary numbers to indicate the subnets. It save some space but T(n)=O(n * 2^n).

Variant:
- duplicates: the answer

#### 16.5
Generate all subsets of size k
- Call recursive. T(n)=O(2^n). 

#### 16.6
Generate strings of matched parens
- Between the ith left paren and i+1 th left paren, there could be at most i right parens. And the total number of right parens should be n. T(n)=O(2^n), S(n)=O(2^n)
- For right each paren, can add at this time, or add at next time, unless it has a left paren already. So to create a recursive, add left paren when available, then call recursive; add right paren when available, then call recursive make the program walk through all possibility. T(k)=O((2k)!/((k!(k+1)!))

#### 16.7
Generate palindromic decompositions
- Cannot understand what is the question and there are typos in the example, so I believe the author was drunk when wrote this question.

#### 16.8
Generate binary trees
- If there are two nodes, for a root, there can be 1 left child and 0 right child, or 0 left child and 1 right child. Recursive left subtree nodes number from 0 to n, and build trees. T(n)=C(n)=(2n)!/(n! * (n+1)!)

#### 16.9
Implement a Sudoku solver
- Recursive call to try all values and stop when invalid value shows. T(n)=O(2^9)

#### 16.10
Compute a Gray code
- By defination, use a hashtable to record added results. Then try modify one bit from the preivous result to see if it should add to results or not. 
- When bit number is 1, there is only 0 and 1. Bit number is 2, use 0 add bit number 1 vector, and then use 1 add the vector in reverse order. T(n)=O(2^n), S(n)=O(2^n).

#### 16.11
Compute the diameter of a tree with weight on edges
- Diameter is the max distance between two leaves. Traverse all the children. One child could has the largest diameter. Otherwise the sum of distances to two farthest leaves is the diameter. T(n)=O(n), S(n)=O(1).

Variant:
- minimize the time for rooted tree sends message to all nodes: use the reverse order of the distance to leaves.

### Chapter 17
Like D&Q solution, but subproblem for same values are reoccuring, so caching the result.

#### 17.1
Count the number of score combinations
- (Wrong!)From 0, 1, ..., x, to record how many ways to reach x, then N(x) = N(x1) + N(x2) + N(x3) + ..., where xi means xi + certain score = x. T(n)=O(x). S(n)=O(x). It is wrong due to it counts one way into two, e.g. to reach 5 with score 2 and 3, it return 2. 
- A 2D matrix, which row m means previous m scores are involved. For column n, it means how many ways to reach n. C(m,n)=C(m-1,n) + C(m,n-A(n)). The first express means without this score A(n), how many ways. The second express means with this score and previous scores, how many ways. T(n)=O(mn), S(n)=S(mn).

Variant:
- Wait for the next time to see <b>?</b>

#### 17.2
Compute the Levenshtein distance
- To make first i chars from s1 and first j chars from s2 to be same, if s1(i) == s2(j), T(i,j)=min(T(i-1,j), T(i,j-1), T(i-1,j-1)), otherwise T(i-1,j-1). T(n)=O(mn), S(n)=O(mn).

Variant:
- Compute the Levenshtein distance using O(min(a,b)) space and O(ab) time: Rotate use space.
- compute a longest sequence of characters that is a subsequence of A and of B: LCA
- Too many... <b>?</b>

#### 17.3
Count the number of ways to traverse a 2D array
- N(i,j)=min(N(i-1,j), N(i,j-1)) + 1. T(n)=O(mn), S(n)=O(mn).
- C(m, n). T(n)=O(m), S(n)=O(1).

Variant:
- next time... <b>?</b>

#### 17.4
Compute the binomial coefficients
- C(n, k)=C(n-1, k)+C(n-1, k-1). Use recursive with records of previous results. T(n)=O(nk), S(n)=O(k). 

#### 17.5
Search for a sequence in a 2D array
- Start from a cell, check if it could be a start of a subarray. Use hash table to record previous results. T(n)=O(mn), S(n)=O(mn)<b>?</b>

Variant:
- later <b>?</b>

#### 17.6
The knapsack problem
- Each item has two state: in pack or not in. Call recursive. When 2^n < n * weight, this algorithm is faster. T(n)=O(2^n), S(n)=O(n).
- Compute weight from 0 to weight. w means the capacity, i means include previous i items. Value(i, w) = max( Value(i-1, w), Value(i-1, w-wi) + vi ). T(n)=O(nw), S(n)=O(nw).

Variant:
- later <b>?</b>

#### 17.7
The BEDBATHANDBEYOND.COM problem
- Use an array to record the length of each substring. For every char, check backward to see if a substring end at this char is in the dictionary and the previous chars have length in array. T(n)=O(n^2), S(n)=O(n).

Variant:
- later <b>?</b>

#### 17.8
Find the minimum weight path in a triangle
- For each row, compute from left to right and record the min weight path to this cell in an array. T(n)=O(n^2), S(n)=O(n).

#### 17.9
Pick up coins for maximum gain
- Two players can only pick the first or last coin each time. Let R(st, ed) means the max revenue one player can get, S(st, ed) means the sum of coins. C[i] is the coin value, then
```
R(st, ed) = max( (C[st] + S(st + 1, ed) - R(st + 1, ed)), (C[ed] + S(st, ed - 1) - R(st, ed - 1)) )
```
If st coin is picked, there are two options for player 2 to get R(st + 1, ed). It makes the first part become
```
C[st] + S(st + 1, ed) - R(st + 1, ed) = one of
  / C[st] + R(st + 1, ed - 1)
  \ C[st] + R[st + 2, ed)

```
Player 2 also try his best, so take the base case, when st = 0 and ed = 3. Player 2 can pick from 1 to 3. 
```
R(1, 3) = 
  / C[1] + S(2, 3) - R(2, 3) = S(1, 3) - R(2, 3)
  \ C[3] + S(1, 2) - R(1, 2) = S(1, 3) - R(1, 2)
```
Player 2 will pick the coin to min( R(2, 3), R(1, 2) ). T(n)=O(n^2), S(n)=O(n^2).

#### 17.10
Count the number of moves to climb stairs
- Advance at most k steps, to climb to n stairs. Use an array with n length. T(n)=O(kn), S(n)=O(n).

#### 17.11
The pretty printing problem
- Use the sum square of the blanks at the end of each line as messiness, find the min messiness of ways to distribute words in fixed-size lines. Consider the last line, there could be one to all words in it. The messiness is messiness of previous lines + messiness of the last line. T(n)=O(nL), S(n)=O(n).

Variant:
- later <b>?</b>

#### 17.12
Find the longest nondecreasing subsequence
- Use an array to record the longest subsequence to element i. Then for i+1, find the longest one for all elements that are not greater than it. T(n)=O(n^2), S(n)=O(1).
- Use an array to record for each length, the last element. For element i, use binary search to find the previous element, which is the last not greater element, and update length array. Why can use binary search is because every time the updated element must not smaller than the new element, and it is already smaller than the next element. T(n)=O(nlogn), S(n)=O(n).

Variant:
- later <b>?</b>

### Chapter 18
An invariant is a condition that is true during execution of a program.

#### 18.1
Compute an optimum assignment of tasks
- Pair the longest task with the shortest task. T(n)=O(nlogn), S(n)=O(n).

#### 18.2
Schedule to minimize waiting time
- Sort the array. T(n)=O(nlogn), S(n)=O(1).

#### 18.3
The interval covering problem
- A list of closed intervals. Find a set of numbers that all intervals have covered those numbers. Always pick the right point of the first end interval. T(n)=O(nlogn), S(n)=O(n).

Variant:
- later <b>?</b>

#### 18.4
The 3-sum problem
- Sort the array. For each A(i), solve 3-sum problem for sum - A(i), by start from the sum of beginning and ending, and then move either one. T(n)=O(n^2), S(n)=O(n).

Variant:
- later <b>?</b>

#### 18.5
Find the majority element
- hash table. T(n)=O(n), S(n)=O(n).
- Majority element must occurs more than n / 2 times in a n-size array. So use a counter on current element. If next element is the same one, increase the count; otherwise decrease the count. If the count back to 0, it means the element cannot be the majority, and the last one differ from it cannot be the majority as well. So pick the next one as candidate. The algorithm return a nonsense result when there are at lest two majority numbers. T(n)=O(n), S(n)=O(1).

#### 18.6
The gasup problem
- If find when reach a city, the gas is not enough, then start from this city. T(n)=O(n), S(n)=O(1).

Variant:
- later <b>?</b>

#### 18.7
Compute the maximum water trapped by a pair of vertical lines
- Always be the distance between st and ed multiple min(A[st], A[ed]), so if A[st] < A[ed], st + 1, else ed - 1. T(n)=O(n), S(n)=O(1).

#### 18.8
Compute the largest rectangle under the skyline
- For each start, find iteratively to an end that is just smaller than it, and then compare with the previous max value. T(n)=O(n^2), S(n)=O(1).
- Use a stack to store increase build indexes. If meet a lower build than the last building in the stack, compute rectangles until the top of the stack is lower. Notice the width is not current idx - stack top. T(n)=O(n), S(n)=O(n).

Variant:
- later <b>?</b>

### Chapter 19
Graph: vertices and edges.

Directed graph edge(u, v): u is the source, and v is the sink.

Directed acyclic graph(DAC): no cycles. Vertices have topological order. Can have several sources and have several sinks.

Weakly connected: a directed graph can produce a connected graph when replace all directed edges into undirected edges.

Strongly connected: for every vertice, it can reach other vertices.

Graph presentation:
- adjacency lists
- adjacency matrix

Spanning tree of a graph: a tree that all its edges are a subnet of a graph.

(The bootcamp is written by that drunk guy again)

From a list of match results, find out if a team A has beat another team B:
- create a directed graph from matches, where the winner is the source and loser is the sink. Then find path between A and B using DFS. T(n)=O(E), S(n)=O(E).

Depth-first search:
- T(n)=O(|V| + |E|)
- Can also check if there are cycles
- discovery time and finishing time

Breadth-first search:
- T(n)=O(|V| + |E|)
- Can compute distances

Advanced graph algorithms with polynominal time complexity
- shortest path
- minimum spanning tree <b>how?</b>
- matching: given an undirected graph, find a maximum collection of edges subject to the constraint that every vertex is incident to at most one edge. <b>how?</b>
- maximum flow <b>how?</b>

#### 19.1

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

`unique(arr.begin(), arr.end())` remove all adjacent duplcates and return the iterator of next element of the end.

http://stackoverflow.com/questions/32685540/c-unordered-map-with-pair-as-key-not-compiling

`unordered_map::at`: return a reference of the key, or out-of-range error.
