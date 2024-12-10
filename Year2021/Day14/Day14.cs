using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Polymer
    {
        public Polymer(string line)
        {
            var splitLine = line.Split(" -> ");
            Left = splitLine[0][0];
            Right = splitLine[0][1];
            Middle = splitLine[1][0];
        }

        public char Left { get; set; }
        public char Middle { get; set; }
        public char Right { get; set; }
    }

    class Day14 : IDay
    {
        private List<Polymer> PolymerList = new List<Polymer>();

        private string GenerateNewPolymerChain(string polymerChain)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i != polymerChain.Length - 1; i++)
            {
                sb.Append(polymerChain[i]);
                var polymerFound = PolymerList.FirstOrDefault(x => x.Left == polymerChain[i] && x.Right == polymerChain[i + 1]);
                if (polymerFound != null)
                {
                    sb.Append(polymerFound.Middle);
                }
            }
            sb.Append(polymerChain[polymerChain.Length - 1]);
            return sb.ToString();
        }

        private Dictionary<string, long> GenerateNewPolymerChainLight(Dictionary<string, long> currentPairs)
        {
            var newPairs = new Dictionary<string, long>();
            foreach(var pair in currentPairs)
            {
                var foundPolimer = PolymerList.FirstOrDefault(x => x.Left == pair.Key[0] && x.Right == pair.Key[1]);
                string newPair1 = foundPolimer.Left.ToString() + foundPolimer.Middle.ToString();
                string newPair2 = foundPolimer.Middle.ToString() + foundPolimer.Right.ToString();
                if (!newPairs.TryAdd(newPair1, pair.Value))
                {
                    newPairs[newPair1]+=pair.Value;
                }
                if (!newPairs.TryAdd(newPair2, pair.Value))
                {
                    newPairs[newPair2]+=pair.Value;
                }
            }
            return newPairs;
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            for (int i = 2; i != lines.Length; i++)
            {
                PolymerList.Add(new Polymer(lines[i]));
            }

            string polymerChain = lines[0];
            for (int i = 0; i != 10; i++)
            {
                polymerChain = GenerateNewPolymerChain(polymerChain);
            }
            var b = polymerChain.GroupBy(x => x).OrderBy(x => x.Count()).ToList();
            Console.WriteLine(b[b.Count-1].Count() - b[0].Count());
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType()); 
            string polymerChain = lines[0];
            Dictionary<string, long> currentPairs = new Dictionary<string, long>();
            for (int i = 0; i != polymerChain.Length - 1; i++)
            {
                string pair = polymerChain[i].ToString() + polymerChain[i + 1].ToString();
                if(!currentPairs.TryAdd(pair, 1))
                {
                    currentPairs[pair]++;
                }
            }

            for (int i = 0; i != 40; i++)
            {
                currentPairs = GenerateNewPolymerChainLight(currentPairs);
            }
            OutputNumberOfLetters(currentPairs, polymerChain);
        }

        private void OutputNumberOfLetters(Dictionary<string,long> currentPairs, string initialChain)
        {
            Dictionary<char, long> numberOfLetters = new Dictionary<char, long>();
            foreach (var pair in currentPairs)
            {
                if(!numberOfLetters.TryAdd(pair.Key[0], pair.Value))
                {
                    numberOfLetters[pair.Key[0]] += pair.Value;
                }
                if(!numberOfLetters.TryAdd(pair.Key[1], pair.Value))
                {
                    numberOfLetters[pair.Key[1]] += pair.Value;
                }
            }
            numberOfLetters[initialChain[0]]++;
            numberOfLetters[initialChain[initialChain.Length - 1]]++;

            var orderedList = numberOfLetters.OrderBy(x => x.Value).ToList();
            Console.WriteLine((orderedList[orderedList.Count - 1].Value - orderedList[0].Value) * 0.5);

        }
    }
}
