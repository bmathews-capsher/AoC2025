using System.Drawing;

namespace AOC2025
{
        public class Day12A
        {
                public void Solve(List<string> data)
                {
                        List<char[,]> shapes = new();

                        int i = 0;
                        for (; i < data.Count; i++)
                        {
                                string line = data[i];
                                if (line.Contains('x')) break;

                                char[,] shape = new char[3, 3];
                                i++;
                                for (int j = 0; j < 3; j++, i++)
                                {
                                        line = data[i];
                                        shape[j, 0] = line[0];
                                        shape[j, 1] = line[1];
                                        shape[j, 2] = line[2];
                                }
                                shapes.Add(shape);
                        }

                        long count = 0;
                        for (; i < data.Count; i++)
                        {
                                string line = data[i];
                                string[] parts = line.Split(':');

                                string[] sizes = parts[0].Split('x');
                                char[,] grid = new char[int.Parse(sizes[0]), int.Parse(sizes[1])];

                                int[] neededShapes = new int[6];
                                string[] shapeCounts = parts[1].Split(' ');
                                for (int j = 1; j < shapeCounts.Length; j++)
                                {
                                        neededShapes[j - 1] = int.Parse(shapeCounts[j]);
                                }

                                bool result = CanFit(grid, shapes, neededShapes);
                                if (result) count++;

                                Console.WriteLine(i - 30 + ":\t" + result);
                        }

                        Console.WriteLine(count);
                }

                private bool CanFit(char[,] grid, List<char[,]> shapes, int[] neededShapes)
                {
                        long totalDots = 0;
                        foreach (int count in neededShapes)
                        {
                                totalDots += count * 7;
                        }

                        if (totalDots < grid.GetLength(0) * grid.GetLength(1)) return true;

                        return false;
                }
        }
}