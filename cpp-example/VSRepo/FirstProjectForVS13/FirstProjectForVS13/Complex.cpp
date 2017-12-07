#include <iostream>
#include "Complex.h"

using namespace std;

int main() {
	Complex<int> c(1, -2);
	cout << c.abs() << endl;

	Complex<int> x(c);
	cout << x << endl;

	Complex<int> r = -c;
	cout << r << endl;

	//Complex<char> r;

	return 0;
}
