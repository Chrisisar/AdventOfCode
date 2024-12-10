using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2023
{
    class Day01 : IDay
    {
        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            var sum = 0;
            foreach (var line in lines)
            {
                int firstDigit = -1;
                int lastDigit = -1;
                for (int i = 0; i != line.Length; i++)
                {
                    if (line[i] >= '0' && line[i] <= '9')
                    {
                        if (firstDigit == -1)
                        {
                            int.TryParse(line[i].ToString(), out firstDigit);
                        }
                        int.TryParse(line[i].ToString(), out lastDigit);
                    }
                }
                sum += firstDigit * 10 + lastDigit;
            }
            Console.WriteLine(sum);
        }

        public void Task2()
        {
            var digitNames = new List<string>() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            var lines = StaticHelpers.GetLines(this.GetType()).ToList();
            var sum = 0;
            for(int l = 0; l != lines.Count; l++)
            {
                string line = lines[l];
                int firstNameIndex = int.MaxValue;
                int firstNameDigit = -1;
                int lastNameIndex = -1;
                int lastNameDigit = -1;
                for (int i = 1; i <= 9; i++)
                {
                    int indexOfDigit = line.IndexOf(digitNames[i - 1]);
                    if (indexOfDigit != -1 && indexOfDigit < firstNameIndex)
                    {
                        firstNameIndex = indexOfDigit;
                        firstNameDigit = i;
                    }
                    int lastIndexOfDigit = line.LastIndexOf(digitNames[i - 1]);
                    if (lastIndexOfDigit != -1 && lastIndexOfDigit > lastNameIndex)
                    {
                        lastNameIndex = lastIndexOfDigit;
                        lastNameDigit = i;
                    }
                    //line = line.Replace(digitNames[i-1], $"{digitNames[i-1]}{i}{digitNames[i-1]}");
                }
                lines[l] = line;
                int firstDigit = -1;
                int lastDigit = -1;
                for (int i = 0; i != line.Length; i++)
                {
                    if (line[i] >= '0' && line[i] <= '9')
                    {
                        if (firstDigit == -1)
                        {
                            firstDigit = i > firstNameIndex ? firstNameDigit : int.Parse(line[i].ToString());
                            //int.TryParse(line[i].ToString(), out firstDigit);
                        }
                        if (i > lastNameIndex)
                        {
                            int.TryParse(line[i].ToString(), out lastDigit);
                        }
                    }
                }
                if (firstDigit == -1)
                {
                    firstDigit = firstNameDigit;
                }
                if (lastDigit == -1)
                {
                    lastDigit = lastNameDigit;
                }
                int tempSum = firstDigit * 10 + lastDigit;
                Console.WriteLine(tempSum);
                if(tempSum < 0)
                {
                    Console.WriteLine("TEMPSUM NEGATIVE");
                }
                sum += tempSum;
            }
            Console.WriteLine(sum);
        }
    }
}