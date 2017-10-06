#include <iostream>

using namespace std;

namespace HasPtr {
	// C++ primer 5th E, Chapter 12
class MyClass {
public:
	MyClass(int n = 0):
		num(n) {
		cout << "constructor" << endl;
	}

	MyClass(const MyClass &other) {
		cout << "copy constructor" << endl;
		num = other.num;
	}

	~MyClass() {
		cout << "destructor" << endl;
	}

	int getNum() {
		return num;
	}

	void setNum(int n) {
		num = n;
	}

private:
	int num;
};

class HasPtr {
public:
	HasPtr(const MyClass &s = MyClass()):
		ps(new MyClass(s)), i(0) {
	}

	HasPtr(const HasPtr &p):
		ps(new MyClass(*p.ps)), i(p.i) {
	}

	//HasPtr& operator=(const HasPtr &);

	~HasPtr() {
		delete ps;
	}

	int getNum() {
		return ps->getNum();
	}

private:
	MyClass * ps;
	int i;
};

int HasPtrMain() {
	MyClass s;

	HasPtr p1(s);

	cout << "get num from p1 " << p1.getNum() << endl;

	HasPtr p2(s);
	s.setNum(1);
	cout << "get num from p1 " << p1.getNum() << endl;
	cout << "get num from p2 " << p2.getNum() << endl;

	return 0;
}

}