using System;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day13 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var lines = input.SplitLines();
            return (
                () => Part1(lines).ToString(),
                () => Part2(lines[1]).ToString()
            );
        }

        public static int Part1(string[] input)
        {
            var schedule = ParseSchedule(input[1]);
            var earliest = int.Parse(input[0]);
            for (var i = earliest;; i++)
                foreach (var (id, _) in schedule)
                    if (i % id == 0)
                        return id * (i - earliest);
        }

        public static long Part2(string input)
        {
            var schedule = ParseSchedule(input);
            long t = 0;
            long product = 1;
            foreach (var (id, index) in schedule)
            {
                // Loop through the bus schedule and add the current running product
                // until the the first valid timestamp is found for the given bus.
                // Once the first timestamp is found, that condition will always be
                // true, since we keep adding that number every iteration when looking
                // for the first valid timestamp for the next bus.
                while ((t + index) % id != 0)
                    t += product;

                product *= id;
            }

            return t;
        }

        private static (int Id, int Index)[] ParseSchedule(string schedule) =>
            schedule
                .Split(",")
                .Select((x, i) => (Known: int.TryParse(x, out var y), Id: y, Index: i))
                .Where(x => x.Known)
                .Select(x => (x.Id, x.Index))
                .ToArray();
    }
}
