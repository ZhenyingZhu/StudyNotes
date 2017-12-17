#include <iostream>
#include <string>

void reverse(char *str) {
	char *end = str;
	char tmp;
	if (str) { //Is this pointer
		while (*end) { // Is the content
			//std::cout<<end<<std::endl;
			//std::cout<<*end<<std::endl;
			++end;
		}
		--end;
		while (str < end) {
			tmp = *str;
			*str++ = *end;
			*end-- = tmp;
		}
	}
}
int main()
{
	using namespace std;
	string input;
	cin>>input;
	char *str=&input[0];
	reverse(str);
	cout<<input<<endl;
	return 0;
}
