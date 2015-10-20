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

## Chapter 1
(!!skipped!!)

## Chapter 2
(!!skipped!!)

## Chapter 3
(!!skipped!!)

## Chapter 4
Data structure  
(!!skipped!!)

## Chapter 5
In C++ integer is 32 or 64 bits, while in Java it is always 32 bits.  

### 5.1
Compute parity of a 64-bit binary word.  
- Brute force: walk through the number by `&1` and `>>=`. Stop when `x == 0`,  Then modula 2. T(n)=O(n), where n is the first 1 to the end.  
- <b>Notice</b>: `^1` equals `%2`, `x&=(x-1)` remove the last 1 bit. T(n)=O(n), where n is the number of 1 bits.   
- Bit fiddling computation: 1. process multiple bits; 2. cache results in an array-based lookup table.  
- <b>Notice</b>: Order of XORs doesn't change the result. Parity of a number equals to XORs between its parts. T(n)=O(logn).  

### 5.2
<b>Notice</b>: `x & ~(x - 1)` extracts the lowest set bit of x.  
View a 64-bit integer as an array, LSB as index 0, MSB as index 63. Swap the bit at i and j.  
- Brute force: first check if two bits differ. If differ, flip them(XOR 1).  

### 5.3
<b>Notice</b>: while design an algorithm, run once and run many times affects.  
Reverse a 64-bit word from LSB to MSB.  
- swap 0-31 bits with 32-63 bits. Good if only need run once.   
- use a lookup table, record all 16-bits numbers and its reversed numbers.  

### 5.4
Find the closest number that has the same weight(same number of set bits) as the given number.  
- Brute force: try (x-1), (x+1), (x-2), (x+2) until reach a same weight number.  
- Swap the first two consecutive different bits from LSB. T(n)=O(n).   

### 5.5
Compute x multiple y with assignment, bitwise operators, boolean combination.  
- Brute force: repeat addition. T(n)=O(n^2)  
- Decimal multiplication with shift. T(n)=O(n^2).  

### 5.6
Compute quotient between x and y use only addition, subtraction and shifting.  
- Brute force: repeat subtract y from x until less y.  
- Substract 2^k*y from x each time. T(n)=O(n).  

### 5.7
Given double x and integer y, compute x^y. Assume won't overflow or underflow.  
- When y is nonnegative, brute force: x multiple itself for y-1 times. T(n)=O(2^n).  
- Recursive: x ^ (1010)base2 = x ^ (101)base2 * x ^ (101)base2. T(n)=O(n).  
- When y is negative, multiple (1/x)^(-y).   

### 5.8
Reverse digits of an integer, but leave the sign unchanged.  
- Brute force: change to string and change back.  
- Modulo 10. T(n)=O(n).   

### 5.9
Check if a decimal integer is a palindrome.  
- Brute force: return false if the integer is negative. Then convert the integer to string. T(n)=O(n), S(n)=O(n).   
- log10(x)+1 is the number of digits. Use this to create a max significent digits mask. T(n)=O(n), S(n)=O(1).  

### 5.10
Generate uniform random numbers between a and b.  
- use smallest i that let 2^i-1 > b-a. Then call random zero-one generator i times. If out-of-bound, retry. T(n)=O(lg(a-b+1)).  

### 5.11
Find rectangle intersection. Rectangles are parallel to x-axis.  
- ask if boundary counts. Here it counts.  
- since there are too many ways to intersect, thinking about not intersect would be easier. T(n)=O(1)   

## Chapter 6
Insertion into an array take O(n) time and O(n) space because of copy.  

### 6.1
Dutch National Flag program. Quick sort with equal band in the middle.  
Notice when use size_t as index, cannot do `for (size_t i = A.size(); i >= 0; --i)`. If so, when i is 0, then `--i` still larger than 0 and out of bound.  
- Brute force: swap all numbers larger than pivot after i to before i. and traverse i through 0 to end. Then from the end reversely do again. T(n)=O(n^2), S(n)=O(1).  
- Quick sort, first recursive from the begining, and then from the end. T(n)=O(n), S(n)=O(1).  
- Quick sort one pass. Maintain smaller, equal, unsorted and larger four bands. Note when swap between larger and current, current should not move forward. T(n)=O(n), S(n)=O(n).  

### 6.2
Increment an arbitrary-precision integer. E.g. Input {1,2,9}, Output {1,3,0}  
- Brute force: convert the array into number. Then convert back. Will overflow.  
- Use a carry-out. T(n)=O(n).   

### 6.3
Multiple two arbitary-precision array presented integers. Negative number has `-` in the first cell.  
- Use reversed vector to record input numbers. Add carry-out to the previous cell. T(n)=O(n^2).  

### 6.4
