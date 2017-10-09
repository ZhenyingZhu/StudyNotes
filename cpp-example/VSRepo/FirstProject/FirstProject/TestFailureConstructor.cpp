#include <iostream>

using namespace std;

namespace TestFailureConstructor {
class MyProperty {
public:
	MyProperty(int x) : num(x) {
	}

private:
	int num;
};

class MyClass {
public:
	MyClass(const MyProperty& x) : x(x) {
		cout << "initialize the object" << endl;
	}

	//MyClass() : x(0) {
	//	cout << "0" << endl;
	//}

	//MyClass(const MyClass& c) {
	//	x = c.x;
	//	cout << "copy" << endl;
	//}

	//MyClass& operator=(const MyClass& c) {
	//	x = c.x;
	//	cout << "=" << endl;
	//	return *this;
	//}

	//int get() {
	//	return x;
	//}
private:
	MyProperty x;
};

int TestFailureConstructorMain() {
	/*	MyClass c1(1);
	MyClass c2(c1);

	int x1 = c1.get();
	int x2 = c2.get();
	cout << x1 << " " << x2 << endl;

	MyClass c3 = c1;
	cout << c3.get() << endl;

	MyClass c4(4);
	c3 = c4;
	cout << c3.get() << endl;
	*/

	int x = 5;
	MyClass c5 = x;
	//MyClass c5 = MyClass(5);
	MyClass c6 = c5;
	//cout << c5.get() << endl;

	return 0;
}
}