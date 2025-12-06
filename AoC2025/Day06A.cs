namespace AOC2025
{
        public class Day06A
        {
                public void Solve(List<string> data)
                {
                        List<List<long>> nums = new();
                        List<char> ops = new();

                        foreach (string row in data)
                        {
                                string[] parts = row.Split(' ');

                                if (row[0] == '*' || row[0] == '+')
                                {
                                        //read ops
                                        foreach (string part in parts)
                                        {
                                                if (part.Length == 0) continue;
                                                ops.Add(part[0]);
                                        }
                                        break;
                                }

                                List<long> currNums = new();
                                nums.Add(currNums);
                                foreach (string part in parts)
                                {
                                        if (part.Length == 0) continue;
                                        long num = long.Parse(part);
                                        currNums.Add(num);
                                }
                        }

                        long total = 0;

                        for (int i = 0; i < ops.Count; i++)
                        {
                                if (ops[i] == '+')
                                {
                                        foreach (List<long> currNums in nums)
                                        {
                                                total += currNums[i];
                                        }
                                }
                                else
                                {
                                        long product = 1;
                                        foreach (List<long> currNums in nums)
                                        {
                                                product *= currNums[i];
                                        }
                                        total += product;
                                }
                        }

                        Console.WriteLine(total);
                }

        }
}