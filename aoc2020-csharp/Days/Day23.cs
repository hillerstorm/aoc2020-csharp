using System;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day23 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var parsed = input.Where(char.IsDigit).Select(x => (int) x - 48).ToArray();
            return (
                () => Part1(parsed).ToString(),
                () => Part2(parsed).ToString()
            );
        }

        public static int Part1(int[] input)
        {
            var cups = Play(input.AsSpan());

            var curr = cups[1];
            var str = string.Empty;
            while (curr != 1)
            {
                str += curr.ToString();
                curr = cups[curr];
            }

            return int.Parse(str);
        }

        public static long Part2(int[] input)
        {
            var cups = Play(input.AsSpan(), 10_000_000, 1_000_000);
            return (long)cups[1] * cups[cups[1]];
        }

        private static Span<int> Play(Span<int> span, int iterations = 100, int? padding = null)
        {
            const int lowest = 1;
            padding ??= span.Length;

            var cups = new int[padding.Value + 1].AsSpan();
            for (var i = 1; i < span.Length + 1; i++)
            {
                var idx = span.IndexOf(i);
                if (idx == span.Length - 1)
                    cups[i] = padding > span.Length ? span.Length + 1 : span[0];
                else
                    cups[i] = span[idx + 1];
            }

            if (padding > span.Length)
            {
                for (var i = span.Length + 1; i < padding; i++)
                    cups[i] = i + 1;
                cups[padding.Value] = span[0];
            }

            var current = span[0];
            for (var i = 0; i < iterations; i++)
            {
                var destination = current - 1;
                var pick1 = cups[current];
                var pick2 = cups[pick1];
                var pick3 = cups[pick2];
                while (destination == pick1 || destination == pick2 || destination == pick3 || destination < lowest)
                {
                    destination--;
                    if (destination < lowest)
                        destination = padding.Value;
                }

                var next = cups[pick3];
                cups[pick3] = cups[destination];
                cups[destination] = pick1;
                cups[current] = next;
                current = next;
            }

            return cups;
        }
    }
}
