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

HERE: https://docs.microsoft.com/en-us/visualstudio/ide/step-8-customize-the-quiz


# Show white spaces
ctrl+R and ctrl+w to toggle.


# Connect to MySQL
1. Install connector and MySQL for VS: See Database.md
2. For VS 2013, Add reference, Assembiles, Extensions, MySQL.Data.

https://stackoverflow.com/questions/21618015/how-to-connect-to-mysql-database


# Plugins
VS plugin: Funel https://marketplace.visualstudio.com/items?itemName=DimitriDering.Funnel

Reshapper plugin: for stycop tool
