using System;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery

namespace aoc2020.Days
{
    public class Day17 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        private static readonly (int DX, int DY, int DZ)[] P1Deltas =
            Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .Where(z => !(x == 0 && y == 0 && z == 0))
                        .Select(z => (x, y, z))
                    )
                )
                .ToArray();

        private static readonly (int DX, int DY, int DZ, int DW)[] P2Deltas =
            Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .SelectMany(z => Enumerable.Range(-1, 3)
                            .Where(w => !(x == 0 && y == 0 && z == 0 && w == 0))
                            .Select(w => (x, y, z, w))
                        )
                    )
                )
                .ToArray();

        private const int Size = 99;
        private const int Mid = 50;

        public static int Part1(string input)
        {
            var current = new bool[Size * Size * Size];

            var width = input.IndexOf('\n');
            var height = input.Count(x => x == '\n');
            var midX = Mid - width / 2;
            var midY = Mid - height / 2;
            var xMin = Mid;
            var xMax = Mid;
            var yMin = Mid;
            var yMax = Mid;
            var zMin = Mid - 1;
            var zMax = Mid + 1;

            var inputArr = input.Replace("\n", string.Empty).ToCharArray();

            for (var i = 0; i < inputArr.Length; i++)
            {
                var x = i % width + midX;
                var y = i / width + midY;

                if (x <= xMin)
                    xMin = x - 1;
                else if (x >= xMax)
                    xMax = x + 1;

                if (y <= yMin)
                    yMin = x - 1;
                else if (y >= yMax)
                    yMax = x + 1;

                current[Mid * Size * Size + y * Size + x] = inputArr[i] == '#';
            }

            var next = current.ToArray();

            for (var t = 0; t < 6; t++)
            {
                next.CopyTo(current.AsSpan());

                var incXMin = false;
                var incXMax = false;
                var incYMin = false;
                var incYMax = false;
                var incZMin = false;
                var incZMax = false;

                for (var i = 0; i < current.Length; i++)
                {
                    var curr = current[i];

                    var iX = i;

                    var z = iX / (Size * Size);
                    if (z < zMin || z > zMax)
                        continue;

                    iX -= z * Size * Size;

                    var x = iX % Size;
                    if (x < xMin || x > xMax)
                        continue;

                    var y = iX / Size;
                    if (y < yMin || y > yMax)
                        continue;

                    var count = 0;
                    foreach (var (dx, dy, dz) in P1Deltas)
                    {
                        var idx = (z + dz) * Size * Size + (y + dy) * Size + x + dx;
                        if (current[idx])
                            count++;
                    }

                    var changes = false;
                    if (!curr && count == 3)
                    {
                        next[i] = true;
                        changes = true;
                    }
                    else if (curr && (count < 2 || count > 3))
                    {
                        next[i] = false;
                        changes = true;
                    }

                    if (!changes)
                        continue;

                    if (x == xMin) incXMin = true;
                    else if (x == xMax) incXMax = true;
                    if (y == yMin) incYMin = true;
                    else if (y == yMax) incYMax = true;
                    if (z == zMin) incZMin = true;
                    else if (z == zMax) incZMax = true;
                }

                if (incXMin) xMin -= 1;
                if (incXMax) xMax += 1;
                if (incYMin) yMin -= 1;
                if (incYMax) yMax += 1;
                if (incZMin) zMin -= 1;
                if (incZMax) zMax += 1;
            }

            return next.Count(n => n);
        }

        public static int Part2(string input)
        {
            var current = new bool[Size * Size * Size * Size];

            var width = input.IndexOf('\n');
            var height = input.Count(x => x == '\n');
            var midX = Mid - width / 2;
            var midY = Mid - height / 2;
            var xMin = Mid;
            var xMax = Mid;
            var yMin = Mid;
            var yMax = Mid;
            var zMin = Mid - 1;
            var zMax = Mid + 1;
            var wMin = Mid - 1;
            var wMax = Mid + 1;

            var inputArr = input.Replace("\n", string.Empty).ToCharArray();

            for (var i = 0; i < inputArr.Length; i++)
            {
                var x = i % width + midX;
                var y = i / width + midY;

                if (x <= xMin)
                    xMin = x - 1;
                else if (x >= xMax)
                    xMax = x + 1;

                if (y <= yMin)
                    yMin = x - 1;
                else if (y >= yMax)
                    yMax = x + 1;

                current[Mid * Size * Size * Size + Mid * Size * Size + y * Size + x] = inputArr[i] == '#';
            }

            var next = current.ToArray();

            for (var t = 0; t < 6; t++)
            {
                next.CopyTo(current.AsSpan());

                var incXMin = false;
                var incXMax = false;
                var incYMin = false;
                var incYMax = false;
                var incZMin = false;
                var incZMax = false;
                var incWMin = false;
                var incWMax = false;

                for (var i = 0; i < current.Length; i++)
                {
                    var curr = current[i];

                    var iX = i;

                    var w = iX / (Size * Size * Size);
                    if (w < wMin || w > wMax)
                        continue;

                    iX -= w * Size * Size * Size;

                    var z = iX / (Size * Size);
                    if (z < zMin || z > zMax)
                        continue;

                    iX -= z * Size * Size;

                    var x = iX % Size;
                    if (x < xMin || x > xMax)
                        continue;

                    var y = iX / Size;
                    if (y < yMin || y > yMax)
                        continue;

                    var count = 0;
                    foreach (var (dx, dy, dz, dw) in P2Deltas)
                    {
                        var idx = (w + dw) * Size * Size * Size + (z + dz) * Size * Size + (y + dy) * Size + x + dx;
                        if (current[idx])
                            count++;
                    }

                    var changes = false;
                    if (!curr && count == 3)
                    {
                        next[i] = true;
                        changes = true;
                    }
                    else if (curr && (count < 2 || count > 3))
                    {
                        next[i] = false;
                        changes = true;
                    }

                    if (!changes)
                        continue;

                    if (x == xMin) incXMin = true;
                    else if (x == xMax) incXMax = true;
                    if (y == yMin) incYMin = true;
                    else if (y == yMax) incYMax = true;
                    if (z == zMin) incZMin = true;
                    else if (z == zMax) incZMax = true;
                    if (w == wMin) incWMin = true;
                    else if (w == wMax) incWMax = true;
                }

                if (incXMin) xMin -= 1;
                if (incXMax) xMax += 1;
                if (incYMin) yMin -= 1;
                if (incYMax) yMax += 1;
                if (incZMin) zMin -= 1;
                if (incZMax) zMax += 1;
                if (incWMin) wMin -= 1;
                if (incWMax) wMax += 1;
            }

            return next.Count(n => n);
        }
    }
}
