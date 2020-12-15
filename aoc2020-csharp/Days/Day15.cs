using System;
using System.Collections.Generic;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day15 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var parsed = input.SplitAsInt(",");
            return (
                () => Part1(parsed.AsSpan()).ToString(),
                () => Part2(parsed.AsSpan()).ToString()
            );
        }

        public static int Part1(Span<int> input) =>
            Solve(input, 2020);

        public static int Part2(Span<int> input) =>
            Solve(input, 30000000);

        private static int Solve(Span<int> input, int max)
        {
            var seen = new Dictionary<int, int>();
            var numbers = new Span<int>(new int[max]);
            var len = 0;
            for (var i = 0; i < input.Length; i++)
            {
                if (i != input.Length - 1)
                    seen[input[i]] = i;
                numbers[i] = input[i];
                len++;
            }

            while (len < max)
            {
                var lastNum = numbers[len - 1];
                var lastIndex = seen.TryGetValue(lastNum, out var idx) ? idx : -1;
                seen[lastNum] = len - 1;

                if (lastIndex == -1)
                    lastNum = 0;
                else
                    lastNum = len - 1 - lastIndex;
                numbers[len] = lastNum;
                len++;
            }

            return numbers[^1];
        }
    }
}
