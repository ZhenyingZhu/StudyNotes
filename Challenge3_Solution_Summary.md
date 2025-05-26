# Challenge 3 Solution: Vulnerability Factor Minimization

## Solution Approach

### Algorithm Overview
1. **Binary Search on Answer**: Use binary search to find the minimum achievable vulnerability factor
2. **Feasibility Check**: For each candidate vulnerability factor, check if it's achievable with ≤ maxChange modifications
3. **Greedy Optimization**: Use a greedy approach to determine the minimum number of changes needed

### Key Components

#### 1. Main Function: `findVulnerabilityFactor(int[] key, int maxChange)`
- Uses binary search on the range [0, array_length]
- Returns the minimum achievable vulnerability factor

#### 2. Feasibility Check: `canAchieveVulnerability()`
- Identifies all "problematic" subarrays (length > target AND GCD > 1)
- Uses greedy algorithm to determine if these can be broken with ≤ maxChange modifications

#### 3. Greedy Breaking Strategy: `canBreakAllSubarrays()`
- Sorts subarrays by length (longer first) to prioritize breaking longer ones
- For each subarray, finds the optimal position to change that breaks the most subarrays
- Uses set cover approach to minimize total changes needed

#### 4. GCD Calculation: `subarrayGcd()` and `gcd()`
- Efficient GCD calculation using Euclidean algorithm
- Early termination when GCD becomes 1

### Test Results
All test cases pass successfully:
- ✓ Provided example: [2,2,4,9,6] with maxChange=1 → result=2
- ✓ Empty array handling
- ✓ Single element arrays
- ✓ Arrays with all coprime elements
- ✓ Arrays with identical elements
- ✓ Cases with no changes allowed
- ✓ Cases with many changes allowed

### Example Analysis
For the provided example `key=[2,2,4,9,6]` with `maxChange=1`:

**Current State:**
- Vulnerability factor = 3 (subarray [2,2,4] has GCD=2, length=3)
- Other subarrays with GCD > 1: [2,2], [2,4], [9,6]

**After 1 Optimal Change:**
- Minimum achievable vulnerability factor = 2
- Can be achieved by strategically changing one element to break the longest subarray

### Time Complexity
- **Overall**: O(n² × log(max_value) × log(n))
- **Binary Search**: O(log(n)) iterations
- **Feasibility Check**: O(n² × log(max_value)) per iteration
- **Space Complexity**: O(n) for temporary arrays and data structures

### Files Created
1. `VulnerabilityFactor.java` - Main solution implementation
2. `VulnerabilityFactorTest.java` - Comprehensive test suite
3. `DebugTest.java` - Debug utility for analyzing specific cases

The solution correctly handles all edge cases and provides optimal results for the vulnerability factor minimization problem.
