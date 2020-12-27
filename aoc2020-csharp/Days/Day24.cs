using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day24 : IDay
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
            GetStartingPosition(input).Count;

        public static int Part2(IEnumerable<string> input)
        {
            var blackTiles = GetStartingPosition(input);

            for (var i = 0; i < 100; i++)
            {
                var toRemove = new HashSet<(int, int)>();
                var toAdd = new HashSet<(int, int)>();
                foreach (var tile in blackTiles)
                {
                    var neighbours = GetNeighbours(tile)
                        .Count(x => blackTiles.Contains(x));
                    if (neighbours == 0 || neighbours > 2)
                        toRemove.Add(tile);
                }

                var whiteTiles = blackTiles
                    .SelectMany(GetNeighbours)
                    .Distinct()
                    .Where(x => !blackTiles.Contains(x));
                foreach (var tile in whiteTiles)
                {
                    var neighbours = GetNeighbours(tile).Count(blackTiles.Contains);
                    if (neighbours == 2)
                        toAdd.Add(tile);
                }

                blackTiles.RemoveWhere(toRemove.Contains);
                blackTiles.UnionWith(toAdd);
            }

            return blackTiles.Count;
        }

        private static (int Q, int R)[] GetNeighbours((int Q, int R) tile) =>
            new[]
            {
                (tile.Q    , tile.R - 1),
                (tile.Q + 1, tile.R - 1),
                (tile.Q - 1, tile.R    ),
                (tile.Q + 1, tile.R    ),
                (tile.Q - 1, tile.R + 1),
                (tile.Q    , tile.R + 1),
            };

        private static IEnumerable<(int Q, int R)[]> ParseLines(IEnumerable<string> input) =>
            input
                .Select(x => x
                    .Replace("se", " 0,1 ")
                    .Replace("sw", " -1,1 ")
                    .Replace("nw", " 0,-1 ")
                    .Replace("ne", " 1,-1 ")
                    .Replace("w", " -1,0 ")
                    .Replace("e", " 1,0 ")
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(y =>
                    {
                        var (a, b, _) = y.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        return (int.Parse(a), int.Parse(b));
                    })
                    .ToArray()
                )
                .ToArray();

        private static HashSet<(int, int)> GetStartingPosition(IEnumerable<string> input)
        {
            var parsed = ParseLines(input);
            var seen = new HashSet<(int, int)>();

            foreach (var deltas in parsed)
            {
                var q = 0;
                var r = 0;

                foreach (var (dq, dr) in deltas)
                {
                    q += dq;
                    r += dr;
                }

                var tile = (q, r);
                if (!seen.Add(tile))
                    seen.Remove(tile);
            }

            return seen;
        }
    }
}
