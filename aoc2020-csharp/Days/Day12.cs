using System;
using System.Collections.Generic;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day12 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var lines = input.SplitLines();
            return (
                () => Part1(lines).ToString(),
                () => Part2(lines).ToString()
            );
        }

        private static readonly Dictionary<char, (int DX, int DY)> Deltas = new()
        {
            {'N', ( 0,  1)},
            {'S', ( 0, -1)},
            {'E', ( 1,  0)},
            {'W', (-1,  0)},
        };

        private static readonly Dictionary<(char Direction, int Amount), char> ClockwiseTurns = new()
        {
            {('E',  90), 'S'},
            {('E', 180), 'W'},
            {('E', 270), 'N'},

            {('S',  90), 'W'},
            {('S', 180), 'N'},
            {('S', 270), 'E'},

            {('W',  90), 'N'},
            {('W', 180), 'E'},
            {('W', 270), 'S'},

            {('N',  90), 'E'},
            {('N', 180), 'S'},
            {('N', 270), 'W'},
        };

        public static int Part1(IEnumerable<string> input)
        {
            var dir = 'E';
            var x = 0;
            var y = 0;

            foreach (var line in input)
            {
                var (direction, amount) = SwapToClockwise(line[0], int.Parse(line[1..]));

                if (direction == 'R')
                    dir = ClockwiseTurns[(dir, amount)];
                else
                {
                    if (direction == 'F')
                        direction = dir;

                    x += Deltas[direction].DX * amount;
                    y += Deltas[direction].DY * amount;
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }

        public static long Part2(IEnumerable<string> input)
        {
            long wX = 10;
            long wY = 1;
            long x = 0;
            long y = 0;

            foreach (var line in input)
            {
                var (direction, amount) = SwapToClockwise(line[0], int.Parse(line[1..]));

                if (direction == 'R')
                    // Rotate waypoint (wX, wY) around ship (x, y)
                    (wX, wY) = amount switch
                    {
                        90  => (  wY - y  + x, -(wX - x) + y),
                        180 => (-(wX - x) + x, -(wY - y) + y),
                        270 => (-(wY - y) + x,   wX - x  + y),
                        _   => (           wX,            wY),
                    };
                else if (direction == 'F')
                {
                    var xDiff = wX - x;
                    var yDiff = wY - y;
                    x += xDiff * amount;
                    y += yDiff * amount;
                    wX = x + xDiff;
                    wY = y + yDiff;
                }
                else
                {
                    wX += Deltas[direction].DX * amount;
                    wY += Deltas[direction].DY * amount;
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }

        private static (char Direction, int Amount) SwapToClockwise(char direction, int amount) =>
            direction == 'L'
                ? (
                    'R',
                    amount switch
                    {
                         90 => 270,
                        270 =>  90,
                          _ => amount,
                    }
                )
                : (direction, amount);
    }
}
