using System.Drawing;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;

namespace AOC2025
{
        public class Day09B
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

                        public override string ToString()
                        {
                                return "(" + x + "," + y + ")";
                        }
                }

                private class Line
                {
                        public long pos;
                        public long min;
                        public long max;

                        public bool dir; //true = down/right

                        public Line(){}
                        public Line(long pos, long a, long b)
                        {
                                this.dir = b > a;

                                this.pos = pos;
                                if(dir)
                                {
                                        this.min = a;
                                        this.max = b;
                                }
                                else
                                {
                                        this.min = b;
                                        this.max = a;
                                }
                        }

                        public override string ToString()
                        {
                                return "(" + pos + "," + min + "-" + max + ")";
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


                        List<Line> horizs = new();
                        List<Line> verts = new();

                        Point2D prev = points[0];
                        for(int i = 1; i <= points.Count; i++)
                        {
                                Point2D curr = points[i%points.Count];

                                if(prev.x == curr.x)
                                {
                                        //vert
                                        verts.Add(new Line(prev.x, prev.y, curr.y));
                                }
                                else
                                {
                                        //horiz
                                        horizs.Add(new Line(prev.y, prev.x, curr.x));
                                }

                                prev = curr;
                        }

                        horizs = horizs.OrderBy(a => a.pos).ToList();

                        //make sure points are listed clockwise
                        if(!horizs[0].dir) Console.WriteLine("counter-clockwise");


                        //look at each corner and figure out what rectangles it can see
                        //just to one algorithm and flip the board for each perspective
                        //should have used the vertical lines too, but didn't need them to get lucky :)

                        //top left
                        long maxArea = CheckTopLeftPoints(horizs);

                        //top right
                        foreach(Line line in horizs)
                        {
                                long temp = line.min;
                                line.min = 100000-line.max;
                                line.max = 100000-temp;
                        }
                        maxArea = Math.Max(maxArea, CheckTopLeftPoints(horizs));

                        //bottom right
                        foreach(Line line in horizs)
                        {
                                line.dir = !line.dir;
                                line.pos = 100000 - line.pos;
                        }
                        maxArea = Math.Max(maxArea, CheckTopLeftPoints(horizs));

                        //bottom left
                        foreach(Line line in horizs)
                        {
                                long temp = line.min;
                                line.min = 100000-line.max;
                                line.max = 100000-temp;
                        }
                        maxArea = Math.Max(maxArea, CheckTopLeftPoints(horizs));


                        Console.WriteLine(maxArea);
                }

                private long CheckTopLeftPoints(List<Line> lines)
                {
                        lines = lines.OrderBy(a => a.pos * 100000 + a.max).ToList();
                        long maxArea = 0;

                        //check top left points
                        for(int i = 0; i < lines.Count; i++)
                        {
                                Line line = lines[i];

                                if(!line.dir) continue; // only checking where the shape is below the line

                                long min = line.min;
                                long max = line.max;

                                //checking min - top left point
                                Point2D minP = new(min, line.pos);
                                for(int j = i+1; j < lines.Count; j++)
                                {
                                        Line comp = lines[j];

                                        //if doesn't overlap, don't care
                                        if(!Overlap(comp.min, comp.max, min, max)) continue;
                                        
                                        if(comp.min >= min && comp.max <= max)
                                        {
                                                //over
                                                // line: ----------
                                                // comp:    ----

                                                //rect to comp max
                                                maxArea = Math.Max(maxArea, Area(minP, new Point2D(comp.max, comp.pos)));

                                                //reset max to comp min
                                                max = comp.min;
                                        }
                                        else if(comp.min <= min && comp.max >= max)
                                        {
                                                //under
                                                // line:    ----
                                                // comp: ----------

                                                //rect to comp max
                                                if(comp.max <= max)
                                                {
                                                        maxArea = Math.Max(maxArea, Area(minP, new Point2D(comp.max, comp.pos)));
                                                }

                                                //reset max to comp min
                                                max = comp.min;
                                        }
                                        else if(max > comp.max)
                                        {
                                                //left
                                                // line:   ----
                                                // comp: ----

                                                if(min == comp.max) continue; //ignore 1 width rects

                                                //rect to comp max
                                                maxArea = Math.Max(maxArea, Area(minP, new Point2D(comp.max, comp.pos)));

                                                //reset max to comp min
                                                max = comp.min;
                                        }
                                        else if(line.dir == comp.dir)
                                        {
                                                //right - same dir
                                                // line: ---->
                                                // comp:   ---->

                                                //rect to comp min
                                                maxArea = Math.Max(maxArea, Area(minP, new Point2D(comp.min, comp.pos)));

                                                //reset max to comp min
                                                max = comp.min;
                                        }
                                        else
                                        {
                                                //right - diff dir
                                                // line: ---->
                                                // comp:   <----

                                                if(max == comp.min) continue; //ignore 1 width rects

                                                //rect to best
                                                long newMax = comp.max;
                                                if(max < comp.max) newMax = comp.min;
                                                maxArea = Math.Max(maxArea, Area(minP, new Point2D(newMax, comp.pos)));

                                                //reset max to comp min
                                                max = comp.min;
                                        }

                                        //hit a wall on this side
                                        if(max < min) break;
                                }
                        }  

                        return maxArea;
                }

                private long Area(Point2D p1, Point2D p2)
                {
                        return (Math.Abs(p1.x - p2.x)+1) * (Math.Abs(p1.y - p2.y)+1);
                }

                private bool Overlap(long range1, long range2, long check1, long check2)
                {
                        return Contains(range1, range2, check1) || Contains(range1, range2, check2) || 
                        Contains(check1, check2, range1) || Contains(check1, check2, range2);
                }

                private bool Contains(long range1, long range2, long check)
                {
                        return check >= range1 && check <= range2;
                }

                /*
                waste of time... :(
                private void Print(List<Point2D> points, long width, long height)
                {
                        List<List<char>> grid = new();

                        for(int y = 0; y < height; y++)
                        {
                                grid.Add(new());
                                for(int x = 0; x < width; x++)
                                {
                                        grid[y].Add('.');
                                }
                        }

                        Point2D prev = points[0];
                        for(int i = 1; i <= points.Count; i++)
                        {
                                Point2D curr = points[i % points.Count];

                                bool vert = curr.x == prev.x;

                                if(vert)
                                {
                                        bool down = curr.y > prev.y;

                                        long y1 = 0;
                                        long y2 = 0;
                                        if(down)
                                        {
                                                y1 = prev.y;
                                                y2 = curr.y;
                                        }
                                        else
                                        {
                                                y1 = curr.y;
                                                y2 = prev.y;
                                        }

                                        for(long y = y1; y <= y2; y++)
                                        {
                                                grid[(int) y][(int) curr.x] = 'X';
                                        }
                                }
                                else
                                {
                                        bool right = curr.x > prev.x;

                                        long x1 = 0;
                                        long x2 = 0;
                                        if(right)
                                        {
                                                x1 = prev.x;
                                                x2 = curr.x;
                                        }
                                        else
                                        {
                                                x1 = curr.x;
                                                x2 = prev.x;
                                        }

                                        for(long x = x1; x <= x2; x++)
                                        {
                                                grid[(int) curr.y][(int) x] = 'X';
                                        }
                                }

                                prev = curr;
                        }

                        Point2D p1 = points[0];
                        Point2D p2 = points[1];

                        grid[(int)p1.y][(int)p1.x] = '1';
                        grid[(int)p2.y][(int)p2.x] = '2';

                        for(int y = 0; y < height; y++)
                        {
                                for(int x = 0; x < width; x++)
                                {
                                        Console.Write(grid[y][x]);
                                }
                                Console.WriteLine();
                        }
                }
                */
        }
}