using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Scanner
    {
        public Scanner(int id)
        {
            ScannerId = id;
            Beacons = new List<Beacon>();
            Coords = new Coords(0, 0, 0);
        }

        public int ScannerId { get; set; }

        public List<Beacon> Beacons { get; set; }

        public Coords Coords { get; set; }

        public int TransformId = 0;

        public void Transform()
        {
            switch (true)
            {
                case bool when TransformId % 2 == 0:
                    Beacons.ForEach(b =>
                    {
                        b.Coords.X = -b.Coords.X;
                        b.Coords.Z = -b.Coords.Z;
                    });
                    Coords.X = -Coords.X;
                    Coords.Z = -Coords.Z;
                    break;
                case bool when TransformId % 8 == 1:
                    Beacons.ForEach(b =>
                    {
                        b.Coords.X = -b.Coords.X;
                        b.Coords.Y = -b.Coords.Y;
                    });
                    Coords.X = -Coords.X;
                    Coords.Y = -Coords.Y;
                    break;
                case bool when TransformId % 8 == 3:
                    Beacons.ForEach(b =>
                    {
                        var temp = b.Coords.X;
                        b.Coords.X = b.Coords.Z;
                        b.Coords.Z = temp;
                        b.Coords.Y = -b.Coords.Y;
                    });
                    var temp = Coords.X;
                    Coords.X = -Coords.Z;
                    Coords.Z = temp;
                    Coords.Y = -Coords.Y;
                    break;
                case bool when TransformId % 8 == 5:
                    Beacons.ForEach(b =>
                    {
                        b.Coords.X = -b.Coords.X;
                        b.Coords.Y = -b.Coords.Y;
                    });
                    Coords.X = -Coords.X;
                    Coords.Y = -Coords.Y;
                    break;
                case bool when TransformId == 7:
                    Beacons.ForEach(b =>
                    {
                        var temp = b.Coords.X;
                        b.Coords.X = b.Coords.Y;
                        b.Coords.Y = -b.Coords.Z;
                        b.Coords.Z = -temp;
                    });
                    temp = Coords.X;
                    Coords.X = Coords.Y;
                    Coords.Y = -Coords.Z;
                    Coords.Z = -temp;
                    break;
                case bool when TransformId == 15:
                    Beacons.ForEach(b =>
                    {
                        var temp = b.Coords.X;
                        b.Coords.X = b.Coords.Y;
                        b.Coords.Y = -temp;
                    });
                    temp = Coords.X;
                    Coords.X = Coords.Y;
                    Coords.Y = -temp;
                    break;
                case bool when TransformId == 23:
                    Beacons.ForEach(b =>
                    {
                        var temp = b.Coords.X;
                        b.Coords.X = b.Coords.Z;
                        b.Coords.Z = -b.Coords.Y;
                        b.Coords.Y = -temp;
                    });
                    temp = Coords.X;
                    Coords.X = Coords.Z;
                    Coords.Z = -Coords.Y;
                    Coords.Y = -temp;
                    break;
            }
            TransformId++;
            TransformId %= 24;
        }

        public void MoveBeacons(int x, int y, int z)
        {
            Coords.X += x;
            Coords.Y += y;
            Coords.Z += z;
            Beacons.ForEach(b =>
            {
                b.Coords.X += x;
                b.Coords.Y += y;
                b.Coords.Z += z;
            });
        }

        public bool AreMatching(Scanner scanner)
        {
            IEnumerable<Coords> allBeaconCoords = scanner.Beacons.Concat(this.Beacons).Select(x => x.Coords);
            var groups = allBeaconCoords.GroupBy(x => new { x.X, x.Y, x.Z });
            var numberOfMatchingCoords = groups.Where(x => x.Count() > 1).Count();
            if (numberOfMatchingCoords >= 12)
            {
                Console.WriteLine($"current scanner({ScannerId}): {numberOfMatchingCoords} matches with fixed scanner {scanner.ScannerId}.");
                return true;
            }
            return false;
        }

    }

    class Beacon
    {
        public Beacon(int x, int y, int z)
        {
            Coords = new Coords(x, y, z);
        }
        public Coords Coords { get; set; }
    }

    class Coords
    {
        public Coords(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X, Y, Z;
    }

    class Day19 : IDay
    {
        public Day19()
        {
            PopulateScanners();
        }

        List<Scanner> Scanners = new List<Scanner>();

        private void PopulateScanners()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            int scannerNumber = 0;
            Scanner scanner = null;
            foreach (var line in lines)
            {
                if (line.StartsWith("---"))
                {
                    scanner = new Scanner(scannerNumber);
                    scannerNumber++;
                }
                else if (string.IsNullOrEmpty(line))
                {
                    Scanners.Add(scanner);
                }
                else
                {
                    var splitLine = line.Split(",");
                    Beacon beacon = new Beacon(int.Parse(splitLine[0]), int.Parse(splitLine[1]), int.Parse(splitLine[2]));
                    scanner.Beacons.Add(beacon);
                }
            }
            Scanners.Add(scanner);
        }

        List<Scanner> FixedScanners = new List<Scanner>();

        private bool TryFitScanner(Scanner scanner)
        {
            foreach (var fixedScanner in FixedScanners)
            {
                if (TryOverlapScanners(fixedScanner, scanner))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TryOverlapScanners(Scanner fixedScanner, Scanner scanner)
        {
            for (int t = 0; t != 24; t++)
            {
                for (int i = 0; i != fixedScanner.Beacons.Count; i++)
                {
                    for (int j = 0; j != scanner.Beacons.Count - 11; j++)
                    {
                        var xDiff = fixedScanner.Beacons[i].Coords.X - scanner.Beacons[j].Coords.X;
                        var yDiff = fixedScanner.Beacons[i].Coords.Y - scanner.Beacons[j].Coords.Y;
                        var zDiff = fixedScanner.Beacons[i].Coords.Z - scanner.Beacons[j].Coords.Z;
                        scanner.MoveBeacons(xDiff, yDiff, zDiff);
                        if (scanner.AreMatching(fixedScanner))
                        {
                            return true;
                        }
                        scanner.MoveBeacons(-xDiff, -yDiff, -zDiff);
                    }
                }
                scanner.Transform();
            }
            return false;
        }

        void OutputFixedBeacons()
        {
            foreach (var beaconCoords in FixedScanners.SelectMany(x => x.Beacons.Select(b => b.Coords)).Distinct().OrderBy(x => x.X))
            {
                Console.WriteLine($"{beaconCoords.X}, {beaconCoords.Y}, {beaconCoords.Z}");
            }
        }

        public void Task1()
        {
            var firstScanner = Scanners.First();
            FixedScanners.Add(firstScanner);
            Scanners.Remove(firstScanner);
            while (Scanners.Count > 0)
            {
                var outstandingScanners = new List<Scanner>(Scanners);
                foreach (var scanner in outstandingScanners)
                {
                    if (TryFitScanner(scanner))
                    {
                        Scanners.Remove(scanner);
                        FixedScanners.Add(scanner);
                    }
                }
            }
            IEnumerable<Coords> allBeaconCoords = FixedScanners.SelectMany(x => x.Beacons.Select(b => b.Coords));
            var groups = allBeaconCoords.GroupBy(x => new { x.X, x.Y, x.Z });
            Console.WriteLine(groups.Count());
        }

        public void Task2()
        {
            var result = 0;
            for (int i = 0; i != FixedScanners.Count; i++)
            {
                for (int j = 0; j != FixedScanners.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    var distance = Math.Abs(FixedScanners[i].Coords.X - FixedScanners[j].Coords.X)
                        + Math.Abs(FixedScanners[i].Coords.Y - FixedScanners[j].Coords.Y)
                        + Math.Abs(FixedScanners[i].Coords.Z - FixedScanners[j].Coords.Z);
                    if (distance > result)
                    {
                        result = distance;
                    }
                }
            }
            Console.WriteLine(result);
        }
    }
}
