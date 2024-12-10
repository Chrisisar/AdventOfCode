using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day7 : IDay
    {

        private List<int> Numbers;
        private int CalculateForPosition(int position)
        {
            int result = 0;
            foreach (int number in Numbers)
            {
                result += Math.Abs(number - position);
            }
            return result;
        }

        private int CalculateForPosition2(int position)
        {
            int result = 0;
            foreach (int number in Numbers)
            {
                int distance = Math.Abs(number - position);
                result += distance * (distance + 1) / 2;
            }
            return result;
        }

        public int FindLowestValue(int minPosition, int maxPosition, Func<int, int> funcToRun)
        {
            if(minPosition == maxPosition)
            {
                return funcToRun(minPosition);
            }
            int middlePosition = (int)(minPosition + (maxPosition - minPosition) * 0.5);
            if(funcToRun(middlePosition) > funcToRun(middlePosition + 1))
            {
                return FindLowestValue(middlePosition + 1, maxPosition, funcToRun);
            }
            else
            {
                return FindLowestValue(minPosition, middlePosition, funcToRun);
            }
        }

        public void Task1()
        {
            var lines = File.ReadAllLines("../../../Day7/Day7Input.txt");
            int sum = 0;
            Numbers = lines[0].Split(",").Select(x=> int.Parse(x)).ToList();
            Console.WriteLine(FindLowestValue(Numbers.Min(), Numbers.Max(),CalculateForPosition));
        }

        public void Task2()
        {
            var lines = File.ReadAllLines("../../../Day7/Day7Input.txt");
            int sum = 0;
            Numbers = lines[0].Split(",").Select(x => int.Parse(x)).ToList();
            Console.WriteLine(FindLowestValue(Numbers.Min(), Numbers.Max(), CalculateForPosition2));
        }
    }
}
