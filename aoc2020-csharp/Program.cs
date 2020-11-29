using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using aoc2020;
using aoc2020.Days;

if (args.Length == 0)
{
    Console.WriteLine("No day given");
    return;
}

var days = new IDay[]
{
    new Day01(),
};

if (!int.TryParse(args[0], out var day) || day <= 0 || day > days.Length)
{
    Console.WriteLine($"Invalid input, must be a day between 1-{days.Length}");
    return;
}

var info = TimeZoneInfo.FindSystemTimeZoneById(
    RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        ? "Eastern Standard Time"
        : "America/New_York"
);
var now = new DateTimeOffset(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, info), info.BaseUtcOffset);
var then = new DateTimeOffset(2020, 12, day, 0, 0, 0, info.BaseUtcOffset);
if (now < then)
{
    Console.WriteLine($"Day {day} can't be started yet, {then - now:d\\:hh\\:mm\\:ss} left");
    return;
}


var (input, error) = await day.GetInput();
if (!string.IsNullOrWhiteSpace(error))
{
    Console.WriteLine(error);
    return;
}
else if (string.IsNullOrWhiteSpace(input))
{
    Console.WriteLine("Empty input");
    return;
}

var (p1, p2) = days[day - 1].Parts(input);

var sw = new Stopwatch();
sw.Start();
var part1 = p1();
sw.Stop();
Console.WriteLine($"Part 1 took {sw.Elapsed:g}");
Console.WriteLine(part1);

sw.Restart();
var part2 = p2();
sw.Stop();
Console.WriteLine($"Part 2 took {sw.Elapsed:g}");
Console.WriteLine(part2);
