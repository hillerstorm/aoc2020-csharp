using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace aoc2020.Days
{
    public class Day14 : IDay
    {
        public (Func<string> Part1, Func<string> Part2) Parts(string input)
        {
            var lines = input.SplitLines();
            return (
                () => Part1(lines).ToString(),
                () => Part2(lines).ToString()
            );
        }

        private const int Bits = 36;

        public static ulong Part1(string[] input)
        {
            var bitMask = ParseBitMask(input[0]);
            var memory = new ulong[100000];
            foreach (var line in input[1..])
            {
                if (line.StartsWith("mask"))
                    bitMask = ParseBitMask(line);
                else
                {
                    var memIndex = int.Parse(line[4..line.IndexOf(']')]);
                    var value = ulong.Parse(line[(line.LastIndexOf(' ') + 1)..]);
                    var bitArray = new BitArray64(value);
                    for (var i = 0; i < Bits; i++)
                    {
                        var maskValue = bitMask[i];
                        if (maskValue == true)
                            bitArray.SetBit(i);
                        else if (maskValue == false)
                            bitArray.UnsetBit(i);
                    }

                    memory[memIndex] = bitArray.Bits;
                }
            }

            return memory.Aggregate(0ul, (a, b) => a + b);
        }

        public static ulong Part2(string[] input)
        {
            var bitMask = ParseBitMask(input[0]);
            var memory = new Dictionary<ulong, ulong>();
            foreach (var line in input[1..])
            {
                if (line.StartsWith("mask"))
                    bitMask = ParseBitMask(line);
                else
                {
                    var indices = new List<BitArray64>
                    {
                        new(ulong.Parse(line[4..line.IndexOf(']')])),
                    };
                    var value = ulong.Parse(line[(line.LastIndexOf(' ') + 1)..]);
                    for (var i = 0; i < Bits; i++)
                    {
                        var maskValue = bitMask[i];
                        if (maskValue == true)
                            for (var j = 0; j < indices.Count; j++)
                                SetBit(indices, j, i);
                        else if (!maskValue.HasValue)
                        {
                            var newIndices = new List<BitArray64>();
                            for (var j = 0; j < indices.Count; j++)
                            {
                                var newIndex = new BitArray64(SetBit(indices, j, i));
                                newIndex.UnsetBit(i);
                                newIndices.Add(newIndex);
                            }
                            indices.AddRange(newIndices);
                        }
                    }

                    foreach (var index in indices)
                        if (memory.ContainsKey(index.Bits))
                            memory[index.Bits] = value;
                        else
                            memory.Add(index.Bits, value);
                }
            }

            return memory.Aggregate(0ul, (a, b) => a + b.Value);
        }

        private static ulong SetBit(IList<BitArray64> indices, int i, int bitIndex)
        {
            var index = indices[i];
            index.SetBit(bitIndex);
            indices[i] = index;
            return index.Bits;
        }

        private static bool?[] ParseBitMask(string line)
        {
            var maskString = line[7..];
            var bitMask = new bool?[Bits];
            for (var i = Bits - 1; i >= 0; i--)
                bitMask[Bits - i - 1] = maskString[i] switch
                {
                    '1' => true,
                    '0' => false,
                      _ => null,
                };

            return bitMask;
        }
    }
}
