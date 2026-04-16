namespace LabManagement.Dtos
{
    public class OrderDto
    {
        public int? OrderId { get; set; }

        public int? PatientId { get; set; }

        public string? OrderNumber { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? Status { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Remarks { get; set; }
        public List<TestMasterDto> OrderNames { get; set; }
    }
}
