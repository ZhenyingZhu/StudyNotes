#include "TwoSum.h"
#include <unordered_map>

using namespace std;

vector<int> TwoSum::twoSumBruteForce(vector<int>& nums, int target)
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

vector<int> TwoSum::twoSum(vector<int>& nums, int target)
{
	unordered_map<int, int> map;

	for (int i = 0; i < nums.size(); ++i)
	{
		if (map.find(target - nums[i]) != map.end())
		{
			return { map[target - nums[i]], i };
		}

		map[nums[i]] = i;
	}

	// Cannot find a pair of numbers sun to the target.
	return { -1, -1 };
}
