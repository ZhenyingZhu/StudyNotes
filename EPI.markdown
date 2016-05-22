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

