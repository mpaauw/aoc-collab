using AdventOfCode.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace AdventOfCode.Test
{
    public class Day1TestData : DayTestData<Day1, int[]>
    {
        public override IEnumerable<(int[] input, string expected)> PartA => new List<(int[] input, string expected)>{
            (new int[] {1,-2,3,1}, "3"),
        };

        public override IEnumerable<(int[] input, string expected)> PartB => new List<(int[] input, string expected)>{
            (new int[] {1,-2,3,1}, "3"),
        };
    }

    public class Day1Test : DayTest<Day1, int[], Day1TestData> { }
}
