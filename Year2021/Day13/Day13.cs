using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day13 : IDay
    {
        private string[] Lines;
        private bool[,] Marked = new bool[2000, 2000];
        private int MaxY = 0;
        private int MaxX = 0;
        private int It = 0;

        private void OutputState()
        {
            for (int j = 0; j <= MaxY; j++)
            {
                for (int i = 0; i <= MaxX; i++)
                {
                    Console.Write(Marked[i, j] ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

        private void PopulateMarked()
        {
            bool stopLoop = false;
            Lines = StaticHelpers.GetLines(this.GetType());

            while (!stopLoop)
            {
                var line = Lines[It];
                if (string.IsNullOrEmpty(line))
                {
                    stopLoop = true;
                    break;
                }
                var splitLine = line.Split(",");
                int x = int.Parse(splitLine[0]);
                if (x > MaxX) MaxX = x;
                int y = int.Parse(splitLine[1]);
                if (y > MaxY) MaxY = y;
                Marked[x, y] = true;
                It++;
            }
        }

        private void PerformFold(string axis, int foldLine)
        {
            if (axis == "x")
            {
                for (int x = foldLine + 1; x <= MaxX; x++)
                {
                    for (int y = 0; y <= MaxY; y++)
                    {
                        Marked[foldLine - (x - foldLine), y] |= Marked[x, y];
                    }
                }
                MaxX = foldLine - 1;
            }
            if (axis == "y")
            {
                for (int x = 0; x <= MaxX; x++)
                {
                    for (int y = foldLine + 1; y <= MaxY; y++)
                    {
                        Marked[x, foldLine - (y - foldLine)] |= Marked[x, y];
                    }
                }
                MaxY = foldLine - 1;
            }
        }

        public void Task1()
        {
            PopulateMarked();
            //OutputState();
            It++;
            var splitFold = Lines[It].Replace("fold along ", "").Split("=");
            var foldLine = int.Parse(splitFold[1]);
            PerformFold(splitFold[0], foldLine);
            int sum = 0;
            for(int i = 0; i <= MaxX;i++)
            {
                for(int j =0;j<= MaxY;j++)
                {
                    if (Marked[i, j]) sum++;
                }
            }
            Console.WriteLine(sum);
            //OutputState();
        }

        

        public void Task2()
        {
            for (int i = It; i != Lines.Length; i++)
            {
                var splitFold = Lines[i].Replace("fold along ", "").Split("=");
                var foldLine = int.Parse(splitFold[1]);
                PerformFold(splitFold[0], foldLine);
            }
            OutputState();
        }
    }
}
