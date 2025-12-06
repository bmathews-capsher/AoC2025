namespace AOC2025
{
        public class Day02A
        {
                public void Solve(List<string> data)
                {
                        List<string> ranges = data[0].Split(',').ToList<string>();

                        long total = 0;
                        foreach (string range in ranges)
                        {
                                string[] parts = range.Split('-');
                                long start = long.Parse(parts[0]);
                                long end = long.Parse(parts[1]);

                                for (long l = start; l <= end; l++)
                                {
                                        string val = l.ToString();
                                        if (val.Length % 2 == 1) continue;

                                        string first = val.Substring(0, val.Length / 2);
                                        string second = val.Substring(val.Length / 2);

                                        if (first == second)
                                        {
                                                total += l;
                                        }

                                }
                        }

                        Console.WriteLine(total);
                }
        }
}