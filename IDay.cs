using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    interface IDay
    {
        public void Task1();

        public void Task2();

        public string[] GetLines()
        {
            return StaticHelpers.GetLines(this.GetType());
        }
    }
}
