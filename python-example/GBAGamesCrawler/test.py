import urllib.request
f = urllib.request.urlopen("http://stackoverflow.com")
content = f.read()
print(content)