using System;
using System.Linq;

namespace aoc2020.Days
{
    public class Day11 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        private static readonly (int DX, int DY)[] Deltas =
        {
            (-1, -1),
            (-1,  0),
            (-1,  1),
            ( 0, -1),
            ( 0,  1),
            ( 1, -1),
            ( 1,  0),
            ( 1,  1),
        };

        public static int Part1(string input) =>
            Iterate(input, Part.One);

        public static int Part2(string input) =>
            Iterate(input, Part.Two);

        private static int Iterate(string input, Part part)
        {
            var split = input.SplitLines();
            var width = split[0].Length;
            var height = split.Length;
            var current = string.Join("", split);
            var next = current.ToArray();
            var changes = true;
            while (changes)
            {
                current = new string(next);
                changes = false;
                for (var i = 0; i < current.Length; i++)
                {
                    var curr = current[i];
                    if (curr == '.')
                        continue;

                    var count = CountOccupiedNeighbours(current, width, height, i % width, i / width, part);
                    if (curr == '#' && count >= (part == Part.One ? 4 : 5))
                    {
                        next[i] = 'L';
                        changes = true;
                    }
                    else if (curr == 'L' && count == 0)
                    {
                        next[i] = '#';
                        changes = true;
                    }
                }
            }

            return current.Count(n => n == '#');
        }

        private static int CountOccupiedNeighbours(string current, int width, int height, int x, int y, Part part) =>
            Deltas.Count(d => HasNeighbour(current, width, height, x, y, d.DX, d.DY, part));

        private static bool HasNeighbour(
            string current,
            in int width,
            in int height,
            int x,
            int y,
            in int dx,
            in int dy,
            Part part
        )
        {
            while (true)
            {
                x += dx;
                y += dy;
                if (x < 0 || y < 0 || x >= width || y >= height)
                    return false;

                var curr = current[width * y + x];
                if (curr != '.')
                    return curr == '#';

                if (part == Part.Two)
                    continue;

                return false;
            }
        }
    }
}
