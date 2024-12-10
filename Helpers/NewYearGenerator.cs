using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Helpers
{
    internal class NewYearGenerator
    {
        public static void GenerateNextYear(int newYear)
        {
            DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent;
            DirectoryInfo newYearDirectory = new DirectoryInfo($"{directory.FullName}\\Year{newYear}");
            //if (!Directory.Exists($"{directory.FullName}\\Year{newYear}"))
            if (!newYearDirectory.Exists)
            {
                newYearDirectory.Create();
            }
            for (int i = 1; i <= 25; i++)
            {
                DirectoryInfo dayDirectory = new DirectoryInfo($"{newYearDirectory.FullName}\\Day{i.ToString("00")}");
                if (!dayDirectory.Exists)
                {
                    dayDirectory.Create();

                    using (var file = File.Create($"{dayDirectory.FullName}/Day{i.ToString("00")}.cs"))
                    {
                        string dataasstring = @$"using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year{newYear}
{{
    class Day{i.ToString("00")} : IDay
    {{
        public Day{i.ToString("00")}(string inputFilePath)
        {{
            ParseInput(inputFilePath);
        }}

        private void ParseInput(string inputFilePath)
        {{
            var lines = StaticHelpers.GetLines(inputFilePath);
        }}

        public void Task1()
        {{
            
        }}

        public void Task2()
        {{

        }}
    }}
}}";
                        byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
                        file.Write(info, 0, info.Length);

                    }
                    File.Create($"{dayDirectory.FullName}/ExampleInput.txt");
                    File.Create($"{dayDirectory.FullName}/MyInput.txt");
                }
            }
        }
    }
}
