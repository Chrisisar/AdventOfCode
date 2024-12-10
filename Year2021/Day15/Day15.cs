using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day15 : IDay
    {
        int[,] Map;
        int[,] LowestCostToGet;
        int MaxX;
        int MaxY;

        public Day15()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            MaxX = lines.Length;
            MaxY = lines[0].Length;
            Map = new int[MaxX * 5, MaxY * 5];
            LowestCostToGet = new int[MaxX * 5, MaxY * 5];

            for (int i = 0; i != MaxX; i++)
            {
                for (int j = 0; j != MaxY; j++)
                {
                    for(int k = 0; k!=5;k++)
                    {
                        for (int l = 0; l != 5; l++)
                        {
                            var value = (lines[i][j] - '0' + k + l) % 9;
                            Map[i + MaxX * k, j + MaxY * l] = value == 0 ? 9 : value;
                            LowestCostToGet[i + MaxX * k, j + MaxY * l] = int.MaxValue;
                        }
                    }
                }
            }
            LowestCostToGet[0, 0] = 0;
        }

        void OutputMap(int maxX, int maxY)
        {
            for (int i = 0; i != maxX; i++)
            {
                for (int j = 0; j != maxY; j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
        }

        void OutputLowestCost()
        {
            for (int i = 0; i != MaxX; i++)
            {
                for (int j = 0; j != MaxY; j++)
                {
                    Console.Write(LowestCostToGet[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        Queue<Tuple<int, int>> PointsQueue = new Queue<Tuple<int, int>>();

        void BFS(int x, int y)
        {
            if (x > 0)
            {
                if (LowestCostToGet[x - 1, y] > LowestCostToGet[x, y] + Map[x - 1, y])
                {
                    LowestCostToGet[x - 1, y] = LowestCostToGet[x, y] + Map[x - 1, y];
                    if (!PointsQueue.Any(item => item.Item1 == (x - 1) && item.Item2 == y))
                    {
                        PointsQueue.Enqueue(new Tuple<int, int>(x - 1, y));
                    }
                }
            }
            if (x < MaxX * 5 - 1)
            {
                if (LowestCostToGet[x + 1, y] > LowestCostToGet[x, y] + Map[x + 1, y])
                {
                    LowestCostToGet[x + 1, y] = LowestCostToGet[x, y] + Map[x + 1, y];
                    if (!PointsQueue.Any(item => item.Item1 == (x + 1) && item.Item2 == y))
                    {
                        PointsQueue.Enqueue(new Tuple<int, int>(x + 1, y));
                    }
                }
            }
            if (y > 0)
            {
                if (LowestCostToGet[x, y - 1] > LowestCostToGet[x, y] + Map[x, y - 1])
                {
                    LowestCostToGet[x, y - 1] = LowestCostToGet[x, y] + Map[x, y - 1];
                    if (!PointsQueue.Any(item => item.Item1 == x && item.Item2 == (y - 1)))
                    {
                        PointsQueue.Enqueue(new Tuple<int, int>(x, y - 1));
                    }
                }
            }
            if (y < MaxY * 5 - 1)
            {
                if (LowestCostToGet[x, y + 1] > LowestCostToGet[x, y] + Map[x, y + 1])
                {
                    LowestCostToGet[x, y + 1] = LowestCostToGet[x, y] + Map[x, y + 1];
                    if (!PointsQueue.Any(item => item.Item1 == x && item.Item2 == (y + 1)))
                    {
                        PointsQueue.Enqueue(new Tuple<int, int>(x, y + 1));
                    }
                }
            }
        }

        public void Task1()
        {
            PointsQueue.Enqueue(new Tuple<int, int>(0, 0));
            while (PointsQueue.Count != 0)
            {
                var popped = PointsQueue.Dequeue();
                BFS(popped.Item1, popped.Item2);
            }
            //OutputLowestCost();
            Console.WriteLine(LowestCostToGet[MaxX - 1, MaxY - 1]);
        }

        public void Task2()
        {
            Console.WriteLine(LowestCostToGet[MaxX * 5 - 1, MaxY * 5 - 1]);
        }
    }
}
