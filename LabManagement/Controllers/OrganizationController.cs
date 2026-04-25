using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabManagement.Controllers
{
    [ApiController]
    [Route("api/organizations")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationDto>>> GetAllOrganizations()
        {
            var organizations = await _organizationService.GetAllOrganizationsAsync();
            return Ok(organizations);
        }

        [HttpGet("{organizationId}")]
        public async Task<ActionResult<OrganizationDto>> GetOrganization(int organizationId)
        {
            var organization = await _organizationService.GetOrganizationByIdAsync(organizationId);
            if (organization == null)
            {
                return NotFound();
            }

            return Ok(organization);
        }

        [HttpPost]
        public async Task<ActionResult<OrganizationDto>> CreateOrganization([FromBody] OrganizationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _organizationService.CreateOrganizationAsync(dto);
            return CreatedAtAction(nameof(GetOrganization), new { organizationId = created.OrganizationId }, created);
        }

        [HttpPut("{organizationId}")]
        public async Task<IActionResult> UpdateOrganization(int organizationId, [FromBody] OrganizationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _organizationService.UpdateOrganizationAsync(organizationId, dto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
