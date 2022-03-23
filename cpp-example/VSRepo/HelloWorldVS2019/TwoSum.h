#pragma once

#include <vector>

class TwoSum
{
public:
	TwoSum() {}

	~TwoSum() {}

	std::vector<int> twoSum(std::vector<int>& nums, int target);

	std::vector<int> twoSumBruteForce(std::vector<int>& nums, int target);
};

