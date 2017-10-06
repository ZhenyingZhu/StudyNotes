#include <iostream>

using namespace std;

namespace TestConstructor {
class MyClass {
public:
	MyClass(): num(0) {
		cout << "default constructor" << endl;
	}

	MyClass(int n): num(n) {
		cout << "normal constructor with num: " << n << endl;
	}

	MyClass(const MyClass &other) {
		cout << "copy constructor" << endl;
		num = other.num;
	}

	MyClass& operator=(const MyClass &other) {
		cout << "copy assignment operator" << endl;
		num = other.num;
		return *this;
	}

	~MyClass() {
		cout << "destructor" << endl;
	}

	int num;
};

int TestConstructorMain() {
	// MyClass mc1 = MyClass(1); // only run one copy?
	MyClass mc2(2);
	MyClass mc2c = mc2;

	cout << mc2c.num << endl;

	return 0;
}
}