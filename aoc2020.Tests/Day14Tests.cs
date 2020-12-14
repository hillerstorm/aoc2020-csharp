using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day14Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/14_1.txt", 165ul),
            };
            assertions.ForEach(((string Input, ulong Expected) x) =>
                Assert.Equal(x.Expected, Day14.Part1(File.ReadAllText(x.Input).SplitLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/14_2.txt", 208ul),
            };
            assertions.ForEach(((string Input, ulong Expected) x) =>
                Assert.Equal(x.Expected, Day14.Part2(File.ReadAllText(x.Input).SplitLines())));
        }
    }
}
