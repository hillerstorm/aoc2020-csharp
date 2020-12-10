using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable ConvertIfStatementToSwitchStatement

namespace aoc2020.Days
{
    public class Day10 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var parsed = input.SplitAsInt().ToList();
            return (
                () => Part1(parsed).ToString(),
                () => Part2(parsed).ToString()
            );
        }

        public static int Part1(IEnumerable<int> input)
        {
            var ones = 0;
            var threes = 0;
            foreach (var diff in PrepInput(input).TakeTwo().Select(x => x.B - x.A))
                if (diff == 1)
                    ones++;
                else if (diff == 3)
                    threes++;

            return ones * threes;
        }

        public static long Part2(IEnumerable<int> input) =>
            CountPaths(0, PrepInput(input), new Dictionary<int, long>());

        private static IReadOnlyList<int> PrepInput(IEnumerable<int> input)
        {
            var list = new List<int>(input) {0};
            list.Sort();
            list.Add(list[^1] + 3);
            return list.AsReadOnly();
        }

        private static long CountPaths(int i, IReadOnlyList<int> input, IDictionary<int, long> memo)
        {
            if (i == input.Count - 1)
                return 1;

            if (memo.TryGetValue(i, out var cached))
                return cached;

            long result = 0;
            for (var j = i + 1; j < input.Count; j++)
                if (input[j] - input[i] <= 3)
                    result += CountPaths(j, input, memo);

            memo[i] = result;

            return result;
        }
    }
}
