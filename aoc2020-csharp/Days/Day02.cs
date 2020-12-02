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

        private static readonly Regex MatchRegex = new(@"^(?<min>\d+)-(?<max>\d+) (?<chr>[a-z]): (?<pass>.+)$");

        private static Func<string, bool> MatchesPolicy(Part part) => line =>
        {
            var match = MatchRegex.Match(line);
            var min = int.Parse(match.Groups["min"].Value);
            var max = int.Parse(match.Groups["max"].Value);
            var chr = match.Groups["chr"].Value[0];
            var pass = match.Groups["pass"].Value;
            switch (part)
            {
                case Part.One:
                    var count = pass.Count(x => x == chr);
                    return count >= min && count <= max;
                case Part.Two:
                    var minMatches = pass[min - 1] == chr;
                    var maxMatches = pass[max - 1] == chr;
                    return (minMatches || maxMatches) && !(minMatches && maxMatches);
            }

            return false;
        }

        public static int Part1(IEnumerable<string> input) =>
            input
                .Count(MatchesPolicy(Part.One));

        public static int Part2(IEnumerable<string> input) =>
            input
                .Count(MatchesPolicy(Part.Two));
    }
}
