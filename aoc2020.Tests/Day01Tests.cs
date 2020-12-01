using System.IO;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day01Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/01.txt", 514579),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day01.Part1(File.ReadAllText(x.Input).SplitAsInt())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/01.txt", 241861950),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day01.Part2(File.ReadAllText(x.Input).SplitAsInt())));
        }
    }
}
