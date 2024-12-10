using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day21 : IDay
    {
        class Monkey
        {
            public string Name { get; set; }
            public long? Value { get; set; }
            public string Monkey1Reference { get; set; }
            public string Monkey2Reference { get; set; }
            public char Operation { get; set; }
        }

        List<Monkey> monkeys = new List<Monkey>();

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            foreach(var line in lines)
            {
                monkeys.Add(ParseMonkey(line));
            }
            Console.WriteLine(CalculateMonkey("root"));
        }

        List<string> routeToHuman = new List<string>();

        private long CalculateMonkey(string name)
        {
            Monkey monkey = monkeys.FirstOrDefault(m => m.Name == name);
            if(!monkey.Value.HasValue)
            {
                long monkey1Value = CalculateMonkey(monkey.Monkey1Reference);
                long monkey2Value = CalculateMonkey(monkey.Monkey2Reference);
                switch (monkey.Operation)
                {
                    case '+':
                        monkey.Value = monkey1Value + monkey2Value;
                        break;
                    case '-':
                        monkey.Value = monkey1Value - monkey2Value;
                        break;
                    case '*':
                        monkey.Value = monkey1Value * monkey2Value;
                        break;
                    case '/':
                        monkey.Value = monkey1Value / monkey2Value;
                        break;
                }
            }
            return monkey.Value.Value;
        }

        bool FindMonkey(string name)
        {
            if(name == "humn")
            {
                routeToHuman.Insert(0, name);
                return true;
            }
            Monkey monkey = monkeys.FirstOrDefault(m => m.Name == name);
            var success = false;
            if (!string.IsNullOrWhiteSpace(monkey.Monkey1Reference))
            {
                success = FindMonkey(monkey.Monkey1Reference);
                if (!success)
                {
                    success = FindMonkey(monkey.Monkey2Reference);
                }
            }
            if (success)
            {
                routeToHuman.Insert(0, name);
            }
            return success;
        }

        private Monkey ParseMonkey(string line)
        {
            Monkey newMonkey = new Monkey();
            var splitMonkey = line.Split(": ");
            newMonkey.Name = splitMonkey[0];
            if(long.TryParse(splitMonkey[1], out long monkeyValue))
            {
                newMonkey.Value = monkeyValue;
            }
            else
            {
                var splitOperation = splitMonkey[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                newMonkey.Monkey1Reference = splitOperation[0];
                newMonkey.Operation = splitOperation[1][0];
                newMonkey.Monkey2Reference = splitOperation[2];
            }
            return newMonkey;
        }

        private void CalculateBackwards(Monkey monkey, long requiredValue)
        {
            routeToHuman.RemoveAt(0);
            if (monkey.Name == "humn")
            {
                Console.WriteLine(requiredValue);
                return;
            }
            Monkey monkey1 = monkeys.First(m => m.Name == monkey.Monkey1Reference);
            Monkey monkey2 = monkeys.First(m => m.Name == monkey.Monkey2Reference);
            if (monkey1.Name == routeToHuman[0])
            {
                if(monkey.Name == "root")
                {
                    requiredValue = monkey2.Value.Value;
                }
                else
                {
                    switch (monkey.Operation)
                    {
                        case '+':
                            requiredValue -= monkey2.Value.Value;
                            break;
                        case '-':
                            requiredValue += monkey2.Value.Value;
                            break;
                        case '*':
                            requiredValue /= monkey2.Value.Value;
                            break;
                        case '/':
                            requiredValue *= monkey2.Value.Value;
                            break;
                    }
                }                
                CalculateBackwards(monkey1, requiredValue);
            }
            else
            {
                if (monkey.Name == "root")
                {
                    requiredValue = monkey1.Value.Value;
                }
                else
                {
                    switch (monkey.Operation)
                    {
                        case '+':
                            requiredValue -= monkey1.Value.Value;
                            break;
                        case '-':
                            requiredValue = monkey1.Value.Value - requiredValue;
                            break;
                        case '*':
                            requiredValue /= monkey1.Value.Value;
                            break;
                        case '/':
                            requiredValue = monkey1.Value.Value / requiredValue;
                            break;
                    }
                }                
                CalculateBackwards(monkey2, requiredValue);
            }
        }

        public void Task2()
        {
            Monkey rootMonkey = monkeys.FirstOrDefault(m => m.Name == "root");
            FindMonkey("root");
            CalculateBackwards(rootMonkey, 0);

        }
    }
}
