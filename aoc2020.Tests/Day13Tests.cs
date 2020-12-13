using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day13Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("939 7,13,x,x,59,x,31,19", 295),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day13.Part1(x.Input.Split(" "))));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("7,13,x,x,59,x,31,19", 1068781L),
                ("17,x,13,19", 3417L),
                ("67,7,59,61", 754018L),
                ("67,x,7,59,61", 779210L),
                ("67,7,x,59,61", 1261476L),
                ("1789,37,47,1889", 1202161486L),
            };
            assertions.ForEach(((string Input, long Expected) x) =>
                Assert.Equal(x.Expected, Day13.Part2(x.Input)));
        }
    }
}
