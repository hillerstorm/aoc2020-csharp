using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Nito.Collections;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

namespace aoc2020
{
    public static class Extensions
    {
        public static IEnumerable<string> SplitLines(this string input) =>
            input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        public static IEnumerable<int> SplitAsInt(this string input, string separator = "\n") =>
            input.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

        public static async Task<(string? Input, string? Error)> GetInput(this int day)
        {
            var inputPath = $"Inputs/{day:00}.txt";
            if (File.Exists(inputPath))
                return (await File.ReadAllTextAsync(inputPath), null);

            // Download
            Console.WriteLine("Input file not found, downloading...");
            if (!File.Exists(".session"))
                return (null, "No .session file found, save value from Cookie header on your aoc page");

            var session = await File.ReadAllTextAsync(".session");
            if (string.IsNullOrWhiteSpace(session))
                return (null, ".session file found but is empty, save value from Cookie header on your aoc page");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cookie", $"session={session}");
            var result = await client.GetStringAsync($"https://adventofcode.com/2020/day/{day}/input");

            Console.WriteLine("Downloaded input, saving to disk");
            await File.WriteAllTextAsync(inputPath, result);

            return (result, null);
        }

        public static IEnumerable<T> Cyclic<T>(this IEnumerable<T> source)
        {
            var arr = source.ToArray();
            var i = 0;
            while (true)
            {
                yield return arr[i % arr.Length];
                i++;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public static IEnumerable<(int X, int Y)> Square(int x, int y, int width, int height) =>
            x.To(width).Pairs(y.To(height));

        public static IEnumerable<(T1 A, T2 B)> Pairs<T1, T2>(this IEnumerable<T1> source, IEnumerable<T2> other) =>
            source.SelectMany(x => other.Select(y => (x, y)));

        public static IEnumerable<int> To(this int from, int max) =>
            Enumerable.Range(from, max);

        public static void Rotate<T>(this Deque<T> source, long offset)
        {
            if (offset < 0)
            {
                offset = Math.Abs(offset);
                for (long i = 0; i < offset; i++)
                    source.AddToFront(source.RemoveFromBack());
            }
            else if (offset > 0)
                for (long i = 0; i < offset; i++)
                    source.AddToBack(source.RemoveFromFront());
        }

        public static IEnumerable<IEnumerable<T1>> PartitionBy<T1>(this IEnumerable<T1> source, int width)
        {
            var entries = source.LongCount() / width;
            for (var i = 0; i < entries; i++)
                yield return source.Skip(i * width).Take(width);
        }
    }
}
