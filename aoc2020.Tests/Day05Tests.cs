using System.Linq;
using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day05Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("FBFBBFFRLR", 357),
                ("BFFFBBFRRR", 567),
                ("FFFBBBFRRR", 119),
                ("BBFFBBFRLL", 820),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day05.Part1(new[]{x.Input.ToArray()})));
        }
    }
}
