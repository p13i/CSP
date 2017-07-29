using System;
using System.Linq;
using System.Collections.Generic;

namespace ConstraintSatisfactionProblem
{
    public static class ValueHeuristics
    {
        public static List<int> TrivialOrderValues(IAssignment assignment, CSP csp, string variable)
        {
            return new List<int>(assignment.VariableDomains[variable]);
        }
    }

    public static class VariableHeuristics
    {
        public static string ChooseFirstVariable(IAssignment assignment, CSP csp)
        {
            return csp.VarDomains.Keys
                .Where(variable => !assignment.IsVariableAssigned(variable))
                .FirstOrDefault();
        }

        public static string ChooseRandomVariable(IAssignment assignment, CSP csp)
        {
            return csp.VarDomains.Keys
                .Where(variable => !assignment.IsVariableAssigned(variable))
                .OrderBy(variable => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}