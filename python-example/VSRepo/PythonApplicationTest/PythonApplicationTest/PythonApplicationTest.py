#!/usr/bin/python

from sys import argv

def main():
    message = "Hello World!"
    if (len(argv) > 1):
        message = argv[1]

    print(message)

if __name__ == '__main__':
    main()
