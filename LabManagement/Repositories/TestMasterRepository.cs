using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManagement.Data;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Repositories
{
    public class TestMasterRepository : ITestMasterRepository
    {
        private readonly LabDbContext _dbContext;

        public TestMasterRepository(LabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<TestMaster?> GetByIdAsync(int testId)
        {
            const string sql = @"SELECT test_id, test_name, test_code, normal_min, normal_max, unit, description, is_active, created_at
                                 FROM lab.test_master
                                 WHERE test_id = @TestId";

            return _dbContext.QuerySingleAsync<TestMaster>(sql, new Dictionary<string, object?> { { "TestId", testId } });
        }

        public async Task<IEnumerable<TestMaster>> GetAllAsync()
        {
            const string sql = @"SELECT test_id, test_name, test_code, normal_min, normal_max, unit, description, is_active, created_at
                                 FROM lab.test_master
                                 ORDER BY test_name";

            return await _dbContext.QueryAsync<TestMaster>(sql, null);
        }

        public async Task<List<TestMaster>> GetByNamesAsync(List<string> testNames)
        {
            const string sql = @"SELECT test_id, test_name, test_code, normal_min, normal_max, unit, description, is_active, created_at
                                 FROM lab.test_master
                                 WHERE test_name = ANY(@TestNames)
                                   AND is_active = true";

            var parameters = new Dictionary<string, object?>
            {
                { "TestNames", testNames.ToArray() }
            };

            return await _dbContext.QueryAsync<TestMaster>(sql, parameters);
        }

        public async Task<List<TestMaster>> GetByIdsAsync(IEnumerable<int> testIds)
        {
            var distinctIds = testIds.Distinct().ToArray();
            if (distinctIds.Length == 0)
            {
                return new List<TestMaster>();
            }

            const string sql = @"SELECT test_id, test_name, test_code, normal_min, normal_max, unit, description, is_active, created_at
                                 FROM lab.test_master
                                 WHERE test_id = ANY(@TestIds)";

            var parameters = new Dictionary<string, object?>
            {
                { "TestIds", distinctIds }
            };

            return await _dbContext.QueryAsync<TestMaster>(sql, parameters);
        }
    }
}
