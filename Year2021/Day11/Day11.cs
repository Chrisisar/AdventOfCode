using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day11 : IDay
    {
        private int[,] Ocean;
        private List<Tuple<int, int>> FlashQueue = new List<Tuple<int, int>>();
        bool[,] Visited;

        private void PopulateOcean()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            Ocean = new int[lines.Length, lines[0].Length];
            for (int i = 0; i != lines.Length; i++)
            {
                for (int j = 0; j != lines[i].Length; j++)
                {
                    Ocean[i, j] = int.Parse(lines[i][j].ToString());
                }
            }
        }


        private int ProcessStep()
        {
            int flashes = 0;
            //increase all by 1
            for (int i = 0; i != Ocean.GetLength(0); i++)
            {
                for (int j = 0; j != Ocean.GetLength(1); j++)
                {
                    Ocean[i, j]++;
                    if (Ocean[i, j] > 9)
                    {
                        FlashQueue.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            Visited = new bool[Ocean.GetLength(0), Ocean.GetLength(1)];
            for (int i = 0; i < FlashQueue.Count; i++)
            {
                Flash(FlashQueue[i].Item1, FlashQueue[i].Item2);
            }

            for (int i = 0; i != Ocean.GetLength(0); i++)
            {
                for (int j = 0; j != Ocean.GetLength(1); j++)
                {
                    if (Visited[i, j])
                    {
                        flashes++;
                    }
                    if (Ocean[i, j] > 9)
                    {
                        Ocean[i, j] = 0;
                    }
                }
            }
            return flashes;
        }

        private void Flash(int x, int y)
        {
            if (Visited[x, y] || Ocean[x, y] <= 9)
            {
                return;
            }
            Visited[x, y] = true;

            if (x > 0)
            {
                Ocean[x - 1, y]++;
                Flash(x - 1, y);
            }
            if (x < Ocean.GetLength(0) - 1)
            {
                Ocean[x + 1, y]++;
                Flash(x + 1, y);
            }

            if (y > 0)
            {
                Ocean[x, y - 1]++;
                Flash(x, y - 1);
            }
            if (y < Ocean.GetLength(1) - 1)
            {
                Ocean[x, y + 1]++;
                Flash(x, y + 1);
            }

            if (x > 0 && y > 0)
            {
                Ocean[x - 1, y - 1]++;
                Flash(x - 1, y - 1);
            }

            if (x > 0 && y < Ocean.GetLength(1) - 1)
            {
                Ocean[x - 1, y + 1]++;
                Flash(x - 1, y + 1);
            }

            if (x < Ocean.GetLength(0) - 1 && y < Ocean.GetLength(1) - 1)
            {
                Ocean[x + 1, y + 1]++;
                Flash(x + 1, y + 1);
            }

            if (x < Ocean.GetLength(0) - 1 && y > 0)
            {
                Ocean[x + 1, y - 1]++;
                Flash(x + 1, y - 1);
            }
        }

        private void OutputOcean()
        {
            for (int i = 0; i != Ocean.GetLength(0); i++)
            {
                for (int j = 0; j != Ocean.GetLength(1); j++)
                {
                    Console.Write(Ocean[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        public void Task1()
        {
            PopulateOcean();
            int sum = 0;
            for (int i = 0; i != 100; i++)
            {
                sum += ProcessStep();
                //if (i < 10) OutputOcean();
            }
            Console.WriteLine(sum);
        }



        public void Task2()
        {
            PopulateOcean();
            for (int i = 0; i != 1000; i++)
            {
                if (ProcessStep() == 100)
                {
                    OutputOcean();
                    Console.WriteLine(i); 
                    break; 
                }
            }
        }
    }
}
