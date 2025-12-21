namespace AOC2025
{
        public class Day10B
        {
                const double EPSILON = 0.000000001;
                private class Expression
                {
                        public double[] vars; // index 0 is always the constant

                        public Expression(int size)
                        {
                                vars = new double[size];
                        }

                        public bool ContainsVar(int var)
                        {
                                return Math.Abs(vars[var]) > EPSILON;
                        }

                        public int CountVars()
                        {
                                int count = 0;
                                for (int i = 1; i < vars.Length; i++)
                                {
                                        if (ContainsVar(i)) count++;
                                }

                                return count;
                        }

                        public void Substitute(int var, Expression e)
                        {
                                for (int v = 0; v < vars.Length; v++)
                                {
                                        vars[v] += vars[var] * e.vars[v];
                                }

                                vars[var] = 0;
                        }

                        public void Substitute(int var, double val)
                        {
                                vars[0] += vars[var] * val;
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

                        public void Multiply(double val)
                        {
                                for (int v = 0; v < vars.Length; v++)
                                {
                                        vars[v] *= val;
                                }
                        }

                        public void Divide(double val)
                        {
                                for (int v = 0; v < vars.Length; v++)
                                {
                                        vars[v] /= val;
                                }
                        }

                        public double Evaluate(double[] vals)
                        {
                                double sum = vars[0];
                                for (int i = 1; i < vars.Length; i++)
                                {
                                        sum += vals[i] * vars[i];
                                }

                                return sum;
                        }

                        public double Evaluate(Dictionary<int, double> values)
                        {
                                double sum = vars[0];
                                foreach (KeyValuePair<int, double> value in values)
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

                                //try subtracting the equations
                                bool result = true;
                                for (int i = 0; i < vars.Length; i++)
                                {
                                        if (Math.Abs(ex.vars[i] - vars[i]) > EPSILON) result = false;
                                }

                                //try adding the equations
                                if (!result)
                                {
                                        result = true;
                                        for (int i = 0; i < vars.Length; i++)
                                        {
                                                if (Math.Abs(ex.vars[i] + vars[i]) > EPSILON) result = false;
                                        }
                                }

                                return result;
                        }


                        public override int GetHashCode()
                        {
                                return vars.GetHashCode();
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
                                                MoveLeft(i);
                                        }
                                        else
                                        {
                                                MoveRight(i);
                                        }
                                }

                                if (var == 0) return;

                                if (!left.ContainsVar(var)) return;

                                right.Divide(left.vars[var]);
                                left.Divide(left.vars[var]);
                        }

                        public int Solve()
                        {
                                if (CountVars() != 1) return -1;

                                MoveRight(0); //move the constant to the right side
                                for (int i = 1; i < size; i++)
                                {
                                        if (right.ContainsVar(i))
                                        {
                                                MoveLeft(i);
                                                return i;
                                        }
                                }

                                return -2;
                        }
                        public void Substitute(int var, Expression e)
                        {
                                left.Substitute(var, e);
                                right.Substitute(var, e);
                        }

                        public void MoveLeft(int var)
                        {
                                left.vars[var] -= right.vars[var];
                                right.vars[var] = 0;
                        }

                        public void MoveRight(int var)
                        {
                                right.vars[var] -= left.vars[var];
                                left.vars[var] = 0;
                        }

                        public bool ContainsVar(int var)
                        {
                                return left.ContainsVar(var) || right.ContainsVar(var);
                        }

                        public int CountVars()
                        {
                                return left.CountVars() + right.CountVars();
                        }

                        public void Substitute(int var, double val)
                        {
                                left.Substitute(var, val);
                                right.Substitute(var, val);
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

                        public override int GetHashCode()
                        {
                                return HashCode.Combine(left, right);
                        }

                        public override bool Equals(object? obj)
                        {
                                if (obj == null || obj.GetType() != typeof(Equation)) return false;

                                Equation eq = (Equation)obj;

                                return left.Equals(eq.left) && right.Equals(eq.right);
                        }

                }

                public void Solve(List<string> data)
                {
                        long sum = 0;
                        for (int l = 0; l < data.Count; l++)
                        {
                                string line = data[l];
                                sum += Solve(line, l + 1);
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
                        int size = buttons.Count + 1;

                        //voltage
                        string voltageString = parts[parts.Length - 1];
                        string[] voltageParts = voltageString.Substring(1, voltageString.Length - 2).Split(',');
                        int[] voltages = new int[voltageParts.Length];
                        for (int i = 0; i < voltageParts.Length; i++)
                        {
                                voltages[i] = int.Parse(voltageParts[i]);
                        }

                        Console.Write(lineNum);

                        //equations
                        List<Equation> eqs = GenerateEquations(buttons, voltages);

                        List<int> unknowns = new();

                        //try to solve, capture unknowns if not
                        var solvedSystem = SolveSystem(CopyEquations(eqs));
                        if (!solvedSystem.success)
                        {
                                for (int i = 1; i < size; i++)
                                {
                                        if (solvedSystem.result[i] == null) unknowns.Add(i);
                                }
                        }
                        else
                        {
                                double answer = SumSystem(solvedSystem.result);

                                if (!CheckValid(eqs, ConvertToVars(solvedSystem.result))) Console.WriteLine("ERROR!!!");

                                Console.Write("\t: 0\t- " + answer);
                                Console.WriteLine("\t- " + (int)(answer + 0.5));
                                return (int)(answer + 0.5);
                        }

                        Console.Write("\t: " + unknowns.Count);


                        //max values
                        double[] maxes = new double[size];
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

                        // search for the answer
                        Dictionary<int, double> vals = new();
                        foreach (int i in unknowns)
                        {
                                vals.Add(i, 0);
                        }

                        double result = Search(vals, 0, eqs, unknowns, maxes);
                        Console.Write("\t- " + result);

                        Console.WriteLine("\t- " + (int)(result + 0.5));

                        return (int)(result + 0.5);
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

                private double Search(Dictionary<int, double> vals, int unknownIndex, List<Equation> eqs,
                        List<int> unknowns, double[] maxes)
                {

                        if (unknownIndex >= unknowns.Count)
                        {
                                // build the equation list
                                List<Equation> newEqs = CopyEquations(eqs);

                                //add equations for the unknowns
                                foreach (KeyValuePair<int, double> val in vals)
                                {
                                        Equation newEq = new(maxes.Length);
                                        newEq.right.vars[0] = val.Value;
                                        newEq.right.vars[val.Key] = -1;
                                        newEqs.Add(newEq);
                                }

                                // solve the system of equations
                                Equation[] solvedEqs = SolveSystem(newEqs).result;

                                if (!CheckValid(eqs, ConvertToVars(solvedEqs))) return double.MaxValue;

                                // sum the result
                                double result = SumSystem(solvedEqs);
                                return result;
                        }

                        int currUnknown = unknowns[unknownIndex];

                        double min = int.MaxValue;

                        for (int i = 0; i <= maxes[currUnknown]; i++)
                        {
                                vals[currUnknown] = i;
                                int nextUnknownIndex = unknownIndex + 1;

                                min = Math.Min(min, Search(vals, nextUnknownIndex, eqs, unknowns, maxes));

                                if (nextUnknownIndex < unknowns.Count)
                                {
                                        vals[unknowns[nextUnknownIndex]] = 0;
                                }
                        }

                        return min;
                }

                //equations are already copied
                private (bool success, Equation[] result) SolveSystem(List<Equation> eqs)
                {
                        Equation[] solved = new Equation[eqs[0].size];
                        int solvedCount = 0;

                        while (solvedCount < solved.Length - 1)
                        {

                                int[] varCounts = new int[solved.Length];
                                foreach (Equation eq in eqs)
                                {
                                        for (int i = 1; i < varCounts.Length; i++)
                                        {
                                                if (eq.ContainsVar(i)) varCounts[i]++;
                                        }
                                }

                                int minCount = int.MaxValue;
                                int nextVar = int.MaxValue;
                                for (int i = 1; i < varCounts.Length; i++)
                                {
                                        if (varCounts[i] > 0 && varCounts[i] < minCount)
                                        {
                                                nextVar = i;
                                                minCount = varCounts[i];
                                        }
                                }

                                if (nextVar == int.MaxValue) return (false, solved);

                                eqs = eqs.OrderBy(eq => eq.CountVars()).ToList();
                                for (int i = 0; i < eqs.Count; i++)
                                {
                                        Equation eq = eqs[i];
                                        if (eq.ContainsVar(nextVar))
                                        {
                                                eqs.RemoveAt(i);
                                                eq.SolveFor(nextVar);

                                                foreach (Equation currEq in eqs)
                                                {
                                                        currEq.Substitute(nextVar, eq.right);
                                                }

                                                foreach (Equation currEq in solved)
                                                {
                                                        if (currEq == null) continue;
                                                        currEq.Substitute(nextVar, eq.right);
                                                }

                                                solved[nextVar] = eq;
                                                solvedCount++;
                                                break;
                                        }
                                }
                        }

                        for (int i = 1; i < solved.Length; i++)
                        {
                                solved[i].SolveFor(i);
                        }


                        return (true, solved);
                }

                private List<Equation> CopyEquations(List<Equation> original)
                {
                        List<Equation> newEqs = new();
                        foreach (Equation eq in original)
                        {
                                newEqs.Add(eq.Copy());
                        }

                        return newEqs;
                }

                private double[] ConvertToVars(Equation[] system)
                {
                        double[] result = new double[system.Length];
                        for (int i = 1; i < result.Length; i++)
                        {
                                result[i] = system[i].right.vars[0];
                        }

                        return result;
                }

                private bool CheckValid(List<Equation> eqs, double[] vars)
                {
                        foreach (double val in vars)
                        {
                                if (val < -EPSILON) return false; //must be non-negative
                                if (Math.Abs(val - Math.Round(val)) > EPSILON) return false; //must be whole
                        }

                        foreach (Equation eq in eqs)
                        {
                                double left = eq.left.Evaluate(vars);
                                double right = eq.right.Evaluate(vars);

                                if (Math.Abs(left - right) > EPSILON) return false;
                        }

                        return true;
                }

                private double SumSystem(Equation[] system)
                {
                        double sum = 0;
                        for (int i = 1; i < system.Length; i++)
                        {
                                Equation eq = system[i];

                                sum += eq.right.vars[0];
                        }

                        return sum;
                }
        }
}