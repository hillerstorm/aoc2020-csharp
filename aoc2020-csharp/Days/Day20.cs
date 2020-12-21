using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable InconsistentNaming

namespace aoc2020.Days
{
    public class Day20 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var tiles = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            return (
                () => Part1(tiles).ToString(),
                () => Part2(tiles).ToString()
            );
        }

        private const int TileSize = 10;

        public static long Part1(IEnumerable<string> input)
        {
            var tiles = ParseTiles(input);

            var sum = 1L;
            foreach (var (id, tile) in tiles)
            {
                var edges = new[]
                {
                    tiles.FirstOrDefault(x => x.Key != id && Tile.GetMatchingEdge(tile.Top, x.Value) != default).Key,
                    tiles.FirstOrDefault(x => x.Key != id && Tile.GetMatchingEdge(tile.Bottom, x.Value) != default).Key,
                    tiles.FirstOrDefault(x => x.Key != id && Tile.GetMatchingEdge(tile.Left, x.Value) != default).Key,
                    tiles.FirstOrDefault(x => x.Key != id && Tile.GetMatchingEdge(tile.Right, x.Value) != default).Key,
                }.Where(x => x != default).ToArray();

                if (edges.Length == 2)
                    sum *= id;
            }

            return sum;
        }

        public static long Part2(IEnumerable<string> input)
        {
            var tiles = ParseTiles(input);
            var gridSize = (int)Math.Sqrt(tiles.Count);

            FindAllMatchingEdges(tiles);

            var topLeft = tiles.Values.FirstOrDefault(x =>
                x.MatchingBottomEdge != null &&
                x.MatchingRightEdge != null &&
                x.MatchingLeftEdge == null &&
                x.MatchingTopEdge == null
            );
            if (topLeft == null)
            {
                var tile = tiles.Values.First(x =>
                    x.MatchingTopEdge == null &&
                    x.MatchingLeftEdge != null &&
                    x.MatchingRightEdge != null &&
                    x.MatchingBottomEdge != null
                );
                while (tile.MatchingEdges > 2)
                {
                    var sb = new StringBuilder();
                    var next = tile.MatchingLeftEdge!.Value.Tile;
                    RotateToMatchRight(sb, tile, tile.MatchingLeftEdge.Value.Edge, tile.MatchingLeftEdge.Value.Flipped);
                    if (sb.Length > 0)
                        next.Data = sb.ToString();
                    tile = next;
                    FindAllMatchingEdges(tiles);
                }

                topLeft = tile;
            }

            FindAllMatchingEdges(tiles);

            var grid = new List<Tile> {topLeft};
            var last = topLeft;
            while (grid.Count < tiles.Count)
            {
                var sb = new StringBuilder();
                Tile next;
                if (last.MatchingRightEdge == null)
                {
                    // new row
                    last = grid[^gridSize];
                    next = last.MatchingBottomEdge!.Value.Tile;
                    RotateToMatchAbove(sb, next, last.MatchingBottomEdge.Value.Edge, last.MatchingBottomEdge.Value.Flipped);
                }
                else
                {
                    next = last.MatchingRightEdge!.Value.Tile;
                    RotateToMatchLeft(sb, next, last.MatchingRightEdge.Value.Edge, last.MatchingRightEdge.Value.Flipped);
                }

                if (sb.Length > 0)
                {
                    next.Data = sb.ToString();
                    FindAllMatchingEdges(tiles);
                }

                grid.Add(next);
                last = next;
            }

            var finishedGrid = PrintGrid(grid, gridSize);
            var seaMonsterIndices = FindSeaMonsters(finishedGrid);

            return finishedGrid.Where((x, i) => !seaMonsterIndices.Contains(i)).Count(x => x == '#');
        }

        private enum Edge
        {
            Top,
            Bottom,
            Left,
            Right,
        }

        private class Tile
        {
            private string _data;

#pragma warning disable 8618
            public Tile(int id, string data)
#pragma warning restore 8618
            {
                (Id, Data) = (id, data);
                UpdateEdges();
            }

            private int Id { get; }

            public string Data
            {
                get => _data;
                set
                {
                    _data = value;
                    UpdateEdges();
                }
            }

            public string Top { get; private set; }
            public string Bottom { get; private set; }
            public string Left { get; private set; }
            public string Right { get; private set; }

            public (Tile Tile, Edge Edge, bool Flipped)? MatchingTopEdge { get; set; }
            public (Tile Tile, Edge Edge, bool Flipped)? MatchingBottomEdge { get; set; }
            public (Tile Tile, Edge Edge, bool Flipped)? MatchingLeftEdge { get; set; }
            public (Tile Tile, Edge Edge, bool Flipped)? MatchingRightEdge { get; set; }

            public int MatchingEdges =>
                (MatchingTopEdge.HasValue ? 1 : 0) +
                (MatchingBottomEdge.HasValue ? 1 : 0) +
                (MatchingLeftEdge.HasValue ? 1 : 0) +
                (MatchingRightEdge.HasValue ? 1 : 0);

            private void UpdateEdges()
            {
                Top = Data[..TileSize];
                Bottom = Data[^TileSize..];
                Left = new string(Data.PartitionBy(TileSize).Select(x => x.First()).ToArray());
                Right = new string(Data.PartitionBy(TileSize).Select(x => x.Last()).ToArray());
            }

            public void FindMatchingEdges(Dictionary<int, Tile> tiles)
            {
                var top = tiles
                    .Where(x => x.Key != Id)
                    .Select(x => GetMatchingEdge(Top, x.Value))
                    .FirstOrDefault(x => x != default);
                MatchingTopEdge = top == default ? null : top;

                var bottom = tiles
                    .Where(x => x.Key != Id)
                    .Select(x => GetMatchingEdge(Bottom, x.Value))
                    .FirstOrDefault(x => x != default);
                MatchingBottomEdge = bottom == default ? null : bottom;

                var left = tiles
                    .Where(x => x.Key != Id)
                    .Select(x => GetMatchingEdge(Left, x.Value))
                    .FirstOrDefault(x => x != default);
                MatchingLeftEdge = left == default ? null : left;

                var right = tiles
                    .Where(x => x.Key != Id)
                    .Select(x => GetMatchingEdge(Right, x.Value))
                    .FirstOrDefault(x => x != default);
                MatchingRightEdge = right == default ? null : right;
            }

            public static (Tile Tile, Edge edge, bool Flipped) GetMatchingEdge(string edge, Tile tile) =>
                new[]
                    {
                        (tile.Top, Edge.Top, false),
                        (tile.Bottom, Edge.Bottom, false),
                        (tile.Left, Edge.Left, false),
                        (tile.Right, Edge.Right, false),
                        (new string(tile.Top.Reverse().ToArray()), Edge.Top, true),
                        (new string(tile.Bottom.Reverse().ToArray()), Edge.Bottom, true),
                        (new string(tile.Left.Reverse().ToArray()), Edge.Left, true),
                        (new string(tile.Right.Reverse().ToArray()), Edge.Right, true),
                    }.Where(x => x.Item1 == edge)
                    .Select(x => (tile, x.Item2, x.Item3))
                    .FirstOrDefault();

            public override string ToString()
            {
                return Id.ToString();
            }
        }

        private static List<int> FindSeaMonsters(string grid)
        {
            var gridSize = (int) Math.Sqrt(grid.Length);
            var seaMonsterMask = new (int DX, int DY)[]
            {
                                                                                                                    (18, -1),
                (0, 0),                (5, 0), (6, 0),                  (11, 0), (12, 0),                  (17, 0), (18,  0), (19, 0),
                        (1, 1), (4, 1),                (7, 1), (10, 1),                   (13, 1), (16, 1)
            };
            var seaMonsterDeltas = new (int DX, int DY)[][]
            {
                // Left -> right
                seaMonsterMask,
                // Right -> Left
                seaMonsterMask.Select(d => (-d.DX, d.DY)).ToArray(),
                // Left -> Right (flipped)
                seaMonsterMask.Select(d => (d.DX, -d.DY)).ToArray(),
                // Right -> Left (flipped)
                seaMonsterMask.Select(d => (-d.DX, -d.DY)).ToArray(),

                // Top -> Down
                seaMonsterMask.Select(d => (-d.DY, d.DX)).ToArray(),
                // Bottom -> Up
                seaMonsterMask.Select(d => (-d.DY, -d.DX)).ToArray(),
                // Top -> Down (flipped)
                seaMonsterMask.Select(d => (d.DY, d.DX)).ToArray(),
                // Bottom -> Up (flipped)
                seaMonsterMask.Select(d => (d.DY, -d.DX)).ToArray(),
            };

            var indices = new List<int>();
            for (var y = 0; y < gridSize; y++)
            for (var x = 0; x < gridSize; x++)
            {
                var idx = y * gridSize + x;
                if (grid[idx] != '#')
                    continue;

                foreach (var delta in seaMonsterDeltas)
                {
                    var allMatches = true;
                    var deltaIndices = delta.Select(d => (y + d.DY) * gridSize + x + d.DX).ToArray();
                    foreach (var (dx, dy) in delta)
                    {
                        var newX = x + dx;
                        var newY = y + dy;
                        if (newX < 0 || newX >= gridSize || newY < 0 || newY >= gridSize)
                        {
                            allMatches = false;
                            break;
                        }

                        var newIdx = newY * gridSize + newX;
                        if (newIdx >= grid.Length || grid[newIdx] != '#')
                            allMatches = false;
                    }

                    if (allMatches)
                        indices.AddRange(deltaIndices);
                }
            }

            return indices;
        }

        private static void FindAllMatchingEdges(Dictionary<int, Tile> tiles)
        {
            foreach (var (_, tile) in tiles)
                tile.FindMatchingEdges(tiles);
        }

        private static void RotateToMatchRight(StringBuilder sb, Tile tile, Edge edge, bool flipped)
        {
            switch (edge)
            {
                case Edge.Top when flipped:
                    for (var x = TileSize - 1; x >= 0; x--)
                    for (var y = TileSize - 1; y >= 0; y--)
                        sb.Append(tile.Data[y * TileSize + x]);
                    UpdateMatchingEdges(tile,
                        tile.MatchingRightEdge, tile.MatchingLeftEdge,
                        tile.MatchingBottomEdge, tile.MatchingTopEdge
                    );
                    break;
                case Edge.Top:
                    for (var x = 0; x < TileSize; x++)
                    for (var y = TileSize - 1; y >= 0; y--)
                        sb.Append(tile.Data[y * TileSize + x]);
                    UpdateMatchingEdges(tile,
                        tile.MatchingLeftEdge, tile.MatchingRightEdge,
                        tile.MatchingBottomEdge, tile.MatchingTopEdge
                    );
                    break;
                case Edge.Bottom when flipped:
                    for (var x = TileSize - 1; x >= 0; x--)
                    for (var y = 0; y < TileSize; y++)
                        sb.Append(tile.Data[y * TileSize + x]);
                    UpdateMatchingEdges(tile,
                        tile.MatchingRightEdge, tile.MatchingLeftEdge,
                        tile.MatchingTopEdge, tile.MatchingBottomEdge
                    );
                    break;
                case Edge.Bottom:
                    for (var x = 0; x < TileSize; x++)
                    for (var y = 0; y < TileSize; y++)
                        sb.Append(tile.Data[y * TileSize + x]);
                    UpdateMatchingEdges(tile,
                        tile.MatchingLeftEdge, tile.MatchingRightEdge,
                        tile.MatchingTopEdge, tile.MatchingBottomEdge
                    );
                    break;
                case Edge.Left when flipped:
                    for (var y = TileSize - 1; y >= 0; y--)
                    for (var x = TileSize - 1; x >= 0; x--)
                        sb.Append(tile.Data[y * TileSize + x]);
                    UpdateMatchingEdges(tile,
                        tile.MatchingBottomEdge, tile.MatchingTopEdge,
                        tile.MatchingRightEdge, tile.MatchingLeftEdge
                    );
                    break;
                case Edge.Left:
                    for (var y = 0; y < TileSize; y++)
                    for (var x = TileSize - 1; x >= 0; x--)
                        sb.Append(tile.Data[y * TileSize + x]);
                    UpdateMatchingEdges(tile,
                        tile.MatchingTopEdge, tile.MatchingBottomEdge,
                        tile.MatchingRightEdge, tile.MatchingLeftEdge
                    );
                    break;
                case Edge.Right when flipped:
                    for (var y = TileSize - 1; y >= 0; y--)
                    for (var x = 0; x < TileSize; x++)
                        sb.Append(tile.Data[y * TileSize + x]);
                    UpdateMatchingEdges(tile,
                        tile.MatchingBottomEdge, tile.MatchingTopEdge,
                        tile.MatchingLeftEdge, tile.MatchingRightEdge
                    );
                    break;
            }
        }

        private static void RotateToMatchAbove(StringBuilder sb, Tile next, Edge edge, bool flipped)
        {
            switch (edge)
            {
                case Edge.Top when flipped:
                    for (var y = 0; y < TileSize; y += TileSize)
                    for (var x = TileSize - 1; x >= 0; x--)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingTopEdge, next.MatchingBottomEdge,
                        next.MatchingRightEdge, next.MatchingLeftEdge
                    );
                    break;
                case Edge.Bottom when flipped:
                    for (var y = TileSize - 1; y >= 0; y--)
                    for (var x = TileSize - 1; x >= 0; x--)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingBottomEdge, next.MatchingTopEdge,
                        next.MatchingRightEdge, next.MatchingLeftEdge
                    );
                    break;
                case Edge.Bottom:
                    for (var y = TileSize - 1; y >= 0; y--)
                    for (var x = 0; x < TileSize; x++)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingBottomEdge, next.MatchingTopEdge,
                        next.MatchingLeftEdge, next.MatchingRightEdge
                    );
                    break;
                case Edge.Left when flipped:
                    for (var x = 0; x < TileSize; x++)
                    for (var y = TileSize - 1; y >= 0; y--)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingLeftEdge, next.MatchingRightEdge,
                        next.MatchingBottomEdge, next.MatchingTopEdge
                    );
                    break;
                case Edge.Left:
                    for (var x = 0; x < TileSize; x++)
                    for (var y = 0; y < TileSize; y++)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingLeftEdge, next.MatchingRightEdge,
                        next.MatchingTopEdge, next.MatchingBottomEdge
                    );
                    break;
                case Edge.Right when flipped:
                    for (var x = TileSize - 1; x >= 0; x--)
                    for (var y = TileSize - 1; y >= 0; y--)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingRightEdge, next.MatchingLeftEdge,
                        next.MatchingBottomEdge, next.MatchingTopEdge
                    );
                    break;
                case Edge.Right:
                    for (var x = TileSize - 1; x >= 0; x--)
                    for (var y = 0; y < TileSize; y++)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingRightEdge, next.MatchingLeftEdge,
                        next.MatchingTopEdge, next.MatchingBottomEdge
                    );
                    break;
            }
        }

        private static void RotateToMatchLeft(StringBuilder sb, Tile next, Edge edge, bool flipped)
        {
            switch (edge)
            {
                case Edge.Top when flipped:
                    for (var x = TileSize - 1; x >= 0; x--)
                    for (var y = 0; y < TileSize; y += TileSize)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingRightEdge, next.MatchingLeftEdge,
                        next.MatchingTopEdge, next.MatchingBottomEdge
                    );
                    break;
                case Edge.Top:
                    for (var x = 0; x < TileSize; x++)
                    for (var y = 0; y < TileSize; y++)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingLeftEdge, next.MatchingRightEdge,
                        next.MatchingTopEdge, next.MatchingBottomEdge
                    );
                    break;
                case Edge.Bottom when flipped:
                    for (var x = TileSize - 1; x >= 0; x--)
                    for (var y = TileSize - 1; y >= 0; y--)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingRightEdge, next.MatchingLeftEdge,
                        next.MatchingBottomEdge, next.MatchingTopEdge
                    );
                    break;
                case Edge.Bottom:
                    for (var x = 0; x < TileSize; x++)
                    for (var y = TileSize - 1; y >= 0; y--)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingLeftEdge, next.MatchingRightEdge,
                        next.MatchingBottomEdge, next.MatchingTopEdge
                    );
                    break;
                case Edge.Left when flipped:
                    for (var y = TileSize - 1; y >= 0; y--)
                    for (var x = 0; x < TileSize; x++)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingBottomEdge, next.MatchingTopEdge,
                        next.MatchingLeftEdge, next.MatchingRightEdge
                    );
                    break;
                case Edge.Right when flipped:
                    for (var y = TileSize - 1; y >= 0; y--)
                    for (var x = TileSize - 1; x >= 0; x--)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingBottomEdge, next.MatchingTopEdge,
                        next.MatchingRightEdge, next.MatchingLeftEdge
                    );
                    break;
                case Edge.Right:
                    for (var y = 0; y < TileSize; y++)
                    for (var x = TileSize - 1; x >= 0; x--)
                        sb.Append(next.Data[y * TileSize + x]);
                    UpdateMatchingEdges(next,
                        next.MatchingTopEdge, next.MatchingBottomEdge,
                        next.MatchingRightEdge, next.MatchingLeftEdge
                    );
                    break;
            }
        }

        private static string PrintGrid(IEnumerable<Tile> grid, int gridSize, bool print = false)
        {
            if (print)
                Console.Write(Environment.NewLine);

            var sb = new StringBuilder();
            foreach (var row in grid.PartitionBy(gridSize).Select(x => x.ToArray()))
            {
                for (var y = 1; y < TileSize - 1; y++)
                {
                    foreach (var tile in row)
                        for (var x = 1; x < TileSize - 1; x++)
                        {
                            var chr = tile.Data[y * TileSize + x];
                            sb.Append(chr);
                            if (print)
                                Console.Write(chr);
                        }

                    if (print)
                        Console.Write(Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        private static void UpdateMatchingEdges(
            Tile tile,
            (Tile Tile, Edge Edge, bool Flipped)? top,
            (Tile Tile, Edge Edge, bool Flipped)? bottom,
            (Tile Tile, Edge Edge, bool Flipped)? left,
            (Tile Tile, Edge Edge, bool Flipped)? right
        ) =>
            (tile.MatchingTopEdge, tile.MatchingBottomEdge, tile.MatchingLeftEdge, tile.MatchingRightEdge) =
            (top, bottom, left, right);

        private static Dictionary<int, Tile> ParseTiles(IEnumerable<string> tiles) =>
            tiles.ToDictionary(
                x => int.Parse(x[(x.IndexOf(' ') + 1)..x.IndexOf(':')]),
                x => ParseEdges(string.Join(string.Empty, x.SplitLines()))
            );

        private static Tile ParseEdges(string tile) =>
            new(
                int.Parse(tile[(tile.IndexOf(' ') + 1)..tile.IndexOf(':')]),
                tile[(tile.IndexOf(':') + 1)..]
            );
    }
}
