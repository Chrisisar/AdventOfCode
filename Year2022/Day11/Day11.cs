using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day11 : IDay
    {
        public delegate long Operation(long item);

        public class Monkey
        {
            public List<long> Items = new List<long>();
            public Operation Operation;
            public long Test { get; set; }
            public int TestTrue { get; set; }
            public int TestFalse { get; set; }
            public long NumberOfInspects { get; set; }
        }

        private List<Monkey> PrepareMonkeys()
        {
            List<Monkey> monkeys = new List<Monkey>();
            var lines = StaticHelpers.GetLines(this.GetType());
            Monkey currentMonkey = new Monkey();
            monkeys.Add(currentMonkey);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    currentMonkey = new Monkey();
                    monkeys.Add(currentMonkey);
                }
                else if (line.StartsWith("Monkey"))
                {
                    //ignore line
                }
                else if (line.Contains("Starting items"))
                {
                    var listOfItems = line.Replace("Starting items: ", "").Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in listOfItems)
                    {
                        currentMonkey.Items.Add(long.Parse(item));
                    }
                }
                else if (line.Contains("Operation"))
                {
                    var splitOperation = line.Replace("Operation: new = ", "").Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var operation = splitOperation[1];
                    switch (operation)
                    {
                        case "+":
                            if (splitOperation[2] == "old")
                            {
                                currentMonkey.Operation = (item) => { return item + item; };
                            }
                            else
                            {
                                currentMonkey.Operation = (item) => { return item + long.Parse(splitOperation[2]); };
                            }
                            break;
                        case "-":
                            if (splitOperation[2] == "old")
                            {
                                currentMonkey.Operation = (item) => { return item - item; };
                            }
                            else
                            {
                                currentMonkey.Operation = (item) => { return item - long.Parse(splitOperation[2]); };
                            }
                            break;
                        case "*":
                            if (splitOperation[2] == "old")
                            {
                                currentMonkey.Operation = (item) => { return item * item; };
                            }
                            else
                            {
                                currentMonkey.Operation = (item) => { return item * long.Parse(splitOperation[2]); };
                            }
                            break;
                        case "/":
                            if (splitOperation[2] == "old")
                            {
                                currentMonkey.Operation = (item) => { return item / item; };
                            }
                            else
                            {
                                currentMonkey.Operation = (item) => { return item / long.Parse(splitOperation[2]); };
                            }
                            break;
                    }

                }
                else if (line.Contains("Test"))
                {
                    currentMonkey.Test = long.Parse(line.Replace("Test: divisible by ", ""));
                }
                else if (line.Contains("If true:"))
                {
                    currentMonkey.TestTrue = int.Parse(line.Replace("If true: throw to monkey ", ""));
                }
                else if (line.Contains("If false:"))
                {
                    currentMonkey.TestFalse = int.Parse(line.Replace("If false: throw to monkey ", ""));
                }
            }
            return monkeys;
        }

        public void Task1()
        {
            List<Monkey> monkeys = PrepareMonkeys();
            for (int round = 0; round != 20; round++)
            {
                foreach (var monkey in monkeys)
                {
                    for (int i = 0;i!= monkey.Items.Count; i++)
                    {
                        monkey.Items[i] = monkey.Operation(monkey.Items[i]);
                        monkey.Items[i] /= 3;
                        monkey.NumberOfInspects++;
                        if (monkey.Items[i] % monkey.Test == 0)
                        {
                            monkeys[monkey.TestTrue].Items.Add(monkey.Items[i]);
                        }
                        else
                        {
                            monkeys[monkey.TestFalse].Items.Add(monkey.Items[i]);
                        }
                    }
                    monkey.Items.Clear();
                }
            }
            monkeys = monkeys.OrderByDescending(x => x.NumberOfInspects).ToList();
            Console.WriteLine(monkeys[0].NumberOfInspects * monkeys[1].NumberOfInspects);
        }

        public void Task2()
        {
            List<Monkey> monkeys = PrepareMonkeys();
            long moduloValue = 1;
            foreach(var monkey in monkeys)
            {
                moduloValue *= monkey.Test;
            }
            for (int round = 0; round != 10000; round++)
            {
                foreach (var monkey in monkeys)
                {
                    for (int i = 0; i != monkey.Items.Count; i++)
                    {
                        monkey.Items[i] = monkey.Operation(monkey.Items[i]);
                        monkey.Items[i] %= moduloValue;
                        monkey.NumberOfInspects++;
                        if (monkey.Items[i] % monkey.Test == 0)
                        {
                            monkeys[monkey.TestTrue].Items.Add(monkey.Items[i]);
                        }
                        else
                        {
                            monkeys[monkey.TestFalse].Items.Add(monkey.Items[i]);
                        }
                    }
                    monkey.Items.Clear();
                }
            }
            monkeys = monkeys.OrderByDescending(x => x.NumberOfInspects).ToList();
            Console.WriteLine(monkeys[0].NumberOfInspects * monkeys[1].NumberOfInspects);
        }
    }
}
