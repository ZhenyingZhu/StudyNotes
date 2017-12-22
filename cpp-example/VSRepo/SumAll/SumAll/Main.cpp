#include <iostream>
int main()
{
	int sum = 0, input_value;
	std::cout<<"输入多少数字我都帮你加起来～"<<std::endl;
	while(std::cin>>input_value)
	{
		sum+=input_value;
	}
	std::cout<<"结果是"<<sum<<"。"<<std::endl;

//Test while condition
/*	int show;
	bool value;
	value=(std::cin>>show);
	std::cout<<value<<std::endl;
*/
	return 0;
}