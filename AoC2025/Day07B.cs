namespace AOC2025
{
        public class Day07B
        {
                public void Solve(List<string> data)
                {
                        long[] beams = new long[data[0].Length];

                        for (int i = 0; i < beams.Length; i++)
                        {
                                if (data[0][i] == 'S')
                                {
                                        beams[i]++;
                                        break;
                                }
                        }

                        for (int y = 1; y < data.Count; y++)
                        {
                                long[] newBeams = new long[beams.Length];

                                for (int x = 0; x < beams.Length; x++)
                                {
                                        long beam = beams[x];
                                        if (beam == 0) continue;

                                        if (data[y][x] == '^')
                                        {
                                                newBeams[x - 1] += beam;
                                                newBeams[x + 1] += beam;
                                        }
                                        else
                                        {
                                                newBeams[x] += beam;
                                        }
                                }

                                beams = newBeams;
                        }

                        long timelines = 0;
                        for (int x = 0; x < beams.Length; x++)
                        {
                                timelines += beams[x];
                        }
                        Console.WriteLine(timelines);
                }

        }
}