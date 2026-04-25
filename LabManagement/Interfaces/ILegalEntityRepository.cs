using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Models;

namespace LabManagement.Interfaces
{
    public interface ILegalEntityRepository
    {
        Task<IEnumerable<LegalEntity>> GetAllLegalEntitiesAsync();
        Task<LegalEntity?> GetLegalEntityByIdAsync(int legalEntityId);
        Task<LegalEntity> CreateLegalEntityAsync(LegalEntity legalEntity);
        Task UpdateLegalEntityAsync(LegalEntity legalEntity);
    }
}
