using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2025
{
    class Day05 : IDay
    {
        List<(long start, long end)> freshLimits = new List<(long start, long end)>();
        List<long> products = new List<long>();

        public Day05(string inputFilePath)
        {
            ParseInput(inputFilePath);
            //Debug();
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            int i = 0;
            while (!string.IsNullOrWhiteSpace(lines[i]))
            {
                var splitLine = lines[i].Split('-');
                long.TryParse(splitLine[0], out long start);
                long.TryParse(splitLine[1], out long end);
                TryAddNewLimit(start, end);
                i++;
            }
            i++;
            while (i < lines.Length)
            {
                long.TryParse(lines[i], out long productId);
                products.Add(productId);
                i++;
            }
        }

        private void TryAddNewLimit(long start, long end)
        {
            var overlaps = freshLimits.Where(x => x.start <= start && x.end >= start
                                            || x.start <= end && x.end >= end
                                            || x.start >= start && x.end <= end).ToList();
            freshLimits = freshLimits.Except(overlaps).ToList();
            overlaps.Add((start, end));
            freshLimits.Add((overlaps.Min(x => x.start), overlaps.Max(x => x.end)));
        }

        public void Task1()
        {
            var result = 0;
            foreach (var product in products)
            {
                if(freshLimits.Any(fl => fl.start <= product && fl.end >= product))
                {
                    result++;
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            long result = 0;
            foreach (var limit in freshLimits)
            {
                if (limit.end >= limit.start)
                    result += limit.end - limit.start + 1;
            }
            Console.WriteLine(result);
        }

        void Debug()
        {
            freshLimits = freshLimits.OrderBy(x => x.start).ToList();
            foreach(var limit in freshLimits)
            {
                Console.WriteLine($"\"({limit.start},{limit.end})\"");
            }
        }
    }
}