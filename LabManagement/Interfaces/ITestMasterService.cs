using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Models;

namespace LabManagement.Interfaces
{
    public interface ITestMasterService
    {
        Task<TestMaster?> GetByIdAsync(int testId);
        Task<IEnumerable<TestMaster>> GetAllAsync();
        Task<List<TestMaster>> GetByNamesAsync(List<string> testNames);
        Task<List<TestMaster>> GetByIdsAsync(IEnumerable<int> testIds);
        Task<TestMasterDto?> GetDtoByIdAsync(int testId);
        Task<IEnumerable<TestMasterDto>> GetAllDtosAsync();
    }
}
