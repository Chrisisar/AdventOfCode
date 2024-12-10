using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day3 : IDay
    {
        int binaryLength = 12;
        public void Task1()
        {
            int[] sumsOfBits = new int[binaryLength];
            var lines = File.ReadAllLines("../../../Day3/Day3Input.txt");
            foreach (var line in lines)
            {
                for (int i = 0; i != binaryLength; i++)
                {
                    if (line[i] == '1')
                    {
                        sumsOfBits[i]++;
                    }
                }
            }
            StringBuilder gammaBuilder = new StringBuilder();
            StringBuilder epsilonBuilder = new StringBuilder();
            for (int i = 0; i != binaryLength; i++)
            {
                if (sumsOfBits[i] > lines.Length / 2)
                {
                    gammaBuilder.Append("1");
                    epsilonBuilder.Append("0");
                }
                else
                {
                    gammaBuilder.Append("0");
                    epsilonBuilder.Append("1");
                }
            }
            int gamma = StaticHelpers.GetIntFromStringBinary(gammaBuilder.ToString());
            int epsilon = StaticHelpers.GetIntFromStringBinary(epsilonBuilder.ToString());
            Console.WriteLine(gamma * epsilon);
        }

        public void Task2()
        {
            int[] sumsOfBits = new int[binaryLength];
            var lines = File.ReadAllLines("../../../Day3/Day3Input.txt");
            var oxygenLines = lines.ToList();
            for (int i = 0; i != binaryLength; i++)
            {
                int numberOfOnes = 0;
                foreach(var line in oxygenLines)
                {
                    if (line[i] == '1')
                    {
                        numberOfOnes++;
                    }
                }
                if (numberOfOnes * 2 >= oxygenLines.Count)
                { 
                    oxygenLines.RemoveAll(x => x[i] == '0');
                }
                else
                {
                    oxygenLines.RemoveAll(x => x[i] == '1');
                }
                if (oxygenLines.Count == 1)
                    break;
            }
            var co2Lines = lines.ToList();
            for (int i = 0; i != binaryLength; i++)
            {
                int numberOfOnes = 0;
                foreach (var line in co2Lines)
                {
                    if (line[i] == '1')
                    {
                        numberOfOnes++;
                    }
                }
                if (numberOfOnes * 2 < co2Lines.Count)
                {
                    co2Lines.RemoveAll(x => x[i] == '0');
                }
                else
                {
                    co2Lines.RemoveAll(x => x[i] == '1');
                }
                if (co2Lines.Count == 1)
                    break;
            }
            int oxygen = StaticHelpers.GetIntFromStringBinary(oxygenLines.Single());
            int co2 = StaticHelpers.GetIntFromStringBinary(co2Lines.Single());
            Console.WriteLine(oxygen * co2);
        }
    }
}
