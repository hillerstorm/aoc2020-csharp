using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day07Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/07_1.txt", 4),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day07.Part1(File.ReadAllText(x.Input).SplitLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/07_1.txt", 32),
                ("Inputs/07_2.txt", 126),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day07.Part2(File.ReadAllText(x.Input).SplitLines())));
        }
    }
}
