using System;
using System.Collections.Generic;
using LabManagement.Interfaces;

namespace LabManagement.Services
{
    public class LdlCalculator : IFormulaCalculator
    {
        public string FormulaName => "LDL";

        public decimal Calculate(IReadOnlyDictionary<string, decimal> inputs)
        {
            var totalCholesterol = GetRequired(inputs, "total cholesterol");
            var hdl = GetRequired(inputs, "hdl");
            var triglycerides = GetRequired(inputs, "triglycerides");
            return totalCholesterol - hdl - (triglycerides / 5m);
        }

        private static decimal GetRequired(IReadOnlyDictionary<string, decimal> inputs, string key)
        {
            if (!inputs.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing required formula input '{key}'.");
            }

            return value;
        }
    }
}
