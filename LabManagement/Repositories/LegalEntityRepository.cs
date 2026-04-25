using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Data;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Repositories
{
    public class LegalEntityRepository : ILegalEntityRepository
    {
        private readonly LabDbContext _dbContext;

        public LegalEntityRepository(LabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<LegalEntity>> GetAllLegalEntitiesAsync()
        {
            var sql = @"SELECT legal_entity_id, organization_id, legal_entity_code, legal_entity_name,
                               legal_entity_address, gst_number, contact_number, email, is_active,
                               created_at, updated_at
                        FROM lab.legal_entity
                        ORDER BY legal_entity_id";

            return await _dbContext.QueryAsync<LegalEntity>(sql, null);
        }

        public async Task<LegalEntity?> GetLegalEntityByIdAsync(int legalEntityId)
        {
            var sql = @"SELECT legal_entity_id, organization_id, legal_entity_code, legal_entity_name,
                               legal_entity_address, gst_number, contact_number, email, is_active,
                               created_at, updated_at
                        FROM lab.legal_entity
                        WHERE legal_entity_id = @LegalEntityId";

            return await _dbContext.QuerySingleAsync<LegalEntity>(
                sql,
                new Dictionary<string, object?> { { "LegalEntityId", legalEntityId } });
        }

        public async Task<LegalEntity> CreateLegalEntityAsync(LegalEntity legalEntity)
        {
            var sql = @"INSERT INTO lab.legal_entity
                        (organization_id, legal_entity_code, legal_entity_name, legal_entity_address,
                         gst_number, contact_number, email, is_active, created_at, updated_at)
                        VALUES
                        (@OrganizationId, @LegalEntityCode, @LegalEntityName, @LegalEntityAddress,
                         @GstNumber, @ContactNumber, @Email, @IsActive, @CreatedAt, @UpdatedAt)
                        RETURNING legal_entity_id;";

            var parameters = new Dictionary<string, object?>
            {
                { "OrganizationId", legalEntity.OrganizationId },
                { "LegalEntityCode", legalEntity.LegalEntityCode },
                { "LegalEntityName", legalEntity.LegalEntityName },
                { "LegalEntityAddress", legalEntity.LegalEntityAddress },
                { "GstNumber", legalEntity.GstNumber },
                { "ContactNumber", legalEntity.ContactNumber },
                { "Email", legalEntity.Email },
                { "IsActive", legalEntity.IsActive },
                { "CreatedAt", legalEntity.CreatedAt },
                { "UpdatedAt", legalEntity.UpdatedAt }
            };

            var id = await _dbContext.ExecuteScalarAsync<int>(sql, parameters);
            legalEntity.LegalEntityId = id;
            return legalEntity;
        }

        public async Task UpdateLegalEntityAsync(LegalEntity legalEntity)
        {
            var sql = @"UPDATE lab.legal_entity
                        SET organization_id = @OrganizationId,
                            legal_entity_code = @LegalEntityCode,
                            legal_entity_name = @LegalEntityName,
                            legal_entity_address = @LegalEntityAddress,
                            gst_number = @GstNumber,
                            contact_number = @ContactNumber,
                            email = @Email,
                            is_active = @IsActive,
                            updated_at = @UpdatedAt
                        WHERE legal_entity_id = @LegalEntityId";

            var parameters = new Dictionary<string, object?>
            {
                { "LegalEntityId", legalEntity.LegalEntityId },
                { "OrganizationId", legalEntity.OrganizationId },
                { "LegalEntityCode", legalEntity.LegalEntityCode },
                { "LegalEntityName", legalEntity.LegalEntityName },
                { "LegalEntityAddress", legalEntity.LegalEntityAddress },
                { "GstNumber", legalEntity.GstNumber },
                { "ContactNumber", legalEntity.ContactNumber },
                { "Email", legalEntity.Email },
                { "IsActive", legalEntity.IsActive },
                { "UpdatedAt", legalEntity.UpdatedAt }
            };

            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }
    }
}
