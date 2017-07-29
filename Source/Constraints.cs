using System;
using System.Collections.Generic;

namespace ConstraintSatisfactionProblem
{
    public interface IConstraint
    {
        bool Affects(string variable);
        bool SatisfiedByVariableValueInAssignment(string variable, int proposedValue, IAssignment assignment);
    }

    public interface IBinaryConstraint : IConstraint
    {
    }

    public class DifferingPairConstraint : IBinaryConstraint
    {
        private readonly string _firstVariable;
        private readonly string _secondVariable;

        public DifferingPairConstraint(string firstVariable, string secondVariable)
        {
            this._firstVariable = firstVariable;
            this._secondVariable = secondVariable;
        }

        public bool Affects(string variable)
        {
            return this._firstVariable.Equals(variable) || this._secondVariable.Equals(variable);
        }

        
        public bool SatisfiedByVariableValueInAssignment(string variable, int proposedValue, IAssignment assignment)
        {
            if (!this.Affects(variable))
            {
                throw new ArgumentException();
            }
            var otherVariable = this.OtherVariable(variable);
            return !assignment.ContainsKey(otherVariable) ||
                   assignment[otherVariable] != proposedValue;
        }

        private string OtherVariable(string firstVariable)
        {
            if (firstVariable.Equals(_firstVariable))
            {
                return _secondVariable;
            }
            if (firstVariable.Equals(_secondVariable))
            {
                return _firstVariable;
            }
            return null;
        }
    }

    public class AllDifferentConstraint : IConstraint
    {
        private readonly ISet<string> _variables;
        public AllDifferentConstraint(params string[] variables)
        {
            this._variables = new HashSet<string>(variables);
        }
        
        public bool Affects(string variable)
        {
            return this._variables.Contains(variable);
        }

        public bool SatisfiedByVariableValueInAssignment(string variable, int proposedValue, IAssignment assignment)
        {
            ISet<int> existingValues = new HashSet<int>();
            foreach (var entry in assignment)
            {
                if (this.Affects(entry.Key))
                {
                    existingValues.Add(entry.Value);
                }
            }
            return !existingValues.Contains(proposedValue);
        }
    }
}