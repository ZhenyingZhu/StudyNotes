# Idle Master

TODO: Move notes to DesktopApp.md.

## Name conversion

- Forms elements start with small letter.

## Windows regedit

C# class `Registry`, `RegistryKey`

## ClickOnce

Project property

<https://docs.microsoft.com/en-us/visualstudio/deployment/clickonce-security-and-deployment>

1. provide updates automatically.
2. With Windows Installer deployment, applications often rely on shared components, but use ClickOnce, each application is self-contained and cannot interfere with other applications.
3. ClickOnce deployment enables non-administrative users to install.

## Publish

Project property

## Command Line Args

`string[] args = Environment.GetCommandLineArgs();`

`args[0]` is the assembling file.

## Application

Start application with ex handling.

```C#
Application.ThreadException += (o, a) => Logger.Exception(a.Exception);
Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
Application.Run(new frmMain());
```

## Reflection

<http://www.runoob.com/csharp/csharp-reflection.html>

`GetType().Assembly` is the executing assembly, which return the same thing as `System.Reflection.Assembly.GetExecutingAssembly()`.

## Get resource from assembly

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

## Stream

Copy non-text file to another place.

```C#
Stream resource = xxx;
string file = "some place";

using (Stream output = File.OpenWrite(file))
{
    resource.CopyTo(output);
}
```

## Form elements

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

## How to add a resource

Add to resx. Then VS will generate a designer file. Move the code that related to the resource manager to the form code.

Relative path ??

## app.config

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

## Create a new form

First add a form `newForm` to the project. Then add the code

```C#
Form newForm = new newForm();
newForm.ShowDialog();
```

## localization

Use resx to store strings in different language.

resx can also create designer.

<https://stackoverflow.com/questions/1142802/how-to-use-localization-in-c-sharp>

## Error

The steam-idle.exe and Steamworks.NET.dll are out-of-dated. Download the release one from the website an replace my build IdleMaster.exe with the one in it makes it work.
Steamworks.NET.dll is not compatiable with x64. Build the solution with x86 solve the issue.
