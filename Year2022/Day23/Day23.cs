using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day23 : IDay
    {
        class Elf
        {
            public Point Position { get; set; }
            public bool WillMove { get; set; }
            public Point NewPosition { get; set; }
        }

        List<Elf> Elfs = new List<Elf>();

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            for (int y = 0; y != lines.Count(); y++)
            {
                for (int x = 0; x != lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        Elfs.Add(new Elf
                        {
                            Position = new Point(x, y)
                        });
                    }
                }
            }

            for (int round = 1; round <= 10; round++)
            {
                Elfs.ForEach(e => e.WillMove = true);
                MarkMoves(round);
                foreach(var elf in Elfs)
                {
                    if(!elf.WillMove)
                    {
                        continue;
                    }
                    foreach(var elf2 in Elfs.Where(e=>e != elf))
                    {
                        if(elf.NewPosition == elf2.NewPosition)
                        {
                            elf.WillMove = false;
                            elf2.WillMove = false;
                        }
                    }
                }
                foreach(var elf in Elfs)
                {
                    if(elf.WillMove)
                    {
                        elf.Position = elf.NewPosition;
                    }
                    else
                    {
                        elf.NewPosition = elf.Position;
                    }
                }
                //DrawMap(round);
            }

            int x1 = Elfs.Min(e => e.Position.X);
            int x2 = Elfs.Max(e => e.Position.X);
            int y1 = Elfs.Min(e => e.Position.Y);
            int y2 = Elfs.Max(e => e.Position.Y);

            Console.WriteLine((x2 - x1 + 1) * (y2 - y1 + 1) - Elfs.Count);
        }

        void DrawMap(int round)
        {
            Console.WriteLine("=============");
            Console.WriteLine($"Round {round}");
            Console.WriteLine("=============");
            int x1 = Elfs.Min(e => e.Position.X);
            int x2 = Elfs.Max(e => e.Position.X);
            int y1 = Elfs.Min(e => e.Position.Y);
            int y2 = Elfs.Max(e => e.Position.Y);
            for(int y = y1; y<=y2;y++)
            {
                for(int x = x1; x<=x2;x++)
                {
                    Console.Write(Elfs.Any(e => e.Position.X == x && e.Position.Y == y) ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

        void MarkMoves(int round) // 1 - N, 2 - S, 3 - W, 0 - E
        {
            foreach (var elf in Elfs)
            {
                if(!Elfs.Any(e=> e!= elf 
                    && (e.Position.X == elf.Position.X 
                        || e.Position.X == elf.Position.X +1 
                        || e.Position.X == elf.Position.X - 1)
                    && (e.Position.Y == elf.Position.Y 
                        || e.Position.Y == elf.Position.Y + 1 
                        || e.Position.Y == elf.Position.Y - 1)))
                {
                    elf.WillMove = false;
                    continue;
                }
                int direction = round % 4;
                do
                {
                    if (direction == 1 && !Elfs.Any(e=>e.Position.Y == elf.Position.Y - 1 && (e.Position.X == elf.Position.X || e.Position.X == elf.Position.X - 1|| e.Position.X == elf.Position.X + 1)))
                    {
                        elf.WillMove = true;
                        elf.NewPosition = new Point(elf.Position.X, elf.Position.Y - 1);
                        break;
                    }
                    else if (direction == 2 && !Elfs.Any(e=>e.Position.Y == elf.Position.Y + 1 && (e.Position.X == elf.Position.X || e.Position.X == elf.Position.X - 1|| e.Position.X == elf.Position.X + 1)))
                    {
                        elf.WillMove = true;
                        elf.NewPosition = new Point(elf.Position.X, elf.Position.Y + 1);
                        break;
                    }
                    else if(direction == 3 && !Elfs.Any(e=>e.Position.X == elf.Position.X - 1 && (e.Position.Y == elf.Position.Y || e.Position.Y == elf.Position.Y - 1|| e.Position.Y == elf.Position.Y + 1)))
                    {
                        elf.WillMove = true;
                        elf.NewPosition = new Point(elf.Position.X - 1, elf.Position.Y);
                        break;
                    }
                    else if(direction == 0 && !Elfs.Any(e=>e.Position.X == elf.Position.X + 1 && (e.Position.Y == elf.Position.Y || e.Position.Y == elf.Position.Y - 1|| e.Position.Y == elf.Position.Y + 1)))
                    {
                        elf.WillMove = true;
                        elf.NewPosition = new Point(elf.Position.X + 1, elf.Position.Y);
                        break;
                    }
                    direction = (direction + 1) % 4;
                } while (direction != round % 4);
            }
        }

        public void Task2() //955, 993 too low
        {
            int round = 10;
            while(Elfs.Any(e=>e.WillMove))
            {
                round++;
                Elfs.ForEach(e => e.WillMove = true);
                MarkMoves(round);
                foreach (var elf in Elfs)
                {
                    if (!elf.WillMove)
                    {
                        continue;
                    }
                    foreach (var elf2 in Elfs.Where(e => e != elf))
                    {
                        if (elf.NewPosition == elf2.NewPosition)
                        {
                            elf.WillMove = false;
                            elf2.WillMove = false;
                        }
                    }
                }
                foreach (var elf in Elfs)
                {
                    if (elf.WillMove)
                    {
                        elf.Position = elf.NewPosition;
                    }
                    else
                    {
                        elf.NewPosition = elf.Position;
                    }
                }
                //DrawMap(round);
            }
            Console.WriteLine(round);
        }
    }
}
