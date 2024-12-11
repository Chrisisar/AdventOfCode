using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day01 : IDay
    {
        List<int> list1 = new List<int>();
        List<int> list2 = new List<int>();
        public Day01(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach(var line in lines)
            {
                var splitLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                list1.Add(int.Parse(splitLine[0]));
                list2.Add(int.Parse(splitLine[1]));
            }
        }

        public void Task1()
        {
            var orderedList1 = list1.OrderBy(x=>x).ToList();
            var orderedList2 = list2.OrderBy(x=>x).ToList();
            int result = 0;
            for(int i =0; i < orderedList1.Count; i++)
            {
                result += Math.Abs(orderedList1[i] - orderedList2[i]);
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            long result = 0;
            foreach(var item in list1)
            {
                result += item * list2.Count(x => x == item);
            }
            Console.WriteLine(result);
        }
    }
}