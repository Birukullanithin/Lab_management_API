using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Models;

namespace LabManagement.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> GetByIdAsync(int orderId);
        Task<Order?> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> CreateAsync(Order order);
        Task<IEnumerable<OrderTests>> GetOrderTestsByOrderIdAsync(int orderId);
        Task<OrderTests> CreateOrderTestAsync(OrderTests orderTest);
        Task<OrderDto?> GetOrderDtoByPatientIdAsync(int patientId);
        Task<OrderDto?> GetOrderDtoByIdAsync(int orderId);
    }
}
