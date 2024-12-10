using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day20 : IDay
    {
        class Number
        {
            public long Value;
            public bool AlreadyUsed;
        }

        List<Number> initialNumbers = new List<Number>();
        List<Number> currentNumbers = new List<Number>();

        void InitializeLists(long modifier)
        {
            initialNumbers = new List<Number>();
            currentNumbers = new List<Number>();
            var lines = StaticHelpers.GetLines(this.GetType());
            foreach (var line in lines)
            {
                var newNumber = new Number
                {
                    Value = long.Parse(line) * modifier,
                    AlreadyUsed = false
                };
                initialNumbers.Add(newNumber);
                currentNumbers.Add(newNumber);
            }
        }

        private void MixItems()
        {
            foreach (var number in initialNumbers)
            {
                int index = currentNumbers.IndexOf(number);
                currentNumbers.RemoveAt(index);
                int newIndex = (int)((index + number.Value) % currentNumbers.Count);

                if (newIndex < 0)
                {
                    newIndex = currentNumbers.Count + newIndex;
                }
                currentNumbers.Insert(newIndex, number);
                number.AlreadyUsed = true;
            }
        }

        long CalculateResult()
        {
            long result = 0;
            int indexOfZero = currentNumbers.FindIndex(x => x.Value == 0);
            for (int i = 0; i != 3; i++)
            {
                indexOfZero = (indexOfZero + 1000) % initialNumbers.Count;
                result += currentNumbers[indexOfZero].Value;
            }
            return result;
        }

        public void Task1()
        {
            InitializeLists(1);
            MixItems();
            Console.WriteLine(CalculateResult());
        }

        public void Task2()
        {
            InitializeLists(811589153);
            for (int i = 0; i != 10; i++)
            {
                MixItems();
            }
            Console.WriteLine(CalculateResult());
        }
    }
}
