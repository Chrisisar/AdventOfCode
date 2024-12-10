using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day16 : IDay
    {
        class Valve
        {
            public string Name { get; set; }
            public int FlowRate { get; set; }
            public List<string> Connections = new List<string>();
            public List<(Valve valve, int distance, string route)> ValveConnections = new List<(Valve valve, int distance, string route)>();
        }

        List<Valve> valves = new List<Valve>();

        private void ParseInputLine(string line)
        {
            var splitLine = line.Replace("Valve ", "").Replace("has flow rate=", "")
                .Replace("; tunnels lead to valves", "").Replace("; tunnel leads to valve", "")
                .Replace(",", "").Split(" ");
            //Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
            Valve newValve = new Valve()
            {
                Name = splitLine[0],
                FlowRate = int.Parse(splitLine[1]),
            };
            for (int i = 2; i != splitLine.Count(); i++)
            {
                newValve.Connections.Add(splitLine[i]);
            }
            valves.Add(newValve);
        }

        long maximumFlow = 0;
        long numberOfDFSattempts = 0;
        private void DFS(Valve standingValve, int minutesRemaining, int sumOfFlow, string openValves)
        {
            numberOfDFSattempts++;
            maximumFlow = Math.Max(maximumFlow, sumOfFlow);
            if (minutesRemaining <= 0 || openValves.Length == (valves.Count - 1) * 3)
            {
                return;
            }
            if (standingValve.FlowRate > 0 && !openValves.Contains(standingValve.Name))
            {
                DFS(standingValve, minutesRemaining - 1, sumOfFlow + (minutesRemaining - 1) * standingValve.FlowRate, $"{openValves},{standingValve.Name}");
            }
            foreach (var availableValve in standingValve.ValveConnections)
            {
                if (minutesRemaining - availableValve.distance >= 0)
                {
                    DFS(availableValve.valve, minutesRemaining - availableValve.distance, sumOfFlow, openValves);
                }
            }
        }

        class HistoryRecord
        {
            public string Valve1Name { get; set; }
            public string Valve2Name { get; set; }
            public int SumOfFlow { get; set; }
            public int MinutesRemaining { get; set; }
            public int ElephantMinutesRemaining { get; set; }
        }
        List<HistoryRecord> history = new List<HistoryRecord>();

        private bool ExistsBetterHistory(Valve valve1, Valve valve2, int minutesRemaining, int elephantMinutesRemaining, int sumOfFlow)
        {
            return history.Any(h => h.Valve1Name == valve1.Name
                && h.Valve2Name == valve2.Name
                && h.MinutesRemaining >= minutesRemaining
                && h.ElephantMinutesRemaining >= elephantMinutesRemaining
                && h.SumOfFlow >= sumOfFlow);
        }

        private void DFS2(Valve standingValve, Valve elephantValve, int minutesRemaining, int elephantMinutes, int sumOfFlow, string openValves)
        {
            if (ExistsBetterHistory(standingValve, elephantValve, minutesRemaining, elephantMinutes, sumOfFlow))
            {
                return;
            }
            else
            {
                history.RemoveAll(h => h.Valve1Name == standingValve.Name
                && h.Valve2Name == elephantValve.Name
                && h.MinutesRemaining <= minutesRemaining
                && h.ElephantMinutesRemaining <= elephantMinutes
                && h.SumOfFlow <= sumOfFlow);
                history.Add(new HistoryRecord
                {
                    Valve1Name = standingValve.Name,
                    Valve2Name = elephantValve.Name,
                    MinutesRemaining = minutesRemaining,
                    ElephantMinutesRemaining = elephantMinutes,
                    SumOfFlow = sumOfFlow
                });
                maximumFlow = Math.Max(maximumFlow, sumOfFlow);
                if (sumOfFlow == 2215)
                {

                }
            }
            numberOfDFSattempts++;
            if (minutesRemaining <= 0 || elephantMinutes <= 0 || openValves.Length == (valves.Count - 1) * 3)
            {
                return;
            }
            if (standingValve.FlowRate > 0 && !openValves.Contains(standingValve.Name))
            {
                DFS2(standingValve, elephantValve, minutesRemaining - 1, elephantMinutes, sumOfFlow + (minutesRemaining - 1) * standingValve.FlowRate, $"{openValves},{standingValve.Name}");
            }
            if (elephantValve.FlowRate > 0 && !openValves.Contains(elephantValve.Name))
            {
                DFS2(standingValve, elephantValve, minutesRemaining, elephantMinutes - 1, sumOfFlow + (elephantMinutes - 1) * elephantValve.FlowRate, $"{openValves},{elephantValve.Name}");
            }
            if (standingValve.Name != elephantValve.Name
                && standingValve.FlowRate > 0 && !openValves.Contains(standingValve.Name)
                && elephantValve.FlowRate > 0 && !openValves.Contains(elephantValve.Name))
            {
                DFS2(standingValve, elephantValve, minutesRemaining - 1, elephantMinutes - 1, sumOfFlow + (elephantMinutes - 1) * elephantValve.FlowRate + (minutesRemaining - 1) * standingValve.FlowRate, $"{openValves},{standingValve.Name},{elephantValve.Name}");
            }
            foreach (var availableValve in standingValve.ValveConnections)
            {
                foreach (var elephantAvailableValve in elephantValve.ValveConnections)
                {
                    DFS2(availableValve.valve, elephantAvailableValve.valve, minutesRemaining - availableValve.distance, elephantMinutes - elephantAvailableValve.distance, sumOfFlow, openValves);
                }
            }
        }

        private void ProcessValves()
        {
            foreach (var valve in valves)
            {
                foreach (var connection in valve.Connections)
                {
                    valve.ValveConnections.Add((valves.Single(x => x.Name == connection), 1, ""));
                }
            }

            foreach (var zeroFlowValve in valves.Where(x => x.FlowRate == 0))
            {
                foreach (var connectedValve1 in zeroFlowValve.ValveConnections)
                {
                    foreach (var connectedValve2 in zeroFlowValve.ValveConnections.Where(x => x != connectedValve1))
                    {
                        var existingConnection = connectedValve1.valve.ValveConnections.FirstOrDefault(x => x.valve == connectedValve2.valve);
                        if (existingConnection == default((Valve, int, string)))
                        {
                            connectedValve1.valve.ValveConnections.Add((connectedValve2.valve, connectedValve1.distance + connectedValve2.distance, connectedValve1.route + "->" + zeroFlowValve.Name + "->" + connectedValve2.route));
                        }
                        else if (connectedValve1.distance + connectedValve2.distance < existingConnection.distance)
                        {
                            existingConnection.distance = connectedValve1.distance + connectedValve2.distance;
                        }
                    }
                    connectedValve1.valve.ValveConnections.RemoveAll(x => x.valve == zeroFlowValve);
                }
            }
            valves.RemoveAll(x => x.FlowRate == 0 && x.Name != "AA");
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            foreach (var line in lines)
            {
                ParseInputLine(line);
            }
            ProcessValves();

            DFS(valves.Single(x => x.Name == "AA"), 30, 0, "");
            Console.WriteLine(maximumFlow);
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType()).ToList();
            maximumFlow = 0;
            numberOfDFSattempts = 0;
            //PrintShortConnections();
            DFS2(valves.Single(x => x.Name == "AA"), valves.Single(x => x.Name == "AA"), 26, 26, 0, "");
            Console.WriteLine(maximumFlow);
        }

        private void PrintShortConnections()
        {
            foreach(var valve in valves)
            {
                foreach(var connection in valve.ValveConnections)
                {
                    Console.WriteLine($"{valve.Name}->{connection.route}->{connection.valve.Name} ({connection.distance})");
                }
            }
        }
    }
}
