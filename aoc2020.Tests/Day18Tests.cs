using aoc2020.Days;
using Xunit;

namespace aoc2020.Tests
{
    public class Day18Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("1 + 2 * 3 + 4 * 5 + 6",                              71L),
                ("1 + (2 * 3) + (4 * (5 + 6))",                        51L),
                ("2 * 3 + (4 * 5)",                                    26L),
                ("5 + (8 * 3 + 9 + 3 * 4 * 3)",                       437L),
                ("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",       12240L),
                ("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632L),
            };
            assertions.ForEach(((string Input, long Expected) x) =>
                Assert.Equal(x.Expected, Day18.Part1(new[] {x.Input})));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("1 + 2 * 3 + 4 * 5 + 6",                             231L),
                ("1 + (2 * 3) + (4 * (5 + 6))",                        51L),
                ("2 * 3 + (4 * 5)",                                    46L),
                ("5 + (8 * 3 + 9 + 3 * 4 * 3)",                      1445L),
                ("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",      669060L),
                ("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340L),
            };
            assertions.ForEach(((string Input, long Expected) x) =>
                Assert.Equal(x.Expected, Day18.Part2(new[] {x.Input})));
        }
    }
}
