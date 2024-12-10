using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day18 : IDay
    {
        struct TreeNode
        {
            public bool IsPopulated;
            public int? Value;
        }

        TreeNode[] Tree = new TreeNode[1024];

        private void GetTreeFromString(string str)
        {
            var treePosition = 1;
            for (int i = 0; i != str.Length; i++)
            {
                switch (str[i])
                {
                    case '[':
                        Tree[treePosition] = new TreeNode { IsPopulated = true };
                        treePosition *= 2;
                        break;
                    case ',':
                        treePosition++;
                        break;
                    case ']':
                        treePosition = (treePosition - 1) / 2;
                        break;
                    default:
                        Tree[treePosition] = new TreeNode { IsPopulated = true, Value = str[i] - '0' };
                        break;
                }
            }
        }

        void AddTree(string newTree)
        {
            for (int i = 16; i != 0; i /= 2)
            {
                for (int j = i; j != i * 2; j++)
                {
                    Tree[j + i] = Tree[j];
                }
            }
            var newTempTree = new TreeNode[1024];
            var treePosition = 1;
            for (int i = 0; i != newTree.Length; i++)
            {
                switch (newTree[i])
                {
                    case '[':
                        newTempTree[treePosition] = new TreeNode { IsPopulated = true };
                        treePosition *= 2;
                        break;
                    case ',':
                        treePosition++;
                        break;
                    case ']':
                        treePosition = (treePosition - 1) / 2;
                        break;
                    default:
                        newTempTree[treePosition] = new TreeNode { IsPopulated = true, Value = newTree[i] - '0' };
                        break;
                }
            }
            for (int i = 16; i != 0; i /= 2)
            {
                for (int j = i; j != i * 2; j++)
                {
                    Tree[j + i + i] = newTempTree[j];
                }
            }
            Tree[1] = new TreeNode { IsPopulated = true };
        }

        void OutputTree()
        {
            int powerOfTwo = 1;
            for (int pt = 1; pt != 6; pt++)
            {
                //if(pt!=5)
                //{
                //    Console.Write(" ");
                //}
                for (int j = 1; j < Math.Pow(2, 5 - pt); j++)
                {
                    Console.Write(" ");
                }
                for (int i = powerOfTwo; i != powerOfTwo * 2; i++)
                {
                    Console.Write(Tree[i].Value?.ToString() ?? (Tree[i].IsPopulated ? "^" : " "));
                    if (i != powerOfTwo * 2 - 1)
                    {
                        for (int j = 0; j < (pt == 5 ? 0 : Math.Pow(2, 4 - pt)) + Math.Pow(2, 5 - pt); j++)
                        {
                            Console.Write(" ");
                        }
                    }
                }
                powerOfTwo *= 2;
                Console.WriteLine();
            }
        }

        int FindNeighbour(int index, bool leftSide)
        {
            int previousIndex = index;
            index /= 2;
            if (leftSide)
            {
                while (index * 2 == previousIndex) //Go up the tree
                {
                    previousIndex = index;
                    index /= 2;
                    if (index == 0)
                    {
                        return 0;
                    }
                }
                index *= 2; //Go left once
                while (Tree[index].IsPopulated && !Tree[index].Value.HasValue) //Go down the tree
                {
                    index *= 2;
                    index++;
                }
            }
            else
            {
                while (index * 2 != previousIndex) //Go up the tree
                {
                    previousIndex = index;
                    index /= 2;
                    if (index == 0)
                    {
                        return 0;
                    }
                }
                index *= 2; index++; //Go right once
                while (Tree[index].IsPopulated && !Tree[index].Value.HasValue) //Go down the tree
                {
                    index *= 2;
                }
            }
            return index;
        }

        int FindLeftMostNode()
        {
            int index = 1;
            while (Tree[index].IsPopulated && !Tree[index].Value.HasValue)
            {
                index *= 2;
            }
            return index;
        }

        void ExplodePair(int index)
        {
            var leftValue = Tree[index * 2].Value;
            var rightValue = Tree[index * 2 + 1].Value;

            var leftNeighbourIndex = FindNeighbour(index, true);
            var rightNeighbourIndex = FindNeighbour(index, false);
            Tree[leftNeighbourIndex].Value += leftValue;
            Tree[rightNeighbourIndex].Value += rightValue;

            Tree[index * 2] = new TreeNode();
            Tree[index * 2 + 1] = new TreeNode();
            Tree[index].Value = 0;
        }

        bool SplitNode(int index)
        {
            var value = Tree[index].Value;
            if (value > 9)
            {
                Tree[index * 2] = new TreeNode { IsPopulated = true, Value = value / 2 };
                Tree[index * 2 + 1] = new TreeNode { IsPopulated = true, Value = value - Tree[index * 2].Value };
                Tree[index].Value = null;
                return true;
            }
            return false;
        }

        private long CalculateMagnitude(int index, int timesX)
        {
            if (Tree[index].Value.HasValue)
            {
                return Tree[index].Value.Value * timesX;
            }
            long result = CalculateMagnitude(index * 2, 3) + CalculateMagnitude(index * 2 + 1, 2);
            return result * timesX;
        }

        private void ProcessListSteps()
        {
            bool breakLoop = false;
            while (!breakLoop)
            {
                bool continueLoop = false;
                for (int i = 16; i != 32; i++)
                {
                    if (Tree[i].IsPopulated && !Tree[i].Value.HasValue)
                    {
                        ExplodePair(i);
                        continueLoop = true;
                        break;
                    }
                }
                if (continueLoop)
                {
                    continue;
                }
                int splitIndex = FindLeftMostNode();
                while (splitIndex != 0)
                {
                    if (SplitNode(splitIndex))
                    {
                        continueLoop = true;
                        break;
                    }
                    splitIndex = FindNeighbour(splitIndex, false);
                }
                if (continueLoop)
                {
                    continue;
                }
                breakLoop = true;
            }
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            GetTreeFromString(lines[0]);
            for (int i = 1; i != lines.Length; i++)
            {
                AddTree(lines[i]);
                ProcessListSteps();
            }
            Console.WriteLine(CalculateMagnitude(1 , 1));
            OutputTree();
        }

        

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            long maxMagnitude = 0;
            int maxI = 0, maxJ = 0;
            for (int i =0;i!=lines.Length;i++)
            {
                for(int j = 0; j!=lines.Length;j++)
                {
                    if (i == j)
                        continue;
                    Tree = new TreeNode[1024];
                    GetTreeFromString(lines[i]);
                    AddTree(lines[j]);
                    ProcessListSteps();
                    var magnitude = CalculateMagnitude(1, 1);
                    if(magnitude > maxMagnitude)
                    {
                        maxMagnitude = magnitude;
                        maxI = i;
                        maxJ = j;
                    }
                }
            }


            GetTreeFromString(lines[maxI]);
            AddTree(lines[maxJ]);
            OutputTree();

            Console.WriteLine(maxMagnitude);
        }
    }
}
