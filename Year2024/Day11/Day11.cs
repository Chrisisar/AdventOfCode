using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode.Year2024
{
    class Day11 : IDay
    {
        public Day11(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        List<long> originalStones = new List<long>();

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            var splitLine = lines[0].Split(' ');
            foreach (var stone in splitLine)
            {
                originalStones.Add(long.Parse(stone));
            }
        }

        public void Task1()
        {
            List<long> stones = originalStones.ToList();

            for (int i = 0; i != 25; i++)
            {
                List<long> newStones = new List<long>();
                foreach (var stone in stones)
                {
                    UpdateNewStone(stone, newStones);
                }
                stones = newStones;
            }
            Console.WriteLine(stones.Count);
        }

        private void UpdateNewStone(long stone, List<long> newStones)
        {
            string stoneString = stone.ToString();
            if (stone == 0)
            {
                newStones.Add(1);
            }
            else if (stoneString.Length % 2 == 0)
            {
                newStones.Add(long.Parse(stoneString.Substring(0, stoneString.Length / 2)));
                newStones.Add(long.Parse(stoneString.Substring(stoneString.Length / 2)));
            }
            else
            {
                newStones.Add(stone * 2024);
            }
        }


        private Dictionary<long, long> UpdateNewStones(Dictionary<long, long> stones)
        {
            Dictionary<long, long> newStones = new Dictionary<long, long>();
            foreach (var stone in stones.Keys)
            {
                string stoneString = stone.ToString();
                if (stone == 0)
                {
                    if(newStones.ContainsKey(1))
                    {
                        newStones[1] += stones[0];
                    }
                    else
                    {
                        newStones.Add(1, stones[0]);
                    }
                }
                else if (stoneString.Length % 2 == 0)
                {
                    long value1 = long.Parse(stoneString.Substring(0, stoneString.Length / 2));
                    long value2 = long.Parse(stoneString.Substring(stoneString.Length / 2));
                    if (newStones.ContainsKey(value1))
                    {
                        newStones[value1] += stones[stone];
                    }
                    else
                    {
                        newStones.Add(value1, stones[stone]);
                    }
                    if (newStones.ContainsKey(value2))
                    {
                        newStones[value2] += stones[stone];
                    }
                    else
                    {
                        newStones.Add(value2, stones[stone]);
                    }
                }
                else
                {
                    if (newStones.ContainsKey(stone * 2024))
                    {
                        newStones[stone * 2024] += stones[stone];
                    }
                    else
                    {
                        newStones.Add(stone * 2024, stones[stone]);
                    }
                }
            }
            return newStones;
        }

        public void Task2()
        {
            Dictionary<long, long> stonesDict = new Dictionary<long, long>();
            foreach (var stone in originalStones)
            {
                stonesDict.Add(stone, 1);
            }
            for (int i = 0; i != 75; i++)
            {
                stonesDict = UpdateNewStones(stonesDict);
            }
            Console.WriteLine(stonesDict.Sum(x=>x.Value));
        }
    }
}