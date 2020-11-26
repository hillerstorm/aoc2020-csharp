using System;

namespace aoc2020
{
    public interface IDay
    {
        (Func<string> Part1, Func<string> Part2) Parts(string input);
    }
}
