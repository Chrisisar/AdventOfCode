using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day1 : IDay
    {
        public void Task1()
        {
            int lastDepth = 0;
            int counter = 0;
            var lines = File.ReadAllLines("../../../Day 1/Day1Input.txt");
            foreach (var line in lines)
            {
                int.TryParse(line, out int depth);
                if (depth > lastDepth)
                {
                    counter++;
                }
                lastDepth = depth;
            }
            Console.WriteLine(counter - 1);
        }

        public void Task2()
        {
            int lastDepth = 0;
            int counter = 0;
            var lines = File.ReadAllLines("../../../Day 1/Day1Input.txt");
            for (int i = 2; i != lines.Length; i++)
            {
                int.TryParse(lines[i - 2], out int depth);
                int.TryParse(lines[i - 1], out int depth2);
                int.TryParse(lines[i], out int depth3);
                int depthSum = depth + depth2 + depth3;
                if (depthSum > lastDepth)
                {
                    counter++;
                }
                lastDepth = depthSum;
            }
            Console.WriteLine(counter - 1);
        }
    }
}
