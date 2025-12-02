using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2025
{
    class Day01 : IDay
    {
        public Day01(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        List<(char c, int n)> input = new List<(char c, int n)>();

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach (var line in lines)
            {
                char c = line.Substring(0, 1)[0];
                int.TryParse(line.Substring(1), out int number);
                input.Add((c,number));
            }
        }

        public void Task1()
        {
            var pointingAtZeroCount = 0;
            int currentPosition = 50;
            foreach (var item in input)
            {
                switch (item.c)
                {
                    case 'L':
                        currentPosition -= item.n;
                        break;
                    case 'R':
                        currentPosition += item.n;
                        break;
                }
                currentPosition %= 100;
                if (currentPosition == 0)
                    pointingAtZeroCount++;
            }
            Console.WriteLine(pointingAtZeroCount);
        }

        public void Task2()
        {
            var pointingAtZeroCount = 0;
            int currentPosition = 50;
            foreach (var item in input)
            {
                pointingAtZeroCount += item.n / 100;
                var number = item.n % 100;
                switch (item.c)
                {
                    case 'L':
                        if (currentPosition == 0)
                            currentPosition = 100;
                        currentPosition -= number;
                        if (currentPosition <= 0)
                        {
                            currentPosition += 100;
                            pointingAtZeroCount++;
                        }
                        break;
                    case 'R':
                        if (currentPosition == 100)
                            currentPosition = 0;
                        currentPosition += number;
                        if (currentPosition >= 100)
                        {
                            currentPosition -= 100;
                            pointingAtZeroCount++;
                        }
                        break;
                }
            }
            Console.WriteLine(pointingAtZeroCount);
        }
    }
}