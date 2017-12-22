#include <iostream>
#include <string>
#include <vector>

using std::vector;
using std::string;
using std::cin;
using std::cout;
using std::endl;

int main()
{
	vector<int> arr;
	int	ele;
	while (cin>>ele)
		arr.push_back(ele);
	vector<int>::size_type siz,si;
	siz=arr.size();
	si=siz/2;
	if((siz-2*si)!=0)
	{
		siz=siz-1;
		cout<<"Last element doesn't caculate."<<endl;
	}
	for(vector<int>::size_type i=0;i!=siz;i=i+2)
	{
		cout<<arr[i]+arr[i+1]<<endl;
	}

/*	vector<string> text;
	string word;
	while (cin>>word)
		text.push_back(word);
	cout<<text[0]<<endl;
*/

/*	vector<int> v4(3);
	v4[1]=4;
	cout<<v4[0]<<endl;
*/

	return 0;
}