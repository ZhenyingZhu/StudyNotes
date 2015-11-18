OBJS = hello_main.o MyClass.o
CC = g++
DEBUG = -g
CFLAGS = -Wall -c $(DEBUG)
LFLAGS = -Wall $(DEBUG)

all : hello.p

hello.p : $(OBJS)
	$(CC) $(LFLAGS) $(OBJS) -o hello.p

MyClass.o : MyClass.cpp MyClass.h
	$(CC) $(CFLAGS) MyClass.cpp

hello_main.o : hello_main.cpp MyClass.h
	$(CC) $(CFLAGS) hello_main.cpp

clean : 
	\rm *.o hello.p
