using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Dtos;

namespace LabManagement.Interfaces
{
    public interface ILegalEntityService
    {
        Task<IEnumerable<LegalEntityDto>> GetAllLegalEntitiesAsync();
        Task<LegalEntityDto?> GetLegalEntityByIdAsync(int legalEntityId);
        Task<LegalEntityDto> CreateLegalEntityAsync(LegalEntityDto dto);
        Task<bool> UpdateLegalEntityAsync(int legalEntityId, LegalEntityDto dto);
    }
}
