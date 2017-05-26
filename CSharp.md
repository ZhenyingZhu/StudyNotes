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

Create package from CLI: [src](https://docs.microsoft.com/en-us/dotnet/articles/csharp/tutorials/console-teleprompter)
1. `dotnet new console`
2. `dotnet restore`: run NuGet package management
3. `dotnet build`
4. `dotnet run`


## Concepts
Enumerator method: [example](https://github.com/dotnet/docs/tree/master/samples/csharp/getting-started/console-teleprompter)
- Enumerator methods return sequences that are evaluated lazily.
- contain one or more `yield` return statements. 
- `foreach (var x in IEnumerable)`

I​Disposable Interface
- Provides a mechanism for releasing unmanaged resources.
- `Dispose`
- `using`

implicitly typed local variable
- `var`

Async Tasks
- `private static async Task Foo()`
- `await` return a `Task`. When reach `await`, the `Task` returns but will resume
- `Task.Wait()`
- `Main` cannot use `await` operator
- `Task.Run(lambda)`
- `Task.WhenAny(Task[])` when anyone first finish

Lock
- Create a private object as a mutex: `private object lockHandle = new object();`
- `lock (lockHandle)`

Lambda
- `Action work = () => {};`
- can also used to define class memeber `public override string ToString() => Title;`

Access
- `internal class MyClass`
- `public int DelayInMilliseconds { get; private set; } = 200;`

Using
- `using static System.Math;`

Class property
- define `internal class Config`
- `public bool Done => done;` make a property `Done` that can access private `done`

type categories
- class: inherit from `Object`
- structs: `Object` and `ValueType`
- delegates: `MulticastDelegate`, `Delegate`, `Object`
- enums: `Enum`, `System.ValueType`, `Object`

`Object`
- All types implicitly inherit from Object
- members: 
  - `ToString`
  - `Equals(Object)` and static `Equals(Object, Object)` and static `ReferenceEquals(Object, Object)`. Unless it is overridden, the `Equals(Object)` method tests for reference equality.
  - `GetHashCode`: When you override the `Equals(Object)` method, you must also override the `GetHashCode()` method
  - `GetType`
  - protected `Finalize` is desctructor
  - `MemberwiseClone` return a shallow clone
  - `.ctor` default constructor

`System.Reflection`
- inspect a type's metadata to get information about that type
- `Type t = typeof(SimpleClass);`

inheritance
- base class vs derived class
- support single inheritance only, but transitive
- Static constructors, Instance constructors, Finalizers are not inherited
- accessibility
  - Private members are visible only in derived classes that are nested in their base class.
  - Protected members are visible only in derived classes.
  - Internal members are visible only in derived classes that are located in the same assembly as the base class.
  - Public members are visible in derived classes and are part of the derived class' public interface.
- In order to be able to override a member, the member in the base class must be marked with the `virtual`, `abstract`, or `override`.
- `sealed` to make a sure a class cannot be use as a base class
- `this`, `base`


"is a" relation represent by inherit from a class, "can do" represents by inherit from interfaces  

Nested class `A.B`:
```
public class A 
{
   private int value = 10;

   public class B : A
   {
       public int GetValue()
       {
           return this.value;
       }
   }
}
```

Ctor
- If constructor is not present in the base class' source code, the C# compiler automatically provides a default (parameterless) constructor.
- If you don't make an explicit call to a base class constructor in your source code, the C# compiler automatically supplies a call to the base class' default or parameterless constructor.

`var method = member as MethodBase;` convert type. If not convertable, return null


## Question
`public int DelayInMilliseconds { get; private set; } = 200;` what is get and private set?
```
   public int Pages
   { get { return totalPages; }
     set 
     {
         if (value <= 0)
            throw new ArgumentOutOfRangeException("The number of pages cannot be zero or negative.");
         totalPages = value;   
     }
   }
```

`BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;`



instantiate the class by using reflection

`ulong nISBN = 0; if (! UInt64.TryParse(isbn, out nISBN))`


## APIs
[doc](https://docs.microsoft.com/en-us/dotnet/api/index?view=netframework-4.7)

[sample](https://github.com/dotnet/docs/tree/master/samples)

namespace
- System

class
- System.Console

`System.Threading.Tasks.Task(seconds).Wait()`


# Stack
https://docs.microsoft.com/en-us/dotnet/articles/welcome
  https://docs.microsoft.com/en-us/dotnet/articles/csharp/
    https://docs.microsoft.com/en-us/dotnet/articles/csharp/tutorials/index
      [console teleprompter](https://docs.microsoft.com/en-us/dotnet/articles/csharp/tutorials/console-teleprompter)
	  [inheritance]https://docs.microsoft.com/en-us/dotnet/articles/csharp/tutorials/inheritance


