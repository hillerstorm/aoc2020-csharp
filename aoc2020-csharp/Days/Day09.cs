using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable IdentifierTypo

namespace aoc2020.Days
{
    public class Day09 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var parsed = input.SplitAsLong();
            return (
                () => Part1(parsed).ToString(),
                () => Part2(parsed).ToString()
            );
        }

        public static long Part1(long[] input, int preabmle = 25)
        {
            for (var i = preabmle; i < input.Length; i++)
            {
                var value = input[i];
                var pre = new ReadOnlySpan<long>(input, i - preabmle, preabmle);
                var found = false;
                for (var x = 0; x < pre.Length - 1; x++)
                {
                    var first = pre[x];
                    for (var y = x + 1; y < pre.Length; y++)
                    {
                        var second = pre[y];
                        if (first == second || first + second != value)
                            continue;

                        found = true;
                        break;
                    }

                    if (found)
                        break;
                }

                if (!found)
                    return value;
            }

            return input[^1];
        }

        public static long Part2(long[] input, int preamble = 25)
        {
            var firstInvalid = Part1(input, preamble);
            for (var len = 2; len < input.Length; len++)
                for (var i = 0; i < input.Length - len; i++)
                {
                    var slice = new ReadOnlySpan<long>(input, i, len);
                    long sum = 0;
                    var min = long.MaxValue;
                    long max = -1;
                    foreach (var value in slice)
                    {
                        if (value < min)
                            min = value;
                        if (value > max)
                            max = value;
                        sum += value;
                    }

                    if (sum == firstInvalid)
                        return min + max;
                }

            return -1;
        }
    }
}
