using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabManagement.Controllers
{
    [ApiController]
    [Route("api/legal-entities")]
    public class LegalEntityController : ControllerBase
    {
        private readonly ILegalEntityService _legalEntityService;

        public LegalEntityController(ILegalEntityService legalEntityService)
        {
            _legalEntityService = legalEntityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LegalEntityDto>>> GetAllLegalEntities()
        {
            var legalEntities = await _legalEntityService.GetAllLegalEntitiesAsync();
            return Ok(legalEntities);
        }

        [HttpGet("{legalEntityId}")]
        public async Task<ActionResult<LegalEntityDto>> GetLegalEntity(int legalEntityId)
        {
            var legalEntity = await _legalEntityService.GetLegalEntityByIdAsync(legalEntityId);
            if (legalEntity == null)
            {
                return NotFound();
            }

            return Ok(legalEntity);
        }

        [HttpPost]
        public async Task<ActionResult<LegalEntityDto>> CreateLegalEntity([FromBody] LegalEntityDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _legalEntityService.CreateLegalEntityAsync(dto);
            return CreatedAtAction(nameof(GetLegalEntity), new { legalEntityId = created.LegalEntityId }, created);
        }

        [HttpPut("{legalEntityId}")]
        public async Task<IActionResult> UpdateLegalEntity(int legalEntityId, [FromBody] LegalEntityDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _legalEntityService.UpdateLegalEntityAsync(legalEntityId, dto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
