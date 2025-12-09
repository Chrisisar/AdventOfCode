using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2025
{
    class Day09 : IDay
    {
        List<(long x, long y)> redTiles = new List<(long x, long y)>();
        List<(long area, int tileA, int tileB)> areas = new List<(long area, int tileA, int tileB)>();
        List<(long x1, long y1, long x2, long y2)> vectors = new List<(long x1, long y1, long x2, long y2)>();

        public Day09(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            (long x, long y) previousTile = default;
            for (int i = 0; i != lines.Length; i++)
            {
                var splitLine = lines[i].Split(',');
                int.TryParse(splitLine[0], out int x);
                int.TryParse(splitLine[1], out int y);
                redTiles.Add((x, y));

                if (i > 0)
                {
                    vectors.Add((previousTile.x, previousTile.y, x, y));
                }
                previousTile = (x, y);
            }
            vectors.Add((previousTile.x, previousTile.y, redTiles[0].x, redTiles[0].y));
        }

        void CalculateDistances()
        {
            for (int i = 0; i < redTiles.Count; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (i == j)
                        continue;

                    var A = redTiles[i];
                    var B = redTiles[j];

                    long area = (Math.Abs(A.x - B.x) + 1) * (Math.Abs(A.y - B.y) + 1);

                    areas.Add((area, i, j));
                }
            }
            areas = areas.OrderByDescending(x => x.area).ToList();
        }

        public void Task1()
        {
            CalculateDistances();
            Console.WriteLine(areas[0].area);
        }

        public void Task2()
        {
            for (int i = 0; i < areas.Count; i++)
            {
                bool isGood = true;
                var tileA = redTiles[areas[i].tileA];
                var tileB = redTiles[areas[i].tileB];

                for (int j = 0; j != vectors.Count; j++)
                {
                    var x1 = vectors[j].x1;
                    var x2 = vectors[j].x2;
                    var y1 = vectors[j].y1;
                    var y2 = vectors[j].y2;

                    if(!(tileA.x <= x1 && tileA.x <= x2 && tileB.x <= x1 && tileB.x <= x2)
                        && !(tileA.x >= x1 && tileA.x >= x2 && tileB.x >= x1 && tileB.x >= x2)
                        && !(tileA.y <= y1 && tileA.y <= y2 && tileB.y <= y1 && tileB.y <= y2)
                        && !(tileA.y >= y1 && tileA.y >= y2 && tileB.y >= y1 && tileB.y >= y2))
                    {
                        isGood = false;
                        break;
                    }
                }
                if (isGood)
                {
                    Console.WriteLine(areas[i].area);
                    return;
                }
            }
        }
    }
}