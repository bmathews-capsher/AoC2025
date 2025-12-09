namespace AOC2025
{
        public class Day08A
        {

                private class Point3D
                {
                        public long x;
                        public long y;
                        public long z;


                        public override bool Equals(object obj)
                        {
                                if (obj is not Point3D other) return false;
                                return x == other.x && y == other.y && z == other.z;
                        }

                        public override int GetHashCode()
                        {
                                return HashCode.Combine(x, y, z);
                        }
                }

                private class Pair
                {
                        public Point3D point1;
                        public Point3D point2;

                        public double distance;
                }

                public void Solve(List<string> data)
                {
                        List<Point3D> points = new();

                        foreach (string line in data)
                        {
                                string[] parts = line.Split(',');

                                Point3D point = new();
                                point.x = long.Parse(parts[0]);
                                point.y = long.Parse(parts[1]);
                                point.z = long.Parse(parts[2]);

                                points.Add(point);
                        }

                        SortedList<double, Pair> pairs = new();

                        for (int i = 0; i < points.Count; i++)
                        {
                                for (int j = i + 1; j < points.Count; j++)
                                {
                                        Point3D point1 = points[i];
                                        Point3D point2 = points[j];

                                        long xDist = point1.x - point2.x;
                                        long yDist = point1.y - point2.y;
                                        long zDist = point1.z - point2.z;

                                        double distance = Math.Sqrt(xDist * xDist + yDist * yDist + zDist * zDist);

                                        Pair pair = new();
                                        pair.point1 = point1;
                                        pair.point2 = point2;
                                        pair.distance = distance;

                                        pairs.Add(pair.distance, pair);

                                        if (pairs.Count > 1000) pairs.RemoveAt(pairs.Count - 1);
                                }
                        }

                        Dictionary<Point3D, HashSet<Point3D>> graph = new();

                        foreach (Pair pair in pairs.Values)
                        {
                                if (graph.ContainsKey(pair.point1) && graph.ContainsKey(pair.point2))
                                {
                                        //join lists
                                        HashSet<Point3D> set1 = graph[pair.point1];
                                        HashSet<Point3D> set2 = graph[pair.point2];

                                        foreach (Point3D point in set2)
                                        {
                                                set1.Add(point);
                                                graph[point] = set1;
                                        }
                                        continue;
                                }

                                HashSet<Point3D> set;
                                if (graph.ContainsKey(pair.point1)) set = graph[pair.point1];
                                else if (graph.ContainsKey(pair.point2)) set = graph[pair.point2];
                                else set = new();

                                set.Add(pair.point1);
                                set.Add(pair.point2);

                                graph[pair.point1] = set;
                                graph[pair.point2] = set;
                        }

                        HashSet<HashSet<Point3D>> sets = new();
                        foreach (HashSet<Point3D> set in graph.Values)
                        {
                                sets.Add(set);
                        }

                        List<long> counts = new();
                        foreach (HashSet<Point3D> set in sets)
                        {
                                counts.Add(set.Count);
                        }

                        counts = counts.OrderByDescending(x => x).ToList();

                        Console.WriteLine(counts[0] * counts[1] * counts[2]);
                }

        }
}