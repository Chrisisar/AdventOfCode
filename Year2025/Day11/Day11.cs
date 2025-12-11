using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2025
{
    class Day11 : IDay
    {
        Dictionary<string, List<string>> input = new Dictionary<string, List<string>>();
        Dictionary<string, Dictionary<string, long>> connections = new Dictionary<string, Dictionary<string, long>>();

        public Day11(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            foreach (var line in lines)
            {
                var splitLine = line.Split(':');
                var server = splitLine[0];
                var splitTargets = splitLine[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                input.Add(server, splitTargets);
            }
            SortInput();
            foreach (var connection in input)
            {
                Dictionary<string, long> targets = new Dictionary<string, long>();
                foreach (var target in connection.Value)
                {
                    targets.Add(target, 1);
                }
                connections.Add(connection.Key, targets);
                AddTargets(connection.Key);
            }
        }

        void AddTargets(string server)
        {
            foreach (var connection in connections)
            {
                if (connection.Value.ContainsKey(server))
                {
                    foreach (var target in connections[server])
                    {
                        if (connection.Value.ContainsKey(target.Key))
                        {
                            connection.Value[target.Key] += target.Value * connection.Value[server];
                        }
                        else
                        {
                            connection.Value.Add(target.Key, target.Value * connection.Value[server]);
                        }
                    }
                }
            }
        }

        void SortInput()
        {
            var tempDict = new Dictionary<string, List<string>>();
            while (input.Any())
            {
                var connection = input.First(x => !input.Any(c => c.Value.Contains(x.Key)));
                tempDict.Add(connection.Key, connection.Value);
                input.Remove(connection.Key);
            }
            input = tempDict;
        }

        public void Task1()
        {
            Console.WriteLine(connections["you"]["out"]);
        }

        public void Task2()
        {
            long result = 1;
            if (connections["fft"].ContainsKey("dac"))
            {
                result *= connections["svr"]["fft"];
                result *= connections["fft"]["dac"];
                result *= connections["dac"]["out"];
            }
            else
            {
                result *= connections["svr"]["dac"];
                result *= connections["dac"]["fft"];
                result *= connections["fft"]["out"];
            }
            Console.WriteLine(result);
        }
    }
}