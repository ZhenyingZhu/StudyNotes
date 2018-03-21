#include <iostream>
#include <vector>

using namespace std;

namespace FindPrimeNumbers {

	int FindPrimeNumbersMain() {
		int n = 100;
		vector<int> res(n + 1, 1);

		// If exclude multiples of 2 first, then start from 3, the repeat exclusion in first 100 would be 3: 45, 75, 63.
		/*
		for (int i = 2 * 2; i <= n; i += 2) {
		res[i] = 0;
		}
		*/

		// Using Sieve of Eratosthenes.
		for (int i = 2; i <= n; i++) {
			if (res[i] == 0)
				continue;

			for (int j = i * i; j <= n; j += 1 * i) {
				if (res[j] == 0) {
					cout << "Repeat computing: " << j << " when i = " << i << endl;
				}
				res[j] = 0;
			}
		}

		return 0;
	}

}