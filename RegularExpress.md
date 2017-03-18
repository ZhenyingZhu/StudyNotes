## Reference
- awk.markdown
- RudyStudy.markdown
- http://www.regular-expressions.info/
- https://regex101.com/

## tools that support
`awk`, `less`

Search in less: 
- ASCII regular express
- E.g. [0-9]{2,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}

## Intro
More than wildcard.

search for an email address: `\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b`

HTML tag: `<[A-Za-z][A-Za-z0-9]*>`, `<[A-Za-z0-9]+>` is wrong because it also matches cases like `<1>`. Best solution: `<[^<>]+>`

Search only match the first occurence by default.

### metacharacters
Need to backclash when use them as character
- the backslash \
- the caret ^
- the dollar sign $
- the period or dot .
- the vertical bar or pipe symbol |
- the question mark ?
- the asterisk or star *
- the plus sign +
- the opening parenthesis (. and the closing parenthesis )
- the opening square bracket [, and the opening curly brace {

### Character classes or sets
- `[ae]` means could be a or e
- `[0-9a-fxA-FX]`: 0 to 9, a to f, x, A to F, X
- `q[^x]`: caret after open parenthesis means negative. So it means start from q and not x following

### Shorthand Character Classes
- `\d` matches a single character that is a digit
- `\w` matches a "word character" (alphanumeric characters plus underscore)
- `\s` matches a whitespace character (includes tabs and line breaks)

### Non-Printable Characters
- `\t` to match a tab character (ASCII 0x09)
- `\r` for carriage return (0x0D)
- `\n` for line feed (0x0A)
- `\a` (bell, 0x07)
- `\e` (escape, 0x1B)
- `\f` (form feed, 0x0C)
- `\v` (vertical tab, 0x0B)
- Windows text files use `\r\n` to terminate lines, while UNIX text files use `\n`
- Unicode, use `\uFFFF` or `\x{FFFF}` to insert a Unicode character. `\u20AC` or `\x{20AC}` matches the euro currency sign
- `\xFF` to match a specific character by its hexadecimal index in the character set. `\xA9` matches the copyright symbol in the Latin-1 character set

### Dot Matches (Almost) Any Character
Matches any single char except line break

If under single line mode, `.` can also match line break

### Anchors
- `^` matches at the start of the string
- `$` matches at the end of the string
- `\b`: word boundary, e.g. `\ba` matches `a` from `cde ab`, and `e\b` matches `e`

If under multi-line mode, `^` is the pos after line break

### Alternation
lowest precedence so that `cat|dog food` matches either `cat` or `dog food`. `(cat|dog) food` matches `cat food` or `dog food`

### Repetition
- `?`: preceding token shows 0 or 1 time. `colou?r` matches `color` or `colour`
- `*`: 0 or more times. 
- `+` : 1 or more times
- Use curly braces to specify times: `\b[1-9][0-9]{2,4}\b` 2 to 4 times, which s a number between 100 to 99999.

### Greedy and Lazy Repetition
Repetition operators or quantifiers are greedy. Use `?` after quantifier(`*` or `+`) to make it lazy.

e.g. `This is a <EM>first</EM> test`
- `<.+>` matches `<EM>first</EM>`
- `<.+?>` matches `<EM>`. `<[^<>]+>` is a better way

### Grouping and Capturing
`Set(?:Value)?` <b>?</b>
[HERE](http://www.regular-expressions.info/quickstart.html)



