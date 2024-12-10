using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day12 : IDay
    {
        int[,] visited;
        int[,] map;
        Point Start;
        Point End;
        int height, width;
        Queue<Point> bfsQueue;

        private void InitializeTask()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            height = lines.Count();
            width = lines[0].Length;
            map = new int[height, width];
            visited = new int[height, width];
            bfsQueue = new Queue<Point>();
            for (int i = 0; i != height; i++)
            {
                for (int j = 0; j != width; j++)
                {
                    if (lines[i][j] == 'S')
                    {
                        Start = new Point(i, j);
                        map[i, j] = 0;
                    }
                    else if (lines[i][j] == 'E')
                    {
                        End = new Point(i, j);
                        map[i, j] = 'z' - 'a';
                    }
                    else
                    {
                        map[i, j] = lines[i][j] - 'a';
                    }
                }
            }
        }

        public void BFS(int x, int y, int steps)
        {
            steps += 1;
            if (x > 0 && visited[x - 1, y] == 0 && map[x, y] + 1 >= map[x - 1, y])
            {
                bfsQueue.Enqueue(new Point(x - 1, y));
                visited[x - 1, y] = steps;
            }
            if (x < height - 1 && visited[x + 1, y] == 0 && map[x, y] + 1 >= map[x + 1, y])
            {
                bfsQueue.Enqueue(new Point(x + 1, y));
                visited[x + 1, y] = steps;
            }
            if (y > 0 && visited[x, y - 1] == 0 && map[x, y] + 1 >= map[x, y - 1])
            {
                bfsQueue.Enqueue(new Point(x, y - 1));
                visited[x, y - 1] = steps;
            }
            if (y < width - 1 && visited[x, y + 1] == 0 && map[x, y] + 1 >= map[x, y + 1])
            {
                bfsQueue.Enqueue(new Point(x, y + 1));
                visited[x, y + 1] = steps;
            }
        }

        public void BFS2(int x, int y, int steps)
        {
            steps += 1;
            if (x > 0 && visited[x - 1, y] == 0 && map[x, y] <= map[x - 1, y] + 1)
            {
                bfsQueue.Enqueue(new Point(x - 1, y));
                visited[x - 1, y] = steps;
            }
            if (x < height - 1 && visited[x + 1, y] == 0 && map[x, y] <= map[x + 1, y] + 1)
            {
                bfsQueue.Enqueue(new Point(x + 1, y));
                visited[x + 1, y] = steps;
            }
            if (y > 0 && visited[x, y - 1] == 0 && map[x, y] <= map[x, y - 1] + 1)
            {
                bfsQueue.Enqueue(new Point(x, y - 1));
                visited[x, y - 1] = steps;
            }
            if (y < width - 1 && visited[x, y + 1] == 0 && map[x, y] <= map[x, y + 1] + 1)
            {
                bfsQueue.Enqueue(new Point(x, y + 1));
                visited[x, y + 1] = steps;
            }
        }

        public void Task1()
        {
            InitializeTask();
            bfsQueue.Enqueue(Start);
            visited[Start.X, Start.Y] = 1;
            while (bfsQueue.Any())
            {
                Point currentPoint = bfsQueue.Dequeue();
                if (currentPoint == End)
                {
                    break;
                }
                BFS(currentPoint.X, currentPoint.Y, visited[currentPoint.X, currentPoint.Y]);
            }
            Console.WriteLine(visited[End.X, End.Y] - 1);
        }

        public void Task2()
        {
            InitializeTask();
            bfsQueue.Enqueue(End);
            visited[End.X, End.Y] = 1;
            while (bfsQueue.Any())
            {
                Point currentPoint = bfsQueue.Dequeue();
                if (map[currentPoint.X, currentPoint.Y] == 0)
                {
                    Console.WriteLine(visited[currentPoint.X, currentPoint.Y] - 1);
                    break;
                }
                BFS2(currentPoint.X, currentPoint.Y, visited[currentPoint.X, currentPoint.Y]);
            }
        }
    }
}
