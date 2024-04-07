using System;
using System.Collections.Generic;

namespace MyApp
{
    class Program
    {

        static void CalculateCombinations(int amount, int[] denominations, List<int> currentCombination, int index, List<List<int>> result)
        {
            //if amount is zero, terminate recursion, found a valid combination
            if (amount == 0)
            {
                //copy the current combination to the result list
                result.Add(new List<int>(currentCombination));
                return;
            }

            //if amount becomes negative or all denominations already tried, return
            if (amount < 0 || index >= denominations.Length)
                return;

            //include the current denomination and try again
            currentCombination.Add(denominations[index]);

            //recurse, pass the new amount with the subtracted current denomination
            CalculateCombinations(amount - denominations[index], denominations, currentCombination, index, result);

            //this line will be executed after recursion is finished from the function call above, remove the current denomination to backtrack
            currentCombination.RemoveAt(currentCombination.Count - 1);

            //skip the current denomination and try with the next denomination
            CalculateCombinations(amount, denominations, currentCombination, index + 1, result);
        }

        static List<List<int>> PayoutCombinations(int amount, int[] denominations)
        {
            List<List<int>> result = new List<List<int>>();
            CalculateCombinations(amount, denominations, new List<int>(), 0, result);
            return result;
        }

        static void Main(string[] args)
        {
            //test data
            int[] payouts = { 30, 50, 60, 80, 140, 230, 370, 610, 980 };
            //denominations
            int[] denominations = { 10, 50, 100 };

            //loop through each test data
            foreach (int payout in payouts)
            {
                //call payout combinations
                List<List<int>> combinations = PayoutCombinations(payout, denominations);
                //start of output
                Console.WriteLine($"Possible combinations for {payout} EUR:");
                //loop through the combinations found
                foreach (List<int> combination in combinations)
                {
                    //define a dictionary to store the number of bills per denomination
                    Dictionary<int, int> counts = new Dictionary<int, int>();
                    //count the number of bills per denomination
                    foreach (int denom in combination)
                    {
                        if (!counts.ContainsKey(denom))
                            counts[denom] = 0;
                        counts[denom]++;
                    }
                    //create a list of string to store the result of the combinations of denomination
                    List<string> parts = new List<string>();
                    foreach (var pair in counts)
                    {
                        parts.Add($"{pair.Value} x {pair.Key} EUR");
                    }
                    //join the combinations to form the string result
                    Console.WriteLine(string.Join(" + ", parts));
                }
                Console.WriteLine();
            }
        }





    }
}


