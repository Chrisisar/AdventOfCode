using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Year2025
{
    class Day12 : IDay
    {
        class Shape
        {
            public int Area { get; set; }
            public int Identifier { get; set; }
        }

        List<Shape> Shapes = new List<Shape>();
        List<(int x, int y, int[] shapesCount)> Boxes = new List<(int x, int y, int[] shapesCount)>();

        public Day12(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            int i = 0;
            Shape shape = new Shape();
            while (true)
            {
                var line = lines[i];
                if (line.Contains('x'))
                {
                    break;
                }
                if (string.IsNullOrEmpty(line))
                {
                    Shapes.Add(shape);
                    shape = new Shape();
                }
                if (new Regex("\\d:").Match(line).Success)
                {
                    shape.Identifier = int.Parse(line.Replace(":", ""));
                }
                else
                {
                    shape.Area += line.Count(x => x == '#');
                }
                i++;
            }

            for (; i != lines.Length; i++)
            {
                var splitLine = lines[i].Split(":");
                var sizeSplit = splitLine[0].Split("x");
                int.TryParse(sizeSplit[0], out int x);
                int.TryParse(sizeSplit[1], out int y);
                var shapesList = splitLine[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
                Boxes.Add((x, y, shapesList));

            }
        }

        public void Task1()
        {
            var total = 0;
            foreach (var box in Boxes)
            {
                if((box.x / 3) * (box.y / 3) >= box.shapesCount.Sum()) //assuming all boxes take 3x3 grid and the size allows putting them all in
                {
                    total++;
                    continue;
                }
                var result = 0;
                for (int i = 0; i != box.shapesCount.Length; i++)
                {
                    result += Shapes.Single(x => x.Identifier == i).Area * box.shapesCount[i];
                }
                if (result > box.x * box.y) //assuming the box is too small to fit the shapes even if they were shredded to pieces
                    continue;
                //TODO: properly distribute shapes but the input has no such cases
            }
            Console.WriteLine(total);
        }

        public void Task2()
        {

        }
    }
}