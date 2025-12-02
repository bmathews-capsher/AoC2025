namespace AOC2025
{
        public class Day01A
        {
                public void Solve(List<string> data)
                {
                        int dial = 50;
                        int count = 0;

                        foreach (string line in data)
                        {
                                int move = 0;
                                switch (line[0])
                                {
                                        case 'R':
                                                move = Int32.Parse(line.Substring(1));
                                                break;
                                        case 'L':
                                                move = -Int32.Parse(line.Substring(1));
                                                break;
                                }

                                dial += move;
                                dial %= 100;
                                if (dial < 0) dial += 100;

                                if (dial == 0) count++;
                        }

                        Console.WriteLine(count);
                }
        }
}