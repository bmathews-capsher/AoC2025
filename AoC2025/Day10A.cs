using System.Drawing;

namespace AOC2025
{
        public class Day10A
        {
                public void Solve(List<string> data)
                {
                        long totalPresses = 0;

                        foreach (string line in data)
                        {
                                string[] parts = line.Split(' ');

                                //indicators
                                int indicators = 0;
                                for (int i = parts[0].Length - 2; i > 0; i--)
                                {
                                        indicators = indicators << 1;
                                        if (parts[0][i] == '#')
                                        {
                                                indicators++;
                                        }
                                }

                                //buttons
                                List<int> buttons = new();
                                for (int i = 1; i < parts.Length - 1; i++)
                                {
                                        int button = 0;
                                        string[] buttonParts = parts[i].Substring(1, parts[i].Length - 2).Split(',');
                                        for (int j = 0; j < buttonParts.Length; j++)
                                        {
                                                button |= 1 << int.Parse(buttonParts[j]);
                                        }
                                        buttons.Add(button);
                                }

                                //voltage - not needed

                                totalPresses += CountPresses(indicators, buttons);
                        }

                        Console.WriteLine(totalPresses);
                }

                private long CountPresses(int finalIndicators, List<int> buttons)
                {
                        List<(int indicators, long presses)> search = new();
                        search.Add((0, 0));

                        HashSet<int> seen = new();
                        seen.Add(0);

                        while (true)
                        {
                                (int indicators, long presses) currSearch = search[0];
                                search.RemoveAt(0);

                                if (currSearch.indicators == finalIndicators) return currSearch.presses;

                                foreach (int button in buttons)
                                {
                                        int newIndicators = currSearch.indicators;
                                        newIndicators ^= button;

                                        if (seen.Contains(newIndicators)) continue;

                                        search.Add((newIndicators, currSearch.presses + 1));
                                        seen.Add(newIndicators);
                                }
                        }
                }
        }
}