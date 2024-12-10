using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class BingoField
    {
        public bool AlreadyMarked { get; set; }
        public int Value { get; set; }
    }

    class BingoBoard
    {
        public BingoField[,] Fields = new BingoField[5, 5];
        public bool IsWinner;
        public void MarkNumberIfExists(int number)
        {
            for (int i = 0; i != 5; i++)
            {
                for (int j = 0; j != 5; j++)
                {
                    if (Fields[i, j].Value == number)
                    {
                        Fields[i, j].AlreadyMarked = true;
                        IsWinner |= CheckIfBoardWins(i,j);
                    }
                }
            }
        }

        public int SumOfUnmarkedFields()
        {
            int result = 0;
            for (int i = 0; i != 5; i++)
            {
                for (int j = 0; j != 5; j++)
                {
                    if (!Fields[i, j].AlreadyMarked)
                    {
                        result += Fields[i, j].Value;
                    }
                }
            }
            return result;
        }

        private bool CheckIfBoardWins(int i, int j)
        {
            bool success = true;
            for(int x=0;x!=5;x++)
            {
                success &= Fields[x, j].AlreadyMarked;
            }
            if (success)
            {
                return true;
            }
            success = true;
            for (int y = 0; y != 5; y++)
            {
                success &= Fields[i, y].AlreadyMarked;
            }
            return success;
        }

    }

    class Day4 : IDay
    {
        public void Task1()
        {
            var lines = File.ReadAllLines("../../../Day4/Day4Input.txt");
            var selectedNumbers = lines[0].Split(",").Select(x => int.Parse(x)).ToList();
            List<BingoBoard> boards = new List<BingoBoard>();
            BingoBoard currentBoard = new BingoBoard();
            int fillingLine = 0;
            for (int i = 2; i != lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    boards.Add(currentBoard);
                    currentBoard = new BingoBoard();
                    fillingLine = 0;
                }
                else
                {
                    int fillingColumn = 0;
                    foreach (var number in lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)))
                    {
                        currentBoard.Fields[fillingLine, fillingColumn] = new BingoField { Value = number };
                        fillingColumn++;
                    }
                    fillingLine++;
                }
            }
            for (int i = 0; i != selectedNumbers.Count; i++)
            {
                boards.ForEach(x => x.MarkNumberIfExists(selectedNumbers[i]));
                var winner = boards.FirstOrDefault(x => x.IsWinner);
                if(winner != null)
                {
                    Console.WriteLine(winner.SumOfUnmarkedFields() * selectedNumbers[i]);
                    break;
                }
            }
        }

        public void Task2()
        {
            var lines = File.ReadAllLines("../../../Day4/Day4Input.txt");
            var selectedNumbers = lines[0].Split(",").Select(x => int.Parse(x)).ToList();
            List<BingoBoard> boards = new List<BingoBoard>();
            BingoBoard currentBoard = new BingoBoard();
            int fillingLine = 0;
            for (int i = 2; i != lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    boards.Add(currentBoard);
                    currentBoard = new BingoBoard();
                    fillingLine = 0;
                }
                else
                {
                    int fillingColumn = 0;
                    foreach (var number in lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)))
                    {
                        currentBoard.Fields[fillingLine, fillingColumn] = new BingoField { Value = number };
                        fillingColumn++;
                    }
                    fillingLine++;
                }
            }
            BingoBoard loser = null;
            for (int i = 0; i != selectedNumbers.Count; i++)
            {
                boards.ForEach(x => x.MarkNumberIfExists(selectedNumbers[i]));

                if(boards.Count(x=>!x.IsWinner) == 1)
                {
                    loser = boards.Single(x => !x.IsWinner);
                }
                
                if (loser != null && loser.IsWinner)
                {
                    Console.WriteLine(loser.SumOfUnmarkedFields() * selectedNumbers[i]);
                    break;
                }
            }
        }
    }
}
