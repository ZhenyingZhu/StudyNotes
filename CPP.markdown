# C++ Primer 中文版
## Chapter 1
### 1.1
主函数
```
int main()
{
    return 0; // Means success
} // this is curly brace
```
函数必须指定4个元素：返回类型, 函数名, 形参表, 函数体。  

Linux下编译：
```
g++ hello.cc –o hello
./hello // Execute
echo $? // see the return value from main
```

### 1.2
Preprocessor directive: include尖括号中是头文件。标准库用`<>`括起来。自定义库用`""`。  

iostream
* istream：cin。输入值与存入的变量类型不符合时, 或读入`ctrl+D`时, 返回的值为假, 可用于while的中。  
* ostream：cout,  cerror, clog。  
* iostream库能所有处理内置类型的输出。  
```
#include <iosteam>
int main()
{
    std::cout << "Enter two numbers: " << std::endl; // std::endl is one of manipulators
    int v1, v2; 
    std::cin >> v1 >> v2; 
    std::cout << "Sum :" << v1 + v2 << std::endl; 
}
```

`<<`操作符：每次接受两个操作数, 左边为ostream对象, 右边为内容。该表达式执行完后, 返回`void*`的`ostream`对象。  
Manipulator操作符：`endl`, 换行并刷新缓冲区(buff)。  

调用前需有`std::`是使用命名空间(namespace)std内的函数或操作符避免定义变量时冲突。  
作用域(Scope)操作符：取namespace中的对象。  

对于出错的情况：
```
    std::cerr << "Error" << std::endl;
    return -1;
```

内置类型：如int。最好都赋初值。  

### 1.3
区块注释：
```
/*
 *
 */
```

### 1.4
控制结构:  
迭代while：括号内条件式返回非0时执行。  
```
int sum = 0, val;
while(std::cin >> val) 
    sum += val;
```

简化循环变量for：循环结束后循环变量释放, 不可再用。  
```
int sum=0;
for(int val = 1; val <= 10; ++val)
    sum += val;
```

条件执行if：    
```
if(条件)
    执行;
else
    执行;
```

### 1.5
类(class):  
自定义数据类型。istream也是。  
三要素：名字、定义域、可执行操作。  
保存在一个与类名相同且后缀为`.h`文件中。  
实例化：
```
Sales_item item; 
```
Sales_item是类, item是对象。

成员函数：`item.same_isbn(item2)`是函数。  
可以覆写操作符。  

点操作符.：左操作数是类的对象, 右操作数是成员。  
调用操作符()：扩住实参。  

### 1.6

### 小结
Argument: 实参; Parameter: 形参。Statement: 语句。  

Routine: 一系列操作组成。用以定义函数或数据类型。  
Statically typed: C是而smalltalk不是。  

## Chapter 2
### 2.1
算术类型(arithmetic type)：
* bool,  true, false。可以为任何算术类型。
* char(8 bit, 1 byte), wchar_t汉语或日语(16位)。每个字符的数值为literal constant。
* short(16位), int(16位), long(32位),  定义时前可加 unsigned。C++中-1给unsigned类型得4294967295(与编译器有关)。unsigned 类型用于数组下标。越界会被wrap around。
* float (32 bit) 6位有效数字, double (32 bit) 10位有效数字, long double (96 或128 bit)10位有效数字。double 的运算效率可能比float 还高。
Void type.  

内存机制: 本为序列存储, 用chunk 来处理, 32 bit为一个word。每个byte 有一个地址。

### 2.2
20等同于20 (decimal), 024 (octal), 0x14 (hexadecimal)。  
20UL得到long和unsigned类型。  
单精度浮点: 3.14159F = 3.14159E0f 0. = 0e0, 1E-3F = .001f  
wchar_t类型: L'a'。  
转义字符(Escape characters)：`\n`, `\t`, `\v` 纵向制表符, `\b`退格符？, `\r`, `\12` 回车符, `\f` 进纸符, `\a` 报警符, `\0` 空字符, `\40` 空格符。`\xxx` (八进制数) = (char)0xxx。  
字符串：`cout << "a" "b""c"`可以用空格、回车、tab连起来。在末尾自动添加空字符：'A' 是一个字符，而"A" 是'A', '\12' 两个字符。

escape `\`: 可以断开单词来换行。不允许之后有空格或注释。下一行的第一个字符, 不论是空格和tab都会包含, 所以不能正常缩进。  

nonportable: 利用未定义行为编程, 如将char string 和long string 相连 `"apple" L"banana"`。  

### 2.3
左值(lvalue): 即变量，可出现在赋值语句左或者右。右值(rvalue): 即常量，只能在右。  

对象: 内存中具有类型的区域。  

C语言大小写敏感。且关键词和替代名不能作为变量名(identifier)。变量名必须以`_`或字母开头。  

关键词表：

|  1  |  2  |  3  |  4  |  5  |  6  |  7  |  8  |  9  |  0  |
|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
|asm|do|if|return|try|auto|double|inline|short|typedef| 
|bool|dynamic_cast|int|signed|typeid|break|else|long|sizeof|typename|
|case|enum|mutable|static|union|catch|explicit|namespace|staitc_cast|unsigned|
|char|export|new|struct|using|class|extern|operator|switch|virtual|
|const|false|private|template|void|const_cast|float|protected|this|volatile|
|continue|for|public|throw|wchar_t|default|friend|register|true|while|
|delete|goto|reinterpret_cast|


替代名：替代操作符

| 1 | 2 | 3 | 4 | 5 | 6 |
|:-:|:-:|:-:|:-:|:-:|:-:|
|and|bitand|compl|not_eq|or_eq|xor_eq|
|and_eq|bitor|not|or|xor|


变量名只能由数字、字母、下划线组成。一般为小写。多个单词由下划线分开，或第二个单词以后的每个单词首字母大写。  

初始化不是赋值。赋值是将对象当前值用新值代替。直接赋值(Direct-initialization)：`int ival (1024);` 对class object 来说, 比copy-initialization 效率高且灵活。   
类对象: 由constructor 初始化。不定义任何构造函数则使用default constructor。有默认构造函数的类可以在初始化时不提供初始值。不然无法初始化。  
```
#include <string>
std::string titleA = "Hello World"; 
std::string titleB("Hello World"); 
std::string all_nines(3, '9'); // 999
```

内置类型，函数体外的初始化自动为0。函数体内不自动初始化。
```
#include <iostream>
#include <string>
int global_var; 

int main()
{
    int local_var; 
    std::string local_str; 
    std::cout<< global_var << std::endl; // it is 0. 
    std::cout << local_var << std::endl; // ？
    std::cout << local_str << std::endl; // ""
}
```
#### 2.3.5
Definition: 分配存储空间，一个变量只能有一个。  
Declaration: 定义也是一种声明。在多个文件中, 一个文件含有变量的定义, 其他的需要声明。
只声明不定义变量，或在新的文件中声明已定义过的变量： 
```
extern int i; 
```
#### 2.3.6
Scope: global scope(全局作用域)中定义的变量可用于local scope(局部作用域)和statement scope(语句作用域)。
- 局部定义可hide(屏蔽)全局定义的变量。
- 对象定义在首次使用的地方。之后在同一个作用域内只能声明不能再定义。
- class scope
- namespace scope
#### 2.3.7

### 2.4
Magic number.  
定义常数：
```
const int bufsize = 512; 
extern const int globalConst = val; // then can be declared in other file
```

### 2.5
Reference(引用): 就是对象，主要用作形参。 
- compound type: 用其他类型定义的。  
- 变量名前加&, 将两个变量的地址联系起来。 
- 引用的变量值不能被赋值, 即&refVal始终指向ival的地址。
```
int ival = 1024; 
int &refVal = ival; 
refVal += 2; // ival = 1026
```
Nonconst reference.  

### 2.6
定义类型的同义词： 
```
typedef double wages; 
wages salary; // salary is a double value
```

### 2.7
Enumeration(枚举): 
- 枚举的第一个enumerator(成员)默认为0, 之后为1, 2。赋值过的成员之后的默认递增。
- Constant expression.  
- enum定义了一种类型, 已定义的不能再定义，只能用另一个同类型赋值。
```
enum Points {point2d = 2, point2w, point3d=3, point3w}; 
```

### 2.8
定义类：定义interface(接口)和implementation(实现)。  
- 接口为该类可执行的member(操作)。有function member 和data member。
- 实现包括包括该类需要的数据和内部函数。
- 类可以有多个private或public的access label(访问标号)。决定代码是否可以访问。public中的成员都可以访问。private中的只能执行规定的操作, 不能修改数据。
- class定义的类中成员默认都是private,  struct定义的默认都是public。

定义类： 
```
class Sales_item {
public: 
    // operations. 
private: 
    std::string isbn; // use constructor to init
    unsigned units_sold;
    double revenue;
}; // Don't miss this one
```
或
```
struct Sales_item {
}
```

### 2.9
Separate compilation.  
#### 2.9.1
头文件：存放程序中名字的使用和声明。
- 包含类的定义， extern变量的声明（不要定义）, 函数的声明。  
- 可以定义类，const 对象和inline 函数。 

编译链接源文件：
```
CC -c main.cc Sale_item.cc
# create executable file
CC -c main.cc Sale_item.cc -o main
```
Or separate compilation
```
CC -c main.cc
CC -c Sale_item.cc
CC main.o Sale_item.o
# create executable file
CC main.o Sale_item.o -o main
```
#### 2.9.2
Preprocessor: 编译时`#include` 处用头文件内容替代。
- 可以用文本格式或编译器认识的格式（系统的头文件）保存。 
- 头文件可嵌套。
- 同一源文件中多次包含的同一头文件要避免重复定义类和对象。 

用header guard 预防重新定义已包含的头文件：
```
#ifndef SALESITEM_H
#define SALESITEM_H 
//Define class. 
#endif
```
其中用到预处理变量SALESITEM_H 做状态检测。 

`<>`的头文件为标准头文件，编译器在预定义位置查找。  
`""`为自定义头文件，从源文件所在路径处查找。  

### 小结

## Chapter 3
Abstract data type： String 和vector 是Iterator 的companion type。 bitset 用集合处理值。    
### 3.1
using声明(命名空间：：名字)：`using std::cin; `  
- 头文件中不用using。  

### 3.2
标准库类型(非基本类型)：需包含
```
#include <string>; 
using std::string;
```
#### 3.2.1
string构造函数：
```
string s1; // empty
string s2 (s1); 
string s1 ("value"); 
string s4(n,’c’); 
```
字符串字面值`"abc"`和string是不同的类型。  
#### 3.2.2
`cin`输入string：
- 返回的是布尔值。 文件尾或无效字符处返回非。 
- 从非空字符开始, 到遇到空字符（space, enter, tab）结束。  
- input： "  Hello World   "，则s1 为"Hello"。
```
cin >> s1 >> s2; 
```
string IO 操作读入一整行： 输入流和string对象
```
while(getline(cin, line)) 
    cout << line << endl; 
```
与`cin`不同，不忽略enter。如果一行只有enter，则返回空string。  
#### 3.2.3
string API：
```
s.empty(); 
s.size(); // \n count as one. return string::size_type 
s[n]; 
s1 + s2; // concatenation
s1 = s2; // high cost
v1 == v2; 
v1 != v2; 
v1 <= v2; 
v1 > v2; 
```
`st.size()`： 
- st中包含空字符的字符数
- 返回string::size_type类型，认为是无符号长整型，不能赋给int型，但是可以用int赋值。
- 使用companion type 来获得machine-independent。 

string关系操作符：  
- 不考虑长度，先依次比较每个字符大小。
- 大写比小于小写。
- 如之前都一样, 短字符串小于长字符串。

字符串字面值无法concatenation，只有string类型才有： 
```
string s1 = "Hello" + "World"; // error
```

下标操作符`[]`读入size_type类型数做index读入单个字符。
- 下标从0开始。下标可以是任何整型值。
- 上下界为0到`str.size()  1`。
```
    for (string::size_type ix = 0; ix != s1.size(); ++ix) {
        cout << s1[ix] << endl; 
    }
``` 
#### 3.2.4
`cctype`头文件中定义处理char值的函数：
- 测试字符串中单个字符的函数, 返回一个int值, 失败为0, 其他为非0值。
- `tolower(c)`, `toupper(c)`返回的是字符。
- `isalpha(c)`, `isalnum(c)`, `isdigit(c)`
- `isxdigit(c)`是否为16进制数。
- `iscntrl(c)`是否为控制字符。 
- `isgraph(c)`不是空格，但可打印。
- `isprint(c)`, `ispunct(c)`, `isspace(c)`
- `islower(c)`, `isupper(c)`

C++标准库中包括C标准库。
- `cctype`定义于`ctype.h`中。
- 要导入C标准库，`#include <c[name]>;`
- 虽然也可以`#include "ctype.h";` ，不建议使用。

### 3.3
vector：container(容器), 可包含同一类型其他对象的集合。
- `vector`头文件中。
- 是class template。可用于不同的数据类型。
- 声明：
```
vector<int> ivec; 
```
#### 3.3.1
构造函数定义和初始化：
```
vector<T> v1; 
vector<T> v2(v1); 
vector<T> v3(n, i); 
vector<T> v4(n) // init with n default T object
```
- 建议初始化空容器，再动态添加元素到vector。
- 为高效添加元素vector 不预先分配连续内存。
- value initialization。
#### 3.3.2
vector的操作：
- `v.empty()`
- `v.size()` 返回`vector<T>::size_type`类型。
- `v.push_back(t)` 添加元素t 到v 的末尾。
- `v[n]` 无法在下标n处添加元素，或尝试获取，不然会buffer overflow
- `v1 = v2`
- `v1 == v2`
- `!=`, `<`， `>=`

C++优先选用`size_type != size`来做循环的判断条件。  
`size()`是inline 函数，执行代价小，编译器在此处扩展代码。  

### 3.4
iterator(迭代器)：
- 标准库为每种容器各定义一迭代器类型，但不是每种容器都支持下标操作。
- 定义：`vector<int>::iterator iter; `
- 标准库容器类型都定义了一个iterator成员。
- `begin()`，`end()`函数指向最后元素的下一个位置。`end()`为off-the-end iterator，起sentinel作用。
```
vector<int>::iterator iter = ivec.end(); 
```
- 访问迭代器指向元素；用解引用操作符`*iter = 0;`。等同于`ivec[iter] = 0;`。
- `++iter;`移动。
- `iter1 == iter2;`来比较迭代器位置。
- `vector<string>::const_iterator iter;` 则`*iter`可得string对象的const引用，不能赋值。
- 迭代器间距离：`iter1 - iter2`, 由`difference_type`储存, 是signed，必须指向同一个容器。
- iterator arithmetic：`iter += n;`n需为vector的`size_type`或`difference_type`类型。
- 中间元素：`vector<int>::iterator mid = vi.begin() + vi.size()/2;`
- `push_back`后迭代器失效。
```
int main()
{
    vector<int> ivec(3); 
    int i = 1; 
    vector<int>::iterator end = ivec.end(); 
    for (vector<int>::iterator iter = ivec.begin(); iter != end; ++iter) {
    
        *iter = i++; 
    }

    for (vector<int>::const_iterator iter = ivec.begin(); iter != end; ++iter) {
        cout << *iter << endl; 
    }
}
```

### 3.5
bitset：位操作。头文件`bitset`中。
#### 3.5.1
定义与初始化：
- 也是类模板，但是用长度区别。
- 长度必须为整型字面值或const对象。
- 32位bitset 的low-order bit 从0 开始，在最右，high-order bit 为31。
```
bitset<32> bitvec; 

bitset<64> bitvec2(0xffff); // a copy of unsigned int, fill from 0 to high-order bit
bitset<16> bitvec3(0xffff); // abandon bits from 16 to 31

string strval("1100"); 
bitset<32> bitvec4(strval); // strval[3] -> bitvec4[0]

bitset<32> bitval5(str, pos, n); // str[pos] to str[pos + n - 1]
bitset<32> bitval6(str, pos); // str[pos] to the end
``` 
#### 3.5.2
bitset 对象的操作：
- `bool is_set = bitvec.any(); ` 测试是否有1，返回1为true。`bitvec.none()`相反。
- `b.count()` 置1的个数。返回`size_t`类型,在`cstddef`头文件中。
- `b.size()`
- `b[pos]`
- `b.test(pos)` 是否为1。结果等同于`b[pos]`。
- `b.set()` 全置为1。`b.set(pos)`，`b.reset()`，`b.reset(pos)`，`b.flip()`，`b.flip(pos)`。
- `unsigned long number = b.to_ulong();` 将二进制数返回为长整型。需`b`的长度小于等于long，不然throw exception。
- `os << b` 输出到OS流，如`cin`。
- 支持各种位操作符。

### 小结

## Chpater 4
Array, pointer 是低级复合类型。  
Array 长度固定。没有size 操作。  
只有在追求速度是才在类实现内部使用数组和指针。  
### 4.1
Array: 
- 复合数据类型：类型名，标识符，dimension (维数)。 
- 没有所有元素都是引用的数组<b>?</b>
#### 4.1.1
维数只能是常量或编译时已知的const 对象。  
```
const unsigned buf_size = 512; 
char input_buffer[buf_size]; 

const unsigned sz = get_size(); // cannot used as dimension
```

显式初始化：
```
int ia[3] = {1, 2, 3}; 
int ia[] = {1, 2, 3}; 
```

不初始化，则
- 函数体内，元素无初始化。
- 函数体外，初始化为0或空。 
- 类类型则调用默认构造函数。 
- 内置类型的局部数组<b>?</b>的元素没有初始化。

字符初始化：
```
char ca[] = {'C', '+', '+', '\0'}; 
char ca[] = "C++"; // add null terminator 
```

标准规定数组无法用另一个数组赋值或初始化。  
#### 4.1.2
数组下标类型是`size_t`。  
```
const size_t array_size = 10; 
int ia[array_size]; // if in int main(), then not init

for (size_t ix = 0; ix != array_size; ++ix) {
    ia[ix] = ix; 
}
```

小心buffer overflow。  

### 4.2
Pointer: 
- dereference operator`*`
- increment operator`++`
- address-of`&`
#### 4.2.1
指针概念：
- 指针是对所指对象的间接访问。  
- 保存另一个对象的地址。  
- 可用于vector, string, array 的下标操作和解引用操作得到的左值。  
```
string s("hello World"); 
string *sp = &s; 
```

容易犯bookkeeping 和语法规则的错误。  
#### 4.2.2
定义：  
```
double dp, *dp2; // dp2 is a pointer
string* ps1, ps2; // legal but can be misleading. ps2 is string object
```

取值：
- 0值常量或`cstdlib`库中的预处理器变量`NULL`，表示无指向。`int *pi = NULL;`
- 特定对象地址。`int *pi2 = &ival; `
- 某对象的下一个对象。
- 同类型的有效地址。`pi = pi2; ` 或`int *pi = pi2;`
- 未初始化的指针无效。但编译器无法检测。
- 一定要初始化指针。最好在创建对象后再定义，不然初始化为0。

预处理变量不在命名空间`std`中。  

`void*`指针可存放任意类型。但是可执行的操作有限：  
- 与另一个指针比较。
- 向函数传递或返回指针。
- 给另一个`void*`指针赋值。
#### 4.2.3
可以通过解引用返回对象左值：
```
*sp = "Hello World"; 
```

指针和引用的区别：
- 引用总是指向某对象。
- 给引用赋值修改对象，给指针赋值修改地址。

指向指针的指针：
```
int val = 3; 
int *pi1 = &val; 
int **ppi = &pi1; 
int *pi2 = *ppi;  // dereference, pointer to pi1
cout << **ppi << endl; // 3
```
#### 4.2.4
数组名也是个指针，指向第一个元素：
```
int ia[] = {1, 2, 3}; 
int *ip = ia; 
int *ip2 = &ia[0]; // same as above
```
Pointer arithmetic: 
```
int *ip2 = ip + 4; // point to ia[4]
ptrdiff_t n = ip2 - ip1; // include in lib cstddef
```

加法操作precedence (优先级)低于解引用。  
```
int last = *(ia + 4); // Array name is a pointer
```

反过来，指针指向数组时也可用下标访问元素。  
```
int ia[] = {1, 2, 3, 4}; 
int *p = &ia[3]; 
cout << p[-2]; // 2
```

数组的哨兵指针：
```
const size_t arr_size = 5; 
int arr[arr_size] = {1, 2, 3, 4, 5}; 
int *p = arr; 
int *pe = p + arr_size;  // cannot dereference 
```

遍历：
```
const size_t arr_sz = 5; 
int ia[arr_sz] = {1, 2, 3, 4}; 

for (int *p = ia, *pe = ia + arr_sz; p != pe; ++p) { // init-statement
    cout << *p << " "; 
}
```

内置类型没有成员函数，数组也是内置类型。  
#### 4.2.5
指向const 对象：
- 指针必须有const 特性的。
- 但该指针不是const 的。不需定义时赋值。
- 可以给该指针重新赋值令其指向另一个对象。
- 但是不能通过该指针给解引用返回的左值赋值。
- 但是对所指对象（未必是const）的改变仍可执行。
```
const doule *cptr; 
double dval = 3.14; 
cptr = &dval; // but cannot change value
```

用于将形参定义为指向const 对象的指针以确保不改变对象。  
不含const 特性的指针不能指向const 对象。  

const 指针：不能改变所指地址。但是能否解引用修改对象取决于对象。  
```
int errNumb = 0; 
int *const curErr = &errNumb; 
if (*curErr) {
    errHandler(); 
    *curErr = 0; 
}
```

Const 限定符可放在类型前或后：
```
string const s1; 
const string s2; // same type as s1
```
Typedef 写const 类型定义：
```
string s; 
typedef string *pstring; 
const pstring sctr1 = &s; 
pstring const sctr2 = &s; // Same as sctr1
string *const sctr3 = &s; // same type
```

### 4.3
C-style character string（C 风格字符串）：
- 不建议使用。  
- 字符串字面值类型是const char 数组。  
```
char ca1[] = {'C', '+', '+'}; // not C style
char ca2[] = {'C', '+', '+', '\0'}; // C style
char ca3[] = "C++"; // C style
const char *cp1 = "C++"; // C style
char *cp2 = ca2; // C style
```

遍历：利用结尾的null。
```
const char *cp = "string"; 
while (*cp) {
    cout << *cp; 
    ++cp; 
}
```

C风格字符串标准库函数：
- `cstring`库中。
- 传入函数的指针必须指向字符数组第一个元素。
- `strlen(s)` 不包括null
- `strcmp(s1, s2)` 相等返回0，s1大于s2返回正。
- `strcat(s1, s2)` 需确保s1足够大。
- `strcpy(s1, s2)` 将s2复制给s1并返回s1。
- `strncat(s1, s2, n)` 将s2前n个字符复制给s1并返回s1。
使用cat和cpy前需计算s1大小。
```
char s1[10]; // cannot shorter than s2, include null
char *s2 = "tail"; 
char *res = strcpy(s1, s2); 
```

避免数组溢出，使用标准库类型string。
```
#include <string>
using std::string; 
char *cp = "string"; 
string s = cp; 
s += " "; 
cout << s; 
```
# 4.3.1
动态分配的数组：
- 长度仍固定。
- 需显式释放。  
- 存储在heap(堆)中。
- 动态分配的数组是对象，而编译时就确定的数组是变量。

堆：也称free store(自由存储区)。存放动态分配的对象。  
- C语言用`malloc`和`free`来分配该空间。
- C++用`new`和`delete`。

定义：
```
int *pia = new int[10]; // array does not have name
int *pia = new int[10]();  // init with 10 0s. 
```
- 内置类型的元素不会初始化。除非用圆括号。
- 对象用默认构造函数初始化。 
- 不能用初始化列表来为元素提供不同初值。 
- const 的动态数组对象必须初始化。然而没有什么用。

动态分配0长度的数组：
```
size_t n = get_size(); 
int *pia = new int[n]; 

for (int *p = pia; p != pia + n; ++p) {
    *p = 1; 
}
```
- 如果n 为0，仍可成功分配空间。
- 但是数组的指针并未指向任何元素，故不能解引用。
- 指针仍可进行比较操作。

释放内存：
```
delete [] pia; 
```
`[]`表明释放的是指针所指的数组空间，而非单个对象。  
- 编译器无法检测出指针所指是数组还是对象。
- 会导致memory leak(内存泄漏)。

用动态分配的数组选择C风格字符串：
```
int main()
{
    bool errorFound = true; 

    const char *noErr = "Success"; 
    const char *err189 = "Error: a function must "
        "specify a function return type!"; 

    const char *errorTxt; 
    if(errorFound) 
        errorTxt = noErr; 
    else 
        errorTxt = err189; 

    int dimension = strlen(errorTxt) + 1; // null char
    char *errMsg = new char[dimension]; 
    strncpy(errMsg, errorTxt, dimension); 
    
    while (*errMsg) { // same as cout << errMsg 
        cout << *errMsg; // and cout << *(errMsg++); 
        ++errMsg; 
    }
    cout << endl; 

    return 0; 
}
```
#### 4.3.2
C风格字符串与字符串字面值是相同的数据类型。
- 可以用来赋值：`string st1 = sp; `
- 可做string类型加法的其中一个操作数。
- 不能用string 对象初始化字符指针。
- 使用`c_str()`函数：
```
string s1("Hello World"); 
const char *cp = s1.c_str(); 
s1 = "Bye World"; 
cout << cp << endl; // Bye World
```

使用数组初始化vector对象：
```
const size_t arr_size = 6; 
int int_arr[arr_size] = {1, 2, 3, 4, 5, 6}; 
vector<int> ivec(int_arr, int_arr + arr_size); // begin and end
```

### 4.4
多维数组就是元素为数组的一维数组。  
- 第一维为row(行)，第二维为column(列)。 
- 初始化：如不用花括号，先行后列。
```
int ia[2][3] = {
    {0, 1, 2}, {3, 4, 0}
}
int ia[2][3] = {
    0, 1, 2, 3, 4 // same, the last is init to 0
}
```
- 下标引用：
```
const size_t rowSize = 3; 
const size_t colSize = 4; 
int ia[rowSize][colSize]; 
for (size_t i = 0; i != rowSize; ++i) {
    for (size_t j = 0; j != colSize; ++j) {
        ia[i][j] = i * colSize + j; 
    }
}
```
- 声明指向多维数组的指针：
```
int ia[3][4]; 
int (*ip)[4] = ia; // ip is a pointer to int[4] type
ip = &ia[2]; 
```
- `&ia`是个指向int[3][4]类型的指针，`ia`是个指向int[4]类型的指针，`ia[0]`是个指向int类型的指针，`ia[0][0]`是个int。
-用`typedef`简化：
```
int ia[2][3] = {0, 1, 2, 3, 4}; 
typedef int int_array[3]; 
for (int_array *p = ia; p != ia + 2; ++p) {
    for (int *q = *p; q != *p + 3; ++q) 
        cout << *q << " "; 
    cout << endl; 
}
cout << endl; 
```

### 小结
compiler extension: 不同编译器对语言添加的特性。难以移植。  
dynamically allocated: 在显式释放前一直存在。  

## Chapter 5
Operator(操作符)：
- 内置或复合类型的操作符含义已定义。
- 可重载。
- 可与数个operand 组成expression，产生result。
- result 一般是右值。 
- 操作符执行什么操作由操作数类型决定。
- unary(一元)和binary(二元) operator 操作符。
- 有一个ternary operator（三元）。
- 有些symbol 既可是一元也可是二元操作符。
- 二元操作符通常需要操作数为相同类型或可转换为相同类型。 

操作符的特性：
- precedence（优先级）。
- associativity（结合性）。
- order of evalution（求值顺序）：如左操作数先于右操作数执行。

### 5.1
操作符表：


| Precedence | Operator | Description | Associativity |
|:---:|:---:|:---:|:---:|
|1|::|Scope resolution (C++ only)|None
|2|`++`|Suffix increment|Left-to-right
||`--`|Suffix decrement
||()|Function call
||[]|Array subscripting
||`.`|Element selection by reference
||`->`|Element selection through pointer
||typeid()|Run-time type information (C++ only)
||const_cast|Type cast (C++ only)
||dynamic_cast|Type cast (C++ only)
||reinterpret_cast|Type cast (C++ only)
||static_cast|Type cast (C++ only)
|3|`++`|Prefix increment|Right-to-left
||`--`|Prefix decrement
||`+`|Unary plus
||`-`|Unary minus
||`!`|Logical NOT
||`~`|Bitwise NOT (One's Complement)
||(type)|Type cast
||`*`|Indirection (dereference)
||&|Address-of
||`sizeof`|Size-of
||new, new[]|Dynamic memory allocation (C++ only)
||delete, delete[]|Dynamic memory deallocation (C++ only)
|4|.*|Pointer to member (C++ only)|Left-to-right
||->*|Pointer to member (C++ only)
|5|`*`|Multiplication|Left-to-right
||`/`|Division
||`%`|Modulo (remainder)
|6|`+`|Addition|Left-to-right
||`-`|Subtraction
|7|`<<`|Bitwise left shift|Left-to-right
||`>>`|Bitwise right shift
|8|`<`|Less than|Left-to-right
||`<=`|Less than or equal to
||`>`|Greater than
||`>=`|Greater than or equal to
|9|`==`|Equal to|Left-to-right
||`!=`|Not equal to
|10|`&`|Bitwise AND|Left-to-right
|11|`^`|Bitwise XOR (exclusive or)|Left-to-right
|12|`|`|Bitwise OR (inclusive or)|Left-to-right
|13|`&&`|Logical AND|Left-to-right
|14|`||`|Logical OR|Left-to-right
|15|`? :`|Ternary conditional|Right-to-left
|16|=|Direct assignment|Right-to-left
||`+=`|Assignment by sum
||`-=`|Assignment by difference
||`*=`|Assignment by product
||`/=`|Assignment by quotient
||`%=`|Assignment by remainder
||`<<=`|Assignment by bitwise left shift
||`>>=`|Assignment by bitwise right shift
||`&=`|Assignment by bitwise AND
||`^=`|Assignment by bitwise XOR
||`|=`|Assignment by bitwise OR
|17|throw|Throw operator (exceptions throwing, C++ only)|Right-to-left
|18|,|Comma|Left-to-right


（摘自[Operators in C and C++](https://en.wikipedia.org/wiki/Operators_in_C_and_C%2B%2B#Operator%20precedence)）

运算次序：
`5 + 10 * 20 / 2`
- Operand: stack: , mem: 5; Operator: stack, mem: +; 
- Operand: stack: 5, mem: 10; Operator: stack: +, mem: *; So first do *. 
- Operand: stack: 5, 10, mem: 20; Operator: stack: +, *, mem: /; `/` is not higher than `*`, so pop 10 to compute. 
- Operand: stack: 5, 200, mem: 2; Operator: stack: +, mem: /; `/` is higher than `+`, pop 200 to do compute. 
- Operand: stack: 5, 100, mem: ; Operator: stack: +, mem: ; Pop all to finish. 

算术异常：
- 数学错误。
- 计算机特性：溢出。 
```
short short_value = 32767; 
short ival = 1; 
short_value += ival; // -32768 wrapped around
```

`%`：reminder 或modulus （求余）：
- 只能用于`bool`, `char`, `short`, `int`, `long` 及对应的`unsigned` 的整型。  
- 两操作数为负，返回负值。
- 一操作数为负，未定，机器决定。

`/`：一操作数为负，上或下取整由机器决定。  

### 5.2
0为假，其他为真。  
Short-circuit evalution（短路求值）：当表达式的布尔值已确定时，不会再执行下去。  
```
string s("Expressions in C++ are composed..."); 
string::iterator it = s.begin(); 
while (it != s.end() && !isspace(*it)) {
    *it = toupper(*it); 
    ++it; 
}
```

bool值true转换为整型1：`i < j < k` 当k 大于1时总成立。  

### 5.3
位操作符：
- 将整型看作二进制序列。
- 或者处理bitset 类型。
- 对于负数，如何操作符号位取决于机器。所以建议使用unsigned 型。  
```
unsigned int bits = 1; 
bits = ~bits; // (2^32 - 1) - 1
```
- `>>`和`<<`抛弃移出去的位。无符号时补0。
- 有符号数，`>>`可能移进符号位副本或0，取决于机器。  
- 右操作数必须为小于左操作数位数的正值。  
- `&` `|` 位异或`^`： 
```
unsigned char b1 = 0145; // oct value
unsigned char b2 = 0257;
unsigned char result = b1 & b2; // 0045
```
#### 5.3.1
用`bitset`做布尔数组：
```
bitset<30> quiz; 
quiz.set(27); 
quiz.reset(28); 
bool status; 
status = quiz[28]; 
```

如果用整型实现会比较复杂：
```
unsigned int quiz; 
quiz |= 1UL << 27; 
quiz &= ~(1UL << 28); 
bool status; 
status = quiz & (1UL << 28); 
```

#### 5.3.2
输入输出标准库重载了`>>`和`<<`。  
- 左结合：
```
cout << "hi" << "there" << endl; 
((cout << "hi") << "there") << endl; 
```
- 优先级低于算术操作符，高于关系操作符：
```
cout << 1 + 2; 
cout << (1 < 2); 
```

### 5.4
赋值操作符的左操作数必须是非const 的左值：  
```
i + j = ival; // error, i + j return rvalues
```

当左右操作数类型不符时，会类型转换而改变值。  
#### 5.4.1
赋值操作是右结合的：
```
int val; 
int *pval; 
val = pval = 0; // error, cannot assign pointer to int
```
#### 5.4.2
优先级很低，故可以写入条件表达式：  
```
int i; 
while ((i = getValue()) != 42)
    // do
```
小心`if (i == val)` 不要写成赋值。  
#### 5.4.3
复合赋值操作符：`+=` 等10个。计算中左操作数只计算一次。  

### 5.5
自增：在变量前和变量后分别是两种运算符。
- `++i`是右结合的，返回的值是左值（对象本身）。
- `i++`是左结合的，返回右值（对象原始值）。
- 如无必要不使用后自增。只有int和指针编译器有优化。
```
vector<int> ivec; 
int cnt = 10; 
while (cnt > 0)
    ivec.push_back(cnt--);  // 9 - 1

vector::iterator iter = ivec.begin(); 
while (iter != ivec.end()) 
    cout << *iter++ << endl; 
```

### 5.6
箭头操作符：`->`等同于点`.`加解引用`*`操作符。  
```
Sales_item item1; 
Sales_item *sp = &item1; 
(*sp).same_isbn(item2); // * is lower priority than .
```
为避免忘记给解引用操作符加括号，引入`->`：
```
p->foo; // (*p).foo
sp->same_isbn(item2); 
```

### 5.7
Conditional operator(条件操作符)：`cond ? expr1 : expr2`。
- 三元。
- 短路求值。
- 优先级低。
```
cout << (i < j ? i : j); 
```

### 5.8
`sizeof`操作符：
- 返回`size_t`类型。
- 单位是字节。 
- 作用于表达式时，其实不执行表达式，只检查返回类型。
- 作用于类型名，一定要括号，返回该类型所需内存：
```
cout << sizeof(int) << endl; // 4
```
- 作用于表达式，返回结果所需内存：
```
int a = 1, b = 2; 
cout << sizeof (a + b) << endl; // 4
```
- 作用于指针，返回地址大小；作用于解引用的指针才返回对象大小：
```
    char *p; 
    cout << sizeof p << endl; // 4
    cout << sizeof *p << endl; // 1
```
- 作用于引用类型，返回存放引用类型对象所需的空间：
```
vector<int> ivec; 
vector<int>::iterator iter; 
cout << sizeof iter << endl; // 12
cout << sizeof *iter << endl; // 4
```
- 作用于数组，是数组元素长和元素个数的乘积。
```
int a[3]; 
cout << (sizeof a / sizeof *a) << endl; // 3
```

### 5.9
逗号表达式：
- 用逗号排列的多个表达式。
- 每个表达式从左向右执行。
- 逗号表达式的值最右边的表达式值。<b>?</b>
```
vector<int> ivec(9); 
int cnt = ivec.size(); 
for (vector<int>::size_type ix = 0; ix != ivec.size(); ++ix, --cnt)
    ivec[ix] = cnt; 
```

