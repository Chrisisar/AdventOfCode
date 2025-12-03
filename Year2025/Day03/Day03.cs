using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2025
{
    class Day03 : IDay
    {
        List<List<int>> banks = new List<List<int>>();

        public Day03(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach(var line in lines)
            {
                var bank = new List<int>();
                foreach(var c in line)
                {
                    int.TryParse(c.ToString(), out int n);
                    bank.Add(n);
                }
                banks.Add(bank);
            }
        }

        public void Task1()
        {
            long result = 0;
            foreach (var bank in banks)
            {
                result += GetHighestResultForBank(bank, 2);
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            long result = 0;
            foreach (var bank in banks)
            {
                result += GetHighestResultForBank(bank, 12);
            }
            Console.WriteLine(result);
        }

        private long GetHighestResultForBank(List<int> bank, int numberOfChargedBatteries)
        {
            long batteryResult = 0;
            var startPosition = 0;
            for (int i = 0; i != numberOfChargedBatteries; i++)
            {
                var highestValue = FindHighestValueFor(startPosition, bank.Count - numberOfChargedBatteries + i + 1, bank);
                startPosition = highestValue.position + 1;
                batteryResult += highestValue.value * (long)Math.Pow(10, numberOfChargedBatteries - i - 1);
            }
            return batteryResult;
        }

        private (int position, int value) FindHighestValueFor(int startingPosition, int furthestPossiblePosition, List<int> bank)
        {
            int maxValue = bank.Take(furthestPossiblePosition).Skip(startingPosition).Max();
            int positionOfMaxValue = bank.Skip(startingPosition).ToList().IndexOf(maxValue) + startingPosition;
            return (positionOfMaxValue, maxValue);
        }
    }
}