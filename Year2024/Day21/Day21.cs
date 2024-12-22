using AdventOfCode.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day21 : IDay
    {
        List<string> codes = new List<string>();
        Dictionary<(string, int), long> cache = new Dictionary<(string, int), long>();

        public Day21(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach (var line in lines)
            {
                codes.Add(line);
            }
        }

        (int x, int y) GetNumericBoardPosition(char number)
        {
            switch (number)
            {
                case 'A':
                    return (3, 4);
                case '0':
                    return (2, 4);
                case '1':
                    return (1, 3);
                case '2':
                    return (2, 3);
                case '3':
                    return (3, 3);
                case '4':
                    return (1, 2);
                case '5':
                    return (2, 2);
                case '6':
                    return (3, 2);
                case '7':
                    return (1, 1);
                case '8':
                    return (2, 1);
                case '9':
                    return (3, 1);
            }
            throw new Exception();
        }

        (int x, int y) GetDirectionalBoardPosition(char character)
        {
            switch (character)
            {
                case 'A':
                    return (3, 1);
                case '<':
                    return (1, 2);
                case '>':
                    return (3, 2);
                case 'v':
                    return (2, 2);
                case '^':
                    return (2, 1);
            }
            throw new Exception();
        }

        string[] GenerateNumericBoardPaths(char start, char end)
        {
            var startPosition = GetNumericBoardPosition(start);
            var endPosition = GetNumericBoardPosition(end);
            int xDif = endPosition.x - startPosition.x;
            int yDif = endPosition.y - startPosition.y;
            string verticals = GenerateVerticals(yDif);
            string horizontals = GenerateHorizontals(xDif);
            if (string.IsNullOrWhiteSpace(verticals))
            {
                return new string[] { horizontals };
            }
            else if (string.IsNullOrWhiteSpace(horizontals))
            {
                return new string[] { verticals };
            }
            else if (startPosition.y == 4 && endPosition.x == 1)
            {
                return new string[] { verticals + horizontals };
            }
            else if (startPosition.x == 1 && endPosition.y == 4)
            {
                return new string[] { horizontals + verticals };
            }
            else
            {
                return new string[] { verticals + horizontals, horizontals + verticals };
            }
        }

        string[] GenerateDirectionalBoardPaths(char start, char end)
        {
            var startPosition = GetDirectionalBoardPosition(start);
            var endPosition = GetDirectionalBoardPosition(end);
            int xDif = endPosition.x - startPosition.x;
            int yDif = endPosition.y - startPosition.y;
            string verticals = GenerateVerticals(yDif);
            string horizontals = GenerateHorizontals(xDif);
            if (startPosition.y == 1 && endPosition.x == 1)
            {
                return new string[] { verticals + horizontals };
            }
            else if (startPosition.x == 1 && endPosition.y == 1)
            {
                return new string[] { horizontals + verticals };
            }
            else
            {
                return new string[] { verticals + horizontals, horizontals + verticals };
            }
        }

        string GenerateHorizontals(int xDif)
        {
            StringBuilder sb = new StringBuilder();
            while (xDif != 0)
            {
                if (xDif > 0)
                {
                    sb.Append('>');
                    xDif--;
                }
                else
                {
                    sb.Append('<');
                    xDif++;
                }
            }
            return sb.ToString();
        }

        string GenerateVerticals(int yDif)
        {
            StringBuilder sb = new StringBuilder();
            while (yDif != 0)
            {
                if (yDif > 0)
                {
                    sb.Append('v');
                    yDif--;
                }
                else
                {
                    sb.Append('^');
                    yDif++;
                }
            }
            return sb.ToString();
        }

        long GenerateDirectionalResult(string patternToMap, char previousChar, int howManyDirectionalChecks, int directionalChecksLimit)
        {
            if (cache.TryGetValue((patternToMap, howManyDirectionalChecks), out long result))
            {
                return result;
            }
            for (int i = 0; i != patternToMap.Length; i++)
            {
                char currentChar = patternToMap[i];
                string[] directionalBoardPaths = GenerateDirectionalBoardPaths(previousChar, currentChar);
                long maxLength = long.MaxValue;
                for (int j = 0; j < directionalBoardPaths.Length; j++)
                {
                    long directionalResult;
                    if (howManyDirectionalChecks < directionalChecksLimit)
                    {
                        directionalResult = GenerateDirectionalResult(directionalBoardPaths[j] + 'A', 'A', howManyDirectionalChecks + 1, directionalChecksLimit);
                    }
                    else
                    {
                        directionalResult = directionalBoardPaths[j].Length + 1;
                    }
                    if (directionalResult < maxLength)
                    {
                        maxLength = directionalResult;
                    }
                }
                result += maxLength;
                previousChar = currentChar;
            }
            cache.Add((patternToMap, howManyDirectionalChecks), result);
            return result;
        }

        long GenerateNumericResult(string patternToMap, char previousChar, int directionalChecksLimit)
        {
            long result = 0;
            for (int i = 0; i != patternToMap.Length; i++)
            {
                char currentChar = patternToMap[i];
                string[] numericBoardPaths = GenerateNumericBoardPaths(previousChar, currentChar);
                long maxLength = long.MaxValue;
                for (int j = 0; j < numericBoardPaths.Length; j++)
                {
                    var directionalResult = GenerateDirectionalResult(numericBoardPaths[j] + 'A', 'A', 1, directionalChecksLimit);
                    if (directionalResult < maxLength)
                    {
                        maxLength = directionalResult;
                    }
                }
                result += maxLength;
                previousChar = currentChar;
            }
            return result;
        }

        public void Task1()
        {
            long result = 0;
            foreach (var code in codes)
            {
                long numericResult = GenerateNumericResult(code, 'A', 2);
                result += long.Parse(code.Substring(0, code.Length - 1)) * numericResult;
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            cache.Clear();
            long result = 0;
            foreach (var code in codes)
            {
                long numericResult = GenerateNumericResult(code, 'A', 25);
                result += long.Parse(code.Substring(0, code.Length - 1)) * numericResult;
            }
            Console.WriteLine(result);

        }
    }
}