def primesList(N):
    '''
    Return all the prime number between 2 and N
    N: an integer
    '''
    res = [2]
    for i in range(3, N+1): 
        isPrime = True
        for prev in res: 
            if i%prev == 0: 
                isPrime = False
                break
        if isPrime: 
            res.append(i)
    return res

