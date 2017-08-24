# Software Development
https://www.edx.org/micromasters/software-development

## How to Code: Simple Data
https://www.edx.org/course/how-code-simple-data-ubcx-htc1x

### Introduction

### Beginning Student Language

Manual:
- https://courses.edx.org/courses/course-v1:UBCx+HtC1x+2T2017/a24b7341216346f2a5c99c6391f64229/
- file:///C:/Program%20Files/Racket/doc/search/index.html

Primitive call

```
(require 2htdp/image)
```

Define a function
```
(define (foo arg)
  (op arg))
```

### How to Design Functions
- How to Design Functions (HtDF) recipe
- complete function design
- evaluate the different elements for clarity, simplicity and consistency with each other
- evaluate the entire design for how well it solves the given problem

Design Recipes: https://courses.edx.org/courses/course-v1:UBCx+HtC1x+2T2017/77860a93562d40bda45e452ea064998b/

HtDF: https://courses.edx.org/courses/course-v1:UBCx+HtC1x+2T2017/77860a93562d40bda45e452ea064998b/#HtDF
- Signature, purpose, stub: write a function prototype, with correct return type, so it can pass to a test and fail
- Examples: write test cases, which cover all user cases
- Inventory (template & constants): template is the outline of the function
- Code body: verify if the code is correct or not
- Test and debug

Design process needs to deal with vague requirement. 

Use enought test cases to get code coverage
- All if normal cases
- boundary cases

### How to Design Data
HtDD: https://courses.edx.org/courses/course-v1:UBCx+HtC1x+2T2017/77860a93562d40bda45e452ea064998b/#HtDD
- identify problem domain information: simple atomic data, intervals, enumerations, itemizations and mixed data itemizations
- templates for functions operating on atomic data

From problem domain to data, interpret real world information as data.

HtDD
- structure defination
- type comment
- interpretation
- examples
- template of a 1 arg func operating on this data

No-primitive data
- Simple Atomic Data, normally primitive data with certain domain
- Interval
- Enumeration
- [Itemization](https://courses.edx.org/courses/course-v1:UBCx+HtC1x+2T2017/77860a93562d40bda45e452ea064998b/#Itemization): is comprised of 2 or more subclasses, at least one of which is not a distinct item. Need a guard when check if one the itemization data is which subclass iteam.

Compound data
- Compound data: consists of two or more items that naturally belong together	
- References to other defined type: is naturally composed of different parts	
- self-referential or mutually referential: is of arbitrary (unknown) size	


