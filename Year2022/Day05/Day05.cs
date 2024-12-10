using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day05 : IDay
    {
        private List<string> stacks;
        //private List<string> stacks = new List<string>
        //{
        //    "",
        //    "ZN",
        //    "MCD",
        //    "P",
        //};

        private void SetUpStacks()
        {
            stacks = new List<string>
            {
                "",
                "ZJG",
                "QLRPWFVC",
                "FPMCLGR",
                "LFBWPHM",
                "GCFSVQ",
                "WHJZMQTL",
                "HFSBV",
                "FJZS",
                "MCDPFHBT"
            };
        }

        private void Move(int amount, int from, int to)
        {
            stacks[to] = stacks[to] + new string(stacks[from].Substring(stacks[from].Length - amount).Reverse().ToArray());
            stacks[from] = stacks[from].Substring(0, stacks[from].Length - amount);
        }
        
        private void Move2(int amount, int from, int to)
        {
            stacks[to] = stacks[to] + stacks[from].Substring(stacks[from].Length - amount);
            stacks[from] = stacks[from].Substring(0, stacks[from].Length - amount);
        }

        public void Task1()
        {
            SetUpStacks();
            var lines = StaticHelpers.GetLines(this.GetType());
            foreach(var line in lines)
            {
                var parsedLine = line.Replace("move", "").Replace("from", "").Replace("to", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                Move(int.Parse(parsedLine[0]), int.Parse(parsedLine[1]), int.Parse(parsedLine[2]));
            }
            for(int i = 1; i != stacks.Count; i++)
            {
                Console.Write(stacks[i].Last());
            }
            Console.WriteLine();
        }

        public void Task2()
        {
            SetUpStacks();
            var lines = StaticHelpers.GetLines(this.GetType());
            foreach (var line in lines)
            {
                var parsedLine = line.Replace("move", "").Replace("from", "").Replace("to", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                Move2(int.Parse(parsedLine[0]), int.Parse(parsedLine[1]), int.Parse(parsedLine[2]));
            }
            for (int i = 1; i != stacks.Count; i++)
            {
                Console.Write(stacks[i].Last());
            }
            Console.WriteLine();
        }
    }
}
