# Config
## location
- `~/.vimrc`
- `~/.viminfo`
- `/etc/vim/vimrc`
- `~/.vim/indent/python.vim` contains filetype indent.


## options
See Command mode section as well.



# VIM Modes
## Normal mode
### Visual block
- `v`: characters select
- `V`: lines select
- `^v`: block select

- `%`: fast jump to the close of the partheses

### gf
Quick open file that under cursor in another window

`ctrl+w` then `ctrl+f`

## Visual Mode


## Insert Mode


## Command mode
[cmds](http://vim.wikia.com/wiki/Displaying_the_current_Vim_environment)

- `:help cmd`
- `:reg`: show paste board
- `:let`: show all variables<b>?</b>
- `:set all`: show all setting. If some settings are not enable, there are "no" before them.
- `:set nu`, `:set nu!`
- `:set showcmd`: in visual mode, show more information at the right down corner.
- `:set cursorline`
- `:set wildmenu`: press tab to auto complete vim cmds
- `:set showmatch`: when complete a pair of braces, cursor jumps back to the start of brace and jump back at once.
- `:set incsearch`: search when char are type in


### Multi-doc editiing
`vim $file1 $file2`

- `:n`/`:N`: next/prev doc.
- `:files`
- `:sp file`: split the window vertically, and open the file. `^w+k`/`^w+j` switch between files.

### Search and replace
[vim search](http://vim.wikia.com/wiki/Search_and_replace>Replease and search tutorial)

- `:%s/old/new/g`: `%` means all lines. `g` means replace all apperances of pattern old in the line.



# Tool chain
[Linux 平台下阅读源码的工具链](http://blog.jobbole.com/101322/)

## ctags
At the src root folder, call `ctags -R`, a `tags` file will be generated. Then use vim to open src files from root folder.

- `ctrl+]`: goto
- `ctrl+t`: return back


## cscope for C
Src root folder, `cscope -Rbq`, a `cscope.out` database will be generated.

In vim, 
- `:cs add cscope.out`: add datebase
- `:cs find [mode] $var`. mode:
  - `s`: Find this C symbol
  - `g`: Find this definition
  - `d`: Find functions called by this function
  - `c`: Find functions calling this function
  - `t`: Find this text string
  - `e`: Find this egrep pattern
  - `f`: Find this file
  - `i`: Find files #including this file


## doxygen for C++
To create documents and draw structure graphs.

Install:
- `sudo apt install doxygen`
- `sudo apt install graphviz` to install dot

Src root folder, `doxygen -g`. Will generate `Doxyfile`, which is a config.

Configuration: edit `Doxyfile`
- `HAVE_DOT` set to yes, which is used to draw graph
- `RECURSIVE = YES`
- `CALL_GRAPH`, `CALLER_GRAPH`: generate function graphs
- `EXTRACT_ALL`, `EXTRACT_STATIC`, `EXTRACT_PRIVATE`: write variables to documents.

Then call `doxygen Doxyfile`. Will create html and latex two folders.








# tmp
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

[Vim and PEP 8 — Style Guide for Python Code](http://stackoverflow.com/questions/9864543/vim-and-pep-8-style-guide-for-python-code)

[turning vim into a modern python ide](https://www.reddit.com/r/Python/comments/h75fm/turning_vim_into_a_modern_python_ide/)

[Vim documentation: syntax](http://vimdoc.sourceforge.net/htmldoc/syntax.html#mysyntaxfile)

[tpope/vim-markdown](https://github.com/tpope/vim-markdown)

[C++ vim setting](https://gist.github.com/rocarvaj/2513367)  
[C++ IDE](http://www.alexeyshmalko.com/2014/using-vim-as-c-cpp-ide/)  

