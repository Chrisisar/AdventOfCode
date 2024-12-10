using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day01 : IDay
    {
        private List<int> CalculateElfCalories()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            int currentElfCalories = 0;
            List<int> elfCalories = new List<int>();
            for (int i = 0; i != lines.Count(); i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    int.TryParse(lines[i], out int calories);
                    currentElfCalories += calories;
                }
                else
                {
                    elfCalories.Add(currentElfCalories);
                    currentElfCalories = 0;
                }
            }
            return elfCalories;
        }

        public void Task1()
        {            
            Console.WriteLine(CalculateElfCalories().Max());
        }

        public void Task2()
        {
            List<int> elfCalories = CalculateElfCalories().OrderByDescending(x=>x).ToList();
            Console.WriteLine(elfCalories[0] + elfCalories[1] + elfCalories[2]);
        }
    }
}
