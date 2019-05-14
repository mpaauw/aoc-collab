using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode.Test
{
    public abstract class DayTest<TDay, TInput, TDayTestData>
        where TDay : IDay<TInput>, new()
        where TDayTestData : DayTestData<TDay, TInput>, new()
    {
        public static IEnumerable<object[]> PartAData => new TDayTestData().PartA.Select(o => new object[] { o.input, o.expected });
        public static IEnumerable<object[]> PartBData => new TDayTestData().PartB.Select(o => new object[] { o.input, o.expected });

        [Theory]
        [MemberData(nameof(PartAData))]
        public void CSharpPartA(TInput input, string expected) => Assert.Equal(expected, new TDay().RunPartA(input));

        [Theory]
        [MemberData(nameof(PartBData))]
        public void CSharpPartB(TInput input, string expected) => Assert.Equal(expected, new TDay().RunPartB(input));
    }
}
