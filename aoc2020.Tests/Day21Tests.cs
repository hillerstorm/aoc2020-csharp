using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day21Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/21.txt", 5),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day21.Part1(File.ReadAllText(x.Input).SplitLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/21.txt", "mxmxvkd,sqjhc,fvjkl"),
            };
            assertions.ForEach(((string Input, string Expected) x) =>
                Assert.Equal(x.Expected, Day21.Part2(File.ReadAllText(x.Input).SplitLines())));
        }
    }
}
