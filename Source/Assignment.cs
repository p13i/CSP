using System.Collections.Generic;
using ConstraintSatisfactionProblem;


namespace ConstraintSatisfactionProblem
{
    public interface IAssignment : IDictionary<string, int>
    {
        bool IsComplete();
        bool IsVariableAssigned(string variable);
        Dictionary<string, ISet<int>> VariableDomains { get; }
    }

    public class Assignment : Dictionary<string, int>, IAssignment
    {
        public CSP CSP;

        public bool IsComplete()
        {
            foreach (var variable in this.CSP.VarDomains.Keys)
            {
                if (!this.IsVariableAssigned(variable))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsVariableAssigned(string variable)
        {
            return this.ContainsKey(variable);
        }

        public Dictionary<string, ISet<int>> VariableDomains { get; } = new Dictionary<string, ISet<int>>();

        public Assignment(CSP csp)

        {
            this.CSP = csp;
            foreach (var variable in csp.VarDomains.Keys)
            {
                this.VariableDomains[variable] = new HashSet<int>(csp.VarDomains[variable]);
            }
        }
    }
}