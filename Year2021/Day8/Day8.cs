using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day8 : IDay
    {
        public void Task1()
        {
            var lines = File.ReadAllLines("../../../Day8/Day8Input.txt");
            int result = 0;
            foreach (var line in lines)
            {
                line.Split("|")[1].Split(" ", StringSplitOptions.TrimEntries).ToList().ForEach(x =>
                {
                    switch (x.Length)
                    {
                        case 2:
                        case 3:
                        case 4:
                        case 7:
                            result++;
                            break;
                    }
                });
            }
            Console.WriteLine(result);
        }


        private int CalculateRow(string line)
        {
            // 00 
            //1  2
            // 33
            //4  5
            // 66
            char[] dashes = new char[7];
            string[] codes = new string[10];

            var splitLine = line.Split(" | ");
            var digits = splitLine[0].Split(" ");
            codes[1] = digits.Single(x => x.Length == 2); //1
            codes[7] = digits.Single(x => x.Length == 3); //7
            codes[4] = digits.Single(x => x.Length == 4); //4
            codes[8] = digits.Single(x => x.Length == 7); //8
            var sixDashesNumbers = digits.Where(x => x.Length == 6); //6,9,0
            var fiveDashesNumbers = digits.Where(x => x.Length == 5); //2,3,5

            dashes[0] = codes[7].Except(codes[1]).Single();
            codes[6] = sixDashesNumbers.Single(x => codes[1].Any(y => !x.Contains(y)));
            dashes[2] = codes[8].Except(codes[6]).Single();
            dashes[5] = codes[1].Single(x => x != dashes[2]);
            codes[5] = fiveDashesNumbers.Single(x => codes[6].Except(x.ToList()).Count() == 1);
            dashes[4] = codes[6].Except(codes[5]).Single();
            codes[9] = sixDashesNumbers.Single(x => !x.Contains(dashes[4]));
            codes[0] = sixDashesNumbers.Single(x => x != codes[9] && x != codes[6]);
            dashes[3] = codes[8].Except(codes[0]).Single();
            dashes[1] = codes[4].Except(codes[1]).Single(x => x != dashes[3]);
            dashes[6] = codes[8].Except(new List<char> { dashes[0], dashes[1], dashes[2], dashes[3], dashes[4], dashes[5] }).Single();
            codes[3] = new string(codes[9].Except(new List<char> { dashes[1] }).ToArray());
            codes[2] = new string(codes[8].Except(new List<char> { dashes[1], dashes[5] }).ToArray());

            var splitNumbers = splitLine[1].Split(" ");
            int result = 0;
            for (int i = 0; i != 4; i++)
            {
                int powerOfTen = (int)Math.Pow(10, 3 - i);
                for (int j = 0; j != 10; j++)
                {
                    if (splitNumbers[i].Length == codes[j].Length && splitNumbers[i].Except(codes[j]).Count() == 0)
                    {
                        result += j * powerOfTen;
                        break;
                    }
                }
            }
            return result;
        }

        public void Task2()
        {
            var lines = File.ReadAllLines("../../../Day8/Day8Input.txt");
            int result = 0;
            foreach (var line in lines)
            {
                int midResult = CalculateRow(line);
               // Console.WriteLine(midResult);
                result += midResult;
            }
            Console.WriteLine(result);
        }
    }
}
