using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day03 : IDay
    {
        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            int result = 0;
            foreach (var line in lines)
            {
                var compartment1 = line.Substring(0, line.Length / 2).ToCharArray();
                var compartment2 = line.Substring(line.Length / 2).ToCharArray();

                var repeatedLetter = compartment1.Intersect(compartment2).First();
                if (repeatedLetter >= 'a' && repeatedLetter <= 'z')
                {
                    result += repeatedLetter - 'a' + 1;
                }
                else
                {
                    result += repeatedLetter - 'A' + 27;
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType()).ToList();
            int result = 0;
            for (int i = 0; i < lines.Count; i += 3)
            {
                var repeatedLetter = lines[i].Intersect(lines[i + 1]).Intersect(lines[i + 2]).FirstOrDefault();
                if (repeatedLetter == default(char))
                {
                    continue;
                }
                if (repeatedLetter >= 'a' && repeatedLetter <= 'z')
                {
                    result += repeatedLetter - 'a' + 1;
                }
                else
                {
                    result += repeatedLetter - 'A' + 27;
                }
                Console.WriteLine(repeatedLetter);

            }
            Console.WriteLine(result);
        }
    }
}
