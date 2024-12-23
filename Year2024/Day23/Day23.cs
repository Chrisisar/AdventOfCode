using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day23 : IDay
    {
        Dictionary<string, List<string>> connections = new Dictionary<string, List<string>>();

        public Day23(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach (var line in lines)
            {
                var splitLine = line.Split('-');
                AddConnection(splitLine[0], splitLine[1]);
            }
        }

        public void Task1()
        {
            var visitedList = new List<string>();
            var threeCliques = new List<(string a, string b, string c)>();
            foreach (var node in connections)
            {
                visitedList.Add(node.Key);
                var internalVisitedList = new List<string>();
                foreach (var connection in node.Value)
                {
                    internalVisitedList.Add(connection);
                    if (visitedList.Any(x => x == connection)) continue;

                    var commonConnections = node.Value.Intersect(connections[connection]);
                    foreach (var commonConnection in commonConnections)
                    {
                        if (visitedList.Any(x => x == commonConnection)) continue;
                        if (internalVisitedList.Any(x => x == commonConnection)) continue;

                        threeCliques.Add((node.Key, connection, commonConnection));
                    }
                }
            }
            Console.WriteLine(threeCliques.Count(x => x.a.StartsWith('t') || x.b.StartsWith('t') || x.c.StartsWith('t')));
        }

        public void Task2()
        {
            var maxClique = new List<string>();
            List<List<string>> cliquesToCheck = new List<List<string>>();
            foreach (var node in connections.Keys)
            {
                cliquesToCheck.Add(new List<string> { node });
            }
            while (cliquesToCheck.Any())
            {
                var currentClique = cliquesToCheck.First();
                cliquesToCheck.RemoveAt(0);

                foreach (var node in connections.Keys.Except(currentClique))
                {
                    if (currentClique.All(x => connections[node].Contains(x)))
                    {
                        currentClique.Add(node);
                        if (!cliquesToCheck.Any(x => !currentClique.Except(x).Any()))
                        {
                            var cliquesToRemove = cliquesToCheck.Where(x => !x.Except(currentClique).Any()).ToList();
                            foreach(var cliqueToRemove in cliquesToRemove)
                            {
                                cliquesToCheck.Remove(cliqueToRemove);
                            }
                            cliquesToCheck.Add(currentClique.ToList());
                        }
                    }
                }

                if (currentClique.Count > maxClique.Count)
                {
                    maxClique = currentClique;
                }
            }
            Console.WriteLine(string.Join(",", maxClique.OrderBy(x => x)));
        }

        void AddConnection(string a, string b)
        {
            if (connections.TryGetValue(a, out List<string> list))
            {
                list.Add(b);
            }
            else
            {
                connections.Add(a, new List<string> { b });
            }
            if (connections.TryGetValue(b, out list))
            {
                list.Add(a);
            }
            else
            {
                connections.Add(b, new List<string> { a });
            }
        }
    }
}