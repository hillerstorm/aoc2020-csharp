using System;
using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day22Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/22.txt", 306),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day22.Part1(File.ReadAllText(x.Input).Split("\n\n", StringSplitOptions.RemoveEmptyEntries))));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/22.txt", 291),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day22.Part2(File.ReadAllText(x.Input).Split("\n\n", StringSplitOptions.RemoveEmptyEntries))));
        }
    }
}
