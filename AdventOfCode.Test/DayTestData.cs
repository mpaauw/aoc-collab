using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Test
{
    public abstract class DayTestData<TDay, TInput>
        where TDay : Day<TInput>
    {
        public abstract IEnumerable<(TInput input, string expected)> PartA { get; }
        public abstract IEnumerable<(TInput input, string expected)> PartB { get; }
    }
}
