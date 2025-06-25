# C# Build

## Dotnet SDK

- Runtime: executes application code.
- Libraries: provides utility functionality like JSON parsing.
- Compiler: compiles C# (and other languages) source code into (runtime) executable code.
- SDK and other tools: enable building and monitoring apps with modern workflows.
- App stacks: like ASP.NET Core and Windows Forms, that enable writing apps.

<https://learn.microsoft.com/en-us/dotnet/core/install/how-to-detect-installed-versions?pivots=os-windows>

- `dotnet --list-sdks`

## .NET Framework Difference

<https://learn.microsoft.com/en-us/dotnet/framework/whats-new/>

- Improvements to the JIT compiler.
- cryptographic enhancements: Support for ephemeral keys
- Additional collection APIs
- Support for .NET Standard 2.0
- Garbage collection performance improvements

### .NET Framework migration

Changes to make

- Target Framework version
- Assembly Reference path
- BindingRedirect
- app/web Config File

To upgrade, can use the upgrade assistant:

- <https://devblogs.microsoft.com/dotnet/upgrade-assistant-now-in-visual-studio/>
  - ASP.NET from .NET Framework only support side-by-side incremental.
- <https://learn.microsoft.com/en-us/dotnet/core/porting/>
- for project dependencies, need to start from the top layer.

### SDK style project differences

<https://stackoverflow.com/questions/46709000/disable-transitive-project-reference-in-net-standard-2>

If no DLLs to build: <https://github.com/microsoft/MSBuildSdks/blob/main/src/NoTargets/README.md>

To use those SDKs, the global.json needs to have

```json
{
    "sdk": {
      "version": "8.0.405",
      "rollforward": "major"
    },
    "msbuild-sdks": {
        "Microsoft.Build.NoTargets": "3.5.6",
        "Microsoft.Build.Traversal": "3.2.0"
    }
  }
```

<https://dansiegel.net/post/2018/08/21/demystifying-the-sdk-project>

- Release builds need to be optimized, while Debug configurations need all of our debug symbols
- [Well-Known Properties](https://learn.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2015/msbuild/msbuild-reserved-and-well-known-properties?view=vs-2015&redirectedfrom=MSDN)
- ItemGroup grouping Items. Can be used with Condition
- Supports multi-target frameworks.
- Common lib should be packed because: 1. reduce build time, 2. versioning, 3. isolate rollout, 4. individual testing
- using a nuspec is not needed for SDK style projects. Can set `GeneratePackageOnBuild` to true in csproj

<https://hermit.no/moving-to-sdk-style-projects-and-package-references-in-visual-studio-part-1/>

<https://hermit.no/moving-to-sdk-style-projects-and-package-references-in-visual-studio-part-2/>

First restore packages then build. Only projects references in the traversal projects will be restored.

### .NET Version upgrade

Error: CSC : warning CS9057: The analyzer assembly 'Microsoft.CodeAnalysis.CodeStyle.dll' references version '4.11.0.0' of the compiler, which is newer than the currently running version '4.3.0.0'.

- <https://stackoverflow.com/questions/77513481/net-8-build-issue-the-analyzer-assembly-references-version-4-8-0-0-of-the-co>
- <https://stackoverflow.com/questions/76740942/how-to-fix-the-analyzer-assembly-references-version-4-7-0-0-of-the-compiler>
- <https://learn.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2017/msbuild/how-to-use-project-sdk?view=vs-2017#how-project-sdks-are-resolved>

<https://stackoverflow.com/questions/76740942/how-to-fix-the-analyzer-assembly-references-version-4-7-0-0-of-the-compiler>

<https://learn.microsoft.com/en-us/dotnet/core/tools/global-json>

Debug vs Release: Release version incorporates compiler optimizations

`Build > Publish {projectname}`

Run it:

```cmd
dotnet HelloWorld.dll
```

### VS build

<https://docs.microsoft.com/zh-CN/visualstudio/ide/get-started-with-visual-studio>

Building blocks

Solution:

- can have several projects
- can set reference between projects

To run a console app with arg, change project properties.

If tests failed. Check if the arch is changed to x64

Change the build configuration from Debug to Release, then build solution.

<https://learn.microsoft.com/en-us/visualstudio/ide/msbuild-logs?view=vs-2022>

- the [Project System Tools](https://github.com/dotnet/project-system-tools) can see build logs.

## MSBuild

<https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild?view=vs-2022>

<https://www.pluralsight.com/courses/msbuild>

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

### Solution Item

Show in VS.

### Custom Build Target

<https://stackoverflow.com/questions/5124731/run-a-custom-msbuild-target-from-visualstudio>

<https://learn.microsoft.com/en-us/visualstudio/ide/how-to-view-save-and-configure-build-log-files?view=vs-2022>

The sln might currupt cause project cannot load properties.

### CopyOutputFromProject

Seems like post build operation.

If nuget package changed, DLL might not be found.

### Traversal target

Include other projects.

### Repo root

<https://stackoverflow.com/questions/56284850/is-there-an-msbuild-property-providing-the-repository-root>

## Sign files

Can use [LocalSigning](https://github.com/microsoft/service-fabric/blob/master/src/packages.ossbuild.config)

- `<package id="LocalSigning" version="2.0.9.3" allowedVersions="[2,3)" autoUpgrade="true" />`
- This is a nuget that sign the file with a cert so others can use it to know that the content has not been changed by someone not the author
- To use it: `<Import Project="$(PkgLocalSigning)\LocalSigning.targets" />`
- Need to define an item group: `<FilesToSign Include="**\*.zip" />`

[Does not work with zip](https://superuser.com/questions/426337/is-it-possible-to-sign-archives)

- zip should use a checksum instead

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

If dotnet restore failed with 401, install `https://github.com/Microsoft/artifacts-credprovider`, <https://github.com/Microsoft/artifacts-credprovider?tab=readme-ov-file#azure-devops-pipelines>

- <https://github.com/dotnet/sdk/issues/10189>

NuGet package testing

- <https://stackoverflow.com/questions/46349547/where-is-nuget-exe>
- <https://learn.microsoft.com/en-us/nuget/reference/nuget-exe-cli-reference?tabs=windows#installing-nugetexe>
- <https://stackoverflow.com/questions/10240029/how-do-i-install-a-nuget-package-nupkg-file-locally-to-visual-studio>
- <https://www.bing.com/search?pglt=129&q=GeneratePackageOnBuild+make+project+reference+also+become+nuget&cvid=71cf4fec864549228de74406c4a8e2c2&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIICAEQ6QcY_FXSAQkxNDA2N2owajGoAgCwAgA&FORM=ANNAB1&PC=U531>
- <https://stackoverflow.com/questions/54871290/changing-a-project-reference-to-a-nuget-package-reference-on-build>

<https://stackoverflow.com/questions/7015149/multiperson-team-using-nuget-and-source-control>

<https://stackoverflow.com/questions/7018913/where-does-nuget-put-the-dll>

<https://docs.microsoft.com/en-us/nuget/concepts/package-versioning#version-ranges>

- `<package id="{package}" version="{this will be updated every build}" allowedVersions="[1,2)" autoUpgrade="true" />`

[Quickstart: Create and publish a package (dotnet CLI)](https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-the-dotnet-cli)

[Nuget sources command (NuGet CLI)](https://docs.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-sources)

- Create a local source: `nuget sources add -name foo.bar -source C:\NuGet\local -username foo -password bar -StorePasswordInClearText -configfile %AppData%\NuGet\my.config`

[Specify a nuget config](https://stackoverflow.com/questions/46795035/project-specific-nuget-config-with-net-core-code#:~:text=Project-specific%20NuGet.Config%20files%20located%20in%20any%20folder%20from,file%20to%20give%20that%20project%20a%20specific%20configuration.)

<https://docs.microsoft.com/en-us/nuget/hosting-packages/local-feeds>

Use NuSpec to create a nuget package: <https://stackoverflow.com/questions/40628116/how-to-specify-configuration-specific-folder-in-nuspec>

<https://stackoverflow.com/questions/16173568/build-nuget-package-automatically-including-referenced-dependencies>

Using the GeneratePackageOnBuild is better than nuproj/nuspec

- <https://www.google.com/search?q=nuproj+is+deprecated&newwindow=1&sca_esv=3d0b5893383aa7ee&sxsrf=ADLYWIJ7vzWMfKkXeC_FVYmzzljTH0dtQg%3A1727916062630&ei=Huj9ZvSSJtzD0PEP66CQQA&ved=0ahUKEwj0mfX4_PCIAxXcITQIHWsQBAgQ4dUDCA8&uact=5&oq=nuproj+is+deprecated&gs_lp=Egxnd3Mtd2l6LXNlcnAiFG51cHJvaiBpcyBkZXByZWNhdGVkMgUQIRigATIFECEYoAEyBRAhGKABMgUQIRigAUj4F1AAWIgRcAB4AZABAJgBXaABigiqAQIxNLgBA8gBAPgBAZgCDqACqwjCAgQQIxgnwgIEEAAYHsICCBAAGIAEGKIEwgIGEAAYFhgewgILEAAYgAQYhgMYigXCAggQABiiBBiJBZgDAJIHAjE0oAe5MA&sclient=gws-wiz-serp>
- <https://github.com/nuproj/nuproj>
- <https://github.com/NuGet/Home/issues/8983>
- <https://stackoverflow.com/questions/14797525/differences-between-nuget-packing-a-csproj-vs-nuspec>
- <https://learn.microsoft.com/en-us/nuget/create-packages/creating-a-package-msbuild>

`dotnet add package {package}` can either add a new package or upgrade the package version.

### Local testing

Created a Nuget source/feed foo.bar: `nuget sources add -name foo.bar -source C:\NuGet\local -username foo -password bar -StorePasswordInClearText -configfile Nuget.config`

Save it in a NuGet.config file and place it under the project folders using the source/feed.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="foo.bar" value="C:\NuGet\local" />
  </packageSources>
  <packageSourceCredentials>
    <foo.bar>
        <add key="Username" value="foo" />
        <add key="ClearTextPassword" value="bar" />
      </foo.bar>
  </packageSourceCredentials>
</configuration>
```

Build the project with retail and then publish the package [doc](https://learn.microsoft.com/en-us/azure/devops/artifacts/nuget/publish?view=azure-devops):

```powershell
dotnet pack
dotnet nuget push --source foo.bar --api-key az <package-path>
# or
nuget add .\bin\Release\TestNugetLibPackage.1.0.1.nupkg -source C:\NuGet\local
```

Find all the packages used in a project: `dotnet list package --include-transitive`

### NuProj vs. csproj

<https://learn.microsoft.com/en-us/nuget/reference/msbuild-targets>

Is it in SDK style project only?

```xml
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
    <PackageOutputPath>$(BinariesDirectory)\packages\</PackageOutputPath>
    <NuspecFile>package.nuspec</NuspecFile>
    <NuspecProperties>$(NuspecProperties);pkgversion=$(BuildVersion);servicebinpath=$(ServiceBinPath)</NuspecProperties>
```

`msbuild -t:pack`

`dotnet pack --no-build`

<https://learn.microsoft.com/en-us/dotnet/core/project-sdk/overview>

<https://stackoverflow.com/questions/62773638/dotnet-build-with-generatepackageonbuild-and-contentfiles>

```xml
<Project Sdk="Microsoft.Build.NoTargets">

  <PropertyGroup>
    <!-- Define target framework -->
    <TargetFramework>net48</TargetFramework>

    <!-- NuGet packaging properties -->
    <PackageId>mypackage</PackageId>
    <Title>MyPackage</Title>

    <!-- Enable NuGet package generation -->
    <IsPackable>true</IsPackable>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>

    <!-- Output directory for the generated package -->
    <NupkgOutputPath>$(BinariesDirectory)\packages</NupkgOutputPath>

  </PropertyGroup>

  <ItemGroup>
    <!-- Project reference -->
    <ProjectReference Include="..\myproject.csproj" />

    <!-- Project artifact path -->
    <PackageContents Include="$(BinariesDirectory)\$(BuildType)\myprojectnamespace\Amd64\**\*.*" />

    <!-- Additional files to include in the package -->
    <None Include="@(PackageContents)" Pack="true" PackagePath="lib\net48"/>
  </ItemGroup>

  <Import Project="$(MsoBuildSettingsPath)\Microsoft.Online.Build.Settings" />
  <Target Name="CoreCompile" />

</Project>
```

### Nuget path

<https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#generatepathproperty>

<https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files>

<https://learn.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference>

<https://stackoverflow.com/questions/24022134/how-exactly-does-the-specific-version-property-of-an-assembly-reference-work-i>

<https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#controlling-dependency-assets>

### Transitive dependency

<https://devblogs.microsoft.com/nuget/introducing-transitive-dependencies-in-visual-studio/#:~:text=There%20is%20now%20a%20new,level%20dependency%20at%20any%20time.>

### PackageReference

<https://dfederm.com/how-does-packagereference-work/#google_vignette>

- packages downloads to repository-relative path configured in the `NuGet.Config` file (normally /packages)
- Conventions of package paths in the `<Import>`, `<Reference>`, and `<Content>` elements are codified
- No need to copy packages from global package cache (by default %UserProfile%\.nuget\packages) to /packages directory
- Generate files describe the restored packages in `$(RestoreOutputPath)`: `project.assets.json`, `<project-file>.nuget.g.props`, and `<project-file>.nuget.g.targets`.
- PackageReference items are only used during the restore. During the build, any package related information comes from the generated files.
- `project.assets.json`: dep graph of the packages, and the contents. The `ResolvePackageAssets` target reads them.

## CoreXT

<https://imperfect.work/2022/06/17/corext-package-management/>

<https://github.com/NuGet/Home/issues/9314>

An NET SDK project reference a CoreXT project is fine. If a CoreXT project reference a NET SDK project, the SDK project won't restore.

### CxCache

It is folder to hold dependency packages. Maybe is related to <https://www.nuget.org/packages/xCache/> ?

## DLL Version

<https://learn.microsoft.com/en-us/dotnet/framework/configure-apps/redirect-assembly-versions>

- Only for .NET Framework
- assembly unification: based on the runtime .NET Framework version, find the .NET Framework DLLs.
- For 3rd-party assemblies, since cannot make sure all packages are building against versions of the dependencies.
- publisher policy: set by vendors of assemblies (package owner)
- automatic binding redirection: the runtime automatically adds a binding redirection to the newer version of the assembly.

For .NET Core: <https://learn.microsoft.com/en-us/dotnet/core/dependency-loading/overview>

- Managed assembly loading: compiler inserted a static assembly reference whenever code uses a type defined in another assembly.
- Satellite assembly: store localized resources
- Unmanaged lib:
- AssemblyLoadContext

### DLL Binding Redirect AutoUnify

<https://stackoverflow.com/questions/33256071/what-is-autounify-and-why-is-it-causing-working-tests-to-fail-in-tfs-2015>

## Misc

### Some Really Weird Build Issue

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

### AfterTargets doesn't work

<https://stackoverflow.com/questions/2855713/what-is-the-difference-between-dependsontargets-and-aftertargets>

- DependsOnTargets: Defines the targets that must be executed before the target can be executed.

### Enforce target to run

msbuild sometimes don't re-restore before build: <https://github.com/NuGet/Home/issues/12437>. The `objd` folder contains some info and if it is deleted, seems like msbuild can mess up. To enforce the restore, can add below target to csproj:

```xml
  <!-- Custom target that enforces restore before building -->
  <Target Name="EnsureRestoreBeforeBuild" BeforeTargets="BeforeBuild">
    <MSBuild Projects="$(MSBuildProjectFile)" Targets="Restore" />
  </Target>
```

### Build Race condition

- <https://stackoverflow.com/questions/5134137/build-error-the-process-cannot-access-the-file-because-it-is-being-used-by-ano>
- <https://stackoverflow.com/questions/6838779/msbuild-fails-with-the-process-cannot-access-the-file-xxxxx-because-it-is-being>

### Build Warnings

DLL conflicts: <https://learn.microsoft.com/en-us/visualstudio/msbuild/errors/msb3277?view=vs-2022>

Suppress build warning: <https://stackoverflow.com/questions/49564022/suppressing-warnings-for-solution>

The dotnet restore might fail when see nuget version downgrade.

The target framework version in the nuget package also matters because otherwise it will throw exception saying dll not found.

Open binding redirect log: <https://stackoverflow.com/questions/255669/how-to-enable-assembly-bind-failure-logging-fusion-in-net>

ildasm

Assembly version vs. nuget package version.

### Top Level Statement

<https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/top-level-statements>

Define a namespace: <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-6.0>

### Check DLL info

`ildasm.exe *.dll`

It is under `C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools\`

### PackageReference Private

[including package references and then private assets in csproj](https://forums.asp.net/t/2162896.aspx?including+package+references+and+then+private+assets+in+csproj#:~:text=By%20default%2C%20all%20package%20assets%20are%20included.%20%60PrivateAssets%60,by%20default%20when%20this%20attribute%20is%20not%20present.)

- `IncludeAssets` attribute specifies which assets belonging to the package specified by `<PackageReference>` should be consumed. By default, all package assets are included.
- `PrivateAssets` attribute specifies which assets belonging to the package specified by `<PackageReference>` should be consumed but not flow to the next project. The Analyzers, Build and ContentFiles assets are private by default when this attribute is not present. But with it, the DLLs are not auto copied to the output folder so cannot be referenced.

### GlobalPackageReference

```xml
<GlobalPackageReference Include="PRSSign" Version="1.0.109" GeneratePathProperty="true" PrivateAssets="All" Condition="'$(EnableCodeSign)' != 'false'" />
```

### AspNetCompiler

<https://learn.microsoft.com/en-us/previous-versions/aspnet/ms227972(v=vs.100)>

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net472</TargetFramework>
    <UseWPP_CopyWebApplication>true</UseWPP_CopyWebApplication>
    <UseWebCompiler>true</UseWebCompiler>
  </PropertyGroup>

  <Import Project="$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets" Condition="Exists('$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets')" />
</Project>
```

or

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
```

### Transitive DLL copy

In MSBuild, if project A references project B, and project B uses DLLs from a NuGet package C, then yes, by default:

The DLLs from package C used by project B will be transitively copied into project A’s output folder — but only if:

Project B marks the references as copy-local (i.e., Private=true), which is the default behavior for package references.

The referenced DLLs are used at runtime, not just at compile time.

There are no PrivateAssets or ExcludeAssets flags set to suppress propagation.

So under normal circumstances with no suppression metadata in the .csproj or .nuspec, you will find DLLs from package C in project A's bin\ output folder, alongside project A and B’s compiled assemblies.
