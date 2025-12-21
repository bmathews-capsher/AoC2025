using System.Drawing;

namespace AOC2025
{
        public class Day10BOld
        {
                private class VoltMeter
                {
                        public int counter = 0;
                        bool[] buttons; //does each button contribute?
                }

                private class ButtonRelationship
                {
                        public int id = -1;
                        public int modifier = 0;
                        public int[] buttons; //which other buttons does this button depend on?

                        public ButtonRelationship(int numButtons)
                        {
                                buttons = new int[numButtons];
                        }

                        public int CountRels()
                        {
                                int count = 0;
                                foreach (int button in buttons)
                                {
                                        count += button;
                                }
                                return count;
                        }
                }


                public void Solve(List<string> data)
                {
                        long totalPresses = 0;

                        for (int l = 1; l <= data.Count; l++)
                        {
                                string line = data[l - 1];
                                string[] parts = line.Split(' ');

                                //indicators - not needed

                                //buttons
                                List<List<int>> buttons = new();
                                for (int i = 1; i < parts.Length - 1; i++)
                                {
                                        List<int> button = new();
                                        string[] buttonParts = parts[i].Substring(1, parts[i].Length - 2).Split(',');
                                        for (int j = 0; j < buttonParts.Length; j++)
                                        {
                                                button.Add(int.Parse(buttonParts[j]));
                                        }
                                        buttons.Add(button);
                                }

                                //voltage
                                string voltageString = parts[parts.Length - 1];
                                string[] voltageParts = voltageString.Substring(1, voltageString.Length - 2).Split(',');
                                int[] voltages = new int[voltageParts.Length];
                                for (int i = 0; i < voltageParts.Length; i++)
                                {
                                        voltages[i] = int.Parse(voltageParts[i]);
                                }

                                //relationships
                                Dictionary<int, ButtonRelationship> rels = new();
                                for (int v = 0; v < voltages.Length; v++)
                                {
                                        //for each voltage counter, relate each of the buttons to each other
                                        ButtonRelationship rel = new(buttons.Count);
                                        rel.modifier += voltages[v];

                                        for (int b = 0; b < buttons.Count; b++)
                                        {
                                                List<int> button = buttons[b];
                                                if (button.Contains(v))
                                                {
                                                        if (rel.id == -1 && !rels.ContainsKey(b))
                                                        {
                                                                //anchor the relationship on a button we have not defined yet
                                                                rel.id = b;
                                                        }
                                                        else
                                                        {
                                                                //connect this button to the rel
                                                                if (rels.ContainsKey(b))
                                                                {
                                                                        //other button already deined, fill in the info
                                                                        ButtonRelationship other = rels[b];
                                                                        rel.modifier -= other.modifier;
                                                                        for (int o = 0; o < other.buttons.Length; o++)
                                                                        {
                                                                                rel.buttons[o] -= other.buttons[o];
                                                                        }
                                                                }
                                                                else
                                                                {
                                                                        //the main button depends on this one
                                                                        rel.buttons[b]--;
                                                                }
                                                        }
                                                }
                                        }

                                        //have all the needed rels, skip the rest
                                        if (rel.id == -1) break;

                                        //self reference, skip
                                        if (rel.buttons[rel.id] != 0) continue;

                                        //substitue into past rels
                                        foreach (ButtonRelationship other in rels.Values)
                                        {
                                                int mod = other.buttons[rel.id];
                                                if (mod != 0)
                                                {
                                                        other.buttons[rel.id] = 0;
                                                        other.modifier += rel.modifier * mod;

                                                        for (int b = 0; b < other.buttons.Length; b++)
                                                        {
                                                                other.buttons[b] += rel.buttons[b];
                                                        }
                                                }

                                                //self reference, remove it
                                                if (other.buttons[other.id] != 0) rels.Remove(other.id);
                                        }

                                        rels.Add(rel.id, rel);
                                }


                                //posiible situations
                                // rels > counters -- overconstrained, should not exist
                                // rels = counters -- perfect, calculate
                                // rels < counters -- underconstrained, search

                                //overconstrained, kill it
                                if (rels.Count > voltages.Length)
                                {
                                        Console.WriteLine("Overconstrained!!!!!");
                                        break;
                                }

                                //add up rels we do know
                                ButtonRelationship minFunc = new(buttons.Count);
                                foreach (ButtonRelationship rel in rels.Values)
                                {
                                        minFunc.modifier += rel.modifier;
                                        for (int i = 0; i < buttons.Count; i++)
                                        {
                                                minFunc.buttons[i] += rel.buttons[i];
                                        }
                                }

                                //perfect, calculate it
                                if (minFunc.CountRels() == 0)
                                {
                                        Console.WriteLine("" + l + ") " + minFunc.modifier);
                                        totalPresses += +minFunc.modifier;
                                        continue;
                                }

                                //underconstrained, search it
                                long presses = CountPresses(rels, buttons, voltages);

                                Console.WriteLine("" + l + ") " + presses);
                                totalPresses += presses;
                        }

                        Console.WriteLine(totalPresses);
                }

                private long CountPresses(Dictionary<int, ButtonRelationship> rels, List<List<int>> buttons, int[] finalVoltages)
                {
                        HashSet<int> undefinedButtons = new();
                        for (int i = 0; i < buttons.Count; i++)
                        {
                                if (!rels.ContainsKey(i)) undefinedButtons.Add(i);
                        }

                        int[] presses = new int[buttons.Count];

                        List<int[]> search = new();
                        search.Add(presses);

                        long minPresses = long.MaxValue;

                        while (search.Count > 0)
                        {
                                int[] curr = search[0];
                                search.RemoveAt(0);

                                int[] tempPresses = new int[buttons.Count];
                                Array.Copy(curr, tempPresses, buttons.Count);

                                //use relationships to figure out how many times to press each button
                                foreach (ButtonRelationship rel in rels.Values)
                                {
                                        tempPresses[rel.id] += rel.modifier;
                                        foreach (int undefinedButton in undefinedButtons)
                                        {
                                                tempPresses[rel.id] += tempPresses[undefinedButton] * rel.buttons[undefinedButton];
                                        }
                                }

                                bool negativePresses = false;
                                for (int b = 0; b < buttons.Count; b++)
                                {
                                        if (tempPresses[b] < 0)
                                        {
                                                negativePresses = true;
                                                break;
                                        }
                                }

                                if (negativePresses) continue;

                                //simulate button presses
                                int[] voltages = new int[finalVoltages.Length];

                                for (int b = 0; b < buttons.Count; b++)
                                {
                                        foreach (int volt in buttons[b])
                                        {
                                                voltages[volt] += tempPresses[b];
                                        }
                                }

                                //check if done
                                int comp = CompareVolatages(voltages, finalVoltages);
                                if (comp == 0) minPresses = Math.Min(minPresses, SumPresses(tempPresses));
                                if (comp > 0) continue;

                                //keep searching
                                foreach (int undefinedButton in undefinedButtons)
                                {
                                        int[] newPresses = new int[buttons.Count];
                                        Array.Copy(curr, newPresses, buttons.Count);

                                        newPresses[undefinedButton]++;
                                        search.Add(newPresses);
                                }
                        }

                        return minPresses;
                }

                private int CompareVolatages(int[] voltages, int[] finalVoltages)
                {
                        bool equal = true;

                        for (int i = 0; i < voltages.Length; i++)
                        {
                                if (voltages[i] == finalVoltages[i]) continue;

                                if (voltages[i] > finalVoltages[i]) return 1;

                                equal = false;
                        }

                        if (equal) return 0;
                        return -1;
                }

                private long SumPresses(int[] presses)
                {
                        long total = 0;
                        foreach (int press in presses) total += press;
                        return total;
                }
        }
}