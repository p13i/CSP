using System.Collections.Generic;

namespace ConstraintSatisfactionProblem
{
    public class CSP
    {
        public List<IConstraint> Constraints;
        public IDictionary<string, ICollection<int>> VarDomains;

        public CSP(IDictionary<string, ICollection<int>> varDomains, List<IConstraint> constraints)
        {
            VarDomains = varDomains;
            Constraints = constraints;
        }
    }
}