using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day12 : IDay
    {
        GardenTile[,] garden;
        bool[,] visited;
        int maxH, maxW;

        public Day12(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            maxH = lines.Length;
            maxW = lines[0].Length;
            garden = new GardenTile[maxH, maxW];
            visited = new bool[maxH, maxW];
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    garden[i, j] = new GardenTile { PlantType = lines[i][j] };
                }
            }
        }

        public void Task1()
        {
            long result = 0;
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    (int area, int perimeter, int fences) measures = DFS(i, j, garden[i, j].PlantType);
                    result += measures.area * measures.perimeter;
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            visited = new bool[maxH, maxW];

            long result = 0;
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    (int area, int perimeter, int fences) measures = DFS(i, j, garden[i, j].PlantType);
                    result += measures.area * measures.fences;
                }
            }
            Console.WriteLine(result);
        }

        (int area, int perimeter, int fences) DFS(int i, int j, char plantType)
        {
            int area = 0;
            int perimeter = 0;
            int fences = 0;

            if (visited[i, j]) return (area, perimeter, fences);

            visited[i, j] = true;
            area = 1;

            if (i > 0 && garden[i - 1, j].PlantType == plantType) //UpTile
            {
                (int a, int p, int f) result = DFS(i - 1, j, plantType);
                area += result.a;
                perimeter += result.p;
                fences += result.f;
            }
            else
            {
                garden[i, j].HasTopFence = true;
                perimeter++;
                if (j == 0 || garden[i, j - 1].PlantType != plantType || (garden[i, j - 1].PlantType == plantType && !garden[i, j - 1].HasTopFence))
                {
                    fences++;
                }
            }

            if (i < maxH - 1 && garden[i + 1, j].PlantType == plantType) //DownTile
            {
                (int a, int p, int f) result = DFS(i + 1, j, plantType);
                area += result.a;
                perimeter += result.p;
                fences += result.f;
            }
            else
            {
                garden[i, j].HasBottomFence = true;
                perimeter++;
                if (j == 0 || garden[i, j - 1].PlantType != plantType || (garden[i, j - 1].PlantType == plantType && !garden[i, j - 1].HasBottomFence))
                {
                    fences++;
                }
            }

            if (j > 0 && garden[i, j - 1].PlantType == plantType) //LeftTile
            {
                (int a, int p, int f) result = DFS(i, j - 1, plantType);
                area += result.a;
                perimeter += result.p;
                fences += result.f;
            }
            else
            {
                garden[i, j].HasLeftFence = true;
                perimeter++;
                if (i == 0 || garden[i - 1, j].PlantType != plantType || (garden[i - 1, j].PlantType == plantType && !garden[i - 1, j].HasLeftFence))
                {
                    fences++;
                }
            }

            if (j < maxW - 1 && garden[i, j + 1].PlantType == plantType) //RightTile
            {
                (int a, int p, int f) result = DFS(i, j + 1, plantType);
                area += result.a;
                perimeter += result.p;
                fences += result.f;
            }
            else
            {
                garden[i, j].HasRightFence = true;
                perimeter++;
                if (i == 0 || garden[i - 1, j].PlantType != plantType || (garden[i - 1, j].PlantType == plantType && !garden[i - 1, j].HasRightFence))
                {
                    fences++;
                }
            }

            return (area, perimeter, fences);
        }

        class GardenTile
        {
            public char PlantType;
            public bool HasLeftFence;
            public bool HasRightFence;
            public bool HasTopFence;
            public bool HasBottomFence;
        }
    }
}