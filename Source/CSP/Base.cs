using System.Collections.Generic;
using System.Linq;

namespace ConstraintSatisfactionProblem
{
    public class CSP
    {
        public IDictionary<string, IList<IConstraint>> VarConstraints;
        public List<IConstraint> Constraints;
        public IDictionary<string, ICollection<int>> VarDomains;

        public CSP(IDictionary<string, ICollection<int>> varDomains, List<IConstraint> constraints)
        {
            VarDomains = varDomains;
            Constraints = constraints;
            
            VarConstraints = new Dictionary<string, IList<IConstraint>>();
            foreach (IConstraint constraint in constraints)
            {
                foreach (string variable in constraint.Variables)
                {
                    if (!VarConstraints.ContainsKey(variable))
                    {
                        VarConstraints[variable] = new List<IConstraint>();
                    }
                    VarConstraints[variable].Add(constraint);
                }
            }
        }
        
        public bool IsVariableValueConsistent(IAssignment assignment, string variable, int value)
        {
            return VarConstraints[variable]
                .All(constraint => constraint.SatisfiedByVariableValueInAssignment(variable, value, assignment));
        }
    }
}