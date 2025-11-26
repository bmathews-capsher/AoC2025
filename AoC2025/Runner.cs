using System.Diagnostics;
using System.Reflection;

namespace AOC2025
{
        public class Runner
        {
                public static void Run(string day, bool firstPart, string testFile = "")
                {
                        if (string.IsNullOrEmpty(testFile))
                        {
                                if (firstPart) testFile = "A";
                                else testFile = "B";
                        }

                        Assembly assembly = Assembly.GetExecutingAssembly();
                        string part = firstPart ? "A" : "B";
                        string typeName = "AOC2024.Day" + day + part;
                        object dayInstance = assembly.CreateInstance(typeName);


                        MethodInfo m = assembly.GetType(typeName).GetMethod("Solve");
                        Stopwatch stopwatch = new();
                        stopwatch.Start();
                        List<string> data = new(File.ReadAllLines("Input\\Day" + day + testFile + ".txt"));
                        m.Invoke(dayInstance, [data]);
                        stopwatch.Stop();

                        Console.WriteLine("Elapsed Time: " + stopwatch.Elapsed);
                }
        }
}