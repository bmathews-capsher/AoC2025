using System.Drawing;

namespace AOC2025
{
        public class Day10B
        {

                public void Solve(List<string> data)
                {
                        long sum = 0;
                        for (int l = 0; l < data.Count; l++)
                        {
                                string line = data[l];
                                sum += Solve(line, l + 1);
                        }

                        Console.WriteLine("Final Sum: " + sum);
                }

                private int Solve(string line, int lineNum)
                {
                        return 0;
                }
        }
}