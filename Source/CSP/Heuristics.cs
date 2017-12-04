using System;
using System.Linq;
using System.Collections.Generic;

namespace ConstraintSatisfactionProblem
{
    public static class ValueHeuristics
    {
        public static IEnumerable<int> TrivialOrderValues(IAssignment assignment, CSP csp, string variable)
        {
            return assignment.GetDomain(variable);
        }
    }

    public static class VariableHeuristics
    {
        public static string ChooseFirstVariable(IAssignment assignment, CSP csp)
        {
            return csp.VarDomains.Keys
                .FirstOrDefault(variable => !assignment.IsAssigned(variable));
        }

        public static string ChooseRandomVariable(IAssignment assignment, CSP csp)
        {
            return csp.VarDomains.Keys
                .Where(variable => !assignment.IsAssigned(variable))
                .OrderBy(variable => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}