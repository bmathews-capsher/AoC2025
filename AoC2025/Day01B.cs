namespace AOC2025
{
        public class Day01B
        {
                public void Solve(List<string> data)
                {
                        int dial = 50;
                        int count = 0;

                        foreach (string line in data)
                        {
                                int move = 0;
                                bool right = 'R' == line[0];
                                move = Int32.Parse(line.Substring(1));

                                count += move / 100;
                                move %= 100;

                                if (right)
                                {
                                        if (dial != 0 && dial + move >= 100) count++;

                                        dial += move;
                                        if (dial >= 100)
                                        {
                                                dial -= 100;
                                        }
                                }
                                else
                                {
                                        if (dial != 0 && dial - move <= 0) count++;

                                        dial -= move;
                                        if (dial < 0)
                                        {
                                                dial += 100;
                                        }
                                }
                        }

                        Console.WriteLine(count);
                }
        }
}