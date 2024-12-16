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
    class Day13 : IDay
    {
        List<Machine> machines = new List<Machine>()
            ;
        public Day13(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            Machine newMachine = new Machine();
            Regex digits = new Regex("\\d+");
            for (int i = 0; i < lines.Length; i++)
            {
                var matches = digits.Matches(lines[i]);
                switch (i % 4)
                {
                    case 0:
                        newMachine.AX = int.Parse(matches[0].Value);
                        newMachine.AY = int.Parse(matches[1].Value);
                        break;
                    case 1:
                        newMachine.BX = int.Parse(matches[0].Value);
                        newMachine.BY = int.Parse(matches[1].Value);
                        break;
                    case 2:
                        newMachine.PrizeX = int.Parse(matches[0].Value);
                        newMachine.PrizeY = int.Parse(matches[1].Value);
                        break;
                    case 3:
                        machines.Add(newMachine);
                        newMachine = new Machine();
                        break;
                }
            }
            machines.Add(newMachine);
        }

        public void Task1()
        {
            long result = 0;
            foreach (var machine in machines)
            {
                machine.CalculateBestScore();
                result += machine.BestScore != long.MaxValue ? machine.BestScore : 0;
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            long result = 0;
            foreach (var machine in machines)
            {
                machine.PrizeX += 10000000000000;
                machine.PrizeY += 10000000000000;
                machine.BestScore = long.MaxValue;
                machine.CalculateBestScore(true);
                if (machine.BestScore != long.MaxValue)
                {
                    result += machine.BestScore;
                    //Console.WriteLine($"Result found for {machines.IndexOf(machine) + 1} machine.");
                }
                else
                {
                    //Console.WriteLine($"Failed to find a result for {machines.IndexOf(machine) + 1} machine.");
                }
            }
            Console.WriteLine(result);
        }

        class Machine
        {
            public int AX;
            public int AY;
            public int BX;
            public int BY;
            public long PrizeX;
            public long PrizeY;
            public long BestScore = long.MaxValue;

            long HowManyAClicks(long bClicks)
            {
                double result = (0f - (BX - BY) * bClicks + (PrizeX - PrizeY)) / (AX - AY);
                return StaticHelpers.TryConvertingDoubleToLongInteger(result, out long aClicks) ? aClicks : long.MinValue;
            }

            public void CalculateBestScore(bool isPart2 = false)
            {
                long bClicks = 0;
                (long x, long y) firstFind = (long.MinValue, long.MinValue);
                long firstFindAClicks = 0;
                long firstFindBClicks = 0;
                while (true)
                {
                    var aClicks = HowManyAClicks(bClicks);
                    if (aClicks >= 0)
                    {
                        if (firstFind.x == long.MinValue)
                        {
                            firstFindAClicks = aClicks;
                            firstFindBClicks = bClicks;
                            firstFind.x = aClicks * AX + bClicks * BX;
                            firstFind.y = aClicks * AY + bClicks * BY;
                        }
                        else
                        {
                            (long x, long y) secondFind = (aClicks * AX + bClicks * BX, aClicks * AY + bClicks * BY);
                            if (StaticHelpers.TryConvertingDoubleToLongInteger(((double)PrizeX - secondFind.x) / (secondFind.x - firstFind.x), out long multiplier))
                            {
                                long realAClicks = aClicks + (aClicks - firstFindAClicks) * multiplier;
                                long realBClicks = bClicks + (bClicks - firstFindBClicks) * multiplier;
                                if (isPart2 || (realAClicks <= 100 && realBClicks <= 100))
                                    BestScore = realAClicks * 3 + realBClicks;
                                break;
                            }
                            break;
                        }
                    }
                    bClicks++;
                }
            }
        }
    }
}