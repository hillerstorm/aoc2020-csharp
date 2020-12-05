using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020.Days
{
    public class Day05 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var parsed = input.SplitLines().Select(x => x.ToArray());
            return (
                () => Part1(parsed).ToString(),
                () => Part2(parsed).ToString()
            );
        }

        public static int Part1(IEnumerable<char[]> input) =>
            ListSeats(input).Last();

        private static int Part2(IEnumerable<char[]> input)
        {
            var seats = ListSeats(input);
            foreach (var seat in seats)
                if (!seats.Contains(seat + 1) && seats.Contains(seat + 2))
                    return seat + 1;

            return -1;
        }

        private static List<int> ListSeats(IEnumerable<char[]> input)
        {
            var seats = new List<int>();
            foreach (var pass in input)
            {
                int rowMin = 0, colMin = 0;
                int rowMax = 127, colMax = 7;
                foreach (var chr in pass)
                    switch (chr)
                    {
                        case 'F':
                            rowMax -= (rowMax - rowMin + 1) / 2;
                            break;
                        case 'B':
                            rowMin += (rowMax - rowMin + 1) / 2;
                            break;
                        case 'L':
                            colMax -= (colMax - colMin + 1) / 2;
                            break;
                        case 'R':
                            colMin += (colMax - colMin + 1) / 2;
                            break;
                    }

                seats.Add(rowMin * 8 + colMin);
            }

            seats.Sort();
            return seats;
        }
    }
}
