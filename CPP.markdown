# C++ Primer 中文版
## Chapter 1
### 1.1
主函数
```
int main()
{
    return 0; # Means success
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
    std::cout << "Enter two numbers: " << std::endl; # std::endl is one of manipulators
    int v1, v2; 
    std::cin >> v1 >> v2; 
    std::cout << "Sum :" << v1 + v2 << std::endl; 
}
```

`<<`操作符：每次接受两个操作数, 左边为ostream对象, 右边为内容。该表达式执行完后, 返回void*的ostream对象。  
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
字符串：`cout<<"a" "b""c"`可以用空格、回车、tab连起来。在末尾自动添加空字符：'A' 是一个字符，而"A" 是'A', '\12' 两个字符。

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
=====================
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
refVal += 2; 
```
Nonconst reference.  

### 2.6
定义类型的同义词： 
```
typedef double wages; 
wages salary; # salary is a double value
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

=====================

头文件：连接程序中名字的使用和声明。包含类的定义, extern变量的声明, 函数的声明。
预防多次包含同一头文件：
	#ifndef SALESITEM.H
	#define SALESITEM.H
	//Define class. 
	#endif

using声明(命名空间：：名字)：using std::cin; 
C++继承C中name.h为cname。

标准库类型(非基本类型)：需#include <>; using std:: ;
string
	初始化：string s1; string s2 (s1); string s1 ("value"); string s4(n,’c’); 
	输入：从非空字符开始, 到空格结束。
	读入一整行：while(getline(cin,line)) cout<<line<<endl; 
	一字符串st, st.size()是一个量(st中包含空格的字符数), 是标准库类型string::size_type类型储存的, 可认为是无符号长整型, 不能赋给int型。
	字符串比大小：依次比较每个字符大小。如之前都一样, 短字符串小于长字符串。
	下标操作符[]和一个size_type类型数访问单个字符。下标从0开始。下标可以是任何整型值。上下界为0到str.size()-1。
 
测试字符串中单个字符的函数, 返回一个int值, 失败为0, 其他为非0值。
tolower, toupper返回的是字符。
 
vector：容器, 可包含同一类型其他对象的集合。
初始化：vector<int> ivec; vector<T> v1; vector<T> v2(v1); vector<T> v3(n, i); vector<T> v4(n) //v4需有值。vector最好动态添加元素。初始值根据元素类型决定。
vector的操作：
v.size()返回的是vector<T>::size_type类型。
添加元素：v.push_back()。
 
下标操作ivec[n]不能添加元素。
	迭代器：vector<int>::iterator iter; 标准库容器类型都有iterator成员。
	begin 和end：vector<int>::iterator iter = ivec.end(); 指向最后元素的下一个位置。::const_iterator 只能写。
	解引用操作符：*iter=0; 等同于ivec[iter]=0。访问迭代器指向的元素。
	迭代器操作：++和==和!=。
	迭代器间距离：iter1-iter2, 由difference_type储存, 是signed。
	中间元素：vector<int>：：iterator mid = vi.begin()+vi.size()/2; 

bitset：位操作。bitset<32> bitvec; 位数只能是整型数或常量。位序从0开始, 为最低阶位。
 
bitset 对象初始化：bitset<32> bitvec2(0xffff); 0~15位为1, 16～32位为0。string strval("1100"); bitset<32> bitvec4(strval); 则bitvec4为00110000, 从右向左读入。bitset<32> bitval5(str, 5, 4); 从str的第5位起读入4个字符。bitset<32> bitval6(str, str.size()-4); 省略第三个参数, 则读入从第二个参数起到结尾的字符。 
	测试操作：bool is_set = bitvec.any(); 返回1为true。
	位个数类型为size_t, 需#include <cstddef>。
	将二进制数返回为长整型：unsigned long number = b.to_ulong(); 
