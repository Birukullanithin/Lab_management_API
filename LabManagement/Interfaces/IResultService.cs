using System.Threading.Tasks;
using LabManagement.Dtos;

namespace LabManagement.Interfaces
{
    public interface IResultService
    {
        Task<ResultResponseDto> GetByOrderIdAsync(int orderId);
        Task<ResultResponseDto> SaveAsync(ResultSaveRequestDto request);
        Task<ResultResponseDto> CompleteAsync(ResultSaveRequestDto request);
        Task<ResultAttachmentDto> UploadAsync(ResultUploadRequestDto request);
    }
}
