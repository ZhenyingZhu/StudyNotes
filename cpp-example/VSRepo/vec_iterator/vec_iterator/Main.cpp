#include <iostream>
#include <vector>

using std::cin;
using std::cout;
using std::endl;
using std::vector;

int main()
{
	vector<int> ivec;
	int inp;
	while(cin>>inp)
		ivec.push_back(inp);
	for(vector<int>::iterator iter = ivec.begin(); iter != ivec.end(); ++iter)
		*iter = 0;
	for(vector<int>::iterator iter = ivec.begin(); iter != ivec.end(); ++iter)
		cout<<*iter<<' ';
	cout<<endl;

	return 0;
}