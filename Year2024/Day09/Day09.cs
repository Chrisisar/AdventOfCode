using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day09 : IDay
    {
        string filePath;
        int[] numbers = new int[200000];
        int index = 0;

        public Day09(string inputFilePath)
        {
            filePath = inputFilePath;
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            index = 0;
            var lines = StaticHelpers.GetLines(inputFilePath);
            bool isBlank = false;
            int id = 0;
            for (int i = 0; i < lines[0].Length; i++)
            {
                int number = int.Parse(lines[0][i].ToString());
                for (int j = 0; j < number; j++)
                {
                    numbers[index] = isBlank ? -1 : id;
                    index++;
                }
                if (!isBlank)
                {
                    id++;
                }
                isBlank = !isBlank;
            }
        }

        public void Task1()
        {
            int startIndex = 0;
            int endIndex = index - 1;
            while (startIndex <= endIndex)
            {
                if (numbers[startIndex] >= 0)
                {
                    startIndex++;
                }
                else if (numbers[endIndex] < 0)
                {
                    endIndex--;
                }
                else
                {
                    numbers[startIndex] = numbers[endIndex];
                    numbers[endIndex] = -1;
                }
            }
            Console.WriteLine(CalculateScore());
        }

        public void Task2()
        {
            ParseInput(filePath);
            int endIndex = index - 1;
            while (endIndex > 0)
            {
                while (numbers[endIndex] == -1)
                {
                    endIndex--;
                }
                int currentNumber = numbers[endIndex];
                int endNumberLength = 0;
                while (endIndex >= 0 && numbers[endIndex] == currentNumber)
                {
                    endIndex--;
                    endNumberLength++;
                }

                int startIndex = 0;
                int startNumberLength = 0;
                while (startIndex < endIndex)
                {
                    while (numbers[startIndex] == -1)
                    {
                        startIndex++;
                        startNumberLength++;
                    }
                    if (startNumberLength >= endNumberLength)
                    {
                        for (int i = 0; i < endNumberLength; i++)
                        {
                            numbers[startIndex - startNumberLength + i] = currentNumber;
                            numbers[endIndex + i + 1] = -1;
                        }
                        break;
                    }
                    else
                    {
                        startNumberLength = 0;
                        startIndex++;
                    }
                }
            }
            Console.WriteLine(CalculateScore());
        }

        private void PrintNumbers()
        {
            for (int i = 0; i != index; i++)
            {
                Console.Write(numbers[i] < 0 ? "." : numbers[i]);
            }
            Console.WriteLine();
        }

        private long CalculateScore()
        {
            long result = 0;
            for (int i = 0; i < 200000; i++)
            {
                if (numbers[i] >= 0)
                    result += i * numbers[i];
            }
            return result;
        }
    }
}