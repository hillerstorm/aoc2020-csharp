using System;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day15Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("0,3,6", 436),
                ("1,3,2", 1),
                ("2,1,3", 10),
                ("1,2,3", 27),
                ("2,3,1", 78),
                ("3,2,1", 438),
                ("3,1,2", 1836),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day15.Part1(x.Input.SplitAsInt(",").AsSpan())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("0,3,6", 175594),
                ("1,3,2", 2578),
                ("2,1,3", 3544142),
                ("1,2,3", 261214),
                ("2,3,1", 6895259),
                ("3,2,1", 18),
                ("3,1,2", 362),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day15.Part2(x.Input.SplitAsInt(",").AsSpan())));
        }
    }
}
