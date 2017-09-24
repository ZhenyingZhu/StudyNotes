#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <array>

using namespace std;

const size_t ARRAY_SIZE = 3;

class GameLife {
public:
    GameLife(string filename):
        generation(0) {
        readInitState(filename);
    }

    void start();

    void update();

    void printLife() {
        for (size_t i = 1; i <= ARRAY_SIZE; i++) {
            for (size_t j = 1; j <= ARRAY_SIZE; j++) {
                cout << L[i][j] << " ";
            }
            cout << endl;
        }
    }

private:
    void readInitState(string filename) {
        string line;

        ifstream fs(filename);

        // first line contains 1. total generation and 2. frequency
        getline(fs, line);
        istringstream iss(line);
        iss >> totalGeneration;
        iss >> frequency;

        // following lines are the init state of the matrix
        for (size_t i = 1; i <= ARRAY_SIZE; i++) {
            getline(fs, line);
            for (size_t j = 1; j <= ARRAY_SIZE; j++) {
                if (line[j - 1] == '*')
                    L[i][j] = 1;
                else
                    L[i][j] = 0;
            }
        }

        fs.close();
    }

private:
    int totalGeneration;
    int frequency;
    int generation;
    int L[ARRAY_SIZE + 2][ARRAY_SIZE + 2];
};

int main() {
    GameLife solution("hw3b.dat");
    solution.printLife();
    solution.update();
}

