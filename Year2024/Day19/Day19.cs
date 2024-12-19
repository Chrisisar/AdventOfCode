using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day19 : IDay
    {
        List<string> patterns = new List<string>();
        List<string> designs = new List<string>();
        Dictionary<string, long> testedDesigns = new Dictionary<string, long>();


        public Day19(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            var splitPatterns = lines[0].Split(", ");
            foreach (var splitPattern in splitPatterns)
            {
                patterns.Add(splitPattern);
            }
            for (int i = 2; i != lines.Length; i++)
            {
                designs.Add(lines[i]);
            }
        }

        public void Task1()
        {
            long result = 0;
            var impossibleDesigns = new List<string>();
            foreach (var design in designs)
            {
                long possibleAlignments = CheckIfDesignPossible(design, true);
                if (possibleAlignments == 0)
                {
                    impossibleDesigns.Add(design);
                }
                result += possibleAlignments;
            }
            designs = designs.Except(impossibleDesigns).ToList();
            Console.WriteLine(result);
        }

        public void Task2()
        {
            long result = 0;
            foreach (var design in designs)
            {
                result += CheckIfDesignPossible(design, false);
            }
            Console.WriteLine(result);
        }

        long CheckIfDesignPossible(string design, bool isPartOne)
        {
            if(testedDesigns.TryGetValue(design, out long previousResult))
            {
                return previousResult;
            }
            long result = 0;
            foreach (var pattern in patterns)
            {
                if (isPartOne && result > 0) return 1;
                if (design == pattern)
                {
                    result++;
                }
                else if (design.StartsWith(pattern))
                {
                    result += CheckIfDesignPossible(design.Substring(pattern.Length), isPartOne);
                }
            }
            testedDesigns.Add(design, result);
            return result;
        }
    }
}