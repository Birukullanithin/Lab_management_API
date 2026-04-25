using System.Threading.Tasks;
using LabManagement.Dtos;

namespace LabManagement.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}
