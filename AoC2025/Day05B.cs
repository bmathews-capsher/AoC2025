namespace AOC2025
{
        public class Day05B
        {
                private class Range
                {
                        public long min;
                        public long max;

                        public Range(long min, long max)
                        {
                                this.min = min;
                                this.max = max;
                        }

                        public bool Includes(long x)
                        {
                                return x >= min && x <= max;
                        }

                        public bool ShouldMerge(Range other)
                        {
                                return Includes(other.min) || Includes(other.max);
                        }

                        public void Merge(Range other)
                        {
                                min = Math.Min(min, other.min);
                                max = Math.Max(max, other.max);
                        }

                        public long GetCount()
                        {
                                return max - min + 1;
                        }

                        public void Print()
                        {
                                Console.WriteLine(min + "\t-\t" + max);
                        }
                }

                public void Solve(List<string> data)
                {
                        List<Range> ranges = new();

                        int row = 0;
                        for (; data[row].Length > 0; row++)
                        {
                                string[] parts = data[row].Split('-');
                                ranges.Add(new Range(long.Parse(parts[0]), long.Parse(parts[1])));
                        }

                        List<Range> mergedRanges = new();

                        int numRanges = ranges.Count;
                        int numMergedRanges = mergedRanges.Count;

                        while (numRanges != numMergedRanges)
                        {
                                numRanges = ranges.Count;
                                mergedRanges = new();

                                foreach (Range range in ranges)
                                {
                                        bool merged = false;

                                        foreach (Range mergedRange in mergedRanges)
                                        {
                                                if (mergedRange.ShouldMerge(range))
                                                {
                                                        mergedRange.Merge(range);
                                                        merged = true;
                                                        break;
                                                }
                                        }

                                        if (!merged) mergedRanges.Add(range);
                                }

                                numMergedRanges = mergedRanges.Count;
                                mergedRanges = mergedRanges.OrderBy(x => x.min).ToList();
                                ranges = mergedRanges;
                        }

                        long total = 0;
                        foreach (Range mergedRange in mergedRanges)
                        {
                                total += mergedRange.GetCount();
                        }

                        Console.WriteLine(total);
                }

        }
}