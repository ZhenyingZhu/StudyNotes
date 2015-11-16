def count7(N):
    '''
    return how many 7 in the number
    N: a non-negative integer
    '''
    if N == 0: 
        return 0
    addition = 1 if N%10 == 7 else 0
    return count7(N/10) + addition

