using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Data;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly LabDbContext _dbContext;

        public OrderRepository(LabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Order?> GetByIdAsync(int orderId)
        {
            const string sql = @"SELECT order_id, patient_id, order_number, order_date, status, total_amount, remarks, created_at
                                 FROM lab.orders
                                 WHERE order_id = @OrderId";

            return _dbContext.QuerySingleAsync<Order>(sql, new Dictionary<string, object?> { { "OrderId", orderId } });
        }

        public Task<Order?> GetByPatientIdAsync(int patientId)
        {
            const string sql = @"SELECT order_id, patient_id, order_number, order_date, status, total_amount, remarks, created_at
                                 FROM lab.orders
                                 WHERE patient_id = @PatientId
                                 ORDER BY created_at DESC, order_id DESC
                                 LIMIT 1";

            return _dbContext.QuerySingleAsync<Order>(sql, new Dictionary<string, object?> { { "PatientId", patientId } });
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            const string sql = @"SELECT order_id, patient_id, order_number, order_date, status, total_amount, remarks, created_at
                                 FROM lab.orders
                                 ORDER BY created_at DESC, order_id DESC";

            return await _dbContext.QueryAsync<Order>(sql, null);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            const string sql = @"INSERT INTO lab.orders
                                    (patient_id, order_number, order_date, status, total_amount, remarks, created_at)
                                 VALUES
                                    (@PatientId, @OrderNumber, @OrderDate, @Status, @TotalAmount, @Remarks, @CreatedAt)
                                 RETURNING order_id;";

            var parameters = new Dictionary<string, object?>
            {
                { "PatientId", order.PatientId },
                { "OrderNumber", order.OrderNumber },
                { "OrderDate", order.OrderDate },
                { "Status", order.Status },
                { "TotalAmount", order.TotalAmount },
                { "Remarks", order.Remarks },
                { "CreatedAt", order.CreatedAt }
            };

            order.OrderId = await _dbContext.ExecuteScalarAsync<int>(sql, parameters);

            return order;
        }

        public async Task<IEnumerable<OrderTests>> GetOrderTestsByOrderIdAsync(int orderId)
        {
            const string sql = @"SELECT order_test_id, order_id, test_id, price, status
                                 FROM lab.order_tests
                                 WHERE order_id = @OrderId";

            return await _dbContext.QueryAsync<OrderTests>(sql, new Dictionary<string, object?> { { "OrderId", orderId } });
        }

        public async Task<OrderTests> CreateOrderTestAsync(OrderTests orderTest)
        {
            const string sql = @"INSERT INTO lab.order_tests
                                    (order_id, test_id, price, status)
                                 VALUES
                                    (@OrderId, @TestId, @Price, @Status)
                                 RETURNING order_test_id;";

            var parameters = new Dictionary<string, object?>
            {
                { "OrderId", orderTest.OrderId },
                { "TestId", orderTest.TestId },
                { "Price", orderTest.Price },
                { "Status", orderTest.Status }
            };

            orderTest.OrderTestId = await _dbContext.ExecuteScalarAsync<int>(sql, parameters);

            return orderTest;
        }
    }
}
