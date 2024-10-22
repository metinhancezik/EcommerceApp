using AuthenticationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationLayer.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> ValidateCredentialsAsync(string email, string password);
        Task<TokenModel> GenerateTokenAsync(long userId);
        Task<bool> RevokeTokenAsync(string token);
        Task<bool> ValidateTokenAsync(string token);
        Task<(bool Success, string Message)> RegisterUserAsync(UserRegistrationModel model);
    }
}
