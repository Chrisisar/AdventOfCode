using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2025
{
    class Day08 : IDay
    {
        int ConnectionAmount;
        List<(long x, long y, long z)> JunctionBoxesLocations = new List<(long x, long y, long z)>();
        List<(long distance, int junctionA, int junctionB)> distancesList = new List<(long distance, int junctionA, int junctionB)>();

        public Day08(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            int.TryParse(lines[0], out ConnectionAmount);
            for (int i = 1; i != lines.Length; i++)
            {
                var splitLine = lines[i].Split(',');
                int.TryParse(splitLine[0], out int x);
                int.TryParse(splitLine[1], out int y);
                int.TryParse(splitLine[2], out int z);
                JunctionBoxesLocations.Add((x, y, z));
            }
        }

        void CalculateDistances()
        {
            for (int i = 0; i < JunctionBoxesLocations.Count; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (i == j)
                        continue;

                    var A = JunctionBoxesLocations[i];
                    var B = JunctionBoxesLocations[j];

                    long distanceSquared = (A.x - B.x) * (A.x - B.x)
                        + (A.y - B.y) * (A.y - B.y)
                        + (A.z - B.z) * (A.z - B.z);

                    distancesList.Add((distanceSquared, i, j));
                }
            }
            distancesList = distancesList.OrderBy(x => x.distance).ToList();
        }

        public void Task1()
        {
            CalculateDistances();
            List<List<int>> junctionBoxesGroups = new List<List<int>>();
            for (int i = 0; i != ConnectionAmount; i++)
            {
                ConnectNextClosestJunction(junctionBoxesGroups, i);
            }
            long result = 1;
            foreach (var boxListCount in junctionBoxesGroups.OrderByDescending(x => x.Count).Take(3))
            {
                result *= boxListCount.Count;
            }
            Console.WriteLine(result);
        }

        private void ConnectNextClosestJunction(List<List<int>> junctionBoxesGroups, int i)
        {
            (_, int A, int B) = distancesList[i];
            var AGroup = junctionBoxesGroups.FirstOrDefault(x => x.Any(y => y == A));
            var BGroup = junctionBoxesGroups.FirstOrDefault(x => x.Any(y => y == B));

            if (AGroup == null && BGroup == null)
            {
                List<int> list = new List<int> { A, B };
                junctionBoxesGroups.Add(list);
            }
            else if (AGroup != null && BGroup == null)
            {
                AGroup.Add(B);
            }
            else if (AGroup == null && BGroup != null)
            {
                BGroup.Add(A);
            }
            else if (AGroup != BGroup)
            {
                AGroup.AddRange(BGroup);
                junctionBoxesGroups.Remove(BGroup);
            }
        }

        public void Task2()
        {
            List<List<int>> junctionBoxesGroups = new List<List<int>>();
            for (int j = 0; j != JunctionBoxesLocations.Count; j++)
            {
                junctionBoxesGroups.Add(new List<int> { j });
            }
            int i = -1;
            while (junctionBoxesGroups.Count > 1)
            {
                i++;
                ConnectNextClosestJunction(junctionBoxesGroups, i);
            }
            var junctionA = distancesList[i].junctionA;
            var junctionB = distancesList[i].junctionB;
            Console.WriteLine(JunctionBoxesLocations[junctionA].x * JunctionBoxesLocations[junctionB].x);
        }
    }
}