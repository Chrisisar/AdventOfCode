using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day14 : IDay
    {
        List<Robot> robots = new List<Robot>();
        int maxH = 103;
        int maxW = 101;

        public Day14(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            Regex digits = new Regex("-{0,1}\\d+");
            foreach (var line in lines)
            {
                var matches = digits.Matches(line);
                var newRobot = new Robot
                {
                    StartingPosition = (int.Parse(matches[0].Value), int.Parse(matches[1].Value)),
                    Velocity = (int.Parse(matches[2].Value), int.Parse(matches[3].Value)),

                };
                robots.Add(newRobot);
            }
        }

        public void Task1()
        {
            foreach (var robot in robots)
            {
                robot.CurrentPosition = ((robot.StartingPosition.X + (100 * robot.Velocity.X) % maxW) % maxW, (robot.StartingPosition.Y + (100 * robot.Velocity.Y) % maxH) % maxH);
                if (robot.CurrentPosition.X < 0)
                {
                    robot.CurrentPosition.X = maxW + robot.CurrentPosition.X;
                }
                if (robot.CurrentPosition.Y < 0)
                {
                    robot.CurrentPosition.Y = maxH + robot.CurrentPosition.Y;
                }
            }
            int topLeftQuadrantAmount = robots.Count(r => r.CurrentPosition.X < maxW / 2 && r.CurrentPosition.Y < maxH / 2);
            int topRightQuadrantAmount = robots.Count(r => r.CurrentPosition.X > maxW / 2 && r.CurrentPosition.Y < maxH / 2);
            int bottomLeftQuadrantAmount = robots.Count(r => r.CurrentPosition.X < maxW / 2 && r.CurrentPosition.Y > maxH / 2);
            int bottomRightQuadrantAmount = robots.Count(r => r.CurrentPosition.X > maxW / 2 && r.CurrentPosition.Y > maxH / 2);
            Console.WriteLine(topRightQuadrantAmount * topLeftQuadrantAmount * bottomLeftQuadrantAmount * bottomRightQuadrantAmount);
        }

        public void Task2()
        {
            foreach (var robot in robots)
            {
                robot.CurrentPosition = (robot.StartingPosition.X, robot.StartingPosition.Y);
            }
            var result = 1;
            while (true)
            {
                foreach (var robot in robots)
                {
                    robot.CurrentPosition = ((robot.CurrentPosition.X + robot.Velocity.X) % maxW, (robot.CurrentPosition.Y + robot.Velocity.Y) % maxH);
                    if (robot.CurrentPosition.X < 0)
                    {
                        robot.CurrentPosition.X = maxW + robot.CurrentPosition.X;
                    }
                    if (robot.CurrentPosition.Y < 0)
                    {
                        robot.CurrentPosition.Y = maxH + robot.CurrentPosition.Y;
                    }
                }
                if (robots.Select(r => r.CurrentPosition).Distinct().Count() == robots.Count())
                {
                    PrintMap();
                    //Set breakpoint here and check visuals for a Christmas Tree
                }
                result++;
            }
        }

        void PrintMap()
        {
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    Console.Write(robots.Any(r => r.CurrentPosition.X == j && r.CurrentPosition.Y == i) ? robots.Count(r => r.CurrentPosition.X == j && r.CurrentPosition.Y == i) : ".");
                }
                Console.WriteLine();
            }
        }

        class Robot
        {
            public (int X, int Y) StartingPosition;
            public (int X, int Y) Velocity;
            public (int X, int Y) CurrentPosition;
        }
    }
}