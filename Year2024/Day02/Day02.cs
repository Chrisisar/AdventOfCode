using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day02 : IDay
    {
        List<List<int>> entries = new List<List<int>>();
        public Day02(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach (var line in lines)
            {
                List<int> entry = new List<int>();
                var splitLine = line.Split(' ');
                foreach (var item in splitLine)
                {
                    entry.Add(int.Parse(item));
                }
                entries.Add(entry);
            }
        }

        public void Task1()
        {
            long result = 0;
            foreach (var entry in entries)
            {
                bool isValid = IsValidEntry(entry);
                if (isValid)
                {
                    result++;
                    //result += entry[entry.Count / 2];
                }
            }
            Console.WriteLine(result);
        }

        private static bool IsValidEntry(List<int> entry)
        {
            bool areDescending = entry[0] > entry[1];
            bool isValid = true;
            for (int i = 1; i < entry.Count; i++)
            {
                if (!isValid) break;
                isValid = false;
                if (areDescending && entry[i] < entry[i - 1]
                    || !areDescending && entry[i] > entry[i - 1])
                {
                    if (Math.Abs(entry[i] - entry[i - 1]) <= 3)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        public void Task2()
        {
            var result = 0;
            foreach (var entry in entries)
            {
                for (int i = 0; i != entry.Count; i++)
                {
                    var entryCopy = entry.ToList();
                    entryCopy.RemoveAt(i);
                    if(IsValidEntry(entryCopy))
                    {
                        result++;
                        break;
                    }
                }
            }
            Console.WriteLine(result);
        }
    }
}