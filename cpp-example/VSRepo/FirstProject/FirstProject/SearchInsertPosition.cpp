#include <iostream>
#include <vector>

using namespace std;

class Solution {
public:
	int searchInsert(vector<int>& nums, int target) {
		if (nums.empty())
			return 0;

		int st = 0, ed = nums.size() - 1;
		while (st + 1 < ed) {
			int md = st + (ed - st) / 2;
			if (nums[md] == target) {
				return md;
			}
			else if (nums[md] < target) {
				st = md;
			}
			else {
				ed = md;
			}
		}

		if (nums[st] >= target)
			return st;
		else if (nums[ed] >= target)
			return ed;
		return ed + 1;
	}
};

int SearchInsertPositionMain() {
	vector<int> nums = {1, 2, 3, 4, 5};

	Solution sol;
	for (int i = 0; i <= 6; i++) {
		cout << i << " insert to " << sol.searchInsert(nums, i) << endl;
	}

	return 0;
}