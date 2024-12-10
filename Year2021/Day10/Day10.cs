using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day10 : IDay
    {
        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            var result = 0;
            foreach (var line in lines)
            {
                Stack<char> stack = new Stack<char>();
                for (int i = 0; i != line.Length; i++)
                {
                    if(line[i] == '(' || line[i] == '{' || line[i] == '[' || line[i] == '<')
                    {
                        stack.Push(line[i]);
                    }
                    else if (line[i] == ')' && stack.Peek() == '(')
                    {
                        stack.Pop();
                    }
                    else if (line[i] == ']' && stack.Peek() == '[')
                    {
                        stack.Pop();
                    }
                    else if (line[i] == '}' && stack.Peek() == '{')
                    {
                        stack.Pop();
                    }
                    else if (line[i] == '>' && stack.Peek() == '<')
                    {
                        stack.Pop();
                    }
                    else
                    {
                        if (line[i] == ')') result += 3;
                        if (line[i] == ']') result += 57;
                        if (line[i] == '}') result += 1197;
                        if (line[i] == '>') result += 25137;
                        break;
                    }
                }
            }
            Console.WriteLine(result);
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            var results = new List<long>();
            foreach (var line in lines)
            {
                Stack<char> stack = new Stack<char>();
                bool corruptedLine = false;
                for (int i = 0; i != line.Length; i++)
                {
                    if (line[i] == '(' || line[i] == '{' || line[i] == '[' || line[i] == '<')
                    {
                        stack.Push(line[i]);
                    }
                    else if (line[i] == ')' && stack.Peek() == '(')
                    {
                        stack.Pop();
                    }
                    else if (line[i] == ']' && stack.Peek() == '[')
                    {
                        stack.Pop();
                    }
                    else if (line[i] == '}' && stack.Peek() == '{')
                    {
                        stack.Pop();
                    }
                    else if (line[i] == '>' && stack.Peek() == '<')
                    {
                        stack.Pop();
                    }
                    else
                    {
                        corruptedLine = true;
                        break;
                    }
                }
                if(corruptedLine)
                {
                    continue;
                }
                long lineResult = 0;
                while(stack.Count > 0)
                {
                    lineResult *= 5;
                    char opening = stack.Pop();
                    if (opening == '(') lineResult += 1;
                    if (opening == '[') lineResult += 2;
                    if (opening == '{') lineResult += 3;
                    if (opening == '<') lineResult += 4;
                }
                results.Add(lineResult);
            }
            Console.WriteLine(results.OrderBy(x=>x).ToList()[results.Count/2]);
        }
    }
}
