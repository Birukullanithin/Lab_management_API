using System;
using System.Collections.Generic;
using LabManagement.Interfaces;

namespace LabManagement.Services
{
    public class BmiCalculator : IFormulaCalculator
    {
        public string FormulaName => "BMI";

        public decimal Calculate(IReadOnlyDictionary<string, decimal> inputs)
        {
            var weight = GetRequired(inputs, "weight");
            var height = GetRequired(inputs, "height");
            if (height == 0)
            {
                throw new InvalidOperationException("Height must be greater than zero.");
            }

            return Math.Round(weight / (height * height), 2, MidpointRounding.AwayFromZero);
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
