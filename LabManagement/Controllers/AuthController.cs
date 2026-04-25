using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResult = await _authService.LoginAsync(request);
            if (loginResult == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(loginResult);
        }
    }
}
