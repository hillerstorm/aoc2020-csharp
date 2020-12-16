using System;
using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day16Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/16.txt", 71),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day16.Part1(File.ReadAllText(x.Input).Split("\n\n", StringSplitOptions.RemoveEmptyEntries))));
        }
    }
}
