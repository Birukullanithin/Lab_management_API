using System;
using System.Threading.Tasks;
using BCrypt.Net;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _authRepository.GetUserByUsernameAsync(request.Username.Trim());
            if (user == null || !user.IsActive)
            {
                return null;
            }

            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                return null;
            }

            return MapToDto(user);
        }

        private static LoginResponseDto MapToDto(AuthUser user)
        {
            return new LoginResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                OrganizationId = user.OrganizationId,
                LegalEntityId = user.LegalEntityId,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        private static bool VerifyPassword(string providedPassword, string storedPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(storedPasswordHash))
            {
                return false;
            }

            if (string.Equals(providedPassword, storedPasswordHash, StringComparison.Ordinal))
            {
                return true;
            }

            if (storedPasswordHash.StartsWith("$2", StringComparison.Ordinal))
            {
                return BCrypt.Net.BCrypt.Verify(providedPassword, storedPasswordHash);
            }

            if (storedPasswordHash.StartsWith("AQAAAA", StringComparison.Ordinal))
            {
                return VerifyWithAspNetIdentity(providedPassword, storedPasswordHash);
            }

            return false;
        }
        private static bool VerifyWithAspNetIdentity(string providedPassword, string storedPasswordHash)
        {
            var passwordHasherType = Type.GetType(
                "Microsoft.AspNetCore.Identity.PasswordHasher`1[[System.Object]], Microsoft.Extensions.Identity.Core");

            if (passwordHasherType == null)
            {
                return false;
            }

            var hasher = Activator.CreateInstance(passwordHasherType);
            var verifyMethod = passwordHasherType.GetMethod(
                "VerifyHashedPassword",
                new[] { typeof(object), typeof(string), typeof(string) });

            if (hasher == null || verifyMethod == null)
            {
                return false;
            }

            var result = verifyMethod.Invoke(hasher, new object?[] { new object(), storedPasswordHash, providedPassword });
            return result is not null && Convert.ToInt32(result) > 0;
        }
    }
}
