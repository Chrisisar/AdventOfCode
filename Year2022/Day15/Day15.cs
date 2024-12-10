using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day15 : IDay
    {
        class Sensor
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Range { get; set; }
        }

        private List<Sensor> sensors = new List<Sensor>();
        private List<Sensor> beacons = new List<Sensor>();

        private void ParseInputLine(string line)
        {
            var splitLine = line.Replace("Sensor at x=", "").Replace(" y=", "").Replace(": closest beacon is at x=", ",").Split(",");
            int.TryParse(splitLine[0], out int sX);
            int.TryParse(splitLine[1], out int sY);
            int.TryParse(splitLine[2], out int bX);
            int.TryParse(splitLine[3], out int bY);
            Sensor newSensor = new Sensor()
            {
                X = sX,
                Y = sY,
                Range = Math.Abs(sX - bX) + Math.Abs(sY - bY)
            };
            sensors.Add(newSensor);
            Sensor newBeacon = new Sensor()
            {
                X = bX,
                Y = bY
            };
            beacons.Add(newBeacon);
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            foreach (var line in lines)
            {
                ParseInputLine(line);
            }
            int maxRange = sensors.Max(x => x.Range);
            int minX = sensors.Min(x => x.X);
            int maxX = sensors.Max(x => x.X);

            int result = 0;

            for (int x = minX - maxRange; x <= maxX + maxRange; x++)
            {
                if (!beacons.Any(b => b.X == x && b.Y == 2000000) && sensors.Any(s => s.Range - Math.Abs(s.Y - 2000000) >= Math.Abs(s.X - x)))
                {
                    //Console.Write("#");
                    result++;
                }
                else
                {
                    //Console.Write(".");
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            List<int> potentialRange0 = new List<int>();
            for (int i = 0; i < sensors.Count; i++)
            {
                for (int j = i + 1; j < sensors.Count; j++)
                {
                    var sensor1 = sensors[i];
                    var sensor2 = sensors[j];
                    if(sensor1.X + sensor1.Y + sensor1.Range + 2 == sensor2.X + sensor2.Y - sensor2.Range) //1 SE, 2 NW
                    {
                        potentialRange0.Add(sensor1.X + sensor1.Y + sensor1.Range + 1);
                    }
                    if(sensor2.X + sensor2.Y + sensor2.Range + 2 == sensor1.X + sensor1.Y - sensor1.Range) //1 NW, 2 SE
                    {
                        potentialRange0.Add(sensor2.X + sensor2.Y + sensor2.Range + 1);
                    }
                }
            }

            for (long i = potentialRange0.First() - 4000000; i <= 4000000; i++)
            {
                if (!sensors.Any(s => s.Range - Math.Abs(s.X - i) >= Math.Abs(s.Y - potentialRange0.First() + i)))
                {
                    Console.WriteLine($"x={i};y={potentialRange0.First() - i}");
                    Console.WriteLine(i * 4000000 + potentialRange0.First() - i);
                    break;
                }
            }
        }
    }
}
