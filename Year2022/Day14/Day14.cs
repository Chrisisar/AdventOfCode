using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day14 : IDay
    {
        bool[,] map = new bool[1000, 1000];
        int maximumY = 0;

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            foreach (var line in lines)
            {
                var points = line.Split(" -> ");
                for (int i = 1; i != points.Count(); i++)
                {
                    int.TryParse(points[i - 1].Split(",")[0], out int point1X);
                    int.TryParse(points[i - 1].Split(",")[1], out int point1Y);
                    int.TryParse(points[i].Split(",")[0], out int point2X);
                    int.TryParse(points[i].Split(",")[1], out int point2Y);

                    maximumY = Math.Max(maximumY, Math.Max(point1Y, point2Y));

                    if (point1X == point2X)
                    {
                        for (int y = Math.Min(point1Y, point2Y); y <= Math.Max(point1Y, point2Y); y++)
                        {
                            map[point1X, y] = true;
                        }
                    }
                    else if (point1Y == point2Y)
                    {
                        for (int x = Math.Min(point1X, point2X); x <= Math.Max(point1X, point2X); x++)
                        {
                            map[x, point1Y] = true;
                        }
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }

            var result = 0;

            while (!SimulateSand(1))
            {
                result++;
            }
            Console.WriteLine(result);
        }

        private bool SimulateSand(int taskNumber)
        {
            int x = 500, y = 0;
            while (y < maximumY)
            {
                if (!map[x, y + 1])
                {
                    y++;
                }
                else if (!map[x - 1, y + 1])
                {
                    y++;
                    x--;
                }
                else if (!map[x + 1, y + 1])
                {
                    y++;
                    x++;
                }
                else
                {
                    map[x, y] = true;
                    break;
                }
            }
            return taskNumber == 1 ? y == maximumY : y == 0;
        }

        public void Task2()
        {
            map = new bool[1000, 1000];
            var lines = StaticHelpers.GetLines(this.GetType());
            maximumY += 2;
            for (int i = 0; i != 1000; i++)
            {
                map[i, maximumY] = true;
            }
            foreach (var line in lines)
            {
                var points = line.Split(" -> ");
                for (int i = 1; i != points.Count(); i++)
                {
                    int.TryParse(points[i - 1].Split(",")[0], out int point1X);
                    int.TryParse(points[i - 1].Split(",")[1], out int point1Y);
                    int.TryParse(points[i].Split(",")[0], out int point2X);
                    int.TryParse(points[i].Split(",")[1], out int point2Y);

                    if (point1X == point2X)
                    {
                        for (int y = Math.Min(point1Y, point2Y); y <= Math.Max(point1Y, point2Y); y++)
                        {
                            map[point1X, y] = true;
                        }
                    }
                    else if (point1Y == point2Y)
                    {
                        for (int x = Math.Min(point1X, point2X); x <= Math.Max(point1X, point2X); x++)
                        {
                            map[x, point1Y] = true;
                        }
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }

            var result = 1;

            while (!SimulateSand(2))
            {
                result++;
            }
            Console.WriteLine(result);
        }
    }
}
