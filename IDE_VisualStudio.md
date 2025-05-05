# Visual Studio

## Ref

See CPP.md, CSharp.md and Database.md

## VS Code

<https://docs.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio-code#debug>

<https://code.visualstudio.com/docs/csharp/testing>

[Open with VS Code](https://stackoverflow.com/questions/64461301/open-folder-in-vs-code-from-windows-explorer)

- `[HKEY_CLASSES_ROOT\Directory\shell\VSCode]`: defines the display name and the icon when click a directory
- `[HKEY_CLASSES_ROOT\Directory\shell\VSCode\command]`: the command
- `[HKEY_CLASSES_ROOT\Directory\background\shell\VSCode]`: when click the background
- `[HKEY_CLASSES_ROOT\Directory\background\shell\VSCode\command]`

```Powershell
$registryKeyPath = "HKEY_CLASSES_ROOT\Directory\shell\VSCode\command"
REG EXPORT $registryKeyPath C:\val.txt /reg:64
```

<https://code.visualstudio.com/docs/getstarted/getting-started#_enhance-your-coding-with-ai-and-github-copilot>

- Workspace Trust: whether code in the folder can be executed
- Can have user vs. workspace settings. `File > Preferences > Settings`
- lightbulb icon, and then select Fix with Copilot.
- `Ctrl + I`

## VS tutorial

[Visual Studio](https://docs.microsoft.com/en-us/dotnet/articles/csharp/getting-started/with-visual-studio)

`Debug > Windows > Immediate` open a cmd which can interact with the app

F5 debug, F11 stepping

Condition break point

### Pic View

<https://docs.microsoft.com/en-us/visualstudio/ide/step-1-create-a-windows-forms-application-project>

Windows Forms Application project

- Add Leftside, Toolbox, TableLayoutPanel.
- Dock the TableLayoutPanel to the form. It means how the panel window is attached to the main window.
- Add Common controls, PictureBox to the TableLayoutPanel.
- Add Common controls, CheckBox to the TableLayoutPanel.
- Add Container, FlowLayoutPanel to the TableLayoutPanel.
- Add Common controls, Button to the FlowLayoutPanel.

### Show white spaces

ctrl+R and ctrl+w to toggle.

### Jump between braces

`ctrl+[` jump to the defination of the method. `ctrl+]` jump between braces.

### Connect to MySQL

1. Install connector and MySQL for VS: See Database.md
2. For VS 2013, Add reference, Assembiles, Extensions, MySQL.Data.

<https://stackoverflow.com/questions/21618015/how-to-connect-to-mysql-database>

### Plugins

VS plugin: [Funel](https://marketplace.visualstudio.com/items?itemName=DimitriDering.Funnel)

Reshapper plugin: for stycop tool

### Wildcard search

<https://docs.microsoft.com/en-us/sql/relational-databases/scripting/search-text-with-wildcards>

### tab management

<https://stackoverflow.com/questions/14254005/let-visual-studio-2012-2013-open-files-to-the-right-instead-of-to-the-left>

### Visual studio intellisense

[Bootstrap 4 Autocomplete in Visual Studio 2017](https://stackoverflow.com/questions/48629436/bootstrap-4-autocomplete-in-visual-studio-2017)

### Visual studio version

- Visual studio 2013: v12
- Visual studio 2017: v15

### Object Viewers

Can search for a class in DLLs.

### npm

`PATH=.\node_modules\.bin;C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\Microsoft\VisualStudio\NodeJs\win-x64;C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\Microsoft\VisualStudio\NodeJs;C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Web\External;%PATH%;C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\cmd;C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\mingw32\bin`

"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\Microsoft\VisualStudio\NodeJs\npm.CMD" install

### Troubleshooting

If build failed but the log doesn't help, try to use the Tools -> Command Line -> Developer Command Prompt to build the project.

### Visual Studio skip building non-changed projects

Turn on build log Level

- Tools > Options > Projects and Solutions > Build and Run > MSBuild project build log file verborsity

<https://stackoverflow.com/questions/71038929/preventing-vs-2019-from-rebuilding-a-solution-though-no-source-has-changed>

<https://stackoverflow.com/questions/14916729/visual-studio-rebuilds-unmodified-projects>

- Looking for log `Input file "Program.cs" is newer than output file "obj\Debug\net8.0\ParentConsoleApp.pdb".`

<https://developercommunity.visualstudio.com/t/there-is-no-easy-way-to-tell-why-a-project-rebuild/185102>

<https://stackoverflow.com/questions/1480008/why-does-vs-have-to-rebuild-all-of-my-projects-every-time-i-hit-play>

### Bad Image

Needs to unselect perfer 32 bit.
