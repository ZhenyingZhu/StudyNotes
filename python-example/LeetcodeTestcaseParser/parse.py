f = open('Case', 'r')
ops = f.readline().split(',')
args = f.readline().split(',')

cmds = []
for i in range(0, len(ops)):
    cmd = ""
    if ops[i] == '"inc"':
        cmd += "obj.inc"
    elif ops[i] == '"dec"':
        cmd += "obj.dec"
    else:
        print ops[i] + " " + args[i]
        continue

    cmd += "(" + args[i][1:-1] + ");"
    cmds.append(cmd)

cnt = 1
# 0-9999 is inc("a")
# 10000 is inc["b"]
for i in range(10000, len(cmds) - 2, 2):
    if cmds[i] ==  'obj.inc("b");' and cmds[i + 1] == 'obj.dec("b");':
        cnt += 1
    else:
        print cmds[i] + " " + cmds[i + 1] + " " + str(cnt)
        cnt = 1

print cnt
#print cmds[10000] + " " + str(cnt)
