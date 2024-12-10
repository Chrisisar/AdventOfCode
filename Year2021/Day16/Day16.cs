using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day16 : IDay
    {
        int Pointer = 0;
        string Binary;
        int SumOfVersions = 0;

        long GetPacket()
        {
            var version = Binary.Substring(Pointer, 3);
            int versionNumber = StaticHelpers.GetIntFromStringBinary(version);
            SumOfVersions += versionNumber;
            Pointer += 3;
            var type = Binary.Substring(Pointer, 3);
            int typeNumber = StaticHelpers.GetIntFromStringBinary(type);
            Pointer += 3;
            if (typeNumber == 4) //literal value
            {
                long literalValue = GetLiteralValue();
                //Console.WriteLine(literalValue);
                return literalValue;
            }
            else
            {
                List<long> internalPackets = new List<long>();
                var lengthType = Binary[Pointer];
                Pointer++;
                if (lengthType == '0')
                {
                    int subPacketLength = StaticHelpers.GetIntFromStringBinary(Binary.Substring(Pointer, 15));
                    //Console.WriteLine("LENGTH TYPE 0 length: " + subPacketLength);
                    Pointer += 15;
                    int supposedPointer = Pointer + subPacketLength;
                    while (Pointer != supposedPointer)
                    {
                        internalPackets.Add(GetPacket());
                    }
                }
                else // LengthType == '1'
                {
                    int numberOfSubPackets = StaticHelpers.GetIntFromStringBinary(Binary.Substring(Pointer, 11));
                    //Console.WriteLine("LENGTH TYPE 1 number: " + numberOfSubPackets);
                    Pointer += 11;
                    for (int i = 0; i != numberOfSubPackets; i++)
                    {
                        internalPackets.Add(GetPacket());
                    }
                }
                switch(typeNumber)
                {
                    case 0:
                        return internalPackets.Sum(x => x);
                    case 1:
                        long product = 1;
                        internalPackets.ForEach(x => product *= x);
                        return product;
                    case 2:
                        return internalPackets.Min();
                    case 3:
                        return internalPackets.Max();
                    case 5:
                        return internalPackets[0] > internalPackets[1] ? 1 : 0;
                    case 6:
                        return internalPackets[0] < internalPackets[1] ? 1 : 0;
                    case 7:
                        return internalPackets[0] == internalPackets[1] ? 1 : 0;
                    default:
                        return 0;
                }
            }
        }

        private long GetLiteralValue()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                var segment = Binary.Substring(Pointer, 5);
                sb.Append(segment.Substring(1, 4));
                Pointer += 5;
                if (segment[0] == '0')
                {
                    break;
                }
            }
            return StaticHelpers.GetLongFromStringBinary(sb.ToString());
        }

        private string GetLiteralValueString()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                var segment = Binary.Substring(Pointer, 5);
                sb.Append(segment.Substring(1, 4));
                Pointer += 5;
                if (segment[0] == '0')
                {
                    break;
                }
            }
            return sb.ToString();
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            Binary = StaticHelpers.GetBinaryFromHex(lines[0]);
            //Console.WriteLine(Binary);
            GetPacket();
            Console.WriteLine(SumOfVersions);
        }

        public void Task2()
        {
            Pointer = 0;
            var result = GetPacket();
            Console.WriteLine(result);
            //Console.WriteLine(StaticHelpers.BinToDec("10110000000111100100011011110101101001011010"));
        }
    }
}
