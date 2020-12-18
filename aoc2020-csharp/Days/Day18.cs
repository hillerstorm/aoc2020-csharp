using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day18 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var lines = input.SplitLines();
            return (
                () => Part1(lines).ToString(),
                () => Part2(lines).ToString()
            );
        }

        private static readonly Regex PairRegex = new(@"([\(]{0,1})(\d+)\s*(\+|\*)\s*(\d+)([\)]{0,1})");

        public static long Part1(IEnumerable<string> input) =>
            input.Sum(line =>
            {
                long parsed;
                while (!long.TryParse(line, out parsed))
                    line = ReplacePair(line);
                return parsed;
            });

        private static readonly Regex AdditionRegex = new(@"([\(]{0,1})(\d+)\s*\+\s*(\d+)([\)]{0,1})");
        private static readonly Regex MultiRegex = new(@"([\(]{0,1})(\d+)\s*\*\s*(\d+)([\)]{0,1})");
        private static readonly Regex PairExactRegex = new(@"(\()(\d+)\s*(\+|\*)\s*(\d+)(\))");
        private static readonly Regex SolvableParensRegex = new(@"\(([^\)\(]+)\)");

        public static long Part2(IEnumerable<string> input) =>
            input.Sum(line =>
            {
                long parsed;
                while (!long.TryParse(line, out parsed))
                {
                    while (AdditionRegex.IsMatch(line))
                        line = ReplaceAddition(line);

                    if (PairExactRegex.IsMatch(line))
                    {
                        while (PairExactRegex.IsMatch(line))
                            line = ReplacePair(line, exact: true);
                        continue;
                    }

                    if (SolvableParensRegex.IsMatch(line))
                    {
                        line = SolveMatchingParens(line);
                        continue;
                    }

                    while (MultiRegex.IsMatch(line))
                        line = ReplaceMulti(line);
                }

                return parsed;
            });

        private static string SolveMatchingParens(string expression) =>
            SolvableParensRegex.Replace(
                expression,
                x =>
                {
                    var contents = x.Groups[1].Value;
                    while (!long.TryParse(contents, out _))
                        contents = ReplacePair(contents);
                    return contents;
                }
            );

        private static string ReplacePair(string expression, bool exact = false) =>
            (exact ? PairExactRegex : PairRegex).Replace(
                expression,
                x =>
                {
                    var left = long.Parse(x.Groups[2].Value);
                    var right = long.Parse(x.Groups[4].Value);
                    var result = x.Groups[3].Value == "+"
                        ? (left + right).ToString()
                        : (left * right).ToString();
                    return (x.Groups[1].Value.Length > 0) switch
                    {
                        true when x.Groups[5].Value.Length > 0 =>
                            result,
                        true =>
                            x.Groups[1].Value + result,
                        _ =>
                            result + x.Groups[5].Value,
                    };
                },
                count: exact ? -1 : 1
            );

        private static string ReplaceAddition(string expression) =>
            AdditionRegex.Replace(
                expression,
                x =>
                {
                    var result = (long.Parse(x.Groups[2].Value) + long.Parse(x.Groups[3].Value)).ToString();
                    return (x.Groups[1].Value.Length > 0) switch
                    {
                        true when x.Groups[4].Value.Length > 0 =>
                            result,
                        true =>
                            x.Groups[1].Value + result,
                        _ =>
                            result + x.Groups[4].Value,
                    };
                }
            );

        private static string ReplaceMulti(string expression) =>
            MultiRegex.Replace(
                expression,
                x =>
                {
                    var result = (long.Parse(x.Groups[2].Value) * long.Parse(x.Groups[3].Value)).ToString();
                    return (x.Groups[1].Value.Length > 0) switch
                    {
                        true when x.Groups[4].Value.Length > 0 =>
                            result,
                        true =>
                            x.Groups[1].Value + result,
                        _ =>
                            result + x.Groups[4].Value,
                    };
                }
            );
    }
}
