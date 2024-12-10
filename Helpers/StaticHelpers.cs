﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Helpers
{
    public static class StaticHelpers
    {
        public static int GetIntFromBinary(int[] binary)
        {
            int result = 0;
            int powerOfTwo = 1;
            for (int i = 0; i != binary.Length; i++)
            {
                result += powerOfTwo * binary[binary.Length - 1 - i];
                powerOfTwo *= 2;
            }
            return result;
        }

        public static int GetIntFromStringBinary(string str)
        {
            int result = 0;
            int powerOfTwo = 1;
            for (int i = 0; i != str.Length; i++)
            {
                result += powerOfTwo * (str[str.Length - 1 - i] - '0');
                powerOfTwo *= 2;
            }
            return result;
        }

        public static long GetLongFromStringBinary(string str)
        {
            long result = 0;
            long powerOfTwo = 1;
            for (int i = 0; i != str.Length; i++)
            {
                result += powerOfTwo * (str[str.Length - 1 - i] - '0');
                powerOfTwo *= 2;
            }
            return result;
        }

        public static string BinToDec(string value)
        {
            // BigInteger can be found in the System.Numerics dll
            BigInteger res = 0;

            // I'm totally skipping error handling here
            foreach (char c in value)
            {
                res <<= 1;
                res += c == '1' ? 1 : 0;
            }

            return res.ToString();
        }

        public static string GetBinaryFromHex(string hex)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var character in hex)
            {
                switch(character)
                {
                    case '0':
                        sb.Append("0000");
                        break;
                    case '1':
                        sb.Append("0001");
                        break;
                    case '2':
                        sb.Append("0010");
                        break;
                    case '3':
                        sb.Append("0011");
                        break;
                    case '4':
                        sb.Append("0100");
                        break;
                    case '5':
                        sb.Append("0101");
                        break;
                    case '6':
                        sb.Append("0110");
                        break;
                    case '7':
                        sb.Append("0111");
                        break;
                    case '8':
                        sb.Append("1000");
                        break;
                    case '9':
                        sb.Append("1001");
                        break;
                    case 'A':
                        sb.Append("1010");
                        break;
                    case 'B':
                        sb.Append("1011");
                        break;
                    case 'C':
                        sb.Append("1100");
                        break;
                    case 'D':
                        sb.Append("1101");
                        break;
                    case 'E':
                        sb.Append("1110");
                        break;
                    case 'F':
                        sb.Append("1111");
                        break;
                }
            }
            return sb.ToString();
        }

        public static string[] GetLines(Type type)
        {
            return File.ReadAllLines($"../../../{type.Namespace.Split(".")[1]}/{type.Name}/{type.Name}Input.txt");
        }

        public static string[] GetAllTxtFiles(Type type)
        {
            return Directory.GetFiles($"../../../{type.Namespace.Split(".")[1]}/{type.Name}", "*.txt");
        }

        public static string[] GetLines(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}