using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    class Day25 : IDay
    {
        char[,] Map;
        Queue<Position> Queue;
        int MaxX;
        int MaxY;

        void PopulateMapAndQueue()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            MaxX = lines.Length;
            MaxY = lines[0].Length;
            Queue = new Queue<Position>();
            Map = new char[MaxX, MaxY];

            for (int i = 0; i != MaxX; i++)
            {
                for (int j = 0; j != MaxY; j++)
                {
                    Map[i, j] = lines[i][j];
                    if (lines[i][j] == '.')
                    {
                        Queue.Enqueue(new Position { X = i, Y = j });
                    }
                }
            }
        }

        void OutputMap(decimal stepNumber)
        {
            Console.WriteLine($"\nStep {stepNumber}:");
            for (int i = 0; i != MaxX; i++)
            {
                for (int j = 0; j != MaxY; j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void Task1()
        {
            PopulateMapAndQueue();
            int stepNumber = 0;
            //OutputMap(stepNumber);
            bool somethingMoved = true;
            while (somethingMoved)
            {
                somethingMoved = false;
                var MapCopy = Map.Clone() as char[,];
                foreach (var emptyPosition in Queue)
                {
                    if (emptyPosition.Y == 0 && MapCopy[emptyPosition.X, MaxY - 1] == '>')
                    {
                        somethingMoved = true;
                        Map[emptyPosition.X, MaxY - 1] = '.';
                        Map[emptyPosition.X, emptyPosition.Y] = '>';
                        emptyPosition.Y = MaxY - 1;
                    }
                    else if (emptyPosition.Y != 0 && MapCopy[emptyPosition.X, emptyPosition.Y - 1] == '>')
                    {
                        somethingMoved = true;
                        Map[emptyPosition.X, emptyPosition.Y - 1] = '.';
                        Map[emptyPosition.X, emptyPosition.Y] = '>';
                        emptyPosition.Y--;
                    }
                }
                //OutputMap(stepNumber * 0.5m);
                MapCopy = Map.Clone() as char[,];
                foreach (var emptyPosition in Queue)
                {
                    if (emptyPosition.X == 0 && MapCopy[MaxX - 1, emptyPosition.Y] == 'v')
                    {
                        somethingMoved = true;
                        Map[MaxX - 1, emptyPosition.Y] = '.';
                        Map[emptyPosition.X, emptyPosition.Y] = 'v';
                        emptyPosition.X = MaxX - 1;
                    }
                    else if (emptyPosition.X != 0 && MapCopy[emptyPosition.X - 1, emptyPosition.Y] == 'v')
                    {
                        somethingMoved = true;
                        Map[emptyPosition.X - 1, emptyPosition.Y] = '.';
                        Map[emptyPosition.X, emptyPosition.Y] = 'v';
                        emptyPosition.X--;
                    }
                }
                stepNumber++;
                //OutputMap(stepNumber);
            }
            OutputMap(stepNumber);
            Console.WriteLine(stepNumber);
        }

        public void Task2()
        {
            throw new NotImplementedException();
        }
    }
}
