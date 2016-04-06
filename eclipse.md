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

[Setting CDT to recognize C++11](http://wiki.eclipse.org/CDT/User/FAQ#CDT_does_not_recognize_C.2B.2B11_features)
1. Project -> Properties -> C/C++ General, click Configure Workspace Settings...
2. C/C++ -> Build -> Setting, Discovery tab, setting CDT GCC Built-in Compiler Settings to be `${COMMAND} ${FLAGS} -E -P -v -dD "${INPUTS}" -std=c++0x`
3. Reindex


