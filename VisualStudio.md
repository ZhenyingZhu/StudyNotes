# Ref
See CPP.md, CSharp.md and Database.md


# VS tutorial

## VS build
https://docs.microsoft.com/zh-CN/visualstudio/ide/get-started-with-visual-studio

Building blocks
Solution:
- can have several projects
- can set reference between projects

To run a console app with arg, change project properties.

If tests failed. Check if the arch is changed to x64

Change the build configuration from Debug to Release, then build solution.

## Pic View
https://docs.microsoft.com/en-us/visualstudio/ide/step-1-create-a-windows-forms-application-project

Windows Forms Application project
- Add Leftside, Toolbox, TableLayoutPanel.
- Dock the TableLayoutPanel to the form. It means how the panel window is attached to the main window.
- Add Common controls, PictureBox to the TableLayoutPanel.
- Add Common controls, CheckBox to the TableLayoutPanel.
- Add Container, FlowLayoutPanel to the TableLayoutPanel.
- Add Common controls, Button to the FlowLayoutPanel.

## Timed quiz
https://docs.microsoft.com/en-us/visualstudio/ide/tutorial-2-create-a-timed-math-quiz

Add an event for a component and create an event handler.

HERE: https://docs.microsoft.com/en-us/visualstudio/ide/step-9-try-other-features

## Idle Master
### Name conversion
- Forms elements start with small letter.

### Windows regedit
C# class `Registry`, `RegistryKey`

### ClickOnce
Project property

https://docs.microsoft.com/en-us/visualstudio/deployment/clickonce-security-and-deployment

1. provide updates automatically.
2. With Windows Installer deployment, applications often rely on shared components, but use ClickOnce, each application is self-contained and cannot interfere with other applications.
3. ClickOnce deployment enables non-administrative users to install.

### Publish
Project property

### Command Line Args
`string[] args = Environment.GetCommandLineArgs();`

`args[0]` is the assembling file.

### Application
Start application with ex handling.
```
Application.ThreadException += (o, a) => Logger.Exception(a.Exception);
Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
Application.Run(new frmMain());
```

### Form elements
ToolStripMenuItem
- `&About` draw an underline under "A"

Button
- Click Event

Form
- FormClosed Event
- Load Event

ColumnHeader
- In the ListView

ListView
- A list

Label

LinkLabel
- LinkClicked Event

ToolStripStatusLabel
- The text to display on the strip
- vs Label ??

MenuStrip
- The container to place ToolStripMenuItem

NotifyIcon
- icon to display in the system tray
- not place on the form

ToolStripProcessBar
- Process bar

PictureBox
- display the picture

StatusStrip
- Can place ToolStripStatusLabel

Timer
- Tick Event
- not place on the form

ToolStripSeparator
- A line between ToolStripItems

TableLayoutPanel
- A container with a table

NumericUpDown

ColorDialog
- select a color
- not place on the form

FlowLayoutPanel
- Property controls can add CheckedBoxes.

OpenFileDialog
- not place on the form

TextBox
- [How to limit the char to number](http://blog.csdn.net/hjingtao/article/details/7302448)
- How to limit the number of char: change property MaxLength


### How to add a resource
Add to resx. Then VS will generate a designer file. Move the code that related to the resource manager to the form code.

Relative path ??

### app.config
`<userSettings>`

https://www.cnblogs.com/yang-fei/p/4744698.html

Edit Project `Settings.settings`, then include `using [Project].Properties;`.

### localization
Use resx to store strings in different language.


# Show white spaces
ctrl+R and ctrl+w to toggle.


# Connect to MySQL
1. Install connector and MySQL for VS: See Database.md
2. For VS 2013, Add reference, Assembiles, Extensions, MySQL.Data.

https://stackoverflow.com/questions/21618015/how-to-connect-to-mysql-database


# Plugins
VS plugin: Funel https://marketplace.visualstudio.com/items?itemName=DimitriDering.Funnel

Reshapper plugin: for stycop tool
