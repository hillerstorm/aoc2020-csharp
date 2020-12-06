using System;
using System.IO;
using System.Linq;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day06Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/06.txt", 11),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day06.Part1(File.ReadAllText(x.Input).Split("\n\n", StringSplitOptions.RemoveEmptyEntries))));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/06.txt", 6),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day06.Part2(File.ReadAllText(x.Input).Split("\n\n", StringSplitOptions.RemoveEmptyEntries))));
        }
    }
}
