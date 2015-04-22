#! /bin/sh

echo "apple" 2>&1 | tee -a appleandscreen
echo "apple" >> appleonly 2>&1
