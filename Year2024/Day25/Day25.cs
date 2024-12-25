using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day25 : IDay
    {
        List<int[]> locks;
        List<int[]> keys;

        public Day25(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            locks = new List<int[]>();
            keys = new List<int[]>();
            var lines = StaticHelpers.GetLines(inputFilePath);
            for (int i = 0; i < lines.Length; i += 8)
            {
                bool isLock = lines[i].Equals("#####");
                int[] newKeyLock = new int[5];
                for (int j = i + 1; j < i + 6; j++)
                {
                    for(int k =0;k <5;k++)
                    {
                        if (lines[j][k] == '#')
                        {
                            newKeyLock[k]++;
                        }
                    }
                }
                if(isLock)
                {
                    locks.Add(newKeyLock);
                }
                else
                {
                    keys.Add(newKeyLock);
                }

            }
        }

        public void Task1()
        {
            var result = 0;
            for(int i =0; i < keys.Count; i++)
            {
                for(int j =0;j<locks.Count;j++)
                {
                    for(int k =0;k<5;k++)
                    {
                        if (keys[i][k] + locks[j][k] > 5) break;
                        if(k==4)
                        {
                            result++;
                        }
                    }
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {

        }
    }
}