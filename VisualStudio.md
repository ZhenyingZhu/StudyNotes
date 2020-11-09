# Visual Studio

## Ref

See CPP.md, CSharp.md and Database.md

## VS tutorial

### VS build

<https://docs.microsoft.com/zh-CN/visualstudio/ide/get-started-with-visual-studio>

Building blocks

Solution:

- can have several projects
- can set reference between projects

To run a console app with arg, change project properties.

If tests failed. Check if the arch is changed to x64

Change the build configuration from Debug to Release, then build solution.

### Pic View

<https://docs.microsoft.com/en-us/visualstudio/ide/step-1-create-a-windows-forms-application-project>

Windows Forms Application project

- Add Leftside, Toolbox, TableLayoutPanel.
- Dock the TableLayoutPanel to the form. It means how the panel window is attached to the main window.
- Add Common controls, PictureBox to the TableLayoutPanel.
- Add Common controls, CheckBox to the TableLayoutPanel.
- Add Container, FlowLayoutPanel to the TableLayoutPanel.
- Add Common controls, Button to the FlowLayoutPanel.

### Timed quiz

<ttps://docs.microsoft.com/en-us/visualstudio/ide/tutorial-2-create-a-timed-math-quiz>

Add an event for a component and create an event handler.

**TODO**: <https://docs.microsoft.com/en-us/visualstudio/ide/step-9-try-other-features>

### Idle Master

#### Name conversion

- Forms elements start with small letter.

#### Windows regedit

C# class `Registry`, `RegistryKey`

#### ClickOnce

Project property

<https://docs.microsoft.com/en-us/visualstudio/deployment/clickonce-security-and-deployment>

1. provide updates automatically.
2. With Windows Installer deployment, applications often rely on shared components, but use ClickOnce, each application is self-contained and cannot interfere with other applications.
3. ClickOnce deployment enables non-administrative users to install.

#### Publish

Project property

#### Command Line Args

`string[] args = Environment.GetCommandLineArgs();`

`args[0]` is the assembling file.

#### Application

Start application with ex handling.

```C#
Application.ThreadException += (o, a) => Logger.Exception(a.Exception);
Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
Application.Run(new frmMain());
```

#### Reflection

<http://www.runoob.com/csharp/csharp-reflection.html>

`GetType().Assembly` is the executing assembly, which return the same thing as `System.Reflection.Assembly.GetExecutingAssembly()`.

#### Get resource from assembly

<http://blog.csdn.net/lyncai/article/details/12945069>

`assembly.GetManifestResourceStream`

Return null if the resource is not embedded resource.

It can be used as moving dll files. First add dll as embedded resource, and then

```C#
private void CopyResource(string resourceName, string file)
{
    using (var resource = GetType().Assembly.GetManifestResourceStream(resourceName))
    {
        if (resource == null)
        {
            return;
        }
        using (Stream output = File.OpenWrite(file))
        {
            resource.CopyTo(output);
        }
    }
}
```

Call

```C#
if (File.Exists(Environment.CurrentDirectory + "\\steam_api.dll") == false)
{
    CopyResource("IdleMaster.Resources.steam_api.dll", Environment.CurrentDirectory + @"\steam_api.dll");
}
```

#### Stream

Copy non-text file to another place.

```C#
Stream resource = xxx;
string file = "some place";

using (Stream output = File.OpenWrite(file))
{
    resource.CopyTo(output);
}
```

#### Form elements

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

#### How to add a resource

Add to resx. Then VS will generate a designer file. Move the code that related to the resource manager to the form code.

Relative path ??

#### app.config

`<userSettings>`

<https://www.cnblogs.com/yang-fei/p/4744698.html>

Edit Project `Settings.settings`, then include `using [Project].Properties;`.

There are some predefined config sections, for example `<appsetting>`. Add key value pairs in it.

[doc](https://docs.microsoft.com/en-us/dotnet/api/system.configuration.appsettingssection?view=netframework-4.8)

```xml
<configuration>
  <appSettings>
    <add key="Key" value="val"/>
  </appSettings>
```

```c#
string val = ConfigurationManager.AppSettings["Key"];
if (string.IsNullOrEmpty(val))
{
    val = defaultVal;
}
return val;
```

#### Create a new form

First add a form `newForm` to the project. Then add the code

```C#
Form newForm = new newForm();
newForm.ShowDialog();
```

#### localization

Use resx to store strings in different language.

resx can also create designer.

<https://stackoverflow.com/questions/1142802/how-to-use-localization-in-c-sharp>

## Show white spaces

ctrl+R and ctrl+w to toggle.

## Jump between braces

`ctrl+[` jump to the defination of the method. `ctrl+]` jump between braces.

## Connect to MySQL

1. Install connector and MySQL for VS: See Database.md
2. For VS 2013, Add reference, Assembiles, Extensions, MySQL.Data.

<https://stackoverflow.com/questions/21618015/how-to-connect-to-mysql-database>

## Plugins

VS plugin: [Funel](https://marketplace.visualstudio.com/items?itemName=DimitriDering.Funnel)

Reshapper plugin: for stycop tool

## Wildcard search

<https://docs.microsoft.com/en-us/sql/relational-databases/scripting/search-text-with-wildcards>

## tab management

<https://stackoverflow.com/questions/14254005/let-visual-studio-2012-2013-open-files-to-the-right-instead-of-to-the-left>

## Visual studio version

- Visual studio 2013: v12
- Visual studio 2017: v15

## Visual studio intellisense

[Bootstrap 4 Autocomplete in Visual Studio 2017](https://stackoverflow.com/questions/48629436/bootstrap-4-autocomplete-in-visual-studio-2017)

## npm

`PATH=.\node_modules\.bin;C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\Microsoft\VisualStudio\NodeJs\win-x64;C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\Microsoft\VisualStudio\NodeJs;C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Web\External;%PATH%;C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\cmd;C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\mingw32\bin`

"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\Microsoft\VisualStudio\NodeJs\npm.CMD" install

## MSBuild

[Doc](https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-concepts?view=vs-2019)

- Property: key/value pairs to config build
- Item: inputs to the build system. Usually files
- Task: atomic build op.
- Target: group tasks together.

[Item vs. Property](https://docs.microsoft.com/en-us/visualstudio/msbuild/comparing-properties-and-items?view=vs-2019)

- Property is attribute
- Item is object with metadata.

```xml
<ItemGroup>
    <OutputDir Include="KeyFiles\;Certificates\" />
</ItemGroup>
<PropertyGroup>
    <OutputDirList>@(OutputDir)</OutputDirList>
</PropertyGroup>
```

MSBuild bin path is under: `C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin`

[XmlUpdate](https://stackoverflow.com/questions/4526847/msbuild-xmlupdate-delete-node-in-web-config)

[XmlUpdate](https://stackoverflow.com/questions/1271840/how-do-i-update-an-xml-attribute-from-an-msbuild-script)

[XmlUpdate](http://geekswithblogs.net/paulwhitblog/archive/2006/04/11/74844.aspx)

Need to install the [MSBuildTasks](https://github.com/loresoft/msbuildtasks) nuget first: `Install-Package MSBuildTasks`

[XmlPoke](https://docs.microsoft.com/en-us/visualstudio/msbuild/xmlpoke-task?view=vs-2019)

- Usage: <https://stackoverflow.com/questions/56758367/xmlpoke-to-add-multiple-appsettings-key-doesnt-work>
- It is case sensitive.
- Add a key: <https://stackoverflow.com/questions/1366233/how-to-append-xml-nodes-using-existing-nant-or-nant-contrib-tasks>
