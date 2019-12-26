﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Position = AdventOfCode.GenericPosition2D<int>;

namespace day10
{
    public class Day10
    {
        readonly static string nsname = typeof(Day10).Namespace;

        static List<Position> ReadInput()
        {
            List<Position> list = new List<Position>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\" + nsname + "\\input.txt");
            StreamReader reader = File.OpenText(path);
            string line;
            int y = 0;
            while ((line = reader.ReadLine()) != null)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                    {
                        list.Add(new Position(x, y));
                    }
                }
                y++;
            }
            return list;
        }

        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        static Position bestPos = new Position();

        static Object PartA()
        {
            List<Position> input = ReadInput();
            int maxAsteroids = int.MinValue;
            foreach (Position c in input)
            {
                HashSet<Position> directions = new HashSet<Position>();
                foreach (Position p in input)
                {
                    if (p != c)
                    {
                        Position d = p - c;
                        int k = Math.Abs(GCD(d.x, d.y));
                        directions.Add(d / k);
                        //Console.WriteLine("Delta [{0},{1}] ==> direction [{2},{3}]", d.x, d.y, (d/k).x, (d/k).y);
                    }
                }
                int n = directions.Count;
                if (n > maxAsteroids)
                {
                    maxAsteroids = n;
                    bestPos = c;
                }
            }
            Console.WriteLine("Part A: Result is {0} (at Position {1},{2}).", maxAsteroids, bestPos.x, bestPos.y);
            return maxAsteroids;
        }

        static Object PartB()
        {
            List<Position> input = ReadInput();
            Position c = bestPos;
            Dictionary<double, List<Position>> directions = new Dictionary<double, List<Position>>();
            foreach (Position p in input)
            {
                if (p != c)
                {
                    Position d = p - c;
                    int k = Math.Abs(GCD(d.x, d.y));
                    Position dir = d / k;
                    double a = Math.Atan2(dir.x, dir.y);
                    if (!directions.ContainsKey(a))
                        directions[a] = new List<Position>();
                    directions[a].Add(d);
                }
            }
            directions = directions.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => 
                x.Value.OrderBy(p => p.x * p.x + p.y * p.y).ToList());
            int i = 1;
            Position resDelta = new Position();
            foreach (var kvp in directions)
            {
                //Position absPos = c + kvp.Value.First();
                //Console.WriteLine("{0} vaporized at [{1},{2}] (key = {3})", i, absPos.x, absPos.y, kvp.Key);
                if (i == 200)
                {
                    resDelta = kvp.Value.First();
                    break;
                }
                i++;
            }
            Position resPos = c + resDelta;
            int res = resPos.x * 100 + resPos.y;
            Console.WriteLine("Part B: Result is {0}", res);
            return res;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2019 - " + nsname + ":");
            PartA();
            PartB();
        }

        public static bool MainTest()
        {
            int a = 280;
            int b = 706;
            return (PartA().Equals(a)) && (PartB().Equals(b));
        }
    }
}
