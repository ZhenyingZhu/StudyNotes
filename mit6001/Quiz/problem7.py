def uniqueValues(aDict):
    '''
    return all keys that have unique values in order
    aDict: a dictionary
    '''
    keyList = aDict.keys()
    valueList = aDict.values()
    
    for k in aDict.keys(): 
        if valueList.count(aDict[k]) > 1: 
            keyList.remove(k)

    keyList.sort()
    return keyList

