using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Year2024
{
    class Day24 : IDay
    {
        Dictionary<string, bool> gates = new Dictionary<string, bool>();
        List<(string gateA, string operation, string gateB, string gateC)> wires = new List<(string, string, string, string)>();

        public Day24(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            int i = 0;
            while (!string.IsNullOrWhiteSpace(lines[i]))
            {
                var splitLine = lines[i].Split(": ");
                gates.Add(splitLine[0], splitLine[1] == "1" ? true : false);
                i++;
            }
            i++;
            for (; i < lines.Length; i++)
            {
                var splitLine = lines[i].Split(' ');

                wires.Add((splitLine[0], splitLine[1], splitLine[2], splitLine[4]));
            }
        }

        void EvaluateWires(List<(string gateA, string operation, string gateB, string gateC)> wires)
        {
            while (wires.Count > 0)
            {
                var selectedWire = wires.First(w => gates.ContainsKey(w.gateA) && gates.ContainsKey(w.gateB));
                wires.Remove(selectedWire);

                gates.TryGetValue(selectedWire.gateA, out bool gate1);
                gates.TryGetValue(selectedWire.gateB, out bool gate2);
                bool result;
                switch (selectedWire.operation)
                {
                    case "AND":
                        result = gate1 && gate2;
                        break;
                    case "OR":
                        result = gate1 || gate2;
                        break;
                    case "XOR":
                        result = gate1 ^ gate2;
                        break;
                    default:
                        throw new Exception();
                }
                gates.Add(selectedWire.gateC, result);
            }
        }

        public void Task1()
        {
            EvaluateWires(wires.ToList());
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var zGate in gates.Where(g => g.Key.StartsWith('z')).OrderByDescending(g => g.Key))
            {
                stringBuilder.Append(zGate.Value ? "1" : "0");
            };
            long result = StaticHelpers.GetLongFromStringBinary(stringBuilder.ToString());
            Console.WriteLine(result);
        }

        List<string> wiresToSwap = new List<string>();

        public void Task2()
        {
            int zCount = gates.Count(x => x.Key.StartsWith('z'));
            string carry = ConfirmHalfAdder();
            for (int i = 1; i < zCount -  1; i++)
            {
                carry = ConfirmFullAdder(i, carry);
            }

            Console.WriteLine(string.Join(",", wiresToSwap.Order()));
        }

        string ConfirmHalfAdder()
        {
            string xGate = "x00";
            string yGate = "y00";
            string zGate = "z00";

            string sumGate = wires.FirstOrDefault(w => MatchWire(w, xGate, yGate, "XOR")).gateC;
            if (sumGate != zGate)
            {
                wiresToSwap.Add(sumGate);
                wiresToSwap.Add(zGate);
                SwapWires(sumGate, zGate);
            }

            string carryGate = wires.FirstOrDefault(w => MatchWire(w, xGate, yGate, "AND")).gateC;
            return carryGate;
        }

        string ConfirmFullAdder(int i, string carryIn, bool secondTry = false)
        {
            string xGate = "x" + i.ToString("00");
            string yGate = "y" + i.ToString("00");
            string zGate = "z" + i.ToString("00");

            string xXorYGate = wires.FirstOrDefault(w => MatchWire(w, xGate, yGate, "XOR")).gateC;
            string sumGate = wires.FirstOrDefault(w => MatchWire(w, xXorYGate, carryIn, "XOR")).gateC;

            if(sumGate == null && !secondTry)
            {
                var expectedConnectors = wires.FirstOrDefault(w => w.gateC == zGate && w.operation == "XOR");
                if (carryIn != expectedConnectors.gateA && carryIn != expectedConnectors.gateB)
                {
                    wiresToSwap.Add(carryIn);                 
                    if (expectedConnectors.gateA == xXorYGate)
                    {
                        wiresToSwap.Add(expectedConnectors.gateB);
                    }
                    else if(expectedConnectors.gateB == xXorYGate)
                    {
                        wiresToSwap.Add(expectedConnectors.gateA);
                    }
                }
                if(xXorYGate != expectedConnectors.gateA && xXorYGate != expectedConnectors.gateB)
                {
                    wiresToSwap.Add(xXorYGate);
                    if (expectedConnectors.gateA == carryIn)
                    {
                        wiresToSwap.Add(expectedConnectors.gateB);
                    }
                    else if (expectedConnectors.gateB == carryIn)
                    {
                        wiresToSwap.Add(expectedConnectors.gateA);
                    }
                }
                SwapWires(wiresToSwap[wiresToSwap.Count - 1], wiresToSwap[wiresToSwap.Count - 2]);

                Console.WriteLine("Sum connectors wrong");
                return ConfirmFullAdder(i, carryIn, true);
            }

            if (sumGate != zGate && !secondTry)
            {
                wiresToSwap.Add(sumGate);
                wiresToSwap.Add(zGate);
                SwapWires(sumGate, zGate);
                return ConfirmFullAdder(i, carryIn, true);
            }

            string carryInAndXXorYGate = wires.FirstOrDefault(w => MatchWire(w, xXorYGate, carryIn, "AND")).gateC;
            string xAndYGate = wires.FirstOrDefault(w => MatchWire(w, xGate, yGate, "AND")).gateC;
            string carryOutGate = wires.FirstOrDefault(w => MatchWire(w, carryInAndXXorYGate, xAndYGate, "OR")).gateC;

            if (carryOutGate == null)
            {
                Console.WriteLine($"CarryOut is null");
            }

            return carryOutGate;
        }

        void SwapWires(string gate1, string gate2)
        {
            var wire1 = wires.FirstOrDefault(x => x.gateC == gate1);
            var wire2 = wires.FirstOrDefault(x => x.gateC == gate2);
            wires.Remove(wire1);
            wires.Remove(wire2);
            wires.Add((wire1.gateA, wire1.operation, wire1.gateB, gate2));
            wires.Add((wire2.gateA, wire2.operation, wire2.gateB, gate1));
        }

        bool MatchWire((string gateA, string operation, string gateB, string gateC) wire, string gate1, string gate2, string operation)
        {
            return wire.operation == operation
                && (wire.gateA == gate1 && wire.gateB == gate2 || wire.gateA == gate2 && wire.gateB == gate1);
        }

        #region Utils

        private void ClearGates()
        {
            foreach(var gate in gates.Keys)
            {
                if (!gate.StartsWith('x') && !gate.StartsWith('y'))
                    gates.Remove(gate);
            }
        }

        List<string> IdentifyDifferencesIndexes()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var zGate in gates.Where(g => g.Key.StartsWith('x')).OrderByDescending(g => g.Key))
            {
                stringBuilder.Append(zGate.Value ? "1" : "0");
            };
            string xBinary = stringBuilder.ToString();

            stringBuilder = new StringBuilder();
            foreach (var zGate in gates.Where(g => g.Key.StartsWith('y')).OrderByDescending(g => g.Key))
            {
                stringBuilder.Append(zGate.Value ? "1" : "0");
            };
            string yBinary = stringBuilder.ToString();

            stringBuilder = new StringBuilder();
            foreach (var zGate in gates.Where(g => g.Key.StartsWith('z')).OrderByDescending(g => g.Key))
            {
                stringBuilder.Append(zGate.Value ? "1" : "0");
            };
            string zBinary = stringBuilder.ToString();
            string xaddyBinary = StaticHelpers.BinaryAddition(xBinary, yBinary);

            List<string> differenceIndexes = new List<string>();
            for (int i = 0; i != zBinary.Length; i++)
            {
                if (zBinary[zBinary.Length - i - 1] != xaddyBinary[zBinary.Length - i - 1])
                    differenceIndexes.Add("z" + i.ToString("00"));
            }
            return differenceIndexes;
        }

        #endregion
    }
}