using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Dtos;

namespace LabManagement.Interfaces
{
    public interface IPatientService
    {
        Task<PatientDto?> GetPatientByIdAsync(int patientId);
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto> CreatePatientAsync(PatientDto dto);
        Task UpdatePatientAsync(int patientId, PatientDto dto);
        Task DeletePatientAsync(int patientId);
    }
}
