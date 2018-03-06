#include <iostream>
#include <vector>

using namespace std;

class MyClass {
public:
    MyClass() {
        int val = 0;
        for (int i = 0; i < 2; i++) {
            myVec.push_back(vector<int>());
            for (int j = 0; j < 2; j++) {
                myVec[i].push_back(val++);
            }
        }
    }

    int getValue(int col, int row) {
        return myVec[col][row];
    }

private:
    vector<vector<int>> myVec;
};

int main() {
    MyClass mc;

    for (int i = 0; i < 2; i++) {
        for (int j = 0; j < 2; j++) {
            cout << mc.getValue(i, j) << " ";
        }
        cout << endl;
    }

    return 0;
}
