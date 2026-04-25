using System.Collections.Generic;

namespace LabManagement.Dtos
{
    public class TreeNodeResponseDto
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public List<TreeNodeResponseDto> Children { get; set; } = new();
    }
}
