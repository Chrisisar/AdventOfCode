using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day23 : IDay
    {
        int BurrowCapacity = 4;
        long LowestResult = long.MaxValue;

        bool NextMove(Stack<char>[] burrows, List<char> hall, long energySpent)
        {
            if (energySpent >= LowestResult)
                return false;


            if (burrows.All(b => b.Count == BurrowCapacity && b.All(x => x == b.FirstOrDefault())))
            {
                if (LowestResult > energySpent)
                {
                    LowestResult = energySpent;
                    var debug = false;
                    //if (LowestResult < 14346) debug = true;
                    return debug;
                }
                //Console.WriteLine(energySpent);
            }

            //Move From Hall
            for (int i = 0; i != hall.Count; i++)
            {
                switch (hall[i])
                {
                    case 'A':
                        if (burrows[0].All(x => x == 'A') && NoObstaclesBetween(hall, i, 2))
                        {
                            var newEnergySpent = energySpent + (Math.Abs(i - 2) + BurrowCapacity - burrows[0].Count) * 1;
                            var newBurrows = burrows.Select(x => new Stack<char>(x.Reverse())).ToArray();
                            newBurrows[0].Push('A');
                            var newHall = new List<char>(hall);
                            newHall[i] = '.';
                            if (NextMove(newBurrows, newHall, newEnergySpent))
                            {
                                OutputState(hall, burrows, energySpent);
                                return true;
                            }
                            return false;
                        }
                        break;
                    case 'B':
                        if (burrows[1].All(x => x == 'B') && NoObstaclesBetween(hall, i, 4))
                        {
                            var newEnergySpent = energySpent + (Math.Abs(i - 4) + BurrowCapacity - burrows[1].Count) * 10;
                            var newBurrows = burrows.Select(x => new Stack<char>(x.Reverse())).ToArray();
                            newBurrows[1].Push('B');
                            var newHall = new List<char>(hall);
                            newHall[i] = '.';
                            if (NextMove(newBurrows, newHall, newEnergySpent))
                            {
                                OutputState(hall, burrows, energySpent);
                                return true;
                            }
                            return false;
                        }
                        break;
                    case 'C':
                        if (burrows[2].All(x => x == 'C') && NoObstaclesBetween(hall, i, 6))
                        {
                            var newEnergySpent = energySpent + (Math.Abs(i - 6) + BurrowCapacity - burrows[2].Count) * 100;
                            var newBurrows = burrows.Select(x => new Stack<char>(x.Reverse())).ToArray();
                            newBurrows[2].Push('C');
                            var newHall = new List<char>(hall);
                            newHall[i] = '.';
                            if (NextMove(newBurrows, newHall, newEnergySpent))
                            {
                                OutputState(hall, burrows, energySpent);
                                return true;
                            }
                            return false;
                        }
                        break;
                    case 'D':
                        if (burrows[3].All(x => x == 'D') && NoObstaclesBetween(hall, i, 8))
                        {
                            var newEnergySpent = energySpent + (Math.Abs(i - 8) + BurrowCapacity - burrows[3].Count) * 1000;
                            var newBurrows = burrows.Select(x => new Stack<char>(x.Reverse())).ToArray();
                            newBurrows[3].Push('D');
                            var newHall = new List<char>(hall);
                            newHall[i] = '.';
                            if (NextMove(newBurrows, newHall, newEnergySpent))
                            {
                                OutputState(hall, burrows, energySpent);
                                return true;
                            }
                            return false;
                        }
                        break;
                }
            }

            //for (int i = 0; i <= 10; i++)
            //{
            //    Console.Write(hall[i]);
            //}
            //Console.WriteLine();
            //if (NoObstaclesBetween(hall, 0, 10))
            //{
            //    Console.WriteLine(energySpent);
            //}
            //Move From Burrows
            for (int burrow = 0; burrow != 4; burrow++)
            {
                char burrowLetter = burrow == 0 ? 'A' : burrow == 1 ? 'B' : burrow == 2 ? 'C' : 'D';
                if (!burrows[burrow].All(x => x == burrowLetter))
                {
                    for (int i = 0; i != 11; i++)
                    {
                        if (i > 1 && i < 9 && i % 2 == 0) //skip entrances
                            continue;

                        if (hall[i] == '.' && NoObstaclesBetween(hall, i, burrow * 2 + 2))
                        {
                            var newBurrows = burrows.Select(x => new Stack<char>(x.Reverse())).ToArray();
                            var poppedLetter = newBurrows[burrow].Pop();
                            var costOfMovement = poppedLetter == 'A' ? 1 : poppedLetter == 'B' ? 10 : poppedLetter == 'C' ? 100 : 1000;
                            var newEnergySpent = energySpent + (Math.Abs(i - (burrow * 2 + 2)) + BurrowCapacity - burrows[burrow].Count + 1) * costOfMovement;
                            var newHall = new List<char>(hall);
                            newHall[i] = poppedLetter;
                            if (NextMove(newBurrows, newHall, newEnergySpent))
                            {
                                OutputState(hall, burrows, energySpent);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool NoObstaclesBetween(List<char> hall, int startIndex, int endIndex)
        {
            if (startIndex > endIndex)
            {
                var temp = startIndex;
                startIndex = endIndex;
                endIndex = temp;
            }
            for (int i = startIndex + 1; i != endIndex; i++)
            {
                if (hall[i] != '.') return false;
            }
            return true;
        }

        void OutputState(List<char> hall, Stack<char>[] burrows, long energySpent)
        {
            Console.WriteLine(new string('#', 13) + " " + energySpent);
            Console.Write('#');
            for (int i = 0; i != 11; i++)
            {
                Console.Write(hall[i]);
            }
            Console.WriteLine('#');
            Console.Write("###");
            for (int i = 0; i != 4; i++)
            {
                char burrowLetter = burrows[i].Count == BurrowCapacity ? burrows[i].Peek() : '.';
                Console.Write(burrowLetter + "#");
            }
            Console.WriteLine("##");
            for (int i = BurrowCapacity - 2; i >= 0; i--)
            {
                Console.Write("  #");
                for (int j = 0; j != 4; j++)
                {
                    var burrowList = burrows[j].Reverse().ToList();
                    char burrowLetter = burrowList.Count > i ? burrowList[i] : '.';
                    Console.Write(burrowLetter + "#");
                }
                Console.WriteLine();
            }
            Console.WriteLine("  #########");
            Console.WriteLine();
        }

        public void Task1()
        {
            //Calculated manually
            //A 5 + 5 + 3 + 3 = 16
            //B 8 + 5 = 130
            //C 2 + 3 + 7 = 1200
            //D 2 + 3 + 8 = 13000
            Console.WriteLine(14346);



            Stack<char>[] burrows = new Stack<char>[4];

            //part1
            BurrowCapacity = 2;
            //burrows[0] = new Stack<char>(new char[] { 'C', /*'D', 'D',*/ 'D' });
            //burrows[1] = new Stack<char>(new char[] { 'A', /*'B', 'C',*/ 'A' });
            //burrows[2] = new Stack<char>(new char[] { 'B', /*'A', 'B',*/ 'C' });
            //burrows[3] = new Stack<char>(new char[] { 'B', /*'C', 'A',*/ 'D' });


            //Michał
            burrows[0] = new Stack<char>(new char[] { 'D', 'B' });
            burrows[1] = new Stack<char>(new char[] { 'C', 'B' });
            burrows[2] = new Stack<char>(new char[] { 'A',  'C' });
            burrows[3] = new Stack<char>(new char[] { 'A',  'D' });
            List<char> hall = new List<char>(11) { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' };

            NextMove(burrows, hall, 0);
            Console.WriteLine(LowestResult);
        }

        public void Task2()
        {
            Stack<char>[] burrows = new Stack<char>[4];

            //part2
            LowestResult = long.MaxValue;
            BurrowCapacity = 4;
            //burrows[0] = new Stack<char>(new char[] { 'C', 'D', 'D', 'D' });
            //burrows[1] = new Stack<char>(new char[] { 'A', 'B', 'C', 'A' });
            //burrows[2] = new Stack<char>(new char[] { 'B', 'A', 'B', 'C' });
            //burrows[3] = new Stack<char>(new char[] { 'B', 'C', 'A', 'D' });

            //Michał
            burrows[0] = new Stack<char>(new char[] { 'D', 'D', 'D', 'B' });
            burrows[1] = new Stack<char>(new char[] { 'C', 'B', 'C', 'B' });
            burrows[2] = new Stack<char>(new char[] { 'A', 'A', 'B', 'C' });
            burrows[3] = new Stack<char>(new char[] { 'A', 'C', 'A', 'D' });
            List<char> hall = new List<char>(11) { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' };


            OutputState(hall, burrows, 0);
            //burrows[0] = new Stack<char>(new char[] { 'C', 'A', 'A'  });
            //burrows[1] = new Stack<char>(new char[] { 'B', 'B', 'B', 'B' });
            //burrows[2] = new Stack<char>(new char[] { 'C', 'C', 'C' });
            //burrows[3] = new Stack<char>(new char[] { 'D', 'D', 'D', 'D' });
            //List<char> hall = new List<char>(11) { '.', 'A', '.', '.', '.', '.', '.', 'A', '.', '.', '.' };


            NextMove(burrows, hall, 0);
            Console.WriteLine(LowestResult);
        }
    }
}
