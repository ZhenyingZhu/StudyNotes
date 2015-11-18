/*
 * MyClass.h
 *
 *  Created on: Nov 18, 2015
 *      Author: zhu91
 */

#ifndef MYCLASS_H_
#define MYCLASS_H_

#include <string>

class MyClass
{
public:
	MyClass(): content("N/A"){};
	void setContent(std::string);
	std::string getContent() const;
private:
	std::string content;
};

#endif /* MYCLASS_H_ */
