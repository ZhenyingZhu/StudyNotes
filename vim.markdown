## Config
- `~/.vimrc`
- `~/.viminfo`
- `/etc/vim/vimrc`

## Command mode
### Visual block
- `v`: characters select
- `V`: lines select
- `^v`: block select

- `%`: fast jump to the close of the partheses

## Last line mode
- `:reg`: show paste board

### Multi-doc editiing
`vim $file1 $file2`

- `:n`/`:N`: next/prev doc.
- `:files`
- `:sp file`: split the window vertically, and open the file. `^w+k`/`^w+j` switch between files.

### Search and replace
[vim search](http://vim.wikia.com/wiki/Search_and_replace>Replease and search tutorial)

- `:%s/old/new/g`: `%` means all lines. `g` means replace all apperances of pattern old in the line.













## tmp
Run shell cmd in vim: https://www.linux.com/learn/tutorials/442419-vim-tips-working-with-external-commands  

http://vimdoc.sourceforge.net/htmldoc/filetype.html  

[Vundle](https://github.com/gmarik/Vundle.vim)  
```
Plugin 'scrooloose/nerdtree'
Plugin 'godlygeek/tabular'
Plugin 'plasticboy/vim-markdown'
```
```
:so ~/.vimrc
:PluginInstall
```

[vim plugin](http://joelhooks.com/blog/2013/04/23/5-essential-vim-plugins/)  

Add in the last line of a plain text file.
```
# vim: set ts=4 sw=4 sts=4 et:
```

