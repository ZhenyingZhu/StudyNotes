#include <iostream>
#include "Sales_item.h"

int main()
{
	Sales_item total, trans;
	if(std::cin>>total)
	{
		while(std::cin>>trans)
			if(total.same_isbn(trans))
				total=total+trans;
			else
			{
				std::cout<<total<<std::endl;
				total=trans;
			}
		std::cout<<total<<std::endl;
	}
	else
	{
		std::cout<<"Êä´íÁË°É£¿"<<std::endl;
		return -1;
	}
	return 0;
}