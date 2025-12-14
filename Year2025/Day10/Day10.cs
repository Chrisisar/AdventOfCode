using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2025
{
    class Day10 : IDay
    {
        class Machine
        {
            public long ExpectedIndicatorLightsLong;
            public List<List<int>> Buttons;
            public List<long> ButtonsLong;
            public List<int> ExpectedJoltages;
        }

        List<Machine> Machines = new List<Machine>();

        public Day10(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach (var line in lines)
            {
                var splitLine = line.Split(' ');
                Machine machine = new Machine();
                machine.ExpectedIndicatorLightsLong = ParseIndicatorLightsLong(splitLine[0]);
                machine.Buttons = ParseButtons(splitLine);
                machine.ButtonsLong = ParseButtonsLong(splitLine);
                machine.ExpectedJoltages = ParseJoltages(splitLine[splitLine.Length - 1]);
                Machines.Add(machine);
            }
        }

        private List<long> ParseButtonsLong(string[] splitLine)
        {
            List<long> buttons = new List<long>();
            for (int i = 1; i != splitLine.Length - 1; i++)
            {
                long buttonValue = 0;
                var splitButton = splitLine[i].Substring(1, splitLine[i].Length - 2).Split(',');
                foreach (var trigger in splitButton)
                {
                    int.TryParse(trigger, out int triggerInt);
                    buttonValue ^= 1 << triggerInt;
                }
                buttons.Add(buttonValue);
            }
            return buttons;
        }

        private List<List<int>> ParseButtons(string[] splitLine)
        {
            List<List<int>> buttons = new List<List<int>>();
            for (int i = 1; i != splitLine.Length - 1; i++)
            {
                var buttonTriggers = new List<int>();
                var splitButton = splitLine[i].Substring(1, splitLine[i].Length - 2).Split(',');
                foreach (var trigger in splitButton)
                {
                    int.TryParse(trigger, out int triggerInt);
                    buttonTriggers.Add(triggerInt);
                }
                buttons.Add(buttonTriggers);
            }
            return buttons;
        }

        private List<int> ParseJoltages(string joltagesString)
        {
            List<int> joltages = new List<int>();
            char[] separators = new char[] { ',', '{', '}' };
            var splitJoltages = joltagesString.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var joltage in splitJoltages)
            {
                joltages.Add(int.Parse(joltage));
            }
            return joltages;
        }

        private long ParseIndicatorLightsLong(string lightsString)
        {
            long result = 0;
            for (int i = 0; i < lightsString.Length - 2; i++)
            {
                if (lightsString[i + 1] == '#')
                {
                    result ^= 1 << i;
                }
            }
            return result;
        }

        public void Task1()
        {
            long result = 0;
            foreach (var machine in Machines)
            {
                var bestForMachine = long.MaxValue;
                for (int i = 0; i != machine.ButtonsLong.Count; i++)
                {
                    var temp = CalculateBestButtons(machine, i, 0, long.MaxValue, 0);
                    if (temp < bestForMachine)
                    {
                        bestForMachine = temp;
                    }
                }
                result += bestForMachine;
            }
            Console.WriteLine(result);
        }

        long CalculateBestButtons(Machine machine, int startingPosition, long clicksSoFar, long bestSoFar, long value)
        {
            if (clicksSoFar >= bestSoFar)
            {
                return bestSoFar;
            }
            clicksSoFar++;
            var calculatedValue = value ^= machine.ButtonsLong[startingPosition];
            if (calculatedValue == machine.ExpectedIndicatorLightsLong)
            {
                return clicksSoFar;
            }

            for (int i = startingPosition + 1; i < machine.ButtonsLong.Count; i++)
            {
                var result = CalculateBestButtons(machine, i, clicksSoFar, bestSoFar, calculatedValue);
                if (result < bestSoFar)
                {
                    bestSoFar = result;
                }
            }
            return bestSoFar;
        }

        void GaussJordanElimination(int[][] matrix)
        {
            int rows = matrix.Length;
            int columns = matrix[0].Length;
            int variables = columns - 1;

            int currentRow = 0;
            for (int col = 0; col < variables && currentRow < rows; col++)
            {
                int pivot = -1;
                for (int row = currentRow; row < rows; row++)
                {
                    if (Math.Abs(matrix[row][col]) == 1)
                    {
                        pivot = row;
                        break;
                    }
                }
                if (pivot == -1)
                    continue;

                var tmp = matrix[currentRow];
                matrix[currentRow] = matrix[pivot];
                matrix[pivot] = tmp;

                if (matrix[currentRow][col] == -1)
                    for (int c = 0; c < columns; c++)
                        matrix[currentRow][c] *= -1;

                for (int r = 0; r < rows; r++)
                {
                    if (r != currentRow)
                    {
                        int factor = matrix[r][col];
                        for (int c = 0; c < columns; c++)
                            matrix[r][c] -= factor * matrix[currentRow][c];
                    }
                }

                currentRow++;
            }
            return;
        }

        int[][] CreateMatrix(Machine machine)
        {
            int[][] matrix = new int[machine.ExpectedJoltages.Count][];
            for (int i = 0; i != matrix.Length; i++)
            {
                matrix[i] = new int[machine.Buttons.Count + 1];
                matrix[i][machine.Buttons.Count] = machine.ExpectedJoltages[i];
            }
            for (int i = 0; i != machine.Buttons.Count; i++)
            {
                var button = machine.Buttons[i];
                foreach (var trigger in button)
                {
                    matrix[trigger][i] = 1;
                }
            }
            return matrix;
        }

        List<int> CalculateClicks(int[][] matrix, List<(int index, int value)> parameters)
        {
            List<int> clicks = new List<int>();
            int row = 0;
            for (int column = 0; column != matrix[0].Length - 1; column++)
            {
                if (parameters.Any(x => x.index == column))
                {
                    var x = parameters.First(x => x.index == column).value;
                    clicks.Add(x);
                    //Console.Write($"{i}: {x}; ");
                    continue;
                }
                else
                {
                    var x = matrix[row][matrix[0].Length - 1];
                    foreach (var parameter in parameters)
                    {
                        x -= matrix[row][parameter.index] * parameter.value;
                    }
                    if (x < 0)
                    {
                        x = Math.Abs(x % 2);
                    }
                    clicks.Add(x);
                    row++;
                    //Console.Write($"{i}: {x}; ");
                }
            }
            return clicks;
        }

        public void Task2()
        {
            long totalResult = 0;
            foreach (var machine in Machines)
            {
                int[][] matrix = CreateMatrix(machine);
                GaussJordanElimination(matrix);
                List<(int index, int value)> parametrized = GetParametrizedButtons(matrix);

                var minimumForMachine = int.MaxValue;
                int maxIteration = machine.ExpectedJoltages.Max(); //TODO: Improve performance by cutting on limits per buttons click
                if (parametrized.Count == 0) //The only solution already found
                {
                    var clicks = CalculateClicks(matrix, new List<(int index, int value)>());
                    if (ValidateEntry(machine, clicks))
                        minimumForMachine = clicks.Sum();
                }
                else
                {
                    while (parametrized.Last().value <= maxIteration)
                    {
                        var clicks = CalculateClicks(matrix, parametrized);
                        //Console.WriteLine($"TOTAL: {result}");
                        if (clicks.Sum() < minimumForMachine && ValidateEntry(machine, clicks))
                            minimumForMachine = clicks.Sum();

                        var parameter = parametrized[0];
                        parameter.value++;
                        parametrized[0] = parameter;
                        for (int i = 0; i != parametrized.Count - 1; i++)
                        {
                            parameter = parametrized[i];
                            if (parameter.value == maxIteration)
                            {
                                parameter.value = 0;
                                parametrized[i] = parameter;
                                var nextParameter = parametrized[i + 1];
                                nextParameter.value++;
                                parametrized[i + 1] = nextParameter;
                            }
                        }
                    }
                }
                //Console.WriteLine($"Minimum for machine: {minimumForMachine}.");
                totalResult += minimumForMachine;
            }
            Console.WriteLine(totalResult);
        }

        private static List<(int index, int value)> GetParametrizedButtons(int[][] matrix)
        {
            var parametrized = new List<(int index, int value)>();
            for (int column = 0; column != matrix[0].Length - 1; column++)
            {
                var nonZeroValue = 0;
                for (int row = 0; row != matrix.Length; row++)
                {
                    if (matrix[row][column] != 0)
                        nonZeroValue++;
                }
                if (nonZeroValue > 1)
                {
                    parametrized.Add((column, 0));
                }
            }
            return parametrized;
        }

        private bool ValidateEntry(Machine machine, List<int> clicks)
        {
            int[] result = new int[machine.ExpectedJoltages.Count];
            for (int i = 0; i != clicks.Count; i++)
            {
                foreach (var trigger in machine.Buttons[i])
                {
                    result[trigger] += clicks[i];
                }
            }
            for (int i = 0; i != result.Length; i++)
            {
                if (machine.ExpectedJoltages[i] != result[i])
                {
                    return false;
                }
            }
            return true;
        }

        private long CalculateBestButtonsForJoltage(Machine machine, int startingPosition, long clicksSoFar, long bestSoFar, int[] joltages)
        {
            if (clicksSoFar >= bestSoFar)
            {
                return bestSoFar;
            }
            clicksSoFar++;

            foreach (var triggers in machine.Buttons[startingPosition])
            {
                joltages[triggers]++;
            }

            for (int i = 0; i != joltages.Length; i++)
            {
                if (joltages[i] > machine.ExpectedJoltages[i])
                {
                    return bestSoFar;
                }
            }
            for (int i = 0; i != joltages.Length; i++)
            {
                if (joltages[i] < machine.ExpectedJoltages[i])
                {
                    break;
                }
                if (i == joltages.Length - 1)
                {
                    return clicksSoFar;
                }
            }

            for (int i = startingPosition; i < machine.ButtonsLong.Count; i++)
            {
                var result = CalculateBestButtonsForJoltage(machine, i, clicksSoFar, bestSoFar, joltages.ToArray());
                if (result < bestSoFar)
                {
                    bestSoFar = result;
                }
            }
            return bestSoFar;
        }
    }
}