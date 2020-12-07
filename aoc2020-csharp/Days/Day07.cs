using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace aoc2020.Days
{
    public class Day07 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var lines = input.SplitLines();
            return (
                () => Part1(lines).ToString(),
                () => Part2(lines).ToString()
            );
        }

        public static int Part1(IEnumerable<string> input) =>
            Contains(Parse(input))("shiny gold bag").Count();

        public static int Part2(IEnumerable<string> input)
        {
            var bags = Parse(input);
            return Count(bags, bags["shiny gold bag"]) - 1;
        }

        private static int Count(
            IReadOnlyDictionary<string, (int Count, string Bag)[]> bags,
            IEnumerable<(int Count, string Bag)> bag,
            int count = 1
        ) =>
            count +
            count * bag.Sum(x => Count(bags, bags[x.Bag], x.Count));

        private static Func<string, IEnumerable<string>> Contains(
            Dictionary<string, (int Count, string Bag)[]> bags
        ) => bag =>
        {
            var directly = bags
                .Where(x => x.Value.Any(y => y.Bag == bag))
                .Select(x => x.Key)
                .ToArray();
            return directly
                .Concat(bags
                    .Select(x => x.Key)
                    .Where(directly.Contains)
                    .SelectMany(Contains(bags))
                )
                .Distinct();
        };

        private static Dictionary<string, (int Count, string Bag)[]> Parse(IEnumerable<string> input) =>
            input
                .ToDictionary(
                    x => x.Substring(0, x.IndexOf("contain")).TrimEnd('s', '.', ' '),
                    x => x
                        .Substring(x.IndexOf("contain") + 8)
                        .Split(",")
                        .Where(y => !y.Contains("no other bags"))
                        .Select(y => y.Trim())
                        .Select(y => (
                            Count: int.Parse(y.Substring(0, y.IndexOf(" "))),
                            Bag: y.Substring(y.IndexOf(" ") + 1).TrimEnd('s', '.', ' ')
                        ))
                        .ToArray()
                );
    }
}
