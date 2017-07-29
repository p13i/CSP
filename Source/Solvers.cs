using System;
using System.Collections.Generic;
using System.Reflection;
using ConstraintSatisfactionProblem;

namespace ConstraintSatisfactionProblem
{
    public interface ISolver
    {
        Func<IAssignment, CSP, string, IEnumerable<int>> OrderValuesMethod { get; }
        Func<IAssignment, CSP, string> SelectVariableMethod { get; }
        IAssignment Solve(CSP csp, IAssignment assignment);
        int NumberOfSteps { get; }
    }

    public class RecusiveBacktrackingSolver : ISolver
    {
        public  Func<IAssignment, CSP, string, IEnumerable<int>> OrderValuesMethod { get; }
        public  Func<IAssignment, CSP, string> SelectVariableMethod { get; }
        
        private int _numberOfSteps = 0;
        public int NumberOfSteps => _numberOfSteps;

        private static readonly int TIMEOUT_NUMBER_OF_STEPS = 10000000;
        
        public RecusiveBacktrackingSolver(Func<IAssignment, CSP, string, IEnumerable<int>> orderValuesMethod, Func<IAssignment, CSP, string> selectVariableMethod)
        {
            this.OrderValuesMethod = orderValuesMethod;
            this.SelectVariableMethod = selectVariableMethod;
        }

        public IAssignment Solve(CSP csp, IAssignment assignment)
        {
            return this.RecursiveBacktracking(csp, assignment);
        }
        
        private IAssignment RecursiveBacktracking(CSP csp, IAssignment assignment)
        {
            this._numberOfSteps++;

            if (this._numberOfSteps > TIMEOUT_NUMBER_OF_STEPS)
            {
                throw new Exception($"Timed out after {TIMEOUT_NUMBER_OF_STEPS} steps.");
            }
            
            if (assignment.IsComplete())
            {
                return assignment;
            }

            var variable = this.SelectVariableMethod(assignment, csp);
            foreach (int value in this.OrderValuesMethod(assignment, csp, variable))
            {
                if (csp.IsVariableValueConsistent(assignment, variable, value))
                {
                    assignment[variable] = value;

                    var result = RecursiveBacktracking(csp, assignment);

                    if (result != null)
                    {
                        return result;
                    }
                    assignment.Remove(variable);
                }
            }
            return null;
        }
    }
}