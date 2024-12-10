using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day10 : IDay
    {
        private long CalculateNextCycle(ref int cycle, long value)
        {
            cycle++;
            if(cycle % 40 == 20)
            {
                //Console.WriteLine($"Cycle: {cycle}, Value: {value}, Result: {cycle * value}" );
                return cycle * value;
            }
            return 0;
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            var cycle = 0;
            long xRegister = 1;
            foreach(var line in lines)
            {
                var splitLine = line.Split(' ');
                Task2Cycle(ref cycle, xRegister);
                switch (splitLine[0])
                {
                    case "addx":
                        Task2Cycle(ref cycle, xRegister);
                        xRegister += long.Parse(splitLine[1]);
                        break;
                    case "noop":
                        break;
                }
            }
        }

        private void Task2Cycle(ref int cycle, long value) 
        {
            cycle++;
            var crtColumn = cycle % 40;
            //Console.WriteLine($"cycle: {cycle}, value: {value}, {(crtColumn >= value && crtColumn <= value + 2 ? "#" : ".")}");
            Console.Write(crtColumn >= value && crtColumn <= value + 2 ? "#" : ".");
            if (cycle % 40 == 0)
            {
                Console.WriteLine();
            }
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            var cycle = 1;
            long xRegister = 1;
            long result = 0;
            foreach (var line in lines)
            {
                var splitLine = line.Split(' ');
                result += CalculateNextCycle(ref cycle, xRegister);
                switch (splitLine[0])
                {
                    case "addx":
                        xRegister += long.Parse(splitLine[1]);
                        //Console.WriteLine($"{long.Parse(splitLine[1])} -> {xRegister}");
                        result += CalculateNextCycle(ref cycle, xRegister);
                        break;
                    case "noop":
                        break;
                }
            }
            Console.WriteLine(result);
        }
    }
}
