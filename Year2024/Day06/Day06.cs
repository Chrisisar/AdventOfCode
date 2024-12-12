using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day06 : IDay
    {
        (int i, int j, Direction dir) guardStartingPosition;
        (int i, int j, Direction dir) currentGuardPosition;
        int maxH, maxW;
        MapTile[,] map;
        List<(int i, int j, Direction dir)> tilesToBlock = new List<(int i, int j, Direction dir)>();

        public Day06(string inputFilePath)
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
                    map[i, j] = new MapTile { IsObstacle = lines[i][j] == '#' };
                    if (lines[i][j] == '^')
                    {
                        guardStartingPosition = (i, j, Direction.North);
                    }
                }
            }
        }

        public void Task1()
        {
            currentGuardPosition = (guardStartingPosition.i, guardStartingPosition.j, guardStartingPosition.dir);
            MoveGuard(true);
            Console.WriteLine(tilesToBlock.Count);
        }

        public void Task2()
        {
            long result = 0;
            foreach(var tileToBlock in tilesToBlock)
            {
                map[tileToBlock.i, tileToBlock.j].IsObstacle = true;
                for(int i =0;i<maxH;i++)
                {
                    for(int j=0;j<maxW;j++)
                    {
                        map[i, j].VisitedDirections.Clear();
                    }
                }
                currentGuardPosition = (tileToBlock.i, tileToBlock.j, tileToBlock.dir);
                if(MoveGuard(false))
                {
                    result++;
                }
                map[tileToBlock.i, tileToBlock.j].IsObstacle = false;
            }
            Console.WriteLine(result);
        }

        bool MoveGuard(bool isPartOne)
        {
            while (true)
            {
                if (currentGuardPosition.i < 0 || currentGuardPosition.i == maxH || currentGuardPosition.j < 0 || currentGuardPosition.j == maxW)
                {
                    return false;
                }

                if (map[currentGuardPosition.i, currentGuardPosition.j].IsObstacle)
                {
                    switch (currentGuardPosition.dir)
                    {
                        case Direction.North:
                            currentGuardPosition.i++;
                            break;
                        case Direction.South:
                            currentGuardPosition.i--;
                            break;
                        case Direction.East:
                            currentGuardPosition.j--;
                            break;
                        case Direction.West:
                            currentGuardPosition.j++;
                            break;
                    }
                    currentGuardPosition.dir = (Direction)(((int)currentGuardPosition.dir + 1) % 4);
                }
                else
                {
                    if (isPartOne && !tilesToBlock.Any(ttb => ttb.i == currentGuardPosition.i && ttb.j == currentGuardPosition.j))
                    {
                        tilesToBlock.Add((currentGuardPosition.i, currentGuardPosition.j, currentGuardPosition.dir));
                    }
                    if(map[currentGuardPosition.i, currentGuardPosition.j].VisitedDirections.Any(vd => vd == currentGuardPosition.dir))
                    {
                        return true;
                    }
                    map[currentGuardPosition.i, currentGuardPosition.j].VisitedDirections.Add(currentGuardPosition.dir);

                    switch (currentGuardPosition.dir)
                    {
                        case Direction.North:
                            currentGuardPosition.i--;
                            break;
                        case Direction.South:
                            currentGuardPosition.i++;
                            break;
                        case Direction.East:
                            currentGuardPosition.j++;
                            break;
                        case Direction.West:
                            currentGuardPosition.j--;
                            break;
                    }
                }
            }
        }

        enum Direction
        {
            North,
            East,
            South,
            West
        }

        class MapTile
        {
            public bool IsObstacle;
            public List<Direction> VisitedDirections = new List<Direction>();
        }
    }
}