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
Iteration: change the test value inside the loop.  
Bisection search.  

Don't overwrite build-in functions like sum.  
https://docs.python.org/2/library/functions.html  
https://docs.python.org/release/2.3.5/ref/keywords.html  

```
x = 1
print(str(x) + "is a number")
```

Exaustive enumeration.  

`range(m, n, t)` not include n. t is the step.  

```
num = 10
for num in range(5): # num is a redefine here
    print num
````

Float:  
- binary present: x=a*2^(-1)+b*2^(-2), then x==0.ab
- x*2^p=n, then x = n/(2^p). Make n an integer(whole number), then present n in binary, and shift left the dot p times.  
- Cannot find a p to make 0.1 be a whole number. Python stop at a point by round it. 0.1 is not store as exactly 0.1 in the computer.  
- don't use x==y, instead use abs(x-y)<0.0001

`0.5%1=0.5`.   

Bisection search: work on problems with "ordering" property(value of function monotonically with input value).  
```
low = 0.0
high = x
while abs(ans**2 - x) >= epsilon: 
    if ans**2 < x: 
        low = ans
    else: 
        high = ans
    ans = (high + low) / 2
```

Print two words in a line.  
```
print "Hi",
print "there"
```

Newton-Raphson algorithm: p(x) = an*x^n + an-1*x^(n-1) + ... + a1*x + a0, find r that p(r) = 0.  
- first guess g and check
- g = g - p(g)/p'(g)  

```
if char in "aeiou": 
    print "char is a vowel. "
```

Lecture 4
Turing complete language.  

Abstruction: black box.  

docstring: inside three pair of quotes.  

In Python 2 functions can also be compared. Based on their id(object adress in memory).  

Python shell is default(global) environment.  

Function is a procedure object. Function name is an environment pointer.  
Function name is expr0, other args are expr1, 2 ...  

