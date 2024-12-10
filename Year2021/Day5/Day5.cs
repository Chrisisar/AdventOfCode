using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day5 : IDay
    {
        public void Task1()
        {
            int[,] field = new int[1000, 1000];
            var lines = File.ReadAllLines("../../../Day5/Day5Input.txt");
            foreach (var line in lines)
            {
                var splitLine = line.Split(" -> ");
                var x1 = int.Parse(splitLine[0].Split(",")[0]);
                var y1 = int.Parse(splitLine[0].Split(",")[1]);
                var x2 = int.Parse(splitLine[1].Split(",")[0]);
                var y2 = int.Parse(splitLine[1].Split(",")[1]);
                if (y1 == y2)
                {
                    for (int i = (x1 < x2) ? x1 : x2; i <= ((x1 < x2) ? x2 : x1); i++)
                    {
                        field[i, y1]++;
                    }
                }
                if (x1 == x2)
                {
                    for (int i = (y1 < y2) ? y1 : y2; i <= ((y1 < y2) ? y2 : y1); i++)
                    {
                        field[x1, i]++;
                    }
                }
            }
            int counter = 0;
            for (int i = 0; i != 1000; i++)
            {
                for (int j = 0; j != 1000; j++)
                {
                    if (field[i, j] > 1)
                    {
                        counter++;
                    }
                }
            }
            Console.WriteLine(counter);
        }

        public void Task2()
        {
            int[,] field = new int[1000, 1000];
            var lines = File.ReadAllLines("../../../Day5/Day5Input.txt");
            foreach (var line in lines)
            {
                var splitLine = line.Split(" -> ");
                var x1 = int.Parse(splitLine[0].Split(",")[0]);
                var y1 = int.Parse(splitLine[0].Split(",")[1]);
                var x2 = int.Parse(splitLine[1].Split(",")[0]);
                var y2 = int.Parse(splitLine[1].Split(",")[1]);
                if (y1 == y2)
                {
                    for (int i = (x1 < x2) ? x1 : x2; i <= ((x1 < x2) ? x2 : x1); i++)
                    {
                        field[i, y1]++;
                    }
                }
                else if (x1 == x2)
                {
                    for (int i = (y1 < y2) ? y1 : y2; i <= ((y1 < y2) ? y2 : y1); i++)
                    {
                        field[x1, i]++;
                    }
                }
                else
                {
                    if(x1 < x2 && y1 < y2 || x2 < x1 && y2 < y1)
                    {
                        for (int i = (x1 < x2) ? x1 : x2, j = (y1 < y2) ? y1 : y2; i <= ((x1 < x2) ? x2 : x1); i++, j++)
                        {
                            field[i, j]++;
                        }
                    }
                    else
                    {
                        for (int i = (x1 < x2) ? x1 : x2, j = (y1 > y2) ? y1 : y2; i <= ((x1 < x2) ? x2 : x1); i++, j--)
                        {
                            field[i, j]++;
                        }
                    }
                }
            }
            int counter = 0;
            for (int j = 0; j != 1000; j++)
            {
                for (int i = 0; i != 1000; i++)
                {
                    //Console.Write(field[i, j] != 0 ? field[i,j] : ".");
                    if (field[i, j] > 1)
                    {
                        counter++;
                    }
                }
                //Console.WriteLine();
            }
            Console.WriteLine(counter);
        }
    }
}
