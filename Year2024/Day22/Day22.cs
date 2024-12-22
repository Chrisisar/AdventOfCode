using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day22 : IDay
    {
        List<Monkey> monkeys = new List<Monkey>();
        static Dictionary<(int a, int b, int c, int d), int> firstOccurences = new Dictionary<(int a, int b, int c, int d), int>();

        public Day22(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach (var line in lines)
            {
                monkeys.Add(new Monkey(long.Parse(line)));
            }
        }

        public void Task1()
        {
            long result = 0;
            foreach (var monkey in monkeys)
            {
                monkey.CalculateMonkeyCycle();
                result += monkey.ValueAfter2000Steps;
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            long maxResult = 0;
            for (int a = -9; a <= 9; a++)
            {
                for (int b = -9; b <= 9; b++)
                {
                    for (int c = -9; c <= 9; c++)
                    {
                        for (int d = -9; d <= 9; d++)
                        {
                            firstOccurences.TryGetValue((a, b, c, d), out int iterationResult);
                            if (maxResult < iterationResult)
                            {
                                maxResult = iterationResult;
                            }
                        }
                    }
                }
            }
            Console.WriteLine(maxResult);
        }

        class Monkey
        {
            public Monkey(long initialValue)
            {
                InitialValue = initialValue;
                PossibleValues = new List<(long value, long? differenceToPrevious)> { (InitialValue, null) };
            }

            public Dictionary<(int a, int b, int c, int d), int> InternalFirstOccurences = new Dictionary<(int a, int b, int c, int d), int>();
            public long InitialValue;
            public List<(long value, long? differenceToPrevious)> PossibleValues;
            public long ValueAfter2000Steps;

            public void CalculateMonkeyCycle()
            {
                long currentValue = InitialValue;
                int threeAgoDifference = 0;
                int twoAgoDifference = 0;
                int oneAgoDifference = 0;
                for (int currentStep = 1; currentStep <= 2000; currentStep++)
                {
                    long previousValue = currentValue;
                    currentValue ^= currentValue * 64;
                    currentValue %= 16777216;

                    currentValue ^= currentValue / 32;
                    currentValue %= 16777216;

                    currentValue ^= currentValue * 2048;
                    currentValue %= 16777216;

                    int difference = (int)(currentValue % 10 - previousValue % 10);
                    if (currentStep >= 4)
                    {
                        if (!InternalFirstOccurences.TryGetValue((threeAgoDifference, twoAgoDifference, oneAgoDifference, difference), out int value))
                        {
                            InternalFirstOccurences.Add((threeAgoDifference, twoAgoDifference, oneAgoDifference, difference), (int)(currentValue % 10));
                            if (!firstOccurences.TryGetValue((threeAgoDifference, twoAgoDifference, oneAgoDifference, difference), out int value2))
                            {
                                firstOccurences.Add((threeAgoDifference, twoAgoDifference, oneAgoDifference, difference), (int)(currentValue % 10));
                            }
                            else
                            {
                                firstOccurences[(threeAgoDifference, twoAgoDifference, oneAgoDifference, difference)] += (int)(currentValue % 10);
                            }
                        }
                    }
                    threeAgoDifference = twoAgoDifference;
                    twoAgoDifference = oneAgoDifference;
                    oneAgoDifference = difference;
                    PossibleValues.Add((currentValue, difference));
                    if (currentStep == 2000)
                    {
                        ValueAfter2000Steps = currentValue;
                    }
                }
            }
        }
    }
}