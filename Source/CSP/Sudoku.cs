using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConstraintSatisfactionProblem
{
    public abstract class Sudoku
    {
        protected CSP Csp;
        protected abstract int[] Domain { get; }
        protected abstract string[][] Grid { get; }
        protected IAssignment Assignment;
        protected IDictionary<string, ICollection<int>> VariableDomains = new Dictionary<string, ICollection<int>>();
        public static readonly int NoValue = 0;

        protected Sudoku()
        {
            foreach (var row in Grid)
            {
                foreach (var variable in row)
                {
                    VariableDomains[variable] = new List<int>(Domain);
                }
            }

            Csp = new CSP(VariableDomains, Constraints);
            Assignment = new Assignment(Csp);
        }

        protected abstract List<IConstraint> Constraints { get; }

        public int GenerateSolution()
        {
            ISolver solver = new RecusiveBacktrackingSolver(ValueHeuristics.TrivialOrderValues, VariableHeuristics.ChooseFirstVariable);
            IAssignment resultantAssignment = solver.Solve(Csp, Assignment);
            
            Assignment = resultantAssignment;
            
            if (Assignment == default(IAssignment))
            {
                throw new Exception($"Couldn't find solution, even with {solver.NumberOfSteps} steps.");
            }
            
            return solver.NumberOfSteps;
        }

        public new string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int gridRowNumber = 0;
            foreach (string[] row in Grid)
            {
                gridRowNumber++;

                sb.Append(" ");

                var gridColumnNumber = 0;
                foreach (var variable in row)
                {
                    gridColumnNumber++;
                    if (Assignment.ContainsKey(variable))
                    {
                        sb.Append($"{Assignment[variable]} ");
                    }
                    else
                    {
                        sb.Append(". ");
                    }

                    if (gridColumnNumber % 3 == 0 && gridColumnNumber < 9) {
                        sb.Append("| ");
                    }
                }
                sb.Append("\n");
                
                if (gridRowNumber % 3 == 0 && gridRowNumber < 9) {
                    sb.Append("-------|-------|-------\n");
                }
            }
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {   
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            Sudoku other = (Sudoku) obj;
            return other.ToString().Equals(ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

    public sealed class RegularSudoku : Sudoku
    {
        protected override int[] Domain { get; } = {1, 2, 3, 4, 5, 6, 7, 8, 9};

        protected override string[][] Grid { get; } =
        {
            new[] {"a1", "a2", "a3", "a4", "a5", "a6", "a7", "a8", "a9"},
            new[] {"b1", "b2", "b3", "b4", "b5", "b6", "b7", "b8", "b9"},
            new[] {"c1", "c2", "c3", "c4", "c5", "c6", "c7", "c8", "c9"},

            new[] {"d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8", "d9"},
            new[] {"e1", "e2", "e3", "e4", "e5", "e6", "e7", "e8", "e9"},
            new[] {"f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9"},

            new[] {"g1", "g2", "g3", "g4", "g5", "g6", "g7", "g8", "g9"},
            new[] {"h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8", "h9"},
            new[] {"i1", "i2", "i3", "i4", "i5", "i6", "i7", "i8", "i9"},
        };

        protected override List<IConstraint> Constraints
        {
            get
            {
                List<IConstraint> constraints = Grid
                    .Select(row => new AllDifferentConstraint(row))
                    .Cast<IConstraint>()
                    .ToList();

                // Set all row constraints

                // Set all column constraints
                for (var i = 0; i < 9; i++)
                {
                    var columnVariables = new string[9];
                    for (var j = 0; j < 9; j++)
                    {
                        columnVariables[j] = Grid[j][i];
                    }
                    constraints.Add(new AllDifferentConstraint(columnVariables));
                }

                // Set box constraints
                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 3; j++)
                    {
                        var variables = new string[9];
                        for (var k = 0; k < 3; k++)
                        {
                            for (var l = 0; l < 3; l++)
                            {
                                variables[3 * k + l] = Grid[i * 3 + k][j * 3 + l];
                            }
                        }
                        constraints.Add(new AllDifferentConstraint(variables));
                    }
                }

                return constraints;
            }
        }

        public RegularSudoku(int[][] initialValues)
        {
            Utilities.EnsureDimensions(initialValues, 9, 9);

            var numberOfCluesProvided = 0;
            
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (initialValues[i][j] == NoValue) continue;
                    
                    numberOfCluesProvided++;
                        
                    char letter = (char) ('a' + i);
                    string variable = $"{letter}{j + 1}";
                    Assignment[variable] = initialValues[i][j];
                }
            }

            if (numberOfCluesProvided <= 16)
            {
                // https://arxiv.org/abs/1201.0749
                throw new Exception("Insufficient number of clues given.");
            }
        }

        public RegularSudoku(string filename) : this(FromFile(filename))
        {
        }

        public static int[][] FromFile(string filename) {
            List<int[]> rows = new List<int[]>();

            string[] gridRows = File.ReadAllLines(filename);

            foreach (string gridRow in gridRows) {
                
                if (gridRow.StartsWith("-") || gridRow.Length == 0) 
                {
                    continue;
                }
                
                string cleanedRow = gridRow
                    .Replace(" ", "")
                    .Replace("|", "")
                    .Replace(".", NoValue.ToString());

                int[] rowValues = cleanedRow
                    .Select(x => int.Parse(x.ToString()))
                    .ToArray();
                
                rows.Add(rowValues);
            }

            int[][] grid = rows.ToArray();
            return grid;
        }
    }

    public sealed class MediumSudoku : Sudoku
    {
        protected override int[] Domain { get; } = {1, 2, 3, 4, 5, 6, 7, 8, 9};

        protected override string[][] Grid { get; } =
        {
            new[] {"a", "b", "c"},
            new[] {"d", "e", "f"},
            new[] {"g", "h", "i"},
        };

        protected override List<IConstraint> Constraints => new List<IConstraint>()
        {
            // Row 1
            new AllDifferentConstraint("a", "b", "c"),

            // Row 2
            new AllDifferentConstraint("d", "e", "f"),

            // Row 3
            new AllDifferentConstraint("g", "h", "i"),

            // Col 1
            new AllDifferentConstraint("a", "d", "g"),

            // Col 2
            new AllDifferentConstraint("b", "e", "h"),

            // Col 3
            new AllDifferentConstraint("c", "f", "i"),
        };
    }

    public sealed class SmallSudoku : Sudoku
    {
        protected override int[] Domain { get; } = {1, 2, 3, 4};

        protected override string[][] Grid { get; } =
        {
            new[] {"a", "b"},
            new[] {"c", "d"},
        };

        protected override List<IConstraint> Constraints => new List<IConstraint>
        {
            // Row 1
            new AllDifferentConstraint("a", "b"),
            // Row 2
            new AllDifferentConstraint("c", "d"),
            // Col 1
            new AllDifferentConstraint("a", "c"),
            // Col 2
            new AllDifferentConstraint("b", "d"),
        };
    }
}