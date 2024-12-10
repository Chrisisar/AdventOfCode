using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day06 : IDay
    {
        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            Console.WriteLine(FindMarkerForLength(4, lines[0]));
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType()).ToList();
            Console.WriteLine(FindMarkerForLength(14, lines[0]));
        }

        private int FindMarkerForLength(int length, string text)
        {
            List<char> characters = new List<char>(14);
            for (int i = 0; i != text.Length; i++)
            {
                if (characters.Count == length)
                {
                    return i;
                }
                var indexOfDuplicate = characters.IndexOf(text[i]);
                if (indexOfDuplicate >= 0)
                {
                    characters = characters.Skip(indexOfDuplicate + 1).ToList();
                }
                characters.Add(text[i]);
            }
            return -1;
        }
    }
}
