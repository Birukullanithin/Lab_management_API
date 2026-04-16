using System;

namespace LabManagement.Dtos
{
    public class TestMasterDto
    {
        public int? TestId { get; set; }

        public string TestName { get; set; } = null!;

        public string? TestCode { get; set; }

        public decimal? NormalMin { get; set; }

        public decimal? NormalMax { get; set; }

        public string? Unit { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}