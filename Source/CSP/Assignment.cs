using System.Collections.Generic;
using System.Linq;
using ConstraintSatisfactionProblem;


namespace ConstraintSatisfactionProblem
{
    public interface IAssignment : IDictionary<string, int>
    {
        bool IsComplete();
        bool IsVariableAssigned(string variable);
        IDictionary<string, ISet<int>> VariableDomains { get; }
    }

    public class Assignment : Dictionary<string, int>, IAssignment
    {
        public CSP CSP { get; }

        public bool IsComplete()
        {
            return CSP.VarDomains.Keys.All(IsVariableAssigned);
        }

        public bool IsVariableAssigned(string variable)
        {
            return ContainsKey(variable);
        }

        public IDictionary<string, ISet<int>> VariableDomains { get; } = new Dictionary<string, ISet<int>>();

        public Assignment(CSP csp)
        {
            CSP = csp;
            foreach (string variable in CSP.VarDomains.Keys)
            {
                VariableDomains[variable] = new HashSet<int>(csp.VarDomains[variable]);
            }
        }
    }
}