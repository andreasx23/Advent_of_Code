﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2016.Day24
{
    public class Day24Part1
    {
        private int H = 0, W = 0;
        private char[][] grid = null;

        private void Day24()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            List<Tile> tiles = new List<Tile>();
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    char c = grid[i][j];
                    if (char.IsDigit(c))
                    {
                        tiles.Add(new Tile()
                        {
                            X = i,
                            Y = j,
                            Value = c
                        });
                    }
                }
            }

            int ans = 0;
            HashSet<Tile> isFinished = new HashSet<Tile>();
            Tile start = tiles.First(t => t.Value == '0');
            isFinished.Add(start);
            for (int i = 0; i < tiles.Count - 1; i++)
            {
                Queue<Tile> queue = new Queue<Tile>();
                queue.Enqueue(start);

                int minDistance = int.MaxValue;
                foreach (var tile in tiles)
                {
                    if (start.Value != tile.Value && !isFinished.Contains(tile))
                    {
                        int distance = CalculateManhattenDistance(start.X, tile.X, start.Y, tile.Y);
                        tile.Distance = distance;
                        minDistance = Math.Min(minDistance, distance);
                    }
                }
                List<Tile> targets = tiles.Where(t => !isFinished.Contains(t) && t.Distance == minDistance).ToList();
                Tile target = targets.Last();

                bool[,] isVisited = new bool[H, W];
                while (queue.Any())
                {
                    Tile current = queue.Dequeue();

                    if (current.X == target.X && current.Y == target.Y)
                    {
                        start = target;
                        isFinished.Add(start);
                        ans += current.Cost;
                        break;
                    }

                    List<Tile> neighbours = Neighbours(current);
                    foreach (var next in neighbours)
                    {
                        if (!isVisited[next.X, next.Y])
                        {
                            isVisited[next.X, next.Y] = true;
                            next.Cost = current.Cost + 1;
                            queue.Enqueue(next);
                        }
                    }
                }
            }

            //Print();

            watch.Stop();
            Console.WriteLine($"Answer: {ans} took {watch.ElapsedMilliseconds} ms");
        }

        private List<Tile> Neighbours(Tile current)
        {
            List<Tile> dirs = new List<Tile>()
            {
                new Tile() { X = 1, Y = 0 },
                new Tile() { X = -1, Y = 0 },
                new Tile() { X = 0, Y = 1 },
                new Tile() { X = 0, Y = -1 },
            };

            List<Tile> result = new List<Tile>();
            foreach (var next in dirs)
            {
                next.X += current.X;
                next.Y += current.Y;

                if (next.X < 0 || next.X >= H || next.Y < 0 || next.Y >= W || grid[next.X][next.Y] == '#')
                {
                    continue;
                }

                result.Add(next);
            }

            return result;
        }

        public int CalculateManhattenDistance(int X1, int X2, int Y1, int Y2)
        {
            return Math.Abs(X2 - X1) + Math.Abs(Y2 - Y1);
        }

        private void Print()
        {
            foreach (var item in grid)
            {
                Console.WriteLine(string.Join("", item));
            }
        }

        private void ReadData()
        {
            string path = @"C:\Users\Andreas\Desktop\AdventOfCode2020\2016\Day24\input.txt";
            var lines = File.ReadAllLines(path).ToList();

            H = lines.Count;
            W = lines[0].Length;
            grid = new char[H][];
            for (int i = 0; i < H; i++)
            {
                grid[i] = lines[i].ToCharArray();
            }
        }

        public void TestCase()
        {
            ReadData();
            Day24();
        }
    }
}
