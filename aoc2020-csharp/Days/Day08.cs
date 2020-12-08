using System;
using System.Collections.Generic;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace aoc2020.Days
{
    public class Day08 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var lines = input.SplitLines();
            return (
                () => Part1(lines).ToString(),
                () => Part2(lines).ToString()
            );
        }

        public static int Part1(string[] input) =>
            Run(input)!.Value;

        public static int Part2(string[] input)
        {
            var changed = new List<int>();
            while (true)
            {
                var res = Run(input, changed);
                if (res.HasValue)
                    return res.Value;
            }
        }

        private static int? Run(IReadOnlyList<string> input, ICollection<int>? changed = null)
        {
            var acc = 0;
            int? changedIndex = null;
            var seen = new List<int>();
            for (var i = 0; i < input.Count;)
            {
                if (seen.Contains(i))
                    return changed != null ? null : acc;

                seen.Add(i);
                var (op, value, _) = input[i].Split(" ");
                switch (op)
                {
                    case "acc":
                        acc += int.Parse(value);
                        break;
                    case "jmp" or "nop" when changed != null && !changedIndex.HasValue && !changed.Contains(i):
                        changed.Add(i);
                        changedIndex = i;
                        if (op == "nop")
                        {
                            i += int.Parse(value);
                            continue;
                        }

                        break;
                    case "jmp":
                        i += int.Parse(value);
                        continue;
                }

                i++;
            }

            return acc;
        }
    }
}
