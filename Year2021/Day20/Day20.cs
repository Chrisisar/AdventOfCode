using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day20 : IDay
    {
        public Day20()
        {
            PopulateArrays();
        }

        bool[] Decipher = new bool[512];
        bool[,] InputPicture;

        void PopulateArrays()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            for (int i = 0; i != lines[0].Length; i++)
            {
                Decipher[i] = lines[0][i] == '#';
            }

            InputPicture = new bool[lines.Length - 2, lines[2].Length];
            for (int i = 0; i != lines.Length - 2; i++)
            {
                for (int j = 0; j != lines[i + 2].Length; j++)
                {
                    InputPicture[i, j] = lines[i + 2][j] == '#';
                }
            }
        }

        bool WhatsOutside = false;

        bool ConvertPixel(int x, int y)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i < 0 || j < 0 || i > InputPicture.GetLength(0) - 1 || j > InputPicture.GetLength(1) - 1)
                    {
                        sb.Append(WhatsOutside ? "1" : "0");
                    }
                    else
                    {
                        sb.Append(InputPicture[i, j] ? "1" : "0");
                    }
                }
            }
            //Console.WriteLine(sb.ToString());
            int pixelCode = StaticHelpers.GetIntFromStringBinary(sb.ToString());
            return Decipher[pixelCode];
        }

        void ConvertImage()
        {
            bool[,] OutputPicture = new bool[InputPicture.GetLength(0) + 20, InputPicture.GetLength(1) + 20];
            for (int i = 0; i != InputPicture.GetLength(0) + 20; i++)
            {
                for (int j = 0; j != InputPicture.GetLength(1) + 20; j++)
                {
                    OutputPicture[i, j] = ConvertPixel(i - 19, j - 19);
                }
            }
            InputPicture = OutputPicture;
            WhatsOutside = !WhatsOutside;
        }

        int HowManyLit()
        {
            int result = 0;
            for (int i = 0; i != InputPicture.GetLength(0); i++)
            {
                for (int j = 0; j != InputPicture.GetLength(1); j++)
                {
                    if (InputPicture[i, j])
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        void OutputPicture()
        {
            for (int i = 0; i != InputPicture.GetLength(0); i++)
            {
                for (int j = 0; j != InputPicture.GetLength(1); j++)
                {
                    Console.Write(InputPicture[i, j] ? "#" : ".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void Task1()
        {
            //OutputPicture();
            ConvertImage();
            //OutputPicture();
            ConvertImage();
            //OutputPicture();
            Console.WriteLine(HowManyLit());
        }

        public void Task2()
        {
            for(int i = 0; i!=48;i++)
            {
                ConvertImage();
            }
            Console.WriteLine(HowManyLit());
        }
    }
}
