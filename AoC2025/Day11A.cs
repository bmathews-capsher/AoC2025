using System.Drawing;

namespace AOC2025
{
        public class Day11A
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

                        Console.WriteLine(CountPaths("you", connections));
                }
                Dictionary<string, long> cache = new();

                private long CountPaths(string from, Dictionary<string, List<string>> connections)
                {
                        if (cache.ContainsKey(from)) return cache[from];

                        long result = _CountPaths(from, connections);
                        cache.Add(from, result);

                        return result;
                }

                private long _CountPaths(string from, Dictionary<string, List<string>> connections)
                {
                        if (from.Equals("out")) return 1;


                        long sum = 0;
                        foreach (string to in connections[from])
                        {
                                sum += CountPaths(to, connections);
                        }
                        return sum;
                }
        }
}