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

[Copy and Rename](https://stackoverflow.com/questions/58561928/how-to-copy-and-rename-file-to-output-folder-as-part-of-build)

- `<Copy SourceFiles="@(Something)" DestinationFolder="$(OutputPath)\$(ArtifactName)\%(RecursiveDir)"/>`
- [Copy Task](https://docs.microsoft.com/en-us/visualstudio/msbuild/copy-task?view=vs-2019)

[well-known item metadata](https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-well-known-item-metadata?view=vs-2019)

- `%(RecursiveDir)`

[Write my own task](https://docs.microsoft.com/en-us/visualstudio/msbuild/task-writing?view=vs-2019)

Different build tools VS uses: [How do I compile a Visual Studio project from the command-line?](https://stackoverflow.com/questions/498106/how-do-i-compile-a-visual-studio-project-from-the-command-line)

- msbuild
- devenv

[slngen](https://microsoft.github.io/slngen/)

- `for /f %c in ('dir /b *.csproj dirs.proj') do %PkgMicrosoft_VisualStudio_SlnGen%\tools\net472\slngen.exe -vs "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\devenv.exe" --folders true %c` to generate sln file and start VS2022.

## NuGet

[NuGet intro](https://docs.microsoft.com/en-us/nuget/what-is-nuget)

- a NuGet package is a single ZIP file with the .nupkg extension that contains compiled code (DLLs), other files related to that code, and a descriptive manifest that includes information like the package's version number.
- `nuget restore` or `dotnet restore` or `Install-Package`
- `PackageReference`: in the csproj; or `packages.config` under the project root folder.
- MSBuild CLI has the ability to restore packages, but it is mainly used for build server.
- the package needs to support the same target framework as the project.
- To create a nuget package:
  - In the csproj, add `PackageId`, `Version`, `Authors` and `Company`. Those values will be in the `.nuspec`.
  - run `dotnet pack`, which will create a nuget package locally.
  - If add `<GeneratePackageOnBuild>true</GeneratePackageOnBuild>` in the csproj, the package will be generated on build.
  - `dotnet nuget push` to publish. Need to reg on nuget.org and get a API key.

**HERE**: <https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio>

[NuProj doc](http://nuproj.net/documentation/)

The file `nuget.config` is actually in the parent folder of the sln. If see any nuget not accessiable, check if the source defined in this config is wrong.

If dotnet restore failed with 401, install `https://github.com/Microsoft/artifacts-credprovider`

## Unit test

MSTest vs. NUnit

Solving UT not found issue

```text
Warning: No test is available in UnitTest.dll. Make sure that installed test discoverers & executors, platform & framework version settings are appropriate and try again.
```

[vstest.console.exe not discovering any of the tests](https://social.msdn.microsoft.com/Forums/vstudio/en-US/f44db2d5-61ae-428b-8412-5a3fc739daf7/vstestconsoleexe-not-discovering-any-of-the-tests?forum=vstest)

[No test found. Make sure that installed test discoverers & executors, platform & framework version settings are appropriate and try again](https://stackoverflow.com/questions/34790339/no-test-found-make-sure-that-installed-test-discoverers-executors-platform)

- [unit test doc](https://docs.microsoft.com/en-us/visualstudio/test/run-a-unit-test-as-a-64-bit-process?view=vs-2019) `<TargetPlatform>x64</TargetPlatform>`.

MSBuild has so many weird errors!

In the UT, if use Console.WriteLine, it logs in the trx.

## Some Really Weird Build Issue

<https://stackoverflow.com/questions/42867434/could-not-load-file-or-assembly-system-valuetuple>

```C#
  <dependentAssembly>
    <assemblyIdentity name="System.ValueTuple" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
    <bindingRedirect oldVersion="0.0.0.0-4.0.3.0" newVersion="4.0.3.0" />
  </dependentAssembly>
```

How to check a DLL version: <https://stackoverflow.com/questions/29772065/how-to-check-the-version-of-an-assembly-dll#:~:text=%20There%20is%20a%20couple%20of%20ways%20to,it%20in%20code:Assembly%20assembly%20=%20Assembly.LoadFrom...%20More>

Use `C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools\ildasm.exe`

`[Reflection.AssemblyName]::GetAssemblyName( (Get-Item .\System.ValueTuple.dll).FullName).Version`

## Sign files

Can use [LocalSigning](https://github.com/microsoft/service-fabric/blob/master/src/packages.ossbuild.config)

- `<package id="LocalSigning" version="2.0.9.3" allowedVersions="[2,3)" autoUpgrade="true" />`
- This is a nuget that sign the file with a cert so others can use it to know that the content has not been changed by someone not the author
- To use it: `<Import Project="$(PkgLocalSigning)\LocalSigning.targets" />`
- Need to define an item group: `<FilesToSign Include="**\*.zip" />`

[Does not work with zip](https://superuser.com/questions/426337/is-it-possible-to-sign-archives)

- zip should use a checksum instead

## Custom Build Target

<https://stackoverflow.com/questions/5124731/run-a-custom-msbuild-target-from-visualstudio>

<https://learn.microsoft.com/en-us/visualstudio/ide/how-to-view-save-and-configure-build-log-files?view=vs-2022>

The sln might currupt cause project cannot load properties.

## AfterTargets doesn't work

<https://stackoverflow.com/questions/2855713/what-is-the-difference-between-dependsontargets-and-aftertargets>
