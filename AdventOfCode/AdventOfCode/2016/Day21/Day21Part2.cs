﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2016.Day21
{
    public class Day21Part2
    {
        private static readonly bool isSample = false;
        private readonly string input = (isSample) ? "abcde" : "fbgdceah";
        private List<string> instructions = new List<string>();

        private void Day21()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            string ans = input;
            foreach (var s in instructions)
            {
                string temp = string.Empty;
                if (s.Contains("swap position"))
                {
                    temp = s.Replace("swap position ", "").Replace("with position ", "");
                    List<int> split = temp.Split(' ').Select(int.Parse).ToList();
                    ans = SwapIndexXWithIndexY(ans.ToCharArray(), split[1], split[0]);
                }
                else if (s.Contains("swap letter"))
                {
                    temp = s.Replace("swap letter ", "").Replace("with letter ", "");
                    List<char> split = temp.Split(' ').Select(char.Parse).ToList();
                    ans = SwapLetterXWithLetterY(ans.ToCharArray(), split[1], split[0]);
                }
                else if (s.Contains("reverse positions"))
                {
                    temp = s.Replace("reverse positions ", "").Replace("through ", "");
                    List<int> split = temp.Split(' ').Select(int.Parse).ToList();
                    ans = Reverse(ans.ToCharArray(), split[0], split[1]);
                }
                else if (s.Contains("rotate left"))
                {
                    temp = s.Replace("rotate left ", "").Replace(" step", "");
                    ans = Rotate(ans.ToCharArray(), int.Parse(temp.First().ToString()), false);
                }
                else if (s.Contains("rotate right"))
                {
                    temp = s.Replace("rotate right ", "").Replace(" step", "");
                    ans = Rotate(ans.ToCharArray(), int.Parse(temp.First().ToString()), true);
                }
                else if (s.Contains("move position"))
                {
                    temp = s.Replace("move position ", "").Replace("to position ", "");
                    List<int> split = temp.Split(' ').Select(int.Parse).ToList();
                    ans = MoveIndexXToIndexY(ans.ToCharArray(), split[1], split[0]);
                }
                else if (s.Contains("rotate based"))
                {
                    temp = s.Replace("rotate based on position of letter ", "");
                    ans = RotateOfLetterX(ans.ToCharArray(), temp.Last());
                }
            }

            Console.WriteLine("bdehafgc");

            watch.Stop();
            Console.WriteLine($"Answer: {ans} took {watch.ElapsedMilliseconds} ms");
        }

        private string SwapIndexXWithIndexY(char[] current, int x, int y)
        {
            char temp = current[x];
            current[x] = current[y];
            current[y] = temp;
            return new string(current);
        }

        private string SwapLetterXWithLetterY(char[] current, char x, char y)
        {
            List<char> temp = current.ToList();
            int indexX = temp.IndexOf(x), indexY = temp.IndexOf(y);
            return SwapIndexXWithIndexY(current, indexX, indexY);
        }

        private string Rotate(char[] current, int steps, bool isLeft)
        {
            int n = current.Length;
            if (!isLeft)
            {
                for (int i = 0; i < steps; i++)
                {
                    char[] temp = (char[])current.Clone();
                    for (int j = n - 1; j >= 0; j--)
                    {
                        temp[j] = current[(j - 1 < 0) ? n - 1 : j - 1];
                    }
                    current = temp;
                }
            }
            else
            {
                for (int i = 0; i < steps; i++)
                {
                    char[] temp = (char[])current.Clone();
                    for (int j = 0; j < n; j++)
                    {
                        temp[j] = current[(j + 1) % n];
                    }
                    current = temp;
                }
            }
            return new string(current);
        }

        private string RotateOfLetterX(char[] current, char letter)
        {
            List<char> temp = current.ToList();
            int index = temp.IndexOf(letter);

            return Rotate(current, (index >= 4 ? index + 2 : index + 1), true);
            /*
             * 
             * Position	Rotate by letter	Inverse
                0	    rotate right 1	    rotate left   1
                1	    rotate right 2  	rotate left   1
                2	    rotate right 3  	rotate right 2
                3	    rotate right 4  	rotate left   2
                4	    rotate left   2 	rotate right 1
                5	    rotate left   1 	rotate left   3
                6   	no change	        no change
                7   	rotate right 1	    rotate right 4
             */

            if (index == 0)
            {
                return Rotate(current, 1, true);
            }
            else if (index == 1)
            {
                return Rotate(current, 1, true);
            }
            else if (index == 2)
            {
                return Rotate(current, 2, false);
            }
            else if (index == 3)
            {
                return Rotate(current, 2, true);
            }
            else if (index == 4)
            {
                return Rotate(current, 1, false);
            }
            else if (index == 5)
            {
                return Rotate(current, 3, true);
            }
            else if (index == 6)
            {
                return new string(current);
            }
            else if (index == 7)
            {
                return Rotate(current, 4, false);
            }

            return null;
            //return Rotate(current, (index >= 4 ? index + 2 : index + 1), true);
        }

        private string Reverse(char[] current, int x, int y)
        {
            string temp = new string(current);
            string reverse = new string(temp.Substring(x, y - x + 1).Reverse().ToArray());
            return temp.Substring(0, x) + reverse + temp.Substring(y + 1);
        }

        private string MoveIndexXToIndexY(char[] current, int x, int y)
        {
            List<char> temp = current.ToList();
            char letter = temp[x];
            temp.RemoveAt(x);
            temp.Insert(y, letter);
            return new string(temp.ToArray());
        }

        private void ReadData()
        {
            string path = @"C:\Users\andre\Desktop\AdventOfCode2020\2016\Day21\input.txt";
            instructions = File.ReadAllLines(path).ToList();
        }

        public void TestCase()
        {
            ReadData();
            Day21();
        }
    }
}
