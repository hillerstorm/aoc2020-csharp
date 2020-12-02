using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc2020.Days
{
    public class Day02 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string rawInput)
        {
            var input = rawInput.SplitLines();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        private static readonly Regex MatchRegex = new(@"^(\d+)-(\d+) ([a-z]): (.+)$");

        private static Func<string, bool> MatchesPolicy(Part part) => line =>
        {
            var match = MatchRegex.Match(line);
            var (_, min, max, chr, pass, _) = match.Groups;
            switch (part)
            {
                case Part.One:
                    var count = pass.Value.Count(x => x == chr.Value[0]);
                    return count >= int.Parse(min.Value) && count <= int.Parse(max.Value);
                case Part.Two:
                    return pass.Value[int.Parse(min.Value) - 1] == chr.Value[0] ^
                           pass.Value[int.Parse(max.Value) - 1] == chr.Value[0];
            }

            return false;
        };

        public static int Part1(IEnumerable<string> input) =>
            input
                .Count(MatchesPolicy(Part.One));

        public static int Part2(IEnumerable<string> input) =>
            input
                .Count(MatchesPolicy(Part.Two));
    }
}
