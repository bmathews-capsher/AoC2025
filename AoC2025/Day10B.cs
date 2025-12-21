using System.Drawing;

namespace AOC2025
{
        public class Day10B
        {
                private class Expression
                {
                        public float[] vars; // index 0 is always the constant

                        public Expression(int size)
                        {
                                vars = new float[size];
                        }

                        public bool ContainsVar(int var)
                        {
                                return Math.Abs(vars[var]) > 0.00000001;
                        }

                        public void Substitute(int var, Expression e)
                        {
                                for (int v = 0; v < vars.Length; v++)
                                {
                                        vars[v] += vars[var] * e.vars[v];
                                }

                                vars[var] = 0;
                        }

                        public void Add(Expression e)
                        {
                                for (int v = 0; v < vars.Length; v++)
                                {
                                        vars[v] += e.vars[v];
                                }
                        }

                        public void Subtract(Expression e)
                        {
                                for (int v = 0; v < vars.Length; v++)
                                {
                                        vars[v] -= e.vars[v];
                                }
                        }

                        public void Divide(float val)
                        {
                                for (int v = 0; v < vars.Length; v++)
                                {
                                        vars[v] /= val;
                                }
                        }

                        public float Evaluate(float[] vals)
                        {
                                float sum = vars[0];
                                for (int i = 1; i < vars.Length; i++)
                                {
                                        sum += vals[i] * vars[i];
                                }

                                return sum;
                        }

                        public float Evaluate(Dictionary<int, float> values)
                        {
                                float sum = vars[0];
                                foreach (KeyValuePair<int, float> value in values)
                                {
                                        sum += value.Value * vars[value.Key];
                                }

                                return sum;
                        }

                        public Expression Copy()
                        {
                                Expression newEx = new(vars.Length);
                                Array.Copy(vars, newEx.vars, vars.Length);
                                return newEx;
                        }

                        public override bool Equals(object? obj)
                        {
                                if (obj == null || obj.GetType() != typeof(Expression)) return false;

                                Expression ex = (Expression)obj;

                                if (ex.vars.Length != vars.Length) return false;

                                for (int i = 0; i < vars.Length; i++)
                                {
                                        if (Math.Abs(ex.vars[i] - vars[i]) > 0.0000001) return false;
                                }
                                return true;
                        }

                        public override string ToString()
                        {
                                return String.Join('+', vars);
                        }

                }

                private class Equation
                {
                        public Expression left;
                        public Expression right;
                        public int size;

                        private Equation()
                        {
                                //for copy
                        }

                        public Equation(int size)
                        {
                                this.size = size;
                                left = new Expression(size);
                                right = new Expression(size);
                        }

                        public Equation(Expression ex)
                        {
                                this.size = ex.vars.Length;
                                left = new Expression(size);
                                right = ex.Copy();
                        }

                        public void SolveFor(int var)
                        {
                                //solving for 0 means moving everything to the right
                                right.vars[0] -= left.vars[0];
                                left.vars[0] = 0;

                                for (int i = 1; i < size; i++)
                                {
                                        if (i == var)
                                        {
                                                left.vars[i] -= right.vars[i];
                                                right.vars[i] = 0;
                                        }
                                        else
                                        {
                                                right.vars[i] -= left.vars[i];
                                                left.vars[i] = 0;
                                        }
                                }

                                if (!left.ContainsVar(var)) return;

                                right.Divide(left.vars[var]);
                                left.Divide(left.vars[var]);
                        }

                        public Equation Copy()
                        {
                                Equation newEq = new();
                                newEq.size = size;
                                newEq.left = left.Copy();
                                newEq.right = right.Copy();
                                return newEq;
                        }

                        public override string ToString()
                        {
                                return left.ToString() + " = " + right.ToString();
                        }

                        public override bool Equals(object? obj)
                        {
                                if (obj == null || obj.GetType() != typeof(Equation)) return false;

                                Equation eq = (Equation)obj;

                                return left.Equals(eq.left) && right.Equals(eq.right);
                        }

                }

                private class EquationSet
                {
                        public Equation[] set;
                        public bool[] containsVar;

                        public EquationSet(Equation source)
                        {
                                //generates the set of equivalent equations solved for all possible variables

                                set = new Equation[source.size];
                                containsVar = new bool[source.size];

                                Equation sourceCopy = source.Copy();

                                sourceCopy.SolveFor(0);

                                for (int i = 1; i < sourceCopy.size; i++)
                                {
                                        if (!sourceCopy.right.ContainsVar(i)) continue;

                                        Equation copy = sourceCopy.Copy();
                                        copy.SolveFor(i);
                                        set[i] = copy;
                                        containsVar[i] = true;
                                }
                        }
                }

                private long startTime;

                public void Solve(List<string> data)
                {
                        startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                        long sum = 0;
                        for (int l = 0; l < data.Count; l++)
                        {
                                string line = data[l];
                                sum += Solve(line, l);
                        }

                        Console.WriteLine("Final Sum: " + sum);
                }

                private int Solve(string line, int lineNum)
                {
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

                        //equations
                        List<Equation> eqs = GenerateEquations(buttons, voltages);
                        List<EquationSet> eqSets = GenerateEquationSet(eqs);

                        //minimal expression
                        (Expression minimalEx, Equation[] simplifiedEqs, HashSet<int> unknowns) system = FindMinimalExpression(eqSets);

                        //max values
                        int size = buttons.Count + 1;
                        float[] maxes = new float[size];
                        Array.Fill(maxes, int.MaxValue);

                        foreach (Equation eq in eqs)
                        {
                                for (int i = 1; i < size; i++)
                                {
                                        if (eq.right.ContainsVar(i))
                                        {
                                                maxes[i] = Math.Min(maxes[i], Math.Abs(eq.right.vars[0]));
                                        }
                                }
                        }

                        List<int> unknownList = new List<int>(system.unknowns);

                        //search for values of the unknowns
                        Dictionary<int, float> vals = new();
                        foreach (int unknown in unknownList)
                        {
                                vals.Add(unknown, 0);
                        }

                        float minPresses = FindSolution(vals, 0, system.minimalEx, system.simplifiedEqs, unknownList, maxes);
                        Console.WriteLine(lineNum + "\t: " + minPresses);

                        return (int)(minPresses + 0.5);
                }

                private float FindSolution(Dictionary<int, float> vals, int unknownIndex, Expression minimalEx, Equation[] simplifiedEqs, List<int> unknowns, float[] maxes)
                {
                        if (unknownIndex >= unknowns.Count)
                        {
                                //have a full set, check to see if it's good
                                if (CheckViable(vals, simplifiedEqs))
                                {
                                        return minimalEx.Evaluate(vals);
                                }
                                else
                                {
                                        return int.MaxValue;
                                }
                        }

                        int currUnknown = unknowns[unknownIndex];

                        float min = int.MaxValue;

                        for (int i = 0; i <= maxes[currUnknown]; i++)
                        {
                                vals[currUnknown] = i;

                                min = Math.Min(min, FindSolution(vals, unknownIndex + 1, minimalEx, simplifiedEqs, unknowns, maxes));

                                if (unknownIndex + 1 < unknowns.Count)
                                {
                                        vals[unknowns[unknownIndex + 1]] = 0;
                                }

                                if (unknownIndex == 0 && unknowns.Count > 3)
                                {
                                        long currTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                        long duration = currTime - startTime;
                                        startTime = currTime;
                                        Console.WriteLine(i + "\t" + duration);
                                }
                        }

                        return min;
                }

                private bool CheckViable(Dictionary<int, float> vals, Equation[] simplifiedEqs)
                {
                        for (int i = 1; i < simplifiedEqs.Length; i++)
                        {
                                if (simplifiedEqs[i].right.Evaluate(vals) < -0.0000001) return false;
                        }

                        return true;
                }

                private List<Equation> GenerateEquations(List<List<int>> buttons, int[] voltages)
                {
                        List<Equation> result = new();

                        for (int v = 0; v < voltages.Length; v++)
                        {
                                Equation eq = new(buttons.Count + 1);
                                eq.right.vars[0] = voltages[v]; //put the constant in the right side

                                for (int b = 0; b < buttons.Count; b++)
                                {
                                        if (buttons[b].Contains(v))
                                        {
                                                //put the button on the left side
                                                eq.left.vars[b + 1] = 1;
                                        }
                                }

                                eq.SolveFor(0);

                                //don't add a duplicate equation
                                if (result.Contains(eq)) continue;

                                result.Add(eq);
                        }

                        return result;
                }

                private List<EquationSet> GenerateEquationSet(List<Equation> eqs)
                {
                        List<Equation> explodedEqs = new();

                        for (int i = 0; i < eqs.Count; i++)
                        {
                                explodedEqs.Add(eqs[i]);
                        }

                        for (int i = 0; i < eqs.Count; i++)
                        {
                                Equation first = eqs[i];
                                for (int j = i + 1; j < eqs.Count; j++)
                                {
                                        Equation second = eqs[j];

                                        Expression newEx = first.right.Copy();
                                        newEx.Subtract(second.right);
                                        Equation newEq = new(newEx);

                                        if (explodedEqs.Contains(newEq)) continue;
                                        explodedEqs.Add(newEq);
                                }
                        }



                        List<EquationSet> result = new();
                        foreach (Equation eq in explodedEqs) result.Add(new EquationSet(eq));

                        return result;
                }

                private (Expression minimalEx, Equation[] simplifiedEqs, HashSet<int> unknowns) FindMinimalExpression(List<EquationSet> eqSets)
                {
                        //breadth first search of minimal set of unknowns
                        //when found return an expression representing what to minimize

                        int size = eqSets[0].containsVar.Length;

                        List<HashSet<int>> search = new();
                        search.Add(new());

                        (Expression minimalEx, Equation[] simplifiedEqs) result = (null, null);

                        while (search.Count > 0)
                        {
                                HashSet<int> curr = search[0];
                                search.RemoveAt(0);

                                result = FindMinimalExpression(curr, eqSets);

                                if (result.minimalEx != null) return (result.minimalEx, result.simplifiedEqs, curr);

                                for (int i = 1; i < size; i++)
                                {
                                        HashSet<int> newOption = new HashSet<int>(curr);
                                        newOption.Add(i);

                                        if (newOption.Count == curr.Count) continue; // didn't add anything new

                                        search.Add(newOption);
                                }
                        }

                        return (null, null, null); // failed
                }

                private (Expression minimalEx, Equation[] simplifiedEqs) FindMinimalExpression(HashSet<int> unknowns, List<EquationSet> eqSets)
                {
                        int size = eqSets[0].containsVar.Length;
                        Equation[] equations = new Equation[size];

                        //create an equation for each known variable
                        //for each known variable, find an eq that contains it
                        //substitute each other equation until the variable is written in terms of only unknowns

                        for (int v = 1; v < size; v++)
                        {
                                if (unknowns.Contains(v))
                                {
                                        //one of the unknowns, use itself
                                        Equation unknownEq = new Equation(size);
                                        unknownEq.left.vars[v] = 1;
                                        unknownEq.right.vars[v] = 1;
                                        equations[v] = unknownEq;
                                        continue;
                                }

                                int eqId = -1;
                                for (int e = 0; e < eqSets.Count; e++)
                                {
                                        if (eqSets[e].containsVar[v])
                                        {
                                                eqId = e;
                                                break;
                                        }
                                }

                                if (eqId == -1)
                                {
                                        return (null, null); //couldn't create an equation for one of the knowns
                                }

                                Equation copy = eqSets[eqId].set[v].Copy();

                                bool[] usedEq = new bool[eqSets.Count];
                                usedEq[eqId] = true;

                                while (HasKnowns(copy.right, unknowns))// && !AllTrue(usedEq))
                                {
                                        // for each variable in the equation that is not an unknown,
                                        // substitute it from one of the other equations
                                        for (int depV = 1; depV < size; depV++)
                                        {
                                                if (v == depV) continue; // self reference
                                                if (!copy.right.ContainsVar(depV)) continue; // not in the equation
                                                if (unknowns.Contains(depV)) continue; //unknown, no need to substitute

                                                if (equations[depV] != null)
                                                {
                                                        //already have an equation using only unknowns, best option

                                                        copy.right.Substitute(depV, equations[depV].right);

                                                        continue;
                                                }

                                                bool foundEq = false;

                                                //actual dependent variable, find an equation to substitute
                                                for (int e = 0; e < eqSets.Count; e++)
                                                {
                                                        if (usedEq[e]) continue; // equation already used

                                                        EquationSet currEqSet = eqSets[e];
                                                        if (!currEqSet.containsVar[depV]) continue; // equation doesn't have the var
                                                        if (currEqSet.containsVar[v]) continue; // can't substitute an equation for the var we're trying to define

                                                        //substitute
                                                        copy.right.Substitute(depV, eqSets[e].set[depV].right);
                                                        usedEq[e] = true;
                                                        foundEq = true;
                                                        break;
                                                }

                                                if (foundEq) continue;

                                                return (null, null); //not able to find an equation to substitute for this know var
                                        }
                                }

                                if (HasKnowns(copy.right, unknowns))
                                {
                                        return (null, null); //can't rewrite eqs in terms of only unknowns
                                }

                                equations[v] = copy;
                        }

                        Expression result = equations[1].right.Copy();

                        for (int v = 2; v < size; v++)
                        {
                                result.Add(equations[v].right);
                        }

                        return (result, equations);
                }

                private bool HasKnowns(Expression ex, HashSet<int> unknowns)
                {
                        for (int i = 1; i < ex.vars.Length; i++)
                        {
                                if (ex.ContainsVar(i) && !unknowns.Contains(i)) return true;
                        }

                        return false;
                }

                private bool AllTrue(bool[] check)
                {
                        for (int i = 0; i < check.Length; i++)
                        {
                                if (!check[i]) return false;
                        }

                        return true;
                }
        }
}