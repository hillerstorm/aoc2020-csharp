using System.IO;
using System.Linq;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day10Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/10_1.txt", 7 * 5),
                ("Inputs/10_2.txt", 22 * 10),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day10.Part1(File.ReadAllText(x.Input).SplitAsInt().ToList())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/10_1.txt", 8L),
                ("Inputs/10_2.txt", 19208L),
            };
            assertions.ForEach(((string Input, long Expected) x) =>
                Assert.Equal(x.Expected, Day10.Part2(File.ReadAllText(x.Input).SplitAsInt().ToList())));
        }
    }
}
