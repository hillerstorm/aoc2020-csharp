using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day01 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            return (
                () => Part1(input),
                () => Part2(input)
            );
        }

        public static string Part1(string input)
        {
            return string.Empty;
        }

        public static string Part2(string input)
        {
            return string.Empty;
        }
    }
}
