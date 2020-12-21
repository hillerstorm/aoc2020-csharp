using System;
using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day20Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/20.txt", 20899048083289),
            };
            assertions.ForEach(((string Input, long Expected) x) =>
                Assert.Equal(x.Expected, Day20.Part1(File.ReadAllText(x.Input).Split("\n\n", StringSplitOptions.RemoveEmptyEntries))));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/20.txt", 273L),
            };
            assertions.ForEach(((string Input, long Expected) x) =>
                Assert.Equal(x.Expected, Day20.Part2(File.ReadAllText(x.Input).Split("\n\n", StringSplitOptions.RemoveEmptyEntries))));
        }
    }
}
