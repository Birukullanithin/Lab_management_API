using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITestMasterService _testMasterService;

        public OrderService(IOrderRepository orderRepository, ITestMasterService testMasterService)
        {
            _orderRepository = orderRepository;
            _testMasterService = testMasterService;
        }

        public Task<Order?> GetByIdAsync(int orderId) => _orderRepository.GetByIdAsync(orderId);

        public Task<Order?> GetByPatientIdAsync(int patientId) => _orderRepository.GetByPatientIdAsync(patientId);

        public Task<IEnumerable<Order>> GetAllAsync() => _orderRepository.GetAllAsync();

        public Task<Order> CreateAsync(Order order) => _orderRepository.CreateAsync(order);

        public Task<IEnumerable<OrderTests>> GetOrderTestsByOrderIdAsync(int orderId) => _orderRepository.GetOrderTestsByOrderIdAsync(orderId);

        public Task<OrderTests> CreateOrderTestAsync(OrderTests orderTest) => _orderRepository.CreateOrderTestAsync(orderTest);

        public Task<OrderDto?> GetOrderDtoByPatientIdAsync(int patientId) => BuildOrderDtoAsync(() => _orderRepository.GetByPatientIdAsync(patientId));

        public Task<OrderDto?> GetOrderDtoByIdAsync(int orderId) => BuildOrderDtoAsync(() => _orderRepository.GetByIdAsync(orderId));

        private async Task<OrderDto?> BuildOrderDtoAsync(System.Func<Task<Order?>> getOrder)
        {
            var order = await getOrder();
            if (order == null)
            {
                return null;
            }

            var orderTests = await _orderRepository.GetOrderTestsByOrderIdAsync(order.OrderId);
            var orderedTestIds = orderTests.Select(test => test.TestId).ToList();
            var tests = await _testMasterService.GetByIdsAsync(orderedTestIds);
            var testsById = tests.ToDictionary(test => test.TestId);

            var orderNames = new List<TestMasterDto>();
            foreach (var testId in orderedTestIds)
            {
                if (testsById.TryGetValue(testId, out var test))
                {
                    orderNames.Add(MapToTestMasterDto(test));
                }
            }

            return new OrderDto
            {
                OrderId = order.OrderId,
                PatientId = order.PatientId,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Remarks = order.Remarks,
                OrderNames = orderNames
            };
        }

        private static TestMasterDto MapToTestMasterDto(TestMaster test)
        {
            return new TestMasterDto
            {
                TestId = test.TestId,
                TestName = test.TestName,
                TestCode = test.TestCode,
                NormalMin = test.NormalMin,
                NormalMax = test.NormalMax,
                Unit = test.Unit,
                Description = test.Description,
                IsActive = test.IsActive
            };
        }
    }
}
