using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Dtos;

namespace LabManagement.Interfaces
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDto>> GetAllOrganizationsAsync();
        Task<OrganizationDto?> GetOrganizationByIdAsync(int organizationId);
        Task<OrganizationDto> CreateOrganizationAsync(OrganizationDto dto);
        Task<bool> UpdateOrganizationAsync(int organizationId, OrganizationDto dto);
    }
}
