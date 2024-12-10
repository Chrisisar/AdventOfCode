using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day6 : IDay
    {
        public void Task1()
        {
            var lines = File.ReadAllLines("../../../Day6/Day6Input.txt");
            List<int> fishes = lines[0].Split(",").Select(x => int.Parse(x)).ToList();
            for (int day = 0; day != 80; day++)
            {
                for (int i = 0; i != fishes.Count; i++)
                {
                    if (fishes[i] == 0)
                    {
                        fishes.Add(9);
                        fishes[i] = 6;
                    }
                    else
                    {
                        fishes[i] = fishes[i] - 1;
                    }
                }
            }
            Console.WriteLine(fishes.Count);
        }

        public void Task2()
        {
            var lines = File.ReadAllLines("../../../Day6/Day6Input.txt");
            long[] buckets = new long[10];
            lines[0].Split(",").ToList().ForEach(x => buckets[int.Parse(x)]++);

            for (int day = 0; day != 256; day++)
            {
                long zeros = buckets[0];
                for (int i = 0; i <= 8; i++)
                {
                    buckets[i] = buckets[i + 1];
                }
                buckets[8] = zeros;
                buckets[6] += zeros;
            }

            long result = 0;
            for(int i = 0;i <=8;i++)
            {
                result += buckets[i];
            }
            Console.WriteLine(result);
        }
    }
}
