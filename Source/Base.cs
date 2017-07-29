using System;
using System.Collections.Generic;

namespace ConstraintSatisfactionProblem
{
    public class CSP
    {
        public List<IConstraint> Constraints;
        public Dictionary<string, ISet<int>> VarDomains;

        public CSP(Dictionary<string, ISet<int>> varDomains, List<IConstraint> constraints)
        {
            this.VarDomains = varDomains;
            this.Constraints = constraints;
        }
        
        public bool IsVariableValueConsistent(IAssignment assignment, string variable, int value)
        {
            foreach (var constraint in this.Constraints)
            {
                if (constraint.Affects(variable) &&
                    !constraint.SatisfiedByVariableValueInAssignment(variable, value, assignment))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class CSPException : Exception
    {
        
    }
}