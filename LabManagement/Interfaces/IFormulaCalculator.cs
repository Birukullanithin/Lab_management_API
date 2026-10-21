using System.Collections.Generic;

namespace LabManagement.Interfaces
{
    public interface IFormulaCalculator
    {
        string FormulaName { get; }

        decimal Calculate(IReadOnlyDictionary<string, decimal> inputs);
    }
}
