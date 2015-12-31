
Merge remote repo to current folder
```
svn merge remote_another_folder
```

```
svn copy -m "Copy from one repo to another" remote_repo1 remote_repo2
```
Notice is remote_repo2/, then it will copy to under remote_repo2, while if remote_repo2, it will replace remote_repo2.  

```
svn -N [--non-recursive]     : obsolete; try --depth=files or --depth=immediates  
```
Then use `svn up folder` to co only that repo.  


diff -r --exclude=".svn" src1/ src2/  

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
- `M` an added file with modification
- `D` deleted file
- `A +` renamed file? 


Add: otherwise the file is not monitored 
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
svn log -v -r 1
svn log file
```

See the diff between revisions: 
```
svn diff -r 1:2
svn diff -r 2
```

Change file name: 
```
svn mv file 
```

Undo changes that not committed:
```
svn revert -R .
```

Return to a previous reversion: 
```
 svn merge -r 4:3 .
```

http://www.cade.utah.edu/faqs/how-do-i-set-up-users-for-my-svn-repository/
http://stackoverflow.com/questions/8159078/being-prompted-for-null-gnome-keyring  
http://svnbook.red-bean.com/en/1.7/svn.tour.importing.html  


