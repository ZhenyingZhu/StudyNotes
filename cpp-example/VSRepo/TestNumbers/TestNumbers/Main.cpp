#include <iostream>
int main()
{
	unsigned int v1;
	double v2;
	char v3;
	wchar_t v4;
//	v1=-1UL;  //give over board value. 
	v1='A';  //give char value. 
	v2=3.14F; //double value.
	v3=97;
	v4='\a';
	std::cout<<v1<<'\n'<<v2<<'\n'<<-1UL<<'\n'<<v3<<'\n'<<v4<<'\b'<<'\b'<<std::endl;
	return 0;
}