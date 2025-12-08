using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Year2020
{
    class Day24 : IDay
    {
        //HEX MAP FROM 0 INDEX - IF ROW IS EVEN SE IS +0, SW IS -1. IF ROW IS ODD SE IS +1, SW IS +0;
        //0 1 2 3 4 5
        // 0 1 2 3 4
        //0 1 2 3 4 5

        enum Direction
        {
            NW,
            NE,
            W,
            E,
            SW,
            SE
        }

        List<(int x, int y)> calculatedHexes = new List<(int x, int y)>();

        public Day24(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach (var line in lines)
            {
                (int x, int y) currentPoint = (0, 0);
                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case 'e':
                            currentPoint = GetNodeByDirection(Direction.E, currentPoint);
                            break;
                        case 'w':
                            currentPoint = GetNodeByDirection(Direction.W, currentPoint);
                            break;
                        case 's':
                            i++;
                            if (line[i] == 'e')
                                currentPoint = GetNodeByDirection(Direction.SE, currentPoint);
                            else
                                currentPoint = GetNodeByDirection(Direction.SW, currentPoint);
                            break;
                        case 'n':
                            i++;
                            if (line[i] == 'e')
                                currentPoint = GetNodeByDirection(Direction.NE, currentPoint);
                            else
                                currentPoint = GetNodeByDirection(Direction.NW, currentPoint);
                            break;
                    }
                }
                if (calculatedHexes.Contains(currentPoint))
                {
                    calculatedHexes.Remove(currentPoint);
                }
                else
                {
                    calculatedHexes.Add(currentPoint);
                }
            }
        }

        public void Task1()
        {
            Console.WriteLine(calculatedHexes.Count);
        }

        public void Task2()
        {
            for (int i = 0; i != 100; i++)
            {
                var nextDayHexes = new List<(int x, int y)>();
                foreach (var calculatedHex in calculatedHexes)
                {
                    if (!FlipWhenBlack(calculatedHex))
                    {
                        nextDayHexes.Add(calculatedHex);
                    }
                    var nwPoint = GetNodeByDirection(Direction.NW, calculatedHex);
                    if (FlipWhenWhite(nwPoint))
                    {
                        nextDayHexes.Add(nwPoint);
                    }
                    var nePoint = GetNodeByDirection(Direction.NE, calculatedHex);
                    if (FlipWhenWhite(nePoint))
                    {
                        nextDayHexes.Add(nePoint);
                    }
                    var wPoint = GetNodeByDirection(Direction.W, calculatedHex);
                    if (FlipWhenWhite(wPoint))
                    {
                        nextDayHexes.Add(wPoint);
                    }
                    var ePoint = GetNodeByDirection(Direction.E, calculatedHex);
                    if (FlipWhenWhite(ePoint))
                    {
                        nextDayHexes.Add(ePoint);
                    }
                    var swPoint = GetNodeByDirection(Direction.SW, calculatedHex);
                    if (FlipWhenWhite(swPoint))
                    {
                        nextDayHexes.Add(swPoint);
                    }
                    var sePoint = GetNodeByDirection(Direction.SE, calculatedHex);
                    if (FlipWhenWhite(sePoint))
                    {
                        nextDayHexes.Add(sePoint);
                    }
                }
                //Console.WriteLine($"Day {i} -> {calculatedHexes.Count}");
                calculatedHexes = nextDayHexes.Distinct().ToList();
                
            }
            Console.WriteLine(calculatedHexes.Count);
        }

        bool FlipWhenWhite((int x, int y) point)
        {
            if (!calculatedHexes.Contains(point))
            {
                return HowManyAdjacentAreBlack(point) == 2;
            }
            return false;
        }

        bool FlipWhenBlack((int x, int y) point)
        {
            var howManyAdjacentAreBlack = HowManyAdjacentAreBlack(point);
            if (howManyAdjacentAreBlack == 0 || howManyAdjacentAreBlack > 2)
            {
                return true;
            }
            return false;
        }

        int HowManyAdjacentAreBlack((int x, int y) point)
        {
            var result = 0;
            result += calculatedHexes.Contains(GetNodeByDirection(Direction.NW, point)) ? 1 : 0;
            result += calculatedHexes.Contains(GetNodeByDirection(Direction.NE, point)) ? 1 : 0;
            result += calculatedHexes.Contains(GetNodeByDirection(Direction.W, point)) ? 1 : 0;
            result += calculatedHexes.Contains(GetNodeByDirection(Direction.E, point)) ? 1 : 0;
            result += calculatedHexes.Contains(GetNodeByDirection(Direction.SW, point)) ? 1 : 0;
            result += calculatedHexes.Contains(GetNodeByDirection(Direction.SE, point)) ? 1 : 0;
            return result;
        }

        (int, int) GetNodeByDirection(Direction direction, (int x, int y) point)
        {
            switch (direction)
            {
                case Direction.NW:
                    if (Math.Abs(point.y) % 2 == 0)
                    {
                        return (point.x - 1, point.y - 1);
                    }
                    return (point.x, point.y - 1);
                case Direction.NE:
                    if (Math.Abs(point.y) % 2 == 1)
                    {
                        return (point.x + 1, point.y - 1);
                    }
                    return (point.x, point.y - 1);
                case Direction.W:

                    return (point.x - 1, point.y);
                case Direction.E:
                    return (point.x + 1, point.y);
                case Direction.SW:
                    if (Math.Abs(point.y) % 2 == 0)
                    {
                        return (point.x - 1, point.y + 1);
                    }
                    return (point.x, point.y + 1);
                case Direction.SE:
                    if (Math.Abs(point.y) % 2 == 1)
                    {
                        return (point.x + 1, point.y + 1);
                    }
                    return (point.x, point.y + 1);
            }
            throw new Exception();
        }
    }
}