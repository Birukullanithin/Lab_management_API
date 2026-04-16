using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using LabManagement.Models;
using LabManagement.Data;
using LabManagement.Interfaces;

namespace LabManagement.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly LabDbContext _dbContext;

        public PatientRepository(LabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Patient?> GetPatientByIdAsync(int patientId)
        {
            var sql = @"SELECT patient_id, organization_id, legal_entity_id, patient_name, age, gender, date_of_birth, phone, email, address, created_at, updated_at
                                FROM lab.patient WHERE patient_id = @Id";  
            var p = await _dbContext.QuerySingleAsync<Patient>(sql, new System.Collections.Generic.Dictionary<string, object?> { { "Id", patientId } });
            return p;
        }
        public async Task<Order?> GetOrderDetailsByPatientIdAsync(int patientId)
        {
            var sql = @"
                    SELECT 
                        order_id,
                        patient_id,
                        order_number,
                        order_date,
                        status,
                        total_amount,
                        remarks,
                        created_at
                    FROM lab.orders 
                    WHERE patient_id = @Id";

            var order = await _dbContext.QuerySingleAsync<Order>(
                sql,
                new Dictionary<string, object?> { { "Id", patientId } }
            );

            return order;
        }
        public async Task<IEnumerable<OrderTests>> GetOrderTestsByOrderIdAsync(int orderId)
        {
            var sql = @"
                        SELECT 
                            order_test_id,
                            order_id ,
                            test_id ,
                            price ,
                            status 
                        FROM lab.order_tests
                        WHERE order_id = @OrderId";

            return await _dbContext.QueryAsync<OrderTests>(
                sql,
                new Dictionary<string, object?> { { "OrderId", orderId } }
            );

        }
        public async Task<TestMaster> GetOrderTestsWithTestIdAsync(int testId)
        {
            var sql = @"
                        SELECT 
                            ot.order_test_id,
                            ot.order_id ,
                            ot.test_id ,
                            tm.test_name ,
                            ot.price ,
                            ot.status
                        FROM lab.order_tests ot
                        JOIN lab.test_master tm ON ot.test_id = tm.test_id
                        WHERE ot.test_id = @TestId";

            return await _dbContext.QuerySingleAsync<TestMaster>(
                sql,
                new Dictionary<string, object?> { { "TestId", testId } }
            );
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            var sql = @"SELECT patient_id, organization_id, legal_entity_id, patient_name, age, gender, date_of_birth, phone, email, address, created_at, updated_at
                                FROM lab.patient";
            var list = await _dbContext.QueryAsync<Patient>(sql, null);
            return list;
        }

        public async Task<Patient> CreatePatientAsync(Patient patient)
        {
            var sql = @"INSERT INTO lab.patient (organization_id, legal_entity_id, patient_name, age, gender, date_of_birth, phone, email, address, created_at, updated_at)
                                VALUES (@OrganizationId, @LegalEntityId, @PatientName, @Age, @Gender, @DateOfBirth, @Phone, @Email, @Address, @CreatedAt, @UpdatedAt)
                                RETURNING patient_id;";

            var parameters = new System.Collections.Generic.Dictionary<string, object?>
            {
                { "OrganizationId", patient.OrganizationId },
                { "LegalEntityId", patient.LegalEntityId },
                { "PatientName", patient.PatientName },
                { "Age", patient.Age },
                { "Gender", patient.Gender },
                { "DateOfBirth", patient.DateOfBirth },
                { "Phone", patient.Phone },
                { "Email", patient.Email },
                { "Address", patient.Address },
                { "CreatedAt", patient.CreatedAt },
                { "UpdatedAt", patient.UpdatedAt }
            };

            var id = await _dbContext.ExecuteScalarAsync<int>(sql, parameters);
            if (id != null)
            {
                patient.PatientId = id;
            }
            return patient;
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            var sql = @"UPDATE lab.patient SET
                                organization_id = @OrganizationId,
                                legal_entity_id = @LegalEntityId,
                                patient_name = @PatientName,
                                age = @Age,
                                gender = @Gender,
                                date_of_birth = @DateOfBirth,
                                phone = @Phone,
                                email = @Email,
                                address = @Address,
                                updated_at = @UpdatedAt
                            WHERE patient_id = @PatientId";

            var parameters = new System.Collections.Generic.Dictionary<string, object?>
            {
                { "OrganizationId", patient.OrganizationId },
                { "LegalEntityId", patient.LegalEntityId },
                { "PatientName", patient.PatientName },
                { "Age", patient.Age },
                { "Gender", patient.Gender },
                { "DateOfBirth", patient.DateOfBirth },
                { "Phone", patient.Phone },
                { "Email", patient.Email },
                { "Address", patient.Address },
                { "UpdatedAt", patient.UpdatedAt },
                { "PatientId", patient.PatientId }
            };

            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }
        public async Task<List<TestMaster>> GetTestsByNamesAsync(List<string> testNames)
        {
            var sql = @"SELECT * FROM lab.test_master 
                WHERE test_name = ANY(@TestNames) 
                AND is_active = true";

            var parameters = new Dictionary<string, object?>
            {
                { "TestNames", testNames.ToArray() } // IMPORTANT
            };

            return (await _dbContext.QueryAsync<TestMaster>(sql, parameters)).ToList();
        }
        public async Task<Order> CreateOrderAsync(Order order)
        {
            var sql = @"INSERT INTO lab.orders 
                (patient_id, order_number, order_date, status, total_amount, remarks, created_at)
                VALUES 
                (@PatientId, @OrderNumber, @OrderDate, @Status, @TotalAmount, @Remarks, @CreatedAt)
                RETURNING order_id;";

            var parameters = new System.Collections.Generic.Dictionary<string, object?>
            {
                { "PatientId", order.PatientId },
                { "OrderNumber", order.OrderNumber },
                { "OrderDate", order.OrderDate },
                { "Status", order.Status },
                { "TotalAmount", order.TotalAmount },
                { "Remarks", order.Remarks },
                { "CreatedAt", order.CreatedAt }
            };

            var id = await _dbContext.ExecuteScalarAsync<int>(sql, parameters);

            if (id != null)
            {
                order.OrderId = id;
            }

            return order;
        }
        public async Task<OrderTests> CreateOrderTestAsync(OrderTests orderTest)
        {
            var sql = @"INSERT INTO lab.order_tests 
                (order_id, test_id, price, status)
                VALUES 
                (@OrderId, @TestId, @Price, @Status)
                RETURNING order_test_id;";

            var parameters = new System.Collections.Generic.Dictionary<string, object?>
            {
                { "OrderId", orderTest.OrderId },
                { "TestId", orderTest.TestId },
                { "Price", orderTest.Price },
                { "Status", orderTest.Status }
            };

            var id = await _dbContext.ExecuteScalarAsync<int>(sql, parameters);

            if (id != null)
            {
                orderTest.OrderTestId = id;
            }

            return orderTest;
        }
        public async Task DeletePatientAsync(int patientId)
        {
            var sql = "DELETE FROM lab.patient WHERE patient_id = @Id";
            await _dbContext.ExecuteNonQueryAsync(sql, new System.Collections.Generic.Dictionary<string, object?> { { "Id", patientId } });
        }
    }
}
