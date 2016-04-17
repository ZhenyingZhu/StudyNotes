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
- Brute force: convert the array into number. Then convert back. Will overflow.
- Use a carry-out. T(n)=O(n).

#### 6.3
Multiple two arbitary-precision array presented integers. Negative number has `-` in the first cell.  
- Use reversed vector to record input numbers. Add carry-out to the previous cell. T(n)=O(n^2).  

#### 6.4
Advancing through an array. Integer in array indicate how far it can go from this cell. Check if it is able to reach the end.  
- Iterate through the array, record the furthest position it can reach from current cell. T(n)=O(n), S(n)=O(1).  

#### 6.5

# HERE

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