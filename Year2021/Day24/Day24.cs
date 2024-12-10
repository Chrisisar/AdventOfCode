using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2021
{
    class Day24 : IDay
    {
        //List<long> PossibleNumbers = new List<long>();
        //string[] AllSteps;

        //public void NextStep(int currentStepId, long modelNumber, int w, int x, int y, int z)
        //{
        //    string currentStep = AllSteps[currentStepId];
        //    var splitStep = currentStep.Split(" ");
        //    switch (splitStep[0])
        //    {
        //        case "inp":
        //            switch (splitStep[1])
        //            {
        //                case "w":
        //                    for (int i = 9; i >= 1; i--)
        //                    {
        //                        NextStep(currentStepId + 1, modelNumber * 10 + i, i, x, y, z);
        //                    }
        //                    break;
        //                case "x":
        //                    for (int i = 9; i >= 1; i--)
        //                    {
        //                        NextStep(currentStepId + 1, modelNumber * 10 + i, w, i, y, z);
        //                    }
        //                    break;
        //                case "y":
        //                    for (int i = 9; i >= 1; i--)
        //                    {
        //                        NextStep(currentStepId + 1, modelNumber * 10 + i, w, x, i, z);
        //                    }
        //                    break;
        //                case "z":
        //                    for (int i = 9; i >= 1; i--)
        //                    {
        //                        NextStep(currentStepId + 1, modelNumber * 10 + i, w, x, y, i);
        //                    }
        //                    break;
        //            }
        //            return;
        //            break;
        //        case "add":
        //            int addValue;
        //            switch (splitStep[2])
        //            {
        //                case "w":
        //                    addValue = w;
        //                    break;
        //                case "x":
        //                    addValue = x;
        //                    break;
        //                case "y":
        //                    addValue = y;
        //                    break;
        //                case "z":
        //                    addValue = z;
        //                    break;
        //                default:
        //                    addValue = int.Parse(splitStep[2]);
        //                    break;
        //            }
        //            switch (splitStep[1])
        //            {
        //                case "w":
        //                    w += addValue;
        //                    break;
        //                case "x":
        //                    x += addValue;
        //                    break;
        //                case "y":
        //                    y += addValue;
        //                    break;
        //                case "z":
        //                    z += addValue;
        //                    break;
        //            }
        //            break;
        //        case "mul":
        //            int mulValue;
        //            switch (splitStep[2])
        //            {
        //                case "w":
        //                    mulValue = w;
        //                    break;
        //                case "x":
        //                    mulValue = x;
        //                    break;
        //                case "y":
        //                    mulValue = y;
        //                    break;
        //                case "z":
        //                    mulValue = z;
        //                    break;
        //                default:
        //                    mulValue = int.Parse(splitStep[2]);
        //                    break;
        //            }
        //            switch (splitStep[1])
        //            {
        //                case "w":
        //                    w *= mulValue;
        //                    break;
        //                case "x":
        //                    x *= mulValue;
        //                    break;
        //                case "y":
        //                    y *= mulValue;
        //                    break;
        //                case "z":
        //                    z *= mulValue;
        //                    break;
        //            }
        //            break;
        //        case "div":
        //            int divValue;
        //            switch (splitStep[2])
        //            {
        //                case "w":
        //                    divValue = w;
        //                    break;
        //                case "x":
        //                    divValue = x;
        //                    break;
        //                case "y":
        //                    divValue = y;
        //                    break;
        //                case "z":
        //                    divValue = z;
        //                    break;
        //                default:
        //                    divValue = int.Parse(splitStep[2]);
        //                    break;
        //            }
        //            switch (splitStep[1])
        //            {
        //                case "w":
        //                    w /= divValue;
        //                    break;
        //                case "x":
        //                    x /= divValue;
        //                    break;
        //                case "y":
        //                    y /= divValue;
        //                    break;
        //                case "z":
        //                    z /= divValue;
        //                    break;
        //            }
        //            break;
        //        case "mod":
        //            int modValue;
        //            switch (splitStep[2])
        //            {
        //                case "w":
        //                    modValue = w;
        //                    break;
        //                case "x":
        //                    modValue = x;
        //                    break;
        //                case "y":
        //                    modValue = y;
        //                    break;
        //                case "z":
        //                    modValue = z;
        //                    break;
        //                default:
        //                    modValue = int.Parse(splitStep[2]);
        //                    break;
        //            }
        //            switch (splitStep[1])
        //            {
        //                case "w":
        //                    w %= modValue;
        //                    break;
        //                case "x":
        //                    x %= modValue;
        //                    break;
        //                case "y":
        //                    y %= modValue;
        //                    break;
        //                case "z":
        //                    z %= modValue;
        //                    break;
        //            }
        //            break;
        //        case "eql":
        //            int eqlValue;
        //            switch (splitStep[2])
        //            {
        //                case "w":
        //                    eqlValue = w;
        //                    break;
        //                case "x":
        //                    eqlValue = x;
        //                    break;
        //                case "y":
        //                    eqlValue = y;
        //                    break;
        //                case "z":
        //                    eqlValue = z;
        //                    break;
        //                default:
        //                    eqlValue = int.Parse(splitStep[2]);
        //                    break;
        //            }
        //            switch (splitStep[1])
        //            {
        //                case "w":
        //                    w = eqlValue == w ? 1 : 0;
        //                    break;
        //                case "x":
        //                    x = eqlValue == x ? 1 : 0;
        //                    break;
        //                case "y":
        //                    y = eqlValue == y ? 1 : 0;
        //                    break;
        //                case "z":
        //                    z = eqlValue == z ? 1 : 0;
        //                    break;
        //            }
        //            break;
        //    }
            
        //    if (currentStepId + 1 == AllSteps.Length)
        //    {
        //        if(z==0)
        //        {
        //            Console.WriteLine(modelNumber);
        //            PossibleNumbers.Add(modelNumber);
        //        }
        //        return;
        //    }
        //    NextStep(currentStepId + 1, modelNumber, w, x, y, z);
        //}

        public void Task1()
        {
            //AllSteps = StaticHelpers.GetLines(this.GetType());
            //NextStep(0, 0, 0, 0, 0, 0);
            //Console.WriteLine(PossibleNumbers.Max());

            /**** Manually calculated requirements for z = 0
            //inp3 = 9 && inp4 = 1
            //inp5 + 2 = inp6
            //inp10 = inp9 - 6
            //inp11 = inp8 - 5
            //inp 12 = inp7 + 7
            //inp 13 = inp2
            //inp14 = imp1 + 5
            ******/

            Console.WriteLine("49917929934999");
        }

        public void Task2()
        {
            Console.WriteLine("11911316711816");
        }
    }
}
