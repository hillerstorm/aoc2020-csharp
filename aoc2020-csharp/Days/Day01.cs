using System;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day01 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string rawInput)
        {
            var input = rawInput.SplitAsInt();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static int Part1(int[] input)
        {
            var (a, b) = input.Pairs(input).First(x => x.A + x.B == 2020);
            return a * b;
        }

        public static int Part2(int[] input)
        {
            var (a, b, c) = input.Triples(input, input).First(x => x.A + x.B + x.C == 2020);
            return a * b * c;
        }
    }
}
