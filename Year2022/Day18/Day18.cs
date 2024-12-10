using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day18 : IDay
    {
        int[,,] map = new int[100, 100, 100]; //x,y,z

        struct Point
        {
            public int X, Y, Z;
            public Point(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            var result = 0;
            foreach (var line in lines)
            {
                var splitLine = line.Split(",");
                int.TryParse(splitLine[0], out int x);
                int.TryParse(splitLine[1], out int y);
                int.TryParse(splitLine[2], out int z);
                x += 1;
                y += 1;
                z += 1;
                if (map[x, y, z] == 0)
                {
                    map[x, y, z] = 1;
                    int delta = 6;
                    if (map[x - 1, y, z]==1)
                    {
                        delta -= 2;
                    }
                    if (map[x + 1, y, z] == 1)
                    {
                        delta -= 2;
                    }
                    if (map[x, y - 1, z] == 1)
                    {
                        delta -= 2;
                    }
                    if (map[x, y + 1, z] == 1)
                    {
                        delta -= 2;
                    }
                    if (map[x, y, z - 1] == 1)
                    {
                        delta -= 2;
                    }
                    if (map[x, y, z + 1] == 1)
                    {
                        delta -= 2;
                    }
                    result += delta;
                }
            }
            Console.WriteLine(result);
        }

        private Queue<Point> queue = new Queue<Point>();
        int task2Result = 0;
        private void BFS(Point point)
        {
            int x = point.X;
            int y = point.Y;
            int z = point.Z;
            if (x-1 >= 0 && map[x - 1, y, z] == 1)
            {
                task2Result++;
            }
            if (x + 1 < 100 && map[x + 1, y, z] == 1)
            {
                task2Result++;
            }
            if (y-1 >=0 && map[x, y - 1, z] == 1)
            {
                task2Result++;
            }
            if (y + 1 < 100 && map[x, y + 1, z] == 1)
            {
                task2Result++;
            }
            if (z - 1 >= 0 && map[x, y, z - 1] == 1)
            {
                task2Result++;
            }
            if (z + 1 < 100 && map[x, y, z + 1] == 1)
            {
                task2Result++;
            }


            if (x - 1 >= 0 && map[x - 1, y, z] == 0)
            {
                map[x - 1, y, z] = 2;
                queue.Enqueue(new Point(x - 1, y, z));
            }
            if (x + 1 < 100 && map[x + 1, y, z] == 0)
            {
                map[x + 1, y, z] = 2;
                queue.Enqueue(new Point(x + 1, y, z));
            }
            if (y - 1 >= 0 && map[x, y - 1, z] == 0)
            {
                map[x, y - 1, z] = 2;
                queue.Enqueue(new Point(x, y-1, z));
            }
            if (y + 1 < 100 && map[x, y + 1, z] == 0)
            {
                map[x, y + 1, z] = 2;
                queue.Enqueue(new Point(x, y+1, z));
            }
            if (z - 1 >= 0 && map[x, y, z - 1] == 0)
            {
                map[x, y, z - 1] = 2;
                queue.Enqueue(new Point(x, y, z-1));
            }
            if (z + 1 < 100 && map[x, y, z + 1] == 0)
            {
                map[x, y, z + 1] = 2;
                queue.Enqueue(new Point(x, y, z+1));
            }
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            int maxX = 0, maxY = 0, maxZ = 0;
            foreach (var line in lines)
            {
                var splitLine = line.Split(",");
                int.TryParse(splitLine[0], out int x);
                int.TryParse(splitLine[1], out int y);
                int.TryParse(splitLine[2], out int z);
                x += 1;
                y += 1;
                z += 1;
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
                maxZ = Math.Max(maxZ, z);
                map[x, y, z] = 1;
            }
            queue.Enqueue(new Point(0, 0, 0));
            map[0, 0, 0] = 2;
            while(queue.Any())
            {
                BFS(queue.Dequeue());
            }
            Console.WriteLine(task2Result);
        }
    }
}
