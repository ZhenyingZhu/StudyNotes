#include <iostream>
#include <vector>

using std::cout; 
using std::endl; 

int f(int *const p) {

}

int main(int argc, char *argv[])
{
    cout << argc << endl; 
    for (char** beg = argv; beg != argv + argc; ++beg) {
        cout << *beg << endl; 
    }

    return 0; 
}