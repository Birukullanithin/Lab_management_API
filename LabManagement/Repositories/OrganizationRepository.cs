using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Data;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly LabDbContext _dbContext;

        public OrganizationRepository(LabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            var sql = @"SELECT organization_id, organization_name, organization_code, organization_address,
                               organization_email, organization_phone, organization_logo, organization_description,
                               is_active, created_at, updated_at
                        FROM lab.organization
                        ORDER BY organization_id";

            return await _dbContext.QueryAsync<Organization>(sql, null);
        }

        public async Task<Organization?> GetOrganizationByIdAsync(int organizationId)
        {
            var sql = @"SELECT organization_id, organization_name, organization_code, organization_address,
                               organization_email, organization_phone, organization_logo, organization_description,
                               is_active, created_at, updated_at
                        FROM lab.organization
                        WHERE organization_id = @OrganizationId";

            return await _dbContext.QuerySingleAsync<Organization>(
                sql,
                new Dictionary<string, object?> { { "OrganizationId", organizationId } });
        }

        public async Task<Organization> CreateOrganizationAsync(Organization organization)
        {
            var sql = @"INSERT INTO lab.organization
                        (organization_name, organization_code, organization_address, organization_email,
                         organization_phone, organization_logo, organization_description, is_active, created_at, updated_at)
                        VALUES
                        (@OrganizationName, @OrganizationCode, @OrganizationAddress, @OrganizationEmail,
                         @OrganizationPhone, @OrganizationLogo, @OrganizationDescription, @IsActive, @CreatedAt, @UpdatedAt)
                        RETURNING organization_id;";

            var parameters = new Dictionary<string, object?>
            {
                { "OrganizationName", organization.OrganizationName },
                { "OrganizationCode", organization.OrganizationCode },
                { "OrganizationAddress", organization.OrganizationAddress },
                { "OrganizationEmail", organization.OrganizationEmail },
                { "OrganizationPhone", organization.OrganizationPhone },
                { "OrganizationLogo", organization.OrganizationLogo },
                { "OrganizationDescription", organization.OrganizationDescription },
                { "IsActive", organization.IsActive },
                { "CreatedAt", organization.CreatedAt },
                { "UpdatedAt", organization.UpdatedAt }
            };

            var id = await _dbContext.ExecuteScalarAsync<int>(sql, parameters);
            organization.OrganizationId = id;
            return organization;
        }

        public async Task UpdateOrganizationAsync(Organization organization)
        {
            var sql = @"UPDATE lab.organization
                        SET organization_name = @OrganizationName,
                            organization_code = @OrganizationCode,
                            organization_address = @OrganizationAddress,
                            organization_email = @OrganizationEmail,
                            organization_phone = @OrganizationPhone,
                            organization_logo = @OrganizationLogo,
                            organization_description = @OrganizationDescription,
                            is_active = @IsActive,
                            updated_at = @UpdatedAt
                        WHERE organization_id = @OrganizationId";

            var parameters = new Dictionary<string, object?>
            {
                { "OrganizationId", organization.OrganizationId },
                { "OrganizationName", organization.OrganizationName },
                { "OrganizationCode", organization.OrganizationCode },
                { "OrganizationAddress", organization.OrganizationAddress },
                { "OrganizationEmail", organization.OrganizationEmail },
                { "OrganizationPhone", organization.OrganizationPhone },
                { "OrganizationLogo", organization.OrganizationLogo },
                { "OrganizationDescription", organization.OrganizationDescription },
                { "IsActive", organization.IsActive },
                { "UpdatedAt", organization.UpdatedAt }
            };

            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }
    }
}
