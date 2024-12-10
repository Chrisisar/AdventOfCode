using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day22 : IDay
    {

        public Day22()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            foreach (var line in lines)
            {
                bool switchOn = line.StartsWith("on");
                var splitLine = line.Replace("on x=", "").Replace("off x=", "").Replace("y=", "").Replace("z=", "").Split(",");
                var splitX = splitLine[0].Split("..");
                int x1 = int.Parse(splitX[0]);
                int x2 = int.Parse(splitX[1]);
                var splitY = splitLine[1].Split("..");
                int y1 = int.Parse(splitY[0]);
                int y2 = int.Parse(splitY[1]);
                var splitZ = splitLine[2].Split("..");
                int z1 = int.Parse(splitZ[0]);
                int z2 = int.Parse(splitZ[1]);
                Cuboids.Add(new Cuboid(x1, x2, y1, y2, z1, z2, switchOn));
                //Console.WriteLine($"{x1}..{x2},{y1}..{y2},{z1}..{z2}");
            }
        }

        class Cuboid
        {
            public Cuboid(int x1, int x2, int y1, int y2, int z1, int z2, bool switchOn)
            {
                X1 = x1;
                X2 = x2;
                Y1 = y1;
                Y2 = y2;
                Z1 = z1;
                Z2 = z2;
                SwitchOn = switchOn;
            }

            public int X1, X2, Y1, Y2, Z1, Z2;
            public bool SwitchOn;

            public List<Cuboid> Split(Cuboid cuboid)
            {
                int newX1 = X1;
                int newX2 = X2;
                int newY1 = Y1;
                int newY2 = Y2;
                int newZ1 = Z1;
                int newZ2 = Z2;
                List<Cuboid> results = new List<Cuboid>();
                if (this.X1 < cuboid.X1) //left
                {
                    newX1 = cuboid.X1;
                    results.Add(new Cuboid(X1, newX1 - 1, newY1, newY2, newZ1, newZ2, true));
                }
                if (this.X2 > cuboid.X2) //right
                {
                    newX2 = cuboid.X2;
                    results.Add(new Cuboid(newX2 + 1, X2, newY1, newY2, newZ1, newZ2, true));
                }
                if (this.Y1 < cuboid.Y1) //top
                {
                    newY1 = cuboid.Y1;
                    results.Add(new Cuboid(newX1, newX2, Y1, newY1 - 1, newZ1, newZ2, true));
                }
                if (this.Y2 > cuboid.Y2) //bottom
                {
                    newY2 = cuboid.Y2;
                    results.Add(new Cuboid(newX1, newX2, newY2 + 1, Y2, newZ1, newZ2, true));
                }
                if (this.Z1 < cuboid.Z1) //front
                {
                    newZ1 = cuboid.Z1;
                    results.Add(new Cuboid(newX1, newX2, newY1, newY2, Z1, newZ1 - 1, true));
                }
                if (this.Z2 > cuboid.Z2) //back
                {
                    newZ2 = cuboid.Z2;
                    results.Add(new Cuboid(newX1, newX2, newY1, newY2, newZ2 + 1, Z2, true));
                }
                return results;
            }
        }

        List<Cuboid> Cuboids = new List<Cuboid>();

        public void Task1()
        {
            bool[,,] Lights = new bool[101, 101, 101];
            int offset = 50;
            foreach (var cuboid in Cuboids)
            {
                int x1 = cuboid.X1 < -50 ? -50 : cuboid.X1;
                int x2 = cuboid.X2 > 50 ? 50 : cuboid.X2;
                int y1 = cuboid.Y1 < -50 ? -50 : cuboid.Y1;
                int y2 = cuboid.Y2 > 50 ? 50 : cuboid.Y2;
                int z1 = cuboid.Z1 < -50 ? -50 : cuboid.Z1;
                int z2 = cuboid.Z2 > 50 ? 50 : cuboid.Z2;

                for (int x = x1; x <= x2; x++)
                {
                    for (int y = y1; y <= y2; y++)
                    {
                        for (int z = z1; z <= z2; z++)
                        {
                            Lights[x + offset, y + offset, z + offset] = cuboid.SwitchOn;
                        }
                    }
                }
            }

            var result = 0;

            for (int x = 0; x <= 100; x++)
            {
                for (int y = 0; y <= 100; y++)
                {
                    for (int z = 0; z <= 100; z++)
                    {
                        result += Lights[x, y, z] ? 1 : 0;
                    }
                }
            }
            Console.WriteLine(result);

        }

        private bool CuboidsOverlap(Cuboid existingCuboid, Cuboid cuboid)
        {
            bool xOverlaps = ((existingCuboid.X1 <= cuboid.X1 && existingCuboid.X2 >= cuboid.X1) || (existingCuboid.X1 <= cuboid.X2 && existingCuboid.X2 >= cuboid.X2))
                || ((cuboid.X1 <= existingCuboid.X1 && cuboid.X2 >= existingCuboid.X1) || (cuboid.X1 <= existingCuboid.X2 && cuboid.X2 >= existingCuboid.X2));

            bool yOverlaps = ((existingCuboid.Y1 <= cuboid.Y1 && existingCuboid.Y2 >= cuboid.Y1) || (existingCuboid.Y1 <= cuboid.Y2 && existingCuboid.Y2 >= cuboid.Y2))
                || ((cuboid.Y1 <= existingCuboid.Y1 && cuboid.Y2 >= existingCuboid.Y1) || (cuboid.Y1 <= existingCuboid.Y2 && cuboid.Y2 >= existingCuboid.Y2));

            bool zOverlaps = ((existingCuboid.Z1 <= cuboid.Z1 && existingCuboid.Z2 >= cuboid.Z1) || (existingCuboid.Z1 <= cuboid.Z2 && existingCuboid.Z2 >= cuboid.Z2))
                || ((cuboid.Z1 <= existingCuboid.Z1 && cuboid.Z2 >= existingCuboid.Z1) || (cuboid.Z1 <= existingCuboid.Z2 && cuboid.Z2 >= existingCuboid.Z2));

            return xOverlaps && yOverlaps && zOverlaps;
        }

        private void SplitExistingCuboids(Cuboid cuboid)
        {
            var ExistindCuboidsCopy = new List<Cuboid>(ExistingCuboids);
            foreach (var existingCuboid in ExistindCuboidsCopy)
            {
                if (CuboidsOverlap(existingCuboid, cuboid))
                {
                    ExistingCuboids.Remove(existingCuboid);
                    var splitCuboids = existingCuboid.Split(cuboid);
                    ExistingCuboids.AddRange(splitCuboids);
                }
            }
        }

        List<Cuboid> ExistingCuboids = new List<Cuboid>();

        public void Task2()
        {
            foreach (var cuboid in Cuboids)
            {
                SplitExistingCuboids(cuboid);
                if (cuboid.SwitchOn)
                { 
                    ExistingCuboids.Add(cuboid);
                }
            }
            long result = 0;
            foreach (var cuboid in ExistingCuboids)
            {
                result += (long)(Math.Abs(cuboid.X2 - cuboid.X1) + 1) * (Math.Abs(cuboid.Y2 - cuboid.Y1) + 1) * (Math.Abs(cuboid.Z2 - cuboid.Z1) + 1);
            }
            Console.WriteLine(result);
        }
    }
}
