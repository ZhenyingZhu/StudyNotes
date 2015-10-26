https://courses.edx.org/courses/course-v1:MITx+6.00.1x_7+3T2015/info

## Lecture 1
Turing's Halting Problem.  

- Declarative knowledge
- Imperative knowledge  

Square root of a number: `g^2 = x` find g
- guess g
- Test: if `g^2 ~= x`, return g
- Caculation: `g = 1/2(g + x/g)`
- Flow control

A special program, interpreter, execute each instruction of a program in order.  

Computer architecture:  
- Memory
- Control unit: include a program counter
- Arithmetic logic unit: include an accumulator
- Input 
- Output

Turing complete: six primitives can compute anything.   

Static semantics: even syntax is right, sometimes not has meaning

- Compiled language
- Interpreted language

Semantic errors can not be captured by computer.  
Defensive programming can avoid semantic errors.  

## Lecture 2
Low level language: src code -> checker -> interpreter -> output  
High level language: src code -> checker -> compiler -> object code -> interpreter -> output  

Objects are: 1. scalar(cannot be subdivied) 2. non-scalar.  
Scalar objects:  
- int  
- float
- bool

`type(object)`  
`3**2`  

Type conversion: `float(3)`  

The order of boolean operations is as follows:
# Parentheses. 
# not statements.
# and statements.
# or statements.

When compare int with float, auto convert the type.  

Assignment: `=`, create a binding between variable and value.   

Non-scalar objects: compound objects.  

Operator overloading.  

`len('abc')`  

Indexing: `'abc'[0]`. `'abc'[-1]` 
Slicing: `s[start:end]`   

`'a' in 'abc'`  

`s[i:j:k]` from i to j-1 with step k. If k is negative, go backward.  

`num = float(raw_input('input a number'))`  

branching program: with flow control.  

```
if express: 
    ...
elif express:
    ...
else: 
    ....
```

Any string is always larger than integer.  

## Lecture 3


