using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using aoc2020;

var iDay = typeof(IDay);
Type[] days = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(x => x.GetTypes())
    .Where(x => x.IsClass && iDay.IsAssignableFrom(x))
    .ToArray();

var info = TimeZoneInfo.FindSystemTimeZoneById(
    RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        ? "Eastern Standard Time"
        : "America/New_York"
);

int day;
var now = new DateTimeOffset(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, info), info.BaseUtcOffset);
if (args.Length == 0)
{
    Console.WriteLine("No day given, trying latest");
    if (now > new DateTimeOffset(2020, 12, 26, 0, 0, 0, info.BaseUtcOffset))
        day = 25;
    else
        day = now.Day;
}
else if (!int.TryParse(args[0], out day) || day <= 0 || day > days.Length)
{
    Console.WriteLine($"Invalid input, must be a day between 1-{days.Length}");
    return;
}

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

var (p1, p2) = Expression.Lambda<Func<IDay>>(
    Expression.New(days[day - 1].GetConstructor(Type.EmptyTypes)!)
).Compile()().Parts(input);

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
