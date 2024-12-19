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
        List<long> PartTwoResults = new List<long>();

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
            PerformProgram(RegA, instructions.Count);
            Console.WriteLine(string.Join(",", output));
        }

        void PerformProgram(long regA, int stopAtOutputLength)
        {
            output = new List<long>();
            RegA = regA;
            pointer = 0;
            while (pointer < instructions.Count && output.Count < stopAtOutputLength)
            {
                int opcode = instructions[pointer];
                int operand = instructions[pointer + 1];
                RunInstruction(opcode, operand);
            }
        }

        public void Task2()
        {
            for (int a = 512; a <= 4096; a++)
            {
                PerformProgram(a, 1);
                if (output[0] == instructions[0])
                {
                    AddNewOctalToA(a, 1);
                }
            }
            Console.WriteLine(PartTwoResults.Min());
        }


        void AddNewOctalToA(long a, int positionToCheck)
        {
            if (positionToCheck == instructions.Count)
            {
                PartTwoResults.Add(a);
                return;
            }
            for (int i = 0; i < 8; i++)
            {
                long tempA = i * CalculateDenominator(8 + positionToCheck * 3) + a;
                PerformProgram(tempA, positionToCheck + 1);
                if (positionToCheck < output.Count && output[positionToCheck] == instructions[positionToCheck])
                {
                    AddNewOctalToA(tempA, positionToCheck + 1);
                }
            }
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
            RegC = RegA / CalculateDenominator(TranslateToComboOperand(operand));
        }

        private void BDV(int operand)
        {
            RegB = RegA / CalculateDenominator(TranslateToComboOperand(operand));
        }

        private long CalculateDenominator(long power)
        {
            long denominator = 1;
            for (int i = 0; i < power; i++)
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
            RegA /= CalculateDenominator(TranslateToComboOperand(operand));
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