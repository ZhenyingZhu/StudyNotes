/*https://leetcode.com/problems/longest-substring-without-repeating-characters/description/
*time:
*solution:
*medium
*/

#include <iostream>
#include <string>
#include <unordered_map>
#include <algorithm>

using namespace std;

namespace LongestSubstringWithoutRepeatingCharacters {

class Solution {
public:
	int lengthOfLongestSubstring(string s) {
		if (s.length() < 2)
			return s.length();

		int res = 0;
		int st = 0, ed = 1;
		unordered_map<char, int> map{ { s[st], st } };
		while (ed < s.length()) {
			if (map.count(s[ed]) && map[s[ed]] >= st) {
				res = max(res, ed - st);
				st = map[s[ed]] + 1;
			}
			map[s[ed]] = ed++;
		}

		res = max(res, ed - st);
		return res;
	}
};

int main() {
	Solution sol;
	cout << sol.lengthOfLongestSubstring("abcabcbb") << endl;

	return 0;
}

}