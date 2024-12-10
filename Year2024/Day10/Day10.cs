using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day10 : IDay
    {
        bool[,] visited;
        int[,] heights;
        int maxH, maxW;
        List<(int i, int j)> trailheads = new List<(int i, int j)>();

        public Day10(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            maxH = lines.Length;
            maxW = lines[0].Length;
            heights = new int[maxH, maxW];
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    heights[i, j] = int.Parse(lines[i][j].ToString());
                    if (heights[i, j] == 0)
                    {
                        trailheads.Add((i, j));
                    }
                }
            }
        }

        public void Task1()
        {
            int result = 0;
            foreach (var trailhead in trailheads)
            {
                visited = new bool[maxH, maxW];
                result += CalculateScore(trailhead.i, trailhead.j, 0, true);
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            int result = 0;
            foreach (var trailhead in trailheads)
            {
                result += CalculateScore(trailhead.i, trailhead.j, 0, false);
            }
            Console.WriteLine(result);
        }

        private int CalculateScore(int i, int j, int expectedValue, bool checkVisited)
        {
            if ((checkVisited && visited[i, j]) || heights[i, j] != expectedValue)
            {
                return 0;
            }
            if (expectedValue == 9 && heights[i, j] == 9)
            {
                return 1;
            }

            visited[i, j] = true;
            int score = 0;
            if (heights[i, j] == expectedValue)
            {
                if(i > 0)
                {
                    score += CalculateScore(i - 1, j, expectedValue + 1, checkVisited);
                }
                if(i < maxH - 1)
                {
                    score += CalculateScore(i + 1, j, expectedValue + 1, checkVisited);
                }
                if(j > 0)
                {
                    score += CalculateScore(i, j - 1, expectedValue + 1, checkVisited);
                }
                if(j < maxW - 1)
                {
                    score += CalculateScore(i, j + 1, expectedValue + 1, checkVisited);
                }
            }
            return score;
        }
    }
}