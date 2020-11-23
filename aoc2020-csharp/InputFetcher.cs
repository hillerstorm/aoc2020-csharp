using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace aoc2020
{
    public static class InputFetcher
    {
        public static async Task<(string Input, string Error)> GetInput(int day)
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
    }
}
