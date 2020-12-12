using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day12Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/12.txt", 25),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day12.Part1(File.ReadAllText(x.Input).SplitLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/12.txt", 286L),
            };
            assertions.ForEach(((string Input, long Expected) x) =>
                Assert.Equal(x.Expected, Day12.Part2(File.ReadAllText(x.Input).SplitLines())));
        }
    }
}
