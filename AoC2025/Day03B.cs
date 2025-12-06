namespace AOC2025
{
        public class Day03B
        {
                public void Solve(List<string> data)
                {
                        long total = 0;

                        foreach (string line in data)
                        {
                                int[] highests = new int[12];
                                for (int i = 0; i < line.Length - 11; i++)
                                {
                                        for (int j = 0; j < 12; j++)
                                        {
                                                int a = line[j + i] - '0';
                                                if (a > highests[j])
                                                {
                                                        highests[j] = a;
                                                        for (int k = j + 1; k < 12; k++)
                                                        {
                                                                highests[k] = 0;
                                                        }
                                                }
                                        }
                                }

                                long count = 0;
                                for (int i = 0; i < 12; i++)
                                {
                                        count *= 10;
                                        count += highests[i];
                                }

                                total += count;
                        }

                        Console.WriteLine(total);
                }
        }
}