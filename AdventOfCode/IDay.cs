using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public interface IDay
    {
        string RunPartA(string input);
        string RunPartB(string input);
    }


    public abstract class Day<TInput> : IDay
    {
        public string RunPartA(string input) => this.RunPartA(this.ParseInput(input));

        public string RunPartB(string input) => this.RunPartB(this.ParseInput(input));

        public abstract TInput ParseInput(string input);

        public abstract string RunPartA(TInput input);

        public abstract string RunPartB(TInput input);
    }
}
