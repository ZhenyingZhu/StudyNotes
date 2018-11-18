# JavaScript Related

## JavaScript

### Resources

<http://www.w3school.com.cn/js/index.asp>

### Introduction

[src1](http://www.w3school.com.cn/js/js_howto.asp), [src2](http://www.w3school.com.cn/js/js_whereto.asp)

HTML 中的脚本(也就是HTML无法访问的内容，如变量，函数内容，而非函数名)必须位于`<script>` 与`</script>` 标签之间。脚本可被放置在HTML 页面的`<body>` 和`<head>` 部分中。函数通常放于`<head>` 中或`<body>` 底部。

旧版本需`<script>` 标签中使用`type="text/javascript"` 。现在不必。

可把脚本存于*.js文件中，并通过`<script src="myScript.js"></script>`导入。外部脚本不能有`<script>`标签。

### Output

- write to an exsiting HTML element: `innerHTML`.
- write to somewhere in the whole HTML page: `document.write()`.
- write to a alert box: `window.alert()`.
- write to browser console: `console.log()`.

脚本通过`id`访问HTML元素。`document.getElementById("id")`。HTML元素的内容为`innerHTML`。

脚本可设置在某个事件发生时执行代码。

点击Try it 按钮，则A Paragraph变为My First JavaScript Function。

```html
<!DOCTYPE html>
<html>
<head>
<script>
function myFunction(){
  document.getElementById("demo").innerHTML="My First JavaScript Function";
}
</script>
</head>
<body>

<h1>My Web Page</h1>
<p id="demo">A Paragraph</p>
<button type="button" onclick="myFunction()">Try it</button>
</body>
</html>
```

直接写入HTML输出中：在页面加载时script向页面写文本`document.write()`。

Never call document.write after the document has finished loading. It will overwrite the whole document.

```html
<body><p></p>
<script>
  document.write("<h1>This is a heading</h1>");
  document.write("<p>This is a paragraph</p>");
</script>
<button type="button" onclick="document.write(5 + 6)">Try it</button>
</p></p></body>
```

### Syntax

[src](http://www.w3school.com.cn/js/js_statements.asp)

Statement

- 脚本代码用`;`分隔，但不必须用来结束语句。
- JavaScript对大小写敏感。
- 忽略多余空格。
- 可在字符串中用`\`跳脱回车。但是不能用于其它地方。

JavaScript是脚本型语言，边读取代码边执行。

注释：`//`, `/**/`

Keywords

- break
- continue
- debugger: Stops the execution of JavaScript, and calls (if available) the debugging function.
- do ... while
- for
- function
- if ... else
- return
- switch
- try ... catch
- var

Comparison operators

- `===`: equal value and equal type.
- `!==`
- `?`: tenary operator.

Type operators

- `typeof`: `typeof "John Doe" // string`.
- `instanceof`

[Operator Precedence Values](https://www.w3schools.com/js/js_arithmetic.asp)

- `in`: Property in Object.
- `yield`: Pause function.

`null` is an object. `undefined` is actually null.

- `null == undefined` true.
- `null === undefined` false.

arrays are objects.

Variable

- 声明(declare)变量：`var x;`。赋值`x=2;`。`var name="Bill";` 变量名一定要以字母开始。不赋值之前值为`undefined`。
- 重复声明不会使值消失。`var x=2; var x; // x is still 2`
- 数据类型是动态的，可赋不同类型的值。
- 字符串用单或双引号括起来。内部可包含与整个字符串外部不同的引号。

null值：可通过将变量设为此值将变量设为undefined。

### Events

`<element event="some JavaScript"/>`

An example: button is the element, onclick is the event.

```javascript
<button onclick="displayDate()">The time is?</button>

<script>
function displayDate() {
    document.getElementById("demo").innerHTML = Date();
}
</script>

<p id="demo"></p>
```

Common HTML Events:

- `onchange`
- `onclick`
- `onmouseover`
- `onmounseout`
- `onkeydown`
- `onload`

### Object

[src](http://www.w3school.com.cn/js/js_obj_intro.asp)

JavaScript中所有事物都是对象。

JavaScript面向对象但不使用类，使用prototype。

[src](http://www.w3school.com.cn/js/js_objects.asp)

- 对象：属性值对形式定义。`var person={firstname:"Bill", lastname:"Gates", id:5566};`
  - 寻址方式 access object properties：`name=person.lastname;` 或 `name=person["lastname"];`
  - 调用方法：`objectName.methodName();`
  - 声明新变量: `person=new Object(); person.firstname="Bill"; person.lastname="Gates"; person.age=56; person.eyecolor="blue";`
  - 使用literals创建：`person={firstname:"John",lastname:"Doe",age:50,eyecolor:"blue",fuuName:function(){return this.firstName+" "+this.lastName}};`
  - 使用构造器：`var myFather=new person("Bill","Gates",56,"blue");`

Functions

- Accessing a function without () will return the function definition, which means the code of the function represent in string.

构造器函数

```javascript
function person(firstname,lastname,age,eyecolor){
  this.firstname=firstname;
  this.lastname=lastname;
  this.age=age;
  this.eyecolor=eyecolor;
  function changeName(name){
    this.lastname=name;
  }
}
```

声明变量类型：

```javascript
var carname=new String;
var x=      new Number;
var y=      new Boolean;
var cars=   new Array;
var person= new Object;
```

```javascript
var x = "John";
var y = new String("John");
var z = new String("John");

x == y // true
x === y // false. x is string, y is object.
y == z // false. They are different objects. Compare two objects will always be false.
```

- After hit the first variable that is a string, all the numbers variables that are not yet evaludated will be treated as concating strings. `2+3+"5"=55`.
- 数字类型只有一种，小数点可有可无。`var z=123e-5;` 均使用8 byte十位底的浮点数存储。
  - 整数精度最多17位，小数误差通过对每个操作数先乘十再除十消除。
  - 前缀为0和x的为8和16进制。
- 方法：`toExponential()`，`toFixed()`，`toPrecision()`，`toString()`，`valueOf()`
- 布尔：`var x=true;` 通过构造器时，`var x=new Boolean(值);` 空是`false`，0是`false`, 1是`true`, `false`, `null`是`false`, `NaN`是`false`,字符串`'false'`是`true`。

数值类型属性：`MAX VALUE`，`MIN VALUE`，`NEGATIVE INFINITIVE`，`POSITIVE INFINITIVE`，`NaN`，`prototype`，`constructor`

字符串对象：

- 属性：txt.length=5
- 方法：
  - `txt.indexOf(str,pos)`返回str第一次出现在txt中位置，如果在头出现就为0；
  - `txt.lastIndexOf()`;
  - `txt.search()`; Can take Regular express.
  - `txt.slice(start, end)`;
  - `txt.substring(start, end)`;
  - `txt.substr(start, length)`;
  - `var str = txt.replace(/MICROSOFT/i, W3Schools")`; Case sensitive. Accept Regex. Flags: `/i` ignore case. `/g` global match.
  - `toUpperCase()`;
  - `toLowerCase()`;
  - `concat()`;
  - `trim()`; same as `str.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, '')`
  - `txt.match(str)`查找str，如果找到返回str，不然返回Null;
  - `charAt`
  - `charCodeAt`: return the UTF-16 code the char.
  - `str[0]`: It makes strings look like arrays (but they are not).
  - `txt.split(",");` or even `txt.split("");`

Understanding of the regular express: `/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g`

- Use <https://regex101.com/>
- `\s` any unprintable chars equal to `[\r\n\t\f\v ]`
- `\uFEFF` this is BOM.
- `\xA0` is the space.
- `|` means the alternative.
- `/g` means not return after the first match. Without it the spaces at the end won't be found.

Define a method to replace the build-in string method:

```javascript
<script>
if (!String.prototype.trim) {
  String.prototype.trim = function () {
    return this.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, '');
  };
};
</script>
```

```javascript
var x = 0.2 + 0.1; // x will be 0.30000000000000004
var x = (0.2 * 10 + 0.1 * 10) / 10; // x will be 0.3
```

JavaScript will try to convert strings to numbers in all numeric operations, except `+`.

```javascript
var x = "100";
var y = "10";
var z = x / y; // z will be 10
```

NaN means not a number.

```javascript
var x = 100 / "Apple"; // x will be NaN (Not a Number)
isNaN(x); // true

var x = 100 / "10"; // x will be 10

typeof NaN; // returns "number"
```

Infinity (or -Infinity) is a number.

```javascript
var x =  2 / 0; // x will be Infinity
var y = -2 / 0; // y will be -Infinity
```

Never write a number with a leading zero (like 07). Some JavaScript versions interpret numbers as octal if they are written with a leading zero.

Use toString to display in base.

```javascript
var myNumber = 32;
myNumber.toString(16); // 32 in base 16 is 10.
```

methods and properties are also available to primitive values.

```javascript
(123).toString();
```

Number methods:

- `toExponential()`
- `toFixed()`
- `toPrecision()`
- `valueOf()`: There is no reason to use it in your code.

Global JavaScript Methods invole numbers:

- `Number(new Date("2017-09-30"))`
- `parseInt()`
- `parseFloat()`

Number properties

```javascript
var x = Number.MAX_VALUE;
var y = x.MAX_VALUE; // cannot use on virable. y becomes undefined
```

- `MAX_VALUE`
- `MIN_VALUE`
- `POSITIVE_INFINITY`
- `NEGATIVE_INFINITY`
- `NaN`

- 数组：
  - 创建`var cars=new Array(); cars[0]="Audi"; cars[1]="BMW"; cars[2]="Volvo";`
  - 或condensed array: `var cars=new Array("Audi","BMW","Volvo");`
  - 或literal array: `var cars=["Audi","BMW","Volvo"];`
  - 如果你需要在数组内指定数值或者逻辑值，那么变量类型应该是数值变量或者布尔变量，而不是字符变量。`true` vs `"true"`
  - `arr.length`
  - 合并数组：`arr.concat(arr2);`
  - 将整个数组组成字符串：`arr.join(".");` 如果不加字符则缺省为`","`。
  - `slice()`
  - 排序：`arr.sort();`, `reverse()`.
  - `arr.push(ele)`: same as `arr[arr.length] = ele;`
  - `pop()`
  - `shift()`
  - `unshift()`
  - `delete fruits[0];` delete and leave a hole.
  - `fruits.splice(2, 0, "Lemon", "Kiwi");` 2 is the index to insert, 0 is the number of elements to remove.
  - `var numbers2 = numbers1.map(myFunction);`
  - `var over18 = numbers.filter(myFunction);`
  - `var sum = numbers1.reduce(myFunction);`, `var sum = numbers1.reduceRight(myFunction);`.
  - `var isAllOver18 = numbers.every(myFunction);`
  - `var isSomeOver18 = numbers.some(myFunction);`
  - `indexOf()`
  - `lastIndexOf()`
  - `find()`
  - `findIndex()`

```javascript
var fruits = ["apple", "banana"]
var text = "<ul>";
fruits.forEach(myFunction);
text += "</ul>";

function myFunction(value) {
    text += "<li>" + value + "</li>";
}
```

ECMAScript 5 defines a method to check if an object is as array, because `typeof(arr)` returns object.

```javascript
Array.isArray(fruits); // returns true
```

Same as

```javascript
function isArray(x) {
    return x.constructor.toString().indexOf("Array") > -1;
}
```

and

```javascript
fruits instanceof Array;
```

To avoid sort alphabetically:

```javascript
var points = [40, 100, 1, 5, 25, 10];
points.sort(function(a, b){return a - b});

points.sort(function(a, b){return 0.5 - Math.random()}); // random
```

Find max/min in an array

```javascript
Math.max.apply(null, arr); // equals to Math.max(arr[0], arr[1] ...)
```

日期对象：

- `Date()`获取当前日期；
- `getTime()`返回1970.1.1至今的毫秒数；
- `date.setFullYear(1992,10,1)`,注意是月是从0开始的为1月；
  - `myDate.setFullYear(2008,8,9);var today = new Date();`
- `date.toUTCString()`返回UTC标准的字符串；
- `date.toDateString()`.
- `date.getDay()`返回数字表示的今天是周几；
- ISO Date: `2018-11-03`.
- 构造器：`var d=new Date();`
- `Date.parse("March 21, 2012")` return the ms since 1970.
- 可用`<`, `>`比较日期：
  - `if (myDate>today){alert("before 9th August 2008");}`
- `date + 50` add 50 days.

More methods see <https://www.w3schools.com/jsref/jsref_obj_date.asp>

Note the difference between using number (millisecond) vs. using string (ISO Date).

```javascript
var d1 = new Date(2018); // It is 2018 ms since 1970.
var d2 = new Date(2018, 1); // With the month, it is Feb 1 2018.
var d3 = new Date("2018"); // It is Jan 1 2018 in the timezone of the current computer.
var d4 = new Date("2018-1"); // Jan 1 2018.
var d5 = new Date("2018-1-1Z"); // In UTC/GMT.
```

算术对象：

- `Math.round(数)` 按小数部分最高位四舍五入；
- `Math.random()`随机产生一个0到1之间的值，产生0～10之间：`Math.floor(Math.random()*11)`；
- `Math.max(a, b)`返回两个数中较大的值；
- `Math.min(a, b)`；
- `Math.sin(90 * Math.PI / 180);` 90 degree. It only accept radians.
- 可被访问的算术值：
  - 常数: `Math.E`;
  - 圆周率: `Math.PI`;
  - 2 的平方根: `Math.SQRT2`;
  - 1/2 的平方根: `Math.SQRT1_2`;
  - 2 的自然对数: `Math.LN2`;
  - 10 的自然对数: `Math.LN10`;
  - 以 2 为底的 e 的对数: `Math.LOG2E`;
  - 以 10 为底的 e 的对数: `Math.LOG10E`

More methods see <https://www.w3schools.com/jsref/jsref_obj_math.asp>

Object constructor:

```javascript
// Constructor function for Person objects
function Person(first, last, age, eye) {
    this.firstName = first;
    this.lastName = last;
    this.age = age;
    this.eyeColor = eye;
}

// Create two Person objects
var myFather = new Person("John", "Doe", 50, "blue");
var myMother = new Person("Sally", "Rally", 48, "green");
```

Add method to prototype:

```javascript
Person.prototype.name = function() {
    return this.firstName + " " + this.lastName;
};
```

### Expression

[src](http://www.w3school.com.cn/js/js_functions.asp)

运算符：

- 算术：`+-*/%,++,--`
- 赋值：`=,+=,-=,*=,/=,%=`
- 字符串运算符：`string1+string2`。数字与字符串相加，数字转为字符。
- 比较：`==`,`===`值和类型全等，`!=`，`>=`，`<=`，`>`，`<`
- 逻辑：`&&` `||` `!`
- 条件运算：`variablename=(condition)?value1:value2;`

When compare, make sure compare numbers not string.

```javascript
age = Number(age);
if (isNaN(age)) {
    voteable = "Input is not a number";
} else {
    voteable = (age < 18) ? "Too young too simple" : "Old enough";
}
```

函数：`function myFunction(参量表){var x; return x;}`

函数内部的是局部变量，函数外的是全局变量。全局变量可直接在函数内部使用。全局变量在页面关闭时删除。

条件控制：

- `if`：

```javascript
if (time<10) {
  x="Good morning";
} else if (time<20) {
  x="Good day";
} else {
  x="Good evening";
}
```

- `switch`：

Switch cases use strict comparison (===).

```javascript
var day=new Date().getDay();
switch (day) {
case 6:
  x="Today it's Saturday";
  break;
case 0:
  x="Today it's Sunday";
  break;
default:
  x="Looking forward to the Weekend";
}
```

- `for`：

```javascript
for (var i=0,len=cars.length; i<len; i++) {
  document.write(cars[i] + "<br>");
}
```

或

```javascript
var i=2,len=cars.length;
for (; i<len; i++)
{
  document.write(cars[i] + "<br>");
}
```

- `for in`：对对象中的所有属性执行一遍。

```javascript
var person={fname:"John",lname:"Doe",age:25};
for (x in person){
  txt=txt + person[x];
}
```

也可对数组遍历。

```javascript
var mycars =["Saab", "Volvo", "BMW"];
var x;
for (x in mycars){
  document.write(mycars[x] + "<br />")
}
```

- `while`：

```javascript
cars=["BMW","Volvo","Saab","Ford"];
var i=0;
while (cars[i]){
  document.write(cars[i] + "<br>");
  i++;
}
```

- `do while`：

```javascript
do{
  x=x + "The number is " + i + "<br>";
  i++;
}while (i<5);
```

- 循环控制：`break`，`continue`。

对语句设定label以后，break label可跳出代码块。

```javascript
cars=["BMW","Volvo","Saab","Ford"];
list: {
  document.write(cars[0] + "<br>");
  break list;
  document.write(cars[3] + "<br>");
  document.write(cars[4] + "<br>");
  document.write(cars[5] + "<br>");
}
```

Primitive Data types:

- string
- number
- boolean
- object
- function

Object types:

- Object
- Date
- Array

No value types:

- null
- undefined

constructor: can be `Array` or `Date`.

```javascript
myDate.constructor === Date;
```

### 正则表达式

[src](http://www.w3school.com.cn/js/js_obj_regexp.asp)

RegExp: 多个字符，解析，格式检查，替换。设置索引位置，要检查的字符串类型。

- 声明：`var patt1=new RegExp("e");` 单个字符'e'表明检查字符串中是否存在这个字符。
- Or `var patt = /w3schools/i;` following `/pattern/modifiers;`.
  - 可添加参数如`var patt1=new RegExp("e","g")`; `g`表明查找所有符合的结果。
- `test()`方法为检索字符串中的指定值，返回布尔值。`patt1.test("The best things in life are free");` 就为true。
- `exec()`同`test()`，但返回该指定值或Null。
- `compile()` 方法用于改变 RegExp。`compile()` 既可以改变检索模式，也可以添加或删除第二个参数。

Modifier:

- `i`: case-insensitive.
- `g`: global match.
- `m`: multiple lines.

Metacharacters:

- `\d`: digit
- `\s`: whitespace.
- `\b`: begin or end.
- `\uxxxx`: unicode.

Quantifiers:

- `n+`: at least 1.
- `n*`: 0 or more.
- `n?`" 0 or 1.

More details see <https://www.w3schools.com/jsref/jsref_obj_regexp.asp>

```javascript
var patt1=new RegExp("e","g");
do
{
  result=patt1.exec("The best things in life are free");
  document.write(result);
}
while (result!=null) // output is eeeeeenull
```

等同于`var patt1=/d/g;`

```javascript
/e/.compile("d");
```

### 错误处理

[src](http://www.w3school.com.cn/js/js_errors.asp)

try catch

```javascript
var txt="";
try {
  ...
} catch(err) {
  txt+=err.message+"\n";
}
```

throw

```javascript
function foo() {
  var x=document.getElementById("demo").value;
  if (x<0)  throw "negative value";
}
```

Error name values:

- `RangeError`: A number "out of range" has occurred.
- `ReferenceError`: An illegal reference has occurred.
- `SyntaxError`:A syntax error has occurred. Include old error `EvalError`.
- `TypeError`: A type error has occurred.
- `URIError`: An error in encodeURI() has occurred.

### Scope

Global variables:

- in HTML, the global scope is the window object.
- If you assign a value to a variable that has not been declared, it will automatically become a GLOBAL variable.
- JavaScript Declarations are Hoisted: a variable can be used before it has been declared.
- Hoisting is JavaScript's default behavior of moving all declarations to the top of the current scope.
- Variables and constants declared with let or const are not hoisted!
- `"use strict";` in ECMAScript v5 can prevent those unexpected issues.

Not in strict mode, this in a function refer to the object window. In strict mode, it is undefined.

This in event handler refer to the element receive the event:

```javascript
<button onclick="this.style.display='none'">
    Click to Remove Me!
</button>
```

Explicit Function Binding:

```javascript
var person1 = {
    fullName: function() {
        return this.firstName + " " + this.lastName;
    }
}
var person2 = {
    firstName:"John",
    lastName: "Doe",
}
person1.fullName.call(person2); // Will return "John Doe"
```

In ECMAScript 2015, block scope is provided with `let`. Also `const` is provided.

```javascript
{
    var x = 2;
}
// x CAN be used here

{
    let x = 2;
}
// x can NOT be used here
```

The keyword const does NOT define a constant value, but a constant reference to a value. So the properties of constant objects can be changed.

### Debug

In browser's develop tool, console tab, the `console.log()` can be seen.

`debugger;` is used as a breakpoint.

```javascript
<script>
var text = '<p id="demo"></p>';
document.write(text);
debugger;
var x = 'content';
document.getElementById('demo').innerHTML = x;
debugger;
</script>
```

### Code style

Naming:

- Hyphens: not allowed in JS.
- Underscores.
- PascalCase.
- camelCase: JS perfered.

Put all declarations and init at the top of each script or function.

```javascript
// Declare at the beginning
var firstName = "", lastName = "";

// Use later
firstName = "John";
lastName = "Doe";
```

Always use `===` Comparison.

Always use parameter defaults:

```javascript
function (a=1, b=1) {
  // function code
}
```

Don't break return, because `return` is a closing statement and semicolon is not needed to complete it:

```javascript
function myFunction1(a) {
  return 3
    * a; // return 3 * a
}

function myFunction2(a) {
  return
    3 * a; // return undefined
}
```

To test if an object is empty:

```javascript
if (typeof myObj !== "undefined" && myObj !== null) {}
```

This loop:

```javascript
var i;
var l = arr.length;
for (i = 0; i < l; i++) { }
```

is faster than:

```javascript
var i;
for (i = 0; i < arr.length; i++) { }
```

DOM access is very slow, so store it as a local variable:

```javascript
var obj;
obj = document.getElementById("demo");
obj.innerHTML = "Hello";
```

Putting your scripts at the bottom of the page body lets the browser load the page first.

Avoid using any words from here as variable name: <https://www.w3schools.com/js/js_reserved.asp>

### JSON

J(ava)S(cript)O(bject)N(otation).

- Data is in name/value pairs: must use double quotes.
- Data is separated by commas
- Curly braces hold objects
- Square brackets hold arrays
- `var obj = JSON.parse(text);`
- `var text = JSON.stringify(obj);`

```json
{
  "employees":[
    {"firstName":"John", "lastName":"Doe"},
    {"firstName":"Peter", "lastName":"Jones"}
  ]
}
```

### ES5 and ES6 features

Object.defineProperty:

```javascript
// Create an Object:
var person = {
    firstName: "John",
    lastName : "Doe",
    language : "NO",
};
// Change a Property:
Object.defineProperty(person, "language", {
    value: "EN",
    writable : true,
    enumerable : false,
    configurable : true
});

Object.defineProperty(person, "language", {
get : function() { return language },
set : function(value) { language = value.toUpperCase()}
});

// Enumerate Properties
var txt = "";
for (var x in person) {
    txt += person[x] + "<br>";
}
document.getElementById("demo").innerHTML = txt;
```

A safe integer is an integer that can be exactly represented as a double precision number.

Arrow functions

```javascript
// ES5
var x = function(x, y) {
     return x * y;
}

// ES6
const x = (x, y) => x * y; // function is always const so don't use var.
```

### 表单验证

[src](http://www.w3school.com.cn/js/js_form_validation.asp)

```html
<!DOCTYPE html>
<html>
<head>
<script>
function validateForm() {
  var x = document.forms["myForm"]["fname"].value;
  if (x == "") {
    alert("Name must be filled out");
    return false;
  }
}
</script>
</head>
<body>

<form name="myForm" action="/action_page.php" onsubmit="return validateForm()" method="post">
  Name:
  <input type="text" name="fname">
  <input type="submit" value="Submit">
</form>

</body>
</html>
```

Constraint validation DOM methods:

- `checkValidity()`: Returns true if an input element contains valid data.
- `setCustomValidity()`: Sets the validationMessage property of an input element.

```html
<input id="id1" type="number" min="100" max="300" required>
<button onclick="myFunction()">OK</button>

<p id="demo"></p>

<script>
function myFunction() {
    var inpObj = document.getElementById("id1");
    if (!inpObj.checkValidity()) { // Auto check if 100 <= number <= 300.
        document.getElementById("demo").innerHTML = inpObj.validationMessage;
    }
}
</script>
```

HTML Constraint Validation: Browser can automatically run the form validation.

- disabled: Specifies that the input element should be disabled
- max: Specifies the maximum value of an input element
- min: Specifies the minimum value of an input element
- pattern: Specifies the value pattern of an input element
- required: Specifies that the input field requires an element
- type: Specifies the type of an input element

```html
<form action="/action_page.php" method="post">
  <input type="text" name="fname" required>
  <input type="submit" value="Submit">
</form>
```

### Objects

`var x = anObject;` x is a reference.

`objectName.property` is same as `objectName["property"]`.

So properties can be iterated.

```javascript
var txt = "";
var person = {fname:"Jhon", lname:"Doe", age:25};
for (x in person) {
  txt += person[x];
}
```

`delete` can deletes a property. It can only be used on object properties.

Property attributes:

- value.
- enumerable.
- configurable.
- writeable.

All attributes can be read, but only the value attribute can be changed.

Inherit:

- objects can inherit from their prototype.
- delete the prototype property will affect all inherited objects.

Add a method to an object:

```javascript
person.name = function() {
  return this.firstName + " " + this.lastName;
}
```

Why Using Getters and Setters?

- It gives simpler syntax
- It allows equal syntax for properties and methods
- It can secure better data quality
- It is useful for doing things behind-the-scenes

Use `Object.defineProperty()` to add getter:

```javascript
var obj = {counter : 0};
Object.defineProperty(obj, "reset", {
    get : function () {this.counter = 0;}
});
```

### Functions

Declaration:

```javascript
function myFunction(a, b) {
    return a * b;
}

var x = function (a, b) {return a * b}; // anonymous function

var myFunction = new Function("a", "b", "return a * b");
```

Self-invoking functions: Function expressions will execute automatically if the expression is followed by ().

```javascript
(function () {
    var x = "Hello!!"; // anonymous self-invoking function
})();

```

Functions have both properties and methods:

```javascript
function myFunction(a, b) {
    return arguments.length;
}

var txt = myFunction.toString();
```

JS doesn't check passed arguments. If a parameter is missing, the argument of it set to undefined.

```javascript
function myFunction(x, y) {
    if (y === undefined) {
          y = 0;
    }
}

function (a=1, b=1) { }
```

Use arguments property to implement max:

```javascript
x = findMax(1, 123, 500, 115, 44, 88);

function findMax() {
    var i;
    var max = -Infinity;
    for (i = 0; i < arguments.length; i++) {
        if (arguments[i] > max) {
            max = arguments[i];
        }
    }
    return max;
}
```

Arguments are passed by value, so changing values in arguments won't change the value outside the function.

A normal but not good way to invoke a function:

```javascript
function myFunction(a, b) {
    return a * b;
}
window.myFunction(10, 2);    // Will also return 20
```

With `call()` and `apply()`, an object can use a method belonging to another object.

```javascript
var person = {
    fullName: function(city, country) {
        return this.firstName + " " + this.lastName + "," + city + "," + country;
    }
}
var person1 = {
    firstName:"John",
    lastName: "Doe",
}
person.fullName.call(person1, "Oslo", "Norway");

person.fullName.apply(person1, ["Oslo", "Norway"]);
```

- The `call()` method takes arguments separately.
- The `apply()` method takes arguments as an array.

Variables created without the keyword var, are always global, even if they are created inside a function.

JavaScript Nested Functions/inner functions:

```javascript
var add = (function () {
    var counter = 0;
    return function () {counter += 1; return counter}
})();
```

- The self-invoke function only run once, and return the inner function it defines.
- The inner function is stored in var add.
- This is called a JavaScript closure. It is a function having access to the parent scope, even after the parent function has closed.

### 文档对象模型(DOM)

[src](http://www.w3school.com.cn/js/js_htmldom.asp)

当网页被加载时，浏览器会创建页面的文档对象模型（Document Object Model）。

![DOM Tree](./Javascript_files/DOM_Tree.jpg)

- JavaScript 能够改变页面中的所有 HTML 元素，HTML 属性，CSS 样式。
- JavaScript 能够对页面中的所有事件做出反应.
- JavaScript can react to all existing HTML events in the page.
- JavaScript can create new HTML events in the page.

W3C DOM standard is separated into 3 different parts:

- Core DOM
- XML DOM
- HTML DOM

HTML DOM is a standard object model and programming interface for HTML that defines:

- The HTML elements as objects
- The properties of all HTML elements
- The methods to access all HTML elements
- The events for all HTML elements

document object is the root of a web page.

Methods and properties:

- `document.getElementById(id)`
- `document.getElementsByTagName(tag)`
- `document.getElementsByClassName(class)`
- `element.style.property = new style`
- `document.createElement(element)`
- `document.removeChild(element)`
- `document.appendChild(element)`
- `document.replaceChild(element)`
- `document.write(text)`
- `document.getElementById(id).onclick = function(){code}`: adding ebent handler.
- `document.anchors`: `<a>` elements.
- `document.baseURI`: HTML DOM Level 3.
- `document.body`
- `document.cookie`
- `document.domain`
- `document.forms`
- `document.images`
- `document.links`: also include `<area>`.
- `document.referrer`: The linking document.
- `document.scripts`: Level 3.
- `document.title`
- `document.URL`

查找元素：

- 通过id：`var x=document.getElementById("intro");` 失败返回Null。
- 通过标签名：`var x=document.getElementById("main"); var y=x.getElementsByTagName("p");` 则找到`<div id=main>`中所有`<p>`元素。

改变:

- 改变内容：`element.innerHTML`指的是标签开始到标签结束之间的内容。
- 改变属性：如`element.src= "new.gif";`
- 改变样式：`element.style.属性名`，如`elment.style.visibility='hidden'/ 'visible';`可显示或隐藏元素。

Use CSS selector:

```javascript
var x = document.querySelectorAll("p.intro");
```

Finding HTML Elements by HTML Object Collections:

```html
<form id="frm1" action="/action_page.php">
  First name: <input type="text" name="fname" value="Donald"><br>
  Last name: <input type="text" name="lname" value="Duck"><br><br>
  <input type="submit" value="Submit">
</form>

<button onclick="myFunction()">Try it</button>

<p id="demo"></p>

<script>
function myFunction() {
    var x = document.forms["frm1"];
    var text = "";
    var i;
    for (i = 0; i < x.length ;i++) {
        text += x.elements[i].value + "<br>";
    }
    document.getElementById("demo").innerHTML = text;
}
</script>
```

HTML objects:

- document.anchors
- document.body
- document.documentElement
- document.embeds
- document.forms
- document.head
- document.images
- document.links
- document.scripts
- document.title

Don't call `document.write()` after the page is loaded. For example in the console.

Change value of attributes: `document.getElementById(id).attribute = "new value"`

Change CSS: `document.getElementById(id).style.property = "new style"`

[All the styles](https://www.w3schools.com/jsref/dom_obj_style.asp)

Handle events:

```javascript
<input type="button" value="Hide text" onclick="document.getElementById('p1').style.visibility='hidden'">
<input type="button" value="Show text" onclick="document.getElementById('p1').style.visibility='visible'">
```

HTML事件：

- 点击鼠标：`onclick`。
- 载入：`onload` 和 `onunload` 事件会在用户进入或离开页面时被触发。用于检测浏览器版本和处理`cookie`。`<body onload="checkCookies()">`是否启用cookies.
- 字段验证：`onchange`，字段改变时调用。`<input type="text" id="fname" onchange="upperCase()">`。
- 鼠标事件：`onmouseover`，`onmouseout`，`onmousedown`，`onmouseup`。
- 输入字段获得焦点：`onfocus`。

在属性中添加事件不用`<script>`标签。

```html
<button type="button" onclick="document.getElementById('id1').style.color='red'">点击这里！</button>
```

也可在`<script>function 函数名(){}</script>`，然后`onclick= "函数名()"`。

还可通过分配属性分配事件：`<script>document.getElementById("myBtn").onclick=function(){displayDate()};</script>` 注意此时不是用引号括起函数名。在HTML中直接执行一段JavaScript函数的方式。

用this表明当前元素，`this.innerHTML和this.attribute`。

```html
<div onmouseover="mOver(this)" onmouseout="mOut(this)" style="background-color:green;width:120px;height:20px;padding:40px;color:#ffffff;">把鼠标移到上面</div>
<script>
function mOver(obj)
{
  obj.innerHTML="谢谢"
}
function mOut(obj)
{
  obj.innerHTML="把鼠标移到上面"
}
</script>
```

添加元素：首先创建节点，在朝里面添加内容。

```html
<div id="div1"></div>
<script>
var para=document.createElement("p");
var node=document.createTextNode("这是新段落。"); //innerHTML内的文字部分。
para.appendChild(node);
var element=document.getElementById("div1");
element.appendChild(para);
</script>
```

删除元素：找到父元素并删除。可以通过属性`parentNode`找到。`child.parentNode.removeChild(child);`

```html
<div id="div1"><p id="p1">这是一个段落。</p></div>
<script>
var parent=document.getElementById("div1");
var child=document.getElementById("p1");
parent.removeChild(child);
</script>
```

Create animate: See <https://www.w3schools.com/js/tryit.asp?filename=tryjs_dom_animate_3>

Use a timer: by calling `timer = setInterval(frame, 5)` and `clearInterval(timer)`.

Events:

- `onload`: can be used to check browser, cookies.
- `onchange`: validate input.

[All events](https://www.w3schools.com/jsref/dom_obj_event.asp)

addEventListener:

- `document.getElementById("myBtn").addEventListener("click", displayDate);`
- can add multiple, same events without overwriting existing event handlers.
- can add event listeners to any DOM object not only HTML elements. i.e the window object.

Passing parameters:

```javascript
var p1 = 5;
var p2 = 7;

document.getElementById("myBtn").addEventListener("click", function() {
    myFunction(p1, p2);
});

function myFunction(a, b) {
    var result = a * b;
    document.getElementById("demo").innerHTML = result;
}
```

event propagation in the HTML DOM: when an element is inside another element and both have an event occured

- bubbling: inner most element's event handled first.
- capturing: vice versa.

DOM Nodes:

- document node.
- element node.
- text node.
- attrbute node (deprecated).
- comment node.

Note relations:

- root.
- 1 parent, n children.
- siblings: has orders.

Text node is also a child.

For

```html
<title id="demo">DOM Tutorial</title>
```

They are same:

```javascript
var myTitle = document.getElementById("demo").innerHTML;
var myTitle = document.getElementById("demo").firstChild.nodeValue;
var myTitle = document.getElementById("demo").childNodes[0].nodeValue;
```

Root node access:

- `document.body`
- `document.documentElement`: even `<head/>` is included.

NodeName property:

- nodeName is read-only.
- nodeName of an element node is the same as the tag name.
- nodeName of an attribute node is the attribute name.
- nodeName of a text node is always `#text`.
- nodeName of the document node is always `#document`.

NodeValue property:

- nodeValue for element nodes is undefined.
- nodeValue for text nodes is the text itself.
- nodeValue for attribute nodes is the attribute value.

NodeType property:

- 1: `ELEMENT_NODE`
- 2: `ATTRIBUTE_NODE` (deprecated)
- 3: `TEXT_NODE`
- 8: `COMMENT_NODE`
- 9: `DOCUMENT_NODE`
- 10: `DOCUMENT_TYPE_NODE`

Create a node:

```html
<div id="div1">
  <p id="p1">something</p>
</div>
```

```javascript
var para = document.createElement("p");
var node = document.createTextNode("This is new.");
para.appendChild(node);

var element = document.getElementById("div1");
var child = document.getElementById("p1");
element.insertBefore(para, child);
```

Remove: `removeChild`. DOM needs to know both the element you want to remove, and its parent.

```javascript
var child = document.getElementById("p1");
child.parentNode.removeChild(child);
```

Replace: `replaceChild`.

`getElementsByTagName()` method returns an `HTMLCollection` object:

- It can be access using index.
- It has `length` property.
- It is not an array so array APIs doesn't work.

# HERE https://www.w3schools.com/js/js_htmldom_nodelist.asp

### JS Window
[src](http://www.w3school.com.cn/js/js_window.asp)

Browser Object Model: 和浏览器的交互。

Window对象：浏览器窗口。
- 所有全局对象，函数和变量均是window的成员。
- 窗口尺寸：`window.innerHeight`，`window.innerWidth`。考虑到老版本的浏览器：`var w=window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;`
- 方法：打开新窗口：`window.open()`。
- 关闭当前窗口：`window.close()`。
- 移动当前窗口：`window.moveTo()`。
- 调整当前窗口的尺寸：`window.resizeTo()`。

屏幕对象：`window.screen`，可不使用window 这个前缀。
- 可用的屏幕宽度：`screen.availWidth`。以像素计，减去了界面特性如任务栏。
- 可用的屏幕高度：`screen.availHeight`。

页面地址对象：`window.location` 对象用于获得当前页面的地址 (URL)，并把浏览器重定向到新的页面。可省window。
- 主机的域名：`location.hostname`。
- 当前页面的路径和文件名：`location.pathname`。
- 端口(80或443): `location.port`。
- web协议（`http://`或`https://`）：`location.protocol`。
- 当前页面的URL：`location.href`。
- 加载新的文档：`window.location.assign("http://www.w3school.com.cn")`;

页面跳转：[src](http://www.111cn.net/wy/js-ajax/48824.htm)
```
onclick="window.location.href='www.aaa.com'"
```

历史对象：`window.history`对象可省前缀，有一定限制。
- 后退到前一个页面：`history.back();`
- 向前到后一个页面：`history.forward();`

[用navigator检测浏览器](http://www.w3school.com.cn/js/js_window_navigator.asp)


三种消息框：
- 警告框：弹出窗口显示字符串：`alert("string”+’\n’+”string");`
- 确认框：OK或Cancel按钮。`confirm(‘’string’);` 返回true或false。
- 提示框：输入值并确认或返回。`prompt("string","default");` 返回输入值或null。

计时事件：
- 延时发生事件：`var t=setTimeout("函数名或执行语句",毫秒); `
```
var c=0
var t
function timedCount(){
		document.getElementById('txt').value=c
		c=c+1
		t=setTimeout("timedCount()",1000)
}
```

- 清除计时器：`clearTimeout(setTimeout_variable)`。

简单时钟：
```
<html>
<head>
<script type="text/javascript">
function startTime(){
  var today=new Date()
  var h=today.getHours()
  var m=today.getMinutes()
  var s=today.getSeconds()
  
  // add a zero in front of numbers<10
  m=checkTime(m)
  s=checkTime(s)
  
  document.getElementById('txt').innerHTML=h+":"+m+":"+s
  t=setTimeout('startTime()',500)
}

function checkTime(i){
  if (i<10) 
    {i="0" + i}
    return i
}
</script>
</head>
<body onload="startTime()">
<div id="txt"></div>
</body>
</html>
```

cookie对象：`document.cookie`可储存各种信息，甚至是密码。
```
<html>
<head>
<script>
function getCookie(c_name){ // Find element
  if (document.cookie.length>0){
    c_start=document.cookie.indexOf(c_name + "=");
    if (c_start!=-1){
      c_start=c_start + c_name.length+1;
      c_end=document.cookie.indexOf(";",c_start); // The char after an index
      if (c_end==-1) c_end=document.cookie.length; 
      return decodeURI(document.cookie.substring(c_start,c_end));
    } 
  }
  return "";
}

function setCookie(c_name,value,expiredays){ // expiredays can be null
  var exdate=new Date();
  exdate.setDate(exdate.getDate()+expiredays);
  document.cookie=c_name+ "=" +encodeURI(value)+((expiredays==null) ? "" : ";expires="+exdate.toGMTString());
}

function checkCookie(){
  username=getCookie('username');
  if (username!=null && username!=""){
    alert('Welcome again '+username+'!');
  }else{
    username=prompt('Please enter your name:',"");
    if (username!=null && username!=""){
      setCookie('username',username,365); 
    }
  }
}
</script>
</head>
<body onLoad="checkCookie()">
</body>
</html>
```


### JavaScript 库
[src](http://www.w3school.com.cn/js/js_library_jquery.asp)

JavaScript框架：库，高级应用程序设计。特别针对浏览器差异处理。常用的有jQuery、Prototype、MooTools。

CDN (Content Delivery Network)把框架库存放在一个通用的位置供每个网页分享就变得很有意义了。

### JQuery
[src](http://www.w3school.com.cn/js/js_library_jquery.asp)

使用前需要引用jQuery 库。
```
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js">
</script>
<script>
mycode
...
</script>
```

测试 JavaScript 框架库
- 主要的jQuery 函数是`$()` 函数(jQuery 函数)。
- 如果您向该函数传递DOM 对象，它会返回jQuery 对象，带有向其添加的jQuery 功能。
- 可以通过CSS 选择器来选取元素
- jQuery 函数返回jQuery 对象。

DOM 对象$(document) 传递到jQuery。返回的jQuery对象有ready 方法。
```
function myFunction() {
  $("#h01").html("Hello jQuery"); // element id "h01", innerHTML set to "Hello jQuery"
}

$(document).ready(myFunction); // onload
```

See JQuery.md

### AJAX
[src](http://www.w3school.com.cn/php/php_ajax_xmlhttprequest.asp)

AJAX(Asynchronous JavaScript And XML)：运用JavaScript，XML，HTML，CSS，通过在幕后向服务器发送 HTTP 请求并交换数据，而不是每当用户作出改变时重载整个 web 页面，AJAX 技术可以使网页更迅速地响应。

不同的浏览器使用不同的方法来创建 XMLHttpRequest 对象：Internet Explorer 使用`ActiveXObject`。其他浏览器使用名为`XMLHttpRequest` 的`JavaScript` 内建对象。
```
function GetXmlHttpObject(){
 var xmlHttp=null;
 try{
  // Firefox, Opera 8.0+, Safari
  xmlHttp=new XMLHttpRequest();
 }catch (e){
  // Internet Explorer
  try{
   xmlHttp=new ActiveXObject("Msxml2.XMLHTTP"); //IE 6
  }catch (e){
   xmlHttp=new ActiveXObject("Microsoft.XMLHTTP"); // IE5.5
  }
 }
 return xmlHttp;
}
```

用xmlHttp对象来后台交互数据。
- `open(method,url,async)`: 规定请求的类型、URL 以及是否异步处理请求。
  - method: 请求的类型；GET 或 POST
  - url: 文件在服务器上的位置
  - async: true（异步）或 false（同步）
- `send(string)`: 将请求发送到服务器。
  - string: 仅用于 POST 请求

GET 更简单也更快，并且在大部分情况下都能用。GET的信息包含在url中。
```
xmlHttp.open("GET", "demo_get.php?q信息&sid="+Math.random(),true); xmlHttp.send();
```

无法使用缓存文件（更新服务器上的文件或数据库，或向服务器发送大量数据（POST 没有数据量限制），或发送包含未知字符的用户输入时，POST 比 GET 更稳定也更可靠。

POST需要为HTML请求添加头。
```
xmlhttp.open("POST","demo_post.php",true);
xmlhttp.setRequestHeader("Content-type","application/x-www-form-urlencoded");
xmlhttp.send("fname=Bill&lname=Gates");
```

`open()`的最后一个参量表示是否异步。XMLHttpRequest 对象如果要用于AJAX 的话，async 参数必须设置为 true。

异步指在等待服务器响应时执行其他脚本，并当响应就绪后对响应进行处理，即`onreadystatechange` 事件中的就绪状态时执行的函数。

响应数据：`responseText`属性为获得字符串形式的响应数据；`responseXML`为获得XML 形式的响应数据。

onreadystatechange事件：
- `onreadystatechange`: 存储函数（或函数名），每当 readyState 属性改变时，就会调用该函数。
- `readyState`: 存有 XMLHttpRequest 的状态。从 0 到 4 发生变化。
  - 0: 请求未初始化
  - 1: 服务器连接已建立
  - 2: 请求已接收
  - 3: 请求处理中
  - 4: 请求已完成，且响应已就绪

status
- 200: "OK"
- 404: 未找到页面

```
xmlhttp.onreadystatechange=function(){
  if (xmlhttp.readyState==4 && xmlhttp.status==200){
    document.getElementById("myDiv").innerHTML=xmlhttp.responseText;
  }
}
```

下例是一段根据form中`<input type="text" id="txt1" onkeyup="showHint(this.value)">`内容显示数据的JavaScript脚本。
```
var xmlHttp;

function showHint(str){
  if (str.length==0){ 
    document.getElementById("txtHint").innerHTML="";
    return;
  }
  xmlHttp=GetXmlHttpObject();
  if (xmlHttp==null){
    alert ("Browser does not support HTTP Request");
    return;
  } 
  var url="gethint.php"; 
  url=url+"?q="+str+"&sid="+Math.random(); // 随机数用于防止浏览器使用缓存
  xmlHttp.onreadystatechange=stateChanged(); 
  xmlHttp.open("GET", url, true); // 发送的目的地
  xmlHttp.send(null); // 发送的内容，因为get的内容在url中。
}

function stateChanged(){ 
  if (xmlHttp.readyState==4 || xmlHttp.readyState=="complete"){
    document.getElementById("txtHint").innerHTML=xmlHttp.responseText; //完成，输出信息到<p>Suggestions: <span id="txtHint"></span></p>
  } 
}
```

用gethint.php接收get的内容并作出回应。
```
<?php
$a[]="Anna"; // Suggestion
$q=$_GET["q"]; //get the q parameter from URL
//lookup all hints from array if length of q>0
if (strlen($q) > 0){
  $hint="";
  for($i=0; $i<count($a); $i++){
    if (strtolower($q)==strtolower(substr($a[$i],0,strlen($q)))){ // Find all the fit suggestion
      if ($hint==""){
        $hint=$a[$i];
      }else{
        $hint=$hint." , ".$a[$i];
      }
    }
  }
}
if ($hint == ""){
  $response="no suggestion";
}else{
  $response=$hint;
}
echo $response; //output the response
?>
```

# Node.js
https://www.nczonline.net/blog/2013/10/07/node-js-and-the-new-web-front-end/

