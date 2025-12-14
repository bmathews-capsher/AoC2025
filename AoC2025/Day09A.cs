using System.Drawing;

namespace AOC2025
{
        public class Day09A
        {
                private class Point2D
                {
                        public long x;
                        public long y;

                        public Point2D(){}
                        public Point2D(long x, long y)
                        {
                                this.x = x;
                                this.y = y;
                        }


                        public override bool Equals(object obj)
                        {
                                if (obj is not Point2D other) return false;
                                return x == other.x && y == other.y;
                        }

                        public override int GetHashCode()
                        {
                                return HashCode.Combine(x, y);
                        }
                }

                public void Solve(List<string> data)
                {
                        List<Point2D> points = new();

                        foreach(string line in data)
                        {
                                string[] parts = line.Split(',');
                                Point2D p = new(long.Parse(parts[0]), long.Parse(parts[1]));
                                points.Add(p);
                        }

                        long max = 0;
                        for(int i = 0; i < points.Count; i++)
                        {
                                Point2D pointI = points[i];
                                for(int j = i+1; j < points.Count; j++)
                                {
                                        Point2D pointJ = points[j]; 
                                        long area = Math.Abs(pointI.x - pointJ.x + 1) * Math.Abs(pointI.y - pointJ.y + 1);
                                        if(area > max) max = area;
                                }
                        }

                        Console.WriteLine(max);
                }
        }
}