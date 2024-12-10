using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day9 : IDay
    {
        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            caveMap = new int[lines.Length, lines[0].Length];
            for (int i = 0; i != lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j != line.Length; j++)
                {
                    caveMap[i, j] = line[j] - '0';
                }
            }

            int result = 0;
            for (int i = 0; i != lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j != lines.Length; j++)
                {
                    if((i-1 < 0 || caveMap[i-1,j] > caveMap[i,j])
                        && (i + 1 >= lines.Length || caveMap[i + 1, j] > caveMap[i, j])
                        && (j - 1 < 0 || caveMap[i, j - 1] > caveMap[i, j])
                        && (j + 1 >= line.Length || caveMap[i, j+1] > caveMap[i, j]))
                    {
                        result += caveMap[i, j] + 1;
                    }
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            caveMap = new int[lines.Length, lines[0].Length];
            visited = new bool[lines.Length, lines[0].Length];
            for (int i = 0; i != lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j != line.Length; j++)
                {
                    caveMap[i, j] = line[j] - '0';
                }
            }

            List<Tuple<int, int>> lowestPoints = new List<Tuple<int, int>>();
            for (int i = 0; i != lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j != lines.Length; j++)
                {
                    if ((i - 1 < 0 || caveMap[i - 1, j] > caveMap[i, j])
                        && (i + 1 >= lines.Length || caveMap[i + 1, j] > caveMap[i, j])
                        && (j - 1 < 0 || caveMap[i, j - 1] > caveMap[i, j])
                        && (j + 1 >= line.Length || caveMap[i, j + 1] > caveMap[i, j]))
                    {
                        lowestPoints.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            List<int> basises = new List<int>();
            foreach(var lowestPoint in lowestPoints)
            {
                basises.Add(BFS(lowestPoint.Item1, lowestPoint.Item2));
            }
            var result = 1;
            basises.OrderByDescending(x => x).Take(3).ToList().ForEach(x => result *= x);
            Console.WriteLine(result);

        }

        private bool[,] visited;
        private int[,] caveMap;

        private int BFS(int i, int j)
        {
            visited[i, j] = true;
            if (caveMap[i,j] == 9)
                return 0;
            int sum = 1;
            if(i > 0 && !visited[i-1,j])
            {
                sum += BFS(i - 1, j);
            }
            if(i < visited.GetLength(0) - 1 && !visited[i+1,j])
            {
                sum += BFS(i + 1, j);
            }
            if(j > 0 && !visited[i,j-1])
            {
                sum += BFS(i, j-1);
            }
            if(j < visited.GetLength(1) - 1 && !visited[i,j+1])
            {
                sum += BFS(i, j+1);
            }
            return sum;
        }
    }
}
