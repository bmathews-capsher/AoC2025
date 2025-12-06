namespace AOC2025
{
        public class Day03A
        {
                public void Solve(List<string> data)
                {
                        long total = 0;

                        foreach (string line in data)
                        {
                                int highestFirst = 0;
                                int highestSecond = 0;
                                for (int i = 0; i < line.Length - 1; i++)
                                {
                                        int a = line[i] - '0';
                                        int b = line[i + 1] - '0';

                                        if (a > highestFirst)
                                        {
                                                highestFirst = a;
                                                highestSecond = 0;
                                        }

                                        if (b > highestSecond)
                                        {
                                                highestSecond = b;
                                        }
                                }

                                total += highestFirst * 10 + highestSecond;
                        }

                        Console.WriteLine(total);
                }
        }
}