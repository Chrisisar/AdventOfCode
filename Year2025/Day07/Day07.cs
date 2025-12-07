using AdventOfCode.Helpers;
using System;

namespace AdventOfCode.Year2025
{
    class Day07 : IDay
    {
        string[] lines;
        (int x, int y) startingPoint;
        long[,] possiblePathsToNode;

        public Day07(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            lines = StaticHelpers.GetLines(inputFilePath);
            startingPoint = (lines[0].IndexOf("S"), 0);
            possiblePathsToNode = new long[lines[0].Length, lines.Length];
            possiblePathsToNode[startingPoint.x, startingPoint.y] = 1;
        }

        public void Task1()
        {
            var result = 0;
            for (int y = 1; y < lines.Length; y++)
            {
                for (int x = 0; x != lines[y].Length; x++)
                {
                    if (lines[y][x] == '.')
                        possiblePathsToNode[x, y] += possiblePathsToNode[x, y - 1];
                    if (lines[y][x] == '^')
                    {
                        if (possiblePathsToNode[x, y - 1] > 0)
                        {
                            result++;
                        }
                        //possiblePathsToNode[x, y] = 0;
                        possiblePathsToNode[x - 1, y] += possiblePathsToNode[x, y - 1];
                        possiblePathsToNode[x + 1, y] += possiblePathsToNode[x, y - 1];
                    }
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            long result = 0;
            for (int i = 0; i != lines[0].Length; i++)
            {
                result += possiblePathsToNode[i, lines.Length - 1];
            }
            Console.WriteLine(result);
        }
    }
}