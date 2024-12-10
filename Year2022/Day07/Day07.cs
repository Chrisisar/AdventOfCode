using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day07 : IDay
    {
        class File
        {
            public File(string name, long size)
            {
                Name = name;
                Size = size;
            }

            public long Size { get; set; }
            public string Name { get; set; }
        }

        class Directory
        {
            public Directory(string name, Directory parent)
            {
                Name = name;
                Files = new List<File>();
                Directories = new List<Directory>();
                Parent = parent;
            }

            public string Name { get; set; }
            public List<File> Files { get; set; }
            public List<Directory> Directories { get; set; }
            public Directory Parent { get; set; }
            public long Size
            {
                get
                {
                    return Files.Sum(x => x.Size) + Directories.Sum(x => x.Size);
                }
            }
        }

        private Directory ParseInput(string[] input)
        {
            Directory root = new Directory("/", null);

            Directory scope = root;
            foreach (var line in input)
            {
                if (line[0] == '$')
                {
                    if (line[2] == 'c') //cd
                    {
                        var directoryName = line.Substring(5);
                        if (directoryName == "/")
                        {
                            scope = root;
                        }
                        else if (directoryName == "..")
                        {
                            scope = scope.Parent;
                        }
                        else
                        {
                            var existingDirectory = scope.Directories.SingleOrDefault(x => x.Name == directoryName);
                            if (existingDirectory != null)
                            {
                                scope = existingDirectory;
                            }
                            else
                            {
                                var newDirectory = new Directory(directoryName, scope);
                                scope.Directories.Add(newDirectory);
                                scope = newDirectory;
                            }
                        }
                    }
                    else //ls
                    {
                        //Tu w sumie nic się nie dzieje, lecimy następne linie aż trafimy na nową komendę
                    }
                }
                else
                {
                    var splitLine = line.Split(' ');
                    if (splitLine[0] == "dir")
                    {
                        var newDirectory = new Directory(splitLine[1], scope);
                        scope.Directories.Add(newDirectory);
                    }
                    else if (int.TryParse(splitLine[0], out int fileSize))
                    {
                        scope.Files.Add(new File(splitLine[1], fileSize));
                    }
                }
            }
            return root;
        }

        private long GetSmallDirectorySizeSum(Directory directory)
        {
            long sizeSum = 0;
            if(directory.Size <= 100000)
            {
                sizeSum = directory.Size;
            }
            foreach(var dir in directory.Directories)
            {
                sizeSum += GetSmallDirectorySizeSum(dir);
            }
            return sizeSum;
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            var root = ParseInput(lines);
            Console.WriteLine(GetSmallDirectorySizeSum(root));
        }

        private long FindSmallestSizeBiggerThan(long minimumSize, Directory directory)
        {
            var minimumFound = directory.Size;
            foreach (var dir in directory.Directories)
            {
                if(dir.Size >= minimumSize)
                {
                    var dirMinimum = FindSmallestSizeBiggerThan(minimumSize, dir);
                    if(dirMinimum < minimumFound)
                    {
                        minimumFound = dirMinimum;
                    }
                }
            }
            return minimumFound;
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            var root = ParseInput(lines);
            var emptySpace = 70000000 - root.Size;
            var minimumSpaceToRemove = 30000000 - emptySpace;
            Console.WriteLine(FindSmallestSizeBiggerThan(minimumSpaceToRemove, root));

        }
    }
}
