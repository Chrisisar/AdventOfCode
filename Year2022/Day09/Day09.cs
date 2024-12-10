using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day09 : IDay
    {
        class Point
        {
            public int X, Y;
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override bool Equals(object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    Point p = (Point)obj;
                    return (X == p.X) && (Y == p.Y);
                }
            }
        }

        List<Point> visited;
        List<Point> points;

        private void MovePoint(Point precedingPoint, Point thisPoint)
        {
            if (precedingPoint.X > thisPoint.X + 1)
            {
                if (precedingPoint.Y != thisPoint.Y)
                {
                    if (precedingPoint.Y < thisPoint.Y)
                        thisPoint.Y--;
                    if (precedingPoint.Y > thisPoint.Y)
                        thisPoint.Y++;
                }
                thisPoint.X++;
            }
            if (precedingPoint.X < thisPoint.X - 1)
            {
                if (precedingPoint.Y != thisPoint.Y)
                {
                    if (precedingPoint.Y < thisPoint.Y)
                        thisPoint.Y--;
                    if (precedingPoint.Y > thisPoint.Y)
                        thisPoint.Y++;
                }
                thisPoint.X--;
            }
            if (precedingPoint.Y > thisPoint.Y + 1)
            {
                if (precedingPoint.X != thisPoint.X)
                {
                    if (precedingPoint.X < thisPoint.X)
                        thisPoint.X--;
                    if (precedingPoint.X > thisPoint.X)
                        thisPoint.X++;
                }
                thisPoint.Y++;
            }
            if (precedingPoint.Y < thisPoint.Y - 1)
            {
                if (precedingPoint.X != thisPoint.X)
                {
                    if (precedingPoint.X < thisPoint.X)
                        thisPoint.X--;
                    if (precedingPoint.X > thisPoint.X)
                        thisPoint.X++;
                }
                thisPoint.Y--;
            }
        }

        private void ParseLine(string line)
        {
            var splitLine = line.Split(' ');
            int.TryParse(splitLine[1], out int moveLength);
            for (int i = 0; i != moveLength; i++)
            {
                switch (splitLine[0])
                {
                    case "U":
                        points[0].X++;
                        break;
                    case "D":
                        points[0].X--;
                        break;
                    case "L":
                        points[0].Y--;
                        break;
                    case "R":
                        points[0].Y++;
                        break;
                }
                for (int j = 0; j != points.Count - 1; j++)
                {
                    MovePoint(points[j], points[j + 1]);
                }
                visited.Add(new Point(points.Last().X, points.Last().Y));
            }
        }

        private int CountVisited()
        {
            for (int i = 0; i < visited.Count; i++)
            {
                for (int j = i + 1; j < visited.Count; j++)
                {
                    if (visited[i].Equals(visited[j]))
                    {
                        visited.RemoveAt(j);
                        j--;
                    }
                }
            }
            var result = visited.Count();
            return result;
        }

        private void InitializeTask(int lineLength)
        {
            visited = new List<Point>();
            points = new List<Point>();
            for (int i = 0; i != lineLength; i++)
            {
                points.Add(new Point(0, 0));
            }
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            InitializeTask(2);
            foreach (var line in lines)
            {
                ParseLine(line);
            }
            Console.WriteLine(CountVisited());
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            InitializeTask(10);
            foreach (var line in lines)
            {
                ParseLine(line);
            }
            Console.WriteLine(CountVisited());
        }

        private void DisplayVisited()
        {
            int maxHeight = visited.Max(x => x.X);
            int minHeight = visited.Min(x => x.X);
            int maxWidth = visited.Max(x => x.Y);
            int minWidth = visited.Min(x => x.Y);

            for (int i = maxHeight; i >= minHeight; i--)
            {
                for (int j = minWidth; j <= maxWidth; j++)
                {
                    if (visited.Any(x => x.X == i && x.Y == j))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        private void DisplayCurrent()
        {
            int maxHeight = points.Max(x => x.X);
            int minHeight = points.Min(x => x.X);
            int maxWidth = points.Max(x => x.Y);
            int minWidth = points.Min(x => x.Y);

            for (int i = maxHeight; i >= minHeight; i--)
            {
                for (int j = minWidth; j <= maxWidth; j++)
                {
                    if (points.Any(x => x.X == i && x.Y == j))
                    {
                        Console.Write(points.IndexOf(points.First(x => x.X == i && x.Y == j)));
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
