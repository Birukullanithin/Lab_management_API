using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Models;

namespace LabManagement.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient?> GetPatientByIdAsync(int patientId);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> CreatePatientAsync(Patient patient);
        Task UpdatePatientAsync(Patient patient);
        Task DeletePatientAsync(int patientId);
        Task<Order> CreateOrderAsync(Order order);
        Task<List<TestMaster>> GetTestsByNamesAsync(List<string> testNames);
        Task<OrderTests> CreateOrderTestAsync(OrderTests orderTest);
        Task<Order?> GetOrderDetailsByPatientIdAsync(int patientId);
        Task<IEnumerable<OrderTests>> GetOrderTestsByOrderIdAsync(int orderId);
        Task<TestMaster> GetOrderTestsWithTestIdAsync(int testId);
    }
}
