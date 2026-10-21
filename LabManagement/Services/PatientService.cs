using LabManagement.Dtos;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IOrderService _orderService;
        private readonly ITestMasterService _testMasterService;

        public PatientService(
            IPatientRepository patientRepository,
            IOrderService orderService,
            ITestMasterService testMasterService)
        {
            _patientRepository = patientRepository;
            _orderService = orderService;
            _testMasterService = testMasterService;
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int patientId)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(patientId);
            if (patient == null)
            {
                return null;
            }

            var patientDto = MapToDto(patient);
            patientDto.order = await _orderService.GetOrderDtoByPatientIdAsync(patientId);
            return patientDto;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllPatientsAsync();
            var list = new List<PatientDto>();

            foreach (var patient in patients)
            {
                var patientDto = MapToDto(patient);
                patientDto.order = await _orderService.GetOrderDtoByPatientIdAsync(patient.PatientId);
                list.Add(patientDto);
            }

            return list;
        }

        public async Task<PatientDto> CreatePatientAsync(PatientDto dto)
        {
            var patient = MapToEntity(dto);
            var createdPatient = await _patientRepository.CreatePatientAsync(patient);

            var order = new Order
            {
                PatientId = createdPatient.PatientId,
                OrderNumber = GenerateOrderNumber(),
                Status = "Pending",
                CreatedAt = System.DateTime.Now
            };

            var createdOrder = await _orderService.CreateAsync(order);
            var tests = await _testMasterService.GetByNamesAsync(dto.TestNames);

            foreach (var test in tests)
            {
                var orderTest = new OrderTests
                {
                    OrderId = createdOrder.OrderId,
                    TestId = test.TestId,
                    Price = 0,
                    Status = "Pending"
                };

                await _orderService.CreateOrderTestAsync(orderTest);
            }

            var createdPatientDto = MapToDto(createdPatient);
            createdPatientDto.order = await _orderService.GetOrderDtoByPatientIdAsync(createdPatient.PatientId);
            return createdPatientDto;
        }

        public async Task UpdatePatientAsync(int patientId, PatientDto dto)
        {
            var existing = await _patientRepository.GetPatientByIdAsync(patientId);
            if (existing == null)
            {
                return;
            }

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

            await _patientRepository.UpdatePatientAsync(existing);
        }

        public Task DeletePatientAsync(int patientId)
        {
            return _patientRepository.DeletePatientAsync(patientId);
        }

        private static string GenerateOrderNumber()
        {
            return $"ORD-{System.DateTime.Now:yyyyMMddHHmmss}";
        }

        private static Patient MapToEntity(PatientDto dto)
        {
            return new Patient
            {
                PatientId = dto.PatientId,
                OrganizationId = dto.OrganizationId,
                LegalEntityId = dto.LegalEntityId,
                PatientName = dto.PatientName,
                Age = dto.Age,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

        private static PatientDto MapToDto(Patient patient)
        {
            return new PatientDto
            {
                PatientId = patient.PatientId,
                OrganizationId = patient.OrganizationId,
                LegalEntityId = patient.LegalEntityId,
                PatientName = patient.PatientName,
                Age = patient.Age,
                Gender = patient.Gender,
                DateOfBirth = patient.DateOfBirth,
                Phone = patient.Phone,
                Email = patient.Email,
                Address = patient.Address,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };
        }
    }
}
