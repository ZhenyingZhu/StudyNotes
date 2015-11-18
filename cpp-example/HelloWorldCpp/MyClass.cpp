/*
 * MyClass.cpp
 *
 *  Created on: Nov 18, 2015
 *      Author: zhu91
 */

#include <string>
#include "MyClass.h"

using std::string;

void MyClass::setContent(string newContent){
	this->content = newContent;
}

string MyClass::getContent() const{
	return this->content;
}
