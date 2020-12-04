using System;
using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day04Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/04_1.txt", 2),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day04.Part1(File.ReadAllText(x.Input).Split("\n\n", StringSplitOptions.RemoveEmptyEntries))));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/04_2.txt", 0),
                ("Inputs/04_3.txt", 4),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day04.Part2(File.ReadAllText(x.Input).Split("\n\n", StringSplitOptions.RemoveEmptyEntries))));
        }
    }
}
