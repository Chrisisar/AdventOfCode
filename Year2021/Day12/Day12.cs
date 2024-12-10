using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day12 : IDay
    {
        private Dictionary<string, List<string>> CaveSystem = new Dictionary<string, List<string>>();

        private void CreateCaveSystem()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            foreach (var line in lines)
            {
                var splitLine = line.Split('-');
                if (CaveSystem.TryGetValue(splitLine[0], out List<string> split1List))
                {
                    split1List.Add(splitLine[1]);
                }
                else
                {
                    CaveSystem.Add(splitLine[0], new List<string> { splitLine[1] });
                }

                if (CaveSystem.TryGetValue(splitLine[1], out List<string> split2List))
                {
                    split2List.Add(splitLine[0]);
                }
                else
                {
                    CaveSystem.Add(splitLine[1], new List<string> { splitLine[0] });
                }
            }
        }

        private void OutputCaveSystem()
        {
            foreach (var cave in CaveSystem)
            {
                Console.Write($"{cave.Key}: ");
                foreach (var connection in cave.Value)
                {
                    Console.Write($"{connection}, ");
                }
                Console.WriteLine();
            }
        }

        private int NumberOfRoads = 0;

        private void DFS(string cave, List<string> visited, bool specialUsed)
        {
            if (cave == "end")
            {
                NumberOfRoads++;
                return;
            }
            visited.Add(cave);
            CaveSystem.TryGetValue(cave, out var connections);
            foreach (var connection in connections)
            {
                if (connection.ToLower() != connection || !visited.Contains(connection))
                {
                    var visitedCopy = new List<string>(visited);
                    DFS(connection, visitedCopy, specialUsed);
                }
                else if(connection != "start" && !specialUsed)
                {
                    var visitedCopy = new List<string>(visited);
                    DFS(connection, visitedCopy, true);
                }
            }
        }

        public void Task1()
        {
            CreateCaveSystem();
            //OutputCaveSystem();
            DFS("start", new List<string>() { "start" }, true);
            Console.WriteLine(NumberOfRoads);
        }

        public void Task2()
        {
            NumberOfRoads = 0;
            //OutputCaveSystem();
            DFS("start", new List<string>() { "start" }, false);
            Console.WriteLine(NumberOfRoads);
        }
    }
}
