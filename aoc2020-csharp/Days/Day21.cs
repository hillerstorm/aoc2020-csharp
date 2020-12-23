using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day21 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var lines = input.SplitLines();
            return (
                () => Part1(lines).ToString(),
                () => Part2(lines)
            );
        }

        public static int Part1(IEnumerable<string> input)
        {
            var (lines, possibleAllergens) = ParseValues(input);

            var awd = possibleAllergens
                .Where(x => x.Value.Count == 0)
                .Select(x => lines.Count(y => y.Ingredients.Contains(x.Key)))
                .Sum();

            return awd;
        }

        private static ((string[] Ingredients, string[] Allergens)[] Lines, Dictionary<string, List<string>> PossibleAllergens)
            ParseValues(IEnumerable<string> input)
        {
            var lines = input.Select(x =>
            {
                var line = x.TrimEnd(')');
                var (ingredients, allergens, _) = line.Split(" (contains ", StringSplitOptions.RemoveEmptyEntries);
                var allIngredients = ingredients.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var allAllergens = allergens.Split(", ", StringSplitOptions.RemoveEmptyEntries);
                return (Ingredients: allIngredients, Allergens: allAllergens);
            }).ToArray();

            var uniqueIngredients = lines.SelectMany(x => x.Ingredients).Distinct().ToArray();
            var uniqueAllergens = lines.SelectMany(x => x.Allergens).Distinct().ToArray();
            var possibleAllergens = uniqueIngredients
                .ToDictionary(
                    x => x,
                    x => uniqueAllergens
                        .Where(y => lines
                            .Any(p => p.Allergens.Contains(y) && p.Ingredients.Contains(x))
                        )
                        .ToList()
                );

            foreach (var ingredient in uniqueIngredients)
            {
                var allergens = possibleAllergens[ingredient].ToArray();
                foreach (var allergen in allergens)
                {
                    var notMatching = lines
                        .Where(x => x.Allergens.Contains(allergen) && !x.Ingredients.Contains(ingredient))
                        .ToArray();

                    if (notMatching.Length == 0)
                        continue;

                    possibleAllergens[ingredient].Remove(allergen);
                }
            }

            return (lines, possibleAllergens);
        }

        public static string Part2(IEnumerable<string> input)
        {
            var (_, possibleAllergens) = ParseValues(input);

            var withIngredients = possibleAllergens
                .Where(x => x.Value.Count > 0)
                .ToArray();

            while (withIngredients.Any(x => x.Value.Count > 1))
            {
                var singles = withIngredients.Where(x => x.Value.Count == 1).ToArray();
                var multis = withIngredients.Where(x => x.Value.Count > 1).ToArray();
                foreach (var (_, ingredients) in singles)
                    foreach (var (_, values) in multis)
                        values.Remove(ingredients[0]);
            }

            return string.Join(
                ",",
                withIngredients
                    .OrderBy(x => x.Value[0])
                    .Select(x => x.Key)
            );
        }
    }
}
