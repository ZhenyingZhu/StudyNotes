#include <iostream>

using namespace std;

namespace TryDerive {
	class MyBase {
	public:
		int base_public;

		void access() {
			base_protected = 1;
			base_private = 1;
		}

	protected:
		int base_protected;

	private:
		int base_private;
	};

	class MyDerived : protected MyBase {
	public:
		void access_self_base() {
			base_public = 1;
			base_protected = 1;
			//base_private = 1;
			access();
		}

		void access_other_base(MyBase &base) {
			base.base_public = 1;
			//base.base_protected = 1;
		}

		void access_other_derive(MyDerived &derived) {
			derived.base_public = 1;
			derived.base_protected = 1;
			//derived.base_private = 1;
		}
	};

	class Base {
	public:
		int x;

		Base(int x = 0, int y = 0, int z = 0) :
			x(x), y(y), z(z) { }

		int virtual getPri() {
			cout << "Base" << endl;
			return z;
		}

	protected:
		int y;

	private:
		int z;
	};

	class PriDerive : public Base {
	public:
		PriDerive(int x = 0, int y = 0, int z = 0) :
			Base(x, y, z) { }

		int getPub() { return x; }

		int getPro() { return y; }

		int virtual getPri() {
			cout << "Derive" << endl;
			return Base::getPri();
		}
	};

	int main() {
		//	PubDerive d(1 ,2, 3);
		//	ProDerive d(1 ,2, 3);
		TryDerive::PriDerive d(1, 2, 3);
		TryDerive::Base& b = d;
		//TryDerive::PriDerive d = TryDerive::PriDerive(1, 2, 3);

		//int a = d.x;
		//int b = d.y;
		//int c = d.z;
		cout << b.getPri() << endl;
		//cout << d.getPri() << endl;

		//MyDerived obj;
		//obj.access_self_base();

		return 0;
	}
}


