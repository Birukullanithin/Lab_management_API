namespace LabManagement.Dtos
{
    public class ResultComponentDto
    {
        public string ComponentName { get; set; } = string.Empty;

        public decimal? EnteredValue { get; set; }

        public decimal? ReferenceMin { get; set; }

        public decimal? ReferenceMax { get; set; }

        public decimal? CriticalMin { get; set; }

        public decimal? CriticalMax { get; set; }

        public string Status { get; set; } = "Pending";

        public bool AbnormalFlag { get; set; }

        public bool IsCritical { get; set; }

        public string? Unit { get; set; }

        public string? Method { get; set; }

        public string? FormulaName { get; set; }
    }
}
