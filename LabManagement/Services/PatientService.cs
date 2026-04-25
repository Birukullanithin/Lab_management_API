using LabManagement.Dtos;
using LabManagement.Interfaces;
using LabManagement.Models;
using LabManagement.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabManagement.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientrepository;

        public PatientService(IPatientRepository repository)
        {
            _patientrepository = repository;
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int patientId)
        {
            // 1. Get patient
            var patient = await _patientrepository.GetPatientByIdAsync(patientId);
            if (patient == null)
                return null;

            // 2. Map patient
            var patientDto = MapToDto(patient);

            // 3. Get order
            var order = await _patientrepository.GetOrderDetailsByPatientIdAsync(patientId);

            if (order != null)
            {
                var orderDto = MapToDto(order);

                // 4. Get order tests (list)
                var orderTests = await _patientrepository
                    .GetOrderTestsByOrderIdAsync(order.OrderId);

                var orderTestDtoList = new List<TestMasterDto>();

                // 5. Loop each test â†’ get test name using testId
                foreach (var ot in orderTests)
                {
                    var testDetails = await _patientrepository
                        .GetOrderTestsWithTestIdAsync(ot.TestId);

                    var dto = new TestMasterDto
                    {
                        TestName = testDetails?.TestName   // ðŸ‘ˆ important
                    };

                    orderTestDtoList.Add(dto);
                }

                // 6. Attach tests to order
                orderDto.OrderNames = orderTestDtoList;

                // 7. Attach order to patient
                patientDto.order = orderDto;
            }

            return patientDto;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientrepository.GetAllPatientsAsync();
            var list = new List<PatientDto>();

            foreach (var p in patients)
            {
                // 1. Map patient
                var patientDto = MapToDto(p);

                // 2. Get order
                var order = await _patientrepository
                    .GetOrderDetailsByPatientIdAsync(p.PatientId);

                if (order != null)
                {
                    var orderDto = MapToDto(order);

                    // 3. Get order tests
                    var orderTests = await _patientrepository
                        .GetOrderTestsByOrderIdAsync(order.OrderId);

                    var orderTestDtoList = new List<TestMasterDto>();

                    // 4. Loop tests â†’ get test names
                    foreach (var ot in orderTests)
                    {
                        var testDetails = await _patientrepository
                            .GetOrderTestsWithTestIdAsync(ot.TestId);

                        var dto = new TestMasterDto
                        {
                            TestId = ot.TestId,
                            TestName = testDetails?.TestName
                        };

                        orderTestDtoList.Add(dto);
                    }

                    // 5. Attach test names to order
                    orderDto.OrderNames = orderTestDtoList;

                    // 6. Attach order to patient
                    patientDto.order = orderDto;
                }

                list.Add(patientDto);
            }

            return list;
        }
        public async Task UpdatePatientAsync(int patientId, PatientDto dto)
        {
            var existing = await    _patientrepository.GetPatientByIdAsync(patientId);
            if (existing == null) return;

            // update fields
            existing.PatientName = dto.PatientName;
            existing.Age = dto.Age;
            existing.Gender = dto.Gender;
            existing.DateOfBirth = dto.DateOfBirth;
            existing.Phone = dto.Phone;
            existing.Email = dto.Email;
            existing.Address = dto.Address;
            existing.OrganizationId = dto.OrganizationId;
            existing.LegalEntityId = dto.LegalEntityId;
            existing.UpdatedAt = dto.UpdatedAt ?? System.DateTime.UtcNow;

            await _patientrepository.UpdatePatientAsync(existing);
        }

        public async Task DeletePatientAsync(int patientId)
        {
            await _patientrepository.DeletePatientAsync(patientId);
        }

        public async Task<PatientDto> CreatePatientAsync(PatientDto dto)
        {
           //using var transaction = _db.BeginTransaction();

            try
            {
                var patient = MapToEntity(dto);
                var createdPatient = await _patientrepository.CreatePatientAsync(patient);

                // 2ï¸âƒ£ Create Order
                var order = new Order
                {
                    PatientId = createdPatient.PatientId,
                    OrderNumber = GenerateOrderNumber(),
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                var createdOrder = await _patientrepository.CreateOrderAsync(order);

                // 3ï¸âƒ£ Get Tests from DB
                var tests = await _patientrepository.GetTestsByNamesAsync(dto.TestNames);

                // 4ï¸âƒ£ Insert into OrderTests
                foreach (var test in tests)
                {
                    var orderTest = new OrderTests
                    {
                        OrderId = createdOrder.OrderId,
                        TestId = test.TestId,
                        Price = 0, // you can update later
                        Status = "Pending"
                    };

                    await _patientrepository.CreateOrderTestAsync(orderTest);
                }

                return MapToDto(createdPatient);
            }
            catch
            {
                throw;
            }
        }
        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.Now:yyyyMMddHHmmss}";
        }

        private static Patient MapToEntity(PatientDto d)
        {
            return new Patient
            {
                PatientId = d.PatientId,
                OrganizationId = d.OrganizationId,
                LegalEntityId = d.LegalEntityId,
                PatientName = d.PatientName,
                Age = d.Age,
                Gender = d.Gender,
                DateOfBirth = d.DateOfBirth,
                Phone = d.Phone,
                Email = d.Email,
                Address = d.Address,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            };
        }
        private static PatientDto MapToDto(Patient p)
        {
            return new PatientDto
            {
                PatientId = p.PatientId,
                OrganizationId = p.OrganizationId,
                LegalEntityId = p.LegalEntityId,
                PatientName = p.PatientName,
                Age = p.Age,
                Gender = p.Gender,
                DateOfBirth = p.DateOfBirth,
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            };
        }
        private static OrderDto MapToDto(Order o)
        {
            return new OrderDto
            {
                OrderId = o.OrderId,
                PatientId = o.PatientId,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Remarks = o.Remarks
            };
        }
    }
}
