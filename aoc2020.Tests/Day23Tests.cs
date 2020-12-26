using System.Linq;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day23Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("389125467", 67384529),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day23.Part1(x.Input.Select(y => (int)y - 48).ToArray())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("389125467", 149245887792L),
            };
            assertions.ForEach(((string Input, long Expected) x) =>
                Assert.Equal(x.Expected, Day23.Part2(x.Input.Select(y => (int)y - 48).ToArray())));
        }
    }
}
