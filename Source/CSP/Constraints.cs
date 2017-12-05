using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstraintSatisfactionProblem
{
    public interface IConstraint
    {
        ICollection<string> Variables { get; }
        bool Affects(string variable);
        bool IsSatisfiedByValue(int proposedValue, IAssignment inAssignment);
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

        public bool IsSatisfiedByValue(int proposedValue, IAssignment inAssignment)
        {
            return Variables
                .All(var => !inAssignment.IsAssigned(var) || inAssignment.GetValue(var) != proposedValue);
        }
    }
}