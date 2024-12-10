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
    class Day24 : IDay
    {
        public class Blizzard
        {
            public Point Coordinates;
            public Direction Direction;
        }

        public enum Direction
        {
            Right = 0,
            Down = 1,
            Left = 2,
            Up = 3
        }

        int Height = 0;
        int Width = 0;

        void Initialise(string[] lines)
        {
            Height = lines.Count() - 2;
            Width = lines[0].Length - 2;

            for (int y = 1; y <= Height; y++)
            {
                for (int x = 1; x <= Width; x++)
                {
                    switch (lines[y][x])
                    {
                        case '>':
                            Blizzards.Add(new Blizzard { Coordinates = new Point(x, y), Direction = Direction.Right });
                            break;
                        case '<':
                            Blizzards.Add(new Blizzard { Coordinates = new Point(x, y), Direction = Direction.Left });
                            break;
                        case 'v':
                            Blizzards.Add(new Blizzard { Coordinates = new Point(x, y), Direction = Direction.Down });
                            break;
                        case '^':
                            Blizzards.Add(new Blizzard { Coordinates = new Point(x, y), Direction = Direction.Up });
                            break;
                    }
                }
            }

        }

        List<Blizzard> Blizzards = new List<Blizzard>();
        Dictionary<int, List<Blizzard>> Setups = new Dictionary<int, List<Blizzard>>();
        List<(int minute, Point expPoint)> History = new List<(int minute, Point expPoint)>();
        Point End;

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            Initialise(lines);
            Point start = new Point(lines[0].IndexOf('.'), 0);
            End = new Point(lines[lines.Count() - 1].IndexOf('.'), lines.Count() - 1);
            queue.Enqueue((start, 0));
            while (queue.Any())
            {
                var item = queue.Dequeue();
                BFS(item.expedition, item.minute);
            }
            Console.WriteLine(TotalMinutes);
        }

        public Point Transform(Blizzard blizzard, int minute)
        {
            switch (blizzard.Direction)
            {
                case Direction.Right:
                    int newX = (blizzard.Coordinates.X - 1 + minute) % Width + 1;
                    return new Point(newX, blizzard.Coordinates.Y);
                case Direction.Left:
                    newX = (blizzard.Coordinates.X - 1 - minute) % Width;
                    if (newX < 0)
                    {
                        newX += Width;
                    }
                    newX++;
                    return new Point(newX, blizzard.Coordinates.Y);
                case Direction.Down:
                    int newY = (blizzard.Coordinates.Y - 1 + minute) % Height + 1;
                    return new Point(blizzard.Coordinates.X, newY);
                case Direction.Up:
                    newY = (blizzard.Coordinates.Y - 1 - minute) % Height;
                    if (newY < 0)
                    {
                        newY += Height;
                    }
                    newY++;
                    return new Point(blizzard.Coordinates.X, newY);
            }
            throw new InvalidOperationException();
        }

        Queue<(Point expedition, int minute)> queue = new Queue<(Point expedition, int minute)>();

        public void BFS(Point expeditionCoordinates, int minute)
        {
            minute++;
            if (History.Any() && History.FirstOrDefault().minute != minute)
            {
                History.Clear();
            }
            if (expeditionCoordinates.X == End.X && Math.Abs(expeditionCoordinates.Y - End.Y) == 1)
            {
                TotalMinutes = minute;
                queue.Clear();
                return;
            }
            if (!Setups.ContainsKey(minute))
            {
                Setups.Add(minute, Blizzards.Select(b => new Blizzard() { Coordinates = Transform(b, minute), Direction = b.Direction }).ToList());
            }

            //WAIT
            if (!Setups[minute].Any(b => b.Coordinates.X == expeditionCoordinates.X && b.Coordinates.Y == expeditionCoordinates.Y))
            {
                if (!History.Any(x => x.expPoint == expeditionCoordinates))
                {
                    History.Add((minute, expeditionCoordinates));
                    queue.Enqueue((expeditionCoordinates, minute));
                }
            }

            //LEFT
            Point newCoords = new Point(expeditionCoordinates.X - 1, expeditionCoordinates.Y);
            if (expeditionCoordinates.Y != 0 && expeditionCoordinates.Y != Height + 1 && expeditionCoordinates.X > 1 && !Setups[minute].Any(b => b.Coordinates == newCoords))
            {
                if (!History.Any(x => x.expPoint == newCoords))
                {
                    History.Add((minute, newCoords));
                    queue.Enqueue((newCoords, minute));
                }
            }

            //RIGHT
            newCoords = new Point(expeditionCoordinates.X + 1, expeditionCoordinates.Y);
            if (expeditionCoordinates.Y != 0 && expeditionCoordinates.Y != Height + 1 && expeditionCoordinates.X < Width && !Setups[minute].Any(b => b.Coordinates == newCoords))
            {
                if (!History.Any(x => x.expPoint == newCoords))
                {
                    History.Add((minute, newCoords));
                    queue.Enqueue((newCoords, minute));
                }
            }

            //UP
            newCoords = new Point(expeditionCoordinates.X, expeditionCoordinates.Y - 1);
            if (expeditionCoordinates.Y > 1 && !Setups[minute].Any(b => b.Coordinates == newCoords))
            {
                if (!History.Any(x => x.expPoint == newCoords))
                {
                    History.Add((minute, newCoords));
                    queue.Enqueue((newCoords, minute));
                }
            }

            //DOWN
            newCoords = new Point(expeditionCoordinates.X, expeditionCoordinates.Y + 1);
            if (expeditionCoordinates.Y < Height && !Setups[minute].Any(b => b.Coordinates == newCoords))
            {
                if (!History.Any(x => x.expPoint == newCoords))
                {
                    History.Add((minute, newCoords));
                    queue.Enqueue((newCoords, minute));
                }
            }
        }

        public void DrawMap(int minute)
        {
            for (int y = 0; y != Height + 2; y++)
            {
                for (int x = 0; x != Width + 2; x++)
                {
                    if (Setups[minute].Any(s => s.Coordinates.X == x && s.Coordinates.Y == y))
                    {

                        Console.Write(Setups[minute].Count(s => s.Coordinates.X == x && s.Coordinates.Y == y));
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        int TotalMinutes = 0;

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());

            Point start = new Point(lines[0].IndexOf('.'), 0);
            Point end = new Point(lines[lines.Count() - 1].IndexOf('.'), lines.Count() - 1);
            queue.Enqueue((end, TotalMinutes));
            End = start;
            while (queue.Any())
            {
                var item = queue.Dequeue();
                BFS(item.expedition, item.minute);
            }
            End = end;
            queue.Enqueue((start, TotalMinutes));
            while (queue.Any())
            {
                var item = queue.Dequeue();
                BFS(item.expedition, item.minute);
            }
            Console.WriteLine(TotalMinutes);
        }
    }
}
