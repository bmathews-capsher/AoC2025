namespace AOC2025
{
        public class Day07A
        {
                public void Solve(List<string> data)
                {
                        HashSet<int> beams = new();

                        for (int i = 0; i < data[0].Length; i++)
                        {
                                if (data[0][i] == 'S')
                                {
                                        beams.Add(i);
                                        break;
                                }
                        }

                        long splits = 0;
                        for (int y = 1; y < data.Count; y++)
                        {
                                HashSet<int> newBeams = new();

                                foreach (int beam in beams)
                                {
                                        if (data[y][beam] == '^')
                                        {
                                                if (beam > 0) newBeams.Add(beam - 1);
                                                if (beam < data[0].Length - 1) newBeams.Add(beam + 1);
                                                splits++;
                                        }
                                        else
                                        {
                                                newBeams.Add(beam);
                                        }
                                }

                                beams = newBeams;
                        }

                        Console.WriteLine(splits);
                }

        }
}