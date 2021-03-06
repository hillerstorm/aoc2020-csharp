﻿using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day03 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var parsed = input.SplitLines();
            return (
                () => Part1(parsed).ToString(),
                () => Part2(parsed).ToString()
            );
        }

        public static int Part1(IEnumerable<string> input) =>
            input.Select((row, i) => row[i * 3 % row.Length] == '#' ? 1 : 0).Sum();

        public static long Part2(string[] input)
        {
            long a = 0;
            long b = 0;
            long c = 0;
            long d = 0;
            long e = 0;
            for (var index = 1; index < input.Length; index++)
            {
                var row = input[index];

                if (row[index % row.Length] == '#')
                    a++;
                if (row[index * 3 % row.Length] == '#')
                    b++;
                if (row[index * 5 % row.Length] == '#')
                    c++;
                if (row[index * 7 % row.Length] == '#')
                    d++;
                if (index % 2 == 0 && row[index / 2 % row.Length] == '#')
                    e++;
            }

            return a * b * c * d * e;
        }
    }
}
