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
    class Day22 : IDay
    {
        int CubeSize = 50;

        enum Tile
        {
            Wrap = 0,
            Empty = 1,
            Wall = 2
        }

        enum Direction
        {
            Right = 0,
            Down = 1,
            Left = 2,
            Up = 3
        }

        bool[,] Cube = new bool[4, 4];

        Tile[,] Map;
        int Width = 0, Height = 0;

        void Initialize(string[] lines)
        {
            Height = lines.Count() - 2;
            Width = lines.Max(x => x.Length);
            Map = new Tile[Width + 2, Height + 2];

            for (int y = 0; y != Height; y++)
            {
                for (int i = 0; i != lines[y].Length; i++)
                {
                    if (lines[y][i] == '.')
                    {
                        Map[i + 1, y + 1] = Tile.Empty;
                        Cube[i / CubeSize, y / CubeSize] = true;
                    }
                    else if (lines[y][i] == '#')
                    {
                        Map[i + 1, y + 1] = Tile.Wall;
                        Cube[i / CubeSize, y / CubeSize] = true;
                    }
                }
            }
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            Initialize(lines);

            //DrawMap();

            Point startingPoint = new Point(lines[0].IndexOf(".") + 1, 1);
            string route = lines[lines.Count() - 1];

            FollowRoute(startingPoint, route);
        }


        void FollowRoute(Point currentPoint, string route)
        {
            Direction direction = Direction.Right;
            string number = "";
            for (int i = 0; i <= route.Length; i++)
            {
                if (i == route.Length || route[i] == 'R' || route[i] == 'L')
                {
                    if (int.TryParse(number, out int numberOfSteps))
                    {
                        for (int step = 0; step != numberOfSteps; step++)
                        {
                            currentPoint = MoveStep(currentPoint, ref direction);
                        }
                    }
                    number = "";
                    if (i == route.Length)
                    {
                        Console.WriteLine(1000 * currentPoint.Y + 4 * currentPoint.X + (int)direction);
                        return;
                    }
                    if (route[i] == 'R')
                    {
                        direction = (Direction)(((int)direction + 1) % 4);
                    }
                    else if (route[i] == 'L')
                    {
                        direction = (Direction)(((int)direction + 3) % 4);
                    }
                }
                else
                {
                    number += route[i];
                }
            }
        }

        bool IsPart2 = false;

        private Point CubeWrap(Point wrapPoint, ref Direction direction)
        {
            /*    A C 
             *  B|_|_|D
             *  E|_|F
             *B|_|_|D
             *A|_|G
             *  C
             */
            Direction oldDirection = direction;
            int cubeX = ((wrapPoint.X - 1) / CubeSize + 4) % 4;
            int cubeY = ((wrapPoint.Y - 1) / CubeSize + 4) % 4;
            Point newPoint = new Point();

            if (cubeX == 1 && cubeY == 0)
            {
                if (direction == Direction.Up) //A
                {
                    newPoint = new Point(1, wrapPoint.X + 100);
                    direction = Direction.Right;
                }
                else if (direction == Direction.Left) //B
                {
                    newPoint = new Point(1, Math.Abs(wrapPoint.Y - 51) + 100);
                    direction = Direction.Right;
                }
            }
            else if (cubeX == 2 && cubeY == 0)
            {
                if (direction == Direction.Up) //C
                {
                    newPoint = new Point(wrapPoint.X - 100, 200);
                    direction = Direction.Up;
                }
                else if (direction == Direction.Right) //D
                {
                    newPoint = new Point(100, Math.Abs(wrapPoint.Y - 51) + 100);
                    direction = Direction.Left;
                }
                else if (direction == Direction.Down) //F
                {
                    newPoint = new Point(100, wrapPoint.X - 50);
                    direction = Direction.Left;
                }
            }
            else if (cubeX == 1 && cubeY == 1)
            {
                if (direction == Direction.Right) //F
                {
                    newPoint = new Point(wrapPoint.Y + 50, 50);
                    direction = Direction.Up;
                }
                else if (direction == Direction.Left) //E
                {
                    newPoint = new Point(wrapPoint.Y - 50, 101);
                    direction = Direction.Down;
                }
            }
            else if (cubeX == 0 && cubeY == 2)
            {
                if (direction == Direction.Left) //B
                {
                    direction = Direction.Right;
                    newPoint = new Point(51, Math.Abs(wrapPoint.Y - 151));
                }
                else if (direction == Direction.Up) //E
                {
                    newPoint = new Point(51, wrapPoint.X + 50);
                    direction = Direction.Right;
                }
            }
            else if (cubeX == 1 && cubeY == 2)
            {
                if (direction == Direction.Right) //D
                {
                    newPoint = new Point(150, Math.Abs(wrapPoint.Y - 151));
                    direction = Direction.Left;
                }
                else if (direction == Direction.Down) //G
                {
                    newPoint = new Point(50, wrapPoint.X + 100);
                    direction = Direction.Left;
                }
            }
            else if (cubeX == 0 && cubeY == 3)
            {
                if (direction == Direction.Left) //A
                {
                    newPoint = new Point(wrapPoint.Y - 100, 1);
                    direction = Direction.Down;
                }
                else if (direction == Direction.Down) //C
                {
                    newPoint = new Point(wrapPoint.X + 100, 1);
                    direction = Direction.Down;
                }
                else if (direction == Direction.Right) //G
                {
                    newPoint = new Point(wrapPoint.Y - 100, 150);
                    direction = Direction.Up;
                }
            }
            if (Map[newPoint.X, newPoint.Y] == Tile.Wall)
            {
                direction = oldDirection;
                return wrapPoint;
            }
            return newPoint;
        }

        private Point MoveStep(Point currentPoint, ref Direction direction)
        {
            Point newPoint = currentPoint;
            switch (direction)
            {
                case Direction.Right:
                    switch (Map[currentPoint.X + 1, currentPoint.Y])
                    {
                        case Tile.Wrap:
                            if (!IsPart2)
                            {
                                newPoint.X = 0;
                                while (Map[newPoint.X, newPoint.Y] == Tile.Wrap)
                                {
                                    newPoint.X++;
                                }
                                if (Map[newPoint.X, newPoint.Y] == Tile.Wall)
                                {
                                    newPoint.X = currentPoint.X;
                                }
                            }
                            else
                            {
                                newPoint = CubeWrap(currentPoint, ref direction);
                            }
                            break;
                        case Tile.Empty:
                            newPoint.X++;
                            break;
                        case Tile.Wall:
                            //We don't move
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (Map[currentPoint.X - 1, currentPoint.Y])
                    {
                        case Tile.Wrap:
                            if (!IsPart2)
                            {
                                newPoint.X = Width;
                                while (Map[newPoint.X, newPoint.Y] == Tile.Wrap)
                                {
                                    newPoint.X--;
                                }
                                if (Map[newPoint.X, newPoint.Y] == Tile.Wall)
                                {
                                    newPoint.X = currentPoint.X;
                                }
                            }
                            else
                            {
                                newPoint = CubeWrap(currentPoint, ref direction);
                            }
                            break;
                        case Tile.Empty:
                            newPoint.X--;
                            break;
                        case Tile.Wall:
                            //We don't move
                            break;
                    }
                    break;
                case Direction.Up:
                    switch (Map[currentPoint.X, currentPoint.Y - 1])
                    {
                        case Tile.Wrap:
                            if (!IsPart2)
                            {
                                newPoint.Y = Height;
                                while (Map[newPoint.X, newPoint.Y] == Tile.Wrap)
                                {
                                    newPoint.Y--;
                                }
                                if (Map[newPoint.X, newPoint.Y] == Tile.Wall)
                                {
                                    newPoint.Y = currentPoint.Y;
                                }
                            }
                            else
                            {
                                newPoint = CubeWrap(currentPoint, ref direction);
                            }
                            break;
                        case Tile.Empty:
                            newPoint.Y--;
                            break;
                        case Tile.Wall:
                            //We don't move
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (Map[currentPoint.X, currentPoint.Y + 1])
                    {
                        case Tile.Wrap:
                            if (!IsPart2)
                            {
                                newPoint.Y = 0;
                                while (Map[newPoint.X, newPoint.Y] == Tile.Wrap)
                                {
                                    newPoint.Y++;
                                }
                                if (Map[newPoint.X, newPoint.Y] == Tile.Wall)
                                {
                                    newPoint.Y = currentPoint.Y;
                                }
                            }
                            else
                            {
                                newPoint = CubeWrap(currentPoint, ref direction);
                            }
                            break;
                        case Tile.Empty:
                            newPoint.Y++;
                            break;
                        case Tile.Wall:
                            //We don't move
                            break;
                    }
                    break;
            }
            return newPoint;
        }

        void DrawMap()
        {
            for (int y = 0; y != Height; y++)
            {
                for (int x = 0; x != Width; x++)
                {
                    switch (Map[x, y])
                    {
                        case Tile.Wrap:
                            Console.Write(" ");
                            break;
                        case Tile.Empty:
                            Console.Write(".");
                            break;
                        case Tile.Wall:
                            Console.Write("#");
                            break;
                    }
                }
                Console.WriteLine();
            }
        }

        public void Task2() //169074, 104310 too high //33551 too low
        {
            IsPart2 = true;
            var lines = StaticHelpers.GetLines(this.GetType()).ToList();

            Point startingPoint = new Point(lines[0].IndexOf(".") + 1, 1);
            string route = lines[lines.Count() - 1];

            FollowRoute(startingPoint, route);
        }
    }
}
