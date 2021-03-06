﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2019.Day06
{
    public class Day6Part1
    {
        private readonly Dictionary<string, Planet> map = new Dictionary<string, Planet>();
        private readonly string start = "COM";

        private void Day6()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            Queue<(Planet node, int val)> queue = new Queue<(Planet node, int val)>();
            queue.Enqueue((map[start], 0));
            int ans = 0;
            while (queue.Any())
            {
                (Planet node, int val) = queue.Dequeue();

                ans += val;

                foreach (var child in node.Children)
                {
                    queue.Enqueue((child, val + 1));
                }
            }

            watch.Stop();
            Console.WriteLine($"Answer: {ans} took {watch.ElapsedMilliseconds} ms");
        }

        private void ReadData()
        {
            string path = @"C:\Users\andre\Desktop\AdventOfCode2020\2019\Day06\input.txt";
            var lines = File.ReadAllLines(path);

            foreach (var s in lines)
            {
                var split = s.Split(')');

                if (!map.ContainsKey(split.First()))
                {
                    map.Add(split.First(), new Planet() { Name = split.First() });
                }

                if (!map.ContainsKey(split.Last()))
                {
                    map.Add(split.Last(), new Planet() { Name = split.Last() });
                }
            }

            foreach (var s in lines)
            {
                var split = s.Split(')');

                var node = map[split.First()];
                var child = map[split.Last()];
                node.Children.Add(child);
                child.Parent = node;
            }
        }

        public void TestCase()
        {
            ReadData();
            Day6();
        }
    }
}
