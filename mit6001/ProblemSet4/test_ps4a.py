from ps4a import *
from ps4b import *

#
# Test code
# You don't need to understand how this test code works (but feel free to look it over!)

# To run these tests, simply run this file (open up in IDLE, then run the file as normal)

def test_getWordScore():
    """
    Unit test for getWordScore
    """
    failure=False
    # dictionary of words and scores
    words = {("", 7):0, ("it", 7):4, ("was", 7):18, ("scored", 7):54, ("waybill", 7):155, ("outgnaw", 7):127, ("fork", 7):44, ("fork", 4):94}
    for (word, n) in words.keys():
        score = getWordScore(word, n)
        if score != words[(word, n)]:
            print "FAILURE: test_getWordScore()"
            print "\tExpected", words[(word, n)], "points but got '" + str(score) + "' for word '" + word + "', n=" + str(n)
            failure=True
    if not failure:
        print "SUCCESS: test_getWordScore()"

# end of test_getWordScore


def test_updateHand():
    """
    Unit test for updateHand
    """
    # test 1
    handOrig = {'a':1, 'q':1, 'l':2, 'm':1, 'u':1, 'i':1}
    handCopy = handOrig.copy()
    word = "quail"

    hand2 = updateHand(handCopy, word)
    expectedHand1 = {'l':1, 'm':1}
    expectedHand2 = {'a':0, 'q':0, 'l':1, 'm':1, 'u':0, 'i':0}
    if hand2 != expectedHand1 and hand2 != expectedHand2:
        print "FAILURE: test_updateHand('"+ word +"', " + str(handOrig) + ")"
        print "\tReturned: ", hand2, "\n\t-- but expected:", expectedHand1, "or", expectedHand2

        return # exit function
    if handCopy != handOrig:
        print "FAILURE: test_updateHand('"+ word +"', " + str(handOrig) + ")"
        print "\tOriginal hand was", handOrig
        print "\tbut implementation of updateHand mutated the original hand!"
        print "\tNow the hand looks like this:", handCopy
        
        return # exit function
        
    # test 2
    handOrig = {'e':1, 'v':2, 'n':1, 'i':1, 'l':2}
    handCopy = handOrig.copy()
    word = "evil"

    hand2 = updateHand(handCopy, word)
    expectedHand1 = {'v':1, 'n':1, 'l':1}
    expectedHand2 = {'e':0, 'v':1, 'n':1, 'i':0, 'l':1}
    if hand2 != expectedHand1 and hand2 != expectedHand2:
        print "FAILURE: test_updateHand('"+ word +"', " + str(handOrig) + ")"        
        print "\tReturned: ", hand2, "\n\t-- but expected:", expectedHand1, "or", expectedHand2

        return # exit function

    if handCopy != handOrig:
        print "FAILURE: test_updateHand('"+ word +"', " + str(handOrig) + ")"
        print "\tOriginal hand was", handOrig
        print "\tbut implementation of updateHand mutated the original hand!"
        print "\tNow the hand looks like this:", handCopy
        
        return # exit function

    # test 3
    handOrig = {'h': 1, 'e': 1, 'l': 2, 'o': 1}
    handCopy = handOrig.copy()
    word = "hello"

    hand2 = updateHand(handCopy, word)
    expectedHand1 = {}
    expectedHand2 = {'h': 0, 'e': 0, 'l': 0, 'o': 0}
    if hand2 != expectedHand1 and hand2 != expectedHand2:
        print "FAILURE: test_updateHand('"+ word +"', " + str(handOrig) + ")"                
        print "\tReturned: ", hand2, "\n\t-- but expected:", expectedHand1, "or", expectedHand2
        
        return # exit function

    if handCopy != handOrig:
        print "FAILURE: test_updateHand('"+ word +"', " + str(handOrig) + ")"
        print "\tOriginal hand was", handOrig
        print "\tbut implementation of updateHand mutated the original hand!"
        print "\tNow the hand looks like this:", handCopy
        
        return # exit function

    print "SUCCESS: test_updateHand()"

# end of test_updateHand

def test_isValidWord(wordList):
    """
    Unit test for isValidWord
    """
    failure=False
    # test 1
    word = "hello"
    handOrig = getFrequencyDict(word)
    handCopy = handOrig.copy()

    if not isValidWord(word, handCopy, wordList):
        print "FAILURE: test_isValidWord()"
        print "\tExpected True, but got False for word: '" + word + "' and hand:", handOrig

        failure = True

    # Test a second time to see if wordList or hand has been modified
    if not isValidWord(word, handCopy, wordList):
        print "FAILURE: test_isValidWord()"

        if handCopy != handOrig:
            print "\tTesting word", word, "for a second time - be sure you're not modifying hand."
            print "\tAt this point, hand ought to be", handOrig, "but it is", handCopy

        else:
            print "\tTesting word", word, "for a second time - have you modified wordList?"
            wordInWL = word in wordList
            print "The word", word, "should be in wordList - is it?", wordInWL

        print "\tExpected True, but got False for word: '" + word + "' and hand:", handCopy

        failure = True


    # test 2
    hand = {'r': 1, 'a': 3, 'p': 2, 'e': 1, 't': 1, 'u':1}
    word = "rapture"

    if  isValidWord(word, hand, wordList):
        print "FAILURE: test_isValidWord()"
        print "\tExpected False, but got True for word: '" + word + "' and hand:", hand

        failure = True        

    # test 3
    hand = {'n': 1, 'h': 1, 'o': 1, 'y': 1, 'd':1, 'w':1, 'e': 2}
    word = "honey"

    if  not isValidWord(word, hand, wordList):
        print "FAILURE: test_isValidWord()"
        print "\tExpected True, but got False for word: '"+ word +"' and hand:", hand

        failure = True                        

    # test 4
    hand = {'r': 1, 'a': 3, 'p': 2, 't': 1, 'u':2}
    word = "honey"

    if  isValidWord(word, hand, wordList):
        print "FAILURE: test_isValidWord()"
        print "\tExpected False, but got True for word: '" + word + "' and hand:", hand
        
        failure = True

    # test 5
    hand = {'e':1, 'v':2, 'n':1, 'i':1, 'l':2}
    word = "evil"
    
    if  not isValidWord(word, hand, wordList):
        print "FAILURE: test_isValidWord()"
        print "\tExpected True, but got False for word: '" + word + "' and hand:", hand
        
        failure = True
        
    # test 6
    word = "even"

    if  isValidWord(word, hand, wordList):
        print "FAILURE: test_isValidWord()"
        print "\tExpected False, but got True for word: '" + word + "' and hand:", hand
        print "\t(If this is the only failure, make sure isValidWord() isn't mutating its inputs)"        
        
        failure = True        

    if not failure:
        print "SUCCESS: test_isValidWord()"


def test_calculateHandlen():
    handOrig = {'a':1, 'q':1, 'l':2, 'm':1, 'u':1, 'i':1}
    if calculateHandlen(handOrig) != 7:
        print "\tFAILURE: test_calculateHandlen()"
    else:
        print "SUCCESS: test_calculateHandlen()"


def test_playHand(wordList):
    hand1 = {'h':1, 'i':1, 'c':1, 'z':1, 'm':2, 'a':1}
    playHand(hand1, wordList, 7)
    hand2 = {'w':1, 's':1, 't':2, 'a':1, 'o':1, 'f':1}
    playHand(hand2, wordList, 7)
    hand3 = {'n':1, 'e':1, 't':1, 'a':1, 'r':1, 'i':2}
    playHand(hand3, wordList, 7)


def test_compChooseWord(wordList):
    if compChooseWord({'a': 1, 'p': 2, 's': 1, 'e': 1, 'l': 1}, wordList, 6) == "appels" \
            or compChooseWord({'a': 1, 'p': 2, 's': 1, 'e': 1, 'l': 1}, wordList, 6) == "apples" \
            and compChooseWord({'a': 2, 'c': 1, 'b': 1, 't': 1}, wordList, 5) == "acta" \
            and compChooseWord({'a': 2, 'e': 2, 'i': 2, 'm': 2, 'n': 2, 't': 2}, wordList, 12) == "immanent" \
            and not compChooseWord({'x': 2, 'z': 2, 'q': 2, 'n': 2, 't': 2}, wordList, 12):
        print "SUCCESS: test_playHand()"
    else:
        print "\tFAILURE: test_playHand()"


def test_compPlayHand(wordList):
    compPlayHand({'a': 1, 'p': 2, 's': 1, 'e': 1, 'l': 1}, wordList, 6)
    compPlayHand({'a': 2, 'c': 1, 'b': 1, 't': 1}, wordList, 5)
    compPlayHand({'a': 2, 'e': 2, 'i': 2, 'm': 2, 'n': 2, 't': 2}, wordList, 12)


wordList = loadWords()
print "----------------------------------------------------------------------"
print "Testing getWordScore..."
test_getWordScore()
print "----------------------------------------------------------------------"
print "Testing updateHand..."
test_updateHand()
print "----------------------------------------------------------------------"
print "Testing isValidWord..."
test_isValidWord(wordList)
print "----------------------------------------------------------------------"
print "Testing calculateHandlen..."
test_calculateHandlen()
print "----------------------------------------------------------------------"
#print "Testing playHand..."
#test_playHand(wordList)
#print "----------------------------------------------------------------------"
#print "Testing compChooseWord..."
#test_compChooseWord(wordList)
#print "----------------------------------------------------------------------"
#print "Testing compPlayHand..."
#test_compPlayHand(wordList)
#print "----------------------------------------------------------------------"
print "All done!"
