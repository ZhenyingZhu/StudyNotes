s = 'abcd'

ans = ""
st = 0
ed = 0

while ed < len(s)-1: 
    if s[ed+1] >= s[ed]: 
        ed += 1
    else:
        ans = s[st:ed+1] if ed - st + 1 > len(ans) else ans
        st = ed + 1
        ed = st

# if s itself is the longest substring
if ans == "":
    ans = s

print "Longest substring in alphabetical order is: " + ans