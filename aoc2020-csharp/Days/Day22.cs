using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day22 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var players = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            return (
                () => Part1(players).ToString(),
                () => Part2(players).ToString()
            );
        }

        public static int Part1(IReadOnlyList<string> input)
        {
            var (player1, player2) = ParsePlayers(input);
            return Combat(player1, player2);
        }

        public static int Part2(IReadOnlyList<string> input)
        {
            var (player1, player2) = ParsePlayers(input);
            return Combat(player1, player2, Part.Two);
        }

        private static (int[], int[]) ParsePlayers(IReadOnlyList<string> input)
        {
            var player1 = input[0][input[0].IndexOf('\n')..].SplitAsInt();
            var player2 = input[1][input[1].IndexOf('\n')..].SplitAsInt();
            return (player1, player2);
        }

        private static int Combat(int[] player1, int[] player2, Part part = Part.One)
        {
            var cardCount = player1.Length + player2.Length;
            var (_, winner) = RecursiveCombat(player1.AsSpan(), player2.AsSpan(), part, returnCards: true);
            return winner!
                .Select((x, i) => (x, i))
                .Aggregate(0, (a, b) => a + b.x * (cardCount - b.i));
        }

        private static (bool PlayerOneWon, int[]? WinnerCards) RecursiveCombat(Span<int> player1, Span<int> player2, Part part, bool returnCards = false)
        {
            var seen = part == Part.One
                ? null
                : new HashSet<string>();
            while (player1.Length > 0 && player2.Length > 0)
            {
                var playerOneWon = true;
                if (seen == null)
                    playerOneWon = player1[0] > player2[0];
                else if (seen.Add(Hash(player1, player2)))
                {
                    var card1 = player1[0];
                    var card2 = player2[0];
                    if (player1.Length - 1 >= card1 && player2.Length - 1 >= card2)
                        (playerOneWon, _) = RecursiveCombat(
                            player1[1..(card1 + 1)],
                            player2[1..(card2 + 1)],
                            part
                        );
                    else
                        playerOneWon = card1 > card2;
                }

                if (playerOneWon)
                {
                    var arr = new int[player1.Length + 1].AsSpan();
                    player1[1..].CopyTo(arr);
                    arr[^2] = player1[0];
                    arr[^1] = player2[0];
                    player1 = arr;
                    player2 = player2[1..];
                }
                else
                {
                    var arr = new int[player2.Length + 1].AsSpan();
                    player2[1..].CopyTo(arr);
                    arr[^2] = player2[0];
                    arr[^1] = player1[0];
                    player2 = arr;
                    player1 = player1[1..];
                }
            }

            return (
                player1.Length > 0,
                returnCards
                    ? player1.Length > 0 ? player1.ToArray() : player2.ToArray()
                    : null
            );
        }

        private static string Hash(Span<int> player1, Span<int> player2)
        {
            var result = new StringBuilder(player1.Length * 3 + player2.Length * 3 - 1);
            for (var i = 0; i < player1.Length - 1; i++)
            {
                result.Append(player1[i]);
                result.Append(',');
            }

            result.Append(player1[^1]);
            result.Append(';');

            for (var i = 0; i < player2.Length - 1; i++)
            {
                result.Append(player2[i]);
                result.Append(',');
            }

            result.Append(player2[^1]);

            return result.ToString();
        }
    }
}
