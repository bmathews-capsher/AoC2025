namespace AOC2025
{
        public class Day04B
        {
                int height = 0;
                int width = 0;

                public void Solve(List<string> data)
                {
                        height = data.Count;
                        width = data[0].Length;

                        char[][] grid = new char[height][];
                        for (int y = 0; y < height; y++)
                        {
                                grid[y] = new char[width];
                                for (int x = 0; x < width; x++)
                                {
                                        grid[y][x] = data[y][x];
                                }
                        }

                        int total = 0;
                        int oldTotal = -1;
                        while (oldTotal != total)
                        {
                                oldTotal = total;
                                for (int y = 0; y < height; y++)
                                {
                                        for (int x = 0; x < width; x++)
                                        {
                                                if (grid[y][x] != '.' && CountNeighbors(y, x, grid) < 4)
                                                {
                                                        total++;
                                                        grid[y][x] = '.';
                                                }
                                        }
                                }
                        }
                        Console.WriteLine(total);
                }

                private int CountNeighbors(int y, int x, char[][] data)
                {
                        int count = 0;

                        // up left
                        if (y > 0 && x > 0 && data[y - 1][x - 1] != '.') count++;
                        // up right
                        if (y > 0 && x < width - 1 && data[y - 1][x + 1] != '.') count++;
                        // down left
                        if (y < height - 1 && x > 0 && data[y + 1][x - 1] != '.') count++;
                        // down right
                        if (y < height - 1 && x < width - 1 && data[y + 1][x + 1] != '.') count++;

                        // up
                        if (y > 0 && data[y - 1][x] != '.') count++;
                        // down
                        if (y < height - 1 && data[y + 1][x] != '.') count++;
                        // left
                        if (x > 0 && data[y][x - 1] != '.') count++;
                        // right
                        if (x < width - 1 && data[y][x + 1] != '.') count++;

                        return count;
                }
        }
}