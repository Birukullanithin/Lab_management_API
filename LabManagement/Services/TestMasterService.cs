using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Services
{
    public class TestMasterService : ITestMasterService
    {
        private readonly ITestMasterRepository _testMasterRepository;

        public TestMasterService(ITestMasterRepository testMasterRepository)
        {
            _testMasterRepository = testMasterRepository;
        }

        public Task<TestMaster?> GetByIdAsync(int testId) => _testMasterRepository.GetByIdAsync(testId);

        public Task<IEnumerable<TestMaster>> GetAllAsync() => _testMasterRepository.GetAllAsync();

        public Task<List<TestMaster>> GetByNamesAsync(List<string> testNames) => _testMasterRepository.GetByNamesAsync(testNames);

        public Task<List<TestMaster>> GetByIdsAsync(IEnumerable<int> testIds) => _testMasterRepository.GetByIdsAsync(testIds);

        public async Task<TestMasterDto?> GetDtoByIdAsync(int testId)
        {
            var test = await _testMasterRepository.GetByIdAsync(testId);
            return test == null ? null : MapToDto(test);
        }

        public async Task<IEnumerable<TestMasterDto>> GetAllDtosAsync()
        {
            var tests = await _testMasterRepository.GetAllAsync();
            return tests.Select(MapToDto).ToList();
        }

        private static TestMasterDto MapToDto(TestMaster test)
        {
            return new TestMasterDto
            {
                TestId = test.TestId,
                TestName = test.TestName,
                TestCode = test.TestCode,
                NormalMin = test.NormalMin,
                NormalMax = test.NormalMax,
                Unit = test.Unit,
                Description = test.Description,
                IsActive = test.IsActive
            };
        }
    }
}
