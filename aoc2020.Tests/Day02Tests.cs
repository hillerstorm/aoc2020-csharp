using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day02Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/02.txt", 2),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day02.Part1(File.ReadAllText(x.Input).SplitLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/02.txt", 1),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day02.Part2(File.ReadAllText(x.Input).SplitLines())));
        }
    }
}
