using System.Text;

namespace AOC2025
{
        public class Day02B
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

                                        for (int i = 1; i < val.Length; i++)
                                        {
                                                string pattern = val.Substring(0, i);
                                                if (val.Length % pattern.Length != 0) continue;
                                                string test = "";
                                                for (int j = 0; j < val.Length / pattern.Length; j++)
                                                {
                                                        test += pattern;
                                                }

                                                if (val == test)
                                                {
                                                        total += l;
                                                        break;
                                                }
                                        }
                                }
                        }

                        Console.WriteLine(total);
                }
        }
}