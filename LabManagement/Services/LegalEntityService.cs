using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Services
{
    public class LegalEntityService : ILegalEntityService
    {
        private readonly ILegalEntityRepository _legalEntityRepository;

        public LegalEntityService(ILegalEntityRepository legalEntityRepository)
        {
            _legalEntityRepository = legalEntityRepository;
        }

        public async Task<IEnumerable<LegalEntityDto>> GetAllLegalEntitiesAsync()
        {
            var legalEntities = await _legalEntityRepository.GetAllLegalEntitiesAsync();
            return legalEntities.Select(MapToDto).ToList();
        }

        public async Task<LegalEntityDto?> GetLegalEntityByIdAsync(int legalEntityId)
        {
            var legalEntity = await _legalEntityRepository.GetLegalEntityByIdAsync(legalEntityId);
            return legalEntity == null ? null : MapToDto(legalEntity);
        }

        public async Task<LegalEntityDto> CreateLegalEntityAsync(LegalEntityDto dto)
        {
            var legalEntity = MapToEntity(dto);
            legalEntity.CreatedAt = dto.CreatedAt == default ? DateTime.UtcNow : dto.CreatedAt;
            legalEntity.UpdatedAt = dto.UpdatedAt;

            var created = await _legalEntityRepository.CreateLegalEntityAsync(legalEntity);
            return MapToDto(created);
        }

        public async Task<bool> UpdateLegalEntityAsync(int legalEntityId, LegalEntityDto dto)
        {
            var existing = await _legalEntityRepository.GetLegalEntityByIdAsync(legalEntityId);
            if (existing == null)
            {
                return false;
            }

            existing.OrganizationId = dto.OrganizationId;
            existing.LegalEntityCode = dto.LegalEntityCode;
            existing.LegalEntityName = dto.LegalEntityName;
            existing.LegalEntityAddress = dto.LegalEntityAddress;
            existing.GstNumber = dto.GstNumber;
            existing.ContactNumber = dto.ContactNumber;
            existing.Email = dto.Email;
            existing.IsActive = dto.IsActive;
            existing.UpdatedAt = dto.UpdatedAt ?? DateTime.UtcNow;

            await _legalEntityRepository.UpdateLegalEntityAsync(existing);
            return true;
        }

        private static LegalEntity MapToEntity(LegalEntityDto dto)
        {
            return new LegalEntity
            {
                LegalEntityId = dto.LegalEntityId,
                OrganizationId = dto.OrganizationId,
                LegalEntityCode = dto.LegalEntityCode,
                LegalEntityName = dto.LegalEntityName,
                LegalEntityAddress = dto.LegalEntityAddress,
                GstNumber = dto.GstNumber,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

        private static LegalEntityDto MapToDto(LegalEntity legalEntity)
        {
            return new LegalEntityDto
            {
                LegalEntityId = legalEntity.LegalEntityId,
                OrganizationId = legalEntity.OrganizationId,
                LegalEntityCode = legalEntity.LegalEntityCode,
                LegalEntityName = legalEntity.LegalEntityName,
                LegalEntityAddress = legalEntity.LegalEntityAddress,
                GstNumber = legalEntity.GstNumber,
                ContactNumber = legalEntity.ContactNumber,
                Email = legalEntity.Email,
                IsActive = legalEntity.IsActive,
                CreatedAt = legalEntity.CreatedAt,
                UpdatedAt = legalEntity.UpdatedAt
            };
        }
    }
}
