using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

// ReSharper disable InconsistentNaming

namespace aoc2020.Days
{
    public class Day04 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var parsed = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            return (
                () => Part1(parsed).ToString(),
                () => Part2(parsed).ToString()
            );
        }

        public static int Part1(IEnumerable<string> input) =>
            ParsePassports(input).Count(x => x.HasRequiredFields);

        public static int Part2(IEnumerable<string> input) =>
            ParsePassports(input).Count(x => x.IsValid);

        private static IEnumerable<Passport> ParsePassports(IEnumerable<string> input)
        {
            var passports = new List<Passport>();
            foreach (var line in input)
            {
                var passport = new Passport();
                var parts = line
                    .SplitLines()
                    .SelectMany(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries));
                foreach (var part in parts)
                {
                    var (key, value, _) = part.Split(":", StringSplitOptions.TrimEntries);
                    switch (key)
                    {
                        case nameof(Passport.byr):
                            passport.byr = value;
                            break;
                        case nameof(Passport.iyr):
                            passport.iyr = value;
                            break;
                        case nameof(Passport.eyr):
                            passport.eyr = value;
                            break;
                        case nameof(Passport.hgt):
                            passport.hgt = value;
                            break;
                        case nameof(Passport.hcl):
                            passport.hcl = value;
                            break;
                        case nameof(Passport.ecl):
                            passport.ecl = value;
                            break;
                        case nameof(Passport.pid):
                            passport.pid = value;
                            break;
                        case nameof(Passport.cid):
                            passport.cid = value;
                            break;
                    }
                }
                passports.Add(passport);
            }

            return passports;
        }

        private class Passport
        {
            public string? byr { get; set; } //(Birth Year)
            public string? iyr { get; set; } //(Issue Year)
            public string? eyr { get; set; } //(Expiration Year)
            public string? hgt { get; set; } //(Height)
            public string? hcl { get; set; } //(Hair Color)
            public string? ecl { get; set; } //(Eye Color)
            public string? pid { get; set; } //(Passport ID)
            public string? cid { get; set; } //(Country ID)

            public bool HasRequiredFields =>
                !string.IsNullOrWhiteSpace(byr) &&
                !string.IsNullOrWhiteSpace(iyr) &&
                !string.IsNullOrWhiteSpace(eyr) &&
                !string.IsNullOrWhiteSpace(hgt) &&
                !string.IsNullOrWhiteSpace(hcl) &&
                !string.IsNullOrWhiteSpace(ecl) &&
                !string.IsNullOrWhiteSpace(pid);

            private static readonly Regex HeightRegex = new Regex(@"^(\d+)(cm|in)$");
            private static readonly Regex HairColorRegex = new Regex(@"^\#[a-f0-9]{6}$");
            private static readonly string[] ValidEyeColors =
            {
                "amb", "blu", "brn", "gry", "grn", "hzl", "oth",
            };

            public bool IsValid =>
                HasRequiredFields &&
                byr != null && int.TryParse(byr, out var a) && a >= 1920 && a <= 2002 &&
                iyr != null && int.TryParse(iyr, out var b) && b >= 2010 && b <= 2020 &&
                eyr != null && int.TryParse(eyr, out var c) && c >= 2020 && c <= 2030 &&
                hgt != null && ValidateHeight(hgt) &&
                hcl != null && HairColorRegex.IsMatch(hcl) &&
                ecl != null && ValidEyeColors.Contains(ecl) &&
                pid != null && pid.Length == 9 && int.TryParse(byr, out _);

            private static bool ValidateHeight(string height)
            {
                if (!HeightRegex.IsMatch(height))
                    return false;

                var (_, value, units, _) = HeightRegex.Match(height).Groups;
                var parsed = int.Parse(value.Value);
                return units.Value switch
                {
                    "cm" => parsed >= 150 && parsed <= 193,
                    /*in*/ _ => parsed >= 59 && parsed <= 76
                };
            }
        }
    }
}
