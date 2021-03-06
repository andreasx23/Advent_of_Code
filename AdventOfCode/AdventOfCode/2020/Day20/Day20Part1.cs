﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day20
{
    public class Day20Part1
    {
        private readonly Dictionary<int, char[][]> grids = new Dictionary<int, char[][]>();
        private readonly Dictionary<char[][], long> ids = new Dictionary<char[][], long>();

        private void Day20()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            int size = 12; //3

            long ans = 0;
            int n = grids.Count;            
            for (int index = 0; index < n; index++)
            {
                List<List<char[][]>> puzzle = new List<List<char[][]>>();

                for (int x = 0; x < size; x++)
                {
                    puzzle.Add(new List<char[][]>());
                }

                var corner = grids[index];
                puzzle[0].Add(corner);

                HashSet<long> isVisited = new HashSet<long>
                {
                    ids[corner]
                };

                int i = 0, nextIndex = 0;
                while (i != size && nextIndex != n)
                {
                    var current = grids[nextIndex];
                    while (isVisited.Contains(ids[current]))
                    {
                        nextIndex++;

                        if (nextIndex >= n) break;

                        current = grids[nextIndex];
                    }

                    if (nextIndex >= n) break;

                    bool isMatch = true;
                    if (i > 0 && puzzle[i].Count == 0) //Upper check
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (puzzle[i - 1].First().Last()[j] != current.First()[j])
                            {
                                isMatch = false;
                                break;
                            }
                        }
                    }
                    else if (i > 0 && puzzle[i].Count > 0) //Left check && Upper check
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (puzzle[i].Last()[j].Last() != current[j].First() || //Left
                                puzzle[i - 1][puzzle[i].Count].Last()[j] != current.First()[j]) //Upper
                            {
                                isMatch = false;
                                break;
                            }
                        }
                    }
                    else if (puzzle[i].Count > 0) //Left check
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (puzzle[i].Last()[j].Last() != current[j].First())
                            {
                                isMatch = false;
                                break;
                            }
                        }
                    }

                    if (isMatch)
                    {
                        puzzle[i].Add(current);
                        isVisited.Add(ids[current]);
                        nextIndex = 0;

                        if (puzzle[i].Count == size)
                        {
                            i++;
                        }

                        if (i == size)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                for (int k = 0; k < size; k++)
                                {
                                    Console.Write(ids[puzzle[j][k]] + " ");
                                }
                                Console.WriteLine();
                            }

                            ans = ids[puzzle.First().First()] * ids[puzzle.First().Last()] * ids[puzzle.Last().First()] * ids[puzzle.Last().Last()];
                            break;
                        }
                    }
                    else
                    {
                        nextIndex++;
                    }
                }
            }

            watch.Stop();
            Console.WriteLine($"Answer: {ans} took {watch.ElapsedMilliseconds} ms");
        }

        private char[][] GenerateGrid(int size)
        {
            char[][] grid = new char[size][];
            for (int i = 0; i < size; i++)
            {
                char[] width = new char[size];
                grid[i] = width;
            }
            return grid;
        }

        private char[][] Rotate(char[][] grid)
        {
            char[][] temp = GenerateGrid(grid.Length);
            for (int i = 0; i < grid.Length; i++)
            {
                for (int k = 0; k < grid[0].Length; k++)
                {
                    temp[k][temp.Length - 1 - i] = grid[i][k];
                }
            }
            return temp;
        }

        private void ReadData()
        {
            string path = @"C:\Users\bruger\Desktop\AdventOfCode2020\2020\Day 20\input.txt";
            var lines = File.ReadAllLines(path).ToList();
            lines.Add(string.Empty);

            //10x10
            char[][] grid = GenerateGrid(10);
            int index = 0;
            int counter = 0;
            long currentID = 0;
            foreach (var s in lines)
            {
                if (string.IsNullOrEmpty(s))
                {
                    grids.Add(index, grid);
                    ids.Add(grid, currentID);

                    for (int i = 0; i < 3; i++)
                    {
                        var last = grids[index++];
                        char[][] temp = Rotate(last);
                        grids.Add(index, temp);
                        ids.Add(temp, currentID);
                    }

                    index++;
                    char[][] flip = GenerateGrid(10);
                    int position = 0;
                    for (int i = grid.Length - 1; i >= 0; i--)
                    {
                        flip[position++] = grid[i];
                    }
                    grids.Add(index, flip);
                    ids.Add(flip, currentID);

                    for (int i = 0; i < 3; i++)
                    {
                        var last = grids[index++];
                        char[][] temp = Rotate(last);
                        grids.Add(index, temp);
                        ids.Add(temp, currentID);
                    }

                    index++;
                    counter = 0;
                    grid = GenerateGrid(10);
                }
                else if (!s.Contains(" "))
                {
                    grid[counter++] = s.ToArray();
                }
                else
                {
                    var split = s.Split(' ');
                    currentID = long.Parse(split[1].Substring(0, 4));
                }
            }
        }

        public void TestCase()
        {
            ReadData();
            Day20();
        }
    }
}
