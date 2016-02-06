# include <iostream>

using std::string; 

int main() {
    string str("HelloWorld");
    std::cout << str << std::endl;
    if (str.find("Wor") != string::npos) {
        std::cout << "found!" << '\n';
    }
}

