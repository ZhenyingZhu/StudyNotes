#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

namespace TestCreateTree {

class Solution {
public:
	int numTrees(int n) {
		if (n < 3)
			return n;
		int res = 0;
		vector<int> used(n + 1, 0);
		used[0] = 1;

		for (int i = 1; i <= n; i++) {
			createTree(i, n, res, used);
		}

		return res;
	}
	void createTree(int i, int n, int& res, vector<int>& used) {
		cout << "i:" << i << ", used: ";
		for (int pt = 0; pt != used.size(); pt++)
			cout << used[pt] << ", ";
		cout << endl;

		if (find(used.begin(), used.end(), used[0] - 1) == used.end()) {
			res++;
			used[0]++;
			return;
		}
		used[i] = used[0];

		for (int left = i - 1; left > 0; left--) {
			if (used[left] == used[0] - 1) {
				createTree(left, n, res, used);
			}
		}
		for (int right = i + 1; right <= n; right++) {
			if (used[right] == used[0] - 1) {
				createTree(right, n, res, used);
			}
		}
	}
};

int TestCreateTreeMain() {
	Solution sol;
	cout << sol.numTrees(3) << endl;

	return 0;
}

}