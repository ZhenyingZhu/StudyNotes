class Frob(object):
    def __init__(self, name):
        self.name = name
        self.before = None
        self.after = None
    def setBefore(self, before):
        # example: a.setBefore(b) sets b before a
        self.before = before
    def setAfter(self, after):
        # example: a.setAfter(b) sets b after a
        self.after = after
    def getBefore(self):
        return self.before
    def getAfter(self):
        return self.after
    def myName(self):
        return self.name


def insert(atMe, newFrob):
    """
    atMe: a Frob that is part of a doubly linked list
    newFrob:  a Frob with no links
    This procedure appropriately inserts newFrob into the linked list that atMe is a part of.
    """
    prevNode = None
    nextNode = None

    if atMe.myName() > newFrob.myName():
        while atMe.getBefore():
            if atMe.getBefore().myName() <= newFrob.myName():
                break
            atMe = atMe.getBefore()

        prevNode = atMe.getBefore()
        nextNode = atMe
    else:
        while atMe.getAfter():
            if atMe.getAfter().myName() > newFrob.myName():
                break
            atMe = atMe.getAfter()

        prevNode = atMe
        nextNode = atMe.getAfter()

    if prevNode:
        prevNode.setAfter(newFrob)
        newFrob.setBefore(prevNode)
    if nextNode:
        newFrob.setAfter(nextNode)
        nextNode.setBefore(newFrob)


def findFront(start):
    """
    start: a Frob that is part of a doubly linked list
    returns: the Frob at the beginning of the linked list
    """
    if not start.getBefore():
        return start
    return findFront(start.getBefore())


# insert test
a = Frob('amara')
j1 = Frob('jennifer')
j2 = Frob('jennifer')
s = Frob('scott')
test_list = Frob('leonid')

insert(test_list, s)
insert(s, j1)
insert(s, j2)
insert(j1, a)

cur = a
res = ""
while(cur):
    res += " " + cur.myName()
    cur = cur.getAfter()

assert res == " amara jennifer jennifer leonid scott"