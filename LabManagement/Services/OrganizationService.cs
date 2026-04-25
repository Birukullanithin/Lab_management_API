using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<IEnumerable<OrganizationDto>> GetAllOrganizationsAsync()
        {
            var organizations = await _organizationRepository.GetAllOrganizationsAsync();
            return organizations.Select(MapToDto).ToList();
        }

        public async Task<OrganizationDto?> GetOrganizationByIdAsync(int organizationId)
        {
            var organization = await _organizationRepository.GetOrganizationByIdAsync(organizationId);
            return organization == null ? null : MapToDto(organization);
        }

        public async Task<OrganizationDto> CreateOrganizationAsync(OrganizationDto dto)
        {
            var organization = MapToEntity(dto);
            organization.CreatedAt = dto.CreatedAt == default ? DateTime.UtcNow : dto.CreatedAt;
            organization.UpdatedAt = dto.UpdatedAt;

            var created = await _organizationRepository.CreateOrganizationAsync(organization);
            return MapToDto(created);
        }

        public async Task<bool> UpdateOrganizationAsync(int organizationId, OrganizationDto dto)
        {
            var existing = await _organizationRepository.GetOrganizationByIdAsync(organizationId);
            if (existing == null)
            {
                return false;
            }

            existing.OrganizationName = dto.OrganizationName;
            existing.OrganizationCode = dto.OrganizationCode;
            existing.OrganizationAddress = dto.OrganizationAddress;
            existing.OrganizationEmail = dto.OrganizationEmail;
            existing.OrganizationPhone = dto.OrganizationPhone;
            existing.OrganizationLogo = dto.OrganizationLogo;
            existing.OrganizationDescription = dto.OrganizationDescription;
            existing.IsActive = dto.IsActive;
            existing.UpdatedAt = dto.UpdatedAt ?? DateTime.UtcNow;

            await _organizationRepository.UpdateOrganizationAsync(existing);
            return true;
        }

        private static Organization MapToEntity(OrganizationDto dto)
        {
            return new Organization
            {
                OrganizationId = dto.OrganizationId,
                OrganizationName = dto.OrganizationName,
                OrganizationCode = dto.OrganizationCode,
                OrganizationAddress = dto.OrganizationAddress,
                OrganizationEmail = dto.OrganizationEmail,
                OrganizationPhone = dto.OrganizationPhone,
                OrganizationLogo = dto.OrganizationLogo,
                OrganizationDescription = dto.OrganizationDescription,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

        private static OrganizationDto MapToDto(Organization organization)
        {
            return new OrganizationDto
            {
                OrganizationId = organization.OrganizationId,
                OrganizationName = organization.OrganizationName,
                OrganizationCode = organization.OrganizationCode,
                OrganizationAddress = organization.OrganizationAddress,
                OrganizationEmail = organization.OrganizationEmail,
                OrganizationPhone = organization.OrganizationPhone,
                OrganizationLogo = organization.OrganizationLogo,
                OrganizationDescription = organization.OrganizationDescription,
                IsActive = organization.IsActive,
                CreatedAt = organization.CreatedAt,
                UpdatedAt = organization.UpdatedAt
            };
        }
    }
}
