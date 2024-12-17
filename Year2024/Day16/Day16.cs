using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day16 : IDay
    {
        int maxH, maxW;
        static MapTile[,] map;
        (int i, int j) startingPosition;
        (int i, int j) finishPosition;
        const int _moveForwardScore = 1;
        const int _turnScore = 1000;

        public Day16(string inputFilePath)
        {
            ParseInput(inputFilePath);
            bestMapTiles = new List<(int i, int j)>();
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
                    MapTile newTile = new MapTile
                    {
                        MapPosition = (i, j),
                        IsObstacle = lines[i][j] == '#'
                    };
                    if (lines[i][j] == 'S')
                    {
                        startingPosition = (i, j);
                    }
                    else if (lines[i][j] == 'E')
                    {
                        finishPosition = (i, j);
                    }
                    map[i, j] = newTile;
                }
            }
        }

        public void Task1()
        {
            map[startingPosition.i, startingPosition.j].EnterTile(0, Direction.East, null);
            Console.WriteLine(map[finishPosition.i, finishPosition.j].GetLowestScoreToEnter());
        }

        public void Task2()
        {
            bestMapTiles.Add(finishPosition);
            map[finishPosition.i, finishPosition.j].PopulateBestMapTiles(Direction.North);
            Console.WriteLine(bestMapTiles.Count);
            //PrintMap();
        }

        void PrintMap()
        {
            for (int i = 0; i != maxH; i++)
            {
                for (int j = 0; j != maxW; j++)
                {
                    if (map[i, j].IsObstacle)
                    {
                        Console.Write("#");
                    }
                    else if (bestMapTiles.Any(x => x.i == i && x.j == j))
                    {
                        Console.Write("O");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        static List<(int i, int j)> bestMapTiles = new List<(int i, int j)>();

        class MapTile
        {
            public (int i, int j) MapPosition;
            public bool IsObstacle;
            public long BestScoreFacingNorth = long.MaxValue;
            List<MapTile> BestNorthCameFrom = new List<MapTile>();
            public long BestScoreFacingSouth = long.MaxValue;
            List<MapTile> BestSouthCameFrom = new List<MapTile>();
            public long BestScoreFacingEast = long.MaxValue;
            List<MapTile> BestEastCameFrom = new List<MapTile>();
            public long BestScoreFacingWest = long.MaxValue;
            List<MapTile> BestWestCameFrom = new List<MapTile>();

            public void EnterTile(long currentScore, Direction direction, MapTile fromTile)
            {
                if (IsObstacle)
                    return;

                switch (direction)
                {
                    case Direction.North:
                        if (currentScore < BestScoreFacingNorth)
                        {
                            BestScoreFacingNorth = currentScore;
                            if (fromTile != null)
                                BestNorthCameFrom = new List<MapTile> { fromTile };
                            else
                                BestNorthCameFrom.Clear();
                            map[MapPosition.i - 1, MapPosition.j].EnterTile(currentScore + _moveForwardScore, Direction.North, this);
                            EnterTile(currentScore + _turnScore, Direction.East, fromTile);
                            EnterTile(currentScore + _turnScore, Direction.West, fromTile);
                        }
                        else if (currentScore == BestScoreFacingNorth)
                        {
                            BestNorthCameFrom.Add(fromTile);
                        }
                        break;
                    case Direction.South:
                        if (currentScore < BestScoreFacingSouth)
                        {
                            BestScoreFacingSouth = currentScore;
                            if (fromTile != null)
                                BestSouthCameFrom = new List<MapTile> { fromTile };
                            else
                                BestSouthCameFrom.Clear();
                            map[MapPosition.i + 1, MapPosition.j].EnterTile(currentScore + _moveForwardScore, Direction.South, this);
                            EnterTile(currentScore + _turnScore, Direction.East, fromTile);
                            EnterTile(currentScore + _turnScore, Direction.West, fromTile);
                        }
                        else if (currentScore == BestScoreFacingSouth)
                        {
                            BestSouthCameFrom.Add(fromTile);
                        }
                        break;
                    case Direction.East:
                        if (currentScore < BestScoreFacingEast)
                        {
                            BestScoreFacingEast = currentScore;
                            if (fromTile != null)
                                BestEastCameFrom = new List<MapTile> { fromTile };
                            else
                                BestEastCameFrom.Clear();
                            map[MapPosition.i, MapPosition.j + 1].EnterTile(currentScore + _moveForwardScore, Direction.East, this);
                            EnterTile(currentScore + _turnScore, Direction.North, fromTile);
                            EnterTile(currentScore + _turnScore, Direction.South, fromTile);
                        }
                        else if (currentScore == BestScoreFacingEast)
                        {
                            BestEastCameFrom.Add(fromTile);
                        }
                        break;
                    case Direction.West:
                        if (currentScore < BestScoreFacingWest)
                        {
                            BestScoreFacingWest = currentScore;
                            if (fromTile != null)
                                BestWestCameFrom = new List<MapTile> { fromTile };
                            else
                                BestWestCameFrom.Clear();
                            map[MapPosition.i, MapPosition.j - 1].EnterTile(currentScore + _moveForwardScore, Direction.West, this);
                            EnterTile(currentScore + _turnScore, Direction.North, fromTile);
                            EnterTile(currentScore + _turnScore, Direction.South, fromTile);
                        }
                        else if (currentScore == BestScoreFacingWest)
                        {
                            BestWestCameFrom.Add(fromTile);
                        }
                        break;
                }
            }

            public void PopulateBestMapTiles(Direction dir)
            {
                switch (dir)
                {
                    case Direction.North:
                        foreach (var mapTile in BestNorthCameFrom)
                        {
                            if (!bestMapTiles.Any(bmt => bmt.i == mapTile.MapPosition.i && bmt.j == mapTile.MapPosition.j))
                            {
                                bestMapTiles.Add((mapTile.MapPosition.i, mapTile.MapPosition.j));
                            }
                            TriggerOtherPopulateBestMapTiles(mapTile);
                        }
                        break;
                    case Direction.South:
                        foreach (var mapTile in BestSouthCameFrom)
                        {
                            if (!bestMapTiles.Any(bmt => bmt.i == mapTile.MapPosition.i && bmt.j == mapTile.MapPosition.j))
                            {
                                bestMapTiles.Add((mapTile.MapPosition.i, mapTile.MapPosition.j));
                            }
                            TriggerOtherPopulateBestMapTiles(mapTile);
                        }
                        break;
                    case Direction.East:
                        foreach (var mapTile in BestEastCameFrom)
                        {
                            if (!bestMapTiles.Any(bmt => bmt.i == mapTile.MapPosition.i && bmt.j == mapTile.MapPosition.j))
                            {
                                bestMapTiles.Add((mapTile.MapPosition.i, mapTile.MapPosition.j));
                            }
                            TriggerOtherPopulateBestMapTiles(mapTile);
                        }
                        break;
                    case Direction.West:
                        foreach (var mapTile in BestWestCameFrom)
                        {
                            if (!bestMapTiles.Any(bmt => bmt.i == mapTile.MapPosition.i && bmt.j == mapTile.MapPosition.j))
                            {
                                bestMapTiles.Add((mapTile.MapPosition.i, mapTile.MapPosition.j));
                            }
                            TriggerOtherPopulateBestMapTiles(mapTile);
                        }
                        break;
                }
            }
            void TriggerOtherPopulateBestMapTiles(MapTile mapTile)
            {
                if (mapTile.MapPosition.i < this.MapPosition.i)
                {
                    mapTile.PopulateBestMapTiles(Direction.South);
                }
                if (mapTile.MapPosition.i > this.MapPosition.i)
                {
                    mapTile.PopulateBestMapTiles(Direction.North);
                }
                if (mapTile.MapPosition.j < this.MapPosition.j)
                {
                    mapTile.PopulateBestMapTiles(Direction.East);
                }
                if (mapTile.MapPosition.j > this.MapPosition.j)
                {
                    mapTile.PopulateBestMapTiles(Direction.West);
                }
            }

            public long GetLowestScoreToEnter()
            {
                return new long[] { BestScoreFacingEast, BestScoreFacingNorth, BestScoreFacingSouth, BestScoreFacingWest }.Min();
            }
        }

        enum Direction
        {
            North,
            East,
            South,
            West
        }
    }
}