namespace LabManagement.Dtos
{
    public class RangeValidationResultDto
    {
        public string Status { get; set; } = "Pending";

        public bool AbnormalFlag { get; set; }

        public bool IsCritical { get; set; }
    }
}
