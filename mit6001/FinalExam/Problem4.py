def getSublistsRecursive(L, n):
    sublists = []
    if n == 0:
        sublists.append([])
        return sublists

    if len(L) == n:
        sublists.append(L[:])
        return sublists

    sublists.append(L[:n])

    for list in getSublists(L[1:], n):
        sublists.append(list)

    #print "===L:" + str(L) + " n:" + str(n) + "==="
    #print sublists
    #print "=== === === ==="
    return sublists


def getSublists(L, n):
    sublists = []
    for i in range(len(L)-n+1):
        sublists.append(L[i:i+n])
    return sublists


def longestRun(L):
    maxRunLength = 1

    if len(L) == 1:
        return maxRunLength

    st = 0
    for i in range(1, len(L)):
        if (L[i] < L[i-1]):
            maxRunLength = max(i-st, maxRunLength)
            st = i

    if (L[-1] >= L[-2]):
        maxRunLength = max(len(L)-st, maxRunLength)

    return maxRunLength


# getSublists tests
assert getSublists([10, 4, 6, 8, 3, 4, 5, 7, 7, 2], 4) \
       == [[10, 4, 6, 8], [4, 6, 8, 3], [6, 8, 3, 4], [8, 3, 4, 5], [3, 4, 5, 7], [4, 5, 7, 7], [5, 7, 7, 2]]
assert getSublists([1, 1, 1, 1, 4], 2) == [[1, 1], [1, 1], [1, 1], [1, 4]]

# longestRun tests
assert longestRun([10, 4, 6, 8, 3, 4, 5, 7, 7, 2]) == 5
assert longestRun([8, 9, 10, 1, 2, 3 ,4 ,5]) == 5
