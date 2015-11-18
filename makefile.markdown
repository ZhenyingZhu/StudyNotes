https://www.cs.umd.edu/class/fall2002/cmsc214/Tutorial/makefile.html  

A .cpp usually consists of:
- the implementations of all methods in a class,
- standalone functions (functions that aren't part of any class),
- global variables (usually avoided). 

The corresponding .h file contains
- class declarations,
- function prototypes,
- and extern variables (again, for global variables). 

The purpose of the .h files is to export "services" to other .cpp files. 

When create an object, the implementations of methods are not necessary. 

Object file:  
- The object file is a dot o file. It is not executable.  
- The object file doesn't need a `main()` to compile.  
- When compile, only classes in this cpp file are compiled. Those in include files are not compiled.  

Create an object file by `-c`:  
```
g++ -Wall -c HelloWorld.cpp # -Wall means warning on all.  
```

Notice when use `g++` compile several source codes, it doesn't look through all codes when compile one. Instead it create internal o files and then link.  
  
A makefile typically consists of many entries. Each entry has:
- a target (usually a file): output  
- the dependencies (files which the target depends on): when need to reconstruct. based on the last-modified timestamp. This happened recursively.  
- and commands to run, based on the target and dependencies. 

A simple example:  
```
Movie.o: Movie.cpp Movie.h Vector.h
   g++ -Wall -c Movie.cpp
```

The basic syntax of an entry looks like:
```
<target>: [ <dependency > ]*
   [ <TAB> <command> <endl> ]+
```

The dependencies for a o file is:  
- the cpp file compile from.  
- all the h files that are in double quote in this cpp file.  

h files are dependent on other h files:  
```
Vector.h: Foo.h
```

Can use program `makedepend` to automately write a make.  

Commands:  
- all lines follow the dependencies line and with a tab at the beginning treat as the commands for this target.  
- cannot use spaces to replace tab.  
- The commands should generate the target.  

An executable:  
- this executable target should be the first target in the makefile.  
- then run `make` to build it. `make` only run the first target.  
- the command should set output.  
- if don't set an output, then `a.out` is created.  

```
p1: a.o b.o c.o
    g++ -Wall a.o b.o c.o -o p1
```

To make:  
- if there is a `makefile`, `make` build the first target in this file.  
- if there is a `Makefile` but no `makefile`, `make` run `Makefile`.  
- `make -f <file>` run the file.  
- `make <target>` only make that target.  

Use macro:  
- `<macro_name> = <macro_string>`  
- use upper case letters and underscores only in the macro name.  
- `$()` and `${}` will be substituted.  
- backslash can escape.  

```
OBJS = MovieList.o Movie.o NameList.o Name.o Iterator.o
CC = g++
DEBUG = -g
CFLAGS = -Wall -c $(DEBUG)
LFLAGS = -Wall $(DEBUG)

p1 : $(OBJS)
    $(CC) $(LFLAGS) $(OBJS) -o p1
```

