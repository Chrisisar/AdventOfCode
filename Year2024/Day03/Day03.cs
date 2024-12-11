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
    class Day03 : IDay
    {
        public Day03(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            line = string.Join("", lines);
        }

        string line;

        public void Task1()
        {
            Console.WriteLine(CalculateResult(line));
        }

        public void Task2()
        {
            Regex dontdoPattern = new Regex("don't\\(\\)(.*?)do\\(\\)");
            line = dontdoPattern.Replace(line, string.Empty);

            Console.WriteLine(CalculateResult(line));
        }

        private long CalculateResult(string input)
        {
            long result = 0;
            Regex mulpattern = new Regex("mul\\((\\d+,\\d+)\\)");
            foreach (Match match in mulpattern.Matches(input))
            {
                var splitMatch = match.Groups[1].Value.Split(',');
                result += long.Parse(splitMatch[0]) * long.Parse(splitMatch[1]);
            }
            return result;
        }
    }
}