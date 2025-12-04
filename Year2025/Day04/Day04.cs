using AdventOfCode.Helpers;
using System;

namespace AdventOfCode.Year2025
{
    class Day04 : IDay
    {
        bool[,] wall;
        int Height, Width;

        public Day04(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            Height = lines.Length + 2;
            Width = lines[0].Length +2;
            wall = new bool[Height, Width];
            for (int i = 0; i < lines.Length; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    wall[i + 1, j + 1] = lines[i][j] == '@';
                }
            }
        }

        public void Task1()
        {
            var result = 0;
            for(int i=0; i < Height; i++)
            {
                for(int j=0; j < Width; j++)
                {
                    if (wall[i,j])
                    {
                        if(CountSurroundingPaperStacks(i, j) <4)
                        {
                            result++;
                        }
                    }
                }
            }
            Console.WriteLine(result);
        }

        private int CountSurroundingPaperStacks(int i, int j)
        {
            var result = 0;
            if (wall[i - 1, j - 1])
            {
                result++;
            }
            if (wall[i - 1, j])
            {
                result++;
            }
            if (wall[i - 1, j + 1])
            {
                result++;
            }
            if (wall[i, j - 1])
            {
                result++;
            }
            if (wall[i, j + 1])
            {
                result++;
            }
            if (wall[i + 1, j - 1])
            {
                result++;
            }
            if (wall[i + 1, j])
            {
                result++;
            }
            if (wall[i + 1, j + 1])
            {
                result++;
            }
            return result;
        }

        public void Task2()
        {
            var result = 0;
            var shouldContinue = true;
            while (shouldContinue)
            {
                shouldContinue = false;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (wall[i, j])
                        {
                            if (CountSurroundingPaperStacks(i, j) < 4)
                            {
                                shouldContinue = true;
                                wall[i, j] = false;
                                result++;
                            }
                        }
                    }
                }
            }
            Console.WriteLine(result);
        }
    }
}