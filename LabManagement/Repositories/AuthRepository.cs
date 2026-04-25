using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Data;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly LabDbContext _dbContext;

        public AuthRepository(LabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuthUser?> GetUserByUsernameAsync(string username)
        {
            var sql = @"SELECT user_id, username, password_hash, email, organization_id,
                               legal_entity_id, role, is_active, created_at
                        FROM lab.users
                        WHERE username = @Username";

            return await _dbContext.QuerySingleAsync<AuthUser>(
                sql,
                new Dictionary<string, object?> { { "Username", username } });
        }
    }
}
