using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    class Day19 : IDay
    {
        class Robot
        {
            public static int MaxOreRobots;
        }

        class OreRobot :Robot
        {
            public static int OreCost;
        }

        class ClayRobot : Robot
        {
            public static int OreCost;
        }
        
        class ObsidianRobot : Robot
        {
            public static int OreCost;
            public static int ClayCost;
        }

        class GeodeRobot : Robot
        {
            public static int OreCost;
            public static int ClayCost;
            public static int ObsidianCost;
        }

        private int ParseBlueprintDetails(string blueprint)
        {
            //14427413
            var splitString = blueprint.Replace("Blueprint ", "")
                .Replace(": Each ore robot costs ", ",")
                .Replace(" ore. Each clay robot costs ", ",")
                .Replace(" ore. Each obsidian robot costs ",",")
                .Replace(" ore and ", ",")
                .Replace(" clay. Each geode robot costs ", ",")
                .Replace(" ore and ", ",")
                .Replace(" obsidian.", "").Split(",");

            int.TryParse(splitString[0], out int blueprintId);
            int.TryParse(splitString[1], out OreRobot.OreCost);
            int.TryParse(splitString[2], out ClayRobot.OreCost);
            int.TryParse(splitString[3], out ObsidianRobot.OreCost);
            int.TryParse(splitString[4], out ObsidianRobot.ClayCost);
            int.TryParse(splitString[5], out GeodeRobot.OreCost);
            int.TryParse(splitString[6], out GeodeRobot.ObsidianCost);

            Robot.MaxOreRobots = Math.Max(OreRobot.OreCost, Math.Max(ClayRobot.OreCost, Math.Max(ObsidianRobot.OreCost, GeodeRobot.OreCost)));
            return blueprintId;
        }

        public void Task1()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            long result = 0;
            foreach(var blueprint in lines)
            {
                result += CalculateBlueprintQuality(ParseBlueprintDetails(blueprint));
            }
            Console.WriteLine(result);
        }

        int maxGeode = 0;
        private void DFS(int minutesLeft, int oreRobotCount, int clayRobotCount, int obsidianRobotCount, int geodeRobotCount,
            int oreCount, int clayCount, int obsidianCount, int geodeCount)
        {
            if(geodeCount + geodeRobotCount * minutesLeft + minutesLeft*(minutesLeft+1)/2 <= maxGeode)
            {
                return;
            }
            maxGeode = Math.Max(maxGeode, geodeCount);
            if (minutesLeft <= 0)
            {
                return;
            }
            bool foundSomethingToDo = false;
            if (oreCount >= OreRobot.OreCost
                && oreCount + minutesLeft * oreRobotCount < Robot.MaxOreRobots * minutesLeft)
            {
                foundSomethingToDo = true;
                DFS(minutesLeft - 1, oreRobotCount + 1, clayRobotCount, obsidianRobotCount, geodeRobotCount, oreCount - OreRobot.OreCost + oreRobotCount, clayCount + clayRobotCount, obsidianCount + obsidianRobotCount, geodeCount + geodeRobotCount);
            }
            if (oreCount >= ClayRobot.OreCost
                && clayCount + minutesLeft * clayRobotCount < ObsidianRobot.ClayCost * minutesLeft)
            {
                foundSomethingToDo = true;
                DFS(minutesLeft - 1, oreRobotCount, clayRobotCount + 1, obsidianRobotCount, geodeRobotCount, oreCount - ClayRobot.OreCost + oreRobotCount, clayCount + clayRobotCount, obsidianCount + obsidianRobotCount, geodeCount + geodeRobotCount);
            }
            if (oreCount >= ObsidianRobot.OreCost && clayCount >= ObsidianRobot.ClayCost
                && obsidianCount + minutesLeft * obsidianRobotCount < GeodeRobot.ObsidianCost * minutesLeft)
            {
                foundSomethingToDo = true;
                DFS(minutesLeft - 1, oreRobotCount, clayRobotCount, obsidianRobotCount + 1, geodeRobotCount, oreCount - ObsidianRobot.OreCost + oreRobotCount, clayCount - ObsidianRobot.ClayCost + clayRobotCount, obsidianCount + obsidianRobotCount, geodeCount + geodeRobotCount);
            }
            if (oreCount >= GeodeRobot.OreCost && obsidianCount >= GeodeRobot.ObsidianCost)
            {
                foundSomethingToDo = true;
                DFS(minutesLeft - 1, oreRobotCount, clayRobotCount, obsidianRobotCount, geodeRobotCount + 1, oreCount - GeodeRobot.OreCost + oreRobotCount, clayCount - GeodeRobot.ClayCost + clayRobotCount, obsidianCount - GeodeRobot.ObsidianCost + obsidianRobotCount, geodeCount + geodeRobotCount);
            }

           
            if (oreRobotCount < Robot.MaxOreRobots || !foundSomethingToDo) //Part1
            //if (!foundSomethingToDo) //Part 2
            {
                DFS(minutesLeft - 1, oreRobotCount, clayRobotCount, obsidianRobotCount, geodeRobotCount, oreCount + oreRobotCount, clayCount + clayRobotCount, obsidianCount + obsidianRobotCount, geodeCount + geodeRobotCount);
            }
        }

        private long CalculateBlueprintQuality(int blueprintId)
        {
            maxGeode = 0;
            DFS(24, 1, 0, 0, 0, 0, 0, 0, 0);
            return maxGeode * blueprintId;
        }
        
        private long CalculateObtainedGeodes()
        {
            maxGeode = 0;
            DFS(28, 1, 0, 0, 0, 4, 0, 0, 0);
            return maxGeode;
        }

        public void Task2()
        {
            var lines = StaticHelpers.GetLines(this.GetType());
            long result = 1;
            for(int i = 0; i!=3;i++)
            {
                ParseBlueprintDetails(lines[i]);
                result *= CalculateObtainedGeodes();
            }
            Console.WriteLine(result);
        }
    }
}
