using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabManagement.Controllers
{
    [ApiController]
    [Route("api/results")]
    public class ResultsController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultsController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<ResultResponseDto>> GetByOrderId(int orderId)
        {
            try
            {
                return Ok(await _resultService.GetByOrderIdAsync(orderId));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("save")]
        public async Task<ActionResult<ResultResponseDto>> Save([FromBody] ResultSaveRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await _resultService.SaveAsync(request));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("complete")]
        public async Task<ActionResult<ResultResponseDto>> Complete([FromBody] ResultSaveRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await _resultService.CompleteAsync(request));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ResultAttachmentDto>> Upload([FromForm] ResultUploadRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await _resultService.UploadAsync(request));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
