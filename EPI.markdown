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


