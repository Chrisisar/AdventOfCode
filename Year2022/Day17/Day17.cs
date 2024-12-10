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
    class Day17 : IDay
    {
        class Shape
        {
            public List<Point> Points = new List<Point>();
            public int Height { get; set; }

            public bool CanBlow(int x, int y, bool toRight)
            {
                if (toRight)
                {
                    return Points.All(p => p.X + x < 6 && !Map[p.X + x + 1, p.Y + y]);
                }
                else
                {
                    return Points.All(p => p.X + x >= 1 && !Map[p.X + x - 1, p.Y + y]);
                }
            }

            public bool TouchesFloor(int x, int y)
            {
                return y == -1 || Points.Any(p => Map[p.X + x, p.Y + y]);
            }

            public void MarkMap(int x, int y)
            {
                foreach (var point in Points)
                {
                    Map[point.X + x, point.Y + y] = true;
                }
            }
        }


        public static bool[,] Map = new bool[7, 10000];
        private List<Shape> ShapeOrder = new List<Shape>();

        void Initialize()
        {
            Shape minusShape = new Shape();
            minusShape.Points.Add(new Point(0, 0));
            minusShape.Points.Add(new Point(1, 0));
            minusShape.Points.Add(new Point(2, 0));
            minusShape.Points.Add(new Point(3, 0));
            minusShape.Height = 0;
            ShapeOrder.Add(minusShape);

            Shape plusShape = new Shape();
            plusShape.Points.Add(new Point(1, 0));
            plusShape.Points.Add(new Point(0, 1));
            plusShape.Points.Add(new Point(1, 1));
            plusShape.Points.Add(new Point(2, 1));
            plusShape.Points.Add(new Point(1, 2));
            plusShape.Height = 2;
            ShapeOrder.Add(plusShape);

            Shape lShape = new Shape();
            lShape.Points.Add(new Point(0, 0));
            lShape.Points.Add(new Point(1, 0));
            lShape.Points.Add(new Point(2, 0));
            lShape.Points.Add(new Point(2, 1));
            lShape.Points.Add(new Point(2, 2));
            lShape.Height = 2;
            ShapeOrder.Add(lShape);

            Shape iShape = new Shape();
            iShape.Points.Add(new Point(0, 0));
            iShape.Points.Add(new Point(0, 1));
            iShape.Points.Add(new Point(0, 2));
            iShape.Points.Add(new Point(0, 3));
            iShape.Height = 3;
            ShapeOrder.Add(iShape);

            Shape squareShape = new Shape();
            squareShape.Points.Add(new Point(0, 0));
            squareShape.Points.Add(new Point(0, 1));
            squareShape.Points.Add(new Point(1, 0));
            squareShape.Points.Add(new Point(1, 1));
            squareShape.Height = 1;
            ShapeOrder.Add(squareShape);
        }

        public Day17()
        {
            Initialize();
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            int rockNumber = 0;
            int roundNumber = 0;
            string winds = lines[0];

            int maxY = -1;

            while (rockNumber < 2022)
            {
                int currentX = 2, currentY = maxY + 4;
                Shape newRock = ShapeOrder[rockNumber % ShapeOrder.Count];
                rockNumber++;

                while (true)
                {
                    bool blowsRight = winds[roundNumber % winds.Length] == '>';
                    roundNumber++;
                    if (newRock.CanBlow(currentX, currentY, blowsRight))
                    {
                        if (blowsRight)
                        {
                            currentX++;
                        }
                        else
                        {
                            currentX--;
                        }
                    }
                    currentY--;
                    if (newRock.TouchesFloor(currentX, currentY))
                    {
                        currentY++;
                        maxY = Math.Max(currentY + newRock.Height, maxY);
                        newRock.MarkMap(currentX, currentY);
                        break;
                    }
                }

            }

            Console.WriteLine(maxY + 1);
        }

        List<(int shape, int wind, int x, int rockNumber, int maxY)> History = new List<(int shape, int wind, int x, int rockNumber, int maxY)>();

        public void Task2()
        {
            Map = new bool[7, 100000000];
            var lines = StaticHelpers.GetLines(this.GetType());
            int rockNumber = 0;
            int roundNumber = 0;
            string winds = lines[0];

            int maxY = -1;

            while (rockNumber <= 3000)
            {
                int currentX = 2, currentY = maxY + 4;
                Shape newRock = ShapeOrder[rockNumber % ShapeOrder.Count];
                rockNumber++;

                while (true)
                {
                    bool blowsRight = winds[roundNumber % winds.Length] == '>';
                    roundNumber++;
                    if (newRock.CanBlow(currentX, currentY, blowsRight))
                    {
                        if (blowsRight)
                        {
                            currentX++;
                        }
                        else
                        {
                            currentX--;
                        }
                    }
                    currentY--;
                    if (newRock.TouchesFloor(currentX, currentY))
                    {
                        currentY++;
                        maxY = Math.Max(currentY + newRock.Height, maxY);
                        newRock.MarkMap(currentX, currentY);
                        foreach (var historicalInfo in History.Where(h => h.shape == rockNumber % ShapeOrder.Count
                                                                             && h.wind == roundNumber % winds.Length
                                                                             && h.x == currentX))
                        {
                            var index = History.IndexOf(historicalInfo);
                            bool success = true;
                            for (int i = 1; i <= 10; i++)
                            {
                                success &= History[index - i].shape == History[History.Count - i].shape
                                    && History[index - i].wind == History[History.Count - i].wind
                                    && History[index - i].x == History[History.Count - i].x;
                                if (!success) break;
                            }
                            if(success)
                            {
                                int rockCycle = rockNumber - historicalInfo.rockNumber;
                                int heightPerCycle = maxY - historicalInfo.maxY;
                                long rocksLeft = 1000000000000 - rockNumber;
                                long cyclesLeft = rocksLeft / rockCycle;
                                long differenceRocks = rocksLeft % rockCycle;
                                long differenceMaxY = History.FirstOrDefault(x => x.rockNumber == historicalInfo.rockNumber + differenceRocks).maxY - historicalInfo.maxY;
                                long totalMaxY = cyclesLeft * heightPerCycle + maxY + differenceMaxY;
                                Console.WriteLine(totalMaxY + 1);
                                return;
                            }
                        }
                        History.Add((rockNumber % ShapeOrder.Count, roundNumber % winds.Length, currentX, rockNumber, maxY));
                        break;
                    }
                }

            }

            Console.WriteLine(maxY + 1);
        }

        public void DrawMap(int height)
        {
            Console.Clear();
            for (int i = height; i >= 0; i--)
            {
                for (int x = 0; x < 7; x++)
                {
                    Console.Write(Map[x, i] ? '#' : '.');
                }
                Console.WriteLine();
            }
        }
    }
}
