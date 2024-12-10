using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day25 : IDay
    {
        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            long result = 0;
            long power = 1;
            foreach (var line in lines)
            {
                long lineResult = 0;
                power = 1;
                for (int i = line.Length - 1; i >= 0; i--)
                {
                    switch (line[i])
                    {
                        case '0':
                            break;
                        case '1':
                            lineResult += power;
                            break;
                        case '2':
                            lineResult += power * 2;
                            break;
                        case '-':
                            lineResult -= power;
                            break;
                        case '=':
                            lineResult -= power * 2;
                            break;
                    }
                    power *= 5;
                }
                result += lineResult;
            }
            Console.WriteLine(result); //30535047052797
            power = 1;
            while (power <= result)
            {
                power *= 5;
            }
            StringBuilder sb = new StringBuilder();
            while (result > 0)
            {
                var divisionResult = result / power;
                result %= power;
                sb.Append(divisionResult);
                power /= 5;
            }
            Console.WriteLine(sb.ToString()); //1009635086997439529

            char[] base5ResultString = sb.ToString().ToCharArray();
            string task1Answer = "";

            for (int i = base5ResultString.Length - 1; i >= 0; i--)
            {
                switch(base5ResultString[i])
                {
                    case '0':
                    case '1':
                    case '2':
                        task1Answer = base5ResultString[i] + task1Answer;
                        break;
                    case '3':
                        task1Answer = "=" + task1Answer;
                        if(i == 0)
                        {
                            task1Answer = "1" + task1Answer;
                        }
                        else
                        {
                            base5ResultString[i - 1]++;
                        }
                        break;
                    case '4':
                        task1Answer = "-" + task1Answer;
                        if (i == 0)
                        {
                            task1Answer = "1" + task1Answer;
                        }
                        else
                        {
                            base5ResultString[i - 1]++;
                        }
                        break;
                    case '5':
                        task1Answer = "0" + task1Answer;
                        if (i == 0)
                        {
                            task1Answer = "1" + task1Answer;
                        }
                        else
                        {
                            base5ResultString[i - 1]++;
                        }
                        break;
                }
            }
            Console.WriteLine(task1Answer);

        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType()).ToList();

        }
    }
}
