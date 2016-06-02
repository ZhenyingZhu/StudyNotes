
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

[svn update](http://stackoverflow.com/questions/1109723/subversion-resolve-all-conflicts-quickly)  
To let stupid svn take all the changes from others:  
```
svn update . --accept theirs-full
```

http://stackoverflow.com/questions/1071857/how-do-i-svn-add-all-unversioned-files-to-svn  
`svn add --force * --auto-props --parents --depth infinity -q`



svn cannot commit after merge:  
with this error: local add/delete, incoming add/delete upon merge  
`svn resolved file`


svn `mine-conflicts`: use all my changes about the conflicts but normal merge about other changes; `mine-full`: discard all their changes. 

svn cherrypick: http://svnbook.red-bean.com/en/1.6/svn.branchmerge.advanced.html

svn mergeinfo --show-revs eligible remoteURL
 
