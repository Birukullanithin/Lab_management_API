using System;
using System.Collections.Generic;
using LabManagement.Interfaces;

namespace LabManagement.Services
{
    public class EgfrCalculator : IFormulaCalculator
    {
        public string FormulaName => "EGFR";

        public decimal Calculate(IReadOnlyDictionary<string, decimal> inputs)
        {
            var serumCreatinine = GetRequired(inputs, "serum creatinine");
            var age = GetRequired(inputs, "age");
            var femaleFactor = inputs.TryGetValue("female factor", out var factor) ? factor : 1m;
            if (serumCreatinine <= 0)
            {
                throw new InvalidOperationException("Serum creatinine must be greater than zero.");
            }

            var value = 175m
                        * Pow(serumCreatinine, -1.154m)
                        * Pow(age, -0.203m)
                        * femaleFactor;

            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private static decimal GetRequired(IReadOnlyDictionary<string, decimal> inputs, string key)
        {
            if (!inputs.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing required formula input '{key}'.");
            }

            return value;
        }

        private static decimal Pow(decimal value, decimal exponent)
        {
            return (decimal)Math.Pow((double)value, (double)exponent);
        }
    }
}
