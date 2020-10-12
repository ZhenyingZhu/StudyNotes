using System;
using System.Collections.Generic;

namespace DotNetCoreConsole
{
    public class SubArrayCost
    {
        public static void testMain()
        {
            List<int> values = new List<int>() { 1, 3, 2 };

            System.Console.WriteLine(solution(values, 2));
        }

        // Divide the values array into k sub arrays. And make the sum
        // of the cost of each array (which is the max num in the array)
        // minimum.
        private static int solution(List<int> values, int k)
        {
            int n = values.Count;
            if (n == 0)
            {
                return 0;
            }

            // Init a 0 matrixs.
            List<List<int>> partEndCosts = new List<List<int>>();
            for (int i = 0; i < k; i++)
            {
                partEndCosts.Add(new List<int>());
                for (int j = 0; j < n; j++)
                {
                    partEndCosts[i].Add(0);
                }
            }

            // Compute costs between i to j.
            List<List<int>> costLookUp = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                costLookUp.Add(new List<int>());
                int currentMax = values[i];
                for (int j = 0; j < n; j++)
                {
                    currentMax = Math.Max(currentMax, values[j]);
                    costLookUp[i].Add(currentMax);
                }
            }

            // Init first row. Means 1 part, end at j, what is the min cost.
            for (int j = 0; j < n; j++)
            {
                partEndCosts[0][j] = costLookUp[0][j];
            }

            // Compute each row. Means i+1 part, end at j, what is the min cost.
            for (int i = 1; i < k; i++)
            {
                for (int j = i; j < n; j++)
                {
                    partEndCosts[i][j] = int.MaxValue;
                    for (int x = i; x <= j; x++)
                    {
                        int currentCost = partEndCosts[i - 1][x - 1] + costLookUp[x][j];
                        partEndCosts[i][j] = Math.Min(currentCost, partEndCosts[i][j]);
                    }
                }
            }

            // Print the internal metrix.
            System.Console.WriteLine("CostLookUp");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    System.Console.Write(costLookUp[i][j]);
                    System.Console.Write("\t");
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine("DP");
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    System.Console.Write(partEndCosts[i][j]);
                    System.Console.Write("\t");
                }
                System.Console.WriteLine();
            }

            return partEndCosts[k - 1][n - 1];
        }
    }
}
