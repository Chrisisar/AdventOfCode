using AdventOfCode.Helpers;
using System;
using System.Linq;

namespace AdventOfCode.Year2025
{
    class Day06 : IDay
    {
        string[] lines;
        long[][] numbers;
        char[] operations;

        public Day06(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            lines = StaticHelpers.GetLines(inputFilePath);
            var splitLine = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            numbers = new long[splitLine.Length][];
            for (int i = 0; i < splitLine.Length; i++)
            {
                numbers[i] = new long[lines.Length - 1];
            }
            for (int i = 0; i < lines.Length - 1; i++)
            {
                splitLine = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j != splitLine.Length; j++)
                {
                    numbers[j][i] = long.Parse(splitLine[j]);
                }
            }
            operations = lines[lines.Length - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray();
        }

        public void Task1()
        {
            long total = 0;
            for (int i = 0; i != numbers.Length; i++)
            {
                long partial = 0;
                switch (operations[i])
                {
                    case '*':
                        partial = 1;
                        for (int j = 0; j != numbers[i].Length; j++)
                        {
                            partial *= numbers[i][j];
                        }
                        break;
                    case '+':
                        for (int j = 0; j != numbers[i].Length; j++)
                        {
                            partial += numbers[i][j];
                        }
                        break;
                }
                total += partial;
            }
            Console.WriteLine(total);
        }

        public void Task2()
        {
            long total = 0;
            int operationPointer = 0;
            long partial = operations[operationPointer] == '+' ? 0 : 1;
            for (int i = 0; i != lines[0].Length; i++)
            {
                long number = 0;
                for (int j = 0; j != lines.Length - 1; j++)
                {
                    if (lines[j][i] != ' ')
                    {
                        number = number * 10 + int.Parse(lines[j][i].ToString());
                    }
                }

                if (number == 0)
                {
                    operationPointer++;
                    total += partial;
                    partial = operations[operationPointer] == '+' ? 0 : 1;
                    continue;
                }

                switch (operations[operationPointer])
                {
                    case '*':
                        partial *= number;
                        break;
                    case '+':
                        partial += number;
                        break;
                }
            }
            total += partial;
            Console.WriteLine(total);
        }
    }
}