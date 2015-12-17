[BeautifulSoup Doc](http://www.crummy.com/software/BeautifulSoup/bs4/doc.zh/)  

# BeautifulSoup Object
```
from bs4 import BeautifulSoup

soup = BeautifulSoup("<html>data</html>")
```

# Tag Object
`Tag` is same as HTML tag: 
- `tag = soup.b`, tag is the first element of `<b>content</b>` in the page.  
- `tag.name`
- `tag.attrs` is a dict. 
- `tag['attr_name']` can also reach the attribute. 
- If this attribute can contain serveral values, a list presents.  

# NavigableString Object
`tag.string` is an NavigableString object.  
Use `unicode(tag.string)` to convert it to unicode string.  

# Comment Object
If a NavigableString has tag `<!-- -->`, it is a comment object.  

