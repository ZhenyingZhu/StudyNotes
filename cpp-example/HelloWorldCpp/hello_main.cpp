/*
 * hello_main.cpp
 *
 *  Created on: Nov 16, 2015
 *      Author: zhu91
 */

#include <iostream>
#include "MyClass.h"

using std::cout;
using std::endl;

int main() {
	cout << "Hello World! " << endl;
	MyClass mc;
	cout << mc.getContent() << endl;
	mc.setContent("New Content. ");
	cout << mc.getContent() << endl;

	return 0;
}
