using AdventOfCode.Helpers;
using AdventOfCode.Year2025;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //NewYearGenerator.GenerateNextYear(2025);
            Type dayType = typeof(Day08);


            var files = StaticHelpers.GetAllTxtFiles(dayType);
            foreach (var file in files)
            {
                DateTime startDate = DateTime.Now;
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
        }
    }
}
