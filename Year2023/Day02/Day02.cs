using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2023
{
    class Day02 : IDay
    {
        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            int sum = 0;
            foreach (var line in lines)
            {
                var list = ParseInputLine(line, out int gameNumber);
                if (list.All(x => x.Item1 <= 12 && x.Item2 <= 14 && x.Item3 <= 13))
                {
                    sum += gameNumber;
                }
            }
            Console.WriteLine(sum);
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType()).ToList();
            long sum = 0;
            foreach (var line in lines)
            {
                var list = ParseInputLine(line, out int gameNumber);
                long gamePower = list.Max(x => x.Item1) * list.Max(x => x.Item2) * list.Max(x => x.Item3);
                sum += gamePower;
            }
            Console.WriteLine(sum);
        }

        private List<Tuple<int, int, int>> ParseInputLine(string line, out int gameNumber)
        {
            line = line.Replace("Game ", "");
            var splitLine = line.Split(":");
            int.TryParse(splitLine[0], out gameNumber);
            var splitGame = splitLine[1].Split(";");
            List<Tuple<int, int, int>> list = new List<Tuple<int, int, int>>();
            foreach (var round in splitGame)
            {
                var splitRound = round.Split(",");
                int red = 0, blue = 0, green = 0;
                foreach (var cubes in splitRound)
                {
                    var cube = cubes.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    switch (cube[1])
                    {
                        case "red":
                            int.TryParse(cube[0], out red);
                            break;
                        case "blue":
                            int.TryParse(cube[0], out blue);
                            break;
                        case "green":
                            int.TryParse(cube[0], out green);
                            break;
                    }
                }
                list.Add(new Tuple<int, int, int>(red, blue, green));
            }
            return list;
        }
    }
}