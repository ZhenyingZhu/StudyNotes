#include <iostream>
int main()
{
	int v1, v2;
	std::cout<<"为比大小，请输2数"<<std::endl;
	std::cin>>v1>>v2;
	int lower, upper;
	if(v1<=v2)
	{
		lower=v1;
		upper=v2;
	}
	else
	{
		lower=v2;
		upper=v1;
	}
	std::cout<<"小数是"<<lower<<",而大数是"<<upper<<"。"<<std::endl;
	return 0;
}