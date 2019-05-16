using System;
using System.Linq;

namespace AdventOfCode
{
    public class Day1 : Day<int[]>
    {
        public override int[] ParseInput(string input)
        {
            return input.Split('\n').Select(int.Parse).ToArray();
        }

        public override string RunPartA(int[] input)
        {
            throw new NotImplementedException();
        }

        public override string RunPartB(int[] input)
        {
            throw new NotImplementedException();
        }
    }
}
