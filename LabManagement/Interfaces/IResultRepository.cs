using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Models;

namespace LabManagement.Interfaces
{
    public interface IResultRepository
    {
        Task<TestResult?> GetResultByOrderIdAsync(int orderId);
        Task<List<TestResult>> GetResultsByOrderIdAsync(int orderId);
        Task<List<TestResultDetail>> GetResultDetailsByResultIdAsync(int resultId);
        Task<List<TestResultAttachment>> GetResultAttachmentsByResultIdAsync(int resultId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<Patient?> GetPatientByIdAsync(int patientId);
        Task<TestMaster?> GetTestMasterByIdAsync(int testId);
        Task<List<TestMaster>> GetOrderTestsAsync(int orderId);
        Task<TestResult> SaveResultAsync(TestResult result);
        Task<List<TestResult>> ReplaceResultsAsync(int orderId, int patientId, IEnumerable<TestResult> results);
        Task ReplaceResultDetailsAsync(int resultId, IEnumerable<TestResultDetail> details);
        Task<TestResultAttachment> SaveAttachmentAsync(TestResultAttachment attachment);
    }
}
