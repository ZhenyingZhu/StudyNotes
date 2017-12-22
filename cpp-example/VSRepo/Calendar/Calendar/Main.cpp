#include <iostream>
int main()
{
	int mon;
	int start, monday;
	int week=1;
	std::cout<<"2013年日历"<<std::endl<<"请输入月份"<<std::endl;
	std::cin>>mon;
	if(mon>=1 && mon<=12)
	{
		std::cout<<"日"<<'\t'<<"一"<<'\t'<<"二"<<'\t'<<"三"<<'\t'<<"四"<<'\t'<<"五"<<'\t'<<"六"<<'\t'<<std::endl;
		start=6; monday=31;
		for(int j=1;j<start;++j)
		{
			std::cout<<" "<<'\t';
			++week;
		}
		for(int i=1;i<=monday;++i)
		{
			if(week<=7)
			{
				std::cout<<i<<'\t';
				++week;
			}
			else
			{
				std::cout<<std::endl;
				week=1;
			}
		}
		std::cout<<std::endl<<mon<<"月的月历哦"<<std::endl;
	}
	else
	{
		std::cout<<"你开玩喜啊？"<<std::endl;
	}
	return 0;
}