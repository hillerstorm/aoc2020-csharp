using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day11Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/11.txt", 37),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day11.Part1(File.ReadAllText(x.Input))));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/11.txt", 26),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day11.Part2(File.ReadAllText(x.Input))));
        }
    }
}
