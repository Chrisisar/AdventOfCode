using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day18 : IDay
    {
        int maxH, maxW;
        static int timeLimit;
        MapTile[,] map;

        public Day18(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            var splitLine = lines[0].Split(' ');
            int.TryParse(splitLine[0], out maxH);
            int.TryParse(splitLine[1], out maxW);
            int.TryParse(splitLine[2], out timeLimit);
            map = new MapTile[maxH + 1, maxW + 1];
            for (int i = 0; i != maxH + 1; i++)
            {
                for (int j = 0; j != maxW + 1; j++)
                {
                    map[i, j] = new MapTile() { ShortestPath = int.MaxValue };
                }
            }
            for (int i = 2; i != lines.Length; i++)
            {
                var line = lines[i];
                splitLine = line.Split(",");
                map[int.Parse(splitLine[0]), int.Parse(splitLine[1])].TimeToCollapse = i - 1;
            }
        }

        Queue<(int i, int j)> bfsQueue = new Queue<(int i, int j)>();

        public void Task1()
        {
            map[0, 0].ShortestPath = 0;
            bfsQueue.Enqueue((0, 0));
            while (bfsQueue.Any())
            {
                BFS();
            }
            Console.WriteLine(map[maxH, maxW].ShortestPath);
        }

        public void Task2()
        {
            timeLimit = 1;
            int lowerLimit = 0;
            int higherLimit = timeLimit;
            bool foundMax = false;
            while (!foundMax || higherLimit - lowerLimit > 1)
            {
                ResetMap();
                bfsQueue.Enqueue((0, 0));
                while (bfsQueue.Any())
                {
                    BFS();
                }
                if (map[maxH, maxW].ShortestPath != int.MaxValue)
                {
                    if (!foundMax)
                    {
                        lowerLimit = timeLimit;
                        timeLimit *= 2;
                        higherLimit = timeLimit;
                    }
                    else
                    {
                        lowerLimit = timeLimit;
                        timeLimit = (lowerLimit + higherLimit) / 2;
                    }
                }
                else
                {
                    foundMax = true;
                    higherLimit = timeLimit;
                    timeLimit = (lowerLimit + higherLimit) / 2;
                }
            }
            for(int i =0;i<=maxH;i++)
            {
                for(int j=0;j<=maxW;j++)
                {
                    if (map[i,j].TimeToCollapse == higherLimit)
                    {
                        Console.WriteLine($"First impassable is at {i},{j} - after {higherLimit} seconds.");
                        return;
                    }
                }
            }
        }

        void ResetMap()
        {
            for (int i = 0; i != maxH + 1; i++)
            {
                for (int j = 0; j != maxW + 1; j++)
                {
                    map[i, j].ShortestPath = int.MaxValue;
                }
            }
            map[0, 0].ShortestPath = 0;
        }

        void BFS()
        {
            (int i, int j) = bfsQueue.Dequeue();
            MapTile currentMapTile = map[i, j];
            int newShortest = currentMapTile.ShortestPath + 1;
            if (i > 0 && map[i - 1, j].ShortestPath > newShortest && !map[i - 1, j].IsCollapsed())
            {
                map[i - 1, j].ShortestPath = newShortest;
                bfsQueue.Enqueue((i - 1, j));
            }
            if (i < maxH && map[i + 1, j].ShortestPath > newShortest && !map[i + 1, j].IsCollapsed())
            {
                map[i + 1, j].ShortestPath = newShortest;
                bfsQueue.Enqueue((i + 1, j));
            }
            if (j > 0 && map[i, j - 1].ShortestPath > newShortest && !map[i, j - 1].IsCollapsed())
            {
                map[i, j - 1].ShortestPath = newShortest;
                bfsQueue.Enqueue((i, j - 1));
            }
            if (j < maxW && map[i, j + 1].ShortestPath > newShortest && !map[i, j + 1].IsCollapsed())
            {
                map[i, j + 1].ShortestPath = newShortest;
                bfsQueue.Enqueue((i, j + 1));
            }
        }

        void PrintMap()
        {
            for(int i =0;i<=maxH;i++)
            {
                for(int j=0;j<=maxW;j++)
                {
                    Console.Write(map[i, j].ShortestPath);
                    Console.Write("\t");
                }
                Console.WriteLine();
            }
        }

        class MapTile
        {
            public int TimeToCollapse;
            public int ShortestPath;

            public bool IsCollapsed()
            {
                return TimeToCollapse > 0 && TimeToCollapse <= timeLimit;
            }
        }
    }
}