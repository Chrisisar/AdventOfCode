using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day20 : IDay
    {
        (int i, int j) startingPosition;
        (int i, int j) finishPosition;
        MapTile[,] map;
        int maxH, maxW;
        List<int> cheatSaves = new List<int>();

        public Day20(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            maxH = lines.Length;
            maxW = lines[0].Length;
            map = new MapTile[maxH, maxW];
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    map[i, j] = new MapTile
                    {
                        IsObstacle = lines[i][j] == '#',
                        BestFromStart = int.MaxValue,
                        BestFromFinish = int.MaxValue,
                    };
                    if (lines[i][j] == 'S')
                    {
                        startingPosition = (i, j);
                    }
                    else if (lines[i][j] == 'E')
                    {
                        finishPosition = (i, j);
                    }
                }
            }
        }

        void InitialDFS(int i, int j, int currentScore, bool isFromStart)
        {
            if (map[i, j].IsObstacle) return;

            if (currentScore < (isFromStart ? map[i, j].BestFromStart : map[i, j].BestFromFinish))
            {
                if (isFromStart)
                {
                    map[i, j].BestFromStart = currentScore;
                }
                else
                {
                    map[i, j].BestFromFinish = currentScore;
                }

                InitialDFS(i - 1, j, currentScore + 1, isFromStart);
                InitialDFS(i + 1, j, currentScore + 1, isFromStart);
                InitialDFS(i, j - 1, currentScore + 1, isFromStart);
                InitialDFS(i, j + 1, currentScore + 1, isFromStart);
            }
        }

        void CalculateCheats(int i, int j, int maxRange)
        {
            for (int k = -maxRange; k <= maxRange; k++)
            {
                for (int l = -maxRange; l <= maxRange; l++)
                {
                    int newi = i + k;
                    int newj = j + l;
                    int range = Math.Abs(k) + Math.Abs(l);

                    if (range > maxRange) continue;
                    if (newi < 0 || newj < 0 || newi >= maxH || newj >= maxW) continue;
                    if (map[newi, newj].IsObstacle) continue;
                    if (map[newi, newj].BestFromStart == int.MaxValue) continue;
                    if (map[newi, newj].BestFromFinish == int.MaxValue) continue;

                    int newBestWithThisCheat = map[i, j].BestFromStart + range + map[newi, newj].BestFromFinish;
                    if (map[finishPosition.i, finishPosition.j].BestFromStart > newBestWithThisCheat)
                    {
                        cheatSaves.Add(map[finishPosition.i, finishPosition.j].BestFromStart - newBestWithThisCheat);
                    }
                }
            }
        }

        public void Task1()
        {
            InitialDFS(startingPosition.i, startingPosition.j, 0, true);
            InitialDFS(finishPosition.i, finishPosition.j, 0, false);
            for (int i = 1; i < maxH - 1; i++)
            {
                for (int j = 1; j < maxW - 1; j++)
                {
                    if (!map[i, j].IsObstacle)
                    {
                        CalculateCheats(i, j, 2);
                    }
                }
            }
            Console.WriteLine(cheatSaves.Count(x => x >= 100));
        }

        public void Task2()
        {
            cheatSaves.Clear();
            for(int i=1;i<maxH-1;i++)
            {
                for(int j=1;j<maxW-1;j++)
                {
                    if (!map[i, j].IsObstacle)
                    {
                        CalculateCheats(i, j, 20);
                    }
                }
            }
            Console.WriteLine(cheatSaves.Count(x => x >= 100));
        }

        void PrintCheatSaves()
        {
            foreach (var save in cheatSaves.OrderBy(x => x).GroupBy(x => x))
            {
                Console.WriteLine($"There are {save.Count()} cheats that save {save.Key} picoseconds.");
            }
        }

        class MapTile
        {
            public int BestFromStart;
            public int BestFromFinish;
            public bool IsObstacle;
        }
    }
}