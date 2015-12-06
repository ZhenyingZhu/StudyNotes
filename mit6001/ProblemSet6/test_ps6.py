from ps6_encryption import *

def test_buildCoder():
    testPass = True
 
    shiftThreeDict = {'A': 'D', 'C': 'F', 'B': 'E', 'E': 'H', 'D': 'G', 'G': 'J', 'F': 'I', 'I': 'L', 'H': 'K', 'K': 'N', 'J': 'M', 'M': 'P', 'L': 'O', 'O': 'R', 'N': 'Q', 'Q': 'T', 'P': 'S', 'S': 'V', 'R': 'U', 'U': 'X', 'T': 'W', 'W': 'Z', 'V': 'Y', 'Y': 'B', 'X': 'A', 'Z': 'C', 'a': 'd', 'c': 'f', 'b': 'e', 'e': 'h', 'd': 'g', 'g': 'j', 'f': 'i', 'i': 'l', 'h': 'k', 'k': 'n', 'j': 'm', 'm': 'p', 'l': 'o', 'o': 'r', 'n': 'q', 'q': 't', 'p': 's', 's': 'v', 'r': 'u', 'u': 'x', 't': 'w', 'w': 'z', 'v': 'y', 'y': 'b', 'x': 'a', 'z': 'c'}
    shiftNineDict = {'A': 'J', 'C': 'L', 'B': 'K', 'E': 'N', 'D': 'M', 'G': 'P', 'F': 'O', 'I': 'R', 'H': 'Q', 'K': 'T', 'J': 'S', 'M': 'V', 'L': 'U', 'O': 'X', 'N': 'W', 'Q': 'Z', 'P': 'Y', 'S': 'B', 'R': 'A', 'U': 'D', 'T': 'C', 'W': 'F', 'V': 'E', 'Y': 'H', 'X': 'G', 'Z': 'I', 'a': 'j', 'c': 'l', 'b': 'k', 'e': 'n', 'd': 'm', 'g': 'p', 'f': 'o', 'i': 'r', 'h': 'q', 'k': 't', 'j': 's', 'm': 'v', 'l': 'u', 'o': 'x', 'n': 'w', 'q': 'z', 'p': 'y', 's': 'b', 'r': 'a', 'u': 'd', 't': 'c', 'w': 'f', 'v': 'e', 'y': 'h', 'x': 'g', 'z': 'i'}
    
    if buildCoder(3) != shiftThreeDict:
        testPass = False
    if buildCoder(9) != shiftNineDict: 
        testPass = False

    if testPass: 
        print "builderCoder() success"
    else:
        print "builderCoder() failed"


def test_applyCoder(): 
    testPass = True
    
    if applyCoder("Hello, world!", buildCoder(3)) != 'Khoor, zruog!':
        testPass = False
    if applyCoder("Khoor, zruog!", buildCoder(23)) != 'Hello, world!': 
        testPass = False
    
    if testPass: 
        print "applyCoder() success"
    else: 
        print "applyCoder() failed" 


def test_applyShift(): 
    testPass = True
    
    if applyShift('This is a test.', 8) != 'Bpqa qa i bmab.':
        testPass = False
    if applyShift('Bpqa qa i bmab.', 18) != 'This is a test.':
        testPass = False
    
    if testPass: 
        print "applyShift() success"
    else:
        print "applyShift() failed"
    

# Main
test_buildCoder()
test_applyCoder()
test_applyShift()
