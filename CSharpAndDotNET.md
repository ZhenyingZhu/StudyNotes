# C# and .NET

## Resources

- <https://www.microsoft.com/net/learn/dotnet/what-is-dotnet>
- <https://docs.microsoft.com/en-us/dotnet/>

## Doc Stack

- <https://docs.microsoft.com/en-us/dotnet/articles/welcome>
  - <https://docs.microsoft.com/en-us/dotnet/articles/csharp/>
    - Fin: Get Started
    - <https://docs.microsoft.com/en-us/dotnet/articles/csharp/tutorials/index>
      - Fin: console teleprompter, inheritance
    - <https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/>
      - Fin: Program Structure, Types and variables, Expressions, Statements, Classes and objects, Structs, Arrays, Interfaces, Enums, Delegates
      - <https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/arrays>

<https://www.microsoft.com/net/tutorials/csharp/getting-started/>

### IDE

[Visual Studio](https://docs.microsoft.com/en-us/dotnet/articles/csharp/getting-started/with-visual-studio)

`Debug > Windows > Immediate` open a cmd which can interact with the app

F5 debug, F11 stepping

Condition break point

Debug vs Release: Release version incorporates compiler optimizations

`Build > Publish {projectname}`

Run it:

```cmd
dotnet HelloWorld.dll
```

### Console

[src](https://docs.microsoft.com/en-us/dotnet/articles/csharp/getting-started/getting-started-with-csharp)

```c#
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

#### VS Code

<https://docs.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio-code#debug>

<https://code.visualstudio.com/docs/csharp/testing>

### Concepts

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

[How C# Reflection Works](https://stackify.com/what-is-c-reflection/)

- can dynamically create an instance of a type and bind that type to an existing object
- also can get the type from an existing object and access its properties.
- Assemblies contain modules
- Modules contain types
- Types contain members
- relection can inspect the contents of an assembly
- C# uses Common Type System (CTS). There is a metadata table.
- GetType: `int i; System.Type type = i.GetType();`
- CreateInstance: `DateTime dateTime = (DateTime)Activator.CreateInstance(typeof(DateTime));`
- `Reflect.SetPropertyValue()`

Access a class from a dll assembly:

The Test dll:

```C#
namespace Test
{
    public class Calculator
    {
        public Calculator() { ... }
        private double _number;
        public double Number { get { ... } set { ... } }
        public void Clear() { ... }
        private void DoClear() { ... }
        public double Add(double number) { ... }
        public static double Pi { ... }
        public static double GetPi() { ... }
    }
}
```

Create the Calculator class:

```C#
Assembly testAssembly = Assembly.LoadFile(@"c:\Test.dll");
Type calcType = testAssembly.GetType("Test.Calculator");
object calcInstance = Activator.CreateInstance(calcType);

PropertyInfo numberPropertyInfo = calcType.GetProperty("Number");
double value = (double)numberPropertyInfo.GetValue(calcInstance, null);
numberPropertyInfo.SetValue(calcInstance, 10.0, null);
```

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

```c#
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
  - delegate: `delegate int D(...)`, references to methods with a particular parameter list and return type. It can be used to replace a static method in UTs.

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

```c#
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

```C#
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

```C#
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

```C#
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

```C#
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

```C#
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

### APIs

[doc](https://docs.microsoft.com/en-us/dotnet/api/index?view=netframework-4.7)

[sample](https://github.com/dotnet/docs/tree/master/samples)

namespace

- System

class

- System.Console

`System.Threading.Tasks.Task(seconds).Wait()`

### Others

[resx](https://docs.microsoft.com/en-us/dotnet/framework/resources/working-with-resx-files-programmatically)

### Question

`public int DelayInMilliseconds { get; private set; } = 200;` // what is get and private set?

```C#
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

[attribute and reflection](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/attributes)

mechanisms

## C# Regex

[Rule](https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference)

E.g.: `^((.|[\r\n])+\:(.|[\r\n])+)$`

A pattern:

```C#
'^(([^sS](.|[\r\n])*)|([sS][^mM](.|[\r\n])*)|([sS][mM][^tT](.|[\r\n])*)|([sS][mM][tT][^pP](.|[\r\n])*)|([sS][mM][tT][pP][^\:](.|[\r\n])*)|([sS][mM][tT][pP]\:((([\u0021-\u007E-[<>\(\)\[\]\\\.,;:@"]]|(\\[\u0000-\u007F]))+(\.([\u0021-\u007E-[<>\(\)\[\]\\\.,;:@"]]|(\\[\u0000-\u007F]))+)*)|("((\\[\u0000-\u007F])|[\u0000-\u007F-[\r\n"\\]])+"))@[^@]*))$'
```

```C#
^ // line start with
( // group a subexpression
 ([^sS] // not in this character group, here means not s or S
  (
    . // any single character except \n
    | // or operator
    [\r\n] // in this character group, here means enter
  )
  * // match previous element 0 or more times
 )
 |
 ([sS][^mM](.|[\r\n])*)
 |
 ([sS][mM][^tT](.|[\r\n])*)
 |
 ([sS][mM][tT][^pP](.|[\r\n])*)
 |
 ([sS][mM][tT][pP][^\:](.|[\r\n])*)
 |
 ([sS][mM][tT][pP]
 \: // escape :
  (
   (
    ([
      \u0021 // !
      -
      \u007E // ~
      -
      [<>\(\)\[\]\\\.,;:@"] // one of those symbols
     ] // [A-Z-[BCD]] means A to Z exclude BCD
     |
     (\\[\u0000-\u007F])
    )
    + // match previous element 0 or more times
    (\.
     ([\u0021-\u007E-[<>\(\)\[\]\\\.,;:@"]]
      |
      (\\[\u0000-\u007F])
     )+
    )*
   )|
   ("
    (
     (\\[\u0000-\u007F])|[\u0000-\u007F-[\r\n"\\]]
    )+"
   )
  )
  @[^@]* // Actually it is an @ with repeat 0 or more no @
 )
)
$ // line end with
```

## LINQ

### query

<https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/query-keywords>

### LINQ Examples

[src](https://code.msdn.microsoft.com/101-LINQ-Samples-3fb9811b)

group: return an IGroup

**TODO**: not complete

```C#
List<int> list1 = {1, 2, 3};
List<int> list2 = {3, 4};
int sameNumCnt = list2.Select(x => x).Intersect(list1).Count;
```

## Function / Action / Predicate

[src](https://stackoverflow.com/questions/4317479/func-vs-action-vs-predicate)

**TODO**: not complete

### Delegate

[src](https://stackoverflow.com/questions/8694921/delegates-vs-interfaces-in-c-sharp)

[use delegate as a callback](https://stackoverflow.com/questions/667742/callbacks-in-c-sharp)

## Unit Test

### Visual studio UT framework

<https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest>

[src](https://docs.microsoft.com/en-us/visualstudio/test/unit-test-basics)

Add a test project to the solution and add reference.

A test case:

```C#
[TestMethod]
[ExpectedException(typeof(ArgumentException))]
[Timeout(2000)]  // Milliseconds
public void TestFoo() {
    ...
    Assert.AreEqual(expected, actual);
}
```

### Rhino mocks

[src](https://hibernatingrhinos.com/oss/rhino-mocks)

[wiki](http://www.ayende.com/wiki/Rhino+Mocks.ashx?AspxAutoDetectCookieSupport=1)

#### Defination

- Mock object
- Interaction based testing
- State based testing
- Expectation
- Record & replay model
- Ordering

Rhino Mocks can only mock interfaces, delegates and virtual methods of classes.

- Mock Objects: `StrictMock()`. Any methods that are not recorded throw exception.
- Dynamic Mock: `DynamicMock()`. Not recorded methods return null or 0.
- Partial Mock: `PartialMock()`. Not recorded methods call real implementation.

Stub

- `MockRepository.GenerateStub<T>`
- Mock the thing you are testing, Stub the thing that are just involved.

#### Rhino Mock Examples

<https://github.com/hibernating-rhinos/rhino-mocks/tree/master/Rhino.Mocks.GettingStarted>

- `var foo = MockRepository.GenerateStub<IFoo>();`: Create a stub instance foo of the interface IFoo.
- `foo.Stub(x => x.ID).Return(123);`: Set a property of the stub instance.

#### Mock a method

```C#
class MyClass {
    public MyReturn Foo(MyArg1 arg1, MyArg2 arg2) {
        try {
            return MyReturn r;
        } catch(Exception e) {
             throw(new MyException());
        }
    }
}

MyClass mc = MockRepository.GenerateMock<MyClass>();
MyReturn mr = new MyReturn();
MyException me = new MyException();

Expect.Call(mc.Foo(Arg<MyArg1>.Is.Anything, Arg<MyArg2>.Is.Anything)).Return(mr);
Expect.Call(mc.Foo(Arg<MyArg1>.Is.Anything, Arg<MyArg2>.Is.Anything)).Return(mr).Repeat.Times(5);
Expect.Call(mc.Foo(Arg<MyArg1>.Is.Anything, Arg<MyArg2>.Is.Anything)).Throw(me);
```

or

```C#
MockRepository mocks = new MockRepository();
MyClass mc = mocks.PartialMock<MyClass>();
MyReturn mr = new MyReturn();
MyException me = new MyException();

Expect.Call(mc.Foo(Arg<MyArg1>.Is.Anything, Arg<MyArg2>.Matches(() => true))).Return(mr);
mocks.ReplayAll(); // to call those mock methods

// Test some real code.

mc.VerifyAllExpectations();
mocks.VerifyAll();
```

or

```C#
MockRepository mocks = new MockRepository();
MyClass mc = mocks.DynamicMock<MyClass>();
mc.Stub(x => x.property).Return(value);
mockRequestContext.Replay();

return mc;
```

[src](https://stackoverflow.com/questions/466520/what-is-rhino-mocks-repeat)
If not a repeat, it only call once.

[src](https://stackoverflow.com/questions/2764929/rhino-mocks-partial-mock)
Partial mock needs work on virtual methods.

#### Create a mock object

[src](https://stackoverflow.com/questions/7831404/can-you-explain-difference-between-strictmock-and-partialmock)

#### Arg Matches

[src](https://stackoverflow.com/questions/3520911/rhino-mocks-using-arg-matches)

#### Out Parameter

<https://en.wikibooks.org/wiki/How_to_Use_Rhino_Mocks/Out_and_Ref_Parameters>

<https://stackoverflow.com/questions/3365237/using-rhino-mocks-to-mock-an-out-parameter-which-is-created-within-the-method-i>

```C#
obj..Stub(serv => serv.IsLoginValid(
            Arg<LoginViewModel>.Is.Equal(a_login_viewmodel),
            out Arg<User>.Out(new User()).Dummy))
.OutRef(new User())
.Return(false);
```

## Collection

### Init a Directory

```C#
Dictionary<string, List<string>> myD = new Dictionary<string, List<string>>()
{
  {"tab1", MyList }
};
```

### Dictionary

TryGetValue can test if key exist or not.

### Enumerable.Intersect

<https://msdn.microsoft.com/en-us/library/bb460136(v=vs.110).aspx>

## NuGet

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

### CxCache

It is folder to hold dependency packages. Maybe is related to <https://www.nuget.org/packages/xCache/> ?

## Multi-thread

<https://devblogs.microsoft.com/pfxteam/>

<https://learn.microsoft.com/en-us/dotnet/standard/threading/managed-threading-basics>

- thread has a scheduling priority.
- threads share virtual address space of the process.
- primary vs. worker threads
- Task Parallel Library (TPL) and Parallel LINQ (PLINQ): use ThreadPool threads.
- Most unhandled exceptions in threads generally terminate the process. Special ex: ThreadAbortException and AppDomainUnloadedException. [Test] If the worker thread throws an exception but primary thread doesn't listen, will the primary thread crash?
- application domain: Common Language Infrastructure (CLI) to isolate executed software applications from one another.
- managed code need to install an exception handler at a point.
- calls need to be synchronized to avoid get interrupted. A class whose members are protected from such interruptions is called thread-safe.
  - Manual synchronization: uses a signaling mechanism for thread interaction. classes inherit WaitHandle: Mutex, Semaphore, etc.
  - Synchronized Context: for .NET Framework, use `SynchronizationAttribute` to sync all instance methods and fields.
  - Collection classes in the `System.Collections.Concurrent`.
  - Synchronized code regions: mark a code block as `lock`. Same as call `Monitor.Enter` and `Monitor.Exit`. If an exception is thrown inside the lock block, the finally handler runs to allow you to do any clean-up work.
  - `Thread.Interrupt` can be used to break a thread.
- Foreground vs. background threads: a background thread does not keep the managed execution environment running. So when foreground threads finish, the application ends.
  - `Thread.IsBackground` can check and set a thread to be background.
  - threads in the managed thread pool (`IsThreadPoolThread`) are background threads.
  - threads creating and starting a new thread are by default forground threads.
- Unmanaged thread can be found in the managed execution environment. Then a new thread object is created.
- here: <https://learn.microsoft.com/en-us/dotnet/standard/threading/managed-and-unmanaged-threading-in-windows#managed-threads-and-com-apartments>
- here: <https://learn.microsoft.com/en-us/windows/win32/com/processes--threads--and-apartments>

[Multithreading With .NET](https://www.c-sharpcorner.com/UploadFile/84c85b/multithreading-with-net/)

- The operating system schedules threads.
- Thread: Sleep, GetDomain, CurrentContext, Start, Name, IsBackground. new Thread(delegate);
- Mutex
- Monitor
- Semaphore
- Interlock
- Threadpool
- ThreadPriority
- When foreground threads are still running, even `main()` method ends, process keeps running.
- **HERE**: Race condition

### Dispose

If an object is disposed, you cannot access it's field any more.

[example in DB](https://stackoverflow.com/questions/5350109/cannot-access-a-disposed-object)

## MySQL

[MySQL connect](https://stackoverflow.com/questions/21618015/how-to-connect-to-mysql-database)

## SQLite

<https://stackoverflow.com/questions/19665370/missing-sqlite-data-provider-in-vs-2013>

- My solution: find `SQLite.Interop.dll` and put under the run path. For example:
- Build setting: Debug x64

Folder

```C#
Project
  bin
    x64
      Debug
        put x64\SQLite.Interop.dll here
```

<http://blog.tigrangasparian.com/2012/02/09/getting-started-with-sqlite-in-c-part-one/>

## UI

See VisualStudio.md

Choose Windows Application.

FlowLayoutPanel: arranges its contents in a horizontal or vertical flow direction. You can wrap the control's contents from one row to the next, or from one column to the next.

## Json Schema

[Newtonsoft.Json.Schema](https://www.newtonsoft.com/jsonschema/help/html/LoadingSchemas.htm)

## gRPC

<https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-6.0&tabs=visual-studio>

<https://grpc.io/docs/what-is-grpc/introduction/>

<https://github.com/grpc/grpc-dotnet/tree/master/examples#mailer>

<https://learn.microsoft.com/en-us/windows/win32/rpc/rpc-start-page>

<https://learn.microsoft.com/en-us/aspnet/core/grpc/authn-and-authz?view=aspnetcore-6.0>

## Open Telemetry

<https://opentelemetry.io/docs/instrumentation/net/getting-started/>

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

## Miscellaneous

### Compare two lists

<https://msdn.microsoft.com/en-us/library/bb348567(v=vs.110).aspx>

### Write to a file

<https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-write-to-a-text-file>

### Action

<https://msdn.microsoft.com/en-us/library/018hxwa8(v=vs.110).aspx>

### sleep

`Thread.sleep`

### GUID never conflict

<https://www.guidgen.com/>

### [Traverse an Enum](https://stackoverflow.com/questions/972307/can-you-loop-through-all-enum-values)

`var values = Enum.GetValues(typeof(Foos));`

### [foreach skip one](https://stackoverflow.com/questions/7942389/how-to-skip-a-specific-position-within-a-for-each-loop-in-c-sharp)

Can also use if == continue.

### [ReadOnlyDictionary](http://www.dotnetcurry.com/dotnet/973/read-only-dictionary-dotnet-45)

Need create by a dictionary and convert to readonly.

### [obj folder struct](https://social.msdn.microsoft.com/Forums/en-US/456ebb0e-6fa3-4a77-a723-6984c5208562/what-is-with-all-of-the-files-for-a-simple-program?forum=csharpide)

### [Tuple as key](https://stackoverflow.com/questions/955982/tuples-or-arrays-as-dictionary-keys-in-c-sharp)

### [print object id](https://stackoverflow.com/questions/5703993/how-to-print-object-id)

### [using for unmanaged resources](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement)

### Cannot use out in linq

Cannot:

```C#
void foo(out int i) {
    ()=>(i);
}
```

Can:

```C#
void foo(out int i) {
    int j;
    ()=>(j);
    i = j;
}
```

### Check if a base object is a derive class

```C#
MyBase myDerive = new MyDerive();
MyDerive res = myDerive as MyDerive;

if (res == null)
{
    Console.WriteLine("It is not derive");
}
```

### Public attribute

Don't forget to add `[DataMember]`

### sealed class

Cannot inherit from it.

### string with @

<https://stackoverflow.com/questions/6134547/what-does-the-prefix-do-on-string-literals-in-c>

Means no need to escape.

### Customize set accessor

<https://stackoverflow.com/questions/1227205/why-can-i-not-add-a-set-accessor-to-an-overriden-property>

```C#
class MyClass
{
    private int myPropertyCpy;

    public int myProperty
    {
        get { return myPropertyCpy; } // cannot use myProperty directly because this will cause infinity loop.
        set { myPropertyCpy = value; }
    }
}
```

### Entity Data Model

[Entity type](https://msdn.microsoft.com/en-us/library/ee382837(v=vs.100).aspx) vs. [Complex Type](https://msdn.microsoft.com/en-us/library/ee382831(v=vs.100).aspx)

Entity type

- Entity name
- Key
- Properties: are also entities.

Complex types

- It is used as value of entites or other complex types.
- do not have identities and therefore cannot exist independently.

<https://msdn.microsoft.com/en-us/library/jj652004(v=vs.113).aspx>

### Make internal visiable in other class

In the class that define as internal, update the AssemblyInfo.cs

```C#
[assembly: InternalsVisibleTo("<namespace>")]
```

### Resolve duplicate reference

Add the line at the top

```C#
using dup = rightnamespace.dup;
```

### DataView

System.Data.DataView.

`foreach(var dataRow in dataView)` to traverse.

### Process

```C#
ProcessStartInfo startInfo = new ProcessStartInfo("ProcessName");
startInfo.UseShellExecute = false;
startInfo.RedirectStandardOutput = true;
startInfo.RedirectStandardError = true;
StringBuilder argumentBuilder = new StringBuilder();
// Add arguments.
startInfo.Arguments = argumentBuilder.ToString();

Process process = Process.Start(startInfo);
string stdout = process.StandardOutput.ReadToEnd();
string stderr = process.StandardError.ReadToEnd();
process.WaitForExit(10 * 1000); // wait 10 seconds.

if (process.ExitCode == 0)
{
    Logging.Logger.LogInfo(stdout);
}
else
{
    Logging.Logger.LogError(stderr);
}
```

### Attribute

[src](https://www.infoworld.com/article/3006630/application-development/how-to-work-with-attributes-in-c.html)

Attribute

- can add metadata information to your assemblies.
- an object that is associated with element:
  - Assembly
  - Class
  - Method
  - Delegate
  - Enum
  - Event
  - Field
  - Interface
  - Property
  - Struct
- used to associate declarative information: the info that can be retrived at runtime when using reflection.

### LINQ Expression

[src](https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions?view=netframework-4.7.2)

### Set a HttpWebRequest timeout

```C#
HttpWebRequest myHttpWebRequest=(HttpWebRequest)WebRequest.Create("http://www.contoso.com");
myHttpWebRequest.Timeout=10;
HttpWebResponse myHttpWebResponse=(HttpWebResponse)myHttpWebRequest.GetResponse();
```

### Catch and Rethrow

[src](https://stackoverflow.com/questions/881473/why-catch-and-rethrow-an-exception-in-c)

Catch and rethrow lose all stack trace.

### Async and await

[Public Doc](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/await)

[Task Async Programming](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/index)

Async vs Parallel: async is done by one core. Parallel is done by multiple cores.

If use `await` without `Task`, each step still runs sequencially. When init a `Task` instance, the task starts. When call `await myTask`, it blocks.

[Example](https://stackoverflow.com/questions/14455293/how-and-when-to-use-async-and-await)

[Result vs. await](https://stackoverflow.com/questions/36594857/tasktresult-result-vs-await-a-task)

- `Task.Result` blocks the current thread until it returns.

### Inversion of control

<https://en.wikipedia.org/wiki/Inversion_of_control>

<http://jinnianshilongnian.iteye.com/blog/1413846>

### Compare two list

`UnorderedEnumerableComparer<Guid>.Default.Compare(guidList1, guidList2) != 0`

### Resolve conflict for same namespace in two different dlls/projects

This can happen when both projects reference to a differant versions of a same lib. One easiest way to resolve it is to use alias of the project. [alias](http://csc-technicalnotes.blogspot.com/2009/07/type-exists-in-both-dlls.html)

In csproj file, add

```xml
<ProjectReference Include="MyProjectCausingConflict.csproj">
    <Project>{Guid}</Project>
    <Name>MyProjectCausingConflict</Name>
    <Aliases>ResolveConflict</Aliases>
</ProjectReference>
```

```c#
// Change from
using MyProjectCausingConflict;

// to
extern alias ResolveConflict;
using ResolveConflict.MyProjectCausingConflict;
```

### XML serialize an object

[src](https://stackoverflow.com/questions/2434534/serialize-an-object-to-string)

```C#
using (StringWriter textWriter = new StringWriter())
{
  XmlSerializer.Serialize(textWriter, objectToSerialize);
  string result = textWriter.ToString();
}
```

### System.CodeDom

Used to generate C# code

- `CodeTypeDeclaration`: the class name
- `CodeAttributeDeclaration`: the attribute name and its value

See [ref](https://docs.microsoft.com/en-us/dotnet/api/system.codedom.codeattributedeclaration?view=dotnet-plat-ext-3.1)

```C#
[attrName(attr_value)]
public class ClassName {
}
```

### System.Xml.Serialization

[Serialize a class to an xml example](https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlschemaproviderattribute?view=netcore-3.1#examples)

- Define a stream class inherits `IXmlSerializable`, with an `XmlSchemaProvider` attribute, which has a value to a `XmlSchemaProviderAttribute`. The xmlSchema is the xsd file.
- Define a method with the name of the XmlSchemaProviderAttribute value. It accepts an `XmlSchemaSet`, and returns a `XmlQualifiedName`.
  - This method is called by the framework to get the schema of this type.
  - First use a `XmlSerializer` to deserialize the xsd file to a `XmlSchema` object.
  - Then add the schema to `XmlSchemaSet`.
  - Retuen a new `XmlQualifiedName` with the stream name and the namespace.
- Implement `IXmlSerializable.WriteXml(XmlWriter)`. Seems like this auto applied the schema.

Schema terms: see examples in [XmlSchemaEnumerationFacet](https://docs.microsoft.com/en-us/dotnet/api/system.xml.schema.xmlschemaenumerationfacet?view=netcore-3.1#examples)

- XmlSchemaSimpleType: xs:simpleType
- XmlSchemaSimpleTypeRestriction: xs:restriction. Accpected values. Restrictions on XML elements are facets.
- XmlSchemaEnumerationFacet: xs:enumeration. The restriction on a set of values. i.e., the values could appear in a value
- XmlSchemaElement: xs:element.
- XmlSchemaComplexType: xs:complexType. contains other elements and/or attributes.
- XmlSchemaAttribute: xs:attribute.

### XmlDocument

[Load XML into the document object model](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmldocument?view=net-6.0#load-xml-into-the-document-object-model)

```C#
using (XmlReader reader = XmlReader.Create(filePath))
{
    XmlDocument doc = new XmlDocument();
    doc.Load(reader);
}
```

[Navigate the document tree](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmldocument?view=net-6.0#navigate-the-document-tree)

[XPath](https://www.w3schools.com/xml/xpath_nodes.asp)

### Predicate

[Doc](https://docs.microsoft.com/en-us/dotnet/api/system.predicate-1?view=netcore-3.1)

Define a rule to search.

### volatile

[Doc](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/volatile)

- put before a field might be modified by multiple threads that are executing at the same time.
- if without `volatile` key word, then in the multi-thread program the sequence updating to this field is unpredictable. CPU might rearange the read-write orders of sequences.

### EventHandler

- [EventHandler Doc](https://docs.microsoft.com/en-us/dotnet/api/system.eventhandler?view=netcore-3.1)
- It is a delegate which accepts a sender and an Arg instance.
- The Arg instance can be used by the registered listener to do some precondition check and act on it.
- [event Doc](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/event)
- `event` are a special kind of multicast delegate that can only be invoked from within the class or struct where they are declared (the publisher class). If other classes or structs subscribe to the event, their event handler methods will be called when the publisher class raises the event.

### Reflection

- Get type: `Type t = myInstance.GetType();` or `Type t = typeof(MyClass);`
- Get properties: `IEnumerable<PropertyInfo> properties = myType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);`
- Create an instance for a special type not using `new`: `Activator.CreateInstance()`. [Doc](https://docs.microsoft.com/en-us/dotnet/api/system.activator.createinstance?view=netcore-3.1)
- To get more info of the type: `myType.IsConstructedGenericType` and `myType.GetGenericTypeDefinition()`

- To make a method that bind to a value type to accept the run time value type: `MakeGenericMethod(valueType)`

Traserve Assembly

- <https://stackoverflow.com/questions/20528291/loop-through-all-classes-in-a-given-namespace-and-create-an-object-for-each>
- <https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly.gettypes?view=net-5.0>

### Listen to File change

<https://docs.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=net-5.0>

File used by another process:

- <https://stackoverflow.com/questions/26741191/ioexception-the-process-cannot-access-the-file-file-path-because-it-is-being>
- <https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use>

### Numeric Suffixes

```C#
1;    // int
1.0;  // double
1.0f; // float
1.0m; // decimal
1u;   // uint
1L;   // long
1UL;  // ulong
```

### OutOfMemory

<https://docs.microsoft.com/en-us/dotnet/api/system.outofmemoryexception?view=net-5.0>

### Embedded resources

<https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file>

### Singleton

<https://stackoverflow.com/questions/12316406/thread-safe-c-sharp-singleton-pattern>

<https://csharpindepth.com/Articles/Singleton#dcl>

### Stream

<https://docs.microsoft.com/en-us/dotnet/api/system.io.file.openread?view=net-5.0>

### GC

<https://stackoverflow.com/questions/151051/when-should-i-use-gc-suppressfinalize#:~:text=5%20Answers&text=SuppressFinalize%20should%20only%20be%20called,object%20was%20cleaned%20up%20fully.&text=Normally%2C%20the%20CLR%20keeps%20tabs,them%20more%20expensive%20to%20create).>

### File System Operations

<https://stackoverflow.com/questions/674479/how-do-i-get-the-directory-from-a-files-full-path>

<https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getfiles?view=net-5.0>

### Mobile App Development

<https://docs.microsoft.com/en-us/visualstudio/cross-platform/cross-platform-mobile-development-in-visual-studio?view=vs-2019>

<https://www.toptal.com/android/developing-mobile-web-apps-when-why-and-how>

<https://www.freecodecamp.org/news/how-to-turn-your-website-into-a-mobile-app-with-7-lines-of-json-631c9c9895f5/>

### Check DLL info

`ildasm.exe *.dll`

It is under `C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools\`

### PackageReference Private

[including package references and then private assets in csproj](https://forums.asp.net/t/2162896.aspx?including+package+references+and+then+private+assets+in+csproj#:~:text=By%20default%2C%20all%20package%20assets%20are%20included.%20%60PrivateAssets%60,by%20default%20when%20this%20attribute%20is%20not%20present.)

- `IncludeAssets` attribute specifies which assets belonging to the package specified by `<PackageReference>` should be consumed. By default, all package assets are included. `PrivateAssets` attribute specifies which assets belonging to the package specified by `<PackageReference>` should be consumed but not flow to the next project. The Analyzers, Build and ContentFiles assets are private by default when this attribute is not present.

### Interlock

<https://docs.microsoft.com/en-us/dotnet/api/system.threading.interlocked?view=net-5.0>

### WCF

[Windows Communication Foundation](https://learn.microsoft.com/en-us/dotnet/framework/wcf/whats-wcf)

[Native APIs](https://learn.microsoft.com/en-us/dotnet/api/system.servicemodel?view=dotnet-plat-ext-6.0)

[Http auth](https://learn.microsoft.com/en-us/dotnet/framework/wcf/feature-details/understanding-http-authentication)

- Anonymous: request does not contain any authentication information
- Negotiate: automatically selects between the Kerberos protocol and NTLM authentication, depending on availability

### Extend a method

<https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods>

### Transitive dependency

<https://devblogs.microsoft.com/nuget/introducing-transitive-dependencies-in-visual-studio/#:~:text=There%20is%20now%20a%20new,level%20dependency%20at%20any%20time.>

### Top Level Statement

<https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/top-level-statements>

Define a namespace: <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-6.0>

### Powershell

<https://stackoverflow.com/questions/34398023/get-powershell-commands-output-when-invoked-through-code>

### Microsoft.Office.Interop

- Import COM <https://stackoverflow.com/questions/19543279/microsoft-office-interop-excel-reference-cannot-be-found>
- Excel R/W data <https://stackoverflow.com/questions/68666728/c-sharp-com-interop-excel-how-to-write-to-cells-from-c-sharp-using-interop-exce>

If use admin permission to start excel, the autosave function is not working.

- <https://stackoverflow.com/questions/2476070/how-to-specify-the-user-in-a-c-sharp-thread>

### Build Warnings

DLL conflicts: <https://learn.microsoft.com/en-us/visualstudio/msbuild/errors/msb3277?view=vs-2022>

Suppress build warning: <https://stackoverflow.com/questions/49564022/suppressing-warnings-for-solution>

The dotnet restore might fail when see nuget version downgrade.

The target framework version in the nuget package also matters because otherwise it will throw exception saying dll not found.

Open binding redirect log: <https://stackoverflow.com/questions/255669/how-to-enable-assembly-bind-failure-logging-fusion-in-net>

ildasm

Assembly version vs. nuget package version.

### Shims

A unit test framework.

### TLS

<https://stackoverflow.com/questions/30664566/authentication-failed-because-remote-party-has-closed-the-transport-stream>

### csvhelper

<https://joshclose.github.io/CsvHelper/getting-started/>

### Build Race condition

- <https://stackoverflow.com/questions/5134137/build-error-the-process-cannot-access-the-file-because-it-is-being-used-by-ano>
- <https://stackoverflow.com/questions/6838779/msbuild-fails-with-the-process-cannot-access-the-file-xxxxx-because-it-is-being>

### SDK style project differences

<https://stackoverflow.com/questions/46709000/disable-transitive-project-reference-in-net-standard-2>

<https://dansiegel.net/post/2018/08/21/demystifying-the-sdk-project>

- Release builds need to be optimized, while Debug configurations need all of our debug symbols
- [Well-Known Properties](https://learn.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2015/msbuild/msbuild-reserved-and-well-known-properties?view=vs-2015&redirectedfrom=MSDN)
- ItemGroup grouping Items. Can be used with Condition
- Supports multi-target frameworks.
- Common lib should be packed because: 1. reduce build time, 2. versioning, 3. isolate rollout, 4. individual testing
- using a nuspec is not needed for SDK style projects. Can set `GeneratePackageOnBuild` to true in csproj

<https://hermit.no/moving-to-sdk-style-projects-and-package-references-in-visual-studio-part-1/>

<https://hermit.no/moving-to-sdk-style-projects-and-package-references-in-visual-studio-part-2/>

### ConfigurationManager

Read from dll.config vs. app.config:

```C#
Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
var configVal = config.AppSettings.Settings["key"].Value;
configVal = ConfigurationManager.AppSettings["key"].Value;
```

### DLL Binding Redirect AutoUnify

<https://stackoverflow.com/questions/33256071/what-is-autounify-and-why-is-it-causing-working-tests-to-fail-in-tfs-2015>
