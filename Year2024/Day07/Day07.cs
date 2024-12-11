using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day07 : IDay
    {

        public Day07(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        List<Equation> equations = new List<Equation>();

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach (var line in lines)
            {
                Equation equation = new Equation();
                var splitColon = line.Split(": ");
                equation.Result = long.Parse(splitColon[0]);
                var splitLine = splitColon[1].Split(" ");
                for(int i =0;i<splitLine.Length; i++)
                {
                    equation.Items.Add(long.Parse(splitLine[i]));
                }
                equations.Add(equation);
            }
        }

        public void Task1()
        {
            long result = 0;
            foreach(var equation in equations)
            {
                result += CalculateEquation(equation, 1, equation.Items[0], false) ? equation.Result : 0;
            }
            Console.WriteLine(result);
        }

        private bool CalculateEquation(Equation equation, int index, long currentResult, bool includeConcatenation)
        {
            if(index == equation.Items.Count)
            {
                return currentResult == equation.Result;
            }

            if(currentResult > equation.Result)
            {
                return false;
            }

            bool success = false;
            success |= CalculateEquation(equation, index + 1, currentResult + equation.Items[index], includeConcatenation);
            success |= CalculateEquation(equation, index + 1, currentResult * equation.Items[index], includeConcatenation);
            if(includeConcatenation)
            {
                success |= CalculateEquation(equation, index + 1, long.Parse(currentResult.ToString() + equation.Items[index].ToString()), includeConcatenation);
            }
            return success;
        }

        public void Task2()
        {
            long result = 0;
            foreach (var equation in equations)
            {
                result += CalculateEquation(equation, 1, equation.Items[0], true) ? equation.Result : 0;
            }
            Console.WriteLine(result);
        }

        class Equation
        {
            public long Result;
            public List<long> Items = new List<long>();
        }
    }
}