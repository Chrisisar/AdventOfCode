using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2024
{
    class Day08 : IDay
    {
        int maxH, maxW;
        bool[,] visited;
        List<(char freq, int i, int j)> antennas = new List<(char freq, int i, int j)>();

        public Day08(string inputFilePath)
        {
            ParseInput(inputFilePath);
        }

        private void ParseInput(string inputFilePath)
        {
            var lines = StaticHelpers.GetLines(inputFilePath);
            maxH = lines.Length;
            maxW = lines[0].Length;
            visited = new bool[maxH, maxW];
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    if (lines[i][j] != '.')
                    {
                        antennas.Add((lines[i][j], i, j));
                    }
                }
            }
        }

        public void Task1()
        {
            foreach (var antenna in antennas)
            {
                foreach (var connectedAntenna in antennas.Where(a => a.freq == antenna.freq && (a.i != antenna.i || a.j != antenna.j)))
                {
                    int difi = antenna.i - connectedAntenna.i;
                    int difj = antenna.j - connectedAntenna.j;
                    if (antenna.i + difi >= 0 && antenna.i + difi < maxH && antenna.j + difj >= 0 && antenna.j + difj < maxW)
                    {
                        visited[antenna.i + difi, antenna.j + difj] = true;
                    }
                }
            }
            var result = 0;
            for(int i =0; i < maxH; i++)
            {
                for(int j = 0;j < maxW; j++)
                {
                    result += visited[i, j] ? 1 : 0;
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            foreach (var antenna in antennas)
            {
                foreach (var connectedAntenna in antennas.Where(a => a.freq == antenna.freq && (a.i != antenna.i || a.j != antenna.j)))
                {
                    int difi = antenna.i - connectedAntenna.i;
                    int difj = antenna.j - connectedAntenna.j;
                    int currenti = antenna.i;
                    int currentj = antenna.j;
                    visited[currenti, currentj] = true;
                    while (currenti + difi >= 0 && currenti + difi < maxH && currentj + difj >= 0 && currentj + difj < maxW)
                    {
                        visited[currenti + difi, currentj + difj] = true;
                        currenti += difi;
                        currentj += difj;
                    }
                }
            }
            var result = 0;
            for (int i = 0; i < maxH; i++)
            {
                for (int j = 0; j < maxW; j++)
                {
                    result += visited[i, j] ? 1 : 0;
                }
            }
            Console.WriteLine(result);
        }
    }
}