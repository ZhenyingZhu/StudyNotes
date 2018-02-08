// created by Jiabin.Li

#include <iostream>
#include <vector>

using namespace std;

int computerLength(char* A) {
        char* idx = A;
        int n = 0;
        while (*idx != '\0') {
                n++;
                idx++;
        }

        return n;

}


int editDistance(char* A, char* B) {
	int a_len = computerLength(A);
	int b_len = computerLength(B);	
	cout << a_len << endl;
	cout << a_len << ' ' << b_len << endl;
	if (a_len == 0)
		return b_len;

	if (b_len == 0)
		return a_len;
	
	vector<vector<int> > dp;
        for(int i = 0; i < a_len + 1; i++) {
            vector<int> one(b_len + 1, 0);
            dp.push_back(one);
        }
	for (int i = 0; i < a_len + 1; i++) {
		for(int j = 0; j < b_len + 1; j++) {
			cout << dp[i][j] << ' ';
		}
		cout << endl;
	}		

	for(int i = 0; i < a_len + 1; i++) {
            dp[i][0] = i;
        }
        for(int j = 0; j < b_len + 1; j++) {
            dp[0][j] = j;
        }

//	char* idx1 = A ;
//	char* idx2 = B ;
	for (int i = 1; i < a_len + 1; i++) {
		for (int j = 1; j < b_len + 1; j++) {
			if (*(A + i -1) == *(B + j - 1)) {
				dp[i][j] = dp[i - 1][j - 1];
			}else {
				dp[i][j] = 1 + min(dp[i - 1][j - 1], min(dp[i - 1][j], dp[i][j - 1]));
			}
						
			if (i == a_len) {
				cout << *(A + i -1) << ' ' << *(B + j - 1) << '\n';
				cout << dp[i][j] << endl;
			}	
		}
		
		cout << dp[a_len][0] <<endl;
	}
	
        
        return dp[a_len][b_len];

}


int main() {
    char A[] = "hello";
//	char A[] = {'h', 'e', 'l', 'l', 'o'};
//	char B[] = {'h', 'e', 'r', 'o'};
    char B[] = "hero";
//	cout << computerLength(A) << ' ' << computerLength(B) << '\n';
	cout << editDistance(A, B) << endl;

	return 0;
}
