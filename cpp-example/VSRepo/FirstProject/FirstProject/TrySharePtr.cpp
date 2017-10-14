#include <iostream>
#include <memory>

using namespace std;

namespace TrySharePtr {
	class MyClass {
	public:
		// the num is using property
		MyClass(int num = 0) : num(num) {
			cout << "Construct MyClass " << num << endl;
		}

		MyClass(const MyClass& other) : num(other.num) {
			cout << "Copy Construct MyClass " << num << endl;
		}

		void setNum(int num) {
			// num = num doesn't work, need this
			this->num = num;
		}

		int getNum() {
			return num;
		}

		~MyClass() {
			cout << "Destory MyClass " << num << endl;
		}

	private:
		int num;
	};

	void TestSharedPtrMain() {
		shared_ptr<MyClass> p1(new MyClass);
		MyClass &mc = *p1;
		p1->setNum(1);
		cout << mc.getNum() << endl;
		
		shared_ptr<MyClass> p2(new MyClass(2));
		p2 = p1;

		p2->setNum(3);
		cout << mc.getNum() << endl;
	}

	void TestUniquePtrMain() {
		MyClass mc(1);
		unique_ptr<MyClass> p 
	}
}

int main() {
	TrySharePtr::TestUniquePtrMain();

	return 0;
}