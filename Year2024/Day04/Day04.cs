using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day04 : IDay
    {
        char[,] map;
        int maxH, maxW;
        List<(int i, int j)> xPoints = new List<(int i, int j)>();
        List<(int i, int j)> aPoints = new List<(int i, int j)>();

        public Day04(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            maxH = lines.Length;
            maxW = lines[0].Length;
            map = new char[maxH, maxW];
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    map[i, j] = lines[i][j];
                    if (map[i, j] == 'X')
                    {
                        xPoints.Add((i, j));
                    }
                    if (map[i, j] == 'A')
                    {
                        aPoints.Add((i, j));
                    }
                }
            }
        }

        public void Task1()
        {
            var result = 0;
            string fullWord = "XMAS";
            foreach(var xPoint in xPoints)
            {
                result += FoundRemainingWord(xPoint.i, xPoint.j, -1, -1, fullWord) ? 1 : 0;
                result += FoundRemainingWord(xPoint.i, xPoint.j, -1, 0, fullWord) ? 1 : 0;
                result += FoundRemainingWord(xPoint.i, xPoint.j, -1, 1, fullWord) ? 1 : 0;
                result += FoundRemainingWord(xPoint.i, xPoint.j, 0, -1, fullWord) ? 1 : 0;
                result += FoundRemainingWord(xPoint.i, xPoint.j, 0, 1, fullWord) ? 1 : 0;
                result += FoundRemainingWord(xPoint.i, xPoint.j, 1, -1, fullWord) ? 1 : 0;
                result += FoundRemainingWord(xPoint.i, xPoint.j, 1, 0, fullWord) ? 1 : 0;
                result += FoundRemainingWord(xPoint.i, xPoint.j, 1, 1, fullWord) ? 1 : 0;
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            var result = 0;
            foreach(var aPoint in aPoints)
            {
                if(((FoundRemainingWord(aPoint.i - 1, aPoint.j - 1, 0,0,"M") &&  FoundRemainingWord(aPoint.i +1, aPoint.j +1,0,0,"S"))
                    || (FoundRemainingWord(aPoint.i - 1, aPoint.j - 1, 0, 0, "S") && FoundRemainingWord(aPoint.i + 1, aPoint.j + 1, 0, 0, "M")))
                    && ((FoundRemainingWord(aPoint.i + 1, aPoint.j - 1, 0, 0, "M") && FoundRemainingWord(aPoint.i - 1, aPoint.j + 1, 0, 0, "S"))
                        || (FoundRemainingWord(aPoint.i + 1, aPoint.j - 1, 0, 0, "S") && FoundRemainingWord(aPoint.i - 1, aPoint.j + 1, 0, 0, "M"))))
                {
                    result++;
                }
            }
            Console.WriteLine(result);
        }

        bool FoundRemainingWord(int i, int j, int diri, int dirj, string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return true;
            }

            if (i < 0 || j < 0 || i >= maxH || j >= maxW)
            {
                return false;
            }

            if (map[i, j] != word[0])
            {
                return false;
            }

            return FoundRemainingWord(i - diri, j - dirj, diri, dirj, word.Substring(1));
        }
    }
}