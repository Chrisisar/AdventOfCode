using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day05 : IDay
    {
        public Day05(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        List<(int prior, int later)> rules = new List<(int prior, int later)>();
        List<List<int>> updates = new List<List<int>>();

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            int i = 0;
            while (!string.IsNullOrWhiteSpace(lines[i]))
            {
                var splitRule = lines[i].Split('|');
                rules.Add((int.Parse(splitRule[0]), int.Parse(splitRule[1])));
                i++;
            }
            i++;
            while (i < lines.Length)
            {
                var splitUpdate = lines[i].Split(',');
                List<int> update = new List<int>();
                for (int j = 0; j != splitUpdate.Length; j++)
                {
                    update.Add(int.Parse(splitUpdate[j]));
                }
                updates.Add(update);
                i++;
            }
        }

        public void Task1()
        {
            long result = 0;
            foreach (var update in updates)
            {
                result += IsUpdateValid(update) ? update[update.Count / 2] : 0;
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            long result = 0;
            List<List<int>> invalidUpdates = new List<List<int>>();
            foreach (var update in updates)
            {
                if (!IsUpdateValid(update))
                {
                    invalidUpdates.Add(update);
                }
            }

            foreach (var update in invalidUpdates)
            {
                result += GetMiddleNumberOfFixedUpdate(update);
            }
            Console.WriteLine(result);
        }

        private long GetMiddleNumberOfFixedUpdate(List<int> update)
        {
            List<int> fixedUpdate = new List<int>();
            while (update.Count > 0)
            {
                for (int i = 0; i < update.Count; i++)
                {
                    if (!rules.Any(r => update[i] == r.later && update.Contains(r.prior)))
                    {
                        fixedUpdate.Add(update[i]);
                        update.RemoveAll(u => u == update[i]);
                    }
                }
            }
            return fixedUpdate[fixedUpdate.Count /2];
        }

        bool IsUpdateValid(List<int> update)
        {
            for (int i = 0; i < update.Count; i++)
            {
                for (int j = i + 1; j < update.Count; j++)
                {
                    if (rules.Any(r => update[i] == r.later && update[j] == r.prior))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}