
# BeautifulSoup
[BeautifulSoup Doc](http://www.crummy.com/software/BeautifulSoup/bs4/doc.zh/)  

## Objects

### BeautifulSoup Object
```
from bs4 import BeautifulSoup

soup = BeautifulSoup("<html>data</html>")
```

### Tag Object
`Tag` is same as HTML tag, access tag by using its name: 
- `tag = soup.b`, tag is the first of `<b>content</b>` in the page.  
- `tag.name`: one of the attribute of tag objects.  
- `tag.attrs` the other attribute of tag objects. is a dict. 
- `tag['attr_name']` can also reach the attribute.  
- If this attribute can contain serveral values, a list presents.  

### NavigableString Object
`tag.string` is an NavigableString object.  
Use `unicode(tag.string)` to convert it to unicode string.  

### Comment Object
If a NavigableString has tag `<!-- -->`, it is a comment object.  

## HTML Tree

### Sub Node
Except NavigableString, other objects can have sub nodes. E.g. `soup.body.b`.  

Access tag:  
- `soup.a`. Using dot, can only access the first tag of the kind of this name.  
- `soup.find_all(a)`: return a list.  
- attribute `tag.contents` return a list of sub nodes. Without the tag itself.  

```
# <a class="sister" href="http://example.com/tillie" id="link3">Tillie</a>
last_a_tag = soup.find("a", id="link3")
```

Tag `<html>` is a sub node of `BeautifulSoup` object.  

Traverse through all the direct sub nodes:  
```
for child in soup.body.children:
    print child
```

Iterate traverse through all the sub nodes(With all parent nodes print out):  
```
for child in soup.descendants:
    print child
```

`tag.string` could be `None` when tag has multiple sub nodes.  

Traverse through all strings:  
```
for string in soup.strings:
    print(repr(string)) # print limit-size string
```

`soup.stripped_strings` remove duplicate spaces and empty lines.  

`tag.parent`: parent of `<html>` is `BeautifulSoup`.  

`tag.parents`: use for traverse.  

Sibling: 
- Have the same parent.  
- Have the same level when print out in pretty form.  
- Sibling can be space or enter.  
- 
```
sibling_soup = BeautifulSoup("<a><b>text1</b><c>text2</c></b></a>")
print(sibling_soup.prettify()) # here b and c

sibling_soup.b.next_sibling # c
sibling_soup.c.previous_sibling # b
```

`tag.next_siblings` and `tag.previous_siblings`.  


### HTML parser behave
Parser executes a series of actions. `tag.next_element` do the same thing. `tag.previous_element` do the reverse way.  

`tag.next_elements` and `tag.previous_elements`.  

## Search
### Functions
Filter:  
- string: `soup.find_all('b')` Default string is UTF-8.  
- regex: `soup.find_all(re.compile("^b"))`  
- list: `soup.find_all(['a', 'b'])`  
- True: `soup.find_all(True)`. Find all elements except NavigableString.  
- function: `soup.find_all(isTrue)`. Find elements when function `isTrue(tag)` return True.  
- 

`find_all(name , attrs , recursive , text , **kwargs)`:  
- `soup.find_all("p", "title")`: result `[<p class="title"><b>The Dormouse's story</b></p>]`  
- name can be tag: `soup.find_all(id=True)`, `soup.find_all(href=re.compile("elsie"))`, `soup.find_all(href=re.compile("elsie"), id='link1')`  
- `data_soup.find_all(attrs={"data-foo": "value"})` result: `[<div data-foo="value">foo!</div>]`  
- Find CSS class: `soup.find_all("a", class_="sister")`  
- `soup.find_all("a", text="Elsie")`  
- Stop when find n results: `soup.find_all("a", limit=n)`  
- Default find_all search recursivly. `soup.html.find_all("title", recursive=False)`  

Other find methods: `find_parents()`, `find_parent()`, `find_next_siblings()`, `find_next_sibling()`, `find_previous_siblings()`, `find_previous_sibling()`, `find_all_next()`, `find_next()`, `find_all_previous()`, `find_previous()`, 

### CSS select
http://www.crummy.com/software/BeautifulSoup/bs4/doc.zh/#id37

## Nodify
http://www.crummy.com/software/BeautifulSoup/bs4/doc.zh/#id40

## Output
http://www.crummy.com/software/BeautifulSoup/bs4/doc.zh/#id44

## Encoding
http://www.crummy.com/software/BeautifulSoup/bs4/doc.zh/#id51

