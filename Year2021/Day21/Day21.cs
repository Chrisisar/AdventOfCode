using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day21 : IDay
    {
        int Player1Position = 2;
        int Player2Position = 10;
        int Player1Score = 0;
        int Player2Score = 0;
        int BoardSize = 10;
        int ScoreLimit = 1000;
        int CurrentDiceRoll = 0;
        int TotalDiceRolls = 0;

        int RollDice()
        {
            var result = CurrentDiceRoll + 1;
            CurrentDiceRoll++;
            CurrentDiceRoll %= 100;
            TotalDiceRolls++;
            return result;
        }

        int SumOf3DiceRolls()
        {
            var result = 0;
            for (int i = 0; i != 3; i++)
            {
                result += RollDice();
            }
            return result;
        }

        bool GameGoing()
        {
            return Player1Score < ScoreLimit && Player2Score < ScoreLimit;
        }

        void MovePlayer(int playerNumber)
        {
            switch (playerNumber)
            {
                case 1:
                    var finishedPosition = SumOf3DiceRolls() + Player1Position;
                    finishedPosition %= BoardSize;
                    Player1Position = finishedPosition;
                    Player1Score += Player1Position == 0 ? BoardSize : Player1Position;
                    break;
                case 2:
                    finishedPosition = SumOf3DiceRolls() + Player2Position;
                    finishedPosition %= BoardSize;
                    Player2Position = finishedPosition;
                    Player2Score += Player2Position == 0 ? BoardSize : Player2Position;
                    break;
            }
        }

        public void Task1()
        {
            int i = 1;
            while (GameGoing())
            {
                MovePlayer(i);
                i++;
                if (i > 2)
                {
                    i = 1;
                }
            }
            if (Player1Score >= 1000)
            {
                Console.WriteLine(Player2Score * TotalDiceRolls);
            }
            else
            {
                Console.WriteLine(Player1Score * TotalDiceRolls);
            }
        }

        public void Task2()
        {
            Player1Position = 2;
            Player2Position = 10;
            Player1Score = 0;
            Player2Score = 0;
            ScoreLimit = 21;
            PlayPart2(Player1Position, Player2Position, 0, 0, 1, true);
            Console.WriteLine(Math.Max(P1Wins,P2Wins));
        }

        List<Tuple<int, int>> RollCombinations = new List<Tuple<int, int>> {
            new Tuple<int, int>(3,1),
            new Tuple<int, int>(4,3),
            new Tuple<int, int>(5,6),
            new Tuple<int, int>(6,7),
            new Tuple<int, int>(7,6),
            new Tuple<int, int>(8,3),
            new Tuple<int, int>(9,1),
        };

        long P1Wins = 0;
        long P2Wins = 0;

        void PlayPart2(int player1Pos, int player2Pos, int player1Score, int player2Score, long numberOfGames, bool player1Turn)
        {
            if (player1Turn)
            {
                foreach(var roll in RollCombinations)
                {
                    var newP1Pos = player1Pos + roll.Item1;
                    if(newP1Pos > BoardSize)
                    {
                        newP1Pos %= BoardSize;
                    }
                    var newP1Score = player1Score + newP1Pos;
                    var newNumberOfGames = numberOfGames * roll.Item2;
                    if (newP1Score >= 21)
                    {
                        P1Wins += newNumberOfGames;
                    }
                    else
                    {
                        PlayPart2(newP1Pos, player2Pos, newP1Score, player2Score, newNumberOfGames, false);
                    }
                }
            }
            else
            {
                foreach (var roll in RollCombinations)
                {
                    var newP2Pos = player2Pos + roll.Item1;
                    if (newP2Pos > BoardSize)
                    {
                        newP2Pos %= BoardSize;
                    }

                    var newP2Score = player2Score + newP2Pos;
                    var newNumberOfGames = numberOfGames * roll.Item2;
                    if (newP2Score >= 21)
                    {
                        P2Wins += newNumberOfGames;
                    }
                    else
                    {
                        PlayPart2(player1Pos, newP2Pos, player1Score, newP2Score, newNumberOfGames, true);
                    }
                }
            }
        }
    }
}
