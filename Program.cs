using AdventOfCode.Helpers;
using AdventOfCode.Year2024;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //NewYearGenerator.GenerateNextYear(2024);
            Type dayType = typeof(Day11);



            DateTime startDate = DateTime.Now;
            var files = StaticHelpers.GetAllTxtFiles(dayType);
            foreach (var file in files)
            {
                try
                {
                    Console.WriteLine($"Running solution for ***{file.Substring(file.LastIndexOf('\\') + 1)}***");
                    IDay day = Activator.CreateInstance(dayType, file) as IDay;
                    Console.WriteLine($"Answer to task 1 is:");
                    day.Task1();
                    Console.WriteLine($"It took {DateTime.Now.Subtract(startDate).TotalMilliseconds}ms.");
                    startDate = DateTime.Now;
                    Console.WriteLine("Answer to task 2 is:");
                    day.Task2();
                    Console.WriteLine($"It took {DateTime.Now.Subtract(startDate).TotalMilliseconds}ms.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
