using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day06 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var parsed = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            return (
                () => Part1(parsed).ToString(),
                () => Part2(parsed).ToString()
            );
        }

        public static int Part1(IEnumerable<string> input) =>
            input.Sum(x =>
                Enumerable.Range(97, 26)
                    .Count(y => x.Contains((char) y))
            );

        public static int Part2(IEnumerable<string> input) =>
            input
                .Select(x => x.SplitLines())
                .Sum(x =>
                    Enumerable.Range(97, 26)
                        .Count(y => x.All(z => z.Contains((char) y)))
                );
    }
}
