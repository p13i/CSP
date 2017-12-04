using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConstraintSatisfactionProblem
{
    public interface IConstraint
    {
        ICollection<string> Variables { get; }
        bool Affects(string variable);
        bool SatisfiedByVariableValueInAssignment(string variable, int proposedValue, IAssignment assignment);
    }

    public interface IBinaryConstraint : IConstraint
    {
    }

    public class DifferingPairConstraint : IBinaryConstraint
    {
        public ICollection<string> Variables { get; }
        private readonly string _firstVariable;
        private readonly string _secondVariable;

        public DifferingPairConstraint(string firstVariable, string secondVariable)
        {
            _firstVariable = firstVariable;
            _secondVariable = secondVariable;
            Variables = new []{ _firstVariable, _secondVariable };
        }

        public bool Affects(string variable)
        {
            return _firstVariable.Equals(variable) || _secondVariable.Equals(variable);
        }

        
        public bool SatisfiedByVariableValueInAssignment(string variable, int proposedValue, IAssignment assignment)
        {
            if (!Affects(variable))
            {
                throw new ArgumentException();
            }
            
            string otherVariable = OtherVariable(variable);
            return !assignment.ContainsKey(otherVariable) || assignment[otherVariable] != proposedValue;
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
        public ICollection<string> Variables { get; }
        public AllDifferentConstraint(params string[] variables)
        {
            Variables = new HashSet<string>(variables);
        }
        
        public bool Affects(string variable)
        {
            return Variables.Contains(variable);
        }

        public bool SatisfiedByVariableValueInAssignment(string variable, int proposedValue, IAssignment assignment)
        {
            ISet<int> existingValues = new HashSet<int>();
            foreach (var entry in assignment)
            {
                if (Affects(entry.Key))
                {
                    existingValues.Add(entry.Value);

                    if (entry.Value == proposedValue)
                    {
                        return false;
                    }
                }
            }
            return !existingValues.Contains(proposedValue);
        }
    }
}