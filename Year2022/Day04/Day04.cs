using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day04 : IDay
    {
        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            int result = 0;
            foreach(var line in lines)
            {
                var elfs = line.Split(',');
                var elf1 = elfs[0];
                var elf2 = elfs[1];

                var elf1Numbers = new int[] { int.Parse(elf1.Split('-')[0]), int.Parse(elf1.Split('-')[1]) };
                var elf2Numbers = new int[] { int.Parse(elf2.Split('-')[0]), int.Parse(elf2.Split('-')[1]) };

                if (elf1Numbers[0] <= elf2Numbers[0] && elf1Numbers[1] >= elf2Numbers[1]
                    || elf1Numbers[0] >= elf2Numbers[0] && elf1Numbers[1] <= elf2Numbers[1])
                {
                    result++;
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            int result = 0;
            foreach (var line in lines)
            {
                var elfs = line.Split(',');
                var elf1 = elfs[0];
                var elf2 = elfs[1];

                var elf1Numbers = new int[] { int.Parse(elf1.Split('-')[0]), int.Parse(elf1.Split('-')[1]) };
                var elf2Numbers = new int[] { int.Parse(elf2.Split('-')[0]), int.Parse(elf2.Split('-')[1]) };

                if (elf1Numbers[0] <= elf2Numbers[0] && elf1Numbers[1] >= elf2Numbers[0]
                    || elf1Numbers[0] <= elf2Numbers[1] && elf1Numbers[1] >= elf2Numbers[1]
                    || elf2Numbers[0] <= elf1Numbers[0] && elf2Numbers[1] >= elf1Numbers[0]
                    || elf2Numbers[0] <= elf1Numbers[1] && elf2Numbers[1] >= elf1Numbers[1])
                {
                    result++;
                }
            }
            Console.WriteLine(result);
        }
    }
}
