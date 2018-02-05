#include <iostream>
#include <fstream>
#include <string>
#include <vector>

using namespace std;

string loadFileNotWork(const char filename[]) {
	ifstream f(filename);
    string contents = "";
    string temp = "";
	// before f.eof(), there is still a char there.
    while (!f.eof()) {
        f >> temp;
        cout << temp << endl;
        contents += temp;
        temp = "";
    }
    return contents;
}

string loadFile(const char filename[]) {
    ifstream f(filename);
	
    vector<char> res;
    char c;
    while(f.get(c)) {
        res.push_back(c);
    }
	
    f.close();

    return string(res.begin(), res.end());
}

int main(int argc, char* argv[]) {
        cout << loadFile(argv[1]) << endl;

        return 0;
}
