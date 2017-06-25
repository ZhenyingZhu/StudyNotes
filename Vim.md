# VIM

## Turtorial

### Hard Way
http://learnvimscriptthehardway.stevelosh.com/chapters/00.html

`:echo` and `:echom`: `:messages` can see `echom` but not `echo`

comment: start with `"`

`:set <field>?` to check, `:set <field>!` or `:set no<field>` to toggle

#### Map
[map keys](http://learnvimscriptthehardway.stevelosh.com/chapters/03.html)

`:vsplit` and `:split`

[abbr](http://learnvimscriptthehardway.stevelosh.com/chapters/08.html)

`:setlocal` perbuff

#### Auto command
```
:edit foo " open file
:quit
```

`:help autocmd-events` see all events

To execute a command when "BufNewFile" event happened on the pattern ".txt"
```
:autocmd BufNewFile *.txt :write
```

For now, let's take it as vim will reindent the current file
```
:normal gg=G
```

A common idiom in vim scripting is to pair the "BufRead" and "BufNewFile" events together

"FileType" is fired whenever Vim sets a buffer's "filetype".
```
:autocmd FileType python nnoremap <buffer> <localleader>c I#<esc>
```

To avoid autocmd defined multiple times
```
augroup filetype_html
    autocmd!
    autocmd FileType html nnoremap <buffer> <localleader>f Vatzf
augroup END
```

#### Operator
`d`, `y`, `c` are operators that wait for a movement

[operator mapping](http://learnvimscriptthehardway.stevelosh.com/chapters/15.html)

#### Status line
[format status line](http://learnvimscriptthehardway.stevelosh.com/chapters/17.html)
- `%f`: Path to the file
- `%y`: Filetype of the file

#### Write VIM scripts
[Responsible Coding](http://learnvimscriptthehardway.stevelosh.com/chapters/18.html)


## Config

### location
- `~/.vimrc`
- `~/.viminfo`
- `/etc/vim/vimrc`
- `~/.vim/indent/python.vim` contains filetype indent.

### options
See Command mode section as well.

## VIM Modes

### Normal mode

#### Visual block
- `v`: characters select
- `V`: lines select
- `^v`: block select

- `%`: fast jump to the close of the partheses

#### gf
Quick open file that under cursor in another window

`ctrl+w` then `ctrl+f`

#### folding
[Intro](http://vim.wikia.com/wiki/Folding)

`:set fdm=syntax` to enable folding.

`za` to toggle current fold.


### Visual Mode


### Insert Mode


### Command mode
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
- `:set mouse=a`, `:set mouse=`: enable/disable mouse mode. After enabled, cannot copy by `ctrl+shift+c`.

#### Multi-doc editiing
`vim $file1 $file2`

- `:n`/`:N`: next/prev doc.
- `:files`
- `:sp file`: split the window vertically, and open the file. `^w+k`/`^w+j` switch between files.

#### Search and replace
[vim search](http://vim.wikia.com/wiki/Search_and_replace>Replease and search tutorial)

- `:%s/old/new/g`: `%` means all lines. `g` means replace all apperances of pattern old in the line.



## plugin
[如何将Vim打造成一个成熟的IDE](https://linux.cn/article-3314-1.html)

### Pathogen
[Intro](https://github.com/tpope/vim-pathogen)

Install plugins
```
cd ~/.vim/bundle && git clone git://github.com/tpope/vim-sensible.git
```

Run `:Helptags` to load help docs. Then `:help $plugin_name`


### SuperTab
[Intro](https://github.com/ervandew/supertab)

After press `tab`, a list of choices shows up. `ctrl+n` and `ctrl+p` switch between them, and then keep typing.


### syntastic
[Intro](https://github.com/scrooloose/syntastic)

[C++11 support](http://stackoverflow.com/questions/18158772/how-to-add-c11-support-to-syntastic-vim-plugin): add to `vimrc`
```
let g:syntastic_cpp_compiler = 'clang++'
let g:syntastic_cpp_compiler_options = ' -std=c++11 -stdlib=libc++'
```

<b>Need some other plugins to make it fully work. Not finish yet</b>


### NERDTree
[Intro](https://github.com/scrooloose/nerdtree)

Add to `vimrc`
```
autocmd vimenter * NERDTree
```

Shortcuts:
- `t`: Open the selected file in a new tab
- `i`: Open the selected file in a horizontal split window
- `s`: Open the selected file in a vertical split window
- `I`: Toggle hidden files
- `m`: Show the NERD Tree menu
- `R`: Refresh the tree, useful if files change outside of Vim
- `?`: Toggle NERD Tree's quick help

### undotree
[info](https://github.com/mbbill/undotree)

### vim-markdown
[Info](https://github.com/tpope/vim-markdown)


## Tool chain
[Linux 平台下阅读源码的工具链](http://blog.jobbole.com/101322/)

### ctags
At the src root folder, call `ctags -R`, a `tags` file will be generated. Then use vim to open src files from root folder.

- `ctrl+]`: goto
- `ctrl+t`: return back


### cscope for C
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


### doxygen for C++
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

[Vim and PEP 8 — Style Guide for Python Code](http://stackoverflow.com/questions/9864543/vim-and-pep-8-style-guide-for-python-code)

[turning vim into a modern python ide](https://www.reddit.com/r/Python/comments/h75fm/turning_vim_into_a_modern_python_ide/)

[Vim documentation: syntax](http://vimdoc.sourceforge.net/htmldoc/syntax.html#mysyntaxfile)

[tpope/vim-markdown](https://github.com/tpope/vim-markdown)

[C++ vim setting](https://gist.github.com/rocarvaj/2513367)  
[C++ IDE](http://www.alexeyshmalko.com/2014/using-vim-as-c-cpp-ide/)  

[Define own highlight](https://superuser.com/questions/194459/vim-syntax-highlighting-how-to-stop-the-automatic-underlining-of-a-href)
copy /usr/share/vim/vim74/syntax/markdown.vim to ~/.vim/syntax, and remove `_` from markdownError in it

[Keep undo history](https://askubuntu.com/questions/292/how-do-i-get-vim-to-keep-its-undo-history)
```
set undofile
set undodir=/home/yourname/.vimundo/
```

