using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day25 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var lines = input.SplitAsInt();
            return (
                () => Part1(lines).ToString(),
                () => string.Empty
            );
        }

        public static long Part1(int[] input)
        {
            var (cardPkey, doorPkey, _) = input;
            var doorLoopSize = GetLoopSize(doorPkey);
            return Transform(doorLoopSize, cardPkey);
        }

        private static int GetLoopSize(in long cardPkey)
        {
            var loopSize = 1;
            const int subjectNumber = 7;
            var previousValue = 1L;
            while (true)
            {
                var val = previousValue * subjectNumber;
                val %= 20201227;

                if (val == cardPkey)
                    return loopSize;

                previousValue = val;
                loopSize++;
            }
        }

        private static long Transform(int loopSize, int subjectNumber)
        {
            var val = 1L;

            for (var i = 0; i < loopSize; i++)
            {
                val *= subjectNumber;
                val %= 20201227;
            }

            return val;
        }
    }
}
