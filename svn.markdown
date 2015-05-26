http://www.redhat.com/magazine/010aug05/features/subversion/  

Create a repo: 
```
svnadmin create --fs-type fsfs svnrepo
```

Checkout a repo:  
```
svn checkout file://$HOME/svnrepo $HOME/checkout
```

Check status:  
```
svn status
```
- `?` means the file is not in HEAD
- `A` added not committed



Add: 
```
svn add file
```

Commit: will create a revision. But not add to HEAD 
```
svn commit -m 'msg'
```

Update: add revision to HEAD
```
svn update
```

View log: 
```
svn log
```

See the diff between revisions: 
```
svn diff 0:1
```



