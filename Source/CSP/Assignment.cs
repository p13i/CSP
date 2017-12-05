using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace ConstraintSatisfactionProblem
{
    public interface IAssignment : IEnumerable<KeyValuePair<string, int>>
    {
        bool IsComplete();
        bool IsAssigned(string variable);
        void Unassign(string variable);
        void Assign(string variable, int value);
        int GetValue(string variable);
        ISet<int> GetDomain(string variable);
        bool IsVariableValueConsistent(string variable, int proposedValue);
    }

    public class Assignment : IAssignment
    {
        #region Instance Properties and Constructor
        
        public IDictionary<string, IList<IConstraint>> VarConstraints;
        private readonly IDictionary<string, ISet<int>> VariableDomains = new Dictionary<string, ISet<int>>();
        private readonly IDictionary<string, int> VariableValues;
        
        public Assignment(CSP csp)
        {
            foreach (string variable in csp.VarDomains.Keys)
            {
                VariableDomains[variable] = new HashSet<int>(csp.VarDomains[variable]);
            }
            VariableValues = new Dictionary<string, int>();
            
            
            VarConstraints = new Dictionary<string, IList<IConstraint>>();
            foreach (IConstraint constraint in csp.Constraints)
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
        
        #endregion
        
        public bool IsComplete()
        {
            return VariableDomains.Keys.All(IsAssigned);
        }

        public ISet<int> GetDomain(string variable)
        {
            return VariableDomains[variable];
        }

        public bool IsVariableValueConsistent(string variable, int proposedValue)
        {
            return VarConstraints[variable]
                .All(constraint => constraint.IsSatisfiedByValue(proposedValue, this));
        }

        public void  Assign(string variable, int value)
        {
            VariableValues[variable] = value;
        }
        
        public bool IsAssigned(string variable)
        {
            return VariableValues.ContainsKey(variable);
        }

        public void Unassign(string variable)
        {
            VariableValues.Remove(variable);
        }

        public int GetValue(string variable)
        {
            return VariableValues[variable];
        }

        #region IEnumerable
        
        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return VariableValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        #endregion
    }
}