using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Models;

namespace LabManagement.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int orderId);
        Task<Order?> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> CreateAsync(Order order);
        Task<IEnumerable<OrderTests>> GetOrderTestsByOrderIdAsync(int orderId);
        Task<OrderTests> CreateOrderTestAsync(OrderTests orderTest);
    }
}
