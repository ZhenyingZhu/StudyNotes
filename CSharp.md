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

## Console
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

Compile C# program: 
- `csc hello.cs`. It return an exe, which compiles for the full framework, and may not be available on all platforms.
- `csc /t:library acme.cs` return a dll
- `csc /r:acme.dll example.cs`
- When a multi-file C# program is compiled, all of the source files are processed together, and the source files can freely reference each other
- `dotnet` also manage dependencies and then invoke `csc`

## Concepts
`String.Empty` vs `null`

A static method can be called as a method of an instance and as a static method

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
- All types implicitly inherit from Object, include primitive types such as int and double
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
- accessibility (See below Accessibility)
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

component-oriented programming
- components present a programming model with properties, methods, and events

Program Structure
- programs
- namespaces
- types
- members
- assemblies

Assemblies
- typically have the file extension .exe or .dll, depending on whether they implement applications or libraries
- contain executable code in the form of Intermediate Language (IL) instructions
- contains symbolic information in the form of metadata
- Before it is executed, the IL code in an assembly is automatically converted to processor-specific code by the Just-In-Time (JIT) compiler of .NET Common Language Runtime

Types [src](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/types-and-variables)
- value
  - simple
  - enum: Every enum type has an underlying type
  - struct: do not typically require heap allocation.
  - nullable value: Extensions of all other value types with a null value. `T?` is the extension of `T`
- Reference, i.e. object
  - class
  - interface
  - array: multi-dimensional `int[,]`
  - delegate: `delegate int D(...)`, references to methods with a particular parameter list and return type

Boxing: `int i = 1; object o = i;`; Unboxing: `int j = (int)o;`

variables : represent storage locations
- fields
- array elements
- local variables
- parameters

Expression: [src](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/expressions)
- precedence of the operators
  - Object creation with initializer: `new T(...){...}`. It returns a reference
  - Anonymous object initializer: `new {...}`
  - Array creation: `new T[..]`
  - Obtain Type object: `typeof(T)`
  - Evaluate expression in checked/unchecked context: `checked(x)`/`unchecked(x)`
  - Obtain default value: `default(T)`
  - Anonymous function: `delegate {...}`
  - Bitwise negation: `~x`
  - Asynchronously wait: `await x`
  - Return true if x is a T: `x is T`
  - Return x typed as T, or null if x is not a T: `x as T`. `var method = member as MethodBase;` convert type. If not convertable, return null
  - Null coalescing: `x ?? y` Evaluates to y if x is null, to x otherwise
  - Anonymous function (lambda expression): `(T x) => y`
- associativity of the operators 
  - assignment operator `=` and conditional operator `?:` is right-associative
- overloaded operators 

Statements
- block: a list of statements written between the delimiters `{` and `}`
- Declaration statement
- Expression statement
- Selection statement: `switch (var) { }`
- Iteration statement: `foreach (var in Collection)`
- Jump statement: `goto`, `throw`, `yield`
- `try`, `catch`, `finally`
- `checked` and `unchecked` statements are used to control the overflow-checking context for integral-type arithmetic operations and conversions
- `lock` statement
- `using` statement

`const`

The end of foreach
```
static IEnumerable<int> Range(int from, int to) 
{
    for (int i = from; i < to; i++) 
    {
        yield return i;
    }
    yield break;
}
static void YieldStatement(string[] args)
{
    foreach (int i in Range(-10,10)) 
    {
        Console.WriteLine(i);
    }
}
```

throw and catch in the same block
```
    try 
    {
        if (args.Length != 2) 
        {
            throw new InvalidOperationException("Two numbers required");
        }
    }
    catch (InvalidOperationException e) 
    {
        Console.WriteLine(e.Message);
    }
```

Check if overflow
```
    int x = int.MaxValue;
    unchecked 
    {
        Console.WriteLine(x + 1);  // Overflow
    }
    checked 
    {
        Console.WriteLine(x + 1);  // Exception
    }
```

class members [src](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/classes-and-objects#other-function-members)
- constants
- fields: variables. `public static readonly Color Black = new Color(0, 0, 0);` or `private byte r, g, b;`
- methods: 
  - Use reference parameters: `static void Swap(ref int x, ref int y);`; 
  - Use output parameters: `static void Divide(int x, int y, out int result, out int remainder);`; 
  - Use parameter array: Only the last parameter of a method can be a parameter array, and the type of a parameter array must be a single-dimensional array type
- properties: Actions associated with reading and writing named properties of the class.  do not denote storage locations. 
  - `public int Count => count;` where `count` is a field
  - `public int Capacity { get { return items.Length; } set {} }` where `items` is an array and a field
- indexers: Actions associated with indexing instances of the class like an array. Actually it is `operator[]`
- events: Notifications that can be generated by the class. the type must be a delegate type. `public event EventHandler Changed;` **?**
- operators: All operators must be declared as public and static.
- constructors
- finalizers: desctructor. classes should implement finalizers only when no other solutions are feasible.
- types: Nested types declared by the class

Accessibility
- public
- protected
- internal: Access limited to the current assembly (.exe, .dll, etc.)
- protected internal: Access limited to the containing class or classes derived from the containing class
- private

Type parameters: used to create a constructed type
```
public class Pair<TFirst,TSecond>
{
    public TFirst First;
    public TSecond Second;
}
Pair<int,string> pair = new Pair<int,string> { First = 1, Second = "two" };
int i = pair.First;     // TFirst is int
```

Struct
- do not require heap allocation
- all struct types implicitly inherit from type `System.ValueType`

Array
- elements 
- Array types are reference types, means an array variable is a pointer
- `int[] a = new int[10];`
- a rank 3 array: `int[,,] a3 = new int[10, 5, 2];`
- jagged array: An array with elements of an array type `int[][] a = new int[3][];`
- default is 0 for int or null
- array initializer: `int[] a = new int[] {1, 2, 3};`


Interface [src](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/interfaces)
- explicit interface member implementations: `void IControl.Paint() { }`. But can only access throw the interface type.

Enum
- a distinct value type with a set of named constants
- use one of the integral value types as their underlying storage
- define enum use underlying type
```
enum Alignment: sbyte
{
    Left = -1,
    Center = 0,
    Right = 1
}
```

delegate type
- can reference any method, even static method, that has the same argument list and return type
- anonymous functions: `(double x) => x * 2.0`
```
delegate double Function(double x);
class Multiplier
{
    double factor;
    public Multiplier(double factor) 
    {
        this.factor = factor;
    }
    public double Multiply(double x) 
    {
        return x * factor;
    }
}

Multiplier m = new Multiplier(2.0);
Function f = m.Multiply;
double res = f(3.0);
```



## APIs
[doc](https://docs.microsoft.com/en-us/dotnet/api/index?view=netframework-4.7)

[sample](https://github.com/dotnet/docs/tree/master/samples)

namespace
- System

class
- System.Console

`System.Threading.Tasks.Task(seconds).Wait()`

## Others
resx
- https://docs.microsoft.com/en-us/dotnet/framework/resources/working-with-resx-files-programmatically




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

attribute and reflection https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/attributes

mechanisms

# Stack
https://docs.microsoft.com/en-us/dotnet/articles/welcome
  https://docs.microsoft.com/en-us/dotnet/articles/csharp/
    Fin: Get Started
    https://docs.microsoft.com/en-us/dotnet/articles/csharp/tutorials/index
      Fin: console teleprompter, inheritance
	https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/
	  Fin: Program Structure, Types and variables, Expressions, Statements, Classes and objects, Structs, Arrays, Interfaces, Enums, Delegates, 
	  https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/arrays

https://www.microsoft.com/net/tutorials/csharp/getting-started/

# Compare two lists
https://msdn.microsoft.com/en-us/library/bb348567(v=vs.110).aspx


# Dictionary
TryGetValue can test if key exist or not.

# Write to a file
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-write-to-a-text-file

# UT
https://docs.microsoft.com/en-us/visualstudio/test/unit-test-basics

# Action
https://msdn.microsoft.com/en-us/library/018hxwa8(v=vs.110).aspx

# sleep
Thread.sleep

# GUID never conflict
https://www.guidgen.com/

# Create a mock
