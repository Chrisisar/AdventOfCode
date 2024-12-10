using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day13 : IDay
    {
        public class Signal
        {
            public List<Signal> ListOfSignals = new List<Signal>();
            public int? Value { get; set; }
            public Signal ParentSignal { get; set; }
        }
        bool breaker = false;

        public void Task1()
        {
            var result = 0;
            var lines = StaticHelpers.GetLines(this.GetType());
            Signal signal1 = null;
            Signal signal2 = null;
            var pair = 1;
            for (int i = 0; i != lines.Count(); i++)
            {
                if (i % 3 == 0)
                {
                    signal1 = ParseSignal(lines[i]);
                }
                else if (i % 3 == 1)
                {
                    signal2 = ParseSignal(lines[i]);
                }
                else
                {
                    breaker = false;
                    var areOrdered = AreSignalsOrdered(signal1, signal2);
                    if (areOrdered)
                    {
                        result += pair;
                    }
                    Console.WriteLine(areOrdered);
                    pair++;
                }
            }
            Console.WriteLine(result);
        }

        private bool AreSignalsOrdered(Signal signal1, Signal signal2)
        {
            if (breaker)
            {
                return true;
            }
            if (signal1.Value.HasValue && signal2.Value.HasValue)
            {
                if (signal1.Value < signal2.Value)
                {
                    breaker = true;
                }
                return signal1.Value <= signal2.Value;
            }
            else if (signal1.ListOfSignals.Any() && signal2.ListOfSignals.Any())
            {
                for (int i = 0; i != signal1.ListOfSignals.Count(); i++)
                {
                    if (signal2.ListOfSignals.Count() < i + 1)
                    {
                        return false;
                    }
                    if (!AreSignalsOrdered(signal1.ListOfSignals[i], signal2.ListOfSignals[i]))
                    {
                        return false;
                    }
                    if (breaker)
                    {
                        return true;
                    }
                }
                if (signal1.ListOfSignals.Count() < signal2.ListOfSignals.Count())
                {
                    breaker = true;
                }
                return true;
            }
            else if (!signal1.Value.HasValue && !signal2.Value.HasValue
                && !signal1.ListOfSignals.Any() && !signal2.ListOfSignals.Any())
            {
                return true;
            }
            else
            {
                if (!signal1.Value.HasValue && !signal1.ListOfSignals.Any())
                {
                    breaker = true;
                    return true;
                }
                else if (!signal2.Value.HasValue && !signal2.ListOfSignals.Any())
                {
                    return false;
                }
                else if (signal1.Value.HasValue)
                {
                    Signal newSignal = new Signal() { ParentSignal = signal1, Value = signal1.Value };
                    signal1.Value = null;
                    signal1.ListOfSignals.Add(newSignal);
                }
                else if (signal2.Value.HasValue)
                {
                    Signal newSignal = new Signal() { ParentSignal = signal2, Value = signal2.Value };
                    signal2.Value = null;
                    signal2.ListOfSignals.Add(newSignal);
                }
                return AreSignalsOrdered(signal1, signal2);
            }
        }

        private Signal ParseSignal(string signalString)
        {
            Signal root = new Signal();
            Signal currentSignal = root;
            Signal newSignal = null;

            string number = "";
            int parsedNumber = 0;
            for (int i = 0; i != signalString.Length; i++)
            {
                switch (signalString[i])
                {
                    case '[':
                        if (currentSignal.Value.HasValue)
                        {
                            newSignal = new Signal() { ParentSignal = currentSignal, Value = currentSignal.Value };
                            currentSignal.ListOfSignals.Add(newSignal);
                            currentSignal.Value = null;
                        }
                        newSignal = new Signal() { ParentSignal = currentSignal };
                        currentSignal.ListOfSignals.Add(newSignal);
                        currentSignal = newSignal;
                        break;
                    case ']':
                        if (!string.IsNullOrWhiteSpace(number))
                        {
                            int.TryParse(number, out parsedNumber);
                            number = "";
                            if (currentSignal.ListOfSignals.Any() || currentSignal.Value.HasValue)
                            {
                                if (currentSignal.Value.HasValue)
                                {
                                    newSignal = new Signal() { ParentSignal = currentSignal, Value = currentSignal.Value };
                                    currentSignal.ListOfSignals.Add(newSignal);
                                    currentSignal.Value = null;
                                }
                                if (currentSignal.ListOfSignals.Any())
                                {
                                    newSignal = new Signal() { ParentSignal = currentSignal, Value = parsedNumber };
                                    currentSignal.ListOfSignals.Add(newSignal);
                                }
                            }
                            else
                            {
                                newSignal = new Signal() { ParentSignal = currentSignal, Value = parsedNumber };
                                currentSignal.ListOfSignals.Add(newSignal);
                            }
                        }
                        //else
                        //{
                        //    if (!currentSignal.ListOfSignals.Any() && !currentSignal.Value.HasValue)
                        //    {
                        //        currentSignal.Value = 0;
                        //    }
                        //}
                        currentSignal = currentSignal.ParentSignal;
                        break;
                    case ',':
                        if (!string.IsNullOrWhiteSpace(number))
                        {
                            int.TryParse(number, out parsedNumber);
                            number = "";
                            if (currentSignal.ListOfSignals.Any() || currentSignal.Value.HasValue)
                            {
                                if (currentSignal.Value.HasValue)
                                {
                                    newSignal = new Signal() { ParentSignal = currentSignal, Value = currentSignal.Value };
                                    currentSignal.ListOfSignals.Add(newSignal);
                                    currentSignal.Value = null;
                                }
                                if (currentSignal.ListOfSignals.Any())
                                {
                                    newSignal = new Signal() { ParentSignal = currentSignal, Value = parsedNumber };
                                    currentSignal.ListOfSignals.Add(newSignal);
                                }
                            }
                            else
                            {
                                currentSignal.Value = parsedNumber;
                            }
                        }
                        break;
                    default:
                        number += signalString[i];
                        break;
                }
            }

            return root;
        }


        public void Task2()
        {
            var smallerThan2 = 1;
            var smallerThan6 = 2;
            var lines = StaticHelpers.GetLines(this.GetType());

            Signal signal1 = null;
            Signal signal2 = ParseSignal("[[2]]");
            Signal signal6 = ParseSignal("[[6]]");
            for (int i = 0; i != lines.Count(); i++)
            {
                if (i % 3 == 2)
                {

                }
                else
                {
                    breaker = false;
                    signal1 = ParseSignal(lines[i]);
                    if (AreSignalsOrdered(signal1, signal2))
                    {
                        smallerThan2++;

                    }
                    breaker = false;
                    signal1 = ParseSignal(lines[i]);
                    if (AreSignalsOrdered(signal1, signal6))
                    {
                        smallerThan6++;
                    }
                }
            }
            Console.WriteLine(smallerThan2 * smallerThan6);

        }
    }
}
