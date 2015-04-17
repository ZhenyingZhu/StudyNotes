#! /bin/env ruby

arr = []
a, b, c = 1, 2, 3

Thread.new(a, b, c) { |d, e, f| arr << d << e << f }.join
arr
