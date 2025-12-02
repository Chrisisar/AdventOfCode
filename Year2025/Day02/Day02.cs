using AdventOfCode.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2025
{
    class Day02 : IDay
    {
        List<(string start, string end)> ranges = new List<(string, string)>();

        public Day02(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            var split = lines[0].Split(',');
            foreach (var splitItem in split)
            {
                var rangeItems = splitItem.Split("-");
                ranges.Add((rangeItems[0], rangeItems[1]));
            }
        }

        public void Task1()
        {
            long numberOfInvalidIds = 0;
            foreach (var range in ranges)
            {
                var invalidIds = CalculateInvalidIds(range.start, range.end);
                numberOfInvalidIds += invalidIds;
                //Console.WriteLine($"{range.start}-{range.end} -> {invalidIds} -> {numberOfInvalidIds}");
            }
            Console.WriteLine(numberOfInvalidIds);
        }

        public void Task2()
        {
            long numberOfInvalidIds = 0;
            foreach (var range in ranges)
            {
                var invalidIds = CalculateInvalidIds2(range.start, range.end);
                numberOfInvalidIds += invalidIds;
                Console.WriteLine($"{range.start}-{range.end} -> {invalidIds} -> {numberOfInvalidIds}");
            }
            Console.WriteLine(numberOfInvalidIds);
        }

        private long CalculateInvalidIds(string start, string end)
        {
            if (start.Length == end.Length && end.Length % 2 == 1)
                return 0; //Skip odd lengths

            var startLong = NormalizeStart(start);
            var endLong = NormalizeEnd(end);

            if (startLong > endLong)
                return 0; //skip empty

            long result = 0;
            //return endLong - startLong + 1; //numberOfInvalidIDs
            for (long i = startLong; i <= endLong; i++)
            {
                long.TryParse(i.ToString() + i.ToString(), out long iCalc);
                result += iCalc;
            }
            return result;
        }

        private long CalculateInvalidIds2(string start, string end)
        {
            long result = 0;
            long.TryParse(start, out long startLong);
            long.TryParse(end, out long endLong);

            for (long i = startLong; i <= endLong; i++)
            {
                if (IsInvalidId(i))
                {
                    result += i;
                }
            }
            return result;
        }

        private bool IsInvalidId(long id)
        {
            string stringId = id.ToString();
            int halfLength = stringId.Length / 2;
            for (int i = 1; i <= halfLength; i++)
            {
                bool shouldSkip = false;
                if (stringId.Length % i == 0)
                {
                    var firstSubString = stringId.Substring(0, i);
                    for (int j = i; j < stringId.Length; j += i)
                    {
                        if (stringId.Substring(j, i) != firstSubString)
                        {
                            shouldSkip = true;
                            break;
                        }
                    }
                    if (!shouldSkip)
                        return true;
                }
            }
            return false;
        }

        private long NormalizeStart(string start)
        {
            if (start.Length % 2 == 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("1");
                for (int i = 0; i < start.Length; i++)
                {
                    sb.Append("0");
                }
                start = sb.ToString();
            }

            int halfLength = start.Length / 2;
            long.TryParse(start.Substring(0, halfLength), out long firstHalf);
            long.TryParse(start.Substring(halfLength), out long secondHalf);

            if (firstHalf < secondHalf)
            {
                firstHalf++;
            }
            secondHalf = firstHalf;
            return firstHalf;
        }

        private long NormalizeEnd(string end)
        {
            if (end.Length % 2 == 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("9");
                for (int i = 2; i < end.Length; i++)
                {
                    sb.Append("9");
                }
                end = sb.ToString();
            }

            int halfLength = end.Length / 2;
            long.TryParse(end.Substring(0, halfLength), out long firstHalf);
            long.TryParse(end.Substring(halfLength), out long secondHalf);

            if (firstHalf > secondHalf)
            {
                firstHalf--;
            }
            secondHalf = firstHalf;
            return firstHalf;
        }
    }
}