# Shortcuts
[10 best eclipse shortcuts](https://dzone.com/articles/10-best-eclipse-shortcuts)  
[Most useful shortcut in Eclipse CDT](http://stackoverflow.com/questions/1266862/most-useful-shortcut-in-eclipse-cdt)  

Most common shortcuts:  
- `F3`: Find reference.  
- `ctrl+left-click`: Go to the definition of the item.  
- `alt+left/right`: shift between tabs.  
- `ctrl+alt+h`: see call hierarchy of a function.  
- `ctrl+shift+g`: find all apperances of a variable.  
- `ctrl+shift+t`: search special type of elements with wildcast.  
- `ctrl+shift+/(numpad)`: collapse all code blocks.  

- `ctrl+shift+l`: show shortcuts list.  
- `ctrl+/`: block comment all choosen codes.  
- `ctrl+space`: show the auto-complete options manually.  
- `ctrl+3`: quick access, can search.  
- `ctrl+m`: maximize/back the current window.  
- `ctrl+F7`, `ctrl+shift+F7`: switch between views.  
- `ctrl+o`: open outline view.  
- `ctrl+tab`: CDT, switch between head and source files when both of them are opened.  
- `alt+shift+r,n`: Rename a function or variable throughout a project.  

# install
[Install eclipse](http://difusal.blogspot.com/2015/06/how-to-install-eclipse-mars-45-on-ubuntu.html)  


# desktop entry
[fix eclipse in launcher](http://askubuntu.com/questions/80013/how-to-pin-eclipse-to-the-unity-launcher)

# C++11 support
Install Eclipse CDT:  
Find one URL here http://www.eclipse.org/cdt/downloads.php and add to eclipse install new software.  
If running on windows, get rid of workplace/.metadata/.plugins/org.eclipse.core.resources/.snap, and turn off Indexing and automatic builds in CDT  
Setup boost: https://theseekersquill.wordpress.com/2010/08/24/howto-boost-mingw/  

[Setting CDT to recognize C++11](http://wiki.eclipse.org/CDT/User/FAQ#CDT_does_not_recognize_C.2B.2B11_features)
1. Project -> Properties -> C/C++ General, click Configure Workspace Settings...
2. C/C++ -> Build -> Setting, Discovery tab, setting CDT GCC Built-in Compiler Settings to be `${COMMAND} ${FLAGS} -E -P -v -dD "${INPUTS}" -std=c++0x`
3. Reindex

This one works [eclipse C++11](http://stackoverflow.com/questions/8312854/eclipse-indexer-cant-resolve-shared-ptr)

A better way [C++ build tool Dialect select C++11](https://www.eclipse.org/forums/index.php/t/1070790/)
under C/C++ Build -> Settings -> Tool Settings. Each compiler on this page has a Dialect entry. You can pick C++11 from this page.

[Eclipse C++11](http://stackoverflow.com/questions/17131744/eclipse-cdt-indexer-does-not-know-c11-containers)
 Does not work

# Other language
ruby enviroment for eclipse https://eclipse.org/dltk/install.php

# Editor setting
[eclipse](http://stackoverflow.com/questions/11596194/how-does-one-show-trailing-whitespace-in-eclipse) 
eclipse show line number:  
Window > Preferences > General > Editors > Text Editors 
eclipse change tab to space:  
Beside the previous step, do
Window > Preferences > [Specific launguage] > Code Style > Formatter: create a profile with tab policy change to space only.  


# problems
[eclipse in ubuntu 16](http://askubuntu.com/questions/761604/eclipse-not-working-in-16-04)

C++ on eclispe: http://stackoverflow.com/questions/4971926/launch-failed-binary-not-found-cdt-on-eclipse-helios


