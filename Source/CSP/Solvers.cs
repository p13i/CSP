using System;
using System.Collections.Generic;
using OrderValuesMethodType = System.Func<ConstraintSatisfactionProblem.IAssignment, ConstraintSatisfactionProblem.CSP, string, System.Collections.Generic.IEnumerable<int>>;
using SelectVariableMethodType = System.Func<ConstraintSatisfactionProblem.IAssignment, ConstraintSatisfactionProblem.CSP, string>;

namespace ConstraintSatisfactionProblem
{
    public interface ISolver
    {
        OrderValuesMethodType OrderValuesMethod { get; }
        SelectVariableMethodType SelectVariableMethod { get; }
        IAssignment Solve(CSP csp, IAssignment assignment);
        int NumberOfSteps { get; }
    }

    public class RecusiveBacktrackingSolver : ISolver
    {
        public  OrderValuesMethodType OrderValuesMethod { get; }
        public  SelectVariableMethodType SelectVariableMethod { get; }

        public int NumberOfSteps { get; private set; }

        private const int TimeoutNumberOfSteps = 10000000;

        public RecusiveBacktrackingSolver(
            OrderValuesMethodType orderValuesMethod, 
            SelectVariableMethodType selectVariableMethod
        )
        {
            OrderValuesMethod = orderValuesMethod;
            SelectVariableMethod = selectVariableMethod;
        }

        public IAssignment Solve(CSP csp, IAssignment assignment)
        {
            return RecursiveBacktracking(csp, assignment);
        }
        
        private IAssignment RecursiveBacktracking(CSP csp, IAssignment assignment)
        {
            NumberOfSteps++;

            if (NumberOfSteps > TimeoutNumberOfSteps)
            {
                throw new Exception($"Timed out after {TimeoutNumberOfSteps} steps.");
            }
            
            if (assignment.IsComplete())
            {
                return assignment;
            }

            string variable = SelectVariableMethod(assignment, csp);
            foreach (int value in OrderValuesMethod(assignment, csp, variable))
            {
                if (csp.IsVariableValueConsistent(assignment, variable, value))
                {
                    assignment.Assign(variable, value);

                    IAssignment result = RecursiveBacktracking(csp, assignment);

                    if (result != default(IAssignment))
                    {
                        return result;
                    }
                    
                    assignment.Unassign(variable);
                }
            }
            return null;
        }
    }
}