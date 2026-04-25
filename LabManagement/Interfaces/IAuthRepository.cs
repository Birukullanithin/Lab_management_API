using System.Threading.Tasks;
using LabManagement.Models;

namespace LabManagement.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthUser?> GetUserByUsernameAsync(string username);
    }
}
