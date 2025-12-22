using System.Drawing;

namespace AOC2025
{
        public class Day11B
        {
                public void Solve(List<string> data)
                {
                        Dictionary<string, List<string>> connections = new();
                        foreach (string line in data)
                        {
                                string[] parts = line.Split(':');

                                connections.Add(parts[0], new());

                                string[] tos = parts[1].Split(' ');

                                for (int i = 1; i < tos.Length; i++)
                                {
                                        string to = tos[i];
                                        connections[parts[0]].Add(to);
                                }
                        }

                        Console.WriteLine(CountPaths("svr", false, false, connections));
                }
                Dictionary<(string, bool, bool), long> cache = new();

                private long CountPaths(string from, bool seenDAC, bool seenFFT, Dictionary<string, List<string>> connections)
                {
                        if (cache.ContainsKey((from, seenDAC, seenFFT))) return cache[(from, seenDAC, seenFFT)];

                        long result = _CountPaths(from, seenDAC, seenFFT, connections);
                        cache.Add((from, seenDAC, seenFFT), result);

                        return result;
                }

                private long _CountPaths(string from, bool seenDAC, bool seenFFT, Dictionary<string, List<string>> connections)
                {
                        if (from.Equals("out"))
                        {
                                if (seenDAC && seenFFT) return 1;
                                else return 0;
                        }

                        if (from.Equals("dac")) seenDAC = true;
                        if (from.Equals("fft")) seenFFT = true;

                        long sum = 0;
                        foreach (string to in connections[from])
                        {
                                sum += CountPaths(to, seenDAC, seenFFT, connections);
                        }
                        return sum;
                }
        }
}