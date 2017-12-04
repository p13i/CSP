using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstraintSatisfactionProblem
{
    public interface IConstraint
    {
        ICollection<string> Variables { get; }
        bool Affects(string variable);
        bool SatisfiedByVariableValueInAssignment(string variable, int proposedValue, IAssignment assignment);
    }

    public class DifferingPairConstraint : IConstraint
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
            return !assignment.IsAssigned(otherVariable) || assignment.GetValue(otherVariable) != proposedValue;
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

        private bool WhereFunc(KeyValuePair<string, int> entry) => Affects(entry.Key);
        
        public bool SatisfiedByVariableValueInAssignment(string variable, int proposedValue, IAssignment assignment)
        {
            return assignment.Where(WhereFunc).All(entry => entry.Value != proposedValue);
        }
    }
}