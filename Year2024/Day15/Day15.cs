using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day15 : IDay
    {
        int maxW, maxH;
        char[,] map;
        string instruction;
        (int i, int j) robotPosition;
        string inputFilePath;
        bool shouldProceed;

        public Day15(string inputFilePath)
        {
            this.inputFilePath = inputFilePath;
        }

        private void ParseInput(string inputFilePath, bool isPart2)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            maxW = isPart2 ? 2 * lines[0].Length : lines[0].Length;

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    maxH = i;
                    break;
                }
            }
            map = new char[maxH, maxW];
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                if (i < maxH)
                {
                    for (int j = 0; j != lines[i].Length; j++)
                    {
                        if (isPart2)
                        {
                            map[i, 2 * j] = lines[i][j];
                            map[i, 2 * j + 1] = lines[i][j];

                            if (lines[i][j] == '@')
                            {
                                robotPosition = (i, 2 * j);
                                map[i, 2 * j] = '.';
                                map[i, 2 * j + 1] = '.';
                            }
                            else if (lines[i][j] == 'O')
                            {
                                map[i, 2 * j] = '[';
                                map[i, 2 * j + 1] = ']';
                            }
                        }
                        else
                        {
                            map[i, j] = lines[i][j];

                            if (lines[i][j] == '@')
                            {
                                robotPosition = (i, j);
                                map[i, j] = '.';
                            }
                        }
                    }
                }
                else
                {
                    stringBuilder.Append(lines[i]);
                }
            }
            instruction = stringBuilder.ToString();
        }

        public void Task1()
        {
            ParseInput(inputFilePath, false);
            for (int i = 0; i != instruction.Length; i++)
            {
                shouldProceed = true;
                switch (instruction[i])
                {
                    case '^':
                        robotPosition = Move(-1, 0, robotPosition);
                        break;
                    case 'v':
                        robotPosition = Move(1, 0, robotPosition);
                        break;
                    case '<':
                        robotPosition = Move(0, -1, robotPosition);
                        break;
                    case '>':
                        robotPosition = Move(0, 1, robotPosition);
                        break;
                }
            }

            long result = 0;
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    if (map[i, j] == 'O')
                    {
                        result += 100 * i + j;
                    }
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            ParseInput(inputFilePath, true);
            for (int i = 0; i != instruction.Length; i++)
            {
                var map2 = map.Clone() as char[,];
                shouldProceed = true;
                switch (instruction[i])
                {
                    case '^':
                        robotPosition = Move(-1, 0, robotPosition);
                        break;
                    case 'v':
                        robotPosition = Move(1, 0, robotPosition);
                        break;
                    case '<':
                        robotPosition = Move(0, -1, robotPosition);
                        break;
                    case '>':
                        robotPosition = Move(0, 1, robotPosition);
                        break;
                }
                if(!shouldProceed)
                {
                    map = map2;
                }
            }

            long result = 0;
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    if (map[i, j] == '[')
                    {
                        result += 100 * i + j;
                    }
                }
            }
            Console.WriteLine(result);
        }

        void PrintMap()
        {
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    Console.Write(robotPosition.i == i && robotPosition.j == j ? "@" : map[i, j]);
                }
                Console.WriteLine();
            }
        }

        (int i, int j) Move(int diri, int dirj, (int i, int j) objectPosition)
        {
            if(!shouldProceed)
                return objectPosition;
            switch (map[objectPosition.i + diri, objectPosition.j + dirj])
            {
                case '#':
                    shouldProceed = false;
                    break;
                case '.':
                    objectPosition.i += diri;
                    objectPosition.j += dirj;
                    break;
                case 'O':
                    int tempi = objectPosition.i + diri;
                    int tempj = objectPosition.j + dirj;
                    while (map[tempi, tempj] == 'O')
                    {
                        tempi += diri;
                        tempj += dirj;
                    }
                    if (map[tempi, tempj] == '.')
                    {
                        map[tempi, tempj] = 'O';
                        objectPosition.i += diri;
                        objectPosition.j += dirj;
                        map[objectPosition.i, objectPosition.j] = '.';
                    }
                    break;
                case '[':
                    var closeBracketPosition = Move(diri, dirj, (objectPosition.i + diri, objectPosition.j + dirj + 1));
                    if (closeBracketPosition != (objectPosition.i + diri, objectPosition.j + dirj + 1))
                    {
                        map[objectPosition.i + diri, objectPosition.j + dirj + 1] = '.';
                    }
                    var openBracketPosition = Move(diri, dirj, (objectPosition.i + diri, objectPosition.j + dirj));
                    if (openBracketPosition != (objectPosition.i + diri, objectPosition.j + dirj))
                    {
                        map[objectPosition.i + diri, objectPosition.j + dirj] = '.';
                    }
                    if (openBracketPosition != (objectPosition.i + diri, objectPosition.j + dirj)
                        && closeBracketPosition != (objectPosition.i + diri, objectPosition.j + dirj + 1))
                    {
                        map[openBracketPosition.i, openBracketPosition.j] = '[';
                        map[closeBracketPosition.i, closeBracketPosition.j] = ']';
                        objectPosition.i += diri;
                        objectPosition.j += dirj;
                        map[objectPosition.i, objectPosition.j] = '.';
                    }
                    break;
                case ']':
                    openBracketPosition = Move(diri, dirj, (objectPosition.i + diri, objectPosition.j + dirj - 1));
                    if (openBracketPosition != (objectPosition.i + diri, objectPosition.j + dirj - 1))
                    {
                        map[objectPosition.i + diri, objectPosition.j + dirj - 1] = '.';
                    }
                    closeBracketPosition = Move(diri, dirj, (objectPosition.i + diri, objectPosition.j + dirj));
                    if (closeBracketPosition != (objectPosition.i + diri, objectPosition.j + dirj))
                    {
                        map[objectPosition.i + diri, objectPosition.j + dirj] = '.';
                    }
                    if (openBracketPosition != (objectPosition.i + diri, objectPosition.j + dirj - 1)
                        && closeBracketPosition != (objectPosition.i + diri, objectPosition.j + dirj))
                    {
                        map[openBracketPosition.i, openBracketPosition.j] = '[';
                        map[closeBracketPosition.i, closeBracketPosition.j] = ']';
                        objectPosition.i += diri;
                        objectPosition.j += dirj;
                        map[objectPosition.i, objectPosition.j] = '.';
                    }
                    break;
            }
            return objectPosition;
        }
    }
}