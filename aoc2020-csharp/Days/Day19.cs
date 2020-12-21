using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day19 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var lines = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            return (
                () => Part1(lines).ToString(),
                () => Part2(lines).ToString()
            );
        }

        public static int Part1(string[] input) =>
            input[1]
                .SplitLines()
                .Count(ParseRules(input[0].SplitLines(), Part.One)[0].Contains);

        public static int Part2(string[] input)
        {
            var rules = ParseRules(input[0].SplitLines(), Part.Two);
            var fortyTwo = rules[42];
            var thirtyOne = rules[31];
            var chunkSize = fortyTwo[0].Length;
            return input[1].SplitLines().Count(x => MatchesRule(x, fortyTwo, thirtyOne, chunkSize));
        }

        private static bool MatchesRule(string message, string[] fortyTwo, string[] thirtyOne, int chunkSize)
        {
            if (message.Length < chunkSize * 3 || message.Length % chunkSize != 0)
                return false;

            if (!fortyTwo.Any(message.StartsWith) || !thirtyOne.Any(message.EndsWith))
                return false;

            var chunks = message.PartitionBy(chunkSize).Select(x => new string(x.ToArray()));
            var end = false;
            var thirtyOnes = 0;
            var fortyTwos = 0;
            foreach (var chunk in chunks)
            {
                if (end)
                {
                    if (!thirtyOne.Contains(chunk))
                        return false;

                    thirtyOnes++;
                }

                if (!end)
                {
                    if (thirtyOne.Contains(chunk))
                    {
                        end = true;
                        thirtyOnes++;
                    }
                    else if (!fortyTwo.Contains(chunk))
                        return false;
                    else
                        fortyTwos++;
                }
            }

            return fortyTwos > thirtyOnes;
        }

        private static Dictionary<int, string[]> ParseRules(IEnumerable<string> lines, Part part)
        {
            var dict = lines
                .Select(x => x.Split(": ", StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(x => int.Parse(x[0]), x => x[1].Split(" | ", StringSplitOptions.TrimEntries));
            string[]? fortyTwo = null;
            string[]? thirtyOne = null;
            while (dict.Count > 1)
            {
                var chars = dict.Where(x => x.Value.All(v => v.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '"'))).ToArray();
                foreach (var key in dict.Keys)
                {
                    foreach (var (k, v) in chars)
                    {
                        if (k == key)
                            continue;

                        var keyRegex = new Regex(@$"(^| ){k.ToString()}($| )");
                        if (!dict[key].Any(x => keyRegex.IsMatch(x)))
                            continue;

                        dict[key] =
                            dict[key]
                                .SelectMany(x =>
                                {
                                    if (!keyRegex.IsMatch(x))
                                        return new[] {x};

                                    var res = new List<string> {x};
                                    do
                                    {
                                        res = res.SelectMany(r =>
                                            keyRegex.IsMatch(r)
                                                ? v.Select(z =>
                                                    keyRegex.Replace(
                                                        r,
                                                        m =>
                                                            m.Groups[1].Value +
                                                            z.Replace(" ", string.Empty).Trim('"') +
                                                            m.Groups[2].Value
                                                    ))
                                                : new[] {r}
                                        ).ToList();
                                    } while (res.Any(r => keyRegex.IsMatch(r)));

                                    return res.ToArray();
                                }).ToArray();
                    }
                }

                foreach (var (k, v) in chars)
                {
                    dict.Remove(k);
                    if (part == Part.One)
                        continue;

                    if (k == 42)
                        fortyTwo = v;
                    else if (k == 31)
                        thirtyOne = v;

                    if (fortyTwo != null && thirtyOne != null)
                        return new Dictionary<int, string[]>
                        {
                            {31, thirtyOne.Select(x => x.Replace(" ", string.Empty)).ToArray()},
                            {42, fortyTwo.Select(x => x.Replace(" ", string.Empty)).ToArray()},
                        };
                }
            }

            return new Dictionary<int, string[]>
            {
                {0, dict[0].Select(x => x.Replace(" ", string.Empty)).ToArray()},
            };
        }
    }
}
