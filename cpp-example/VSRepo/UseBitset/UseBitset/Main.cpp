#include <iostream>
#include <string>
#include <bitset>

using std::string;
using std::bitset;
using std::cin;
using std::cout;
using std::endl;

int main()
{
	long bi = 18;
	bitset<8> b(bi);
	unsigned long tranlong = b.to_ulong();
	cout<<tranlong<<endl;


/*	string str("01111");
	bitset<8> b(str,str.size()-2);
	size_t size = b.count();
	cout<<b<<endl;
*/

	return 0;
}