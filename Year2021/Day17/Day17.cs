using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day17 : IDay
    {
        int MinX = 265;
        int MaxX = 287;
        int MinY = -103;
        int MaxY = -58;


        public void Task1()
        {
            //x velocity calculated by x*(x+1)/2 in range of x
            int xVelocity = 23;
            int yVelocity = MinY * (MinY + 1) / 2;
            Console.WriteLine(yVelocity);
        }

        public void Task2()
        {
            int result = 0;
            for (int i = 23; i <= MaxX; i++)
            {
                for (int j = MinY; j <= 5253; j++)
                {
                    if (SimulateThrow(i, j))
                    {
                        result++;
                    }
                }
            }
            Console.WriteLine(result);
        }

        bool SimulateThrow(int velX, int velY)
        {
            int posX = 0; int posY = 0;
            while (posX <= MaxX && posY >= MinY)
            {
                if (posX >= MinX && posX <= MaxX && posY >= MinY && posY <= MaxY)
                {
                    return true;
                }
                posX += velX;
                if (velX > 0)
                {
                    velX--;
                }
                posY += velY;
                velY--;
            }
            return false;
        }
    }
}
