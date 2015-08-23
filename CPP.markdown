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
using std::vector; 
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
- 中间元素：`vector<int>::iterator mid = vi.begin() + vi.size() / 2;`
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
#### 4.3.1
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
|1|`::`|Scope resolution (C++ only)|None
|2|`++`|Suffix increment|Left-to-right
||`--`|Suffix decrement
||`()`|Function call
||`[]`|Array subscripting
||`.`|Element selection by reference
||`->`|Element selection through pointer
||`typeid()`|Run-time type information (C++ only)
||`const_cast`|Type cast (C++ only)
||`dynamic_cast`|Type cast (C++ only)
||`reinterpret_cast`|Type cast (C++ only)
||`static_cast`|Type cast (C++ only)
|3|`++`|Prefix increment|Right-to-left
||`--`|Prefix decrement
||`+`|Unary plus
||`-`|Unary minus
||`!`|Logical NOT
||`~`|Bitwise NOT (One's Complement)
||`(type)`|Type cast
||`*`|Indirection (dereference)
||`&`|Address-of
||`sizeof`|Size-of
||`new, new[]`|Dynamic memory allocation (C++ only)
||`delete, delete[]`|Dynamic memory deallocation (C++ only)
|4|`.*`|Pointer to member (C++ only)|Left-to-right
||`->*`|Pointer to member (C++ only)
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
|16|`=`|Direct assignment|Right-to-left
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
|17|`throw`|Throw operator (exceptions throwing, C++ only)|Right-to-left
|18|`,`|Comma|Left-to-right


（摘自[Operators in C and C++](https://en.wikipedia.org/wiki/Operators_in_C_and_C%2B%2B#Operator%20precedence) 和5.10.2）

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

### 5.10
Compound expression(符合表达式)：
- 含有多个操作符。
- 操作符的优先级决定操作数的结合方式，并没有说明计算次序。
- 由结合性决定次序。

#### 5.10.1
优先级相同的操作符，由结合性决定谁先执行。
```
int res = 6 + 3 * 4 / 2 + 2; 
int tmp1 = 3 * 4; // same as /, left to right
int tmp2 = tmp1 / 2; 
int tmp3 = 6 + tmp2; 
int res = tmp3 + 2; 
```
#### 5.10.2
左结合：
```
a + b + c; 
((a + b) + c); 
```
右结合：
```
a = b = c; 
(a = (b = c));
```
#### 5.10.3
`&&`和`||`和`?:`当且仅当右操作数（为一个函数且改变某对象的值）有影响时才计算。  
其他操作符并未指定操作数的执行次序。  
```
if (ia[index++] < ia[index]) // ia[0] < ia[0] or ia[0] < ia[1] undifined
```

避免错误的建议：
- 不清楚时使用括号。
- 修改值的语句内不要出现调用该值的操作。除了`*--iter`。

### 5.11
动态创建对象时返回的是指针而不是该对象名称。  
```
int i; // i is name
int *pi = new int; 
```
初始化对象与初始化变量相同：
```
int *pi = new int(1024); // Direct-initialization
int *pi = new int; // uninitialized
int *pi = new int(); // value-initialize, 0
```
对未初始化的对象的引用没有意义。  

内存耗尽后，会抛出`bad_alloc`的异常。  

释放：
```
delete pi; 
```
- 如果不是用`new`动态创建的对象，`delete`该指针非法。
```
int i; 
int *pi = &i; 
string str = "dwarves"; 
int *pz = 0; 

delete pi; // error but can compile
delete str; // failed to compile
delete pz; // ok but useless
```
- 释放了对象后内存释放，但指针仍指向该处，为dangling pointer（悬垂指针）。为避免错误最好置0。

`const`对象创建时必须初始化。
```
const int *pci = new int(1024); 
delete pci; 
```

易发错误：
- memory leak（内存泄漏）：删除动态分配的对象指针失败。很难发现。  
- 读写已删除的对象：所以释放后立即将指针置0。  
- 两次删除同一内存空间：当两个指针指向相同对象时。会造成只有存储区破坏。  

### 5.12
Conversion(互相转换)：
- 相关的类型。
- Implicit type conversion(隐式类型转换)：由编译器执行。
- 算术类型：内置转换由低精度向高精度转换，如`double`加`int`则转换`int`为`double`。
- 左值精度低于计算结果时会产生`warning`。

#### 5.12.1
混合类型的表达式中隐式转换操作数：
```
int ival; double dval; 
ival >= dval; // ival change to double
if (ival); // ival change to bool
ival = 3.14; // 3.14 change to int
```
#### 5.12.2
Arithmetic conversion(算术转换)：
- 二元操作符的操作数
- 转换为两数中较大精度的类型
- Integral promotion(整型提升)：小于`int`的如`char`
- 无符号的比同类型有符号的大，但不同类型，如`unsigned short`仍比`int`小。
- 依赖于机器，如32位机器，`long`和`int`都用一个字长。
- 转换无符号类型时可能会出错，避免使用。

#### 5.12.3
指针转换：
- 将数组转换为第一个元素的地址。  
- 所有指针转换为`void*`
- 0转换为任意指针类型。

布尔转换：0值，包括指针为false。  

枚举对象和成员自动转换为整型：最小能容纳成员数目的整型，如`int`不够，则为`unsigned int`或`long`。  
```
enum Points {point2d = 2, point2w, point3d = 3, point3w}; 
const size_t arr_size = 1024; 
int chunk_size = arr_size * point2w; 
int array_3d = arr_size * point3w; 
```

用非const的对象初始化const对象的引用，或用地址赋给const类型指针，自动转换该对象为const：
```
int i; 
const int &ref = i; 
const int *pt = &i; 
```

标准库类型转换：
- `cin >> s`返回的是`isteam`类型的`cin`，但是可转换为bool。 

#### 5.12.4
cast(强制类型转换)：
- 非常危险。
- 操作符：`static_cast`，`dynamic_cast`，`const_cast`，`reinterpret_cast`。

#### 5.12.5
```
double dval; 
int dval; 
ival *= static_cast<int>(dval); 
```
#### 5.12.6
强制类型转换概述：
`cast-name<type>(expression)`
- `dynamic_cast`: 运行中识别指针或引用指向的对象。
- `const_cast`: 转换表达式的const属性：如函数`string_copy`只接受非const参数。
```
const char *pt_str; 
string_copy(const_cast<*char>(pt_str)); 
```
- `static_cast`: 与编译器的自动类型转换相同。可用于覆盖编译器精度丢失的警告。
```
void* p = &d; 
double *dp = static_cast<double*>(p); 
```
- `reinterpret_cast`: 操作数位模式的低层次解释。强行改变对象类型。强烈不建议使用。 
```
int *ip; 
char *pc = reinterpret_cast<char*>(ip); 
string str(pc); // meaningless and unexpected
```

#### 5.12.7
旧式强制转换：
- 可视性差（无法搜索定位）。
- `type (expr)`：function-style cast notation。
- `(type) expr`: C-language-style cast notation。
- 先尝试较安全的`static_cast`和`const_cast`，如不合法，再执行`reinterpret_cast`。

## Chapter 6
Flow-of-control（控制流）。  

### 6.1
Expression statement(表达式语句)。  
null statement(空语句): `;`  
```
while (cin >> s && s != sought)
    ;
```

### 6.2
Declaration statement(声明语句)。

### 6.3
Compound statement(复合语句): 又称block (块)。  
- `{}`内。
- 内部变量的作用域仅为内部。
- 不以`;`结尾。

### 6.4
控制结构中，即`while`, `for`括号内定义的变量作用域仅在后面的块语句中。  
如要在块外访问，则需在控制结构外定义：  
```
vector<int>::size_type index = 0; 
for (; index != vec.size(); ++index) 
    ;
```

### 6.5
```
if (int ival = compute_value()) { // must init and can convert to bool
} else if {
} else {
}
```

Dangling-else(悬垂else): 两个`if`后接一个`else`。`else`与最后出现的未于`else`匹配的`if`配对。  

### 6.6
#### 6.6.1
Switch 语句：
- `switch`括号中的表达式必须返回一个整数。
- `case`后的值为case label(case 标号)，必须是整型常量，且需互相不同。
```
char ch; 
int aCnt = 0, bCnt = 0, otherCnt = 0; 
while (cin >> ch) {
    switch (ch) {
        case "a":
            ++aCnt; 
            break; 
        case "b":
            ++bCnt; 
            break; 
        default: 
            ++otherCnt;
            break; 
    }
}
```
#### 6.6.2
```
switch (ch) {
    case 'a': case 'b':
        ++a_or_bCnt; 
        break; 
}
```
#### 6.6.3
Default label(default 标号)： 
#### 6.6.4
`switch(int ival = get_response())` 是正确的语法。  
#### 6.6.5
`switch`内部的变量只能定义在最后一个case中，或在case执行的操作中加入块。  

### 6.7
`while`循环条件中的变量作用域只在之后的块中。  
每执行一次循环，循环条件都执行一遍，所以该处的定义的变量会重复定义。  
```
int *source = arr1; 
size_t sz = sizeof(arr1) / sizeof(*arr1); 
int *dest = new int[sz]; 
while (source != arr1 + sz) 
    *dest++ = *source++; 
```

### 6.8
```
for (init-statement condition; expression)
    ;
```
init-statement(初始化语句)：同时包括initializer 和condition。 
```
for (vector<int>::size_type ind = 0; ind != svec.size(); ++ind) {
    cout << svec[ind]; 
    if (ind + 1 != svec.size()) 
        cout << " "; 
}
```
#### 6.8.1
可用一个空语句省略`for`循环中的任意一部分。但是后两部分省略了循环都无法退出。  
#### 6.8.2
`for`初始化可以同时定义多个变量，用逗号分隔，所以只能是同类型。  
表达式部分也可用逗号分隔。  

### 6.9
`do ... while`不能在循环条件中定义变量。  

### 6.10
`break`只能出现在循环和`switch`中，用于跳出当前层的循环。  

### 6.11
`continue`提前结束此次循环。  

### 6.12
`goto`语句难以理解和修改，不使用。  
(!!skip!!)

### 6.13
异常处理：  
- `throw` expression: `raise`了异常条件，但当场不处理。  
- `try` block: 处理`try`块中抛出的异常，以多个`catch` clause(子句) 结束。`catch`子句又称handler(处理代码)。  
- exception class(异常类): 标准库定义，用以传递错误信息。  

#### 6.13.1
```
if (!item1.same_isbn(item2)) 
    throw runtime_error("Data must refer to same ISBN"); 
```
`runtime_error` 是该表达式的类型。定义在`stdexcept` 头文件中。用字符串创建。  
#### 6.13.2
```
try {
    program-statements
} catch (exception-specifier) {
    handler-statements
}
```
- exception specifier: 异常说明符。  
- program statements: 任意语句，作用域仅在`try` 和之后的`catch`中。  
- `err.what()`返回标准库异常类error 的初始化字符串对象副本，为C风格字符串。 
```
while (cin >> item1 >> item2) {
    try {
        func_throw_runtime_err(); 
    } catch (runtime_error err) {
        cout << err.what()
             << "\nTry again? y/n"; 
        char c; 
        cin >> c; 
        if (cin && c == 'n')
            break; 
    }
}
```

如果找不到匹配的`catch`子句，程序跳转到定义在`exception`中的标准库函数`terminate`。  
#### 6.13.3
标准库异常：
- `exception`头文件：定义`exception`类，用于通知异常发生。 
- `stdexcept`头文件：
 1. `exception`: 最常见的异常，标准库中只定义了默认构造函数。 
 1. `runtime_error`: 运行时才能检测到的异常。 
 1. `range_error`: 超出值域。 
 1. `overflow_error`。
 1. `underflow_error`。
 1. `logic_error`: 可在运行前检测到。 
 1. `domain_error`: 参数的值不存在。 
 1. `invalid_argument`: 不合适的参数。 
 1. `length_error`: 产生了超出类型长度的对象。 
 1. `out_of_range`: 超出有效范围的值。 
- `new`头文件：`bad_alloc`异常类型，只定义了默认构造函数。
- `type_info`头文件：`bad_cast`异常类型，只定义了默认构造函数。 

除了`exception`,`bad_alloc`和`bad_cast`外，其他异常只定义了用`string`初始化的构造函数。  
只有一个`what()`操作。返回`const char*`。  

### 6.14
用预处理器调试：
```
int main() 
{
    #ifndef NDEBUG
    cerr << "Starting main" << endl; 
    cerr << __FILE__ << endl; 
    #endif
}
```
- 开发过程中，保持`NDEBUG`未定义，则会执行其中的测试代码。  
- 交付时，定义预处理变量再编译：
```
$ CC -DNDEBUG main.C # same as #define NDEBUG
```

调试常量：
- `__FILE__`
- `__LINE__`
- `__TIME__`: 文件编译时间。 
- `__DATE__`

Preprocessor macro(预处理宏): 类似函数调用，用表达式作为条件。  
`assert`(断言)预处理宏： 
- 定义于`cassert`中。 
- `assert(expr);`。如果表达式结果为false或0，输出信息并终止程序。 

### 小结
flow of control(控制流): 程序执行路径。  
现代C++很少使用预处理宏。 


## Chapter 7
function(函数)：传递参数，返回值。  
inline(内联)函数，类成员函数和重载函数。  

### 7.1
函数：
- 函数名和操作数类型唯一表示一个函数。 
- Parameter(形参)。
- function body(函数体)。 
- return type(返回类型)。

Call operator(调用操作符): `()`，操作数是函数名和一组argument(实参)。  

执行过程：

1.创建形参并用实参初始化。  
1.Calling function(主调函数) 挂起。  
1.Called function(被调函数) 执行。  


形参是Local variable(局部变量)。  

`return`: 返回结果。  
#### 7.1.1
`Date &calendar(const char*)` 返回一个Date对象的引用。  
`int *foo_bar()` 返回int指针，可指向函数或数组。  
#### 7.1.2
形参表每个参数都需单独定义类型。`int v1, v2`是错的。  
C++是静态强类型语言，编译时会检查实参的类型是否与形参相同或可隐式转换。不然会报`interface error`(接口错误)。  

### 7.2
形参如果不是引用，复制实参；不然是实参的别名。  
#### 7.2.1
函数非引用地调用实参，不会改变实参的值。  
指针形参：实参指向的地址不变，但该地址存储的数据可以改变，除非是`const`类型指针。  
可以将const指针指向非const的对象，但不能用非const指针指向const的对象。  
为了兼容C，编译器将const的形参认为是非const的普通类型。  

当1. 需要改变; 2. 时空间占用大; 3. 无法复制对象 时，使用指针形参。  
#### 7.2.2
Local copy(局部副本)。  

一个函数返回多个结果：  
```
#include <iostream>
#include <vector>

using std::cout; 
using std::endl; 
using std::vector; 

vector<int>::const_iterator find_val(
    vector<int>::const_iterator beg, 
    vector<int>::const_iterator end, 
    int value, vector<int>::size_type &occurs) 
{
    vector<int>::const_iterator res_iter = end; 
    occurs = 0; 
    
    for (; beg != end; ++beg) 
        if (*beg == value) {
            if (res_iter == end) 
                res_iter = beg; 
            ++occurs; 
        }

    return res_iter; 
}

int main() 
{
    vector<int> ivec(5, 4); 
    vector<int>::size_type occurs; 

    vector<int>::const_iterator res_iter = find_val(ivec.begin(), ivec.end(), 4, occurs); 
    int val = *res_iter; 
    cout << val << endl; 
    cout << occurs << endl; 

    return 0; 
}
```

非const引用形参只能与完全相同类型的非const对象关联。  
应该将不修改实参的形参定义为const引用，不然函数无法传入字面值，右值或const对象。  

交换指针： 
```
#include <iostream>

using std::cout; 
using std::endl; 

void ptrswap(int *&v1, int *&v2) {
    int *tmp = v1; 
    v1 = v2; 
    v2 = tmp; 
}

int main() 
{
    int a = 1, b = 2; 
    int *p1 = &a; 
    int *p2 = &b; 

    ptrswap(p1, p2); 
    
    cout << *p1 << endl; 
    cout << *p2 << endl; 

    return 0; 
}
```

#### 7.2.3
通过传递迭代器来传递容器到函数。  
```
void print(vector<int>::const_iterator beg, vector<int>::const_iterator end) {
    while (beg != end) {
        cout << *beg++; 
        if (beg != end) 
            cout << " "; 
    }

    cout << endl; 
}

int main() 
{
    vector<int> ivec(5, 4); 
    print(ivec.begin(), ivec.end()); 

    return 0; 
}
```

#### 7.2.4
数组无法复制，只能用指针做形参。  
`int*`, `int[]`, `int[10]`三种形参等价，但是第一种最好。  

如形参是数组的引用，不会将数组转换为指针，而是传递数组的引用。这时数组大小决定传入的形参需什么大小。  
```
void printValues(int (&arr)[10]) {
    const size_t array_size = 10; 
    for (size_t ix = 0; ix != array_size; ++ix) {
        cout << arr[ix] << " "; 
        if (ix != array_size - 1) cout << " "; 
    }

    cout << endl; 
}

int main() 
{
    int arr[10] = {1, 2, 3, 4, 5, 6, 7, 8, 9, 0}; 
    printValues(arr); 

    return 0; 
}
```
注意`int &arr`是一个`int`的引用，而`arr[10]`是一个数组元素，不能引用。`[]`比引用操作符优先级高。   

多维数组的传递：元素也是数组。形参指针需是一个数组指针。  
```
void printValues(int (*matrix)[2], int rowSize) {
    size_t colSize = 2; 
    for (size_t rowIx = 0; rowIx != rowSize; ++rowIx) {
        for (size_t colIx = 0; colIx != colSize; ++colIx) {
            cout << matrix[rowIx][colIx]; 
            if (colIx != colSize - 1) cout << " "; 
        }
        cout << endl; 
    }
}

int main() 
{
    int matrix[3][2] = {{1, 2}, {3, 4}, {5, 6}}; 
    printValues(matrix, 3); 

    return 0; 
}
```

#### 7.2.5
传递数组需确保不会越界：
- 在数组内放置一个标记，如C风格字符串。  
- 使用标准库规范，提供指向数组的第一个和最后一个元素的数组。  
- 显示传递数组大小。C中常用。调用`printValues(arr, sizeof(arr) / sizeof(*arr))`。  

标准库规范：
```
void printValues(const int *beg, const int *end) {
    while (beg != end) {
        cout << *beg++; 
        if (beg != end) cout << " "; 
    }
}

int main()
{
    int arr[3] = {1, 2, 3}; 
    printValues(arr, arr + 3); 

    return 0; 
}
```

#### 7.2.6
给`main`函数传递参数：
```
int main(int argc, char *argv[])
int main(int argc, char **argv)
```
- `argc`表示字符串数组的个数。  
- `argv`是字符串数组，两种方式等价。  
- 如果`main`在`prog`的可执行文件中，则`$ prog -d -o ofile data0`调用。 
- 则`argc`为5，数组的元素为
```
int main(int argc, char *argv[])
{
    cout << argc << endl; 
    for (char** beg = argv; beg != argv + argc; ++beg) {
        cout << *beg << endl; 
    }

    return 0; 
}
```
结果为
```
argv[0] = "prog"; 
argv[1] = "-d"; 
argv[2] = "-o"; 
argv[3] = "ofile"; 
argv[4] = "data0"; 
```

Visual Studio 2010 调试main： 
- 菜单 -> 项目 -> [工程名]属性 -> 配置属性 -> 调试 -> 命令参数 添加传递的参数。 
- 输出的第一个字符串是 [项目路径]\Debug\[工程名].exe。

#### 7.2.7
省略符形参`...`：  
- 暂停了类型检查机制。  
- 可以有0或多个实参。 
- C++中的省略符形参是为了编译使用了`varargs`的C程序<b>?</b>。  
- 只能传递简单数据类型，类对象可能不能正确复制。  
- 显式声明的形参仍会检查类型。  
```
void foo(param_list, ...)
void foo(...)
```

### 7.3
```
return; 
return expression; 
```

#### 7.3.1
`return`用于`void`类型函数中类似`break`功能。  

#### 7.3.2
非`void`的函数必须`return`正确的或可隐式转换的结果。  
就算循环内有`return`，非`void`函数的循环外一定要有`return`。  
`main`函数可以没有`return`就结束，自动返回`0`。  

`main`函数返回值代表执行成功与否： 
- `cstdlib`头文件中定义了预处理变量：  
```
#include <cstdlib>
int main()
{
    if (success) 
        return EXIT_SUCCESS; 
    else
        return EXIT_FAILURE; 
}
```

求解表达式时，如要临时存储某结果，编译器创建temporary object(临时对象)。  
函数的返回值初始化了调用函数时创建的临时对象。方法与用实参初始化形参相同。   
注意千万不要返回局部对象的引用。  
```
const string &shorterString(const string &s1, const string &s2) {
    return s1.size() < s2.size() ? s1 : s2; // Not copy 
}
```

引用返回左值：这返回的还是引用，可修改。  
```
char &get_val(string &str, string::size_t ix) {
    return str[ix]; 
}
int main() 
{
    string str = "a value"; 
    get_val(str, 0) = "A"; // now string is "A value"
}
```
如不需要修改返回值，声明为const: 
```
const char &get_val(str, 0)
```

千万不要返回指向局部对象的指针。  

#### 7.3.3
recursion function(递归函数): 
- 必须有个终止条件，不然会infinite recursion error。 
- `main()`不能调用自己。 

求最大公约数： 
```
int rgcd(int v1, int v2) {
    if (v2 != 0) 
        return rgcd(v2, v1 % v2); 
    return v1; 
}
```

论证： 
* v1 = ax, v2 = bx
* 如设i 为除数，则v1 = iv2 + (a - ib)x
* 如余数(a - ib)x = 0, 则a = ib
* 则v1 = ibx, v2 = bx, 最大公约数是v2

percolate(向上回渗)：返回值作为上层调用的返回值。  

### 7.4
函数声明可以和定义分离，可以声明多次但只可定义一次。  
function prototype(函数原型): 是组成声明的部分。返回类型，函数名，形参列表。形参不必命名。  
函数声明放在头文件中。  

默认实参：给形参预定义一个值。形参表中之后的形参也都需是默认实参。调用函数时可省去这些参数。  
```
string screenInit(string::size_type height = 24, string::size_type width = 80, char background = ' '); 
screenInit(66); // equal to screenInit(66, 80, ' ')
```

只能给形参指定默认实参一次。故在头文件的函数声明中指定默认实参。  

### 7.5
lifetime(生命期)：对象存在的时间。  

#### 7.5.1
automatic object(自动对象): 当函数被调用时才存在的对象。  

#### 7.5.2
static local object(静态局部变量): 用`static`定义。生命周期跨越函数多次调用。第一次执行时初始化。  
```
size_t count_call() {
    static size_t ctr = 0; 
    return ++ctr; 
}
int main()
{
    for (int i = 0; i != 10; ++i) 
        cout << count_call() << endl; 
    return 0; 
}
```

### 7.6
调用函数比表达式执行慢。  
内联函数：
- 编译器在调用函数处展开代码。
- 用`inline`定义。  
- inline specification(内联说明) 不强制编译器执行。
- 应该在头文件中定义。 
- 修改后需全部文件重新编译。
```
inline &shortString(const string &s1, const string &s2) {
    return s1.size() < s2.size() ? s1 : s2; 
}
```

### 7.7
成员函数： 
- 函数原型必须在类中定义，即必须在类中声明函数。
- 但是函数体可在类中或类外定义。  
```
class Sales_item {
public: 
    double avg_price() const; 
    bool same_isbn(const Sales_item &rhs) const {
        return isbn == rhs.isbn; // isbn is private
    }
    
private: 
    std::string isbn; 
    unsigned units_sold; 
    double revenue; 
}; 
```

#### 7.7.1
- 类的所有成员必须在类定义中说明。
- 在类内定义的函数隐式成为内联函数。 
- 类的成员函数可以访问该类的private 成员。 
- 调用成员函数时，隐藏的形参是调用该成员函数的对象，即`this`指针指向自己。 
- `const`改变隐藏的`this`形参类型。 
- 不能显式地形参表中加入`this`，但是在函数体中使用`this->attr`是合法但不必须的。 

编译器重写为： 
```
Sales_item::same_isbn(const Sales_item &rhs) const {
    return (this->isbn == rhs.isbn); //const Sales_item *const this 
}
```

const member function(常量成员函数): 声明函数时在形参表后添加`const`。  

#### 7.7.2
在类定义外定义成员函数：  
```
double Sales_item::avg_price() const {
    if (units_sold)
        return revenue / units_sold; 
    else 
        return 0; 
}
```

#### 7.7.3
constructor(构造函数)： 
- 与类名相同，没有返回类型。 
- 一个类可有多个不同形参表的构造函数。  
- 应确保每个数据成员都初始化了。 
- default constructor(默认构造函数): 无形参。 
- 构造函数需为`public`。 

构造函数定义：  
```
class Sales_item {
public: 
    Sales_item: units_sold(0), revenue(0.0) {}
}
```

constructor initializer list(构造函数的初始化列表): 为数据成员指定初值。  

synthesized default constructor(合成的默认构造函数): 未定义默认构造函数，则用变量初始化规则初始化。但不会初始化内置类型。  

#### 7.7.4
`type`类定义置于`type.h`中。成员函数的定义则置于`type.cc`。    

### 7.8
overloaded function(重载函数): 
- 相同作用域中的两函数有相同的名字但不同的形参表。  
- function overloading(函数重载)简化了程序的实现。  
- `main()` 不能重载。  
- 两函数的非引用形参如只有是否为`const`这样的区别，则不是重载，而是重定义。 
- 但引用形参和指针形参，为`const`和不为`const`是不同的，函数会重载。  

#### 7.8.1
局部声明的函数将屏蔽而非重载全局作用域中同名的函数。所以重载函数都需定义在同一作用域中。  
不建议局部声明函数。  

#### 7.8.2
overload resolution(重载确定): 即function matching(函数匹配)。用实参表去找函数。可能发生：  
- 编译器找到best match(最佳匹配)。  
- 找不到。  
- 找到多个存在ambiguous(二义性)的函数。  

#### 7.8.3
```
void f(int); 
void f(double, double = 3.14); 

f(5.6); // use f(double, double)
```
candidate funtion(候选函数)，viable funtion(可行函数)。  
最佳匹配：
- 每个实参的匹配都不劣于其他可行函数。 
- 至少一个实参匹配优于别的函数。 

#### 7.8.4
实参类型转换： 
- exact match(精确匹配)。 
- promotion(类型提升)。  
- standard conversion(标准转换)。 
- class-type conversion(类类型转换)。 

较小的整型优先提升为`int`而非`short`，如`char`。  
类型提升优于标准转换。  
标准类型转换的优先级相同，会造成二义性。  

枚举的匹配： 
```
enum Tokens {INLINE = 128, VIRTUAL = 129}; 
void ff(Tokens); 
void ff(int); 
void newf(unsigned char); 
void newf(int); 

int main()
{
    Tokens curTok = INLINE; 
    ff(128); // ff(int)
    ff(INLINE); 
    ff(curTok); 
    newf(INLINE); // newf(int)
}
```
因为枚举无法用整型初始化。但可以将枚举传递给整型。  

指针是否为`const`不影响重载，指向`const`对象的指针才用于寻找匹配。  
```
f(int *const); // a const pointer 
f(const int*); // a pointer to const object
```

### 7.9
函数指针：  
- 指向函数的指针。 
- 函数类型由其形参表及返回类型确定，与函数名无关。  
- 用`typedef`将该指针简化成一种类型。 
- 不存在类型转换。 

```
bool (*pf)(const string&, const string&); // a pointer to function
bool *pf(const string&, const string&); // a function that return a bool pointer
```

初始化和赋值： 
```
bool lengthCompare(const string&, const string&); 

typedef bool *(cmpFcn)(const string&, const string&); 
cmpFcn pf1 = 0; 
cmpFcn pf2 = lengthCompare; 
pf1 = lengthCompare; 
pf1 = pf2; 
```

函数名等于该函数地址： 
```
cmpFcn pf1 = lengthCompare; 
cmpFcn pf2 = &lengthCompare; 
```

调用： 
```
pf("hi", "bye"); // implicitly dereferenced
(*pf)("hi", "bye"); // explicitly dereferenced
```

函数的形参可以是函数或函数指针； 
```
void useBigger(const string&, bool(const string&)); 
void useBigger(const string&, bool (*)(const string&)); // equal
```
但返回类型必须是函数指针。  

返回指向函数的指针: 
```
int (*ff(int))(int*, int); 

typedef int (*PF)(int *, int); 
PF ff(int); 
```
这是个叫`ff`的函数，形参是一个`int`，返回值是指向`int (*)(int*, int)`函数的指针。  

指向重载函数：  
```
extern void ff(vector<double>); 
extern void ff(unsigned int); 

void (*pf1)(unsigned int) = &ff; // reloaded function
```

## Chapter 8
IO标准库面向对象。  

### 8.1
wide-character(宽字符)。  
IO操作可作用于不同设备和不同大小的字符。  

标准库用inheritance(继承)来定义object-oriented(面向对象)的类。  
通过继承关联的类都共享共同的接口。  
base class(基类)和derived class(派生类)。  

IO类型定义于三个头文件中:  
- `iostream`读写控制窗口。定义`istream`, `ostream`, `iostream`类型。  
- `fstream`读写文件。定义`ifstream`, `ofstream`和`fstream`类型。  
- `sstream`读写内存中的`string`。定义`istringstream`, `ostringstream`和`stringstream`类型。  

![IO Classes](./CPP_files/IO_classes.png)  

如函数有基类类型的引用形参时，可给函数传递派生类型的对象。  

支持`wchar_t`类型(国际字符)的类：`wostream`, `wistream`和`wiostream`, `wifstream`, `wofstream`和`wfstream`, `wistringstream`, `wostringstream`和`wiostringstream`。  
标准输入`wcin`, 输出`wcout`, 错误`wcerr`。  
也在三个头文件中定义。  

标准库类型不允许复制或赋值。`ofstream out1, out2;`，则`out1 = out2`是错误的。也不能做形参，只能用指针或引用。  
```
ofstream &print(ofstream&); 
```
对IO 对象的读写会改变它的状态，所以引用不能是`const`的。  

### 8.2
IO标准库管理一系列condition state(条件状态):  
- `strm::isolate`: 机器相关，定义条件状态。 
- `strm::badbit`: `strm::isolate`类型的值，指出被破坏的流。无法恢复该流。  
- `strm::failbit`: `isolate`类型的值，失败的操作。如将字符串读入整型变量。 
- `strm::eofbit`: `isolate`类型的值，到达文件结束符。 
- `s.eof()`: 如设置了`eofbit`值，返回true。 
- `s.fail()`: 如设置了`failbit`值，返回true。 
- `s.bad()`: 如设置了`badbit`值，返回true。 
- `s.good()`: 处于有效状态否。 
- `s.clear()`: 所有s的状态重置。 
- `s.clear(flag)`: flag是`isolate`类型。 
- `s.setstate(flag)`。 
- `s.rdstate()`: 返回流s的当前条件。是`iostate`类型。 

```
int ival; 
while (cin >> ival, !cin.eof()) {
    if (cin.bad()) 
        throw runtime_error("IO Stream corrupted"); 
    if (cin.fail()) {
        cerr << "bad data, try again"; 
        cin.clear(istream::failbit); 
        continue; 
    }
    
    cout << ival; 
}
```

```
istream::iostate old_state = cin.rdstate(); 
cin.clear(); 
process_input(); 
cin.clear(old_state); // reset
```

利用位操作同时操作多个状态： 
```
is.setstate(ifstream::badbit, ifstream::failbit); 
```

### 8.3
IO对象管理一个缓冲区。刷新时才真正写入输出设备： 
- `main`函数结束。 
- 缓冲区满。 
- 操纵符如`endl`换行, `flush`, `ends`插入`null`字符，可显式刷新。 
- 用操纵符`unitbuf`设置流的内部状态令其每次执行完写操作后都刷新。`cout << unitbuf << "a" << "b" << nounitbuf;` 用`nounitbuf`复原。 
- 将输出流与输入流tie(关联)起来。如`cin`和`cout`已关联。  

注意程序崩溃不会刷新缓冲。  

关联输入与输出流：
```
cin.tie(&cout); // What IO library does
ostream *old_tie = cin.tie(); 

cin.tie(0); // break tie
cin.tie(&cerr); // tie to cerr is a bad idea
cin.tie(0); 
cin.tie(old_tie); // reset
```

### 8.4 
`fstream`头文件中定义的类型: 
- `ifstream`
- `ofstream`
- `fstream`: 读写同一文件。
  - 由`iostream`派生，故条件状态、操作符等都适用。  
  - 新定义了`open`和`close`操作。  

#### 8.4.1
`cin`, `cout`, `cerr`是绑定在标准输入输出上的。  
读写文件时，需定义对象并绑定于文件上。注意文件名为C风格字符串。  

用文件名初始化流对象，则相当于打开了文件。  
```
string ifile("in"); // input file name
string ofile("out"); 

ifstream infile(ifile.c_str()); 
ofstream outfile(ofile.c_str()); 
```

或者用`open`函数将对象与文件绑定。  
```
infile.close(); // Otherwise next step will fail
infile.open("in"); 
```  

检查文件打开是否成功：  
```
string it("in.txt"); 
ifstream infile(it->c_str()); 
if (!infile) {
    cerr << "Fail" << endl; 
    return -1; 
} else {
    string s; // include <string>
    while (infile >> s)
        cout << s << endl; // read file word by word
}
```

注意`close()`并不改变流对象的内部状态，需要`clear()`。  
```
#include <iostream>
#include <fstream>
#include <string>
#include <vector>

using std::cout; 
using std::cerr; 
using std::endl; 
using std::string; 
using std::ifstream; 
using std::vector; 

int main()
{
	vector<string> files; 
	files.push_back("ReadMe.txt"); 
	files.push_back("in.txt"); 

	ifstream input;

	vector<string>::const_iterator it = files.begin(); 
	while (it != files.end()) {
		input.open(it->c_str()); 
		if (!input) 
			break; 
		
		string s; 
		while (input >> s) 
			cout << s << endl; 

		input.close(); 
		input.clear(); 
		++it; 
	}

	return 0; 
}
```

#### 8.4.2
file mode(文件模式): 
- 是个整型常量。 
- 由`fstream`类定义。 
- 模式是文件的属性而非流的属性。  
- 构造函数和`open`函数都有默认实参。 
- 显式的模式： 
  - `in`: `ifstream`或`fsteam`类。
  - `out`: `ofstream`或`fsteam`类。不与`app`一起使用则会删除文件已有内容。  
  - `app`: 写之前找到文件尾。`ofstream`或`fsteam`类。 
  - `ate`: 打开文件时定位于文件尾。 
  - `trunc`: 打开文件时清空已有的流。`ofstream`或`fsteam`类。 
  - `binary`: 以二进制模式进行IO操作。`ofstream`或`fsteam`类。  

默认的`ofsteam`对象打开文件时文件会被清空，不然要显式指定`app`模式。  
```
ofstream outfile("file"); 
ofstream outfile("file", ofstream::out | ofstream::trunc); // same as above
ofstream appfile("file", ofstream::app); 
```

默认的`fstream`对象以`in`和`out`模式打开文件。  

常用程序：  
```
ifstream& open_file(ifstream &in, string &file) {
    in.close(); 
    in.clear(); 
    in.open(file.c_str()); 
    return in; 
}
```

### 8.5
`sstream`头文件： 
- 将流与内存中的`string`对象绑定起来。 
- 以`string`为形参的构造函数：将`string`对象复制给`stringstream`对象。 
- 对象的`str`成员为原`string`。 
- 特殊操作： 
  - `stringstream strm;`
  - `stringstream strm(s);`
  - `strm.str();`
  - `strm.str(s);`: 将`s`复制给`strm`，返回`void`。 

```
string line, word; 
while (getline(cin, line)) {
    istringstream stream(line); 
    while (stream >> word) {
        cout << word << endl; 
    }
}
```

格式转换：  
```
int val1 = 512, val2 = 1024; 
ostringstream format_message; 
format_message << "val1" << val1 << "\n"
               << "val2" << val2 << "\n"; 

istringstream input_string(format_message.str()); 
string dump; // useless part
input_string >> dump >> val1 >> dump >> val2; 
```
其中输入操作符忽略了空白字符如"\n"。  

## Chapter 9
容器共享公共接口。区别在于时间与效率。  
sequential container(顺序容器)类型: 
- `vector`: 支持快速随机访问。  
- `list`: 支持快速插入删除。 
- `deque`: double-ended queue(双端列表)。  

adaptor(适配器): 根据原始的容器类型提供的操作，定义新的操作接口。  
适配器类型: 
- `stack`：LIFO。  
- `queue`：FIFO。  
- `priority_queue`。  

其他操作由算法库提供。  

### 9.1
使用顺序容器： 
```
#include <vector>
vector<string> svec; 
```
用默认构造函数创建容器性能最佳。  

#### 9.1.1
容器构造函数： 
- `C<T> c; `
- `C c(c2); `: 容器类型和元素类型必须都相同。 
- `C c(b, e); `: `b` 和`e` 是标示开始和结束的迭代器。只要元素可复制，不要求容器或元素类型都相同。 
- `C c(n, t); `: 用`n` 个`t` 元素创建。只适用于顺序容器。 
- `C c(n); `: 用`n` 个初始化值的元素。只适用于顺序容器。 

指针也是迭代器。  
```
char *words[] = {"a", "b", "c"}; 
size_t words_size = sizeof(word) / sizeof(char *); 
list<string> words2(words, words + words_size); 
```

容器大小可以是非常量表达式。  
```
const list<int>::size_type list_size = 64; 
list<string> slist(list_size, "eh?"); 
```

#### 9.1.2
可做容器元素的类型； 
- 可赋值。 
- 对象可复制。 

引用类型不支持赋值运算，不能作容器元素。  
除`IO`标准库类型(不支持复制和赋值)和`auto_ptr`类型外，其他标准库类型都可做容器元素，包括容器。  
```
vector< vector<string> > lines;  // cannot ommit space
```

容器操作的要求： 
- 构造函数, 如类型不提供初始化值, 无法只指定容器大小. 

### 9.2
每种容器都有若干迭代器类型.   
所有迭代器具有相同的接口.  

迭代器操作:  
- `*iter`
- `iter->mem`: 等效于`(*iter).mem;`  
- `iter++`
- `iter1 == iter2; `

`vector` 和`deque` 独有的运算:  
- `iter + n`
- `iter1 += iter2`: 
- `iter1 - iter2`: 只适用于`vector` 和`deque`. 
- `>`: 比大小. 只适用于`vector` 和`deque`. 

#### 9.2.1
iterator range(迭代器范围): 是left-inclusive interval(左闭合区间). 要求`first` 经过自增运算能得到`last`.  

#### 9.2.2
修改容器内在状态, 移动元素会导致迭代器失效.  
`erase()` 函数删除元素. 迭代器无意义.    
该类错误难以发现, 尽量使使用某一迭代器的代码简短.  

### 9.3

#### 9.3.1
所有容器定义的类型, 即`typedef`:  
- `size_type`: 无符号整型.  
- `iterator`
- `const_iterator`: 元素只读
- `reverse_iterator`
- `const_reverse_iterator`
- `difference_type`: 存储两迭代器差值的有符号整型.  
- `value_type`: 元素类型. 
- `reference`: 元素的左值类型, 是`value_type&`.  
- `const_reference`: `const value_type&`.  

声明时需用作用域操作符: `list<string>::iterator iter; `  

#### 9.3.2
容器返回迭代器的操作:  
- `c.begin()`
- `c.end()`: 最后一个元素的后一个. 
- `c.rbegin()`: 逆序迭代器的第一个. 
- `c.rend()`

如果容器不是`const`的, 则返回`iterator` 或`reverse_iterator`. 不然返回`const_iterator` 和`const_reverse_iterator`.    

#### 9.3.3
添加元素:  
- `push_back(t)`: 容器尾部添加元素(实参副本)并且长度加1. 返回`void`.  
- `push_front(t)`: 返回`void`, 只适用于`list` 和`deque`.  
- `c.insert(p, t)`: 迭代器`p` 之前插入元素, 返回指向插入元素的迭代器.  
- `c.insert(p, n, t)`: 插入`n`个, 返回`void`.  
- `c.insert(p, b, e)`: 插入迭代器`b` 和`e` 之间的元素.  

```
list<string> lst; 
list<string>::iterator iter = lst.begin(); 
string word; 
while (cin >> word) {
    iter = lst.insert(iter, word); 
}
```

```
string sarray[4] = {"a", "b", "c", "d"}; 
slist.insert(slist.end(), sarray, sarray + 4); 
```

插入元素后指向后一个元素的迭代器一定失效.  

在`vector` 或`deque` 中插入元素后, 假设全部迭代器失效. 因为元素不一定在原内存.   

#### 9.3.4
容器的比较: 
- 只能比较相同的容器类型, 且元素类型也相同 
- 比较元素. 如长度相同且元素也都相同, 则两容器相同. 
- 如对应位置的元素都相同, 但一个容器长度长, 则该容器大于另一个.  
- 不然比较第一个不相等的元素. 

#### 9.3.5
容器大小操作: 
- `c.size()`: 返回类型`c::size_type`. 
- `c.max_size()`: 容器最多可容纳的元素个数.  
- `c.empty()`: 返回布尔值.  
- `c.resize(n)`: 调整容器大小为`n`. 如果容器内已有超过`n`个元素, 删除. 不然补初始值的元素.  
- `c.resize(n, t)`: 补充值为`t`的元素.  

#### 9.3.6
返回元素的引用:  
- `c.back()`: 返回最有一个元素的引用, 等同于`*--c.end()`, 如容器为空则该行为未定义.  
- `c.front()`
- `c[n]`: 只适用于`vector` 和`deque`. 越界时行为未定义.   
- `c.at(n)`: 只适用于`vector` 和`deque`. 下标时抛出`out_of_range` 异常.  

#### 9.3.7
删除元素:  
- `c.erase(p)`: 删除迭代器`p` 所指元素, 返回指向后一个元素的迭代器. `p` 如为`c.end()` 则行为未定义.  
- `c.erase(b, e)`: 删除迭代器`b` 到`e` 之间的元素, 不包括`e`. 返回后一个元素的迭代器, 即`e`.  
- `c.clear()`: 删除所有元素. 返回`void`.  
- `c.pop_back()`: 删除最后一个, 返回`void`. 如果容器为空则未定义.  
- `c.pop_front()`: 返回`void`. 只适用于`list` 和`deque`. 

要用`pop` 获取元素, 需先读取:  
```
while (!ilist.empty()) {
    cout << ilist.front() << endl; 
    ilist.pop_front(); 
}
```

`erase` 不会检查参数有效性, 需确保迭代器不是`end`, 且元素存在.  
```
#include <algorithm>; 

string searchValue("a"); 
list<string>::iterator iter = find(slist.begin(), slist.end(), searchValue); 
if (iter != slist.end()) 
    slist.erase(iter); 
```
调用了`algorithm`头文件中定义的`find(b, e, value)` 函数.  

#### 9.3.8
容器赋值:  
- `c1 = c2;`: 清空容器`c1` 再插入所有`c2` 元素. 不管之前`c1` 长度, 赋值后`c1` 都为`c2` 的长度. 迭代器失效.  
- `c1.swap(c2)`: `c1` 中为`c2` 的元素, `c2` 中为`c1` 的元素. `c1`, `c2` 类型需相同. 比复制操作快. 迭代器不失效且仍指向原元素. 
- `c.assign(b, e)`: 清空`c` 再复制元素. 迭代器`b` 和`e` 必须不指向`c` 中的元素. 容器的迭代去都失效. 元素类型只需兼容即可. 
- `c.assign(n, t)`: `n` 个`t` 元素.  

### 9.4
`vector`中的元素连续存储. 当无可用的内存时, 重新分配内存并复制元素.   
不连续存储元素的容器, 如`list`, 不存在内存分配问题.  
普通情况下`vector`更优. 分配的内存大. 增长效率较高. 

`vector` 容量有关的成员函数:  
- `capacity()`: 能够存储的元素总数.  
- `reserve(n)`: 设置预留存储空间为`n`个.  
- `size()`: 当前实际包含的元素个数.  

### 9.5
`vector` 和`deque` 提供对元素的快速随机访问, 代价是在随机位置插入删除元素比在尾部的开销大.  
`list` 插入, 删除元素的开销小, 但随机访问的开销大, 需遍历.  

`vector` 删除一个元素后该处是hole(空洞), 需挪动之后的元素.  

`deque` 队列两端插入删除数据很快, 且不会使迭代器失效; 但在中间`insert` 或`erase` 效率较低, 且迭代器失效.  

根据所需的操作, 选择合适的容器. 可以先用一种容器进行其擅长的操作然后存储到另一种容器中进行之后的操作.  

### 9.6
`string` 的成员: 
- `size_type`: 无符号整型.  
- `iterator`
- `const_iterator`
- `reverse_iterator`
- `const_reverse_iterator`
- `difference_type`: 存储两迭代器差值的有符号整型.  
- `value_type`
- `reference`
- `const_reference`: `const value_type&`.  

`string` 的基本操作:  
- `string s; `: 定义空`string`对象.  
- `string s(cp); `: 用C风格字符串`cp`初始化.  
- `string s(s2); `: 用`string`对象`s2`初始化.  
- `is >> s; `: 输入流`is`.  
- `s << os; `: 输出流`os`.  
- `getline(is, s); `
- `s1 += s2`
- `==`, `!=`, `<` 等关系符.  

`string` 和`vector` 类似的操作:  
- `string s(b, e); `: `b` 和`e` 是标示开始和结束的迭代器。
- `string s(n, c); `: 用`n` 个`c` 字符。
- `s.insert(p, c)`: 迭代器`p` 之前插入字符`c`. 
- `s.insert(p, n, c)`: 插入`n`个`c`, 返回`void`.  
- `s.insert(p, b, e)`: 插入迭代器`b` 和`e` 之间的字符.  
- `s.size()`: 返回类型`string::size_type`. 
- `s.max_size()`: 最多可容纳的字符个数.  
- `s.empty()`: 返回布尔值.  
- `s.resize(n)`: 调整`string`大小为`n`. 
- `s.resize(n, t)`: 补充值为`t`的字符.  
- `s[n]`
- `s.at(n)`
- `s.begin()` 
- `s.end()`
- `s.clear()`: 返回`void`.  
- `s.erase(p)`: 删除迭代器`p` 所指字符, 返回指向后一个字符的迭代器. 
- `s.erase(b, e)`: 删除迭代器`b` 到`e` 之间的字符, 不包括`e`. 返回后一个字符的迭代器, 即`e`.  
- `s1 = s2;`
- `s1.swap(s2)`
- `s.assign(b, e)`
- `s.assign(n, t)`
- `s.capacity()`
- `s.reserve(n)`

可将`string`类型看作字符容器, 与`vector` 类似.  

#### 9.6.1
`string` 类的构造函数:  
- `string s; `
- `string s(cp); `: 例子: `char *cp = "Hiya"; `. 结果的`null`不会被复制.   
- `string s(cp, n); `: 
- `string s(s2); `
- `string s(s2, pos2); `: 从下标`pos2`的字符开始复制. 如越界, 则行为未定义. `pos2`为`unsigned` 类型.   
- `string s(s2, pos2, len2); `: 从下标`pos2`的字符开始复制`len2`个字符, 或到结尾. `len2`为`unsigned` 类型.   
- `string s(b, e); `
- `string s(n, c); `

注意这不是C风格字符串, 如果用该指针初始化`string` 而不设置计数器, 会导致`runtime error: no_null`.  
```
char array[] = {'a', 'p', 'p', 'l', 'e'}; 
string s(array, 3); // app
```

#### 9.6.2
- `s.insert(p, c)`
- `s.insert(p, n, c)`
- `s.insert(p, b, e)`
- `s.assign(b, e)`
- `s.assign(n, t)`
- `s.erase(p)`
- `s.erase(b, e)`
- `s.insert(pos, n, c)`: 下标`pos`.  
- `s.insert(pos, s2)`
- `s.insert(pos, s2, pos2, len2)`: 从下标`pos`开始, 插入`s2`从`pos2`开始的`len2`个字符.  
- `s.insert(pos, cp)`: `cp`指向的数组需以`null`结尾.  
- `s.insert(pos, cp, len)`
- `s.assign(s2)`: 用`s2`的副本替换`s`.  
- `s.assign(s2, pos2, len2)`
- `s.assign(cp)`
- `s.assign(cp, len)`
- `s.erase(pos, len)`

上述操作都返回`s`的应用.  

#### 9.6.3
只适用于`string` 的操作:  
1. 返回子串的操作`substr`: 

- `s.substr(pos, n)`: 从下标`pos`开始的`n`个字符.  
- `s.substr(pos)`: 到结尾.  
- `s.substr()`: 返回副本.  


2. `append` 和`replace` 函数:  

- `s.append(args)`: 将`args` 接于`s`后. 返回`s`的引用.  
- `s.replace(pos, len, args)`: 用`args` 替换`pos`开始`len`长的字符串. 此处`args`不能为两个迭代器.   
- `s.replace(b, e, args)`: 迭代器`b`和`e`之间的字符串用`args`替换. 此处`args`不能为"字符串和下标和长度对".  

其中`args` 可为: 
- `s2`
- `s2, pos2, len2`
- `cp`: 指向C风格字符串的指针.  
- `cp, len2`
- `n, c`: `n`个`c`字符.  
- `b2, e2`: 两迭代器.  

#### 9.6.4
`string`类型的查找函数: 返回`string::size_type`类型的值或`string::npos`这一特殊值(大于任何有效下标).  
- `s.find(args)`: `args`第一次出现的位置.  
- `s.rfind(args)`: 最后一次.  
- `s.find_first_of(args)`: `args`中的任意字符第一次出现的位置.  
- `s.find_last_of(args)`
- `s.find_first_not_of(args)`: 第一个不属于`args`的字符.  
- `s.find_last_not_of(args)`

每个函数都有4个重载版本, 取决于`args`:  
- `c, pos`: 从`pos`开始查找字符`c`. `pos`有默认值.  
- `s2, pos`
- `cp, pos`: C风格字符串的指针.  
- `cp, pos, n`: `cp`指向数组的前`n`个字符, 此时`pos`和`n`都没有默认值.  

区分大小写.   

查找到所有匹配的字符的位置:  
```
string numerics("0123456789"); 
string name("r2d2"); 

string::size_type pos = 0; 
while ((pos = name.find_first_of(numerics, pos)) != string::npos) {
    cout << pos << endl; 
    ++pos; 
}
```
注意每次循环`pos`一定要加1.  

#### 9.6.5
`string`对象的关系符比较采用字典顺序比较.  

`compare`函数, 类似于C语言库函数`strcmp`:  
- `s.compare(s2)`
- `s.compare(pos1, n1, s2)`: `s`从`pos1`开始的`n1`个字符去比较.  
- `s.compare(pos1, n1, s2, pos2, n2)`: 和`s2`从`pos2`开始的`n2`个字符比.  
- `s.compare(cp)`
- `s.compare(pos1, n1, cp)`
- `s.compare(pos1, n1, cp, n2)`: `cp`所指数组的前`n2`个字符.  

比较结果有: 
- 正数, 表示`s`大.  
- 负数
- 0

### 9.7
顺序容器适配器, 使用前需包含对应的头文件:  
- `queue`: 头文件`queue`
- `priority_queue`: 头文件`queue`
- `stack`:头文件`stack`

容器Adaptor(适配器): 让已存在的容器类型用一种抽象类型的工作方式实现.  

容器适配器通用操作和类型:  
- `size_type`
- `value_type`
- `container_type`: 基础容器的类型.  
- `A a; `: 创建空适配器.  
- `A a(c); `: 初始化为容器`c`的副本.  
- 关系操作符.  

默认`stack`和`queue`基于`deque`实现, `priority_queue`基于`vector`实现.  
可以指定基础容器的类型:  
```
stack< string, vector<string> > str_stk(svec); 
```
约束条件:  
- `stack`可以用任何顺序容器实现, 即`vector`, `list`, `deque`.  
- `queue`要有`push_front()`支持, 所以只能用`list`.  
- `priority_queue`要有随机访问, 可用`vector`和`deque`.  

适配器的大小比较由元素依次比较实现.  

#### 9.7.1
栈适配器的操作:  
- `s.empty()`
- `s.size()`
- `s.pop()`: 删除栈顶元素, 但不返回值.  
- `s.top()`: 返回栈顶值, 不删除.  
- `s.push(item)`: 压入栈顶.  

就算是基于`deque`实现, `deque`的操作不能使用.  

#### 9.7.2
priority queue(优先级队列): 将新元素放于优先级低于它的元素前面.  
默认使用`<`操作符来决定两元素间的优先级关系. 大的元素优先级高.   

`queue`和`priority_queue`的操作:  
- `q.empty()`
- `q.size()`
- `q.pop()`: 删除队首.  
- `q.front()`: 返回队首元素, 只适用于`queue`.  
- `q.back()`: 队尾, 只适用于`queue`.  
- `q.top()`: 最高优先级的元素. 只适用于`priority_queue`.  
- `q.push(item)`: `queue`插入队尾, `priority_queue`插入优先级低于其的元素之前.  


## Chapter 10
Associative container(关联容器):  
- `map`
- `set`
- `multimap`: 允许出现相同的键.  
- `multiset`

### 10.1
`pair`类型在`utility`头文件中定义:  
- `pair<T1, T2> p1;`: 初始化空`pair`对象.  
- `pair<T1, T2> p1(v1, v2);`: `first`成员为`T1`类型的`v1`, `second`成员为`T2`类型的`v2`.  
- `make_pair(v1, v2)`: 创建新的`pair`对象.  
- `p1 < p2`: 按序比较元素.  
- `p1 == p2`
- `p.first`: 是公有的.  
- `p.second`: 是公有的.  

`pair`是模板类型.  

用`typedef`简化声明:  
```
typedef pair<string, string> Author; 
Author joyce("James", "Joyce"); 
```

### 10.2
关联容器支持的通用容器操作:  
- `C<T> c;`
- `C<T> c1(c2);`
- `C<T> c(b, e);` 
- 关系符
- `c.begin()`
- `c.end()`: 最后一个元素的后一个. 
- `c.rbegin()`: 逆序迭代器的第一个. 
- `c.rend()`
- `size_type`: 无符号整型.  
- `iterator`
- `const_iterator`: 元素只读
- `reverse_iterator`
- `const_reverse_iterator`
- `difference_type`: 存储两迭代器差值的有符号整型.  
- `value_type`: 描述键值对的`pair`类型. 
- `reference`: 元素的左值类型, 是`value_type&`.  
- `const_reference`: `const value_type&`.  
- `c1 = c2;`
- `c1.swap(c2)`: `c1`, `c2` 对换.  
- `c.erase(p)`: 返回`void`. 
- `c.erase(b, e)`: 返回`void`. 
- `c.clear()`: 返回`void`.  
- `c.size()`: 返回类型`c::size_type`. 
- `c.max_size()`
- `c.empty()`: 返回布尔值.  

其他操作与键有关.  
迭代遍历关联容器时, 按键的顺序访问.  

### 10.3

#### 10.3.1
`map`(关联数组)定义于`map`头文件中:  
- `map<k, v> m; `
- `map<k, v> m(m2); `
- `map<k, v> m(b, e);`: 创建副本. 迭代器所指的元素须能转换为`pair<const k, v>`.  

对键类型的唯一约束: 能够比较大小.  
键的比较函数: 
- 默认情况下使用`<`关系符.  
- 必须在键类型上定义strict weak ordering(严格弱排序): 自己与自己比返回`false`; 不存在互相小于; 如互相不小于, 则为等于.   

#### 10.3.2
`map`类定义的类型:  
- `map<K, V>::key_type`: 键的类型.  
- `map<K, V>::mapped_type`: 值的类型.  
- `map<K, V>::value_type`: `pair`类型, `first`是`const map<K, V>::key_type`类型, `second`是`map<K, V>::mapped_type`类型.  

其中值成员可以修改, 但键成员不可.  

对迭代器解引用, 得到指向`value_type`类型的引用.  
```
map<string, int>::iterator map_it = word_count.begin(); 
cout << map_it->first << endl; 
++map_it->second; 
```

`map<string, int>::key_type`可获得类型成员.  

#### 10.3.3
添加元素到`map`: `insert()`或用下标获取元素并赋值. 

#### 10.3.4
用下标访问`map`中不存在的元素会添加一个元素, 其键为下标值, 值为初始化值.  
```
map<string, int> word_count; 
word_count["Anna"] = 1; // add a new pair (Anna, 0), then assign value as 1

string word; 
while (cin >> word)
    ++word_count["word"]; // a new word will always has count as 1
```

下标操作符返回左值, 即键关联的值. 与迭代器解引用返回的不同.  

#### 10.3.5
`map`的`insert`成员:  
- `m.insert(e)`: `e`是`m`上的`value_type`类型. 返回`pair`类型的对象, 包含指向`e.first`的`map`迭代器和表示是否插入元素的`bool`类型.  
- `m.insert(beg, end)`: 迭代器`beg`和`end`包括的元素需是`m.value_type`类型的. 返回`void`.  
- `m.insert(iter, e)`: 如`e.first`不存在, 则创建新元素, 并以`iter`为起点搜索新元素的存储位置. 返回指向`e`元素的迭代器.  

```
word_count.insert(map<string, int>::value_type("Anna", 1)); 
word_count.insert(make_pair("Anna", 1)); 

typedef map<string, int>::value_type valType; 
word_count.insert(valType("Anna", 1)); 
```

如果试图`insert`的元素的键已存在, 则不做任何操作. 这时返回的`bool`值为`false`.  
```
map<string, int> word_count; 
string word; 
while (cin << word) {
    pair< map<string, int>::iterator, bool > ret = word_count.insert(make_pair(word, 1)); 
    if (!ret.second)
        ++ret.first->second; 
}
```

#### 10.3.6
判断元素是否存在:  
- `m.count(k)`: 返回`m`中`k`出现的次数.  
- `m.find(k)`: 如存在, 返回指向该元素的迭代器, 不然返回超出末端迭代器.  

```
int occurs; 
map<string, int>::iterator it = word_count.find("footbar"); 
if (it != word_count.end()) 
    occurs = it.second; 
```

#### 10.3.7
删除元素:  
- `m.erase(k);`: 删除键为`k`的元素, 返回`size_type`类型的值, 表示删除的元素个数.  
- `m.erase(p);`: 删除迭代器`p`所指, 返回`void`.  
- `m.erase(b, e);`: 删除从迭代器`b`到`e`之前一个元素. 返回`void`.  

#### 10.3.8
遍历, 按键的升序排列:  
```
map<string, int>::const_iterator map_it = word_count.begin(); 
while (map_it != word_count.end()) {
    cout << map_it->first << " " << map_it->second << endl; 
    ++map_it; 
}
```

#### 10.3.9
一个荔枝:  
```
#include <iostream>
#include <fstream>
#include <sstream>
#include <map>
#include <string>

using namespace std; 

ifstream& open_file(ifstream &in, char *file) {
	in.close(); 
	in.clear(); 
	in.open(file); 
	return in; 
}

int main(int argc, char **argv)
{
	if (argc != 4)
		throw runtime_error("wrong number of arguments"); // Not handle here
	
	map<string, string> trans_map; 
	string key, value; 

	ifstream map_file; 
	if (!open_file(map_file, argv[1]))
		throw runtime_error("no transformation file"); 
	while (map_file >> key >> value) 
		trans_map.insert(make_pair(key, value)); 

	ifstream input; 
	if (!open_file(input, argv[2]))
		throw runtime_error("no input file"); 
		
	string line; 
	while (getline(input, line)) {
		istringstream stream(line); 
		string word; 
		bool first_word = true; 
		
		while (stream >> word) {
			map<string, string>::const_iterator map_it = trans_map.find(word); 
			if (map_it != trans_map.end())
				word = map_it->second; 

			if (first_word)
				first_word = false;
			else
				cout << " "; 
			cout << word; 
		}

		cout << endl; 
	}
	
	return 0; 
}
```

### 10.4
`set`不支持下标操作符.  
`set`的`value_type`是`key_type`类型.  

#### 10.4.1
`set`定义于`set`头文件中.  

初始化:  
```
vector<int> ivec; 
set<int> iset(ivec.begin(), ivec.end()); 
```

添加元素:  
```
iset.insert(1); // return a pair <iterator, bool>
iset.insert(ivec.begin(), ivec.end()); // return void
```

获取元素: 不能通过下标访问  
```
iset.find(1); // return iterator
iset.count(1); // return size_type
```
键为`const`的, 不能赋值.  

#### 10.4.2
`set`中的键都只出现一次.  

### 10.5
`multimap`和`multiset`也定义于`map`和`set`头文件中.  

#### 10.5.1
插入元素:  
```
author.insert(make_pair("John", "Book a")); 
author.insert(make_pair("John", "Book b")); // same key can still insert
```

删除元素:  
```
string search_item("john"); 
multimap<string, string>::size_type cnt = author.erase(search_item); // return the number it delete
```

#### 10.5.2
相同键的元素相邻存放.  

查找元素时会出现多个元素.  
1. 使用`find`和`count`: 
```
string search_item("John"); 
typedef multimap<string, string>::size_type sztype; 
zstype entries = author.count(search_item); 
multimap<string, string>::iterator iter = author.find(search_item); 
for (sztype cnt = 0; cnt != entries; ++cnt, ++iter) {
    cout << iter->second << endl; 
}
```

2. 使用关联容器的特定操作:  
- `m.lower_bound(k)`: 返回指向键不小于`k`的第一个元素的迭代器.  
- `m.upper_bound(k)`: 返回指向键大于`k`的第一个元素的迭代器.  
- `m.equal_range(k)`: 返回pair类型, `first`为`m.lower_bound(k)`, `second`为`m.upper_bound(k)`. 如键`k`不存在, 返回都指向`m.upper_bound(k)`的`pair`.  

```
multimap<string, string>::iterator beg = author.lower_bound(search_item), end = author.upper_bound(search_item); 
while (beg != end) {
    cout << beg->second << endl; 
    ++beg; 
}
```

3. 使用`equal_range()`:  
```
typedef multimap<string, string>::iterator authors_it; 
pair<authors_it, authors_it> pos = author.equal_range(search_item); 
while (pos.first != pos.second) {
    cout << pos.first->second << endl; 
    ++pos.first; 
}
```

### 10.6

#### 10.6.1
设计程序:  
