using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabManagement.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderDto?>> GetById(int orderId)
        {
            var order = await _orderService.GetOrderDtoByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<ActionResult<OrderDto?>> GetByPatientId(int patientId)
        {
            var order = await _orderService.GetOrderDtoByPatientIdAsync(patientId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}
