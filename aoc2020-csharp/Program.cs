using System;
using System.Collections.Generic;
using aoc2020;
using aoc2020.Days;

if (args.Length == 0)
{
    Console.WriteLine("No day given");
    return;
}

var days = new Dictionary<int, Action<string>>
{
    {1, Day01.Run}
};

var day = int.Parse(args[0]);
var (input, error) = await InputFetcher.GetInput(day);
if (string.IsNullOrWhiteSpace(error))
    days[day](input);
else
    Console.WriteLine(error);
