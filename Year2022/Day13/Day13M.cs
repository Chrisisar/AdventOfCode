using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    public record Packet : PacketList
    {
        private Packet(IEnumerable<PacketComponent> content) : base(content)
        {

        }

        public static Packet Parse(string input)
        {
            var pos = 0;
            return new Packet(PacketList.Parse(input, ref pos).Content);
        }

        public bool IsDividerKey()
        {
            var str = ToString();
            return str is "[[6]]" or "[[2]]";
        }
    }

    public abstract record PacketComponent
        : IComparable<PacketComponent>, IComparable<PacketInteger>, IComparable<PacketList>
    {
        public int CompareTo(PacketComponent? other)
        {
            return other switch
            {
                PacketInteger integer => CompareTo(integer),
                PacketList list => CompareTo(list),
                _ => throw new InvalidOperationException()
            };
        }

        public abstract int CompareTo(PacketInteger? other);

        public abstract int CompareTo(PacketList? other);
    }

    public record PacketInteger(int Content) : PacketComponent
    {
        public static PacketInteger Parse(string input, ref int pos)
        {
            var builder = new StringBuilder();
            while (pos < input.Length)
            {
                if (char.IsDigit(input[pos]))
                {
                    builder.Append(input[pos]);
                    pos++;
                }
                else
                {
                    return new PacketInteger(int.Parse(builder.ToString()));
                }
            }

            throw new InvalidOperationException();
        }

        public override int CompareTo(PacketInteger? other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return Content.CompareTo(other.Content);
        }

        public override int CompareTo(PacketList? other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return -1 * other.CompareTo(this);
        }

        public override string ToString()
        {
            return Content.ToString();
        }
    }

    public record PacketList : PacketComponent
    {
        public PacketList(IEnumerable<PacketComponent> content)
        {
            _content = content.ToArray();
        }

        private readonly PacketComponent[] _content;
        public IEnumerable<PacketComponent> Content => Array.AsReadOnly(_content);

        protected static PacketList Parse(string input, ref int pos)
        {
            pos++; //skip over the first '['
            var contents = new List<PacketComponent>();

            while (pos < input.Length)
            {
                if (input[pos] == '[')
                {
                    contents.Add(Parse(input, ref pos));
                    pos++; //move over the last ']'
                }
                else if (input[pos] == ']')
                {
                    break;
                }
                else if (char.IsDigit(input[pos]))
                {
                    contents.Add(PacketInteger.Parse(input, ref pos));
                }
                else if (input[pos] == ',')
                {
                    pos++;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return new PacketList(contents);
        }

        public override int CompareTo(PacketInteger? other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return CompareTo(new PacketList(new[] { other }));
        }

        public override int CompareTo(PacketList? other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            for (var i = 0; i < Math.Max(_content.Length, other._content.Length); i++)
            {
                if (i >= _content.Length)
                {
                    return -1;
                }

                if (i >= other._content.Length)
                {
                    return 1;
                }

                var diff = _content[i].CompareTo(other._content[i]);

                if (diff != 0)
                {
                    return diff;
                }
            }

            return 0;
        }

        public override string ToString()
        {
            return $"[{string.Join(',', Content.Select(x => x.ToString()))}]";
        }
    }

    class Day13M : IDay
    {
        public void Task1()
        {
            File.ReadAllText("../../../Year2022/Day13/Day13Input.txt")
            .Trim()
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(pair => pair
                .Split(Environment.NewLine)
                .Select(Packet.Parse)
                .ToArray()
            )
            .Select((pair, i) => (i: i + 1, pair))
            .ToList()
            .ForEach(t => Console.WriteLine(t.pair[0].CompareTo(t.pair[1]) == -1));

            //Console.WriteLine($"Puzzle 1: {sumOfIndices}");

            var decoderKey = File.ReadAllLines("../../../Year2022/Day13/Day13Input.txt")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Prepend("[[6]]")
                .Prepend("[[2]]")
                .Select(Packet.Parse)
                .OrderBy(static e => e)
                .Select((packet, i) => (packet, i: i + 1))
                .Where(t => t.packet.IsDividerKey())
                .Select(t => t.i)
                .Aggregate((a, b) => a * b);

            Console.WriteLine($"Puzzle 2: {decoderKey}");
        }
    

        public void Task2()
        {
            //throw new NotImplementedException();
        }
    }
}