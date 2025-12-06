namespace AOC2025
{
        public class Day05A
        {
                private class Range
                {
                        long min;
                        long max;

                        public Range(long min, long max)
                        {
                                this.min = min;
                                this.max = max;
                        }

                        public bool Includes(long x)
                        {
                                return x >= min && x <= max;
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

                        row++;

                        long total = 0;
                        for (; row < data.Count; row++)
                        {
                                long id = long.Parse(data[row]);
                                bool valid = false;

                                foreach (Range range in ranges)
                                {
                                        if (range.Includes(id))
                                        {
                                                valid = true;
                                                break;
                                        }
                                }

                                if (valid) total++;
                        }

                        Console.WriteLine(total);
                }

        }
}