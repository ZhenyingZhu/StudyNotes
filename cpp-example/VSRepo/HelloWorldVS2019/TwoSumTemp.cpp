#include <iostream>
#include <vector>

using namespace std;

class Solution
{
public:
	vector<int> twoSum(vector<int>& nums, int target)
	{
		for (int i = 0; i < nums.size() - 1; ++i)
		{
			for (int j = i + 1; j < nums.size(); ++j)
			{
				if (nums[i] + nums[j] == target)
				{
					return { i, j };
				}
			}
		}

		// Cannot find a pair of numbers sun to the target.
		return { -1, -1 };
	}
};