#include <iostream>
#include <vector>

using namespace std;

class MyClass {
public:
    MyClass(int size = 2) {
        for (int i = 0; i < size; i++) {
            myVec.push_back(vector<int>(size));
        }
    }

    void setValue() {
        int val = 0;
        for (int i = 0; i < myVec.size(); i++) {
            for (int j = 0; j < myVec.size(); j++) {
                myVec[i][j] = val++;
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
    mc.setValue();

    for (int i = 0; i < 2; i++) {
        for (int j = 0; j < 2; j++) {
            cout << mc.getValue(i, j) << " ";
        }
        cout << endl;
    }

    return 0;
}
