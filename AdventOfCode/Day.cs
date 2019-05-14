using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public interface IDay<TInput>
    {
        string RunPartA(TInput input);
        string RunPartB(TInput input);
    }
}
