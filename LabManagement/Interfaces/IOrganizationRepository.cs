using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Models;

namespace LabManagement.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
        Task<Organization?> GetOrganizationByIdAsync(int organizationId);
        Task<Organization> CreateOrganizationAsync(Organization organization);
        Task UpdateOrganizationAsync(Organization organization);
    }
}
