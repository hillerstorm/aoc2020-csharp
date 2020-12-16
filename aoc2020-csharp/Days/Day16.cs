using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day16 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var parts = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            return (
                () => Part1(parts).ToString(),
                () => Part2(parts).ToString()
            );
        }

        public static int Part1(string[] input) =>
            ParseTickets(input[2], ParseRules(input[0]))
                .Aggregate(0, (a, b) => a + b.invalidRules.Sum());

        public static long Part2(string[] input)
        {
            var rules = ParseRules(input[0]);
            var validTickets = ParseTickets(input[2], rules)
                .Where(x => x.invalidRules.Count == 0)
                .Select(x => x.validRules)
                .ToArray();
            var myTicket = input[1].SplitLines()[1]
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var fields = new Dictionary<int, string>();
            while (fields.Count < rules.Length)
            {
                for (var i = 0; i < myTicket.Length; i++)
                {
                    if (fields.ContainsKey(i))
                        continue;

                    var nearbyFields = validTickets.Select(x => x[i]).ToArray();
                    var matchingRules = rules
                        .Where(r =>
                            !fields.Values.Contains(r.Name) &&
                            nearbyFields.All(f => r.Ranges.Any(x => x.Start.Value <= f && x.End.Value >= f)))
                        .ToArray();
                    if (matchingRules.Length == 1)
                        fields.Add(i, matchingRules[0].Name);
                }
            }

            return fields
                .Where(x => x.Value.StartsWith("departure"))
                .Select(x => myTicket[x.Key])
                .Aggregate(1L, (a, b) => a * b);
        }

        private static (string Name, Range[] Ranges)[] ParseRules(string rules) =>
            rules
                .SplitLines()
                .Select(x => x.Split(":"))
                .Select(x => (
                    Name: x[0],
                    Ranges: x[1].Trim()
                        .Split(" or ")
                        .Select(y => y.Split("-").Select(int.Parse).ToArray())
                        .Select(y => new Range(y[0], y[1]))
                        .ToArray()
                ))
                .ToArray();

        private static IEnumerable<(List<int> validRules, List<int> invalidRules)> ParseTickets(
            string ticketsString,
            (string Name, Range[] Ranges)[] rules
        ) =>
            ticketsString.SplitLines()[1..]
                .Select(x =>
                {
                    var validRules = new List<int>();
                    var invalidRules = new List<int>();
                    var ticketRules = x.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

                    foreach (var ticketRule in ticketRules)
                        if (rules.Any(y => y.Ranges.Any(z => z.Start.Value <= ticketRule && z.End.Value >= ticketRule)))
                            validRules.Add(ticketRule);
                        else
                            invalidRules.Add(ticketRule);

                    return (validRules, invalidRules);
                });
    }
}
