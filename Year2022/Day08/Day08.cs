using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day08 : IDay
    {
        class Tree
        {
            public Tree(int height)
            {
                Height = height;
                HeightToBeVisibleFromTop = 10;
                HeightToBeVisibleFromLeft = 10;
                HeightToBeVisibleFromRight = 10;
                HeightToBeVisibleFromBottom = 10;
            }

            public int Height { get; set; }
            public int HeightToBeVisibleFromTop { get; set; }
            public int HeightToBeVisibleFromLeft { get; set; }
            public int HeightToBeVisibleFromRight { get; set; }
            public int HeightToBeVisibleFromBottom { get; set; }
            public bool IsVisible
            {
                get
                {
                    return Height >= HeightToBeVisibleFromTop
                        || Height >= HeightToBeVisibleFromLeft
                        || Height >= HeightToBeVisibleFromRight
                        || Height >= HeightToBeVisibleFromBottom;
                }
            }
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            var noOfRows = lines.Count();
            var noOfColumns = lines[0].Length;
            Tree[,] trees = new Tree[noOfRows, noOfColumns];
            for (int i = 0; i != noOfRows; i++)
            {
                for (int j = 0; j != noOfColumns; j++)
                {
                    Tree newTree = new Tree(int.Parse(lines[i][j].ToString()));
                    newTree.HeightToBeVisibleFromTop = i == 0
                        ? 0
                        : trees[i - 1, j].Height >= trees[i - 1, j].HeightToBeVisibleFromTop
                            ? trees[i - 1, j].Height + 1
                            : trees[i - 1, j].HeightToBeVisibleFromTop;
                    newTree.HeightToBeVisibleFromLeft = j == 0
                        ? 0
                        : trees[i, j-1].Height >= trees[i, j - 1].HeightToBeVisibleFromLeft
                            ? trees[i, j-1].Height + 1
                            : trees[i, j-1].HeightToBeVisibleFromLeft;                    

                    trees[i, j] = newTree;
                }
            }

            int sumOfVisibles = 0;

            for (int i = noOfRows - 1; i >= 0; i--)
            {
                for (int j = noOfColumns -1; j >= 0; j--)
                {
                    trees[i, j].HeightToBeVisibleFromBottom = i == noOfRows - 1
                        ? 0
                        : trees[i + 1, j].Height >= trees[i + 1, j].HeightToBeVisibleFromBottom
                            ? trees[i + 1, j].Height + 1
                            : trees[i + 1, j].HeightToBeVisibleFromBottom;
                    trees[i, j].HeightToBeVisibleFromRight = j == noOfColumns - 1
                        ? 0
                        : trees[i, j + 1].Height >= trees[i, j + 1].HeightToBeVisibleFromRight
                            ? trees[i, j + 1].Height + 1
                            : trees[i, j + 1].HeightToBeVisibleFromRight;

                    if(trees[i,j].IsVisible)
                    {
                        sumOfVisibles++;
                    }
                }
            }
            Console.WriteLine(sumOfVisibles);
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            var noOfRows = lines.Count();
            var noOfColumns = lines[0].Length;
            Tree[,] trees = new Tree[noOfRows, noOfColumns];
            for (int i = 0; i != noOfRows; i++)
            {
                for (int j = 0; j != noOfColumns; j++)
                {
                    Tree newTree = new Tree(int.Parse(lines[i][j].ToString()));
                    trees[i, j] = newTree;
                }
            }

            long maxScenicScore = 0;

            for (int i = 0; i != noOfRows; i++)
            {
                for (int j = 0; j != noOfColumns; j++)
                {
                    int k = i - 1;
                    int visibleToNorth = 0;
                    while(k>=0 && trees[k,j].Height <= trees[i,j].Height)
                    {
                        visibleToNorth++;
                        if(trees[k, j].Height == trees[i, j].Height)
                        {
                            break;
                        }
                        k--;
                    }

                    int visibleToSouth = 0;
                    k = i + 1;
                    while(k<noOfRows && trees[k,j].Height <= trees[i,j].Height)
                    {
                        visibleToSouth++;
                        if(trees[k, j].Height == trees[i, j].Height)
                        {
                            break;
                        }
                        k++;
                    }

                    int visibleToRight = 0;
                    k = j + 1;
                    while(k<noOfColumns && trees[i,k].Height <= trees[i,j].Height)
                    {
                        visibleToRight++;
                        if(trees[i, k].Height == trees[i, j].Height)
                        {
                            break;
                        }
                        k++;
                    }

                    int visibleToLeft = 0;
                    k = j - 1;
                    while(k>=0 && trees[i,k].Height <= trees[i,j].Height)
                    {
                        visibleToLeft++;
                        if(trees[i, k].Height == trees[i, j].Height)
                        {
                            break;
                        }
                        k--;
                    }

                    maxScenicScore = Math.Max(maxScenicScore, visibleToLeft * visibleToRight * visibleToNorth * visibleToSouth);
                }
            }
            Console.WriteLine(maxScenicScore);
        }
    }
}
