using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabManagement.Controllers
{
    [ApiController]
    [Route("api/test-master")]
    public class TestMasterController : ControllerBase
    {
        private readonly ITestMasterService _testMasterService;

        public TestMasterController(ITestMasterService testMasterService)
        {
            _testMasterService = testMasterService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestMasterDto>>> GetAll()
        {
            var tests = await _testMasterService.GetAllDtosAsync();
            return Ok(tests);
        }

        [HttpGet("{testId:int}")]
        public async Task<ActionResult<TestMasterDto?>> GetById(int testId)
        {
            var test = await _testMasterService.GetDtoByIdAsync(testId);
            if (test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }
    }
}
