using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Models;

namespace LabManagement.Interfaces
{
    public interface ITestMasterRepository
    {
        Task<TestMaster?> GetByIdAsync(int testId);
        Task<IEnumerable<TestMaster>> GetAllAsync();
        Task<List<TestMaster>> GetByNamesAsync(List<string> testNames);
        Task<List<TestMaster>> GetByIdsAsync(IEnumerable<int> testIds);
    }
}
