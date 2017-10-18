#include <iostream>

using namespace std;

namespace TestDynamicArray {
	int main() {
		//typedef int arrT[42];
		//int i = 5;
		//int *p = new int[i];

		int len = 128;
		int *p1 = new int[len];
		cout << p1[127] << endl;

		for (int i = 0; i != len; i++) {
			p1[i] = 1;
		}

		cout << p1[127] << endl;

		int *p2 = new int[len];
		for (int i = 0; i != len; i++) {
			p2[i] = 2;
		}

		len = 129;

		cout << p1[128] << endl;

		return 0;
	}

}

