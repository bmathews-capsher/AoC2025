namespace AOC2025
{
        public class Day06B
        {
                private class Problem
                {
                        public int start;
                        public int end;
                        public char op;

                        public int GetLength()
                        {
                                return end - start + 1;
                        }
                }

                public void Solve(List<string> data)
                {
                        List<Problem> problems = new();
                        int width = data[0].Length;

                        string opsRow = data[data.Count - 1];

                        Problem problem = new();
                        problem.op = opsRow[0];
                        problem.start = 0;
                        problem.end = 0;
                        problems.Add(problem);

                        for (int i = 1; i < width; i++)
                        {
                                char c = opsRow[i];
                                if (c == ' ')
                                {
                                        problem.end++;
                                }
                                else
                                {
                                        problem = new();
                                        problem.op = c;
                                        problem.start = i;
                                        problem.end = i;
                                        problems.Add(problem);
                                }

                        }
                        problem.end++;

                        long total = 0;

                        foreach (Problem currProb in problems)
                        {
                                List<long> nums = new();
                                for (int x = currProb.start; x < currProb.end; x++)
                                {
                                        string strNum = "";
                                        for (int y = 0; y < data.Count - 1; y++)
                                        {
                                                if (data[y][x] != ' ')
                                                {
                                                        strNum += data[y][x];
                                                }
                                        }
                                        nums.Add(long.Parse(strNum));
                                }


                                if (currProb.op == '+')
                                {
                                        foreach (long num in nums)
                                        {
                                                total += num;
                                        }
                                }
                                else
                                {
                                        long product = 1;
                                        foreach (long num in nums)
                                        {
                                                product *= num;
                                        }
                                        total += product;
                                }
                        }

                        Console.WriteLine(total);
                }

        }
}