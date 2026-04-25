using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabManagement.Dtos
{
    public class TreeNodeRequestDto
    {
        [Required]
        public int Value { get; set; }

        public List<TreeNodeRequestDto> Children { get; set; } = new();
    }
}
