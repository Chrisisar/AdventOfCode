using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day17 : IDay
    {
        long RegA, RegB, RegC;
        List<int> instructions = new List<int>();
        int pointer;
        List<long> output = new List<long>();

        public Day17(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            long.TryParse(lines[0].Split(": ")[1], out RegA);
            long.TryParse(lines[1].Split(": ")[1], out RegB);
            long.TryParse(lines[2].Split(": ")[1], out RegC);

            instructions = lines[4].Split(": ")[1].Split(",").Select(int.Parse).ToList();
        }

        public void Task1()
        {
            pointer = 0;
            while (pointer < instructions.Count)
            {
                int opcode = instructions[pointer];
                int operand = instructions[pointer + 1];
                RunInstruction(opcode, operand);
            }
            Console.WriteLine(string.Join(",", output));
        }

        public void Task2Brute()
        {
            long startingRegAValue = 6109323397;
            string instructionString = string.Join(",", instructions);
            while(true)
            {
                RegA = startingRegAValue;
                RegB = 0;
                RegC = 0;
                output = new List<long>();

                pointer = 0;
                while (pointer < instructions.Count)
                {
                    int opcode = instructions[pointer];
                    int operand = instructions[pointer + 1];
                    RunInstruction(opcode, operand);
                    string outputString = string.Join(",", output);
                    if(!instructionString.StartsWith(outputString))
                    {
                        break;
                    }
                }
                if(instructionString == string.Join(",", output))
                {
                    break;
                }

                startingRegAValue++;
            }
            Console.WriteLine(startingRegAValue);
        }

        public void Task2()
        {

        }

        void RunInstruction(int opcode, int operand)
        {
            switch (opcode)
            {
                case 0:
                    ADV(operand);
                    break;
                case 1:
                    BXL(operand);
                    break;
                case 2:
                    BST(operand);
                    break;
                case 3:
                    if (JNZ(operand))
                        return;
                    break;
                case 4:
                    BXC(operand);
                    break;
                case 5:
                    OUT(operand);
                    break;
                case 6:
                    BDV(operand);
                    break;
                case 7:
                    CDV(operand);
                    break;
            }
            pointer += 2;
        }

        private void CDV(int operand)
        {
            RegC = RegA / CalculateDenominator(operand);
        }

        private void BDV(int operand)
        {
            RegB = RegA / CalculateDenominator(operand);
        }

        private long CalculateDenominator(int operand)
        {
            long denominator = 1;
            for (int i = 0; i < TranslateToComboOperand(operand); i++)
            {
                denominator *= 2;
            }
            return denominator;
        }

        private void OUT(int operand)
        {
            output.Add(TranslateToComboOperand(operand) % 8);
        }

        private void BXC(int operand)
        {
            RegB ^= RegC;
        }

        private bool JNZ(int operand)
        {
            if (RegA == 0) return false;
            pointer = operand;
            return true;
        }

        private void BST(int operand)
        {
            RegB = TranslateToComboOperand(operand) % 8;
        }

        private void BXL(int operand)
        {
            RegB ^= operand;
        }

        private void ADV(int operand)
        {
            RegA /= CalculateDenominator(operand);
        }

        long TranslateToComboOperand(int operand)
        {
            if (operand <= 3) return operand;
            if (operand == 4) return RegA;
            if (operand == 5) return RegB;
            if (operand == 6) return RegC;
            throw new InvalidOperationException();
        }
    }
}