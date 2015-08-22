http://www.grymoire.com/Unix/Awk.html#uh-1  

line oriented.  

# form: 
```
pattern { action }
```
test each line with pattern. If true, do action.  

```
BEGIN { }
      { }
END   { }
```
actions taken before first/after last lines are read  

`\t` tab.  
`$n` the nth argument.  

# execute
```
awk 'script' file
```

# pattern 
regular express in '/ /'  
```
awk '/^1/ { print $1 }' file
```

Not logic for pattern: 
```
awk '! /^1/ {}' file
```

# Seperate char: 
https://www.gnu.org/software/gawk/manual/html_node/Field-Separators.html  
```
awk 'BEGIN { FS = "," } ; { print $3 }' 
```
