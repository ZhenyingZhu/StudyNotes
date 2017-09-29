// https://leetcode.com/problems/longest-common-prefix/description/

#include <iostream>
#include <vector>
#include <string>

using namespace std;

class JiabinSolution {
public:
	string longestCommonPrefix(vector<string> strs) {
		string commonPrefix = strs[0];

		for (int i = 1; i < strs.size(); i++) {
			int j = 0;
			for (; j < strs[i].size(); j++) {
				if (j >= commonPrefix.size()) {
					break;
				}

				if (strs[i][j] != commonPrefix[j]) {
					commonPrefix = commonPrefix.substr(0, j);
					break;
				}
			}

			if (j < commonPrefix.size()) {
				commonPrefix = commonPrefix.substr(0, j);
			}
		}

		return commonPrefix;
	}
};

class Solution {
public:
	string longestCommonPrefix(vector<string>& strs) {
		if (strs.empty())
			return "";

		string prefix = strs[0];
		for (int i = 1; i != strs.size(); i++) {
			prefix = getCommonPrefix(strs[i], prefix);
		}

		return prefix;
	}

private:
	string getCommonPrefix(const string& str1, const string& str2) {
		string prefix;
		for (int i = 0; i != str1.length(); i++) {
			if (i >= str2.length() || str1[i] != str2[i])
				break;
			prefix += str1[i];
		}
		return prefix;
	}
};

int LongestCommonPrefixMain() {
	JiabinSolution sol;
	vector<string> input = { "apple", "app","apply" };
	//{"abc", "ab", "abcd"};

	cout << sol.longestCommonPrefix(input) << endl;

	return 0;
}