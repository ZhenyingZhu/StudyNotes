## IDE
[Visual Studio](https://docs.microsoft.com/en-us/dotnet/articles/csharp/getting-started/with-visual-studio)

`Debug > Windows > Immediate` open a cmd which can interact with the app

F5 debug, F11 stepping

Condition break point

Debug vs Release: Release version incorporates compiler optimizations

`Build > Publish {projectname}`

Run it:
```
dotnet HelloWorld.dll
```

## Get start
[src](https://docs.microsoft.com/en-us/dotnet/articles/csharp/getting-started/getting-started-with-csharp)

```
var name = Console.ReadLine();
var date = DateTime.Now;
Console.WriteLine("\n {0}, {1:d}, {1:t}", name, date);
```

Create a class library as an extension method
- a third-party component
- include it as a bundled component with one or more applications.

Create a unit test
- Add a dependency
- Use `Assert` class

`String.Empty` vs `null`

A static method can be called as a method of an instance and as a static method

If the library will be used by a single solution, include it as a project in your solution
- create a project and `Set as StartUp Project`
- `Add Reference`

If the library will be generally accessible, you can distribute it as a NuGet package
- Under the project directory with `*.csproj` file, run `dotnet pack --no-build`. The `*.nupkg` is the package

`where dotnet.exe`


## APIs
[doc](https://docs.microsoft.com/en-us/dotnet/api/index?view=netframework-4.7)

namespace
- System

class
- System.Console


# Stack
https://docs.microsoft.com/en-us/dotnet/articles/csharp/tutorials/