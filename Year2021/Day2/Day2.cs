using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day2 : IDay
    {
        public void Task1()
        {
            int depth = 0;
            int horizontal = 0;
            var lines = File.ReadAllLines("../../../Day2/Day2Input.txt");
            foreach(var line in lines)
            {
                var splitLine = line.Split(" ");
                int.TryParse(splitLine[1], out int value);
                switch (splitLine[0])
                {
                    case "forward":
                        horizontal += value;
                        break;
                    case "up":
                        depth -= value;
                        break;
                    case "down":
                        depth += value;
                        break;
                }
            }
            Console.WriteLine(depth * horizontal);
        }

        public void Task2()
        {
            int depth = 0;
            int horizontal = 0;
            int aim = 0;
            var lines = File.ReadAllLines("../../../Day2/Day2Input.txt");
            foreach (var line in lines)
            {
                var splitLine = line.Split(" ");
                int.TryParse(splitLine[1], out int value);
                switch (splitLine[0])
                {
                    case "forward":
                        horizontal += value;
                        depth += aim * value;
                        break;
                    case "up":
                        aim -= value;
                        break;
                    case "down":
                        aim += value;
                        break;
                }
            }
            Console.WriteLine(depth * horizontal);
        }
    }
}
