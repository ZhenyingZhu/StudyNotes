#include <iostream>
#include <string>

using std::string;
using std::cin;
using std::cout;
using std::endl;

int main()
{
/*	string s1, s2;
	cin>>s1>>s2;
	cout<<s1<<' '<<s2<<endl;
*/

/*	string line;
	while(getline(cin,line))
		cout<<line<<endl;
*/

/*	string word;
	while (cin>>word)
		cout<<word<<endl; 
*/

/*	string st("the expanse of spirit. \n");
	cout<<"size is "<<st.size()<<endl;
*/

/*	string str("some string");
	for(string::size_type ix = 0; ix != str.size(); ++ix)
		cout<<str[ix]<<endl;
*/

	string stri ("How many spaces?");
	string::size_type space_cnt = 0;
	for(string::size_type ix = 0; ix != stri.size(); ++ix)
		if (isspace(stri[ix])) space_cnt++;
	cout<<space_cnt<<endl;

	return 0;
}