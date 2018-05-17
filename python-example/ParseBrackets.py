"""Test parse brackets."""
from pyparsing import nestedExpr
from lepl import Any, Delayed, Node, Space

txt = "[a[b=1][c=3]]"

lst = nestedExpr('{','}').parseString(txt).asList()

expr = Delayed()
expr += '{' / (Any() | expr[1:,Space()[:]]) / '}' > Node
print(expr.parse("{{a}{b}{{{c}}}}")[0])